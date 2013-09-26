<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pur_PoM.aspx.cs" Inherits="RDFNew.Web.Admin.Pur.PoM.Pur_PoM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .mygrid-row-summary.x-grid3-row
        {
            background-color: #efefef !important;
            background-image: none !important;
            border-color: #fff #ededed #ededed !important;
        }
        .mygrid-row-summary.x-grid3-row .x-grid3-td-numberer, .mygrid-row-summary.x-grid3-row .x-grid3-td-checker
        {
            background-image: none !important;
        }
        .mygrid-row-summary.x-grid3-row .x-grid3-td-numberer .x-grid3-col-numberer, .mygrid-row-summary.x-grid3-row .x-grid3-td-checker .x-grid3-col-checker
        {
            display: none;
        }
        .mygrid-row-summary.x-grid3-row td
        {
            font-size: 14px;
            line-height: 16px;
            font-weight: bold;
            color: #000;
            text-align :right ;
        }
    </style>
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
                Layout="Fit" Height="140">
                <Items>
                    <x:TabStrip ID="TabStrip1" runat="server" ShowBorder="false">
                        <Tabs>
                            <x:Tab ID="Tab1" runat="server" Layout="Fit" Title="主表信息">
                                <Items>
                                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                                        ShowHeader="False" runat="server" LabelWidth="60px">
                                        <Rows>
                                            <x:FormRow ColumnWidths="160 160 160">
                                                <Items>
                                                    <x:TextBox ID="txtPoMID" runat="server" Text="" Label="订单编号" NextFocusControl=""
                                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                                    </x:TextBox>
                                                    <x:DatePicker ID="txtPoMDate" runat="server" Text="" Label="制单日期" NextFocusControl=""
                                                        Required="true">
                                                    </x:DatePicker>
                                                    <x:DatePicker ID="txtDeliveryDate" runat="server" Text="" Label="交货日期" NextFocusControl="">
                                                    </x:DatePicker>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow ColumnWidths="160 100%">
                                                <Items>
                                                    <x:TriggerBox ID="txtPartnerID" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                                        Label="供方编号" EnableEdit="false" Required="true">
                                                    </x:TriggerBox>
                                                    <x:TextBox ID="txtPartnerName" runat="server" Text="" Label="供方名称" Readonly="true"
                                                        CssStyle="background:#c0c0c0;">
                                                    </x:TextBox>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow>
                                                <Items>
                                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="">
                                                    </x:TextArea>
                                                </Items>
                                            </x:FormRow>
                                        </Rows>
                                    </x:Form>
                                </Items>
                            </x:Tab>
                        </Tabs>
                    </x:TabStrip>
                </Items>
            </x:Region>
            <x:Region ID="Region2" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit" EnableBackgroundColor="true">
                <Items>
                    <x:TabStrip ID="TabStrip2" runat="server" ShowBorder="false">
                        <Tabs>
                            <x:Tab ID="Tab2" runat="server" Layout="Fit" Title="明细列表">
                                <Toolbars>
                                    <x:Toolbar ID="Toolbar2" runat="server">
                                    </x:Toolbar>
                                </Toolbars>
                                <Items>
                                    <x:Grid ID="Grid1" ShowHeader="false" PageSize="50" ShowBorder="false" AutoHeight="true"
                                        AllowPaging="false" runat="server" EnableMultiSelect="false" DataKeyNames="PoDID"
                                        EnableRowNumber="True" EnableTextSelection="true" OnRowCommand="Grid1_RowCommand"
                                        OnRowDataBound="Grid1_RowDataBound">
                                        <Columns>
                                            <x:LinkButtonField ID="LinkButtonField1" runat="server" HeaderText="序号" DataTextField="Seq"
                                                Width="45" CommandName="View" />
                                            <x:BoundField Width="70" DataField="MaterialID" HeaderText="物料编号" />
                                            <x:BoundField Width="120" DataField="MaterialName" HeaderText="物料名称" ColumnID="MaterialName" />
                                            <x:BoundField Width="35" DataField="UnitID" HeaderText="单位" TextAlign="Center" />
                                            <x:TemplateField HeaderText="数量" Width="65px" ColumnID="Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQty" runat="server" Width="50px" TabIndex='<%# Container.DataItemIndex + 10 %>'
                                                        Text='<%# Eval("Qty","{0:0.##}") %>' Style="text-align: right;" onchange='CalcAmt(this,0);'></asp:TextBox>
                                                </ItemTemplate>
                                            </x:TemplateField>
                                            <x:TemplateField HeaderText="单价" Width="65px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrice" runat="server" Width="50px" TabIndex='<%# Container.DataItemIndex + 10 %>'
                                                        Text='<%# Eval("Price","{0:0.00}") %>' Style="text-align: right;" onchange='CalcAmt(this,1);'></asp:TextBox>
                                                </ItemTemplate>
                                            </x:TemplateField>
                                            <x:TemplateField HeaderText="金额" Width="75px" ColumnID="Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmt" runat="server" Width="100%" Text='<%# Eval("Amt","{0:0.00}") %>'
                                                        ReadOnly="true" Style="text-align: right; border-width: 0px;">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </x:TemplateField>
                                            <x:BoundField DataField="Remark" HeaderText="备注" />
                                        </Columns>
                                    </x:Grid>
                                </Items>
                            </x:Tab>
                        </Tabs>
                    </x:TabStrip>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="选择" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent" IsModal="true" Width="520px"
        Height="450px" WindowPosition="Center">
    </x:Window>
    <x:HiddenField runat="server" ID="hfGrid1Summary">
    </x:HiddenField>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

<script src="/Res/Jscript/PageList.js" type="text/javascript"></script>

<script type="text/javascript">

    function CalcAmt(o, p) {
        var qty;
        var price;
        var a;
        if (p == 0) {
            qty = o.value;
            price = document.getElementById(o.id.replace("txtQty", "txtPrice").replace("_c4r", "_c5r")).value;
            a = document.getElementById(o.id.replace("txtQty", "txtAmt").replace("_c4r", "_c6r"));
            if (isNaN(qty))
                o.value = 0;
        }
        else {
            qty = document.getElementById(o.id.replace("txtPrice", "txtQty").replace("_c5r", "_c4r")).value;
            price = o.value;
            a = document.getElementById(o.id.replace("txtPrice", "txtAmt").replace("_c5r", "_c6r"));
            if (isNaN(price))
                o.value = 0;
        }
        var v = qty * price;
        if (isNaN(v)) {
            v = 0;
        }
        a.value = v.toFixed(2);
    }
</script>

<script type="text/javascript">
    var gridClientID = '<%= Grid1.ClientID %>';
    var gridSummaryID = '<%= hfGrid1Summary.ClientID %>';

    function calcGridSummary(grid) {
        var donateTotal = 0, store = grid.getStore(), view = grid.getView(), storeCount = store.getCount();

        // 防止重复添加了合计行
        if (Ext.get(view.getRow(storeCount - 1)).hasClass('mygrid-row-summary')) {
            return;
        }

        // 从隐藏字段获取全部数据的汇总
        var summaryJSON = JSON.parse(X(gridSummaryID).getValue());
        store.add(new Ext.data.Record({
            'MaterialName': '合计：',
            'Qty': summaryJSON['Qty'],
            'Amt': summaryJSON['Amt'].toFixed(2)
        }));

        // 为合计行添加自定义样式（隐藏序号列、复选框列，取消 hover 和 selected 效果）
        Ext.get(view.getRow(storeCount)).addClass('mygrid-row-summary');
    }

    function onReady() {
        var win = Ext.getCmp('<%= Window1.ClientID %>');
        var L = parent.Ext.getBody().getSize().width - win.getWidth();
        win.x_property_left = L;
        win.x_property_top = 0;

        var grid = X(gridClientID);
        grid.on('viewready', function() {
            registerSelectEvent();
            calcGridSummary(grid);
        });

        // 防止选中合计行
        grid.getSelectionModel().addListener('beforerowselect', function(sm, rowIndex, keepExisting, record) {
            if (Ext.get(grid.getView().getRow(rowIndex)).hasClass('mygrid-row-summary')) {
                return false;
            }
            return true;
        });       

        PageList.grid1ClientID = '<%= Grid1.ClientID %>';
        PageList.window1ClientID = '<%= Window1.ClientID %>';
        PageList.setOnReady();
    }

    function onAjaxReady() {
        registerSelectEvent();
        PageList.setOnAjaxReady();

        calcGridSummary(X(gridClientID));
    }

    function registerSelectEvent() {
        var grid = X(gridClientID);
        grid.el.select('.x-grid-tpl input').on('click', function(evt, el) {
            el.select();
        });
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

