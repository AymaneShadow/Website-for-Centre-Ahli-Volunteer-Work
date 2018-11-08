using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Delete_Comment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string c_id = HttpUtility.ParseQueryString(myUri.Query).Get("c_id");
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteMaster)this.Master).dbConnectionSQLServer;

        string sql = "DELETE FROM ACTIVITY_COMMENT WHERE A_COM_ID = @c_id;";

        SqlCommand command2;
        //SqlDataReader dataReader2;

        try
        {

            command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            //dataReader2 = command2.ExecuteReader();

            command2.Parameters.AddWithValue("@c_id", c_id);

            command2.ExecuteNonQuery();

            //dataReader2.Close();
            command2.Dispose();
            //dbConnectionSQLServer.cn.Close();
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message + " in Delete_Comment().</br>");
        }

        //HttpContext.Current.Response.Redirect("Activity_Locations.aspx?a_id=" + a_id);

    }
}