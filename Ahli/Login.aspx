<%@ Page Title="Log-in" Language="C#" MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Log-in</h1>
                            <p>Please enter your log-in details:</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <section id="shortcodes">
        <div class="container">

            <div id="tab-container">
                <div class="row">
                    <div class="col-md-6">

                        <asp:Panel class="alert alert-danger fade in" runat="server" ID="error_div" Visible="false">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">Ok, got it.</button>
                            <h4>Either your username or password is wrong!</h4>
                            <p>Change what you typed and try again. If it still doesn't work maybe you've forgotten your password, in this case click <a href=#>here.</a></p>
                        </asp:Panel>

                    <div class="contact-form bottom" >
                        <form name="login-form-user" id="loginForm" runat="server" method="post" >
                            <div class="form-group">
                                <input runat="server" type="text" name="IdentifierLogin" class="form-control" required="required" placeholder="Username or Email" id="txtIdentifier" />                                
                            </div>
                            <div class="form-group">
                                <input runat="server" type="password" name="passwordLogin" class="form-control" required="required" placeholder="Password" id="txtPassword" />                                
                            </div>
                     
                            <div class="form-group">
                                <asp:Button runat="server" type="submit" name="submit" class="btn btn-submit" Text="Log-in" ID="LoginButton" OnClick="Login_Click" />                                
                            </div>
                        </form>
                    </div>

                    </div>

                    <div class="col-md-6">


                    </div>
                </div>
            </div><!--/#table-container-->

            <div class="padding"></div>

        </div>
    </section>           

</asp:Content>
