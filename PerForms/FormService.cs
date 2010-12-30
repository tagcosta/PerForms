#region License
// Copyright (c) 2010 Tiago Costa
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Data;
using PerForms.JQGrid;
using PerForms.Filters;
using System.Data.SqlClient;
using PerForms.Config;
using PerForms.Util;
using System.Web.UI;
using PerForms.Logging;
using System.Web;
using System.IO;
using PerForms.Actions;

namespace PerForms
{
    /// <summary>
    /// <para>This class serves as a base class to Custom_PerFormsService.</para>
    /// <para>It defines what methods you must implement and provides a variety of helper methods you can override.</para>
    /// </summary>
    public abstract class FormService
    {
        /// <summary>
        /// <para>This method serves the purpose of allowing interaction between the server and the client via AJAX.</para>
        /// </summary>
        public abstract AJAXActions GetAJAXActions(string actionKey, Dictionary<string, List<string>> values);

        /// <summary>
        /// <para>All interactions automatically call this method to force validation (ex: session might have timed out).</para>
        /// </summary>
        public abstract bool IsAuthenticated();


        /// <summary>
        /// <para>This method should return an identifier for a user. It is used to persist logging information.</para>
        /// </summary>
        public abstract string GetCurrentUser();

        /// <summary>
        /// <para>Used in FieldGridViews, when you want server-side paging, sorting and filtering.</para>
        /// </summary>
        public abstract string GetQuery(string actionKey, Dictionary<string, List<string>> values, out string columnName);

        /// <summary>
        /// <para>Executes a query using the ConnectionString configured in DBConfig.</para>
        /// </summary>
        public virtual DataTable Select(string query)
        {
            SqlConnection con = new SqlConnection(PerFormsConfig.DBConfig.DataConnectionString);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand(query, con);
                DataTable dt = new DataTable("Table");
                dt.Load(com.ExecuteReader());
                return dt;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// <para>Converts arrays of keys and values to a dictionary.</para>
        /// </summary>
        public virtual Dictionary<string, List<string>> GetSelectedValues(string[] keys, string[] values)
        {
            if (keys == null || values == null) return new Dictionary<string, List<string>>();

            if (keys.Length != values.Length)
            {
                throw new Exception("Keys and Values should have the same size.");
            }
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 0; i < keys.Length; i++)
            {
                if (dic.ContainsKey(keys[i]))
                {
                    dic[keys[i]].Add(values[i]);
                }
                else
                {
                    List<string> list = new List<string>();
                    list.Add(values[i]);
                    dic.Add(keys[i], list);
                }
            }
            return dic;
        }

        /// <summary>
        /// <para>Gets the given key of the dictionary as an int.</para>
        /// </summary>
        public virtual int GetSelectedID(Dictionary<string, List<string>> values, string fieldKey)
        {
            return Convert.ToInt32(values[fieldKey][0]);
        }

        /// <summary>
        /// <para>Gets the first element of the dictionary as an int.</para>
        /// <para>Can be used in GetAJAXUpdateItems to get the combobox's selected key.</para>
        /// </summary>
        public virtual int GetSelectedID(Dictionary<string, List<string>> values)
        {
            foreach (KeyValuePair<string, List<string>> kvp in values)
            {
                return Convert.ToInt32(kvp.Value[0]);
            }
            throw new Exception("The key/values dictionary was empty.");
        }

        /// <summary>
        /// <para>Gets the given key of the dictionary as a string.</para>
        /// </summary>
        public virtual string GetSelectedValue(Dictionary<string, List<string>> values, string fieldKey)
        {
            return Convert.ToString(values[fieldKey][0]);
        }

        /// <summary>
        /// <para>Gets the first element of the dictionary as a string.</para>
        /// </summary>
        public virtual string GetSelectedValue(Dictionary<string, List<string>> values)
        {
            foreach (KeyValuePair<string, List<string>> kvp in values)
            {
                return Convert.ToString(kvp.Value[0]);
            }
            throw new Exception("The key/values dictionary was empty.");
        }
        
        /// <summary>
        /// <para>Log an exception in the database.</para>
        /// </summary>
        public virtual ExceptionLog LogException(Exception exp, ActionLog actionLog)
        {
            string guid = Guid.NewGuid().ToString("N");
            ExceptionLog exLog = new ExceptionLog
            {
                Message = exp.Message,
                StackTrace = exp.StackTrace,
                Type = exp.GetType().ToString(),
                Guid = guid,
                ActionLog = actionLog
            };

            if (exp.InnerException != null)
            {
                exLog.InnerMessage = exp.InnerException.Message;
                exLog.InnerStackTrace = exp.InnerException.StackTrace;
            }

            new BaseRepository().Insert<ExceptionLog>(exLog);
            return exLog;
        }

        /// <summary>
        /// <para>Log an action in the database.</para>
        /// </summary>
        public virtual ActionLog LogAction(string actionKey, string[] keys, string[] values, string userKey)
        {
            ActionLog actionLog = GetActionLog(actionKey, keys, values, userKey);
            new BaseRepository().Insert<ActionLog>(actionLog);
            return actionLog;
        }

        /// <summary>
        /// <para>Updates the database with the time it took for the action to complete.</para>
        /// </summary>
        public virtual void LogActionOperationTime(ActionLog actionLog)
        {
            actionLog.Milliseconds = actionLog.GetOperationTime();
            NH_PERFORMS.Instance.Flush();
        }

        /// <summary>
        /// <para>Creates a new ActionLog.</para>
        /// </summary>
        public virtual ActionLog GetActionLog(string actionKey, string[] keys, string[] values, string userKey)
        {
            ActionLog actionLog = new ActionLog();

            actionLog.ActionKey = actionKey;
            actionLog.Milliseconds = null;
            actionLog.UserKey = userKey;

            Dictionary<string, List<string>> keyValues = GetSelectedValues(keys, values);
            foreach (KeyValuePair<string, List<string>> kvp in keyValues)
            {
                ActionLogParameter actionLogParam = new ActionLogParameter();
                actionLogParam.ActionLog = actionLog;
                actionLogParam.Key = kvp.Key;
                foreach (string s in kvp.Value)
                {
                    actionLogParam.Values += s + ";";
                }
                actionLogParam.Values = actionLogParam.Values.TrimEnd(';');
                actionLog.ActionLogParameters.Add(actionLogParam);
            }

            return actionLog;
        }

        public virtual void EmailException(ExceptionLog exLog)
        {
            try
            {
                string smtp_server = PerFormsConfig.EmailConfig.EmailServerAddress;
                MailMessage msg = new MailMessage(PerFormsConfig.EmailConfig.OnExceptionEmailFrom, PerFormsConfig.EmailConfig.OnExceptionEmailTo, PerFormsConfig.AppConfig.ApplicationName + " Exception [" + exLog.Guid + "]", "<pre>Type: " + exLog.Type + "\r\n\r\nMessage: " + exLog.Message + "\r\n\r\nStackTrace: " + exLog.StackTrace + "\r\n\r\nInner Message: " + exLog.InnerMessage + "\r\n\r\nInner StackTrace: " + exLog.InnerStackTrace + "</pre>");
                msg.IsBodyHtml = true;
                SmtpClient mail = new SmtpClient(smtp_server);
                mail.Send(msg);                
            }
            catch { }
        }

        public virtual int GetScalarInt(DataTable dt)
        {
            return Convert.ToInt32(dt.Rows[0][0]);
        }


        protected virtual JQGrid.JQGrid GetJQGrid(string originalQuery, string columnName, QueryStringInfo queryStringInfo)
        {
            if (queryStringInfo == null)
            {
                queryStringInfo = new QueryStringInfo
                {
                    filters = new QueryStringFilter { groupOp = "AND", rules = new QueryStringFilterRule[0] },
                    page = 1,
                    rows = 10,
                    sidx = columnName,
                    sord = "ASC"
                };
            }
            string totalRecordsQuery;
            string query = new FilterFormatter().Format(new FilterParser().Parse(queryStringInfo), originalQuery, out totalRecordsQuery);
            return queryStringInfo.GetJQGrid(Select(query), GetScalarInt(Select(totalRecordsQuery)));
        }

        public virtual JQGrid.JQGrid GetJQGrid(string actionKey, Dictionary<string, List<string>> values, QueryStringInfo queryStringInfo)
        {
            string columnName;
            string query = GetQuery(actionKey, values, out columnName);
            return GetJQGrid(query, columnName, queryStringInfo);
        }

        public virtual DataTable GetDataTableAll(string actionKey, Dictionary<string, List<string>> values)
        {
            string columnName;
            string query = GetQuery(actionKey, values, out columnName);
            return Select(query);
        }

        public virtual DataTable GetDataTableZero(string actionKey, Dictionary<string, List<string>> values)
        {
            string columnName;
            string query = GetQuery(actionKey, values, out columnName);
            string queryZero = "SELECT TOP 0 * FROM (" + query + ") [T]";
            return Select(queryZero);
        }

        public virtual UserControl LoadUserControl(string path)
        {
            return (UserControl)new Page().LoadControl(path);
        }

        public virtual AJAXActions GetNotAuthenticatedResult()
        {
            return new AJAXActions().AddShowMessageAction(ShowMessageAction.EMessageType.ERROR, "[ERROR]", "[ERROR]", "[SessionExpired]").AddRedirectToURLAction(new Paths().GetRootURL());
        }

        public virtual byte[] GetUploadData(string guid)
        {
            return File.ReadAllBytes(new Paths().GetRootPath() + "PerForms/Uploads/" + guid);
        }

        public virtual byte[] GetUploadDataAndDeleteUpload(string guid)
        {
            byte[] output = GetUploadData(guid);
            DeleteUpload(guid);
            return output;
        }

        public virtual void DeleteUpload(string guid)
        {
            File.Delete(new Paths().GetRootPath() + "PerForms/Uploads/" + guid);
        }
    }
}