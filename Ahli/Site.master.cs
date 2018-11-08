using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : MasterPage
{

    public string keyword, date;
    public DBConnectionSQLServer dbConnectionSQLServer = new DBConnectionSQLServer();

    public string name = "Dummy";
    public string email = "Dummy email";
    public string username = "Dummy username";

    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        loadNavigationBar();
    }

    protected void loadNavigationBar()
    {
        if (Session["id"] != null)
        {
            string id = Session["id"].ToString();
            //Response.Write("Session[\"id\"] is " + id + "</br>");

            string sql = "SELECT P_FNAME, P_EMAIL, P_USERNAME FROM PERSON WHERE P_ID = " + Session["id"] + ";";

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
                    name = (String)dataReader.GetValue(0);
                    email = (String)dataReader.GetValue(1);
                    username = (String)dataReader.GetValue(2);
                }

                dataReader.Close();
                command.Dispose();
                dbConnectionSQLServer.cn.Close();
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message + " in MasterPage.</br>");
            }


            name = name.ToUpper();

            // This function calls the Master page's method to modify the navigation bar.
            nameHref.InnerText = name;


            navConnectedLi.Visible = true;
            navCreateActivity.Visible = true;
            navAddChild.Visible = true;
            navAddRelatedPerson.Visible = true;

            navLogin.Visible = false;
            navSignup.Visible = false;
            navLogout.Visible = true;
            navPersonName.Visible = true;
            nameBottomContactForm.Visible = false;
            emailBottomContactForm.Visible = false;

            unknownUBottomContactForm.InnerHtml = name + " - <a href=\"Logout.aspx\">Log-out</a>";
            emailUnknownUBottomContactForm.InnerText = email;

            unknownUBottomContactForm.Visible = true;
            emailUnknownUBottomContactForm.Visible = true;
        }
        else
        {
            //Response.Write("Session[\"id\"] is null.</br>");

            navConnectedLi.Visible = false;
            navCreateActivity.Visible = false;
            navAddChild.Visible = false;
            navAddRelatedPerson.Visible = false;

            navLogin.Visible = true;
            navSignup.Visible = true;
            navLogout.Visible = false;
            navPersonName.Visible = false;
            nameBottomContactForm.Visible = true;
            emailBottomContactForm.Visible = true;
            unknownUBottomContactForm.Visible = false;
            emailUnknownUBottomContactForm.Visible = false;
        }
    }

/*    protected void OnClick_Search(object sender, EventArgs e)
    {
        keyword = keyword_search.Value.ToString();
        date = datepicker_search.Value.ToString();
        Response.Redirect("SearchResult.aspx?keyword=" + keyword + "&date=" + date + "&city=0");
    }
*/
}