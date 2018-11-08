<%@ Page Title="Profile" Language="C#" MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="_Profile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<form runat="server">

        <asp:ScriptManager runat="server"> 
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>

                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" /> 
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="lightbox" />
                <asp:ScriptReference Name="wow" />
                <asp:ScriptReference Name="main" />                

            </Scripts>
        </asp:ScriptManager>   

<div class="container">                

    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title"><asp:Label runat="server" ID="NameLabel" CssClass="name" Text="DEFAULT USER" Height="30px"></asp:Label>'s profile details</h1>
                            <p>You can modify your profile details here!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->      

        <br />

            <div id="tab-container">
                <div class="row">

                        <ul id="tab1" class="nav nav-tabs">
                            <li class="active"><a runat="server" id="profilePictureTab" name="profilePicture" href="#tab1-item1" data-toggle="tab">Profile picture</a></li>
                            <li><a runat="server"  href="#tab1-item2" data-toggle="tab">Details</a></li>                            
                        </ul>                            

                        <div class="tab-content">

                            <!-- Tab 2 -->
                            <div class="tab-pane fade active in" id="tab1-item1">
                <div class="col-sm-3 wow fadeInUp" >

                    <div id="coverAndProfile">            

                                <div class="single-price price-one">
                                    <ul>                                                        
                                        <li id="profilePictureText" runat="server">Profile picture<span><a runat="server" id="profilePictureHref"><i class="fa fa-pencil"></i></a></span></li>                        
                                            <div class="profilePic" style="text-align: center">
                                                <a runat="server" id="ProfilePicture"><img runat="server" id="profile_pic" width="262"/></a>
                                            </div>
                                    </ul>
                                    <br />
                               </div>
                    </div>

                 </div>

            </div>


                            <!-- Tab 1 -->
                            <div class="tab-pane fade" id="tab1-item2">

<div class="row" runat="server" id="Biography">
                  <div class="col-sm-4 wow fadeInUp" >
                    <div class="single-price price-one">
                        <ul>                                                        
                            <li>Email: <a runat="server" ID="emailLabel">No availaible email for the moment</a><span><i class="fa fa-pencil"></i></span></li>

<%--                            <a runat="server" id="edit_email" class="fa fa-pencil" aria-hidden="true" onserverclick="onCLick_emailEdit">edit</a>                           
                            <asp:TextBox runat="server" CssClass="form-control" ID="emailEditBox" TextMode="MultiLine"></asp:TextBox>
                            <asp:Button runat="server" CssClass="btn btn-submit" ID="emailSubmit" OnClick="onClick_SubmitEmailChanges" Text="Submit"/>
                            <asp:Button runat="server" CssClass="btn btn-submit" ID="emailCancel" OnClick="onClick_CancelEmailChanges" Text="Cancel"/>
--%>
                            <li>Birthday:  <a runat="server" ID="birthdayLabel">No availaible Biography for the moment</a><span><i class="fa fa-pencil"></i></span></li>

<%--                            <a runat="server" id="edit_birthday" class="fa fa-pencil" aria-hidden="true" onserverclick="onCLick_aboutmeEdit">edit</a>                          
                            <asp:TextBox runat="server" CssClass="form-control" ID="birthdayEditBox" TextMode="MultiLine"></asp:TextBox>
                            <asp:Button runat="server" CssClass="btn btn-submit" ID="birthdaySubmit" OnClick="onClick_SubmitBirthdayChanges" Text="Submit"/>
                            <asp:Button runat="server" CssClass="btn btn-submit" ID="birthdayCancel" OnClick="onClick_CancelBirthdayChanges" Text="Cancel"/>
--%> 
                            <li>City: <a runat="server" ID="LocationLabel" cssClass="introParagraph">No availaible city for the moment</a><span><a runat="server" id="edit_location" aria-hidden="true" onserverclick="onClick_locationEdit"><i class="fa fa-pencil"></i></a></span>

                            <div runat="server" id="editCity" class="row">                                
                                <br />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="CityList"></asp:DropDownList>
                                <br />
                                <asp:Button runat="server" ID="location_edit_submit" OnClick="onClick_SubmitLocationChanges" CssClass="btn btn-info" Text="Submit"/>
                                <asp:Button runat="server" ID="location_edit_cancel" OnClick="onClick_CancelLocationChanges" CssClass="btn btn-info" Text="Cancel"/>
                            </div>

                            </li>

                            <li>Biography: <a ID="AboutMeLabel" runat="server">No availaible Biography for the moment</a><span><a runat="server" id="edit_aboutme" aria-hidden="true" onserverclick="onCLick_aboutmeEdit"><i class="fa fa-pencil"></i></a></span>
                              
                            <div runat="server" id="editBio" class="row">
                                <br />                                
                                <asp:TextBox runat="server" CssClass="form-control" ID="aboutme_edit" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <asp:Button runat="server" ID="aboutme_edit_submit" OnClick="onClick_SubmitAboutmeChanges"  CssClass="btn btn-info"  Text="Submit"/>
                                <asp:Button runat="server" ID="aboutme_cancel_button" OnClick="onClick_CancelAboutmeChanges" CssClass="btn btn-info"  Text="Cancel"/>
                            </div>
                            </li>

                            <li>Preferences: <a runat="server" id="preferencesMessage">No prefereces</a><span><a runat="server" id="edit_preferences" aria-hidden="true" onserverclick="onCLick_preferencesEdit"><i class="fa fa-pencil"></i></a></span>
                                                       
                            <div runat="server" id="editpreferences" class="row">
                                <ul runat="server" id="preferences_list" class = "dropdown-menu">
                                </ul>
                            <br />
                                <div class="col-md-4 column">
                                    <asp:ListBox runat="server" ID="listBoxSubCategories" SelectionMode="Single" Rows="10"></asp:ListBox>
                                </div>
                                <div class="col-md-4 column">
                                    <asp:Button runat="server" ID="add" OnClick="onClick_AddPreference" CssClass="btn btn-success" Text="Add >>"/>
                                    <p> </p>
                                    <asp:Button runat="server" ID="remove" OnClick="OnClick_RemovePreference" CssClass="btn btn-primary" Text="<< Remove"/>
                                    <p> </p>
                                    <asp:Button runat="server" ID="cancel" OnClick="OnClick_CancelPreference" CssClass="btn btn-info" Text="Cancel"/>
                                </div>
                                <div class="col-md-4 column">
                                    <asp:ListBox runat="server" ID="listBoxPreferences" SelectionMode="Single" Rows="10"></asp:ListBox>
                                </div>
                            </div>
                            
                            </li>

                        </ul>                        
                    </div>
                  </div>

                </div>                                

                                <br />
                            </div>



                        </div>

                </div>
            </div><!--/#table-container-->  
            



    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Your latest activity</h1>
                            <p>You can see your latest activity here!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <br />

<div id="tab-containerLatest">
                <div class="row">

                        <ul id="tab1Latest" class="nav nav-tabs">

                            <li class="active"><a runat="server" id="notifications_label" href="#tab1-item6Latest" data-toggle="tab">Notifications</a></li>
                            <li><a runat="server" id="eventsUpcoming_label" href="#tab1-item5Latest" data-toggle="tab">Upcoming Activities</a></li>
                            

                        </ul>                            

                        <div class="tab-content">

                            <div class="tab-pane fade active in" id="tab1-item6Latest">
                                
                                <div runat="server" id="Notifications">
                               
                                </div>

                                <br />
                            </div>

                            <div class="tab-pane fade" id="tab1-item5Latest">
                                
                                <div runat="server" id="Upcoming">
                                    <br />
                                </div>

                                <br />

                            </div>

                            
                        </div>

                </div>
            </div>





    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Your Latest history</h1>
                            <p>You can see your history activity here!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <br />

            <div id="tab-containerHistory">
                <div class="row">

                        <ul id="tab1History" class="nav nav-tabs">
                            <li class="active"><a runat="server" id="eventsCreated_label" name="eventscreated" href="#tab1-item3History" data-toggle="tab">Created Activities</a></li>
                            <li><a runat="server" id="eventsAttended_label" title="attended" name="eventsattended" href="#tab1-item4History" data-toggle="tab">Attended Activities</a></li>                                               
                        </ul>                            

                        <div class="tab-content">

                            <!-- Third  Tab -->
                            <div class="tab-pane fade active in" id="tab1-item3History">
                                
            <div runat="server" id="Created" class="row">
                
                <%-- The activities will be added right here! --%>

                <%-- GridView was used in order to display all the created events, it however did not satisfy an appropriate visual aspect --%>
                <%--<asp:GridView CellSpacing="10" HeaderStyle-Font-Underline="true" GridLines="none" cssClass="gridview" ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="CreatedEvents" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="A_NAME" HeaderText="Name" SortExpression="A_NAME" />
                        <asp:BoundField DataField="A_DESCRIPTION" HeaderText="Description" SortExpression="A_DESCRIPTION" />
                        <asp:BoundField DataField="A_START" HeaderText="From" SortExpression="A_START" />
                        <asp:BoundField DataField="A_END" HeaderText="To" SortExpression="A_END" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="CreatedEvents" runat="server" ConnectionString="<%$ ConnectionStrings:joinin_projectConnectionString %>" SelectCommand="SELECT [A_NAME], [A_DESCRIPTION], [A_START], [A_END] FROM [ACTIVITY] WHERE ([U_ID] = @U_ID)">
                    <SelectParameters>
                        <asp:SessionParameter Name="U_ID" SessionField="id" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <a runat="server" href="~/CreateActivity" class="btn btn-primary">Create new activity </a>
            </div>

                                <br />

                            </div>
                            <div class="tab-pane fade" id="tab1-item4History">
                                
                                <div runat="server" id="Attended">

                                    <br />
                                </div>

                                <br />

                            </div>

                        </div>

                </div>
            </div><!--/#table-container-->  

        </div>        
</form>
</asp:Content>