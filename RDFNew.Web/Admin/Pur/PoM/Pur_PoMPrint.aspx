<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pur_PoMPrint.aspx.cs"
    Inherits="RDFNew.Web.Admin.Pur.PoM.Pur_PoMPrint" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../../../Res/Jscript/jquery-1.6.min.js"></script>

</head>
<body style="background-color: #ccc;" scroll="auto">
    <form id="form1" runat="server">
    <div id="rptMain">
        <cc1:webreport id="WebReport1" runat="server" zoom="0.65" AutoWidth="false" AutoHeight="false"
            localizationfile="~/Chinese (Simplified).frl" toolbarcolor="Lavender" printinpdf="False"
            pdfembeddingfonts="True" layers="False" />
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function() {
        setPadding();
        $(window).resize(function() {
            setPadding();
        });
    });

    function setPadding() {
        var pd = ($(window).width() - $("#WebReport1").width()) / 2;
        $("#rptMain").css("padding-left", pd);
    }
</script>