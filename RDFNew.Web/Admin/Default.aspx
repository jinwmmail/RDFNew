<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RDFNew.Web.Admin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></x:PageManager>
    <x:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Regions>
            <x:Region ID="Region1" Margins="0 0 0 0" ShowBorder="true" Height="50px" ShowHeader="false"
                Position="Top" Layout="HBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
                runat="server" Split="true" CollapseMode="Mini">
                <Items>
                    <x:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" BoxFlex="3"
                        Layout="Column">
                        <Items>
                            <x:ContentPanel ID="Panel8" runat="server" ShowBorder="false" ShowHeader="false">
                                <a href="Default.aspx">
                                    <x:Image ID="Image2" runat="server" ImageUrl="~/Res/Images/Logo/Logo.gif">
                                    </x:Image>
                                </a>
                            </x:ContentPanel>
                            <x:Panel ID="Panel3" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="25px 0 0 0">
                                <Items>
                                    <x:Label ID="Label6" runat="server" Text="企业管理及工作流应用快速开发平台" CssStyle="font-family: 黑体;font-size:20px;">
                                    </x:Label>
                                </Items>
                            </x:Panel>
                        </Items>
                    </x:Panel>
                    <x:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" BoxFlex="1"
                        Layout="HBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start">
                    </x:Panel>
                </Items>
            </x:Region>
            <x:Region ID="Region2" Split="true" EnableSplitTip="true" CollapseMode="Mini" Width="200px"
                Margins="0 0 0 0" ShowHeader="false" Layout="Fit" Position="Left" runat="server">
                <Toolbars>
                    <x:Toolbar ID="Toolbar11" Position="Top" runat="server">
                        <Items>
                            <x:Image ID="Image1" runat="server" Icon="Outline">
                            </x:Image>
                            <x:ToolbarText ID="ToolbarText1" runat="server" Text="系统导航">
                            </x:ToolbarText>
                            <x:ToolbarFill ID="ToolbarFill1" runat="server">
                            </x:ToolbarFill>
                            <x:Button ID="Button2" EnablePostBack="false" Icon="Cog" runat="server">
                                <Menu ID="Menu1" runat="server">
                                    <x:MenuButton EnablePostBack="false" Text="菜单样式" ID="MenuStyle" runat="server">
                                        <Menu ID="Menu2" runat="server">
                                            <x:MenuCheckBox Text="树菜单" ID="MenuStyleTree" GroupName="MenuStyle" AutoPostBack="true"
                                                OnCheckedChanged="MenuStyle_CheckedChanged" runat="server">
                                            </x:MenuCheckBox>
                                            <x:MenuCheckBox Text="手风琴+树菜单" ID="MenuStyleAccordion" GroupName="MenuStyle" AutoPostBack="true"
                                                OnCheckedChanged="MenuStyle_CheckedChanged" runat="server">
                                            </x:MenuCheckBox>
                                        </Menu>
                                    </x:MenuButton>
                                    <x:MenuButton ID="MenuTheme" EnablePostBack="false" Text="主题" runat="server">
                                        <Menu ID="Menu4" runat="server">
                                            <x:MenuCheckBox Text="Blue" ID="MenuThemeBlue" GroupName="MenuTheme" AutoPostBack="true"
                                                OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                                            </x:MenuCheckBox>
                                            <x:MenuCheckBox Text="Gray" ID="MenuThemeGray" GroupName="MenuTheme" AutoPostBack="true"
                                                OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                                            </x:MenuCheckBox>
                                            <x:MenuCheckBox Text="Access" ID="MenuThemeAccess" GroupName="MenuTheme" AutoPostBack="true"
                                                OnCheckedChanged="MenuTheme_CheckedChanged" runat="server">
                                            </x:MenuCheckBox>
                                        </Menu>
                                    </x:MenuButton>
                                    <x:MenuSeparator ID="MenuSeparator2" runat="server">
                                    </x:MenuSeparator>
                                    <x:MenuButton ID="MenuButton1" Text="关于" EnablePostBack="false" runat="server">
                                    </x:MenuButton>
                                    <x:MenuSeparator ID="MenuSeparator3" runat="server">
                                    </x:MenuSeparator>
                                    <x:MenuButton ID="menuExit" Text="注销" runat="server">
                                    </x:MenuButton>
                                </Menu>
                            </x:Button>
                        </Items>
                    </x:Toolbar>
                </Toolbars>
            </x:Region>
            <x:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                runat="server">
                <Items>
                    <x:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                        <Tabs>
                            <x:Tab ID="Tab1" Title="首页" Layout="Fit" Icon="House" runat="server">
                            </x:Tab>
                        </Tabs>
                    </x:TabStrip>
                </Items>
            </x:Region>
            <x:Region ID="Region3" runat="server" Margins="0 0 0 0" ShowHeader="false" Position="Bottom"
                ShowBorder="false" Height="20px">
                <Toolbars>
                    <x:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <x:ToolbarText ID="labWelcome" runat="server" Text="欢迎您:">
                            </x:ToolbarText>
                            <x:ToolbarText ID="labUser" runat="server" Text="" CssStyle="color:blue;">
                            </x:ToolbarText>
                            <x:ToolbarText ID="ToolbarText3" runat="server" Text="" CssStyle="width:0px;">
                            </x:ToolbarText>
                            <x:ToolbarText ID="labLgoin" runat="server" Text="登录时间:">
                            </x:ToolbarText>
                            <x:ToolbarText ID="labDateTime" runat="server" Text="">
                            </x:ToolbarText>
                            <x:ToolbarFill ID="ToolbarFill110" runat="server">
                            </x:ToolbarFill>
                            <x:ToolbarText ID="labCopyright" runat="server" Text="@2013 Copyright Powered By:Jinwmmail Co.,Ltd">
                            </x:ToolbarText>
                        </Items>
                    </x:Toolbar>
                </Toolbars>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Icon="PageWhiteCode" Title="弹窗" Popup="false" EnableIFrame="true"
        runat="server" IsModal="true" Width="860px" Height="570px" EnableClose="true"
        WindowPosition="GoldenSection" EnableMaximize="true" EnableResize="true">
    </x:Window>
    </form>
</body>
</html>

<script src="/Res/Jscript/default.js" type="text/javascript"></script>

<script type="text/javascript">

    function restoreStatus() {
        //header展开状态
        var header = Ext.getCmp('<%= Region1.ClientID %>');
        var B = Ext.util.Cookies.get("ck_HeaderState");
        if (B == "Closed")
            header.collapse(true);

        header.addListener('collapse', function() {
            var N = new Date();
            N.setMonth(N.getYear() + 1);
            Ext.util.Cookies.set("ck_HeaderState", "Closed", N);
        });
        header.addListener('expand', function() {
            var N = new Date();
            N.setMonth(N.getYear() + 1);
            Ext.util.Cookies.set("ck_HeaderState", "Opened", N);
        });

        //记住左边系统导航宽度
        var left = Ext.getCmp('<%= Region2.ClientID %>');
        var C = Ext.util.Cookies.get("regionLeft_Width");
        if (C != null) {
            left.setWidth(C);
            var regionPanel = Ext.getCmp('<%= RegionPanel1.ClientID %>');
            regionPanel.doLayout();
        }

        left.addListener('resize', function(e) {
            var N = new Date();
            N.setMonth(N.getYear() + 1);
            Ext.util.Cookies.set("regionLeft_Width", e.getWidth(), N);
        });
    }

    Ext.onReady(restoreStatus);
</script>

