using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PerForms_RenderControl : System.Web.UI.UserControl
{
    private Control _CTL;
    public Control CTL
    {
        get { return _CTL; }
        set { _CTL = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        pHControl.Controls.Add(CTL);
    }
}