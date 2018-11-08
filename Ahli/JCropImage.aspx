<%@ Page Title="JCrop" Language="C#" MasterPageFile="~/SiteWOform.Master" AutoEventWireup="true" CodeFile="JCropImage.aspx.cs" Inherits="JCropImage" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="headerBottom" runat="server">

<link rel="stylesheet" href="Jcrop/css/jquery.Jcrop.css" type="text/css" />

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

<script src="Jcrop/js/jquery.Jcrop.js"></script>

<script type="text/javascript">



    jQuery(function ($) {

        var jcrop_api;

        initJcrop();

        function DestroyJC()//{{{
        {
            jcrop_api.destroy();
            $('#y1').val(2);
        };

        function initJcrop() {
            // Invoke Jcrop in typical fashion
            $('#target').Jcrop({
                onChange: showCoords,
                onSelect: showCoords,
                aspectRatio: 280 / 150,
                minSize: [298.65, 198.9],
                onRelease: clearCoords
            }, function () {
                jcrop_api = this;
            });
        };

        var getUrlParameter = function getUrlParameter(sParam) {
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
        };


        if (getUrlParameter("username") != null && getUrlParameter("a_id") == null)
            jcrop_api.setOptions({ aspectRatio: 250 / 250, minSize: [250, 250] });
        //else if (getUrlParameter("username") != null && getUrlParameter("a_id") != null)
        //    $('#comment').val("This is event page");




        $('#coords').on('change', 'input', function (e) {
            var x1 = $('#x1').val(),
                x2 = $('#x2').val(),
                y1 = $('#y1').val(),
                y2 = $('#y2').val();
            jcrop_api.setSelect([x1, y1, x2, y2]);
        });

        $('#x2').click(function (e) {

            $('#y2').val(1);
            initJcrop();

        });

        $('#x1').click(function (e) {

            $('#y1').val(1);
            DestroyJC();

        });

    });

    // Simple event handler, called from onChange and onSelect
    // event handlers, as per the Jcrop invocation above
    function showCoords(c) {
        $('#x1').val(c.x);
        $('#y1').val(c.y);
        $('#x2').val(c.x2);
        $('#y2').val(c.y2);
        $('#w').val(c.w);
        $('#h').val(c.h);
    };

    function clearCoords() {
        $('#coords input').val('');
    };

</script>

<asp:Panel CssClass="container" id="Container" Runat="Server">
<div class="row">

<asp:Panel CssClass="" id="mostInnerContainer" Runat="Server">

    <asp:Panel id="title" Runat="Server">
    </asp:Panel>

    <asp:Panel id="goBack" Runat="Server">
    </asp:Panel>

<br />

  <!-- This is the image we're attaching Jcrop to -->
  <asp:Image src="images/people/profile-generic.jpg" id="target" alt="Target" runat="server" ClientIDMode="Static" />

  <!-- This is the form that our event handler fills -->

    <asp:Panel runat="server" id="labels" style="display: none;"  >
    <%--<asp:Textbox size="4" id="x1" name="x1" runat="server" ClientIDMode="Static" />--%>
    <input  size="4" id="x1" name="x1" runat="server" ClientIDMode="Static" OnClientClick="simulatex1Click();" />
    <input  size="4" id="y1" name="y1" runat="server" ClientIDMode="Static" />
    <input  size="4" id="x2" name="x2" runat="server" ClientIDMode="Static" OnClientClick="simulatex2Click();"  />
    <input  size="4" id="y2" name="y2" runat="server" ClientIDMode="Static" />
    <input  size="4" id="w" name="w" runat="server" ClientIDMode="Static" />
    <input  size="4" id="h" name="h" runat="server" ClientIDMode="Static" />
    </asp:Panel>

    <br>

    <asp:Panel runat="server" id="CropDiv">
        <asp:Button id="Crop" Text="Crop" ClientIDMode="Static" OnClick="Crop_Click" runat="server" />
        <asp:Button id="Delete" Text="Delete" ClientIDMode="Static" OnClick="Delete_Click" runat="server" />   
    </asp:Panel>

    <br>


    <asp:FileUpload type="file" id="File1" ClientIDMode="Static" name="File1" onChange="this.form.submit()" runat="server" />

    <br>
<%--    <asp:Button id="Upload" Text="Upload a new one" OnClick="Upload_Click" runat="server"/>--%>


  <div class="description">
  <p>
    <b>Upload your photo and crop it.</b> Click in the middle of the photo, a rectangle should appear. It has got a minimum size, you can't go bellow it. Select the part you want to have in your photo and then click on Crop. Once you crop your photo it is saved on our server.
  </p>
  </div>

<div class="clearfix"></div>

</asp:Panel>
</div>
</asp:Panel>

</form>
</asp:Content>