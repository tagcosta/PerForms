<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        string connectionString = "Data Source=TAG-PC\\SQLEXPRESS;Initial Catalog=PerForms;Integrated Security=True";
        
        PerForms.Config.PerFormsConfig.DBConfig.LogsConnectionString = connectionString;
        PerForms.Config.PerFormsConfig.DBConfig.DataConnectionString = connectionString;
    }
</script>