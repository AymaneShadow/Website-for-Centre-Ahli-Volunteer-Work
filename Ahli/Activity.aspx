<%@ Page Title="Activity" Language="C#" MasterPageFile="~/SiteWOform.Master"  AutoEventWireup="true" CodeFile="Activity.aspx.cs" Inherits="Activity" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="headerBottom" runat="server">

    <script type="text/javascript">var switchTo5x = true;</script>
    <script type="text/javascript" src="http://w.sharethis.com/button/buttons.js"></script>
    <script type="text/javascript">stLight.options({ publisher: "7e8eb33b-fbe0-4915-9b93-09490e3d10df", doNotHash: false, doNotCopy: false, hashAddressBar: false });</script>

<style>

.addCommentBtn
{
   margin-bottom: 20px;
}

</style>

</asp:Content>

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


    <script>
        $(document).ready(function () {

            $('.deleteIcon').on('click touchstart', function (e) {

                $(this).closest('.post-comment').hide('slow', function () { $(this).remove(); });

                e.preventDefault();

                url = $(this).attr('href');

                var abc = $(this).attr('href');
                var ajaxParam = {
                    val: abc,
                    format: 'raw'
                };

                //$('#comment').val(ajaxParam.val);

                $.get(url, ajaxParam, function (result) {

                });
                

                return false;

            });


            $('#submitCommentButton').on('click touchstart', function (e) {

                e.preventDefault();

                var abc = "Add_Comment.aspx" + window.location.search + "&commentToAdd=" + $('#comment').val() + "&replyToCommentID=NULL";                

                imgSrc = $('#connectedAvatar').attr('src');

                username = "<%= ((SiteWOform)this.Master).username %>";

                if ($('#comment').val() != "") {
                    $.ajax({
                        url: "Add_Comment.aspx",
                        type: "get", //send it through post method
                        data: {
                            a_id: $.urlParam('a_id'),
                            commentToAdd: $('#comment').val(),
                            replyToCommentID: "NULL"
                        },
                        success: function (response) {

                            /*

                            var $newComSuccessBox = jQuery('<p> </p>' +
                                '<div ID="successCommentAddedBox" class="alert alert-success fade in">' +
                                '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>' +
                                '<p><b>Success</b></p>' +
                                '<p>Comment added.</p>' +
                                '</div>');
                            
                            $('#commentBoxesUnderAddCommentButton').html($newComSuccessBox).fadeIn(1000);   
                            
                            */                         

                            var $newCom = jQuery("<div class=\"post-comment\">" +
                                "<a class=\"pull-left\" href=\"#\">" +
                                "<img width=\"127px\" class=\"media-object\" src=\"" + imgSrc + " \" alt=\"" + username + "\">" +
                                "</a>" +
                                "<div class=\"media-body\">" +
                                "<span><i class=\"fa fa-user\"></i>Posted by <a href=\"#\">" + username + "</a></span>" +
                                "<p>" + $('#comment').val() + "</p>" +
                                "<ul class=\"nav navbar-nav post-nav\">" +
                                "<li><a href=\"#\"><i class=\"fa fa-clock-o\"></i>Just now.</a></li>" +
                                "</ul>" +
                                "</div>" +
                                "</div>");

                            $newCom.prependTo('#commentsContainer').hide().fadeIn(1000);

                            $('#comment').val('');
                            $('#noCommentsToDisplay').hide();
                            $('#commentTxtDiv').removeClass('has-danger');
                            $('#commentTxtDiv').removeClass('has-warning');
                        },
                        error: function (xhr) {

                            var $newComDangerBox = jQuery('<p> </p>' +
                                '<div ID="dangerCommentAddedBox" class="alert alert-danger fade in">' +
                                '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>' +
                                '<p><b>Failure</b></p>' +
                                '<p>Couldn\'t add comment.</p>' +
                                '</div>');
                            
                            $('#commentBoxesUnderAddCommentButton').html($newComDangerBox).fadeIn(1000);

                            $('#commentTxtDiv').removeClass('has-warning');
                            $('#commentTxtDiv').addClass('has-danger');
                        }
                    });

                } else
                {
                    /*

                    var $newComDangerBox = jQuery('<p> </p>' +
                        '<div ID="warningCommentAddedBox" class="alert alert-warning fade in">' +
                        '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>' +
                        '<p><b>Woops</b></p>' +
                        '<p>Write something.</p>' +
                        '</div>');

                    $('#commentBoxesUnderAddCommentButton').html($newComDangerBox).fadeIn(1000);

                    */

                    $('#comment').focus();
                    $('#commentTxtDiv').addClass('has-warning');
                }

                return true;

            });

            $.urlParam = function (name) {
                var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.search);
                return results[1] || 0;
            }

        });
</script>

<div class="container" id="activityContainer" runat="server">

    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title" ID="activityTitleContainer" runat="server"></h1>
                            <p>Check out this activity details bellow</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!--/#action-->

    <section id="blog-details" class="padding-top">
        <div class="container">
            <div class="row">
                <div class="col-md-9 col-sm-7">
                    <div class="row">
                         <div class="col-md-12 col-sm-12">
                            <div class="single-blog blog-details two-column">
                                <div class="post-thumb">                                    
                                    <a href="#"><asp:Panel id="activityImageContainer" alt="" CssClass="img-responsive" runat="server" ClientIDMode="Static" /></a>

                                    <div class="post-overlay">
                                        <span class="uppercase"><a href="#" id="triangleDate" runat="server"></a></span>
                                    </div>
                                </div>

                                <div class="post-content overflow">
                                    <%-- <h2 class="post-title bold"><a href="#" ID="activityTitleContainer" runat="server"></a></h2> --%>

                                      <asp:Panel ID="activityDescriptionContainer" runat="server">
                                      </asp:Panel>

                                    <div class="post-bottom overflow">
                                        <ul class="nav navbar-nav post-nav">
                                            <li ID="categoryContainer" runat="server"/>
                                            <li ID="subcategoryContainer" runat="server"/>
                                            <li><a href="#" id="numberOfComments" runat="server"></a></li>
                                        </ul>
                                    </div>

                                <h2 class="page-header">Location</h2>  
                                <div class="col-md-12 col-sm-4">                                                                             
                                    <div class="col-md-4 col-sm-4">
                                        <div ID="imgCityContainer" runat="server">  
                                        </div>
                                    </div>                        
                                    <!-- style="border-right:1px solid #eee;height:100%" -->

                                    <div class="col-md-4">
                                        <blockquote>                            
                                            <p><b>Location name: </b></p>
                                            <p ID="activityLocationNameContainer" runat="server"></p>
                                        </blockquote>
                                    </div>

                                    <div class="col-md-4">
                                        <blockquote>
                                            <p><b>Address: </b></p>
                                            <p ID="addressContainer" runat="server">Address: </p>
                                        </blockquote>
                                    </div>
                                    <div class="col-md-12 col-sm-12">
                                        <p> </p>
                                    </div>
                                </div>

                                        <h2 class="page-header">Date</h2>

                                        <div class="col-md-12 col-sm-5">                                                                                        

                                                <div class="col-md-5 col-sm-5" ID="dateContainer" runat="server">
                                        
                                                </div>

                                                <div class="col-md-2 col-sm-2">

                                                    <h3>To</h3>

                                                </div>

                                                <div class="col-md-5 col-sm-5" ID="dateContainer2" runat="server">                                        

                                                </div>
                                            </div>


            <div class="feature-container"> <!-- need to modify JS for this, it had id="feature-container" instead of class="" -->
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="page-header">Participating volunteers</h2>                    
                        <div id="volunteersRepeater" runat="server"> 
                        </div>
                    </div>
                </div>
            </div><!--/#feature-container-->

<h2 class="page-header">Volunteers Attendance</h2>
<asp:Panel ID="volunteersAttendance" runat="server" class="col-md-12 col-sm-12">

            <div id="progressbar-container">

                <div class="progress">
                    <div runat="server" id="progressionBar" class="progress-bar progress-bar-success" role="progressbar" aria-valuemin="0">                        
                    </div>
                </div>
            </div><!--/#progressbar-container-->
              
</asp:Panel>

            <div class="feature-container"> <!-- need to modify JS for this, it had id="feature-container" instead of class="" -->
                    <h2 class="page-header">Participating children</h2>
                <div class="row">
                    <div class="col-md-12">                        
                        <div id="childrenRepeater" runat="server"> 
                        </div>
                    </div>
                </div>
            </div><!--/#feature-container-->

<h2 class="page-header">Children Attendance</h2>
<asp:Panel ID="childrenAttendance" runat="server" class="col-md-12 col-sm-12">
            <div id="progressbar-container">               
                <div class="progress">
                    <div runat="server" id="childrenAttendanceProgressionBar" class="progress-bar progress-bar-info" role="progressbar" aria-valuemin="0">                        
                    </div>
                </div>
            </div><!--/#progressbar-container-->

</asp:Panel>

            <br />

            <div id="progressbar-container">
                <h2 class="page-header">Share</h2>
            </div><!--/#progressbar-container-->
                                    
                                        <span class='st_facebook_hcount'></span>
                                        <span class='st_twitter_hcount'></span>
                                        <span class='st_linkedin_hcount'></span>
                                        <span class='st_pinterest_hcount'></span>
                                        <span class='st_email_hcount'></span>   


                                    <div class="response-area">                                    
                                    <div class="author-profile padding">
                                        <h2 class="page-header">Activity organizer</h2>
                                        <div class="row">
                                            <div class="col-sm-2">
                                                 <asp:Panel ID="imgContainer" runat="server"/>
                                            </div>
                                            <div class="col-sm-10">
                                                <h3><asp:Panel ID="organiserNameContainer" runat="server"/></h3>
                                                <p><asp:Panel ID="aboutContainer" runat="server"/></p>                                                
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="col-sm-12" id="commentsBox" runat="server">

                                    <h2 class="page-header">Comments</h2>

                                    <div class="row">
                                    <div class="col-sm-2"> 
                                        <asp:Panel id="comment_form" runat="server" ClientIDMode="Static">                                    
                                        </asp:Panel>
                                    </div>

                                    <div class="col-sm-8" id="commentTxtDiv">                                    
                                        <asp:TextBox MaxLength="3000" CssClass="form-control" TextMode="multiline" rows="4" name="comment" id="comment" placeholder="What's on your mind?" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
                                        <br />
		                                
                                    </div>
                                    <div class="col-sm-2">                                        
                                        <asp:Button CssClass="btn btn-info addCommentBtn" type="submit" Text="Add Comment" runat="server" ID="submitCommentButton" ClientIDMode="Static" Visible="false" />
                                        <div id="commentBoxesUnderAddCommentButton">
                                        </div>
                                    </div>
                                        </div>

                                     
  

                                    <ul class="media-list">
                                     <li class="media" runat="server" id="commentsContainer" ClientIDMode="Static"></li>                                      
                                    </ul>     
                                </div>

                                </div><!--/Response-area-->
                                </div>
                            </div>
                        </div>
                    </div>
                 </div>
                <asp:Panel class="col-md-3 col-sm-5" runat="server">
                    <div class="sidebar blog-sidebar">
                        <div class="sidebar-item recent">

                    <asp:Panel ID="addActivityDiv" runat="server" class="w3-container" Visible="false">      

                        <h3>Add to coming-up activities</h3>                        

                        <asp:Panel ID="warningBox" class="alert alert-warning fade in" runat="server" Visible="false" ClientIDMode="Static">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4>Oh snap! You got an error!</h4>
                            <p>The event is already full, there are no more spots left :(</p>
                        </asp:Panel>

                        <asp:Panel ID="successBox" class="alert alert-success fade in" runat="server" Visible="false" ClientIDMode="Static">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4>Added!</h4>
                            <p>The event was added to your up-coming list!</p>
                        </asp:Panel>

                        <asp:Panel ID="noticeBox" class="alert alert-info fade in" runat="server" Visible="false" ClientIDMode="Static">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">X</button>
                            <h4>Removed</h4>
                            <p>The event was removed from your up-coming list.</p>
                        </asp:Panel>

                        <asp:Panel ID="doesntExist" class="alert alert-danger fade in" runat="server" Visible="false" ClientIDMode="Static">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">x</button>
                            <h4>Error 404: Activity Not Found.</h4>
                            <p>The activity you're looking for does not exist.</p>
                        </asp:Panel>

                        <div runat="server" id = "leftColumn">
                                <asp:Button type="submit" Text="Add this activity" runat="server" ID="addToUserActivityButton" ClientIDMode="Static" OnClick="addToUserActivityButton_Click" Visible="false" CssClass="btn btn-sm btn-success" />
                                <asp:Button type="submit" Text="Remove this activity" runat="server" ID="removeFromUserActivityButton" ClientIDMode="Static" OnClick="removeFromUserActivityButton_Click" Visible="false" CssClass="btn btn-sm btn-warning" />
                            <hr />
                        </div>
                    </asp:Panel>                                                                

                           <asp:Panel ID="deleteActivityFromDBDiv" runat="server" class="w3-container" Visible="false">      
                            <h3>Delete activity</h3>
                              <asp:Button type="submit" Text="Delete this activity" runat="server" ID="deleteActivityButton" ClientIDMode="Static" OnClick="deleteActivityButton_Click" CssClass="btn btn-sm btn-danger" />
                               <hr />
                            </asp:Panel>


                            <h3>Comments</h3>
                            <div class="media">
                                <div class="pull-left">
                                    <a href="#"><img src="images/portfolio/project1.jpg" alt=""></a>
                                </div>
                                <div class="media-body">
                                    <h4><a href="#">Lorem ipsum dolor sit amet consectetur adipisicing elit,</a></h4>
                                    <p>posted on  07 March 2014</p>
                                </div>
                            </div>
                            <div class="media">
                                <div class="pull-left">
                                    <a href="#"><img src="images/portfolio/project2.jpg" alt=""></a>
                                </div>
                                <div class="media-body">
                                    <h4><a href="#">Lorem ipsum dolor sit amet consectetur adipisicing elit,</a></h4>
                                    <p>posted on  07 March 2014</p>
                                </div>
                            </div>
                            <div class="media">
                                <div class="pull-left">
                                    <a href="#"><img src="images/portfolio/project3.jpg" alt=""></a>
                                </div>
                                <div class="media-body">
                                    <h4><a href="#">Lorem ipsum dolor sit amet consectetur adipisicing elit,</a></h4>
                                    <p>posted on  07 March 2014</p>
                                </div>
                            </div>
                        </div>
                        <div class="sidebar-item categories">
                            <h3>Categories</h3>
                            <ul class="nav navbar-stacked">
                                <li><a href="#">Lorem ipsum<span class="pull-right">(1)</span></a></li>
                                <li class="active"><a href="#">Dolor sit amet<span class="pull-right">(8)</span></a></li>
                                <li><a href="#">Adipisicing elit<span class="pull-right">(4)</span></a></li>
                                <li><a href="#">Sed do<span class="pull-right">(9)</span></a></li>
                                <li><a href="#">Eiusmod<span class="pull-right">(3)</span></a></li>
                                <li><a href="#">Mockup<span class="pull-right">(4)</span></a></li>
                                <li><a href="#">Ut enim ad minim <span class="pull-right">(2)</span></a></li>
                                <li><a href="#">Veniam, quis nostrud <span class="pull-right">(8)</span></a></li>
                            </ul>
                        </div>
                        <div class="sidebar-item tag-cloud">
                            <h3>Tag Cloud</h3>
                            <ul class="nav nav-pills">
                                <li><a href="#">Corporate</a></li>
                                <li><a href="#">Joomla</a></li>
                                <li><a href="#">Abstract</a></li>
                                <li><a href="#">Creative</a></li>
                                <li><a href="#">Business</a></li>
                                <li><a href="#">Product</a></li>
                            </ul>
                        </div>
                        <div class="sidebar-item popular">
                            <h3>Latest Photos</h3>
                            <ul class="gallery">
                                <li><a href="#"><img src="images/portfolio/popular1.jpg" alt=""></a></li>
                                <li><a href="#"><img src="images/portfolio/popular2.jpg" alt=""></a></li>
                                <li><a href="#"><img src="images/portfolio/popular3.jpg" alt=""></a></li>
                                <li><a href="#"><img src="images/portfolio/popular4.jpg" alt=""></a></li>
                                <li><a href="#"><img src="images/portfolio/popular5.jpg" alt=""></a></li>
                                <li><a href="#"><img src="images/portfolio/popular1.jpg" alt=""></a></li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </section>
    <!--/#blog-->

    </div>
</form>
</asp:Content>

<asp:Content ID="FooterBottomContent" ContentPlaceHolderID="footerBottom" runat="server">

</asp:Content>
