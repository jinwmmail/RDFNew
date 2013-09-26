<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_UserList.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.User.Sys_UserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="true"
                BodyPadding="5px" Title="查询条件" Height="60px" EnableCollapse="true">
                <Items>
                    <x:Form ID="FormQuery" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="130px 230px 230px">
                                <Items>
                                    <x:TextBox ID="txtUserID" runat="server" Text="" Width="60px" Label="用户代码" NextFocusControl="txtUserName">
                                    </x:TextBox>
                                    <x:TextBox ID="txtUserName" runat="server" Text="" Width="160px" Label="中文姓名" NextFocusControl="txtNameE">
                                    </x:TextBox>
                                    <x:TextBox ID="txtNameE" runat="server" Text="" Width="160px" Label="外文名字" NextFocusControl="txtUserID">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Grid ID="Grid1" Title="用户列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="UserID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="用户代码" DataTextField="UserID" SortField="Sys_User.UserID"
                                Width="75px" CommandName="View" />
                            <x:BoundField Width="120px" DataField="UserName" SortField="Sys_User.UserName" HeaderText="中文姓名" />
                            <x:BoundField DataField="NameE" SortField="Sys_User.NameE" HeaderText="外文名字" />
                            <x:BoundField DataField="EmployeeID" SortField="Sys_User.EmployeeID" HeaderText="员工工号"
                                Width="75" />
                            <x:BoundField Width="65px" DataField="LoginTimes" SortField="Sys_User.LoginTimes"
                                HeaderText="登录<br/>次数" TextAlign="Right" />
                            <x:BoundField Width="130px" DataField="LoginLast" SortField="Sys_User.LoginLast"
                                HeaderText="最近登录" />
                            <x:CheckBoxField Width="45px" RenderAsStaticField="true" DataField="Enabled" SortField="Sys_User.Enabled"
                                HeaderText="是否<br/>启用" TextAlign="Center" />
                            <x:CheckBoxField Width="65px" RenderAsStaticField="true" DataField="IsAdmin" SortField="Sys_User.IsAdmin"
                                HeaderText="是超级<br/>用户" TextAlign="Center" />
                            <x:BoundField DataField="Remark" HeaderText="备注" />
                        </Columns>
                    </x:Grid>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="弹出窗体" Popup="false" EnableIFrame="true" IFrameUrl="about:blank"
        EnableMaximize="true" Target="Self" EnableResize="true" runat="server" IsModal="true"
        WindowPosition="GoldenSection" Width="550px" EnableConfirmOnClose="true" Height="450px">
    </x:Window>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageList.js" type="text/javascript"></script>

<script type="text/javascript">
    function onReady() {
        PageList.grid1ClientID = '<%= Grid1.ClientID %>';
        PageList.window1ClientID = '<%= Window1.ClientID %>';
        PageList.setOnReady();
    }

    function onAjaxReady() {
        PageList.setOnAjaxReady();
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#" + window.location.pathname;
    }       
</script>

