using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerForms;
using System.Data;
using PerForms.Util;

public partial class PerForms_FileDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string actionKey = string.Empty;
        if (Request.QueryString.AllKeys.Contains("actionKey"))
        {
            actionKey = Request.QueryString["actionKey"];
        }

        DataTable dt = new Custom_PerFormsService().GetDataTableAll(actionKey, null);
        string fileName = "report.xls";
        string mimeType = new MIMEType().GetMIMEType(new MIMEType().GetExtension(fileName));

        byte[] buffer = new Excel().GetExcelBufferFromDataTable(dt);
        Response.Clear();
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        Response.ContentType = mimeType;
        Response.OutputStream.Write(buffer, 0, buffer.Length);
        Response.OutputStream.Close();
        Response.End();
    }
}