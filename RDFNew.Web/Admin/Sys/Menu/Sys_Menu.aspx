<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Menu.Sys_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1"></x:PageManager>
    <x:HiddenField ID="hidPID" runat="server">
    </x:HiddenField>
    <x:HiddenField ID="hidMenuID" runat="server">
    </x:HiddenField>
    <x:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <x:Toolbar ID="Toolbar1" runat="server">
                <Items>
                </Items>
            </x:Toolbar>
        </Toolbars>
        <Regions>
            <x:Region ID="Region1" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="60px">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMenuID" runat="server" Text="" Width="100px" Label="菜单代码" NextFocusControl="txtMenuName"
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TriggerBox ID="txtModuleID" runat="server" Width="160px" EnablePostBack="false"
                                        TriggerIcon="Search" Label="模块代码" EnableEdit="false">
                                    </x:TriggerBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtURL" runat="server" Text="" Label="URL" Readonly="true" CssStyle="background:#c0c0c0;">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtUrlParameter" runat="server" Text="" Label="URL参数">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMenuName" runat="server" Text="" Width="160px" Label="菜单名称" NextFocusControl="txtNameE"
                                        Required="true" FocusOnPageLoad="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtIcon" runat="server" Text="" Width="160px" Label="图标" NextFocusControl="txtRemark">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <x:TriggerBox ID="txtPMenuID" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                        Label="上级菜单" EnableEdit="false" Required="false" ShowRedStar="true">
                                    </x:TriggerBox>
                                    <x:TextBox ID="txtPMenuName" runat="server" Text="" Label="" Readonly="true" CssStyle="background:#c0c0c0;"
                                        ShowLabel="false">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:CheckBox ID="ckbEnabled" runat="server" Label="已启用">
                                    </x:CheckBox>
                                    <x:CheckBox ID="ckbVisibled" runat="server" Label="可见">
                                    </x:CheckBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtMenuID">
                                    </x:TextArea>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="选择模块" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent" IsModal="true" Width="520px"
        Height="450px" WindowPosition="Center">
    </x:Window>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

<script type="text/javascript">
    function onReady() {
        var win = Ext.getCmp('<%= Window1.ClientID %>');
        var L = parent.Ext.getBody().getSize().width - win.getWidth();
        win.x_property_left = L;
        win.x_property_top = 0;
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

