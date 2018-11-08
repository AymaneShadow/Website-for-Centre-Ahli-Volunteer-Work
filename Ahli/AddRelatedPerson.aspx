<%@ Page Title="Add Related Person" Language="C#" MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="AddRelatedPerson.aspx.cs" Inherits="AddRelatedPerson" %>


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

            $('#educationLevelForm').keyup(function () {
                var max = 100;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#educationLevelHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#educationLevelHelpBlock').text(char);
                }
            });


            $('#relationshipTypeForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#relationshipTypeHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#relationshipTypeHelpBlock').text(char);
                }
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


            $('#professionForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#professionHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#professionHelpBlock').text(char);
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

            $('#marriedToForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#marriedToHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#marriedToHelpBlock').text(char);
                }
            });

            $('#medicalStateForm').keyup(function () {
                var max = 3000;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#medicalStateHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#medicalStateHelpBlock').text(char);
                }
            });

            $('#extraInfoForm').keyup(function () {
                var max = 4000;
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
                            <h1 class="title" id="whichChild" runat="server">Add a related person to a child</h1>
                            <p>Please start by choosing a child then enter the related person's details:</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <br />

    <section id="shortcodes">
        <div class="container" id="relatedChilContainer" runat="server">

                <div class="row">
                    <div class="col-md-12">
                                                     
                            <div class="row">         
                                <div class="col-xs-4">                                                                           
                                    <div runat="server" class="bfh-selectbox" id="childList" data-name="pickedChild" data-filter="true" data-icon="glyphicon glyphicon-chevron-down"> <!--data-input="form-control input-lg"-->                                         
                                    </div>
                                    <span id="pickChildHelpBlock" class="help-block" style="float:right">Choose a child</span>
                                </div>
                                <div class="col-xs-3">
                                    <asp:Button runat="server" type="submit" name="Button" class="btn btn-info" Text="Continue" ID="pickedChildButton" OnClick="pickedChild_Submit_Click" />
                                </div>                                        
                            </div>
                            
                            <div class="row">   
                                <br />
                            </div>
                        </div>
                    </div>
                </div>

        <div class="container" id="relatedPersonContainer" runat="server" visible="false">

            <div id="tab-container">
                <div class="row">
                    <div class="col-md-6">

                        <input type="hidden" id="hiddenChildIDInput" runat="server"/>

                        <asp:Panel class="alert alert-success fade in" runat="server" ID="successMessage" Visible="false">                            
                            <h4><b>Success!</b></h4>
                            <h4>The related person's information was added to the database.</h4>
                            <h4> To view this related person's new information page click <a href="~/RelatedPersonProfile" runat="server" class="btn btn-xs btn-success">here</a></h4>
                            <h4> To add another related person's information click <a href="~/AddRelatedPerson" runat="server" class="btn btn-xs btn-info">here</a></h4>
                        </asp:Panel>

                        <asp:Panel class="alert alert-danger fade in" runat="server" ID="errorMessage" Visible="false">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4><b>Error.</b></h4>
                            <p>A problem occured, the related person's info wasn't added to the database.</p>
                        </asp:Panel>

                    <asp:Panel id="addRelatedPersonForm" runat="server">                        
                            <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <input id="firstNameForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="First Name *" ClientIDMode="Static" />
                                    <span id="firstNameHelpBlock" class="help-block" style="float:right">50</span>
                                </div>                            
                                <div class="col-xs-6">
                                    <input id="lastNameForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="Last Name *" ClientIDMode="Static" />
                                    <span id="lastNameHelpBlock" class="help-block" style="float:right">50</span>
                                </div>
                            </div>
                            </div>
                                
                            <div class="row">                                
                                    <div class="col-xs-6">

                                    <div class="form-group form-group-lg">                                        
                                        <asp:DropDownList ID="genderForm" runat="server" CssClass="form-control">
                                          <asp:ListItem Selected="True" Value="0" Text="Male"></asp:ListItem>
                                          <asp:ListItem Selected="False" Value="1" Text="Female"></asp:ListItem>
                                        </asp:DropDownList>                                                                                                                        
                                        <span id="genderHelpBlock" class="help-block" style="float:right">Gender</span>
                                    </div>

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
                                        <input id="addressForm" runat="server" maxlength="100" type="text" class="form-control input-lg" placeholder="Home address" ClientIDMode="Static" />
                                        <span id="addressHelpBlock" class="help-block" style="float:right">100</span>
                                  </div>

                                  <div class="col-xs-6">
                                        <input id="educationLevelForm" runat="server" maxlength="100" type="text" class="form-control input-lg" placeholder="Education Level" ClientIDMode="Static" />
                                        <span id="educationLevelHelpBlock" class="help-block" style="float:right">100</span>
                                  </div>
                            </div>
                        </div>      

                            <div class="row">
                            <div class="form-group">
                                <div class="col-xs-6">
                                    <input id="relationshipTypeForm" runat="server" maxlength="50" type="text" class="form-control input-lg" required="required" placeholder="Kind of relationship to the child *" ClientIDMode="Static" />
                                    <span id="relationshipTypeHelpBlock" class="help-block" style="float:right">50</span>
                                </div>                            
                                <div class="col-xs-6">
                                    <input id="professionForm" runat="server" maxlength="50" type="text" class="form-control input-lg" placeholder="Profession" ClientIDMode="Static" />
                                    <span id="professionHelpBlock" class="help-block" style="float:right">50</span>
                                </div>
                            </div>
                            </div>

                        <div class="row">
                            <div class="form-group form-group-lg">
                                <div class="col-xs-6">
                                    <input id="phoneNumberForm" runat="server" maxlength="50" type="text" data-format="+212 d dd dd dd dd" class="form-control bfh-phone" required="required" ClientIDMode="Static" />
                                    <span id="phoneNumberHelpBlock" class="help-block" style="float:right">The related person's phone number</span>
                                </div>
                                <div class="col-xs-6">
                                    <input id="marriedToForm" runat="server" maxlength="50" type="text" class="form-control input-lg" placeholder="If married, to whom?" ClientIDMode="Static" />
                                    <span id="marriedToHelpBlock" class="help-block" style="float:right">50</span>
                                </div>
                            </div>
                        </div>

                            <div class="form-group">
                                <textarea id="medicalStateForm" runat="server" maxlength="3000" rows="6" class="form-control input-lg" placeholder="Medical State" ClientIDMode="Static"/>
                                <span id="medicalStateHelpBlock" class="help-block" style="float:right">3000</span>
                            </div>

                            <div class="form-group">
                                <textarea id="extraInfoForm" runat="server" maxlength="4000" rows="6" class="form-control input-lg" placeholder="Extra information" ClientIDMode="Static"/>
                                <span id="extraInfoHelpBlock" class="help-block" style="float:right">4000</span>
                            </div>

                            <div class="form-group">
                                <span id="lastHelpBlock" class="help-block">Forms with a star (*) are mandatory.</span>
                                <asp:Button runat="server" type="submit" name="Button" class="btn btn-submit" Text="Add Related Person" ID="LoginButton" OnClick="Submit_Click" />
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