<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ContentPlaceHolderID="ScriptPlaceHolder" runat="server">

    <script type="text/javascript" src="http://maps.google.com/maps/api/js?key=AIzaSyBysJp0zEEUb8GVEPR-_nlbORo5EGk7S_g"></script>
    <script type="text/javascript" src="js/gmaps.js"></script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <section id="page-breadcrumb">
        <div class="vertical-center sun">
             <div class="container">
                <div class="row">
                    <div class="action">
                        <div class="col-sm-12">
                            <h1 class="title">Contact US</h1>
                            <p>Here is our contact details and our location on google maps:</p>
                        </div>                        
                    </div>
                </div>
            </div>
        </div>
   </section>
    <!--/#action-->

    <section id="map-section">
        <div class="container">
            <div id="gmap"></div>
            <div class="contact-info">
                <h2>Contacts</h2>
                <address>
                E-mail: <a href="mailto:someone@example.com">email@email.com</a> <br> 
                Phone: +212 X XX XX XX XX <br> 
                Fax: +212 X XX XX XX XX <br> 
                </address>

                <h2>Address</h2>
                <address>
                KM 2 Route d’Immouzer, <br> 
                Fès, 30050, <br> 
                Maroc <br> 



 
                </address>
            </div>
        </div>
    </section> <!--/#map-section-->        

</asp:Content>
