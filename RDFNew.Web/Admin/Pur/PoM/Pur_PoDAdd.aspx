<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pur_PoDAdd.aspx.cs" Inherits="RDFNew.Web.Admin.Pur.PoM.Pur_PoDAdd" %>

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
                            <x:FormRow ColumnWidths="130px 230px">
                                <Items>
                                    <x:TextBox ID="txtMaterialID" runat="server" Text="" Width="60px" Label="物料编号" NextFocusControl="txtMaterialName">
                                    </x:TextBox>
                                    <x:TextBox ID="txtMaterialName" runat="server" Text="" Width="160px" Label="物料名称"
                                        NextFocusControl="txtMaterialID">
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
                    <x:Grid ID="Grid1" Title="物料列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="true"
                        EnableCheckBoxSelect="true" DataKeyNames="MaterialID" IsDatabasePaging="true"
                        OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort" EnableRowNumber="True"
                        AllowSorting="true" EnableTextSelection="true">
                        <Columns>
                            <x:BoundField Width="75" DataField="MaterialID" SortField="Bas_Material.MaterialID" HeaderText="物料编号" />
                            <x:BoundField Width="120" DataField="MaterialName" SortField="Bas_Material.MaterialName" HeaderText="物料名称" />                            
                            <x:BoundField Width="35" DataField="UnitID" SortField="Bas_Material.UnitID" HeaderText="单位" />
                            <x:TemplateField HeaderText="数量" Width="65px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" Width="50px" TabIndex='<%# Container.DataItemIndex + 10 %>'
                                        Text='1' Style="text-align: right;"></asp:TextBox>
                                </ItemTemplate>
                            </x:TemplateField>
                            <x:TemplateField HeaderText="采购<br/>单价" Width="65px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPrice" runat="server" Width="50px" TabIndex='<%# Container.DataItemIndex + 10 %>'
                                        Text='<%# Eval("PricePur","{0:0.00}") %>' Style="text-align: right;"></asp:TextBox>
                                </ItemTemplate>
                            </x:TemplateField>
                            <x:BoundField Width="120" DataField="MaterialSpec" HeaderText="物料规格" />
                            <x:BoundField DataField="Remark" HeaderText="备注" />
                        </Columns>
                    </x:Grid>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    </form>
</body>
</html>

<script type="text/javascript">
    var gridClientID = '<%= Grid1.ClientID %>';

    function registerSelectEvent() {
        var grid = X(gridClientID);
        grid.el.select('.x-grid-tpl input').on('click', function(evt, el) {
            el.select();
        });
    }

    function onReady() {
        var grid = X(gridClientID);

        grid.on('viewready', function() {
            registerSelectEvent();
        });
    }

    function onAjaxReady() {
        registerSelectEvent();
    }
</script>

