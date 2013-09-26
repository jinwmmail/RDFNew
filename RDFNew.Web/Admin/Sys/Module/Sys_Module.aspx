<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Module.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Module.Sys_Module" %>

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
    <x:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <x:Toolbar ID="Toolbar1" runat="server">
                <Items>
                </Items>
            </x:Toolbar>
        </Toolbars>
        <Regions>
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="200">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="60px">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtModuleID" runat="server" Text="" Width="160px" Label="模块代码" NextFocusControl="txtCaption"
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtCaption" runat="server" Text="" Width="160px" Label="模块描述" NextFocusControl="txtUrl"
                                        Required="true" FocusOnPageLoad="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtUrl" runat="server" Text="" Label="URL" NextFocusControl="txtRemark">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:CheckBox ID="ckbEnabled" runat="server" Label="已启用">
                                    </x:CheckBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtModuleID">
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
                            <x:Tab ID="Tab1" runat="server" Title="所含功能" Layout="HBox" BoxConfigAlign="Stretch"
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
                                    <x:Panel ID="Panel21" runat="server" Width="70" Layout="Row" ShowHeader="false"
                                        EnableBackgroundColor="true" ShowBorder="false" BodyStyle="padding-left:15px;">
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
