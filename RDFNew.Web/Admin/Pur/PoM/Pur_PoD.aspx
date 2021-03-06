﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pur_PoD.aspx.cs" Inherits="RDFNew.Web.Admin.Pur.PoM.Pur_PoD" %>

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
                                    <x:TextBox ID="txtSeq" runat="server" Text="" Label="序号" Regex="^[0-9]{4}$" RegexMessage="只能输入4位数字,不能含有中文字符."
                                        Required="true" Width="60" MaxLength="4" MinLength="4">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMaterialID" runat="server" Text="" Label="物料编号" Width="100" NextFocusControl=""
                                        Readonly="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtMaterialName" runat="server" Text="" Label="物料名称" NextFocusControl=""
                                        Readonly="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtUnitID" runat="server" Text="" Label="单位" Width="50" NextFocusControl=""
                                        Readonly="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:NumberBox ID="txtQty" runat="server" Label="采购数量" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px"                                        
                                        >
                                    </x:NumberBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:NumberBox ID="txtPrice" runat="server" Label="采购单价" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px"                                        
                                        >
                                    </x:NumberBox>
                                </Items>
                            </x:FormRow>                            
                            <x:FormRow>
                                <Items>
                                    <x:NumberBox ID="txtAmt" runat="server" Label="金额" DecimalPrecision="2" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;" Width="75px" Readonly="true"                                        
                                        >
                                    </x:NumberBox>
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
        var c;
        c = Ext.getCmp('<%=this.txtQty.ClientID %>');
        c.on('change', function() { CalcAmt(); });
        c = Ext.getCmp('<%=this.txtPrice.ClientID %>');
        c.on('change', function() { CalcAmt(); });
    }

    function CalcAmt() {
        var qty = document.getElementById('<%=this.txtQty.ClientID %>').value;
        var price = document.getElementById('<%=this.txtPrice.ClientID %>').value;
        var a = document.getElementById('<%=this.txtAmt.ClientID %>');
        var v = qty * price;
        if (isNaN(v)) {
            v = 0;
        }
        a.value = v.toFixed(2);
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

