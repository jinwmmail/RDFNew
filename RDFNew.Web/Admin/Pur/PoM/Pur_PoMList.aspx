﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pur_PoMList.aspx.cs" Inherits="RDFNew.Web.Admin.Pur.PoM.Pur_PoMList" %>

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
                                    <x:TextBox ID="txtPoMID" runat="server" Text="" Label="订单编号" NextFocusControl="">
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
                    <x:Grid ID="Grid1" Title="采购订单列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="PoMID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="订单编号" DataTextField="PoMID" SortField="Pur_PoM.PoMID" Width="90"
                                CommandName="View" />
                            <x:BoundField Width="90" DataField="PoMDate" SortField="Pur_PoM.PoMDate" HeaderText="制单日期" />
                            <x:BoundField Width="90" DataField="PartnerID" SortField="Pur_PoM.PartnerID" HeaderText="供方编号" />
                            <x:BoundField Width="250" DataField="PartnerName" HeaderText="供方名称" />
                            <x:BoundField Width="90" DataField="DeliveryDate" SortField="Pur_PoM.DeliveryDate" HeaderText="交货日期" />
                            <x:BoundField Width="300" DataField="Remark" HeaderText="备注" />
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
        var p1 = Ext.getCmp('<%= txtPoMID.ClientID %>');        
        var win = top.Ext.getCmp('<%= Window1.ClientID %>');
        win.box_show("Pur/PoM/Pur_PoMListPrint.aspx?action=print&pm1=" +
            p1.getValue(), "打印-[采购订单列表]");
    }


    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#" + window.location.pathname;
    }   
</script>

