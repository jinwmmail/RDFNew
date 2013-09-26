<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_Designer.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.Designer.Flow_Designer" %>

<%@ Register Assembly="dswbu" Namespace="DS.Web.UI" TagPrefix="ds" %>
<%@ Register Assembly="DS.XBPM.Design.Web" Namespace="DS.XBPM.Design.Web" TagPrefix="ds" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form
        {
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">    
    <ds:XScriptManager ID="XScriptManager1" runat="server">
    </ds:XScriptManager>
    <ds:WorkflowDesigner runat="server" ID="WorkflowDesigner1" Width="100%" Height="100%"
        FolderPath="~/_PDLFolder" OnSavingFile="WorkflowDesigner1_SavingFile">
    </ds:WorkflowDesigner>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

<script type="text/javascript">
    function onReady() {

    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

