using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerForms.Util;

public partial class God : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new HeadLinksAndTemplates(HeadLinksAndTemplates.ELang.PT).AddPerFormsLinksAndTemplates(this);

        if (!Page.IsPostBack)
        {
            string html = new PerFormsWS().GetInitialForm("God");
            litForm.Text = html;
        }
    }
}