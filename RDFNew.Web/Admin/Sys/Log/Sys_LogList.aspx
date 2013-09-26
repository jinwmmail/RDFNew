<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_LogList.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Log.Sys_LogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                            <x:FormRow ColumnWidths="200 180">
                                <Items>                                   
                                    <x:TriggerBox ID="txtModuleID" runat="server" EnablePostBack="false"
                                        TriggerIcon="Search" Label="模块代码">
                                    </x:TriggerBox>
                                    <x:TriggerBox ID="txtUserID" runat="server" EnablePostBack="false"
                                        TriggerIcon="Search" Label="用户">
                                    </x:TriggerBox>                                    
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Grid ID="Grid1" Title="系统日志列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="LogID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="日志编号" DataTextField="LogID" SortField="Sys_Log.LogID"
                                Width="150" CommandName="View" />
                            <x:BoundField DataField="Module" SortField="Sys_Log.Module" HeaderText="模块" />                            
                            <x:BoundField DataField="Action" SortField="Sys_Log.Action" HeaderText="操作" Width="60" />
                            <x:BoundField DataField="Table" SortField="Sys_Log.[Table]" HeaderText="表" Width="100" />
                            <x:BoundField DataField="Key" SortField="Sys_Log.Key" HeaderText="主键字段" />
                            <x:BoundField DataField="Value" SortField="Sys_Log.Value" HeaderText="主键值" />
                            <x:BoundField DataField="User" SortField="Sys_Log.[User]" HeaderText="用户" Width="60" />
                            <x:BoundField DataField="DateTime" SortField="Sys_Log.DateTime" HeaderText="操作时间"
                                Width="150" />
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
    <x:Window ID="Window2" Title="弹出窗体" Popup="false" EnableIFrame="true" IFrameUrl="about:blank"
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

        var win = Ext.getCmp('<%= Window2.ClientID %>');
        var L = Ext.getBody().getSize().width - win.getWidth();
        win.x_property_left = L;
        win.x_property_top = 0;
    }

    function onAjaxReady() {
        PageList.setOnAjaxReady();
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#" + window.location.pathname;
    }   
</script>

