<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_ToDoMList.aspx.cs"
    Inherits="RDFNew.Web.Admin.Flow.ToDoM.Flow_ToDoMList" %>

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
                            <x:FormRow ColumnWidths="150 150">
                                <Items>
                                    <x:TextBox ID="txtToDoMID" runat="server" Text="" Label="流程编号" NextFocusControl="">
                                    </x:TextBox>
                                    <x:HiddenField ID="HiddenField1" runat="server">
                                    </x:HiddenField>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Grid ID="Grid1" Title="流程列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="ToDoMID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand" OnPreRowDataBound="Grid1_PreRowDataBound">
                        <Columns>
                            <x:BoundField DataField="InstanceID" Hidden="true" />
                            <x:LinkButtonField runat="server" HeaderText="流程编号" DataTextField="ToDoMID" SortField="Flow_ToDoM.ToDoMID"
                                Width="90" CommandName="View" />
                            <x:BoundField Width="200" DataField="Description" HeaderText="流程名称" />
                            <x:BoundField Width="100" DataField="BillID" HeaderText="单据编号" />                            
                            <x:BoundField DataField="BillPage" Hidden="true"/>
                            <x:BoundField DataField="ToDoMID" Hidden="true"/>
                            <x:BoundField Width="60" DataField="OwnerName" HeaderText="发起人" />                            
                            <x:BoundField Width="60" DataField="State" HeaderText="状态" />
                            <x:BoundField Width="200" DataField="ResultDesc" HeaderText="说明" />
                            <x:LinkButtonField ID="LinkButtonField2" ColumnID="lkbInstanceView" runat="server"
                                HeaderText="" Text="查看进度" CommandName="InstanceView" Width="65" />
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

