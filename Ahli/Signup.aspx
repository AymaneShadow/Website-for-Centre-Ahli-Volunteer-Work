<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Signup.aspx.cs" Inherits="Signup" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>Sign up is coming soon | Ahli Center</title>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/font-awesome.min.css" rel="stylesheet"> 
    <link href="css/main.css" rel="stylesheet">
    <link href="css/responsive.css" rel="stylesheet">

    <!--[if lt IE 9]>
        <script src="js/html5shiv.js"></script>
        <script src="js/respond.min.js"></script>
    <![endif]-->       
    <link rel="shortcut icon" href="images/ico/favicon.ico">
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="images/ico/apple-touch-icon-144-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="images/ico/apple-touch-icon-114-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="images/ico/apple-touch-icon-72-precomposed.png">
    <link rel="apple-touch-icon-precomposed" href="images/ico/apple-touch-icon-57-precomposed.png">
</head><!--/head-->

<body>
    <div class="logo-image">                                
       <a href="index.html"><img class="img-responsive" src="images/logo.png" alt=""> </a> 
    </div>
     <section id="coming-soon">        
         <div class="container">
            <div class="row">
                <div class="col-sm-8 col-sm-offset-2">                    
                    <div class="text-center coming-content">
                        <h1>UNDER CONSTRUCTION</h1>
                        <p>We have been spending long hours in order to launch our new website. 
                           Follow us on<br /> Facebook or Youtube to stay up to date.</p>
                        <div class="social-link">
                            <span><a href="https://www.facebook.com/centre.ahlifes"><i class="fa fa-facebook"></i></a></span>
                            <span><a href="https://www.youtube.com/watch?v=j77Zdc7JiAI&feature=youtu.be"><i class="fa fa-youtube"></i></a></span>
                        </div>
                    </div>                    
                </div>
                <div class="col-sm-12">
                    <div class="time-count">
                        <ul id="countdown">
                            <li class="angle-one">
                                <span class="days time-font">00</span>
                                <p>Days</p>
                            </li>
                            <li class="angle-two">
                                <span class="hours time-font">00</span>
                                <p>Hours</p>
                            </li>
                            <li class="angle-three">
                                <span class="minutes time-font">00</span>
                                <p class="minute">Mins</p>
                            </li>                            
                            <li class="angle-four">
                                <span class="seconds time-font">00</span>
                                <p>Secs</p>
                            </li>               
                        </ul>   
                    </div>
                </div>
            </div>
        </div>       
    </section>

    <section id="coming-soon-footer" class="container">
        <div class="row">
            <div class="col-sm-12">
                <div class="text-center">
                    <p>&copy; Centre Ahli 2018. All Rights Reserved.</p>
                    <p>Website made by <a target="_blank" href="~/Profile?u_id=1" runat="server">AymaneShadow</a></p>
                    <p>Design crafted by <a target="_blank" href="http://designscrazed.org/">Allie</a></p>
                </div>
            </div>
        </div>       
    </section>
    

    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript" src="js/coundown-timer.js"></script>
    <script type="text/javascript" src="js/wow.min.js"></script>
    <script type="text/javascript" src="js/main.js"></script>
    <script type="text/javascript">
        //Countdown js
        $("#countdown").countdown({
            date: "15 january 2018 12:00:00",
            format: "on"
        },
            function () {
                // callback function
            });
    </script>
    
</body>
</html>