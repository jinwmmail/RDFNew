<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_Org.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.Org.Flow_Org" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1"></x:PageManager>
    <x:HiddenField ID="hidPID" runat="server">
    </x:HiddenField>
    <x:HiddenField ID="hidRoleID" runat="server">
    </x:HiddenField>
    <x:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <x:Toolbar ID="Toolbar1" runat="server">
                <Items>
                </Items>
            </x:Toolbar>
        </Toolbars>
        <Regions>
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="140">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <x:TextBox ID="txtOrgID" runat="server" Text="" Label="组织代码" NextFocusControl="txtOrgName"
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                    <x:TextBox ID="txtOrgName" runat="server" Text="" Label="组织名称" NextFocusControl="txtRemark"
                                        Required="true" FocusOnPageLoad="true" ShowRedStar="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <x:TriggerBox ID="txtPOrgID" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                        Label="上级组织" EnableEdit="false" Required="false">
                                    </x:TriggerBox>
                                    <x:TextBox ID="txtPOrgName" runat="server" Text="" Label="" Readonly="true" CssStyle="background:#c0c0c0;"
                                        ShowLabel="false">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <x:TriggerBox ID="txtRoleName" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                        Label="主管角色" EnableEdit="false" Required="false">
                                    </x:TriggerBox>
                                    <x:DropDownList ID="ddlJobID" runat="server" Label="主管职位" AutoPostBack="false" Required="true"
                                        ShowRedStar="true">
                                    </x:DropDownList>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtOrgID">
                                    </x:TextArea>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:TabStrip ID="tsX" runat="server" ShowBorder="false">
                        <Tabs>
                            <x:Tab ID="Tab1" runat="server" Title="所含角色" Layout="HBox" BoxConfigAlign="Stretch"
                                BoxConfigPosition="Start" EnableBackgroundColor="true">
                                <Items>
                                    <x:Panel ID="Panel6" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false"
                                        Layout="Fit">
                                        <Items>
                                            <x:Tree ID="treeFr" runat="server" AutoScroll="true" ShowBorder="false" Title="可选项"
                                                EnableMultiSelect="true">
                                            </x:Tree>
                                        </Items>
                                    </x:Panel>
                                    <x:Panel ID="Panel21" runat="server" Width="70" Layout="Row" ShowHeader="false" EnableBackgroundColor="true"
                                        ShowBorder="false" BodyStyle="padding-left:15px;">
                                        <Items>
                                            <x:Button ID="xAdd" runat="server" Icon="ResultsetNext" CssStyle=" margin-top:10px;"
                                                OnClick="btnX_Click">
                                            </x:Button>
                                            <x:Button ID="xAddAll" runat="server" Icon="ForwardBlue" OnClick="btnX_Click" CssStyle=" margin-top:10px;">
                                            </x:Button>
                                            <x:Button ID="xDel" runat="server" Icon="ReverseBlue" OnClick="btnX_Click" CssStyle=" margin-top:20px;">
                                            </x:Button>
                                            <x:Button ID="xDelAll" runat="server" Icon="RewindBlue" OnClick="btnX_Click" CssStyle=" margin-top:10px;">
                                            </x:Button>
                                        </Items>
                                    </x:Panel>
                                    <x:Panel ID="Panel1" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false"
                                        Layout="Fit">
                                        <Items>
                                            <x:Tree ID="treeTo" runat="server" AutoScroll="true" AnchorValue="40% 100%" ShowBorder="false"
                                                Title="已选项" EnableMultiSelect="true">
                                            </x:Tree>
                                        </Items>
                                    </x:Panel>
                                </Items>
                            </x:Tab>
                        </Tabs>
                    </x:TabStrip>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="选择" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent" IsModal="true" Width="550px"
        Height="450px">
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

