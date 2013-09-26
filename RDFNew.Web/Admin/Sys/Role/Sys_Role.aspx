<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Role.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Role.Sys_Role" %>

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
                Layout="Fit" Height="160">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="60px">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtRoleID" runat="server" Text="" Width="100px" Label="角色代码" NextFocusControl="txtSeq"
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtSeq" runat="server" Text="" Width="60px" Label="序号" NextFocusControl="txtRoleName"
                                        Regex="^[0-9]+$" RegexMessage="只能输入数字.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtRoleName" runat="server" Text="" Label="角色名称" NextFocusControl="txtRemark"
                                        Required="true" FocusOnPageLoad="true" Width="200px">
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
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtRoleID">
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
                            <x:Tab ID="Tab1" runat="server" Title="所含用户" Layout="HBox" BoxConfigAlign="Stretch"
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
                            <x:Tab ID="Tab2" runat="server" Title="所含权限" Layout="HBox" BoxConfigAlign="Stretch"
                                BoxConfigPosition="Start" EnableBackgroundColor="true">
                                <Items>
                                    <x:Panel ID="Panel4" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false"
                                        Layout="Fit">
                                        <Items>
                                            <x:Tree ID="treeFr2" runat="server" AutoScroll="true" ShowBorder="false" Title="可选项"
                                                EnableMultiSelect="true">
                                            </x:Tree>
                                        </Items>
                                    </x:Panel>
                                    <x:Panel ID="Panel2" runat="server" Width="70" Layout="Row" ShowHeader="false" EnableBackgroundColor="true"
                                        ShowBorder="false" BodyStyle="padding-left:15px;">
                                        <Items>
                                            <x:Button ID="xAdd2" runat="server" Icon="ResultsetNext" CssStyle=" margin-top:10px;"
                                                OnClick="btnX2_Click">
                                            </x:Button>
                                            <x:Button ID="xAddAll2" runat="server" Icon="ForwardBlue" OnClick="btnX2_Click" CssStyle=" margin-top:10px;">
                                            </x:Button>
                                            <x:Button ID="xDel2" runat="server" Icon="ReverseBlue" OnClick="btnX2_Click" CssStyle=" margin-top:20px;">
                                            </x:Button>
                                            <x:Button ID="xDelAll2" runat="server" Icon="RewindBlue" OnClick="btnX2_Click" CssStyle=" margin-top:10px;">
                                            </x:Button>
                                        </Items>
                                    </x:Panel>
                                    <x:Panel ID="Panel3" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false"
                                        Layout="Fit">
                                        <Items>
                                            <x:Tree ID="treeTo2" runat="server" AutoScroll="true" AnchorValue="40% 100%" ShowBorder="false"
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

