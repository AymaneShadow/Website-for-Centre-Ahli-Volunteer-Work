using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login_Click(object sender, EventArgs e)
    {

        string identifier = txtIdentifier.Value,
                password = txtPassword.Value;

        string query_loginCheck = "SELECT dbo.fnLoginValidity('" + identifier + "','" + password + "');";
        string query_login = "SELECT P_ID FROM PERSON " +
                                "WHERE (P_USERNAME = '" + identifier + "' AND P_PASSWORD ='" + password + "') " +
                                "OR (P_EMAIL = '" + identifier + "' AND P_PASSWORD = '" + password + "');";


        SqlCommand loginCheckCommand = null, loginCommand = null;
        SqlDataReader loginCheckDataReader = null, loginDataReader = null;

        int status_login;
        DataTable dt;
        int user_id;

        loginCheckCommand = new SqlCommand(query_loginCheck, dbConnectionSQLServer.cn);
        loginCheckDataReader = loginCheckCommand.ExecuteReader();

        while (loginCheckDataReader.Read())
        {
            status_login = (Int32)loginCheckDataReader.GetValue(0);

            //Response.Write("status_login is: " + status_login + "</br>");

            if (status_login == 1)
            {
                loginCheckDataReader.Close();
                loginCommand = new SqlCommand(query_login, dbConnectionSQLServer.cn);
                loginDataReader = loginCommand.ExecuteReader();

                while (loginDataReader.Read())
                {
                    user_id = (Int32)loginDataReader.GetValue(0);
                    loginDataReader.Close();

                    SqlDataAdapter sda = new SqlDataAdapter(loginCommand);

                    dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Session["id"] = user_id;
                        Response.Redirect("~/");
                        Session.RemoveAll();
                    }
                }
            }
            else
            {
                error_div.Visible = true;
            }
        }

        if (loginCheckDataReader != null && loginDataReader != null && loginCheckCommand != null && loginCommand != null)
        {
            loginCheckDataReader.Close();
            loginDataReader.Close();
            loginCheckCommand.Dispose();
            loginCommand.Dispose();
            dbConnectionSQLServer.cn.Close();
        }



    }
}