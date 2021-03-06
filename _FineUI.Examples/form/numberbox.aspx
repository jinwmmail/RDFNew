﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="numberbox.aspx.cs" Inherits="FineUI.Examples.form.numberbox" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" />
    <x:SimpleForm ID="SimpleForm1" BodyPadding="5px" Width="350px" EnableBackgroundColor="true"
        Title="简单表单" runat="server" LabelWidth="120px">
        <Items>
            <x:NumberBox Label="0 到 9 的整数" ID="NumberBox1" runat="server" MaxValue="9" MinValue="0"
                NoDecimal="true" NoNegative="True" Required="True" EmptyText="比如 8" ShowRedStar="True" />
            <x:NumberBox Label="大于 100 的整数" ID="NumberBox3" runat="server" EmptyText="比如 99"
                MinValue="100" NoDecimal="True" DecimalPrecision="4" NoNegative="True" Required="True"
                ShowRedStar="True" />
            <x:NumberBox ID="NumberBox4" runat="server" EmptyText="精度为 2，比如 1.35" Label="0 到 1 之间的小数"
                MaxValue="1" MinValue="0" NoNegative="True" Required="True" ShowRedStar="True">
            </x:NumberBox>
            <x:NumberBox Label="任意整数" ID="NumberBox5" NoDecimal="true" runat="server" />
            <x:Button ID="btnSubmit" runat="server" ValidateForms="SimpleForm1" Text="提交表单" OnClick="btnSubmit_Click">
            </x:Button>
        </Items>
    </x:SimpleForm>
    </form>
</body>
</html>
