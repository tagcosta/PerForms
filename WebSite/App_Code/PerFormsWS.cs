using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PerForms;
using System.Data;
using System.Collections;
using PerForms.Actions;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class PerFormsWS : System.Web.Services.WebService
{
    private WebServiceInterface WebServiceInterface = new WebServiceInterface(new Custom_PerFormsService());

    [WebMethod(EnableSession = true)]
    public AJAXActions GetAJAXActions(string actionKey, string[] keys, string[] values)
    {
        return WebServiceInterface.GetAJAXActions(actionKey, keys, values);
    }

    public string GetInitialForm(string actionKey)
    {
        return WebServiceInterface.GetInitialForm(actionKey);
    }
}