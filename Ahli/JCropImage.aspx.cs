using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;


public partial class JCropImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string username = HttpUtility.ParseQueryString(myUri.Query).Get("username");

        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string relativePath = "images/";
        string currentPage = "";
        string errorMessage = "No error here.";

        // Flag variable to check if all is good in the sql part.
        bool allGood = true;

        DBConnectionSQLServer dbConnectionSQLServer = ((SiteWOform)this.Master).dbConnectionSQLServer;

        string sql;
        int sqlU_ID = -1;
        int sqlA_ID = -1;

        if (dbConnectionSQLServer != null && dbConnectionSQLServer.cn.State == ConnectionState.Closed)
        {
            dbConnectionSQLServer.cn.Open();
        }

        SqlCommand command;
        SqlDataReader dataReader;

        if (username == null)
        {
            if (a_id == null)
            {
                errorMessage = "Both the username and the activity ID are null.";
                mostInnerContainer.Controls.Clear();
            }
            else
            {
                errorMessage = "The username is null.";
                mostInnerContainer.Controls.Clear();
            }

        }
        else if (username != null && a_id == null)
        {
            // If an activity has the same username as the one specified.
            sql = "SELECT P_ID FROM PERSON WHERE P_USERNAME = '" + username + "';";

            try
            {

                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    sqlU_ID = (int)dataReader.GetValue(0);
                }


                // If no rows are found, or if session[id] is not equal
                // to the specified U_ID, it's not all good.
                int seissionWithoutNull = -1;

                if (Session["id"] != null)
                    seissionWithoutNull = (int)Session["id"];

                if (!dataReader.HasRows)
                {
                    allGood = false;
                    errorMessage = "There is no username called " + username + ".";
                    mostInnerContainer.Controls.Clear();
                }
                else if (seissionWithoutNull != sqlU_ID)
                {
                    allGood = false;
                    errorMessage = "There is no match between the session ID and the user's ID.<br />In other words please log-in or if you did try to modify your own profile picture =)";
                    mostInnerContainer.Controls.Clear();
                }


                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();

            }

            catch (Exception ex)
            {
                Response.Write(ex.Message + " in Page_Load() 1.</br>");
            }

        }
        else
        {
            // If an activity has the same username as the one specified.
            sql = "SELECT S.SUB_P_ID, A.A_ID FROM ACTIVITY AS A JOIN SUBSCRIBE AS S ON A.A_ID = S.SUB_A_ID JOIN PERSON AS P ON S.SUB_P_ID = P.P_ID WHERE A.a_id = " + a_id + " AND P.P_USERNAME = '" + username + "';";

            try
            {
                command = new SqlCommand(sql, dbConnectionSQLServer.cn);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    sqlU_ID = (int)dataReader.GetValue(0);
                    sqlA_ID = (int)dataReader.GetValue(1);
                }


                // If no rows are found, or if session[id] is not equal
                // to the specified U_ID, it's not all good.
                int seissionWithoutNull = -1;

                if (Session["id"] != null)
                    seissionWithoutNull = (int)Session["id"];

                if (!dataReader.HasRows)
                {
                    allGood = false;
                    errorMessage = "There is no match between the username and the activity ID.<br />In other words, try to modify your own activity =)";
                    mostInnerContainer.Controls.Clear();
                }
                else if (seissionWithoutNull != sqlU_ID)
                {
                    allGood = false;
                    errorMessage = "There is no match between the session ID and the user's ID.";
                    mostInnerContainer.Controls.Clear();
                }



                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();

            }

            catch (Exception ex)
            {
                Response.Write(ex.Message + " in Page_Load() 2.</br>");
            }
        }

        // If there is a username in the URL.
        if (username != null && allGood)
        {   // If there is an event ID in the URL.
            if (a_id != null)
            {   // If there is a already a user's event photo stored.
                if (File.Exists(absolutePath + "activities/" + "event_" + a_id + ".jpg"))
                {
                    // Display it instead of the default picture.
                    target.Attributes.Remove("src");
                    target.Attributes.Add("src", relativePath + "activities/" + "event_" + a_id + ".jpg" + "?ver=" + File.GetLastWriteTime(Server.MapPath("/" + relativePath)).ToFileTime());

                    target.Attributes.Remove("style");

                    CropDiv.Attributes.Remove("style");


                }
                // If there is NO user's event photo stored.
                else
                {
                    target.Attributes.Remove("src");
                    target.Attributes.Add("src", relativePath + "activities/defaultEvent.png");

                    // Hide Crop Button
                    CropDiv.Attributes.Remove("style");
                    CropDiv.Attributes.Add("style", "display: none;");

                    //javascriptPlaceHolder.Controls.Add();
                    //javascriptPlaceHolder.Controls.Clear();
                }

                currentPage = "activity page.";


                HtmlGenericControl h3Title = new HtmlGenericControl("h3");
                h3Title.InnerHtml = "Upload and crop a photo for your " + currentPage;

                title.Controls.Add(h3Title);
            }
            // If there is NO event ID in the URL.
            else
            {   // If the image in JCrop.aspx is the default image.
                if (target.Attributes["src"] == "images/people/profile-generic.jpg")
                {
                    // If there is a already a user's profile photo stored.
                    if (File.Exists(absolutePath + "people/" + username + ".jpg"))
                    {
                        // Display it instead of the default picture.
                        target.Attributes.Remove("src");
                        target.Attributes.Add("src", relativePath + "people/" + username + ".jpg" + "?ver=" + File.GetLastWriteTime(Server.MapPath("/" + relativePath)).ToFileTime());

                        CropDiv.Attributes.Remove("style");

                    }
                    // If there is NO user's profile photo stored.
                    else
                    {
                        // Hide Crop Button
                        CropDiv.Attributes.Remove("style");
                        CropDiv.Attributes.Add("style", "display: none;");

                    }
                }

                //if (!File.Exists(absolutePath + "people/" + username + ".jpg") && !File.Exists(absolutePath + "people/" + username + "_uploaded.jpg"))
                //{
                //    Response.Write("somekeyx1 <br />");


                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "somekeyx1", "simulatex1Click();", true);
                //}
                //else 
                //{
                //    Response.Write("somekeyx2 <br />");


                //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "somekeyx2", "simulatex2Click();", true);
                //}


                currentPage = "profile page.";

                HtmlGenericControl h3Title = new HtmlGenericControl("h3");
                h3Title.InnerHtml = "Upload and crop a photo for your " + currentPage;

                title.Controls.Add(h3Title);
            }
        }
        else
        {
            // if the username and the activity ID are both null, don't display anything.
            //mostInnerContainer.Attributes.Add("style", "display: none;");

            HtmlGenericControl pErrorBr = new HtmlGenericControl("p");
            pErrorBr.InnerHtml = " ";

            HtmlGenericControl h2ErrorMessage = new HtmlGenericControl("h3");
            h2ErrorMessage.InnerText = "Error:";

            HtmlGenericControl pErrorMessage = new HtmlGenericControl("p");
            pErrorMessage.InnerHtml = errorMessage;

            mostInnerContainer.Controls.Add(pErrorBr);
            mostInnerContainer.Controls.Add(h2ErrorMessage);
            mostInnerContainer.Controls.Add(pErrorMessage);
        }

        // If the user has uploaded a photo
        if (IsPostBack && File1.PostedFile != null)
        {
            if (File1.PostedFile.FileName.Length > 0)
            {
                Upload_Click(sender, e);
            }
        }

        HtmlGenericControl pGoBack = new HtmlGenericControl("p");
        pGoBack.InnerText = "<< Go back to the " + currentPage;

        HtmlGenericControl aGoBack = new HtmlGenericControl("a");
        aGoBack.Attributes.Add("href", GoBack());

        aGoBack.Controls.Add(pGoBack);
        goBack.Controls.Add(aGoBack);
    }

    protected void Crop_Click(Object sender, EventArgs e)
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string username = HttpUtility.ParseQueryString(myUri.Query).Get("username");
        string type = "";

        if (a_id == null)
            type = "profile_photo";
        else
            type = "event";


        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
        string relativePath = "images/";

        string InputPath = "";
        string OutputPath = "";

        if (type == "profile_photo")
        {
            relativePath += "people/" + username + ".jpg";
            OutputPath = absolutePath + "people/" + username + ".jpg";
            InputPath = absolutePath + "people/" + username + "_uploaded.jpg";

            if (File.Exists(OutputPath) && !File.Exists(InputPath))
            {
                InputPath = OutputPath;
            }

        }
        else if (type == "event")
        {
            relativePath += "activities/" + "event_" + a_id + ".jpg";
            OutputPath = absolutePath + "activities/" + "event_" + a_id + ".jpg";
            InputPath = absolutePath + "activities/" + "event_" + a_id + "_uploaded.jpg";

            if (File.Exists(OutputPath) && !File.Exists(InputPath))
            {
                InputPath = OutputPath;
            }
        }

        int X1 = Convert.ToInt32(Double.Parse(x1.Value));
        int Y1 = Convert.ToInt32(Double.Parse(y1.Value));
        int X2 = Convert.ToInt32(Double.Parse(x2.Value));
        int Y2 = Convert.ToInt32(Double.Parse(y2.Value));
        int W = Convert.ToInt32(Double.Parse(w.Value));
        int H = Convert.ToInt32(Double.Parse(h.Value));


        //Image img = Image.FromFile(InputPath);        

        FileStream fs = new FileStream(InputPath, FileMode.Open);
        Image img = Image.FromStream(fs);

        Image croppedJpeg = CropAndResizeImage(img, W, H, X1, Y1, X2, Y2, ImageFormat.Jpeg);


        fs.Close();
        File.Delete(InputPath);

        croppedJpeg.Save(OutputPath, ImageFormat.Jpeg);

        // Display the croped image.
        modifyTargetImage(relativePath, croppedJpeg.Width.ToString(), croppedJpeg.Height.ToString());


        img.Dispose();
        croppedJpeg.Dispose();


    }

    protected void Delete_Click(Object sender, EventArgs e)
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string username = HttpUtility.ParseQueryString(myUri.Query).Get("username");
        string type = "", path = "";

        if (a_id == null)
            type = "profile_photo";
        else
            type = "event";


        string absolutePath = Server.MapPath("/images/").Replace("\\", "/");

        if (type == "profile_photo")
            path = absolutePath + "people/" + username + ".jpg";

        else if (type == "event")
            path = absolutePath + "activities/" + "event_" + a_id + ".jpg";


        File.Delete(path);

        Response.Redirect(Request.RawUrl);
    }

    protected void Upload_Click(object sender, EventArgs e)
    {

        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string username = HttpUtility.ParseQueryString(myUri.Query).Get("username");
        string type = "";

        if (a_id == null)
            type = "profile_photo";

        else
            type = "event";

        if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
        {
            //string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
            //string SaveLocation = Server.MapPath("Data") + "\\" + fn;

            string absolutePath = Server.MapPath("/images/").Replace("\\", "/");
            string relativePath = "images/";

            string SaveLocation = absolutePath;


            if (type == "profile_photo")
            {
                relativePath = relativePath + "people/" + username + "_uploaded.jpg";
                SaveLocation = absolutePath + "people/" + username + "_uploaded.jpg";
            }


            else if (type == "event")
            {
                relativePath = relativePath + "activities/" + "event_" + a_id + "_uploaded.jpg";
                SaveLocation = absolutePath + "activities/" + "event_" + a_id + "_uploaded.jpg";
            }

            try
            {
                File1.PostedFile.SaveAs(SaveLocation);
                //Response.Write("The file has been uploaded.<br>");

                Image uploadedJpeg = Image.FromFile(SaveLocation);

                // Display the uploaded image.
                modifyTargetImage(relativePath, uploadedJpeg.Width.ToString(), uploadedJpeg.Height.ToString());

                uploadedJpeg.Dispose();
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message + "<br>");
                //Note: Exception.Message returns a detailed message that describes the current exception. 
                //For security reasons, we do not recommend that you return Exception.Message to end users in 
                //production environments. It would be better to put a generic error message. 
            }
        }
        else
        {
            //Response.Write("Please select a file to upload.<br>");
        }

    }

    protected void modifyTargetImage(string relativePath, string width, string height)
    {
        target.Attributes.Remove("src");
        target.Attributes.Add("src", relativePath + "?ver=" + File.GetLastWriteTime(Server.MapPath("/" + relativePath)).ToFileTime());

        target.Attributes.Remove("width");
        target.Attributes.Add("width", width);

        target.Attributes.Remove("height");
        target.Attributes.Add("height", height);

        // Removes style="display: none;" from CropDiv.
        // In other words, the buttons in CropDiv will appear again.
        CropDiv.Attributes.Remove("style");
    }

    public Image CropAndResizeImage(Image img, int targetWidth, int targetHeight, int x1, int y1, int x2, int y2, ImageFormat imageFormat)
    {
        var bmp = new Bitmap(targetWidth, targetHeight);
        Graphics g = Graphics.FromImage(bmp);

        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.CompositingQuality = CompositingQuality.HighQuality;

        int width = x2 - x1;
        int height = y2 - y1;

        g.DrawImage(img, new Rectangle(0, 0, targetWidth, targetHeight), x1, y1, width, height, GraphicsUnit.Pixel);

        var memStream = new MemoryStream();
        bmp.Save(memStream, imageFormat);

        bmp.Dispose();

        return Image.FromStream(memStream);
    }

    protected string GoBack()
    {
        // Fetch current page url and extract a_id from it.
        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
        string a_id = HttpUtility.ParseQueryString(myUri.Query).Get("a_id");
        string username = HttpUtility.ParseQueryString(myUri.Query).Get("username");
        string type = "", path = "";

        if (a_id == null)
            type = "profile_photo";
        else
            type = "event";

        if (type == "profile_photo")
            return "Profile.aspx?u_id=" + "";

        else if (type == "event")
            return "Activity.aspx?a_id=" + a_id;

        else
            return "username & a_id were both null.";

        File.Delete(path);

        Response.Redirect(Request.RawUrl);
    }
}