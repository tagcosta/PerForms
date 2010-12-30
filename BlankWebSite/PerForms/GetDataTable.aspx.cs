using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PerForms;
using PerForms.JQGrid;
using PerForms.Filters;
using System.Data.SqlClient;
using System.Collections.Specialized;
using PerForms.Util;

public partial class PerForms_GetDataTable : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        string actionKey = string.Empty;
        if (Request.QueryString.AllKeys.Contains("actionKey"))
        {
            actionKey = Request.QueryString["actionKey"];
        }

        QueryStringInfo queryStringInfo = new QueryStringInfo().Fill(Request.QueryString);
        
        Response.Clear();

        JQGrid grid = new Custom_PerFormsService().GetJQGrid(actionKey, null, queryStringInfo);
        
        Response.Write(new JSON().Serialize(grid));
        Response.End();
    }
}