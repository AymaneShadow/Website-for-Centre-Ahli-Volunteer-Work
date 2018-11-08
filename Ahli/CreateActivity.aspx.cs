using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateActivity : System.Web.UI.Page
{

    DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();
    bool allGood = true;

    protected void Page_Load(object sender, EventArgs e)
    {
            if (Session["id"] != null)
            {
                string id = Session["id"].ToString();

                SqlDataReader dataReader;
                SqlCommand command_cities, command_categories;

                try
                {

                    if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }

                    string query_Cities = "SELECT CI.CI_NAME FROM CITY AS CI JOIN COUNTRY AS CO ON CI.CI_CO_ID = CO.CO_ID;";
                    string city;

                    command_cities = new SqlCommand(query_Cities, dbConnectionSQLServer.cn);
                    dataReader = command_cities.ExecuteReader();

                    while (dataReader.Read())
                    {
                        // reader.GetSqlInt32(1).ToString()
                        city = dataReader.GetValue(0).ToString();
                        cityList.Items.Add(new ListItem(city));
                    }

                    dataReader.Close();
                    command_cities.Dispose();

                    dbConnectionSQLServer.cn.Close();
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number:(cities dropdown) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR:(cities dropdown) " + ex.Message + "</p>\n");
                }
                //TRY FOR THE CATEGORY DROPWDOWN
                try
                {

                    if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }

                    string query_Categories = "SELECT SUB_CAT_NAME FROM SUB_CAT ORDER BY SUB_CAT_ID;";
                    string subCategory;

                    command_categories = new SqlCommand(query_Categories, dbConnectionSQLServer.cn);
                    dataReader = command_categories.ExecuteReader();

                    while (dataReader.Read())
                    {
                        subCategory = (String)dataReader.GetValue(0);
                        categoryList.Items.Add(new ListItem(subCategory));
                    }

                    dataReader.Close();
                    command_categories.Dispose();
                    dbConnectionSQLServer.cn.Close();
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number:(category dropdown) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR:(category dropdown) " + ex.Message + "</p>\n");
                }


                try
                {

                    PopulateChildList();
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number: (PopulateChildList)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR: (PopulateChildList)" + ex.Message + "</p>\n");
                }

            }
            else
            {
                Response.Redirect("~/Login");
            }        
    }

    public void PopulateChildList()
    {
        string childName = "";
        int childID = -1;

        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        string query_children = "SELECT CONCAT(CH_LNAME, ' ' + CH_FNAME), CH_ID FROM CHILD WHERE CH_LEFT_CENTER_DATE IS NULL;";
        SqlCommand cmd_children = new SqlCommand(query_children, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = cmd_children.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    childName = reader.GetString(0);
                    childID =  reader.GetInt32(1);

                    childrenList.Items.Add(new ListItem(childName, childID.ToString()));

                } while (reader.Read());
            }

            reader.Close();
            cmd_children.Dispose();
            dbConnectionSQLServer.cn.Close();
        }
    }


    protected void CustomVal_Description(object sender, ServerValidateEventArgs e)
    {
        int length = descriptionForm.ToString().Length;
        if (length > 4000)
        {
            e.IsValid = false;
            allGood = false;
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void CustomVal_EndDate(object sender, ServerValidateEventArgs e)
    {
        DateTime dt1 = DateTime.ParseExact(startDateForm.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime dt2 = DateTime.ParseExact(endDateForm.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (dt1 > dt2)
        {
            e.IsValid = false;
            allGood = false;
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void CustomVal_EndTime(object sender, ServerValidateEventArgs e)
    {
        DateTime dt1 = DateTime.ParseExact(startDateForm.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime dt2 = DateTime.ParseExact(endDateForm.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (dt1 == dt2)
        {
            DateTime time1 = DateTime.Parse(startTimeForm.Value.ToString());
            DateTime time2 = DateTime.Parse(endTimeForm.Value.ToString());
            if (time1 > time2)
            {
                e.IsValid = false;
                allGood = false;
            }
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        if (allGood)
        {

            //Response.Write("<p>Values: </p>\n");
            //Response.Write("<p>txtLocationAddress: " + txtLocationAddress.Value.ToString() + "</p>\n");
            //Response.Write("<p>CityList: " + CityList.SelectedValue.ToString() + "</p>\n");
            //Response.Write("<p>CityList: " + CityList.SelectedValue.ToString() + "</p>\n");


            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }

            if (Session["id"] != null)
            {
                try
                {
                    SqlCommand command_checkAddress, command_countryID, command_suBCategoryID, command_addLocation, command_addActivity, command_addCreator;

                    int cityID = getCityID();

                    string query_countryID = "SELECT CI_CO_ID FROM CITY WHERE CI_ID = " + cityID + " ;";
                    int countryID;

                    command_countryID = new SqlCommand(query_countryID, dbConnectionSQLServer.cn);
                    countryID = (Int32)command_countryID.ExecuteScalar();


                    string query_checkAddress = "SELECT dbo.fnCheckAdress('" + addressForm.Value.ToString() + "', (SELECT LOC_ID FROM LOCALITY WHERE LOC_ADDRESS = '" + addressForm.Value.ToString().Replace("'", "''") + "' AND LOC_CI_ID = " + cityID + "))";
                    int status_addressCheck = 1;

                    // Response.Write(query_checkAddress);

                    command_checkAddress = new SqlCommand(query_checkAddress, dbConnectionSQLServer.cn);
                    status_addressCheck = (Int32)command_checkAddress.ExecuteScalar();

                    string query_suBCategoryID = "SELECT SUB_CAT_ID FROM SUB_CAT WHERE SUB_CAT_NAME = '" + categoryList.SelectedItem.Text + "';";
                    int suBCategoryID;

                    command_suBCategoryID = new SqlCommand(query_suBCategoryID, dbConnectionSQLServer.cn);
                    suBCategoryID = (Int32)command_suBCategoryID.ExecuteScalar();

                    //Response.Write("\n<p>status_addressCheck= " + status_addressCheck + "</p>\n");


                    if (status_addressCheck == 0)
                    {
                        string query_addLocation = "INSERT INTO LOCALITY VALUES(N'" + localityNameForm.Value.ToString() + "', " + cityID + ", N'" + addressForm.Value.ToString() + "');";

                        command_addLocation = new SqlCommand(query_addLocation, dbConnectionSQLServer.cn);

                        command_addLocation.ExecuteNonQuery();
                        command_addLocation.Dispose();
                    }


                    // Response.Write("txtLocationAddress.Value.ToString() is \"" + txtLocationAddress.Value.ToString());
                    // Response.Write("\" and CityList.SelectedValue.ToString() is \"" + CityList.SelectedValue.ToString() + "\"");

                    int id = (int)Session["id"];
                    string query_Insertactivity = "INSERT INTO ACTIVITY VALUES (N'" + nameForm.Value.ToString().Replace("'", "''") +
                                                                                "', N'" + descriptionForm.Value.ToString().Replace("'", "''") +
                                                                                "', (SELECT LOC_ID FROM LOCALITY WHERE LOC_ADDRESS = N'" + addressForm.Value.ToString().Replace("'", "''") + "' AND LOC_CI_ID = " + cityID + ")" +
                                                                                ", (SELECT SUB_CAT_MAIN_CAT_ID FROM SUB_CAT WHERE SUB_CAT_ID = " + suBCategoryID + ")" +
                                                                                ", " + suBCategoryID +
                                                                                ", '" + startDateForm.Value.ToString() + " " + startTimeForm.Value.ToString() +
                                                                                "', '" + endDateForm.Value.ToString() + " " + endTimeForm.Value.ToString() +
                                                                                "', " + capacityForm.Value.ToString() +
                                                                                ", " + capacityForm.Value.ToString() + ");";

                    // Response.Write("<p>The QUery:" + query_Insertactivity + "</p>");

                    command_addActivity = new SqlCommand(query_Insertactivity, dbConnectionSQLServer.cn);
                    command_addActivity.ExecuteNonQuery();


                    string query_Creator = "INSERT INTO SUBSCRIBE VALUES (" + id + ", IDENT_CURRENT('ACTIVITY'), 'CREATOR', SYSDATETIME());";
                    command_addCreator = new SqlCommand(query_Creator, dbConnectionSQLServer.cn);
                    command_addCreator.ExecuteNonQuery();


                    command_addCreator.Dispose();
                    command_addActivity.Dispose();
                    //command_checkAddress.Dispose();
                    dbConnectionSQLServer.cn.Close();

                    addChildren();

                    createActivityForm.Visible = false;
                    successHref.Attributes.Add("href", "~/Activity?a_id=" + getLatestActivityID().ToString());
                    successMessage.Visible = true;                    

                    // Response.Redirect("~/");
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number:(submit) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR:(submit) " + ex.Message + "</p>\n");
                }
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("Make sure you login before you create a new event!");
            }

        }
    }


    public void addChildren()
    {
        int child_id = -1;

        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        try
        {
            string selectedChildren = Request.Form[childrenList_to.UniqueID].ToString();

            string[] namesArray = selectedChildren.Split(',');

            SqlCommand command_addChild;


            for(int i = 0; i < namesArray.Length; i++)
            {
                child_id = Convert.ToInt32(namesArray[i]);

//                Response.Write("child_id is: " + child_id + "<br />");

                string query_addChild = "INSERT INTO SUBSCRIBE_CHILD VALUES (" + child_id + ", IDENT_CURRENT('ACTIVITY'), SYSDATETIME());";
                command_addChild = new SqlCommand(query_addChild, dbConnectionSQLServer.cn);
                command_addChild.ExecuteNonQuery();
                command_addChild.Dispose();
            }        
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number:(addChildren) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR:(addChildren) " + ex.Message + "</p>\n");
        }
    }

// Not used.
/*    public int getChildID(string childName)
    {
        SqlCommand command_childID;
        string query_childID = "SELECT CH_ID FROM CHILD WHERE CONCAT(CH_LNAME, ' ' + CH_FNAME) = '" + childName + "' AND CH_LEFT_CENTER_DATE IS NULL ;";
        int childID = -1;

        try
        {
            command_childID = new SqlCommand(query_childID, dbConnectionSQLServer.cn);
            childID = (Int32)command_childID.ExecuteScalar();

            command_childID.Dispose();
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number:(getChildID) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR:(getChildID) " + ex.Message + "</p>\n");
        }

        return childID;
    }*/

    public int getCityID()
    {
        SqlCommand command_cityID;
        string query_cityID = "SELECT CI_ID FROM CITY WHERE CI_NAME = '" + cityList.SelectedItem.Text + "' ;";
        int cityID = -1;

        try
        {
            command_cityID = new SqlCommand(query_cityID, dbConnectionSQLServer.cn);
            cityID = (Int32)command_cityID.ExecuteScalar();

            command_cityID.Dispose();
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number:(getCityID) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR:(getCityID) " + ex.Message + "</p>\n");
        }

        return cityID;
    }

    public int getLatestActivityID()
    {

        SqlCommand command_activityID;
        string query_activityID = "SELECT IDENT_CURRENT('ACTIVITY');";
        int activityID = -1;

        try
        {
            command_activityID = new SqlCommand(query_activityID, dbConnectionSQLServer.cn);
            activityID = Convert.ToInt32(command_activityID.ExecuteScalar());

            command_activityID.Dispose();
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number:(getLatestActivityID) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR:(getLatestActivityID) " + ex.Message + "</p>\n");
        }

        return activityID;
    }
}