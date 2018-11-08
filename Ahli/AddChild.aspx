<%@ Page Title="Add Child" Language="C#" MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="AddChild.aspx.cs" Inherits="AddChild" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="headerBottom" runat="server">

    <link rel="stylesheet" type="text/css" href="https://jonthornton.github.io/jquery-timepicker/jquery.timepicker.css" />
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.5.0/css/bootstrap-datepicker.standalone.css" />
    <!-- Bootstrap Form Helpers -->
    <link href="css/bootstrap-formhelpers.min.css" rel="stylesheet" media="screen">

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<form runat="server" method="post">

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

    <script src="https://jonthornton.github.io/jquery-timepicker/jquery.timepicker.js"></script>    
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.5.0/js/bootstrap-datepicker.js"></script>
    
    <script type="text/javascript" src="js/datepair.js"></script>
    <script type="text/javascript" src="js/jquery.datepair.js"></script>


    <!-- Bootstrap Form Helpers -->
    <script src="js/bootstrap-formhelpers.min.js"></script>

    <script>

        $(document).ready(function () {

            $('#birthdayForm').on('change.bfhdatepicker', function (e) {

                var date = $('#birthdayForm').val();

                $('#birthdayHiddenInput').val(date);

            });

            $('#joinedCenterDateForm').on('change.bfhdatepicker', function (e) {

                var date = $('#joinedCenterDateForm').val();

                $('#joinedCenterDateHiddenInput').val(date);

            });

            $('#firstNameForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#firstNameHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#firstNameHelpBlock').text(char);
                }
            });

            $('#lastNameForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#lastNameHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#lastNameHelpBlock').text(char);
                }
            });


            $('#joinedCenterMethodForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#joinedCenterMethodHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#joinedCenterMethodHelpBlock').text(char);
                }
            });

            $('#addressForm').keyup(function () {
                var max = 100;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#addressHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#addressHelpBlock').text(char);
                }
            });

            $('#joinedCenterReasonForm').keyup(function () {
                var max = 3000;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#joinedCenterReasonHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#joinedCenterReasonHelpBlock').text(char);
                }
            });

            $('#extraInfoForm').keyup(function () {
                var max = 3000;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#extraInfoHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#extraInfoHelpBlock').text(char);
                }
            });

        });

    </script>
    
    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Add a child to the database</h1>
                            <p>Please enter the child's details:</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <br />

    <section id="shortcodes">
        <div class="container">

            <div id="tab-container">
                <div class="row">
                    <div class="col-md-6">


                        <asp:Panel class="alert alert-success fade in" runat="server" ID="successMessage" Visible="false">                            
                            <h4><b>Success!</b></h4>
                            <h4>The child's information was added to the database.</h4>
                            <h4> To view this child's new information page click <a href="~/ChildProfile" runat="server" class="btn btn-xs btn-success">here</a></h4>
                            <h4> To add another child's information click <a href="~/AddChild" runat="server" class="btn btn-xs btn-info">here</a></h4>
                        </asp:Panel>

                        <asp:Panel class="alert alert-danger fade in" runat="server" ID="errorMessage" Visible="false">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4><b>Error.</b></h4>
                            <p>A problem occured, the child's info wasn't added to the database.</p>
                        </asp:Panel>

                    <asp:Panel id="addChildForm" runat="server">                        
                            <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <input id="firstNameForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="First Name" ClientIDMode="Static" />
                                    <span id="firstNameHelpBlock" class="help-block" style="float:right">50</span>
                                </div>                            
                                <div class="col-xs-6">
                                    <input id="lastNameForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="Last Name" ClientIDMode="Static" />
                                    <span id="lastNameHelpBlock" class="help-block" style="float:right">50</span>
                                </div>
                            </div>
                            </div>
                                
                            <div class="row">                                
                                    <div class="col-xs-6">
                                     <fieldset disabled>
                                    <div class="form-group form-group-lg">                                        
                                        <asp:DropDownList ID="genderForm" runat="server" CssClass="form-control">
                                          <asp:ListItem Selected="True" Value="0" Text="Male"></asp:ListItem>
                                          <asp:ListItem Selected="False" Value="1" Text="Female"></asp:ListItem>
                                        </asp:DropDownList>                                                                                                                        
                                        <span id="genderHelpBlock" class="help-block" style="float:right">Gender</span>
                                    </div>
                                    </fieldset>
                                    </div>
                                

                                        <div class="col-xs-6">
                                            <div class="form-group form-group-lg">
                                                <div id="birthdayForm" runat="server" class="bfh-datepicker" data-format="y-m-d" data-date="today" required="required" ClientIDMode="Static">
                                                </div>
                                                <input type="hidden" id="birthdayHiddenInput" runat="server" ClientIDMode="Static"/>
                                                <span id="birthdayHelpBlock" class="help-block" style="float:right">Birthday</span>
                                             </div>
                                        </div>
                           </div>
                        <div class="row">
                            <div class="form-group form-group-lg">
                                  <div class="col-xs-6">
                                        <input id="addressForm" runat="server" maxlength="100" type="text" class="form-control input-lg" required="required" placeholder="Home address" ClientIDMode="Static" />
                                        <span id="addressHelpBlock" class="help-block" style="float:right">100</span>
                                  </div>
                                  <div class="col-xs-6">
                                        <asp:DropDownList CssClass="form-control" ID="birthPlaceForm" runat="server">                                            
                                        </asp:DropDownList>
                                        <span id="birthPlaceHelpBlock" class="help-block" style="float:right" >Birth place</span>
                                  </div>
                            </div>
                        </div>      

                        <div class="row">
                            <div class="form-group form-group-lg">
                                <div class="col-xs-6">
                                    <input id="phoneNumberForm" runat="server" maxlength="50" type="text" data-format="+212 d dd dd dd dd" class="form-control bfh-phone" required="required" ClientIDMode="Static" />                                
                                    <span id="phoneNumberHelpBlock" class="help-block" style="float:right">The child's phone number</span>
                                </div>
                                <div class="col-xs-6">
                                        <div id="joinedCenterDateForm" runat="server" class="bfh-datepicker" data-format="y-m-d" data-date="today" required="required" ClientIDMode="Static">
                                        </div>
                                        <input type="hidden" id="joinedCenterDateHiddenInput" runat="server" ClientIDMode="Static"/>
                                        <span id="joinedCenterHelpBlock" class="help-block" style="float:right">Child's joined center date</span>
                                </div>
                            </div>
                        </div>
                                    <div class="form-group form-group-lg">                                
                                        <input id="joinedCenterMethodForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="Method of joining the center" ClientIDMode="Static" />
                                        <span id="joinedCenterMethodHelpBlock" class="help-block" style="float:right">50</span>
                                    </div>

                            <div class="form-group">
                                <textarea id="joinedCenterReasonForm" runat="server" maxlength="3000" rows="6" class="form-control input-lg" required="required" placeholder="Reason of joining the center" ClientIDMode="Static"/>
                                <span id="joinedCenterReasonHelpBlock" class="help-block" style="float:right">3000</span>
                            </div>

                            <div class="form-group">
                                <textarea id="extraInfoForm" runat="server" maxlength="3000" rows="6" class="form-control input-lg" required="required" placeholder="Extra information" ClientIDMode="Static"/>
                                <span id="extraInfoHelpBlock" class="help-block" style="float:right">3000</span>
                            </div>

                            <div class="form-group">
                                <asp:Button runat="server" type="submit" name="Button" class="btn btn-submit" Text="Add Child" ID="LoginButton" OnClick="Submit_Click" />                                
                            </div>
                    </asp:Panel>

                    </div>

                    <div class="col-md-6">


                    </div>
                </div>
            </div><!--/#table-container-->

            <div class="padding"></div>

        </div>
    </section>           

</form>
</asp:Content>

<asp:Content ID="FooterBottomContent" ContentPlaceHolderID="footerBottom" runat="server">

</asp:Content>