using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Activity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");        

        if (doesActiviyExist())
        {
            LoadActivities(a_id);
            loadComments(a_id);
            loadVolunteers(a_id);
            loadChildren(a_id);
        }

    }

    protected bool doesActiviyExist()
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");

        string sql = "SELECT 1 FROM ACTIVITY WHERE A_ID = " + a_id + ";";

        bool activityExists = false;

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

            if (dataReader.HasRows)
            {
                activityExists = true;
            }
            else
            {
                leftColumn.Controls.Clear();
                volunteersAttendance.Controls.Clear();
                activityDescriptionContainer.Controls.Clear();
                commentsBox.Controls.Clear();

                doesntExist.Visible = true;
            }

            dataReader.Close();
            command.Dispose();
            dbConnectionSQLServer.cn.Close();            
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message + " in doesActiviyExist().</br>");
        }

        return activityExists;
    }

    protected void LoadActivities(string a_id)
    {        
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        // Variable needed by the File class.
        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");

        // Flag variable to check how many rows have been used in the loop.
        int i = 0;

        // Variables used in the html building.
        string activityName = "Dummy activity name";
        string activityCityName = "Dummy activity's location name";
        string activityDescription = "Dummy activity description";
        string activityLocationName = "Dummy whereabouts text";
        string activityAddress = "Dummy whereabouts text";
        string activitySubcategory = "Dummy activity subcategory";
        string activityCategory = "Dummy activity category";
        string activityImagePath = "Dummy activity image path";

        string organiserName = "Dummy organiser name";
        string organiserBio = "Dummy organiser bio";
        string organiserUsername = "Dummy organiser username";

        DateTime activityStartTime;
        DateTime activityEndTime;

        int neededPeople = 0;
        int availableSpots = 0;
        int activityID = 0;
        int u_id = 0;

        bool hasSub = false;

        if (Session["id"] != null)
        hasSub = hasSubscribed(a_id);

        // String variables to store the SQL queries.
        string sql;


        //if(isCurrentUserOwner(a_id) == true)
        //Response.Write("isCurrentUserOwner is: true <br />");
        //else
        //Response.Write("isCurrentUserOwner is: false <br />");

        sql = "SELECT A_NAME, CI.CI_NAME, LOC_NAME, LOC_ADDRESS, A_DESCRIPTION, P_FNAME, P_LNAME, PROF_BIOGRAPHY, A_START_TIME, A_END_TIME, A_CAPACITY, A_AVAILABILITY, P_USERNAME, SUB_CAT_NAME, CAT_NAME, A.A_ID, S.SUB_P_ID FROM ACTIVITY AS A JOIN LOCALITY AS L ON A.A_LOC_ID = L.LOC_ID JOIN CITY AS CI ON L.LOC_CI_ID = CI.CI_ID JOIN SUBSCRIBE AS S ON A.A_ID = S.SUB_A_ID JOIN PERSON AS PE ON S.SUB_P_ID = PE.P_ID JOIN PER_PROFILE AS PROF ON PE.P_ID = PROF.PROF_P_ID JOIN SUB_CAT AS SC ON A.A_SUB_CAT_ID = SC.SUB_CAT_ID JOIN CATEGORY AS C ON SC.SUB_CAT_MAIN_CAT_ID = C.CAT_ID WHERE A.A_ID = " + a_id + " AND S.SUB_TYPE = 'CREATOR';";

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }                                               

            SqlCommand command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {

                activityName = (string)dataReader.GetValue(0);
                activityCityName = (string)dataReader.GetValue(1);
                activityLocationName = (string)dataReader.GetValue(2);
                activityAddress = (string)dataReader.GetValue(3);
                activityDescription = (string)dataReader.GetValue(4);
                organiserName = String.Concat((string)dataReader.GetValue(5), " ", (string)dataReader.GetValue(6));

                // Check if dataReader.GetValue(6) is null or not.
                if (!dataReader.IsDBNull(7))
                    organiserBio = truncate((string)dataReader.GetValue(7), 250);

                activityStartTime = (DateTime)dataReader.GetValue(8);
                activityEndTime = (DateTime)dataReader.GetValue(9);
                neededPeople = (int)dataReader.GetValue(10);
                availableSpots = (int)dataReader.GetValue(11);
                organiserUsername = (string)dataReader.GetValue(12);
                activitySubcategory = (string)dataReader.GetValue(13);
                activityCategory = (string)dataReader.GetValue(14);
                activityID = (int)dataReader.GetValue(15);
                u_id = (int)dataReader.GetValue(16);
                // Begin html generation.

                triangleDate.InnerHtml = String.Format("{0:dd}", activityStartTime) + "<br><small>" + String.Format("{0:MMM}", activityStartTime) + "</small>";


                HtmlGenericControl imgOrganiser = new HtmlGenericControl("img");


                imgOrganiser.Attributes.Add("src", getUniquePath("people", organiserUsername));

                imgOrganiser.Attributes.Add("style", "width:100%;");
                imgOrganiser.Attributes.Add("alt", "Avatar");

                imgContainer.Controls.Add(imgOrganiser);

                HtmlGenericControl h2OrganiserName = new HtmlGenericControl("h2");
                h2OrganiserName.InnerText = organiserName;

                organiserNameContainer.Controls.Add(h2OrganiserName);

                HtmlGenericControl pAbout = new HtmlGenericControl("p");

                if (organiserBio != null)
                    pAbout.InnerText = organiserBio;
                else
                    pAbout.InnerText = (string) dataReader.GetValue(3) + " didn't write something to share.";

                aboutContainer.Controls.Add(pAbout);

                if (Session["id"] != null)
                {
                    addActivityDiv.Visible = true;
                    
                    if ((int)Session["id"] != u_id)
                    {
                        if (!hasSub)
                        {
                            addToUserActivityButton.Visible = true;
                            removeFromUserActivityButton.Visible = false;
                        }
                        else
                        {
                            addToUserActivityButton.Visible = false;
                            removeFromUserActivityButton.Visible = true;
                        }
                    }
                    else
                    {

                        leftColumn.InnerHtml = "<p>You're the event organiser.</p><hr />";

                        deleteActivityFromDBDiv.Visible = true;

                    }
                }

                activityTitleContainer.InnerText = activityName;

                if (File.Exists(absolutePath + "activities/" + "event_" + a_id + ".jpg"))
                    activityImagePath = "images/activities/" + "event_" + activityID + ".jpg" + "?ver=" + File.GetLastWriteTime(Server.MapPath("/images/")).ToFileTime();
                else
                    activityImagePath = "images/activities/defaultEvent.png";

                HtmlGenericControl imgActivity = new HtmlGenericControl("img");
                imgActivity.Attributes.Add("src", activityImagePath + "?ver=" + File.GetLastWriteTime(absolutePath).ToFileTime());
                imgActivity.Attributes.Add("style", "max-width: 100%;");
                imgActivity.Attributes.Add("alt", "Activity");

                activityImageContainer.Controls.Add(imgActivity);

                if (Session["id"] != null && (int)Session["id"] == u_id)
                {
                    imgActivity.Attributes.Add("class", "editable");

                    HtmlGenericControl imgEditIcon = new HtmlGenericControl("img");
                    imgEditIcon.Attributes.Add("src", "images/icons/gallery-edit-32.png");
                    imgEditIcon.Attributes.Add("class", "edit");

                    HtmlGenericControl aimgEditIcon = new HtmlGenericControl("a");
                    aimgEditIcon.Attributes.Add("href", "JCropImage.aspx?username=" + organiserUsername + "&a_id=" + a_id);
                    aimgEditIcon.Controls.Add(imgEditIcon);

                    activityImageContainer.Controls.Add(aimgEditIcon);

                }
                else
                    imgActivity.Attributes.Add("class", "notEditable");

                imgCityContainer.InnerHtml = "<div class=\"thumbnail\">" +
                      "<img src=\"images/cities/" + activityCityName.ToLower() + ".png\" alt=\"Lights\" style=\"width:100%\">" +
                      "<div class=\"caption\">" +
                      "<p>" + activityCityName + "</p>" +
                      "</div>" +
                      "</div>";
                
                // style=\"margin-left:22px\" 

                dateContainer.InnerHtml = "<h3><i class=\"fa fa-calendar fa-fw w3-margin-right\" style=\"display: inline-block; width: 5%; margin-right:22px\"></i>" + FirstCharToUpper(String.Format("{0:MMMM dd, yyyy HH:mm}", activityStartTime)) + "</h3>";
                dateContainer2.InnerHtml = "<h3><i class=\"fa fa-calendar fa-fw w3-margin-right\" style=\"display: inline-block; width: 5%; margin-right:22px\"></i>" + FirstCharToUpper(String.Format("{0:MMMM dd, yyyy HH:mm}", activityEndTime)) + "</h3>";

                HtmlGenericControl PActivityDescription = new HtmlGenericControl("p");
                PActivityDescription.InnerText = activityDescription;

                activityDescriptionContainer.Controls.Add(PActivityDescription);

                activityLocationNameContainer.InnerText = activityLocationName;

                
                addressContainer.InnerText = activityAddress;

                subcategoryContainer.InnerHtml = "<a href=\"#\"><i class=\"fa fa-tags\"></i>" + FirstCharToUpper(activitySubcategory.ToLower()) + "</a>";

                categoryContainer.InnerHtml = "<a href=\"#\"><i class=\"fa fa-tag\"></i>" + FirstCharToUpper(activityCategory.ToLower()) + "</a>";                

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

    protected string getCategories(int id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        if (Session["id"] != null)
        {

            string subCatName = "Dummy subcategory name";
            string concatenatedCategories = "";


            string sql = "SELECT SUB_CAT_NAME FROM PREFERENCE AS P JOIN SUB_CAT AS SC ON P.SUB_CAT_ID = SC.SUB_CAT_ID WHERE P_ID = " + id + ";";

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
                    concatenatedCategories = concatenatedCategories + FirstCharToUpper(subCatName.ToLower()) + " - ";
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

            return concatenatedCategories.TrimEnd(charsToTrim);

        }
        else
        {
            //Response.Write("Session[\"id\"] is null.</br>");
            return "Session[\"id\"] is null.</br>";
        }
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


    protected void loadComments(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

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

            sql = "SELECT P.P_USERNAME, P.P_FNAME, P.P_LNAME, P.P_GENDER FROM PERSON AS P WHERE P.P_ID = " + Session["id"] + ";";

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

                if (!dataReader.HasRows)
                {
                    commentsContainer.InnerHtml = "<hr /><center>There are no comments to display here.<br />Be the first one to comment!</center><br /><hr />";
                }
                else
                {
                    while (dataReader.Read())
                    {
                        username = (string)dataReader.GetValue(0);
                    }
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " in loadComments() in connected user part.</br>");
            }


            HtmlGenericControl imgConnectedAvatar = new HtmlGenericControl("img");
            //imgConnectedAvatar.Attributes.Add("width", "55px");
            //imgConnectedAvatar.Attributes.Add("height", "55px");
            imgConnectedAvatar.Attributes.Add("width", "100%");        
            imgConnectedAvatar.Attributes.Add("alt", username);
            imgConnectedAvatar.Attributes.Add("id", "connectedAvatar");
            imgConnectedAvatar.Attributes.Add("class", "img-rounded");
            imgConnectedAvatar.Attributes.Add("style", "margin-bottom: 20px");



            imgConnectedAvatar.Attributes.Add("src", getUniquePath("people", username));
            

            //HtmlGenericControl txtAreaConnected = new HtmlGenericControl("textarea");
            //txtAreaConnected.Attributes.Add("rows", "10");
            //txtAreaConnected.Attributes.Add("name", "comment");
            //txtAreaConnected.Attributes.Add("id", "comment");
            //txtAreaConnected.Attributes.Add("placeholder", "What's on your mind?");

            //HtmlGenericControl inputConnected = new HtmlGenericControl("input");
            //inputConnected.Attributes.Add("type", "submit");
            //inputConnected.Attributes.Add("name", "submit");
            //inputConnected.Attributes.Add("value", "Add Comment");
            //inputConnected.Attributes.Add("id", "submitCommentButton");

            //comment_form.Controls.Add(txtAreaConnected);
            //comment_form.Controls.Add(inputConnected);

            comment_form.Controls.Add(imgConnectedAvatar);

            comment.Visible = true;
            submitCommentButton.Visible = true;
        }
        else
        {
            commentsContainer.InnerHtml = "<center>To comment please <a href=\"Login.aspx\"><b>log-in</b></a>.</center><br />";
        }


        sql = "SELECT C.A_COM_ID, C.A_COM_P_ID, C.A_COM_A_ID, C.A_COM_CONTENT, C.A_COM_TIME, PE.P_USERNAME FROM ACTIVITY_COMMENT AS C JOIN PERSON AS PE ON C.A_COM_P_ID = PE.P_ID WHERE C.A_COM_A_ID = " + a_id + " AND C.A_COM_MAIN_COMMENT_ID IS NULL ORDER BY C.A_COM_TIME DESC;";

        SqlCommand command2;
        SqlDataReader dataReader2;

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader2 = command2.ExecuteReader();

            if (!dataReader2.HasRows)
            {

                HtmlGenericControl aAvatar = new HtmlGenericControl("h5");
                aAvatar.InnerHtml = "<hr /><center>There are no comments to display here.<br />Be the first one to comment!</center><br /><hr />";
                aAvatar.Attributes.Add("id", "noCommentsToDisplay");

                commentsContainer.Controls.Add(aAvatar);
            }
            else
            {
                string imgSrc;

                List <int> commentIDs = new List<int>();
                List <int> userIDs = new List<int>();
                List <int> activityIDs = new List<int>();
                List <string> commentTexts = new List<string>();
                List <DateTime> commentDates = new List<DateTime>();
                List <string> usernames = new List<string>();

                while (dataReader2.Read())
                {
                    //concatenatedCategories = concatenatedCategories + FirstCharToUpper(subCatName.ToLower()) + " - ";
                    c_id = (int)dataReader2.GetValue(0);
                    u_id = (int)dataReader2.GetValue(1);
                    sqlA_ID = (int)dataReader2.GetValue(2);
                    commentText = (String)dataReader2.GetValue(3);
                    commentPubDate = (DateTime)dataReader2.GetValue(4);
                    username = (String)dataReader2.GetValue(5);

                    commentIDs.Add(c_id);
                    userIDs.Add(u_id);
                    activityIDs.Add(sqlA_ID);
                    commentTexts.Add(commentText);
                    commentDates.Add(commentPubDate);
                    usernames.Add(username);
                }
                    dataReader2.Close();
                    command2.Dispose();
                    dbConnectionSQLServer.cn.Close();

                int count = 0;

                for(int i = 0; i < commentIDs.Count; i++)
                {

                    imgSrc = getUniquePath("people", usernames.ToArray()[i].ToString());

                    string deleteCommentLi = "";
                    string replyCommentLi = "";

                    if (Session["id"] != null)
                    {
                        if ((int)Session["id"] == (int) userIDs.ToArray()[i])
                        {                            
                            deleteCommentLi = "<a class=\"deleteIcon\" href=\"Delete_Comment.aspx?c_id=" + commentIDs.ToArray()[i].ToString() + "&a_id=" + a_id + "\" target=\"_blank\" style=\"float: right;\"><i class=\"fa fa-remove\"></i></a>";
                        }
                        else
                            replyCommentLi = "<li><a href=\"#\"><i class=\"fa fa-reply\"></i>Reply</a></li>";
                    }

                    commentsContainer.InnerHtml += "<div id=\"comment_ID_" + commentIDs.ToArray()[i].ToString() + "\" class=\"post-comment\">" +
                    "<a class=\"pull-left\" href=\"#\">" +
                    "<img width=\"127px\" class=\"media-object\" src=\"" + imgSrc + " \" alt=\"" + usernames.ToArray()[i].ToString() + "\" >" +
                    "</a>" +
                    "<div class=\"media-body\" style=\"min-width: 180px\">" +
                    "<span>" +
                    "<i class=\"fa fa-user\"></i><a href=\"#\">" + usernames.ToArray()[i].ToString() + "</a>" +
                    deleteCommentLi +
                    "</span>" +
                    "<p>" + commentTexts.ToArray()[i].ToString() + "</p>" +
                    "<ul class=\"nav navbar-nav post-nav\">" +
                    "<li><a href=\"#\"><i class=\"fa fa-clock-o\"></i>" + FirstCharToUpper(String.Format("{0:MMMM dd, yyyy HH:mm}", (DateTime) commentDates.ToArray()[i])) + "</a></li>" +
                    replyCommentLi +                    
                    "</ul>" +
                    "</div>" +
                    "</div>" +
                    loadReplyComments(activityIDs.ToArray()[i].ToString(), Convert.ToInt32(commentIDs.ToArray()[i].ToString()));                   
                }

                count = activityTotalNumberOfComments(a_id);
                numberOfComments.InnerHtml = "<i class=\"fa fa-comments\"></i>" + count + " Comments";
            }

            dataReader2.Close();
            command2.Dispose();
            dbConnectionSQLServer.cn.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in loadComments() comments part.</br>");
        }


    }


    protected string loadReplyComments(string a_id, int commentID)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string replyCommentHtml = "";

        string commentText = "Dummy commentText";
        string username = "Dummy username";

        DateTime commentPubDate;

        int c_id = -1;
        int u_id = -1;
        int sqlA_ID = -1;
        int commentReplyToID = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");

        string sql = "SELECT C.A_COM_ID, C.A_COM_P_ID, C.A_COM_A_ID, C.A_COM_CONTENT, C.A_COM_TIME, C.A_COM_MAIN_COMMENT_ID, PE.P_USERNAME FROM ACTIVITY_COMMENT AS C JOIN PERSON AS PE ON C.A_COM_P_ID = PE.P_ID WHERE C.A_COM_A_ID = " + a_id + " AND C.A_COM_MAIN_COMMENT_ID = " + commentID + " ORDER BY C.A_COM_TIME ASC;";

        SqlCommand command3;
        SqlDataReader dataReader3;

        try
        {
            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }

            command3 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader3 = command3.ExecuteReader();

            if (dataReader3.HasRows)
            {                
                string imgSrc;

                while (dataReader3.Read())
                {
                    //concatenatedCategories = concatenatedCategories + FirstCharToUpper(subCatName.ToLower()) + " - ";
                    c_id = (int)dataReader3.GetValue(0);
                    u_id = (int)dataReader3.GetValue(1);
                    sqlA_ID = (int)dataReader3.GetValue(2);
                    commentText = (String)dataReader3.GetValue(3);
                    commentPubDate = (DateTime)dataReader3.GetValue(4);
                    commentReplyToID = Convert.ToInt32(dataReader3.GetValue(5));
                    username = (String)dataReader3.GetValue(6);
                    
                    imgSrc = getUniquePath("people", username);


                    string replyCommentLi = "";
                    string deleteCommentLi = "";

                    if (Session["id"] != null)
                    {
                        if ((int)Session["id"] == u_id)
                        {
                            deleteCommentLi = "<li><a class=\"deleteIcon\" href=\"Delete_Comment.aspx?c_id=" + c_id + "&a_id=" + a_id + "\" target=\"_blank\"><i class=\"fa fa-remove\"></i>Delete</a></li>";
                        }
                        else
                            replyCommentLi = "<li><a href=\"#\"><i class=\"fa fa-reply\"></i>Reply</a></li>";
                    }

                    replyCommentHtml += "<div id=\"commentUl\" class=\"parrent\">" +
                    "<ul class=\"media-list\">" +
                    "<li class=\"post-comment reply\">" +
                    "<a class=\"pull-left\" href=\"#\">" +
                    "<img width=\"70px\" class=\"media-object\" src=\"" + imgSrc + " \" alt=\"" + username + "\">" +
                    "</a>" +
                    "<div class=\"media-body\" style=\"min-width:175px;\">" +
                    "<span><i class=\"fa fa-user\"></i><a href=\"#\">" + username + "</a></span>" +
                    "<p>" + commentText + "</p>" +
                    "<ul class=\"nav navbar-nav post-nav\">" +
                    "<li><a href=\"#\"><i class=\"fa fa-clock-o\"></i>" + FirstCharToUpper(String.Format("{0:MMMM dd, yyyy HH:mm}", commentPubDate)) + "</a></li>" +
                    replyCommentLi +
                    deleteCommentLi +
                    "</ul>" +
                    "</div>" +
                    "</li>" +
                    "</ul>" +
                    "</div>";
                }
            }

            dataReader3.Close();
            command3.Dispose();
            dbConnectionSQLServer.cn.Close();
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in loadComments() reply comments part.</br>");
        }
        
        return replyCommentHtml;
    }


    protected int activityTotalNumberOfComments(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        int count = -1;

        string sql = "SELECT COUNT(*) FROM ACTIVITY_COMMENT WHERE A_COM_A_ID = " + a_id + ";";

        SqlCommand command3;
        SqlDataReader dataReader3;

        try
        {
            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }

            command3 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader3 = command3.ExecuteReader();

            if (dataReader3.HasRows)
            {                
                while (dataReader3.Read())
                {                    
                    count = (int) dataReader3.GetValue(0);
                }
            }

            dataReader3.Close();
            command3.Dispose();
            dbConnectionSQLServer.cn.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in activityTotalNumberOfComments(string a_id).</br>");
        }

        return count;
    }

    protected bool isCurrentUserOwner(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string sql = "SELECT SUB_P_ID FROM SUBSCRIBE WHERE SUB_TYPE = 'CREATOR' AND SUB_A_ID =" + a_id;

        int u_id = -1;

        SqlCommand command2;
        SqlDataReader dataReader2;

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader2 = command2.ExecuteReader();

            while (dataReader2.Read())
            {
                u_id = (int)dataReader2.GetValue(0);
            }

            if (dataReader2.HasRows && (int)Session["id"] == u_id)
            {
                dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();
                return true;
            }
            else
            {
                dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();
                return false;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in isCurrentUserOwner().</br>");
            return true;
        }
    }

    protected bool hasSubscribed(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string sql = "SELECT SUB_A_ID FROM SUBSCRIBE WHERE SUB_A_ID=" + a_id + "AND SUB_TYPE = 'PARTICIPANT';";
        //bool hasSub = false;

        int u_id = -1;

        SqlCommand command2;
        SqlDataReader dataReader2;

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader2 = command2.ExecuteReader();

            while (dataReader2.Read())
            {
                u_id = (int)dataReader2.GetValue(0);

                if ((int)Session["id"] == u_id)

                dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();

                return true;
            }

            if (dataReader2.HasRows)
            {
                dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();

                return true;
            }
            else
            {
                dataReader2.Close();
                command2.Dispose();
                dbConnectionSQLServer.cn.Close();

                return false;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in hasSubscribed().</br>");
            return true;
        }
    }

    protected void addToUserActivityButton_Click(object sender, EventArgs e)
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        int a_id = Int32.Parse(HttpUtility.ParseQueryString(myUri.Query).Get("a_id"));

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        int u_id = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string sql;

        if (Session["id"] != null)
        {
            u_id = (int)Session["id"];

            sql = "INSERT INTO SUBSCRIBE VALUES (@u_id, @a_id, 'PARTICIPANT', SYSDATETIME());";

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
                command2.ExecuteNonQuery();

                //dataReader2.Close();
                command2.Dispose();

                warningBox.Visible = false;
                noticeBox.Visible = false;
                successBox.Visible = true;
                addToUserActivityButton.Visible = false;
                removeFromUserActivityButton.Visible = true;


                int avaiableSpots = -1;

                sql = "SELECT A_AVAILABILITY FROM ACTIVITY WHERE A_ID = " + a_id + ";";

                SqlDataReader dataReader2;
                command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);

                dataReader2 = command2.ExecuteReader();

                while (dataReader2.Read())
                {
                    avaiableSpots = (int)dataReader2.GetValue(0);
                }

                dataReader2.Close();

                sql = "UPDATE ACTIVITY SET A_AVAILABILITY = @avail_spots WHERE A_ID = @a_id;";

                SqlCommand command3;
                //SqlDataReader dataReader2;


                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }


                command3 = new SqlCommand(sql, dbConnectionSQLServer.cn);
                //dataReader2 = command2.ExecuteReader();

                command3.Parameters.AddWithValue("@avail_spots", avaiableSpots - 1);
                command3.Parameters.AddWithValue("@a_id", a_id);
                command3.ExecuteNonQuery();

                //dataReader2.Close();
                command3.Dispose();


                HtmlGenericControl p = new HtmlGenericControl("p");
                p.InnerText = (avaiableSpots - 1).ToString();

                //Response.Write("avaiableSpots is: " + avaiableSpots + "<br />");

/*
                availableSpotsContainer.Controls.Clear();
                availableSpotsContainer.Controls.Add(p);


                int numberIn = avaiableSpots - Int32.Parse(neededPeople.ToString());
                int percentage = numberIn * 100 / Int32.Parse(availableSpots.ToString());

                progressionBar.Attributes.Add("aria-valuenow", (numberIn.ToString()));
                progressionBar.Attributes.Add("aria-valuemax", (Int32.Parse(availableSpots.ToString())).ToString());
                progressionBar.Attributes.Add("style", "width: " + percentage + "%");
*/
                dbConnectionSQLServer.cn.Close();

            }
            catch (Exception ex)
            {
                // If it's the error we raised in trIncreaseCapacityTrg
                if (ex.HResult == -2146232060)
                {
                    successBox.Visible = false;
                    noticeBox.Visible = false;
                    warningBox.Visible = true;
                }
                else
                    Response.Write(ex.Message + " in addToUserActivityButton_Click().</br>");
            }

        }
        else
        {
            Response.Write("You can't add to your activities since you're not loged-in.<br />");
        }

        //Response.Redirect("Activity_Locations.aspx?a_id=" + a_id);
    }

    protected void removeFromUserActivityButton_Click(object sender, EventArgs e)
    {

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        int u_id = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string sql;

        if (Session["id"] != null)
        {
            u_id = (int)Session["id"];

            if (hasSubscribed(a_id))
            {
                sql = "DELETE FROM SUBSCRIBE WHERE SUB_A_ID = " + a_id + " AND SUB_P_ID = " + u_id + ";";

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
                    command2.ExecuteNonQuery();

                    //dataReader2.Close();
                    command2.Dispose();

                    warningBox.Visible = false;
                    successBox.Visible = false;
                    noticeBox.Visible = true;
                    addToUserActivityButton.Visible = true;
                    removeFromUserActivityButton.Visible = false;

                    int avaiableSpots = -1;

                    sql = "SELECT A_AVAILABILITY FROM ACTIVITY WHERE A_ID = " + a_id + ";";

                    SqlDataReader dataReader2;
                    command2 = new SqlCommand(sql, dbConnectionSQLServer.cn);

                    dataReader2 = command2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        avaiableSpots = (int)dataReader2.GetValue(0);
                    }

                    dataReader2.Close();

                    sql = "UPDATE ACTIVITY SET A_AVAILABILITY = @avail_spots WHERE A_ID = @a_id;";

                    SqlCommand command3;
                    //SqlDataReader dataReader2;


                    if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                    {
                        dbConnectionSQLServer.cn.Open();
                    }


                    command3 = new SqlCommand(sql, dbConnectionSQLServer.cn);
                    //dataReader2 = command2.ExecuteReader();

                    command3.Parameters.AddWithValue("@avail_spots", avaiableSpots + 1);
                    command3.Parameters.AddWithValue("@a_id", a_id);
                    command3.ExecuteNonQuery();

                    //dataReader2.Close();
                    command3.Dispose();


                    HtmlGenericControl p = new HtmlGenericControl("p");
                    p.InnerText = (avaiableSpots + 1).ToString();

                    //Response.Write("avaiableSpots is: " + avaiableSpots + "<br />");

/*                    availableSpotsContainer.Controls.Clear();
                    availableSpotsContainer.Controls.Add(p);
*/
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message + " in removeFromUserActivityButton_Click() in look for availability part.</br>");
                }

                dbConnectionSQLServer.cn.Close();
            }
            else
            {
                Response.Write("You can't add to your activities since you're not loged-in.<br />");
            }

            //Response.Redirect("Activity_Locations.aspx?a_id=" + a_id);

        }
    }


    protected void deleteActivityButton_Click(object sender, EventArgs e)
    {

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        int u_id = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string sql;

        if (Session["id"] != null)
        {
            u_id = (int)Session["id"];

                sql = "DELETE FROM ACTIVITY WHERE A_ID = " + a_id + ";";

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

                    command2.Parameters.AddWithValue("@a_id", a_id);
                    command2.ExecuteNonQuery();

                    //dataReader2.Close();
                    command2.Dispose();
                    dbConnectionSQLServer.cn.Close();

                    leftColumn.Controls.Clear();
                    volunteersAttendance.Controls.Clear();
                    activityDescriptionContainer.Controls.Clear();
                    commentsBox.Controls.Clear();
                    activityImageContainer.Controls.Clear();
                    activityTitleContainer.Controls.Clear();

                    warningBox.Visible = false;
                    successBox.Visible = false;
                    noticeBox.Visible = false;

//                    doesntExist.Controls.Clear();

                    activityContainer.Controls.Clear();

                    HtmlGenericControl span = new HtmlGenericControl("b");
                    span.InnerText = "Activity deleted";

                    HtmlGenericControl p = new HtmlGenericControl("p");
                    p.InnerText = "Your activity was permanently deleted.";

                    activityContainer.Controls.Add(span);
                    activityContainer.Controls.Add(p);

//                    doesntExist.Visible = true;

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message + " in deleteActivityButton_Click().</br>");
                }

            //Response.Redirect("Activity_Locations.aspx?a_id=" + a_id);

        }
    }


    protected void loadChildren(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string childName = "Dummy childName";

        int ms = 300;
        int totalChildrenNumber = -1;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string sql;        

        SqlCommand command;
        SqlDataReader dataReader;

        try
        {
            // SELECT all children who didn't leave yet.
            sql = "SELECT COUNT(*) FROM CHILD AS CH WHERE CH.CH_LEFT_CENTER_DATE IS NULL;";

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    totalChildrenNumber = (int)dataReader.GetValue(0);                   
                }
            }

            dataReader.Close();
            command.Dispose();
            dbConnectionSQLServer.cn.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in loadChildren() in total children number part.</br>");
        }

        try
        {
            sql = "SELECT COUNT(*) FROM SUBSCRIBE_CHILD AS SUB_CH JOIN CHILD AS CH ON SUB_CH.SUB_CH_CHILD_ID = CH.CH_ID WHERE SUB_CH.SUB_CH_ACTIVITY_ID = " + a_id + " AND CH.CH_LEFT_CENTER_DATE IS NULL;";

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }


            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    int childCount = (int) dataReader.GetValue(0);

                    int percentage = childCount * 100 / totalChildrenNumber;

                    childrenAttendanceProgressionBar.Attributes.Add("aria-valuenow", (childCount.ToString()));
                    childrenAttendanceProgressionBar.Attributes.Add("aria-valuemax", totalChildrenNumber.ToString());
                    childrenAttendanceProgressionBar.Attributes.Add("style", "width: " + percentage + "%");

                    childrenAttendanceProgressionBar.InnerHtml = "<b>" + percentage + "% - " + childCount.ToString() + "/ " + totalChildrenNumber.ToString() + " children</b>";
                }
            }

            dataReader.Close();
            command.Dispose();
            dbConnectionSQLServer.cn.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in loadChildren() in progressBar part.</br>");
        }


        if (Session["id"] != null)
        {
            sql = "SELECT CONCAT(CH.CH_FNAME, ' ' + CH.CH_LNAME) FROM SUBSCRIBE_CHILD AS SUB_CH JOIN CHILD AS CH ON SUB_CH.SUB_CH_CHILD_ID = CH.CH_ID WHERE SUB_CH.SUB_CH_ACTIVITY_ID = " + a_id + ";";

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }


                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (ms > 1200)
                        ms = 300;

                        childName = (string)dataReader.GetValue(0);

                        childrenRepeater.InnerHtml += "<div class=\"col-sm-3 wow fadeInUp\" data-wow-duration=\"500ms\" data-wow-delay=\"" + ms + "ms\">" +
                            "<div class=\"feature-inner\">" +
                            "<div class=\"icon-wrapper\">" +
                            "<i class=\"fa fa-2x fa-heart\"></i>" +
                            "</div>" +
                            "<h3>" + childName + "</h3>" +
                            "</div>" +
                            "</div>";

                        ms += 300;
                    }
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " in loadChildren() in repeater part.</br>");
            }

/*
            HtmlGenericControl imgConnectedAvatar = new HtmlGenericControl("img");
            //imgConnectedAvatar.Attributes.Add("width", "55px");
            //imgConnectedAvatar.Attributes.Add("height", "55px");
            imgConnectedAvatar.Attributes.Add("width", "100%");
            imgConnectedAvatar.Attributes.Add("alt", username);
            imgConnectedAvatar.Attributes.Add("id", "connectedAvatar");

            if (File.Exists(absolutePath + "people/" + username + ".jpg"))
                imgConnectedAvatar.Attributes.Add("src", "images/people/" + username + ".jpg" + "?ver=" + File.GetLastWriteTime(Server.MapPath("/images/")).ToFileTime());
            else
                imgConnectedAvatar.Attributes.Add("src", "images/people/square-profile-generic.jpg");

            comment_form.Controls.Add(imgConnectedAvatar);

            comment.Visible = true;
            submitCommentButton.Visible = true;
*/
        }
        else
        {
            childrenRepeater.InnerHtml = "<center>You can only view this if you're connected. <a href=\"Login.aspx\"><b>log-in</b></a>.</center><br />";
        }
    }


    protected void loadVolunteers(string a_id)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");

        string volunteerName = "Dummy volunteerdName";
        string volunteerUsername = "Dummy userName";

        int totalVolunteersCapacity = -1;
        int participatingVolunteersNumber = -1;

        int ms = 300;
        int volunteerID = -1;

        string sql;
        
        SqlCommand command;
        SqlDataReader dataReader;

        try
        {

            if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
            {
                dbConnectionSQLServer.cn.Open();
            }

            sql = "SELECT COUNT(*) FROM SUBSCRIBE WHERE SUB_A_ID = " + a_id + " AND SUB_TYPE = 'PARTICIPANT';";

            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    participatingVolunteersNumber = (int) dataReader.GetValue(0);
                }                
            }

            dataReader.Close();
            command.Dispose();

            sql = "SELECT A_CAPACITY FROM ACTIVITY WHERE A_ID = " + a_id + ";";

            command = new SqlCommand(sql, dbConnectionSQLServer.cn);
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    totalVolunteersCapacity = (int)dataReader.GetValue(0);
                }
            }

            dataReader.Close();
            command.Dispose();

            int partialNumber = participatingVolunteersNumber;
            int percentage = participatingVolunteersNumber * 100 / totalVolunteersCapacity;

            progressionBar.Attributes.Add("aria-valuenow", (partialNumber.ToString()));
            progressionBar.Attributes.Add("aria-valuemax", "32");
            progressionBar.Attributes.Add("style", "width: " + percentage + "%");

            progressionBar.InnerHtml = "<b>" + percentage + "% - " + partialNumber.ToString() + "/" + totalVolunteersCapacity.ToString() + " spots</b>";

            dbConnectionSQLServer.cn.Close();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in loadVolunteers() in progressBar part.</br>");
        }


        if (Session["id"] != null)
        {
            sql = "SELECT CONCAT(P.P_FNAME, ' ' + P.P_LNAME), P_USERNAME, P_ID FROM SUBSCRIBE AS SUB JOIN PERSON AS P ON SUB.SUB_P_ID = P.P_ID WHERE SUB_A_ID = " + a_id + " and SUB_TYPE = 'PARTICIPANT';";

            try
            {

                if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
                {
                    dbConnectionSQLServer.cn.Open();
                }


                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (ms > 1200)
                            ms = 300;

                        volunteerName = (string)dataReader.GetValue(0);
                        volunteerUsername = (string)dataReader.GetValue(1);
                        volunteerID = (int)dataReader.GetValue(2);

                        string imgSrc = getUniquePath("people", volunteerUsername);

                        volunteersRepeater.InnerHtml += "<div class=\"col-sm-3 wow fadeInUp\" data-wow-duration=\"500ms\" data-wow-delay=\"" + ms + "ms\">" +
                            "<div class=\"feature-inner\">" +
                            "<a href=\"Profile.aspx?id=" + volunteerID.ToString() + " \">" +
                            "<img src=\"" + imgSrc + "\" width=\"100px\" alt=\"Volunteer " + volunteerName + "\" class=\"img-rounded\" />" +
                            "<h3>" + volunteerName + "</h3>" +
                            "</a>" +
                            "</div>" +
                            "</div>";

                        ms += 300;
                    }
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + " in loadVolunteers() in repeater part.</br>");
            }

            /*
                        HtmlGenericControl imgConnectedAvatar = new HtmlGenericControl("img");
                        //imgConnectedAvatar.Attributes.Add("width", "55px");
                        //imgConnectedAvatar.Attributes.Add("height", "55px");
                        imgConnectedAvatar.Attributes.Add("width", "100%");
                        imgConnectedAvatar.Attributes.Add("alt", username);
                        imgConnectedAvatar.Attributes.Add("id", "connectedAvatar");

                        if (File.Exists(absolutePath + "people/" + username + ".jpg"))
                            imgConnectedAvatar.Attributes.Add("src", "images/people/" + username + ".jpg" + "?ver=" + File.GetLastWriteTime(Server.MapPath("/images/")).ToFileTime());
                        else
                            imgConnectedAvatar.Attributes.Add("src", "images/people/square-profile-generic.jpg");

                        comment_form.Controls.Add(imgConnectedAvatar);

                        comment.Visible = true;
                        submitCommentButton.Visible = true;
            */
        }
        else
        {
            volunteersRepeater.InnerHtml = "<center>You can only view this if you're connected. <a href=\"Login.aspx\"><b>log-in</b></a>.</center><br />";
        }
    }


    public string getUniquePath(string where, string name)
    {
        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        SqlCommand command;
        SqlDataReader dataReader;

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string path = absolutePath + where + "/" + name + ".jpg";
        string uniquePath = "";
        string gender = "";

        try
        {
            if (File.Exists(path))
                uniquePath = "images/" + where + "/" + name + ".jpg" + "?ver=" + File.GetCreationTime(Server.MapPath("/images/" + where)).ToFileTime();
            else
            {
                if (where == "images/people/")
                {
/*                    string sql = "SELECT P_GENDER FROM PERSON WHERE P_USERNAME = " + name + ";";

                    command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                    dataReader = command.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            gender = (string)dataReader.GetValue(0);
                        }
                    }

                    dataReader.Close();
                    command.Dispose();
*/
//                    if (gender.ToLower() == "male")
//                        uniquePath = "images/people/male-generic-profile.jpg";
//                    else
                        uniquePath = "images/people/female-generic-profile.jpg";
                }
                else if (where == "images/activities/")
                {
                    uniquePath = "images/activities/defaultEvent.png";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + " in getUniquePath().</br>");
        }

        return uniquePath;
    }
}