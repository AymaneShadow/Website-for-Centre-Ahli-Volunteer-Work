using System;
using System.Data.SqlClient;
using System.Web;


public class DBConnectionSQLServer
{
    public string connetionString = "Data Source=AymaneShadow-PC;Initial Catalog=AFPEE;User ID=AymaneShadow;Password=ThisShouldBeAPassword";
    //public string connetionString = "Data Source=SQL6003.site4now.net;Initial Catalog=DB_A3227B_Ahli;User ID=DB_A3227B_Ahli_admin;Password=EfbV4e9kjQbH-b5C";
    public SqlConnection cn;

    public DBConnectionSQLServer()
    {
        cn = new SqlConnection(connetionString);

        try
        {
            cn.Open();
            //HttpContext.Current.Response.Write("Connected to the SQL Server successfully.</br>");

        }
        catch (Exception ex)
        {
            //HttpContext.Current.Response.Write("Could not open connection to SQL Server.</br>");
        }
    }

}
