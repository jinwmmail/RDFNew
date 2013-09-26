<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_FunctionList.aspx.cs"
    Inherits="RDFNew.Web.Admin.Sys.Function.Sys_FunctionList" %>

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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                Title="查询条件" Height="60px" EnableCollapse="true">
                <Items>
                    <x:Form ID="FormQuery" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="130px 230px">
                                <Items>
                                    <x:TextBox ID="txtFunctionID" runat="server" Text="" Width="60px" Label="功能代码"
                                        NextFocusControl="txtNameC">
                                    </x:TextBox>
                                    <x:TextBox ID="txtNameC" runat="server" Text="" Width="160px" Label="功能名称" NextFocusControl="txtFunctionID">
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
                    <x:Grid ID="Grid1" Title="功能列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="FunctionID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField ID="LinkButtonField1" runat="server" HeaderText="功能编号" DataTextField="FunctionID" SortField="Sys_Function.FunctionID"
                                Width="75" CommandName="View" />
                            <x:BoundField Width="50px" DataField="Seq" HeaderText="序号" TextAlign="Center" SortField="Sys_Function.Seq" />
                            <x:BoundField Width="150px" DataField="FunctionName"  HeaderText="功能名称" SortField="Sys_Function.FunctionName" />
                            <x:ImageField Width="40px" DataImageUrlField="IconUrl" SortField="Sys_Function.IconUrl"  DataImageUrlFormatString="{0}"
                                TextAlign="Center" HeaderText="图标"></x:ImageField>
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

    function btnPrint_onclick() {
        var p1 = Ext.getCmp('<%= txtFunctionID.ClientID %>');
        var p2 = Ext.getCmp('<%= txtNameC.ClientID %>');
        var win = top.Ext.getCmp('<%= Window1.ClientID %>');
        win.box_show("Sys/Function/Sys_FunctionListPrint.aspx?action=print&pm1=" +
            p1.getValue() + "&pm2=" + p2.getValue(), "打印-[功能]");
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#" + window.location.pathname;
    }         
</script>
