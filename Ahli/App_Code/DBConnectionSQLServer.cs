using System;
using System.Data.SqlClient;
using System.Web;


public class DBConnectionSQLServer
{
    public string connetionString = "Data Source=AymaneShadow-PC;Initial Catalog=AFPEE;User ID=AymaneShadow;Password=ThisShouldBeAPassword";
    
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
