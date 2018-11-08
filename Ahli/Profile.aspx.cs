using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Profile : System.Web.UI.Page
{

    DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();
    public string u_id_uri ="";

    protected void Page_Load(object sender, EventArgs e)
    {
        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");

        string id = "";
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        //Response.Write(myUri.ToString());
        u_id_uri = HttpUtility.ParseQueryString(myUri.Query).Get("id");

        // This is a div:
        editBio.Visible = false;

        // This is a div:
        editCity.Visible = false;


        Biography.Visible = true;
/*
        Notifications.Visible = false;        
        Created.Visible = false;
        Attended.Visible = false;
        Upcoming.Visible = false;
*/
        // This is a div:
        editpreferences.Visible = false;
        preferences_list.Visible = true;

         Created.InnerHtml = "";
         Attended.InnerHtml = "";
         Upcoming.InnerHtml = "";
         Notifications.InnerHtml = "";

        if (!string.IsNullOrEmpty(u_id_uri))
        {
            //Response.Write("HERE!");
            id = u_id_uri;
            edit_aboutme.Visible = false;
            edit_location.Visible = false;
            edit_preferences.Visible = false;

            if (Session["id"] != null)
            {
                if (Session["id"].ToString() != u_id_uri)
                {
                    profilePictureText.Visible = false;
                    Biography.InnerHtml = "You can't view this information.";
                    profilePictureText.Visible = false;
                    Notifications.InnerHtml = "You can't view this information.";
                }
            }
            else
            {
                profilePictureText.Visible = false;
                Biography.InnerHtml = "You can't view this information.";
                profilePictureText.Visible = false;
                Notifications.InnerHtml = "You can't view this information.";
            }
        }
        else
        {
            if (Session["id"] != null)
            {
                id = Session["id"].ToString();
            }
            else
                Response.Redirect("~/");

        }
            

            string name ="";
            string username ="";
            //get the username
            try{
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                string sql = "SELECT P_USERNAME FROM PERSON WHERE P_ID = " + id + ";";
                SqlCommand command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                username = (String)command.ExecuteScalar();

                if(File.Exists(absolutePath + "people/" + username + ".jpg"))
                profile_pic.Attributes.Add("src", "~/images/people/" + username + ".jpg");
                else
                profile_pic.Attributes.Add("src", "~/images/people/square-profile-generic.jpg");

                ProfilePicture.Attributes.Add("href", "~/JCropImage?username=" + username);                
                profilePictureHref.Attributes.Add("href", "~/JCropImage?username=" + username);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: " + ex.Message + "</p>^n");
            }

        if (!string.IsNullOrEmpty(id))
        {
            string sqlUserTypecheckQuery = "SELECT dbo.fnGetUserType(" + id + ");";
            // INDIVIDUAL SQL QUERIES BELOW:
            string sqlAgeQuery = "SELECT FLOOR((CAST (GetDate() AS INTEGER) - CONVERT(INT, CONVERT(DATETIME,P_BIRTHDAY))) / 365.25) AS Age  FROM PERSON WHERE P_ID = " + id + ";";            

            //ADD THE ACTIVITIES QUERIES
            // ORGANIZATION SQL QUERIES BELOW: COMING UP ...
            SqlCommand command_UserAge;
            int UserAge;
            //Try for NameCommand
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                NameContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: " + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: " + ex.Message + "</p>^n");
            }
            //-----------------------------
            //Try for the aboutMe paragraph
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                AboutMeContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AboutMe)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (AboutMe)" + ex.Message + "</p>\n");
            }
            //-----------------------------
            //Try for the UserAge paragraph
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }                

                //Age Command
                command_UserAge = new SqlCommand(sqlAgeQuery, dbConnectionSQLServer.cn);
                UserAge = Convert.ToInt32(command_UserAge.ExecuteScalar());                                

                command_UserAge.Dispose();
                dbConnectionSQLServer.cn.Close();
                birthdayLabel.InnerText = UserAge.ToString();
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (UserAge)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (UserAge)" + ex.Message + "</p>\n");
            }
            //----------------------------------
            //Try for the UserLocation paragraph
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                LocationContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (UserLocation)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (UserLocation)" + ex.Message + "</p>\n");
            }
            //----------------------------
            //Try for the Preferences list
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                PreferencesContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (Preferences)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (Preferences)" + ex.Message + "</p>\n");
            }
            //TRY FOR THE CITYLIST DROPDOWN
            if (CityList.Items.Count == 0)
            {
                try
                {
                    if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }
                    CityContent(id);
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number: (citiesList)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR: (citiesList)" + ex.Message + "</p>\n");
                }
            }
            //Try for the list of all created events
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                CreatedContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (createdEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (CreatedEvents)" + ex.Message + "</p>\n");
            }
            //Try for the list of all attended events
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                AttendedContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            //TRy for the upcomingEVents
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                UpcomingContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (UpcomingEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (UpcomingEvents)" + ex.Message + "</p>\n");
            }
            //Try for the notifications
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                NotificationContent(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (UpcomingEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (UpcomingEvents)" + ex.Message + "</p>\n");
            }
            //Try for the NUMBER of attended events
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                CreatedNumber(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
            }
            //Try for the attended number
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                AttendedNumber(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
            }
            //Try for the NUMBER of upcoming events
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                UpcomingNumber(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
            }
            //Try for the NUMBER of notifications
            try
            {
                if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }
                NotificationsNumber(id);
            }
            catch (SqlException sqlex)
            {
                Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
            }
            catch (Exception ex)
            {
                Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
            }
            
            if (!IsPostBack)
            {
                //Try for the listBox SubCategories
                try
                {
                    if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }
                    PopulateListCategories();
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
                }
                //Try for the listBox Preferences
                try
                {
                    if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }
                    PopulateListPreferences(id);
                }
                catch (SqlException sqlex)
                {
                    Response.Write("\n<p>SQL Error Number: (AttendedEvents)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
                }
                catch (Exception ex)
                {
                    Response.Write("\n<p>ERROR: (AttendedEvents)" + ex.Message + "</p>\n");
                }
            }
        }
    }
    public void PopulateListCategories()
    {
        string query_subcategories = "SELECT SUB_CAT_NAME, SUB_CAT_ID FROM SUB_CAT;";
        SqlCommand cmd_subcategories = new SqlCommand(query_subcategories,dbConnectionSQLServer.cn);
        using (SqlDataReader reader = cmd_subcategories.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    listBoxSubCategories.Items.Add(new ListItem(reader.GetString(0), reader.GetInt32(1).ToString()));

                    //if (reader.GetString(1) != null){
                    //    HtmlGenericControl i = new HtmlGenericControl("i");
                    //    i.Attributes.Add("class", reader.GetString(1));
                    //    i.Attributes.Add("aria-hidden", "true");
                    //    li.Controls.Add(i);
                    //}
                } while (reader.Read());
            }
        }
    }
    public void PopulateListPreferences(string id)
    {
        string query_preferences = "SELECT SC.SUB_CAT_NAME, P.PREF_SUB_CAT_ID FROM SUB_CAT AS SC JOIN PREFERENCE AS P ON SC.SUB_CAT_ID = P.PREF_SUB_CAT_ID JOIN PERSON AS PE ON P.PREF_P_ID = PE.P_ID WHERE PE.P_ID = " + id + ";";
        SqlCommand cmd_preferences = new SqlCommand(query_preferences, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = cmd_preferences.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    listBoxPreferences.Items.Add(new ListItem(reader.GetString(0), reader.GetInt32(1).ToString()));

                    //if (reader.GetString(1) != null){
                    //    HtmlGenericControl i = new HtmlGenericControl("i");
                    //    i.Attributes.Add("class", reader.GetString(1));
                    //    i.Attributes.Add("aria-hidden", "true");
                    //    li.Controls.Add(i);
                    //}
                } while (reader.Read());
            }
        }
    }

    protected void onClick_Biography(object sender, EventArgs e)
    {
        Biography.Focus();
        Biography.Visible = true;
        Notifications.Visible = false;
        Created.Visible = false;
        Attended.Visible = false;
        Upcoming.Visible = false;
    }
    protected void onClick_EventsCreated(object sender, EventArgs e)
    {
        Biography.Visible = false;
        Notifications.Visible = false;
        Created.Visible = true;
        Attended.Visible = false;
        Upcoming.Visible = false;
    }
    protected void onClick_EventsAttended(object sender, EventArgs e)
    {
        //Attended.Focus();
        Biography.Visible = false;
        Notifications.Visible = false;
        Created.Visible = false;
        Attended.Visible = true;
        Upcoming.Visible = false;
    }
    protected void onClick_UpcomingEvents(object sender, EventArgs e)
    {
        //Upcoming.Focus();
        Biography.Visible = false;
        Notifications.Visible = false;
        Created.Visible = false;
        Attended.Visible = false;
        Upcoming.Visible = true;
    }
    protected void onCLick_aboutmeEdit(object sender, EventArgs e)
    {
        // This is a div:
        editBio.Visible = true;

        aboutme_edit.Text = AboutMeLabel.InnerText;
        AboutMeLabel.Visible = false;
    }
    protected void onClick_locationEdit(object sender, EventArgs e)
    {
        // This is a div:
        editCity.Visible = true;
        LocationLabel.Visible = false;
    }
    protected void onClick_SubmitAboutmeChanges(object sender, EventArgs e)
    {
        string id = Session["id"].ToString();
        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }
        string query_editprofile = "UPDATE PER_PROFILE SET PROF_BIOGRAPHY = '" + aboutme_edit.Text.ToString() + "' WHERE PROF_P_ID = " + id + ";";
        try
        {
            SqlCommand command_editprofile = new SqlCommand(query_editprofile, dbConnectionSQLServer.cn);
            command_editprofile.ExecuteNonQuery();
            AboutMeContent(id);
            AboutMeLabel.Visible = true;
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number: (editBio)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: (editBio)" + ex.Message + "</p>\n");
        }

    }

    protected void onClick_CancelAboutmeChanges(object sender, EventArgs e)
    {
        aboutme_edit.Text = null;
        aboutme_edit.Visible = false;
        AboutMeLabel.Visible = true;
    }

    protected void onClick_CancelLocationChanges(object sender, EventArgs e)
    {        
        CityList.Visible = false;
        LocationLabel.Visible = true;
        location_edit_submit.Visible = false;
        location_edit_cancel.Visible = false;        
    }
    protected void OnClick_CancelPreference(object sender, EventArgs e)
    {
        preferences_list.Visible = true;
        editpreferences.Visible = false;
    }
    
    protected void onClick_SubmitLocationChanges(object sender, EventArgs e)
    {
        string id = Session["id"].ToString();
        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        int cityID = getCityID();

        string location_change_query = "UPDATE PERSON SET P_CI_ID = " + cityID + " WHERE P_ID = " + id + ";";
        SqlCommand command_updateLocation;
        try
        {
            command_updateLocation = new SqlCommand(location_change_query, dbConnectionSQLServer.cn);
            command_updateLocation.ExecuteNonQuery();
            LocationContent(id);
            LocationLabel.Visible = true;
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number: (editLocation)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: (editLocation)" + ex.Message + "</p>\n");
        }
    }

    protected void LocationContent(string id)
    {
        SqlCommand command_UserLocation;
        string sqlLocationQuery = "SELECT CONCAT(CI.CI_NAME, ', ', CO.CO_NAME) FROM CITY AS CI JOIN COUNTRY AS CO ON CI.CI_CO_ID = CO.CO_ID JOIN PERSON AS PE ON CI.CI_ID = PE.P_CI_ID WHERE PE.P_ID = " + id + ";";

        String UserLocation;

        //Location Command
        command_UserLocation = new SqlCommand(sqlLocationQuery, dbConnectionSQLServer.cn);
        UserLocation = (String)command_UserLocation.ExecuteScalar();

        command_UserLocation.Dispose();
        dbConnectionSQLServer.cn.Close();
        LocationLabel.InnerText = UserLocation;
    }

    protected void NameContent(string id)
    {
        SqlCommand command_Name;
        string sqlName = "SELECT P_USERNAME FROM PERSON WHERE P_ID = " + id + ";";
        String Name;

        //Name Command
        command_Name = new SqlCommand(sqlName, dbConnectionSQLServer.cn);
        Name = (String)command_Name.ExecuteScalar();
        //Do this at the end!
        command_Name.Dispose();
        dbConnectionSQLServer.cn.Close();
        //ASSIGN VALUES
        NameLabel.Text = Name;
    }

    protected void AboutMeContent(string id)
    {
        if(Session["id"] != null)
        {
            SqlCommand command_AboutMe;
            string sqlAboutMeQuery = "SELECT PROF_BIOGRAPHY FROM PER_PROFILE WHERE PROF_P_ID = " + id + ";";
            String AboutMe;

            command_AboutMe = new SqlCommand(sqlAboutMeQuery, dbConnectionSQLServer.cn);
            var aboutme_db = command_AboutMe.ExecuteScalar();
            if (aboutme_db != DBNull.Value)
            {
                AboutMe = (String)command_AboutMe.ExecuteScalar();
                command_AboutMe.Dispose();
                dbConnectionSQLServer.cn.Close();
                AboutMeLabel.InnerText = AboutMe;
            }
        }
        else
        {
            AboutMeLabel.InnerText = "wuut";
        }
    }

    protected void PreferencesContent(string id)
    {
        SqlCommand command_Preferences;
        string sqlPreferences = "SELECT SUB_CAT_NAME FROM SUB_CAT AS S JOIN PREFERENCE AS P ON S.SUB_CAT_ID = P.PREF_SUB_CAT_ID WHERE PREF_P_ID = " + id + ";";

        //Preferences Command
        command_Preferences = new SqlCommand(sqlPreferences, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_Preferences.ExecuteReader())
        {
            if (reader.Read())
            {
                preferencesMessage.Visible = false;
                do
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    preferences_list.Controls.Add(li);

                    //if (reader.GetString(1) != null){
                    //    HtmlGenericControl i = new HtmlGenericControl("i");
                    //    i.Attributes.Add("class", reader.GetString(1));
                    //    i.Attributes.Add("aria-hidden", "true");
                    //    li.Controls.Add(i);
                    //}

                    li.InnerText = reader.GetString(0);
                } while (reader.Read());
            }
        }

        command_Preferences.Dispose();
        dbConnectionSQLServer.cn.Close();
    }

    protected void CityContent(string id)
    {
        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        string query_Cities = "SELECT CI.CI_NAME FROM CITY AS CI JOIN COUNTRY AS CO ON CI.CI_CO_ID = CO.CO_ID;";
        SqlCommand command_cities;
        command_cities = new SqlCommand(query_Cities, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_cities.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    CityList.Items.Add(new ListItem(reader.GetString(0)));
                } while (reader.Read());
            }
        }
        command_cities.Dispose();        
    }

    protected void CreatedContent(string id)
    {
        SqlCommand command_CreatedEvents;
        string sqlCreatedEvents = "SELECT A_NAME, A_DESCRIPTION, A_START_TIME, A_END_TIME, A.A_ID FROM ACTIVITY AS A JOIN SUBSCRIBE AS S ON A.A_ID = S.SUB_A_ID WHERE SUB_TYPE = 'CREATOR' AND S.SUB_P_ID = " + id + ";";
        command_CreatedEvents = new SqlCommand(sqlCreatedEvents, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_CreatedEvents.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    Created.InnerHtml += "\n<hr />";
                    string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader.GetString(0).ToLower());
                    string description = reader.GetString(1);
                    //string start = DateTime.ParseExact(reader.GetDateTime(2).ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("f");
                    //start = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLower());
                    DateTime start = reader.GetDateTime(2);
                    string start_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLongDateString().ToLower());
                    string start_time = start.ToShortTimeString();
                    DateTime end = reader.GetDateTime(3);
                    string end_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(end.ToLongDateString().ToLower());
                    string end_time = end.ToShortTimeString();
                    string activity_id = reader.GetInt32(4).ToString();
                    //path = path.Replace('\\','/');

                    Created.InnerHtml += "\n<div class=\"created_event row\">" +
                                            //"\n<div class=\"col-md-4\">" +
                                            //"\n<img src=\"" + path + "\"/>" +
                                            //"\n</div>" +
                                            //"\n<div class=\"col-md-8\">" +
                                            "\n<a href=\"Activity_Locations.aspx?a_id=" + activity_id + "\" class=\"activity_name\">" + name + "</a>" +
                                            "\n<p>" + description + "</p>" +
                                            "\n<p>From <span class=\"date\">" + start_date + "</span> at <span class=\"date\">" + start_time + "</span><br /> to <span class=\"date\">" + end_date + "</span> at <span class=\"date\">" + end_time + "</span></p>" +
                                            //"\n<button class=\"btn btn-default\" id=\"about_activity_btn\" runat=\"server\" onserverclick=\"onClick_AboutActivity\">Read More</button>" +
                                            //"\n</div>" +
                                            "\n</div>" +
                                            //"\n<hr />" +
                                            "<style type=\"text/css\">.created_event{margin-top: 40px;margin-bottom: 42px;}.date{font-style:italic}</style>";

                } while (reader.Read());
            }
            else
            {
                //Response.Write("NoTHIIING in created");
                Created.InnerHtml += "\n<h2>You have not created any activity so far.</h2>";
            }
            Created.InnerHtml += "<a href=\"CreateActivity.aspx\" class=\"btn btn-primary\">Create new activity</a>";
            command_CreatedEvents.Dispose();
            dbConnectionSQLServer.cn.Close();
        }
    }

    protected void AttendedContent(string id)
    {
        SqlCommand command_AttendedEvents;
        string sqlAttendedEvents = "SELECT A_NAME, A_DESCRIPTION, A_START_TIME, A_END_TIME, A.A_ID FROM ACTIVITY AS A JOIN SUBSCRIBE AS S ON A.A_ID = S.SUB_A_ID WHERE SUB_TYPE = 'PARTICIPANT' AND S.SUB_P_ID = " + id + " AND A.A_END_TIME < GETDATE();";
        command_AttendedEvents = new SqlCommand(sqlAttendedEvents, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_AttendedEvents.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    Attended.InnerHtml += "\n<hr />";
                    string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader.GetString(0).ToLower());
                    string description = reader.GetString(1);
                    //string start = DateTime.ParseExact(reader.GetDateTime(2).ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("f");
                    //start = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLower());
                    DateTime start = reader.GetDateTime(2);
                    string start_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLongDateString().ToLower());
                    string start_time = start.ToShortTimeString();
                    DateTime end = reader.GetDateTime(3);
                    string activity_id = reader.GetInt32(4).ToString();
                    string end_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(end.ToLongDateString().ToLower());
                    string end_time = end.ToShortTimeString();
                    //string path = "file:///" + reader.GetString(4);
                    //path = path.Replace('\\','/');

                    Attended.InnerHtml += "\n<div class=\"created_event row\">" +
                                            //"\n<div class=\"col-md-4\">" +
                                            //"\n<img src=\"" + "#" + "\"/>" +
                                            //"\n</div>" +
                                            //"\n<div class=\"col-md-8\">" +
                                            "\n<a href=\"ActivityLocation.aspx?a_id="+ activity_id +"\" class=\"activity_name\">" + name + "</a>" +
                                            "\n<p>" + description + "</p>" +
                                            "\n<p>From <span class=\"date\">" + start_date + "</span> at <span class=\"date\">" + start_time + "</span><br /> to <span class=\"date\">" + end_date + "</span> at <span class=\"date\">" + end_time + "</span></p>" +
                                            //"\n<button class=\"btn btn-default\" id=\"about_activity_btn\" runat=\"server\" onserverclick=\"onClick_AboutActivity\">Read More</button>" +
                                            "\n</div>" +
                                            //"\n</div>" +
                                            //"\n<hr />" +
                                            "<style type=\"text/css\">.created_event{margin-top: 40px;margin-bottom: 42px;}.date{font-style:italic}</style>";

                } while (reader.Read());
            }
            else
            {
                //Response.Write("NoTHIIING in attended");
                Attended.InnerHtml += "\n<h2>You have no attended activities for now.</h2>";
            }
        }
        command_AttendedEvents.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void UpcomingContent(string id)
    {
        SqlCommand command_UpcomingEvents;
        string sqlUpcomingEVents = "SELECT A_NAME, A_DESCRIPTION, A_START_TIME, A_END_TIME, A.A_ID FROM ACTIVITY AS A JOIN SUBSCRIBE AS S ON A.A_ID = S.SUB_A_ID WHERE SUB_TYPE = 'PARTICIPANT' AND S.SUB_P_ID = " + id + " AND A.A_END_TIME > GETDATE();";
        command_UpcomingEvents = new SqlCommand(sqlUpcomingEVents, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_UpcomingEvents.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    Upcoming.InnerHtml += "\n<hr />";
                    string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader.GetString(0).ToLower());
                    string description = reader.GetString(1);
                    //string start = DateTime.ParseExact(reader.GetDateTime(2).ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("f");
                    //start = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLower());
                    DateTime start = reader.GetDateTime(2);
                    string start_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(start.ToLongDateString().ToLower());
                    string start_time = start.ToShortTimeString();
                    DateTime end = reader.GetDateTime(3);
                    string activity_id = reader.GetInt32(4).ToString();
                    string end_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(end.ToLongDateString().ToLower());
                    string end_time = end.ToShortTimeString();
                    //string path = "file:///" + reader.GetString(4);
                    //path = path.Replace('\\','/');

                    Upcoming.InnerHtml += "\n<div class=\"created_event row\">" +
                                            //"\n<div class=\"col-md-4\">" +
                                            //"\n<img src=\"" + "#" + "\"/>" +
                                            //"\n</div>" +
                                            //"\n<div class=\"col-md-8\">" +
                                            "\n<a href=\"ActivityLocation?a_id="+ activity_id +"\" class=\"activity_name\">" + name + "</a>" +
                                            "\n<p>" + description + "</p>" +
                                            "\n<p>From <span class=\"date\">" + start_date + "</span> at <span class=\"date\">" + start_time + "</span><br /> to <span class=\"date\">" + end_date + "</span> at <span class=\"date\">" + end_time + "</span></p>" +
                                            //"\n<button class=\"btn btn-default\" id=\"about_activity_btn\" runat=\"server\" onserverclick=\"onClick_AboutActivity\">Read More</button>" +
                                            "\n</div>" +
                                            //"\n</div>" +
                                            //"\n<hr />" +
                                            "<style type=\"text/css\">.created_event{margin-top: 40px;margin-bottom: 42px;}.date{font-style:italic}</style>";

                } while (reader.Read());
            }
            else
            {
                Upcoming.InnerHtml += "\n<h2>You have no upcoming activities for now.</h2>";
            }
        }
        command_UpcomingEvents.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void NotificationContent(string id)
    {
        SqlCommand command_Notifications;
        string sqlNotifications = "SELECT NOT_NAME, A_NAME, NOT_CONTENT, NOT_TIME FROM NOTIFICATION AS N JOIN ACTIVITY AS A ON N.NOT_A_ID = A.A_ID WHERE N.NOT_P_ID = " + id + ";";
        command_Notifications = new SqlCommand(sqlNotifications, dbConnectionSQLServer.cn);
        using (SqlDataReader reader = command_Notifications.ExecuteReader())
        {
            if (reader.Read())
            {
                do
                {
                    Notifications.InnerHtml += "\n<hr />";
                    string notification_name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader.GetString(0).ToLower());
                    string activity_name = reader.GetString(1);
                    string notification_description = reader.GetString(2);
                    DateTime notification_date_time = reader.GetDateTime(3);
                    string notification_date = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(notification_date_time.ToLongDateString().ToLower());
                    string notification_time = notification_date_time.ToShortTimeString();

                    Notifications.InnerHtml += "\n<div class=\"notification row\">" +
                                            "\n<p class=\"notification_name\">" + notification_name + "on <span class=\"activity_name\">" + activity_name + "</span></p>" +
                                            "\n<p>" + notification_description + "</p>" +
                                            "\n<p>On <span class=\"date\">" + notification_date + "</span> at <span class=\"date\">" + notification_time + "</span>" +
                                            "\n</div>" +
                                            //"\n<hr />" +
                                            "<style type=\"text/css\">.notification{margin-top: 40px;margin-bottom: 42px;}.notification_name{font-weight: bold;font-size: 22px;letter-spacing: 0.5px;}.activity_name{font-weight: none;font-size: 12px;}.date{font-style:italic}</style>";
                } while (reader.Read());
            }
            else
            {
                Notifications.InnerHtml += "\n<h2>You have no notifications for now.</p>";
            }
            command_Notifications.Dispose();
            dbConnectionSQLServer.cn.Close();
        }
    }
    protected void AttendedNumber(string id)
    {
        SqlCommand command_AttendedNumber;
        string sqlAttendedNumber = "SELECT dbo.fnGetAttendedCount(" + id + ");";
        command_AttendedNumber = new SqlCommand(sqlAttendedNumber, dbConnectionSQLServer.cn);

        int attendedNumber = (Int32)command_AttendedNumber.ExecuteScalar();
        eventsAttended_label.InnerText = "Activities Attended (" + attendedNumber + ")";

        command_AttendedNumber.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void CreatedNumber(string id)
    {
        SqlCommand command_CreatedNumber;
        string sqlCreatedNumber = "SELECT dbo.fnGetCreatedNumber(" + id + ");";
        command_CreatedNumber = new SqlCommand(sqlCreatedNumber, dbConnectionSQLServer.cn);

        int createdNumber = (Int32)command_CreatedNumber.ExecuteScalar();
        eventsCreated_label.InnerText = "Activities Created (" + createdNumber + ")";

        command_CreatedNumber.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void UpcomingNumber(string id)
    {
        SqlCommand command_UpcomingNumber;
        string sqlUpcomingNumber = "SELECT dbo.fnGetUpcomingNumber(" + id + ");";
        command_UpcomingNumber = new SqlCommand(sqlUpcomingNumber, dbConnectionSQLServer.cn);

        int upcomingNumber = (Int32)command_UpcomingNumber.ExecuteScalar();
        eventsUpcoming_label.InnerText = "Upcoming Activities (" + upcomingNumber + ")";

        command_UpcomingNumber.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void NotificationsNumber(string id)
    {
        SqlCommand command_NotificationsNumber;
        string sqlNitificationsNumber = "SELECT dbo.fnGetNotificationCount(" + id + ");";
        command_NotificationsNumber = new SqlCommand(sqlNitificationsNumber, dbConnectionSQLServer.cn);

        int notificationNumber = (Int32)command_NotificationsNumber.ExecuteScalar();
        notifications_label.InnerText = "Notifications (" + notificationNumber + ")";

        command_NotificationsNumber.Dispose();
        dbConnectionSQLServer.cn.Close();
    }
    protected void onClick_Notifications(object sender, EventArgs e)
    {
        //Notifications.Focus();
        Biography.Visible = false;
        Notifications.Visible = true;
        Created.Visible = false;
        Attended.Visible = false;
        Upcoming.Visible = false;
    }

    protected void onClick_AddPreference(object sender, EventArgs e)
    {
        string id = Session["id"].ToString();
        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }
        string subcategory_id;
        ListItem subcat = listBoxSubCategories.SelectedItem;
        string subcat_name = listBoxSubCategories.SelectedItem.ToString();
        subcategory_id = listBoxSubCategories.SelectedValue.ToString();
        string query_addpreference = "INSERT INTO PREFERENCE VALUES (" + id + "," + subcategory_id + ");";
        try
        {
            SqlCommand cmd_addpreference = new SqlCommand(query_addpreference, dbConnectionSQLServer.cn);
            cmd_addpreference.ExecuteNonQuery();
            listBoxPreferences.Items.Add(subcat);
            HtmlGenericControl li = new HtmlGenericControl("li");
            preferences_list.Controls.Add(li);
            li.InnerText = subcat_name;
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number: (editLocation)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: (editLocation)" + ex.Message + "</p>\n");
        }
    }

    protected void OnClick_RemovePreference(object sender, EventArgs e)
    {
        string id = Session["id"].ToString();
        if (dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }
        string subcategory_id;
        ListItem subcat = listBoxPreferences.SelectedItem;
        subcategory_id = listBoxPreferences.SelectedValue.ToString();
        string query_addpreference = "DELETE FROM PREFERENCE WHERE PREF_P_ID IN ("+id+") AND PREF_SUB_CAT_ID IN("+subcategory_id+");";
        try
        {
            SqlCommand cmd_addpreference = new SqlCommand(query_addpreference, dbConnectionSQLServer.cn);
            cmd_addpreference.ExecuteNonQuery();
            listBoxPreferences.Items.Remove(subcat);
            
        }
        catch (SqlException sqlex)
        {
            Response.Write("\n<p>SQL Error Number: (editLocation)" + sqlex.Number + " -- SQL Error: " + sqlex.Message + "</p>\n");
        }
        catch (Exception ex)
        {
            Response.Write("\n<p>ERROR: (editLocation)" + ex.Message + "</p>\n");
        }
    }
    protected void onCLick_preferencesEdit(object sender, EventArgs e)
    {
        editpreferences.Visible = true;
        preferences_list.Visible = false;
    }

    public int getCityID()
    {
        SqlCommand command_cityID;
        string query_cityID = "SELECT CI_ID FROM CITY WHERE CI_NAME = '" + CityList.SelectedItem.Text + "' ;";
        int cityID = -1;

        command_cityID = new SqlCommand(query_cityID, dbConnectionSQLServer.cn);
        cityID = (Int32)command_cityID.ExecuteScalar();

        return cityID;
    }
}