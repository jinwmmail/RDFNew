<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_UserPwd.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.User.Sys_UserPwd" %>

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
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="120px">
                        <Rows>
                            <x:FormRow ColumnWidths="250px 250px">
                                <Items>
                                    <x:TextBox ID="txtUserID" runat="server" Text="" Width="100px" Label="用户代码" EmptyText="为空则自动编号">
                                    </x:TextBox>
                                    <x:TextBox ID="txtUserCode" runat="server" Text="" Width="100px" Label="登录帐号">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="250px 250px">
                                <Items>
                                    <x:TextBox ID="txtUserName" runat="server" Text="" Width="100px" Label="中文姓名">
                                    </x:TextBox>
                                    <x:TextBox ID="txtNameE" runat="server" Text="" Width="100px" Label="外文名字">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtEmail" runat="server" Text="" Width="350px" Label="邮件地址">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPwd" runat="server" Text="" Width="100px" Label="请输入原密码" TextMode="Password"
                                        MaxLength="8" FocusOnPageLoad="true" NextFocusControl="txtPwdNew">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPwdNew" runat="server" Text="" Width="100px" Label="请输入新密码" TextMode="Password"
                                        MaxLength="8" NextFocusControl="txtPwdCfg">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPwdCfg" runat="server" Text="" Width="100px" Label="请再次输入新密码" TextMode="Password"
                                        MaxLength="8" NextFocusControl="txtPwd">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    </form>
</body>
</html>

<script type="text/javascript">
    function onReady() {

    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

