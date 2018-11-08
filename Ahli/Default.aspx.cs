using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadActivities();
    }

    protected void LoadActivities()
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteMaster)this.Master).dbConnectionSQLServer;

        // Flag variable to check how many rows have been used in the loop.
        int i = 0;

        // Flag variable to check if the connected user has preferences that match the activities or not.
        // 0 for found matches, 1 for no matches found.
        int connectedButNoPreferenceMatch = 0;

        // Variables used in the html building.
        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string activityName = "Dummy activity name";
        string activityLocationName = "Dummy activity's location name";
        string activityDescription = "Dummy activity description";
        string sub_cat_name = "Dummy sub-category name";
        ArrayList categoriesList = new ArrayList();

        int a_id = -1;        

        // String variables to store the SQL queries.
        string sql, sql1, sql2;

        sql1 = "SELECT A_NAME, CI.CI_NAME, A_DESCRIPTION, A_ID, SC.SUB_CAT_NAME FROM ACTIVITY AS A JOIN LOCALITY AS L ON A.A_LOC_ID = L.LOC_ID JOIN CITY AS CI ON L.LOC_CI_ID = CI.CI_ID JOIN SUB_CAT AS SC ON A.A_SUB_CAT_ID = SC.SUB_CAT_ID; ";

        sql = sql1;
        categoriesList = getCategories();
/*
        // If no one is connected.
        if (Session["id"] == null)
        {
            sql = sql1;
        }
        else // If someone is connected.
        {
            sql2 = "SELECT A_NAME, CI.CI_NAME, A_DESCRIPTION, A_ID FROM ACTIVITY AS A JOIN LOCALITY AS L ON A.A_LOC_ID = L.LOC_ID JOIN CITY AS CI ON L.LOC_CI_ID = CI.CI_ID JOIN PREFERENCE AS P ON A.A_SUB_CAT_ID = P.PREF_SUB_CAT_ID JOIN PERSON AS PE ON P.PREF_P_ID = PE.P_ID WHERE P.PREF_P_ID =" + Session["id"] + "; ";

            sql = sql2;            
        }

*/

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }            

            SqlCommand command;
            SqlDataReader dataReader;

            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            

            // Check if the connected user has preferences that match events.
            // If he doesn't, make the SQL query to fetch all activities.

            if (Session["id"] != null && !dataReader.HasRows)
            {
                dataReader.Close();
                command.Dispose();

                sql = sql1;

                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                // Make this variable 1 to say that there are no preferences that match the activities.
                connectedButNoPreferenceMatch = 1;
            }

            for (int k = 0; k < categoriesList.Count; k++)
            {
                HtmlGenericControl liCategory = new HtmlGenericControl("li");

                HtmlGenericControl aCategory = new HtmlGenericControl("a");
                aCategory.InnerText = (string)categoriesList.ToArray()[k].ToString();
                aCategory.Attributes.Add("class", "btn btn-default");
                aCategory.Attributes.Add("data-filter", "." + (string)categoriesList.ToArray()[k].ToString().ToLower().Replace(" ", String.Empty));

                liCategory.Controls.Add(aCategory);
                portfolioFilter.Controls.Add(liCategory);
            }

            while (dataReader.Read())
            {                
                activityName = (string)dataReader.GetValue(0);
                activityLocationName = (string)dataReader.GetValue(1);
                activityDescription = truncate((string)dataReader.GetValue(2), 65);
                a_id = (int)dataReader.GetValue(3);
                sub_cat_name = (string)dataReader.GetValue(4);

                string imgSrc;

                if (File.Exists(absolutePath + "activities/" + "event_" + a_id + ".jpg"))
                    imgSrc = "images/activities/" + "event_" + a_id + ".jpg";
                else
                    imgSrc = "images/activities/defaultEvent.png";

                HtmlGenericControl portfolioItem = new HtmlGenericControl("div");
                portfolioItem.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-3 portfolio-item " + sub_cat_name.ToLower().Replace(" ", String.Empty) + "");
                portfolioItem.InnerHtml = "<div class=\"portfolio wrapper\">" +
                    "<div class=\"portfolio-single\">" +
                    "<div class=\"portfolio-thumb\">" +
                    "<img src=\"" + imgSrc + "\" class=\"img-responsive\" alt=\"" + activityLocationName.ToLower() + "\">" +
                    "</div>" +
                    "<div class=\"portfolio-view\">" +
                    "<ul class=\"nav nav-pills\">" +
                    "<li><a href=\"Activity.aspx?a_id=" + a_id +  "\"><i class=\"fa fa-link\"></i></a></li>" +
                    "<li><a href=\"" + imgSrc + "\" data-lightbox=\"example-set\"><i class=\"fa fa-eye\"></i></a></li>" +
                    "</ul>" +
                    "</div>" +
                    "</div>" +
                    "<div class=\"portfolio-info \">" +
                    "<h2>" + activityName + "</h2>" +
                    "<p> " + activityDescription + "</p>" +
                    "</div>" +
                    "</div>";

                portfolioItems.Controls.Add(portfolioItem);

/*
                    // For every 3 activities found, make a new <ul>.
                    if ((i % 3) == 0)
                    {
                        ul = new HtmlGenericControl("ul");
                        ul.Attributes.Add("class", "cityContainer list-unstyled row");
                        ulContainer.Controls.Add(ul);

                    }

                    ul.Controls.Add(li);
*/

// Increase row count.
                i++;

                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();            
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message + " in LoadActivities().</br>");
        }

    }

    protected ArrayList getCategories(int id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteMaster)this.Master).dbConnectionSQLServer;

            string subCatName = "Dummy subcategory name";
            ArrayList categoriesList = new ArrayList();


            string sql = "SELECT SC.SUB_CAT_NAME FROM PREFERENCE AS P JOIN SUB_CAT AS SC ON P.PREF_SUB_CAT_ID = SC.SUB_CAT_ID WHERE PREF_P_ID = " + id + ";";

            SqlCommand command;
            SqlDataReader dataReader;

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }


                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    subCatName = (String)dataReader.GetValue(0);
                    categoriesList.Add(FirstCharToUpper(subCatName.ToLower()));
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message + " in getCategories().</br>");
            }

            char[] charsToTrim = {'-', ' '};

            return categoriesList;
    }


    protected ArrayList getCategories()
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteMaster)this.Master).dbConnectionSQLServer;

        string subCatName = "Dummy subcategory name";
        ArrayList categoriesList = new ArrayList();


        string sql = "SELECT SUB_CAT_NAME FROM SUB_CAT;";

        SqlCommand command;
        SqlDataReader dataReader;

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                subCatName = (String)dataReader.GetValue(0);
                categoriesList.Add(FirstCharToUpper(subCatName.ToLower()));
            }

            dataReader.Close();
            command.Dispose();
            dbConnectionSQLServer.cn.Close();
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message + " in getCategories().</br>");
        }

        char[] charsToTrim = { '-', ' ' };

        return categoriesList;
    }

    protected string truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }

    public static string FirstCharToUpper(string input)
    {
        if (String.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
    }
}