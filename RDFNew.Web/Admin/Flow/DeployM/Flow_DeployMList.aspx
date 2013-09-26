<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_DeployMList.aspx.cs"
    Inherits="RDFNew.Web.Admin.Flow.DeployM.Flow_DeployMList" %>

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
                            <x:FormRow ColumnWidths="160 160">
                                <Items>
                                    <x:TextBox ID="txtDeployMID" runat="server" Text="" Label="部署编号" NextFocusControl="">
                                    </x:TextBox>
                                    <x:TextBox ID="txtDeployName" runat="server" Text="" Label="部署名称" NextFocusControl="">
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
                    <x:Grid ID="Grid1" Title="流程列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="DeployMID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand" OnPreRowDataBound="Grid1_PreRowDataBound">
                        <Columns>
                            <x:LinkButtonField ColumnID="lkbDeploy" runat="server" HeaderText="" Text="部署" CommandName="Install"
                                Width="45" />
                            <x:LinkButtonField ColumnID="lkbConfig" runat="server" HeaderText="" Text="配置" CommandName="Deploy"
                                Width="45" />
                            <x:BoundField Width="85" DataField="DeployMID" SortField="Flow_DeployM.DeployMID" HeaderText="部署编号" />
                            <x:BoundField Width="300" DataField="DeployName" SortField="Flow_DeployM.DeployName"
                                HeaderText="部署名称" />
                            <x:BoundField Width="300" DataField="Remark" SortField="Flow_DeployM.Remark" HeaderText="备注" />
                        </Columns>
                    </x:Grid>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="弹出窗体" Hidden="true" EnableIFrame="true" IFrameUrl="about:blank"
        EnableMaximize="true" Target="Self" EnableResize="true" runat="server" IsModal="true"
        CloseAction="HidePostBack" OnClose="Window1_Close" WindowPosition="GoldenSection"
        Width="550px" Height="450px">
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

