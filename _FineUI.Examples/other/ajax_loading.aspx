﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ajax_loading.aspx.cs" Inherits="FineUI.Examples.other.ajax_loading" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" AjaxLoadingType="Mask" runat="server" />
    <x:Button Text="不显示提示信息" EnableAjaxLoading="false" runat="server" ID="Button1"
        OnClick="Button1_Click">
    </x:Button>
    <br />
    <x:Button Text="使用PageManager定义的提示信息" runat="server" ID="Button2" OnClick="Button2_Click">
    </x:Button>
    <br />
    <x:Button Text="默认风格提示信息" AjaxLoadingType="Default" runat="server" ID="Button3"
        OnClick="Button3_Click">
    </x:Button>
    <br />
    <x:Button Text="Mask风格提示信息" AjaxLoadingType="Mask" runat="server" ID="Button4"
        OnClick="Button4_Click">
    </x:Button>
    <br />
    注：为了观察Ajax提示信息，所有按钮动作在后台都会睡眠1秒钟。
    </form>
</body>
</html>
