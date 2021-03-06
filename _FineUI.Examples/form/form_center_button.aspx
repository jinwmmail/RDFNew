﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="form_center_button.aspx.cs"
    Inherits="FineUI.Examples.form.form_center_button" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <style>
        .mypanel
        {
            text-align: center;
            padding-top: 10px;
            margin-top: 10px;
            border-top: solid 1px #ccc;
        }
        .mypanel .mybutton
        {
            display: inline-block;
            *display: inline;
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="_form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" />
    <x:Form Width="600px" BodyPadding="5px" ID="Form1" EnableBackgroundColor="true" LabelWidth="100px"
        runat="server" Title="表单">
        <Rows>
            <x:FormRow ColumnWidths="40% 60%">
                <Items>
                    <x:Label ID="Label1" runat="server" Label="标签" Text="标签的值">
                    </x:Label>
                    <x:CheckBox ID="CheckBox1" runat="server" Text="复选框" Label="复选框" CssClass="redcolor">
                    </x:CheckBox>
                </Items>
            </x:FormRow>
            <x:FormRow ColumnWidths="40% 60%">
                <Items>
                    <x:DropDownList ID="DropDownList1" runat="server" Label="下拉列表" Required="true" ShowRedStar="True">
                        <x:ListItem Selected="true" Text="可选项 1" Value="0"></x:ListItem>
                        <x:ListItem Text="可选项 2" Value="1"></x:ListItem>
                    </x:DropDownList>
                    <x:TextBox ID="TextBox1" ShowRedStar="true" runat="server" Label="文本框" Required="true"
                        Text="">
                    </x:TextBox>
                </Items>
            </x:FormRow>
            <x:FormRow>
                <Items>
                    <x:Panel ID="Panel1" runat="server" EnableBackgroundColor="true" ShowBorder="false"
                        CssClass="mypanel" ShowHeader="false">
                        <Items>
                            <x:Button runat="server" Text="验证此表单并提交" CssClass="mybutton" ValidateForms="Form1"
                                ID="btnSubmitForm1" OnClick="btnSubmitForm1_Click">
                            </x:Button>
                            <x:Button ID="btnResetForm1" EnablePostBack="false" CssClass="mybutton" Text="重置表单"
                                runat="server">
                            </x:Button>
                        </Items>
                    </x:Panel>
                </Items>
            </x:FormRow>
        </Rows>
    </x:Form>
    </form>
</body>
</html>
