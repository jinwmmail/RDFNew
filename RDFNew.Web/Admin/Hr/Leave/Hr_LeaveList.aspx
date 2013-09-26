<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hr_LeaveList.aspx.cs" Inherits="RDFNew.Web.Admin.Hr.Leave.Hr_LeaveList" %>

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
                            <x:FormRow ColumnWidths="160px 160px">
                                <Items>
                                    <x:TextBox ID="txtLeaveID" runat="server" Text="" Label="请假单号" NextFocusControl="txtLeaveDate">
                                    </x:TextBox>
                                    <x:TextBox ID="txtLeaveDate" runat="server" Text="" Label="申请日期" NextFocusControl="txtLeaveID">
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
                    <x:Grid ID="Grid1" Title="请假列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="LeaveID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="请假单号" DataTextField="LeaveID" SortField="Hr_Leave.LeaveID"
                                Width="85" CommandName="View" />
                            <x:BoundField Width="75px" DataField="LeaveDate" SortField="Hr_Leave.LeaveDate" HeaderText="申请日期" />
                            <x:BoundField DataField="EmployeeName" HeaderText="员工姓名" Width="75px" />
                            <x:BoundField DataField="LeaveTypeName" HeaderText="类别名称" Width="75px" />
                            <x:BoundField DataField="DTFrom" SortField="Hr_Leave.DTFrom" HeaderText="开始时间" Width="120" />
                            <x:BoundField DataField="DTTo" SortField="Hr_Leave.DTTo" HeaderText="结束时间" Width="120" />
                            <x:BoundField DataField="Times" SortField="Hr_Leave.Times" HeaderText="请假时间<br/>&nbsp;(分钟)&nbsp;&nbsp;"
                                Width="75" TextAlign="Right" />
                            <x:BoundField Width="300px" DataField="Remark" HeaderText="备注" />
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

