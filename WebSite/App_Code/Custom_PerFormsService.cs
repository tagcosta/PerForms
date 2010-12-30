using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerForms;
using PerForms.JQGrid;
using PerForms.Filters;
using PerForms.Util;
using PerForms.Actions;

public class Custom_PerFormsService : FormService
{
    #region Expendable Aux Methods

    private List<KeyValue> GetYesOrNo()
    {
        List<KeyValue> list = new List<KeyValue>();
        list.Add(new KeyValue("0", "Não"));
        list.Add(new KeyValue("1", "Sim"));
        return list;
    }

    private List<KeyValue> Get10Things()
    {
        List<KeyValue> list = new List<KeyValue>();
        list.Add(new KeyValue("1", "Thing 1"));
        list.Add(new KeyValue("2", "Thing 2"));
        list.Add(new KeyValue("3", "Thing 3"));
        list.Add(new KeyValue("4", "Thing 4"));
        list.Add(new KeyValue("5", "Thing 5"));
        list.Add(new KeyValue("6", "Thing 6"));
        list.Add(new KeyValue("7", "Thing 7"));
        list.Add(new KeyValue("8", "Thing 8"));
        list.Add(new KeyValue("9", "Thing 9"));
        list.Add(new KeyValue("10", "Thing 10"));
        return list;
    }

    private List<KeyValue> GetXSubThings(int things)
    {
        List<KeyValue> list = new List<KeyValue>();
        for (int i = 0; i < things; i++)
        {
            list.Add(new KeyValue((i + 1).ToString(), "SubThing " + (i + 1)));
        }
        return list;
    }

    #endregion

    public enum MyEnum { No = 0, Yes = 1 }

    public override bool IsAuthenticated()
    {
        /*if (HttpContext.Current.Session["UserID"] == null)
        {
            HttpContext.Current.Session["UserID"] = 0;
            return true;
        }
        else return false;*/
        return true;
    }

    public override string GetQuery(string actionKey, Dictionary<string, List<string>> values, out string columnName)
    {
        switch (actionKey)
        {
            case "GetActionLogs":
                columnName = "ID";
                return "SELECT * FROM [PrFActionLog]";

            case "GetActionLogParameters":
                columnName = "ID";
                return "SELECT * FROM [PrFActionLogParameter]";

            default:
                columnName = "ID";
                return "SELECT [ID],[Milliseconds],[ActionKey] 'Action Key',[UserKey] 'User Key',[Date] FROM [PrFActionLog]";
        }
    }

    public override string GetCurrentUser()
    {
        return "USER";
    }

    public override AJAXActions GetAJAXActions(string actionKey, Dictionary<string, List<string>> values)
    {
        string html = string.Empty;
        switch (actionKey)
        {
            case "Things":
                return new AJAXActions().AddUpdateItemsAction("SubThings", GetXSubThings(GetSelectedID(values)));
            case "Reception":
                html = new PrForm("Reception")
                    .SetHeader("Recepção")
                    .SetFooter("&nbsp;")
                    .AddFieldGridView("GV", "GetActionLogs", this, true, "Action Logs").SetRequired().SetMultipleSelection()
                    .AddFieldText("Text", "Text").SetRequired()
                    .AddFieldRadios("Radios", "Radios", GetYesOrNo())
                        .AddConditionalVisibility(MyEnum.No, "X")
                    .AddFieldLabel("X", "X", "X").SetHidden()
                    .AddFieldComboBox("Things", "Things", Get10Things()).SetDefaultValue("5").AddGetAJAXActions("Things")
                    .AddFieldMultiSelectBox("SubThings", "SubThings", GetXSubThings(10)).AddGetAJAXActions("Submit")
                    .AddFieldButton("Submit", "Sub'mit")
                        .AddGetAJAXActionsAll("Submit")
                    .RenderForm();
                return new AJAXActions().AddShowFormAction(html);
            case "CheckIn":
                html = new PrForm("CheckIn")
                    .SetHeader("CheckIn")
                    .SetFooter("&nbsp;")
                    .AddFieldLabel("Label", "Label", GetSelectedValue(values))
                    .AddFieldButton("Cancel", "Cancel")
                        .AddCancel()
                    .RenderForm();
                return new AJAXActions().AddShowFormAction(html, ShowFormAction.EPreviousFormActionType.MERGE);
            case "God":
                html = new PrForm("God")
                    .SetModalDialog()
                    .SetHeader("<b>Form Header</b>")
                    .SetFooter("<b>Form Footer</b>")
                    .AddFieldCheckBoxes("CheckBoxes", "CheckBoxes", GetXSubThings(3))
                    .AddFieldComboBox("ComboBox", "ComboBox", GetXSubThings(20))
                    .AddFieldDate("Date", "Date")
                    .AddFieldFree("Free", "Free", "<i>Free</i>")
                    .AddFieldLabel("Label", "Label", "Label")
                    .AddFieldMultiSelectBox("MultiSelectBox", "MultiSelectBox", Get10Things())
                    .AddFieldRadios("Radios", "Radios", GetXSubThings(3))
                    .AddFieldText("Text", "Text")
                    .AddFieldTextArea("TextArea", "TextArea")
                    .AddFieldUpload("Upload", "Upload")
                    .AddFieldButton("Button", "Button")
                    .RenderForm();
                return new AJAXActions().AddShowFormAction(html, ShowFormAction.EPreviousFormActionType.APPEND);
            case "Submit":
                return GetAJAXActions("God", null).AddShowMessageAction("Informação", "Operação concluída com sucesso.");
                //return new AJAXActions().AddShowMessageAction("Informação", "Operação concluída com sucesso.");
            default: throw new NotImplementedException();
        }
    }
}