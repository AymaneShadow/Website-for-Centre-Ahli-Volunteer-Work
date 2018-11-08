<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <section id="home-slider">
        <div class="container">
            <div class="row">
                <div class="main-slider">
                    <div class="slide-text">
                        <h1>Welcome to Ahli Center</h1>
                        <p>The establishment welcomes boys in precarious situations whose ages vary between 9 and 14 years. The center's mission is to take care of these children in order to reintegrate them into their cultural, familial and social environment.</p>
                        <a href="~/About" class="btn btn-common" runat="server">Learn More</a>
                    </div>
                    <img src="images/home/slider/mosque3.png" class="slider-hill" alt="slider image">
                    <img src="images/home/slider/house.png" class="slider-house" alt="slider image">
                    <img src="images/home/slider/sun.png" class="slider-sun" alt="slider image">
                    <img src="images/home/slider/birds1.png" class="slider-birds1" alt="slider image">
                    <img src="images/home/slider/birds2.png" class="slider-birds2" alt="slider image">
                </div>
            </div>
        </div>
        <div class="preloader"><i class="fa fa-sun-o fa-spin"></i></div>
    </section>
    <!--/#home-slider-->


    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Presentation Video</h1>
                            <p>Check out our presentation video made by one of our coolest volunteers, <a href="~/Profile?u_id=2" target="_blank" runat="server">Enas Nader Ismail</a>:</p>
                        </div>
                     </div>
                </div>
            </div>
        </div>
   </section>
    <!--/#page-breadcrumb-->

    <div class="container">
            <div id="video-container">

                    <div class="col-md-12">
                        <br />
                        <iframe src="https://www.youtube.com/embed/j77Zdc7JiAI" ></iframe>
                        <br />
                    </div>

            </div><!--/#video-container-->
    </div>

    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Latest Activities</h1>
                            <p>Here are the latest activities that were held in our center.</p>
                        </div>
                     </div>
                </div>
            </div>
        </div>
   </section>
    <!--/#page-breadcrumb-->


    <section id="portfolio">
        <div class="container">
            <div class="row">
                <ul class="portfolio-filter text-center" id="portfolioFilter" runat="server">
                    <li><a class="btn btn-default active" href="#" data-filter="*">All</a></li>                    

                </ul><!--/#portfolio-filter-->
                    
                <div runat="server" class="portfolio-items" id="portfolioItems">

                </div>

                <div class="portfolio-pagination">
                    <ul class="pagination">
                      <li><a href="#">left</a></li>
                      <li><a href="#">1</a></li>
                      <li><a href="#">2</a></li>
                      <li class="active"><a href="#">3</a></li>
                      <li><a href="#">4</a></li>
                      <li><a href="#">5</a></li>
                      <li><a href="#">6</a></li>
                      <li><a href="#">7</a></li>
                      <li><a href="#">8</a></li>
                      <li><a href="#">9</a></li>
                      <li><a href="#">right</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </section>
    <!--/#portfolio-->


</asp:Content>
