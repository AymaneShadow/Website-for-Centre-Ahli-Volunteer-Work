using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddChild : System.Web.UI.Page
{
    DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();
    bool allGood = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] != null)
        {
            string id = Session["id"].ToString();

            SqlDataReader dataReader;
            SqlCommand command_cities;

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }

                string query_Cities = "SELECT CI.CI_NAME FROM CITY AS CI JOIN COUNTRY AS CO ON CI.CI_CO_ID = CO.CO_ID WHERE CO.CO_NAME = 'MOROCCO';";
                string city;

                command_cities = new SqlCommand(query_Cities, dbConnectionSQLServer.cn);
                dataReader = command_cities.ExecuteReader();

                for(int i = 0; dataReader.Read(); i++)
                {
                    // reader.GetSqlInt32(1).ToString()

                    // if (i == 0)
                    //    birthPlaceForm.Items.Add(new ListItem("- Birth City -", "0"));                    

                        city = dataReader.GetValue(0).ToString();
                        birthPlaceForm.Items.Add(new ListItem(city));                    
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
        }
        else
        {
            Response.Redirect("~/Login");
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
                    // addLocation();

                    // Response.Write("txtLocationAddress.Value.ToString() is \"" + txtLocationAddress.Value.ToString());
                    // Response.Write("\" and CityList.SelectedValue.ToString() is \"" + CityList.SelectedValue.ToString() + "\"");

                    int id = (int) Session["id"];

                    string firstName = firstNameForm.Value.ToString().Replace("'", "''");
                    string lastName = lastNameForm.Value.ToString().Replace("'", "''");
                    string gender = genderForm.SelectedItem.Text.Replace("'", "''");
                    string address = addressForm.Value.ToString().Replace("'", "''");
                    string birthday = Request.Form[birthdayHiddenInput.UniqueID].Replace("'", "''");
                    string birthPlace = birthPlaceForm.SelectedItem.Text.ToString().Replace("'", "''");
                    string phoneNumber = phoneNumberForm.Value.ToString().Replace("'", "''");
                    string joinedCenterMethod = joinedCenterMethodForm.Value.ToString().Replace("'", "''");
                    string joinedCenterDate = joinedCenterDateHiddenInput.Value.ToString().Replace("'", "''");
                    string joinedCenterReason = joinedCenterReasonForm.Value.ToString().Replace("'", "''");
                    string extraInfo = extraInfoForm.Value.ToString().Replace("'", "''");

                    string query_addChild = "INSERT INTO CHILD VALUES ('" + firstName + "', " +
                        "'" + lastName + "', " +
                        "'" + gender + "', " +
                        "'" + address + "', " +
                        "'" + birthday + "', " +
                        "'" + birthPlace + "', " +
                        "'" + phoneNumber + "', " +
                        "'" + joinedCenterMethod + "', " +
                        "'" + joinedCenterDate + "', " +
                        "'" + joinedCenterReason + "', " +
                        "NULL, " +
                        "NULL, " +
                        "'" + extraInfo + "');";

                    SqlCommand command_addChild;
                    command_addChild = new SqlCommand(query_addChild, dbConnectionSQLServer.cn);
                    command_addChild.ExecuteNonQuery();

                    dbConnectionSQLServer.cn.Close();

                    addChildForm.Visible = false;
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

    public void addLocation()
    {
        try
        {
            SqlCommand command_checkAddress, command_countryID, command_addLocation;

            int cityID = getCityID();

            string query_countryID = "SELECT CI_CO_ID FROM CITY WHERE CI_ID = " + cityID + " ;";
            int countryID;

            command_countryID = new SqlCommand(query_countryID, dbConnectionSQLServer.cn);
            countryID = (Int32)command_countryID.ExecuteScalar();

            string query_checkAddress = "SELECT dbo.fnCheckAdress('" + addressForm.Value.ToString() + "', (SELECT LOC_ID FROM LOCALITY WHERE LOC_ADDRESS = '" + addressForm.Value.ToString().Replace("'", "''") + "' AND LOC_CI_ID = " + cityID + "))";
            int status_addressCheck = 1;

            command_checkAddress = new SqlCommand(query_checkAddress, dbConnectionSQLServer.cn);
            status_addressCheck = (Int32)command_checkAddress.ExecuteScalar();

            //Response.Write("\n<p>status_addressCheck= " + status_addressCheck + "</p>\n");

            if (status_addressCheck == 0)
            {
                string query_addLocation = "INSERT INTO LOCALITY VALUES('Name of location', " + cityID + ", " + countryID + ", '" + addressForm.Value.ToString() + "');";

                command_addLocation = new SqlCommand(query_addLocation, dbConnectionSQLServer.cn);

                command_addLocation.ExecuteNonQuery();
                command_addLocation.Dispose();
            }
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: in addLocation() " + ex.Message + "</p>\n");
        }
    }

    public int getCityID()
    {
        int cityID = -1;

        try
        {
            SqlCommand command_cityID;
            string query_cityID = "SELECT CI_ID FROM CITY WHERE CI_NAME = '" + birthPlaceForm.SelectedItem.Text + "' ;";

            command_cityID = new SqlCommand(query_cityID, dbConnectionSQLServer.cn);
            cityID = (Int32)command_cityID.ExecuteScalar();
            command_cityID.Dispose();
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: in getCityID() " + ex.Message + "</p>\n");
        }

        return cityID;
    }
}