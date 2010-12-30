using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PerForms;
using System.Data;
using PerForms.Util;

public partial class _Default : System.Web.UI.Page
{
    private DataTable GetDataTable()
    {
        Random random = new Random();

        DataTable dt = new DataTable("Tabela Teste");
        dt.Columns.Add("ID", typeof(Int32));
        dt.Columns.Add("Número Inteiro", typeof(Int32));
        dt.Columns.Add("Número Double", typeof(Double));
        dt.Columns.Add("Descrição", typeof(String));
        dt.Columns.Add("Data", typeof(DateTime));

        for (int i = 0; i < 20; i++)
        {
            DataRow dr = dt.NewRow();

            dr["ID"] = i + 1;
            dr["Número Inteiro"] = random.Next();
            dr["Número Double"] = random.NextDouble();
            dr["Descrição"] = "sadsdadasd asd asd sa d";
            dr["Data"] = DateTime.Now;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        new HeadLinksAndTemplates(HeadLinksAndTemplates.ELang.PT).AddPerFormsLinksAndTemplates(this);
        if (!Page.IsPostBack) litForm.Text = new PerFormsWS().GetInitialForm("Reception");
    }

    private List<KeyValue> GetYesNo()
    {
        List<KeyValue> list = new List<KeyValue>();
        list.Add(new KeyValue("1", "Yes"));
        list.Add(new KeyValue("2", "No"));
        return list;
    }

    private List<KeyValue> GetVDs(string name, int number)
    {
        List<KeyValue> list = new List<KeyValue>();
        for (int i = 1; i <= number; i++)
        {
            list.Add(new KeyValue(i.ToString(), name + " " + i));
        }
        return list;
    }
}