using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Add_Comment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string commentToAdd = HttpUtility.ParseQueryString(myUri.Query).Get("commentToAdd");
        string replyToCommentID = HttpUtility.ParseQueryString(myUri.Query).Get("replyToCommentID");

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteMaster)this.Master).dbConnectionSQLServer;

        //DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();

        string commentText = "Dummy commentText";
        string username = "Dummy username";

        DateTime commentPubDate;

        int c_id = -1;
        int u_id = -1;
        int sqlA_ID = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string sql;

        if (Session["id"] != null)
        {
            u_id = (int)Session["id"];

            sql = "SELECT P_USERNAME FROM PERSON WHERE P_ID = " + Session["id"] + ";";

            SqlCommand command;
            SqlDataReader dataReader;

            try
            {
                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    username = (string)dataReader.GetValue(0);
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " in submitCommentButton_Click().</br>");
            }


            sql = "INSERT INTO ACTIVITY_COMMENT VALUES (@u_id, @a_id, @commentToAdd, SYSDATETIME(), @replyToCommentID);";

            SqlCommand command2;
            //SqlDataReader dataReader2;

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }


                command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);
                //dataReader2 = command2.ExecuteReader();

                command2.Parameters.AddWithValue("@u_id", u_id);
                command2.Parameters.AddWithValue("@a_id", a_id);
                command2.Parameters.AddWithValue("@commentToAdd", commentToAdd);

                if(replyToCommentID == "NULL")
                command2.Parameters.AddWithValue("@replyToCommentID", DBNull.Value);
                else
                command2.Parameters.AddWithValue("@replyToCommentID", replyToCommentID);

                command2.ExecuteNonQuery();

                //dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " in submitCommentButton_Click() in insertion part.</br>");
            }

        }
        else
        {
            Response.Write("You can't submit a comment since you're not loged-in.<br />");
        }

        //Response.Redirect("Activity_Locations.aspx?a_id=" + a_id);
    }


}