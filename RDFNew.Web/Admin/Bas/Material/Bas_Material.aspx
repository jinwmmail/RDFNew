<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_Material.aspx.cs" Inherits="RDFNew.Web.Admin.Bas.Material.Bas_Material" %>

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
            <x:Region ID="Region1" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                        ShowHeader="False" runat="server" LabelWidth="60px">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMaterialID" runat="server" Text="" Width="75px" Label="物料编号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMaterialName" runat="server" Text="" Label="物料名称" NextFocusControl=""
                                        Required="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMaterialSpec" runat="server" Text="" Label="物料规格" NextFocusControl="">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtUnitID" runat="server" Text="" Width="75px" Label="计量单位" NextFocusControl="">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:NumberBox ID="txtPricePur" runat="server" Label="采购单价" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px">
                                    </x:NumberBox>
                                    <x:NumberBox ID="txtPriceCost" runat="server" Label="成本单价" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px">
                                    </x:NumberBox>
                                    <x:NumberBox ID="txtPriceSal" runat="server" Label="销售单价" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px">
                                    </x:NumberBox>                                                                        
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:CheckBox ID="ckbEnabled" runat="server" Label="是否有效">
                                    </x:CheckBox>
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
            </x:Region>
        </Regions>
    </x:RegionPanel>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

<script type="text/javascript">
    function onReady() {
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

