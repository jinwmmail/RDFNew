<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_MaterialList.aspx.cs"
    Inherits="RDFNew.Web.Admin.Bas.Material.Bas_MaterialList" %>

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
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="MaterialID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="物料编号" DataTextField="MaterialID" SortField="Bas_Material.MaterialID" Width="75"
                                CommandName="View" />
                            <x:BoundField Width="200px" DataField="MaterialName" SortField="Bas_Material.MaterialName"  HeaderText="物料名称"  />
                            <x:BoundField Width="200px" DataField="MaterialSpec" SortField="Bas_Material.MaterialSpec" HeaderText="物料规格" />
                            <x:BoundField Width="45px" DataField="UnitID" SortField="Bas_Material.UnitID" HeaderText="计量<br/>单位"
                                TextAlign="Center" />
                            <x:BoundField Width="75px" DataField="PricePur" SortField="Bas_Material.PricePur" HeaderText="采购单价"
                                TextAlign="Right" DataFormatString="{0:#.##}" />
                            <x:BoundField Width="75px" DataField="PriceCost" SortField="Bas_Material.PriceCost" HeaderText="成本单价"
                                TextAlign="Right" DataFormatString="{0:#.##}" />
                            <x:BoundField Width="75px" DataField="PriceSal" SortField="Bas_Material.PriceSal" HeaderText="销售单价"
                                TextAlign="Right" DataFormatString="{0:#.##}" />                           
                            <x:CheckBoxField Width="35px" RenderAsStaticField="true" DataField="Enabled" SortField="Bas_Material.Enabled" HeaderText="是否<br/>有效"
                                TextAlign="Center" />
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

