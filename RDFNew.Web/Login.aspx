<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RDFNew.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></x:PageManager>
    <x:Panel ID="Panel1" runat="server" Layout="HBox" BoxConfigAlign="Center" BoxConfigPosition="Center"
        ShowBorder="false" ShowHeader="false" AutoScroll="true">
        <Items>
            <x:Panel ID="Panel2" runat="server" Layout="Row" Width="792" Height="510" ShowBorder="false"
                ShowHeader="false">
                <Items>
                    <x:Panel ID="Panel110" runat="server" ShowBorder="false" ShowHeader="false" Layout="Column">
                        <Items>
                            <x:Panel ID="Panel8" runat="server" ColumnWidth="60%" ShowBorder="false" ShowHeader="false">
                                <Items>
                                    <x:Image ID="Image1" runat="server" ImageUrl="~/Res/Images/Login/Security.jpg">
                                    </x:Image>
                                </Items>
                            </x:Panel>
                            <x:Panel ID="Panel9" runat="server" ColumnWidth="40%" ShowBorder="false" ShowHeader="false">
                                <Items>
                                    <x:Form ID="Form2" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="30">
                                        <Rows>
                                            <x:FormRow ID="FormRow1" runat="server">
                                                <Items>
                                                    <x:Label ID="Label1" runat="server" Text="登录" CssStyle="margin:40px 0px 40px 0px;font-family: 黑体;font-size:40px;">
                                                    </x:Label>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow ID="FormRow3" runat="server"  >
                                                <Items>
                                                    <x:TextBox ID="txtUser" runat="server" EmptyText="工号、帐户或邮件地址." 
                                                    Height="40" NextFocusControl="txtPwd" Text="Guest" 
                                                    CssStyle="margin:5px 0px 5px 0px;padding:10px;font-size:16px;font-weight:bold;">
                                                    </x:TextBox>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow ID="FormRow4" runat="server">
                                                <Items>
                                                    <x:TextBox ID="txtPwd" runat="server" EmptyText="" Height="40" FocusOnPageLoad="true"
                                                    CssStyle="margin:5px 0px 5px 0px;padding:10px;font-size:16px;font-weight:bold;" TextMode="Password">
                                                    </x:TextBox>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow ID="FormRow2" runat="server">
                                                <Items>
                                                    <x:CheckBox ID="ckbRemember" runat="server" Text="记住我的帐号">
                                                    </x:CheckBox>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow ID="FormRow6" runat="server" ColumnWidths="30 200">
                                                <Items>
                                                    <x:Label ID="Label2" runat="server">
                                                    </x:Label>
                                                    <x:Button ID="btnLogin" runat="server" Type="Submit"
                                                        Text="&nbsp;&nbsp;&nbsp;&nbsp;登&nbsp;&nbsp;&nbsp;&nbsp;录&nbsp;&nbsp;&nbsp;&nbsp;"
                                                        Size="Large" CssStyle="margin:5px 0px 5px 0px;padding:6px;font-size:16px;font-weight:bold;width:200px;">
                                                    </x:Button>
                                                </Items>
                                            </x:FormRow>
                                        </Rows>
                                    </x:Form>
                                </Items>
                            </x:Panel>
                        </Items>
                    </x:Panel>
                    <x:Panel ID="Panel3" runat="server" ShowBorder="false" ShowHeader="false" Layout="Column"
                        Height="120">
                        <Items>
                            <x:Form ID="Panel6" runat="server" ColumnWidth="60%" ShowBorder="false" ShowHeader="false"
                                Height="100%" EnableBackgroundColor="true" LabelWidth="30">
                                <Rows>
                                    <x:FormRow>
                                        <Items>
                                            <x:Label ID="Label5" runat="server" Text="欢迎使用" CssStyle="margin:15px 0px 5px 0px;font-family: 黑体;font-size:30px;">
                                            </x:Label>
                                        </Items>
                                    </x:FormRow>
                                    <x:FormRow>
                                        <Items>
                                            <x:Label ID="Label6" runat="server" Text="企业管理及工作流应用快速开发平台" CssStyle="margin:5px 0px 5px 0px;font-family: 黑体;font-size:20px;">
                                            </x:Label>
                                        </Items>
                                    </x:FormRow>
                                </Rows>
                            </x:Form>
                            <x:Form ID="Form3" runat="server" ColumnWidth="40%" ShowBorder="false" ShowHeader="false"
                                Height="100%" LabelWidth="30">
                                <Rows>
                                    <x:FormRow>
                                        <Items>
                                            <x:Label ID="labInfo" runat="server" Text="" CssStyle="margin:5px 0px 5px 0px;color:Red;">
                                            </x:Label>
                                        </Items>
                                    </x:FormRow>
                                </Rows>
                            </x:Form>
                        </Items>
                    </x:Panel>
                    <x:ContentPanel ID="Panel4" runat="server" ShowBorder="false" ShowHeader="false">
                        <hr />
                    </x:ContentPanel>
                    <x:Panel ID="Panel5" runat="server" ShowBorder="false" ShowHeader="false" Layout="Column"
                        Height="25">
                        <Items>
                            <x:Panel ID="Panel10" runat="server" ColumnWidth="40%" ShowBorder="false" ShowHeader="false"
                                Height="100%" Layout="HBox" BoxConfigPosition="Left">
                                <Items>
                                    <x:Label ID="Label3" runat="server" Text="">
                                    </x:Label>
                                </Items>
                            </x:Panel>
                            <x:Panel ID="Panel11" runat="server" ColumnWidth="60%" ShowBorder="false" ShowHeader="false"
                                Height="100%" Layout="HBox" BoxConfigPosition="Right">
                                <Items>
                                    <x:Label ID="labCopyright" runat="server" Text="@2013 Copyright Powered By:Jinwmmail Co.,Ltd">
                                    </x:Label>
                                </Items>
                            </x:Panel>
                        </Items>
                    </x:Panel>
                </Items>
            </x:Panel>
        </Items>
    </x:Panel>
    </form>
</body>
</html>

<script type="text/javascript">
    function onReady() {
        var A = Ext.getDom('<%=this.txtUser.ClientID %>');
        var B = Ext.util.Cookies.get("ck_UserName");
        var C = Ext.getDom('<%=this.ckbRemember.ClientID %>');
        var ck = Ext.util.Cookies.get("ck_Remenber");
        if (ck == "1" && A != undefined && B != null) {
            A.value = B;
            C.checked = true;
        }
        if (ck==undefined)
            C.checked = true;
    }

    //以下必须放在最后面
    if (self != top) {
        top.location.href = window.location.protocol + "//" + window.location.host + window.location.pathname;
    }     
</script>
