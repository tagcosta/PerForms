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
using System.Data;
using PerForms.Util;
using PerForms.Config;
using PerForms.Logging;
using PerForms.Actions;

namespace PerForms
{
    public class WebServiceInterface
    {
        public FormService FormService { get; set; }

        public WebServiceInterface(FormService formService)
        {
            FormService = formService;
        }

        public AJAXActions GetAJAXActions(string actionKey, string[] keys, string[] values)
        {
            ActionLog actionLog = null;
            try
            {
                if (FormService.IsAuthenticated())
                {
                    if (PerFormsConfig.DBConfig.LogsActive) actionLog = FormService.LogAction(actionKey, keys, values, FormService.GetCurrentUser());
                    AJAXActions actions = FormService.GetAJAXActions(actionKey, FormService.GetSelectedValues(keys, values));
                    if (actionLog != null) FormService.LogActionOperationTime(actionLog);
                    return actions;
                }
                else return FormService.GetNotAuthenticatedResult();
            }
            catch (Exception exp)
            {
                ExceptionLog exLog = null;
                if (actionLog != null)
                {
                    exLog = FormService.LogException(exp, actionLog);
                }
                if (exLog != null && PerFormsConfig.EmailConfig.EmailActive)
                {
                    FormService.EmailException(exLog);
                    return new AJAXActions().AddShowMessageAction(ShowMessageAction.EMessageType.ERROR, "[ERROR]", "[ERROR]", "[ExceptionErrorMessage][" + exLog.Guid + "]");
                }
                else
                {
                    return new AJAXActions().AddShowMessageAction(ShowMessageAction.EMessageType.ERROR, "[ERROR]", "[ERROR]", "[GenericErrorMessage]");
                }
            }
            finally
            {
                NH_PERFORMS.Instance.Disconnect();
            }
        }

        public string GetInitialForm(string actionKey)
        {
            string output = "<script type='text/javascript'>";
            AJAXActions actions = GetAJAXActions(actionKey, null, null);
            foreach (AJAXAction action in actions.Actions)
            {
                switch (action.AJAXActionType)
                {
                    case AJAXAction.EAJAXActionType.ShowForm:
                        ShowFormAction showFormAction = action as ShowFormAction;
                        output += "</script>" + showFormAction.Form + "<script type='text/javascript'>";
                        break;
                    case AJAXAction.EAJAXActionType.ShowMessage:
                        ShowMessageAction showMessageAction = action as ShowMessageAction;
                        output += "performs_showMessageDialog('" + showMessageAction.MessageType + "','" + showMessageAction.MessageTitle + "', '" + showMessageAction.MessageSubtitle + "', '" + showMessageAction.MessageContent + "');";
                        break;
                    case AJAXAction.EAJAXActionType.UpdateItems:
                        break;
                    case AJAXAction.EAJAXActionType.RedirectToUrl:
                        RedirectToURLAction redirectToURLAction = action as RedirectToURLAction;
                        output += "window.location = '" + redirectToURLAction.RedirectToURL + "';";
                        break;
                    case AJAXAction.EAJAXActionType.SetValues:
                        break;
                }
            }
            output += "</script>";
            return output;
        }
    }
}