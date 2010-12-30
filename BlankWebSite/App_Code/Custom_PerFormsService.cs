using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerForms;
using PerForms.Util;
using PerForms.Actions;

public class Custom_PerFormsService : FormService
{
    public override string GetCurrentUser()
    {
        throw new NotImplementedException();
    }

    public override AJAXActions GetAJAXActions(string actionKey, Dictionary<string, List<string>> values)
    {
        switch (actionKey)
        {
            case "Test":
                string form = new PrForm("Test")
                    .SetHeader("Test")
                    .SetFooter("&nbsp;")
                    .AddFieldText("Test", "Test")
                    .RenderForm();
                return new AJAXActions().AddShowFormAction(form);
            default: throw new NotImplementedException();
        }
    }

    public override string GetQuery(string actionKey, Dictionary<string, List<string>> values, out string columnName)
    {
        throw new NotImplementedException();
    }

    public override bool IsAuthenticated()
    {
        //ToDo: You should validate if the user is valid.
        return true;
    }
}