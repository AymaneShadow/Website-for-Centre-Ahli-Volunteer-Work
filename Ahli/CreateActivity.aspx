<%@ Page EnableEventValidation="false" Title="Create Activity" Language="C#"  MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="CreateActivity.aspx.cs" Inherits="CreateActivity" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="headerBottom" runat="server">

    <link rel="stylesheet" type="text/css" href="https://jonthornton.github.io/jquery-timepicker/jquery.timepicker.css" />
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.5.0/css/bootstrap-datepicker.standalone.css" />
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
    <script src="js/multi-select.js"></script>

    <script>

        $(document).ready(function () {

            $('#childrenList').multiselect();

            // initialize input widgets first
            $('#datepairExample .time').timepicker({
                'showDuration': true,
                'timeFormat': 'g:ia'
            });

            $('#datepairExample .date').datepicker({
                'format': 'yyyy-m-d',
                'autoclose': true
            });

            // initialize datepair
            $('#datepairExample').datepair();


            function countChars(val) {
                var len = val.value.length;
                if (len >= 3000) {
                    val.value = val.value.substring(0, 3000);
                } else {
                    $('#descriptionHelpBlock').text(3000 - len);
                    alert("hello");
                }
            };

            $('#nameForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#nameHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#nameHelpBlock').text(char);
                }
            });


            $('#descriptionForm').keyup(function () {
                var max = 3000;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#descriptionHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#descriptionHelpBlock').text(char);
                }
            });

            $('#localityNameForm').keyup(function () {
                var max = 50;
                var len = $(this).val().length;
                if (len >= max) {
                    $('#localityNameHelpBlock').text('You have reached the limit.');
                } else {
                    var char = max - len;
                    $('#localityNameHelpBlock').text(char);
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


        });

    </script>
   
    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Create an activity</h1>
                            <p>Please enter your activity details:</p>
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
                            <h4>The activity was added to the database.</h4>
                            <h4> To view this activity's new page click <a id="successHref" runat="server" class="btn btn-xs btn-success">here</a></h4>
                            <h4> To add another activity click <a href="~/CreateActivity" runat="server" class="btn btn-xs btn-info">here</a></h4>
                        </asp:Panel>

                        <asp:Panel class="alert alert-danger fade in" runat="server" ID="errorMessage" Visible="false">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4><b>Error.</b></h4>
                            <p>A problem occured, the activity wasn't added to the database.</p>
                        </asp:Panel>

                    <asp:Panel id="createActivityForm" runat="server">
                            <div class="form-group">
                                <input runat="server" maxlength="50" type="text" name="name" class="form-control input-lg" required="required" placeholder="Name" id="nameForm" ClientIDMode="Static" />
                                <span id="nameHelpBlock" class="help-block" style="float:right">50</span>
                            </div>
                            <div class="form-group">
                                <textarea runat="server" maxlength="3000" rows="6" name="description" class="form-control input-lg" required="required" placeholder="Description" id="descriptionForm" ClientIDMode="Static"/>
                                <span id="descriptionHelpBlock" class="help-block" style="float:right">3000</span>
                            </div>
                            <div class="form-group form-group-lg">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="cityList"></asp:DropDownList>
                                <span id="cityListHelpBlock" class="help-block" style="float:right">City of the activity</span>
                            </div>
                            <div class="form-group form-group-lg">
                                <input runat="server" type="text" name="Locality" class="form-control" required="required" placeholder="Location Name" id="localityNameForm" ClientIDMode="Static"/>                                
                                <span id="localityNameHelpBlock" class="help-block" style="float:right">50</span>
                            </div>
                            <div class="form-group form-group-lg">
                                <input runat="server" maxlength="100" type="text" name="IdentifierLogin" class="form-control" required="required" placeholder="Location Address" id="addressForm" ClientIDMode="Static"/>                                
                                <span id="addressHelpBlock" class="help-block" style="float:right">100</span>
                            </div>

                            
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group form-group-lg">                                
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="categoryList"></asp:DropDownList>
                                        <span id="categoryListHelpBlock" class="help-block" style="float:right">Category of the activity</span>
                                    </div>
                                </div>
                            </div>

                        <div class="row">
                            <div class="form-group form-group-lg">
                                <div class="col-xs-12">
                                    <input runat="server" type="text" min="1" name="Capacity" class="form-control bfh-number" required="required" placeholder="Number of allowed volunteers" id="capacityForm" />
                                    <span id="capacityHelpBlock" class="help-block" style="float:right">Number of all volunteers that can come</span>
                                 </div>
                            </div>         
                         </div>

                            <div class="row">
                                <div class="col-xs-5">
                                    <asp:ListBox multiple="true" class="form-control input-lg" Rows="13" id="childrenList" runat="server" ClientIDMode="Static" >
                                    </asp:ListBox>
                                    <span id="childrenInActivityHelpBlock" class="help-block" style="float:right">Children not attending</span>
                                </div>
                                <div class="col-xs-2" style="text-align:center">

					                <button type="button" id="childrenList_undo" class="btn btn-danger btn-block">undo</button>
					                <button type="button" id="childrenList_rightAll" class="btn btn-default btn-block"><i class="glyphicon glyphicon-forward"></i></button>
					                <button type="button" id="childrenList_rightSelected" class="btn btn-default btn-block"><i class="glyphicon glyphicon-chevron-right"></i></button>
					                <button type="button" id="childrenList_leftSelected" class="btn btn-default btn-block"><i class="glyphicon glyphicon-chevron-left"></i></button>
					                <button type="button" id="childrenList_leftAll" class="btn btn-default btn-block"><i class="glyphicon glyphicon-backward"></i></button>
					                <button type="button" id="childrenList_redo" class="btn btn-warning btn-block">redo</button>

                                </div>
                                <div class="col-xs-5">
                                    <asp:ListBox multiple="true" class="form-control input-lg" Rows="13" id="childrenList_to" name="childrenList_to" runat="server" ClientIDMode="Static" required="required">
                                    </asp:ListBox>
                                    <span id="childrenNotInActivityHelpBlock" class="help-block" style="float:right">Children attending</span>
                                </div>
                            </div>

                            <div id="datepairExample">
                                <div class="row">                                    
                                    <div class="form-group form-group-lg">                                
                                        <div class="col-xs-6">
                                            <input runat="server" type="text" name="startDate" class="date start form-control" required="required" placeholder="Starting date" id="startDateForm" />
                                            <br />
                                        </div>
                                        <div class="col-xs-6">
                                            <input runat="server" type="text" name="startTime" class="time start form-control" required="required" placeholder="Starting Time" id="startTimeForm" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">                         
                                    <div class="form-group form-group-lg">                                        
                                        <div class="col-xs-6">
                                            <input runat="server" type="text" name="endDate" class="date end form-control form-control-lg input-lg" required="required" placeholder="Ending date" id="endDateForm" />
                                            <br />
                                        </div>
                                        <div class="col-xs-6">
                                            <input runat="server" type="text" name="endTime" class="time end form-control form-control-lg input-lg" required="required" placeholder="Ending time" id="endTimeForm" />                                
                                        <br />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="form-group">
                                <asp:Button runat="server" type="submit" name="Button" class="btn btn-submit" Text="Create Activity" ID="LoginButton" OnClick="Submit_Click" />                                
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