using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddRelatedPerson : System.Web.UI.Page
{
    DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();
    bool allGood = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["id"] != null)
        {
            string id = Session["id"].ToString();

            SqlDataReader dataReader;
            SqlCommand command_children;

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }

                string query_Children = "SELECT CONCAT(CH_LNAME, ' ' + CH_FNAME), CH_ID FROM CHILD WHERE CH_LEFT_CENTER_DATE IS NULL;";
                string childName = "";
                int kidID = -1;

                command_children = new SqlCommand(query_Children, dbConnectionSQLServer.cn);
                dataReader = command_children.ExecuteReader();

                for (int i = 0; dataReader.Read(); i++)
                {
                    // reader.GetSqlInt32(1).ToString()

                    // if (i == 0)
                    //    birthPlaceForm.Items.Add(new ListItem("- Birth City -", "0"));                    

                    childName = dataReader.GetValue(0).ToString();
                    kidID = (int) dataReader.GetValue(1);

                    childList.InnerHtml += "<div data-value=\"" + kidID + "\">" + childName  + "</div>";

                    if(i == 0)
                    childList.Attributes.Add("data-value", kidID.ToString());
                }

                dataReader.Close();
                command_children.Dispose();
                dbConnectionSQLServer.cn.Close();
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number:(children dropdown) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR:(children dropdown) " + ex.Message + "</p>\n");
            }            
        }
        else
        {
            Response.Redirect("~/Login");
        }
    }

    protected void pickedChild_Submit_Click(object sender, EventArgs e)
    {

        relatedChilContainer.Visible = false;
        relatedPersonContainer.Visible = true;

        int childID = Convert.ToInt32(Request.Form["pickedChild"]);

        hiddenChildIDInput.Value = childID.ToString();

        //Response.Write("childID in pickedChild_Submit_Click is: " + childID + "<br />");

        whichChild.InnerText = "Add a related person to the child " + getChildName(childID.ToString()); ;

    }

    public string getChildName(string childID)
    {

        if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        SqlCommand command_childName;
        string query_childName = "SELECT CONCAT(CH_LNAME, ' ' + CH_FNAME) FROM CHILD WHERE CH_ID = " + childID + ";";
        string childName = "";

        try
        {
            command_childName = new SqlCommand(query_childName, dbConnectionSQLServer.cn);
            childName = (string) command_childName.ExecuteScalar();

            using (SqlDataReader reader = command_childName.ExecuteReader())
            {
                if (reader.Read())
                {
                    do
                    {
                        childName = reader.GetString(0);

                    } while (reader.Read());
                }

                reader.Close();
            }
                command_childName.Dispose();
                dbConnectionSQLServer.cn.Close();            
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number:(getChildName) " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR:(getChildName) " + ex.Message + "</p>\n");
        }

        return childName;
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        if (allGood)
        {
            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }

            if (Session["id"] != null)
            {
                try
                {
                    int childID = Convert.ToInt32(hiddenChildIDInput.Value.ToString());
                    string relationshipType = relationshipTypeForm.Value.ToString().Replace("'", "''");
                    string firstName = firstNameForm.Value.ToString().Replace("'", "''");
                    string lastName = lastNameForm.Value.ToString().Replace("'", "''");
                    string gender = genderForm.SelectedItem.Text.Replace("'", "''");
                    string profession = professionForm.Value.ToString().Replace("'", "''");
                    string address = addressForm.Value.ToString().Replace("'", "''");
                    string birthday = Request.Form[birthdayHiddenInput.UniqueID].Replace("'", "''");
                    string educationLevel = educationLevelForm.Value.ToString().Replace("'", "''");
                    string marriedTo = marriedToForm.Value.ToString().Replace("'", "''");
                    string medicalState = medicalStateForm.Value.ToString().Replace("'", "''");
                    string phoneNumber = phoneNumberForm.Value.ToString().Replace("'", "''");
                    string extraInfo = extraInfoForm.Value.ToString().Replace("'", "''");

                    
                    string query_addChild = "INSERT INTO RELATED_PERSON VALUES (" + childID.ToString() + ", " +
                        "'" + relationshipType + "', " +
                        "'" + firstName + "', " +
                        "'" + lastName + "', " +
                        "'" + gender + "', " +
                        "'" + profession + "', " +
                        "'" + address + "', " +
                        "'" + birthday + "', " +
                        "'" + educationLevel + "', " +
                        "'" + marriedTo + "', " +
                        "'" + medicalState + "', " +
                        "'" + phoneNumber + "', " +
                        "'" + extraInfo + "');";

                    SqlCommand command_addChild;
                    command_addChild = new SqlCommand(query_addChild, dbConnectionSQLServer.cn);
                    command_addChild.ExecuteNonQuery();

                    dbConnectionSQLServer.cn.Close();

                    addRelatedPersonForm.Visible = false;
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

}