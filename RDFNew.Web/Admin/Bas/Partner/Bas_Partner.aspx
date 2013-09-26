<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_Partner.aspx.cs" Inherits="RDFNew.Web.Admin.Bas.Partner.Bas_Partner" %>

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
                                    <x:TextBox ID="txtPartnerID" runat="server" Text="" Width="75px" Label="伙伴编号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPartnerName" runat="server" Text="" Label="伙伴名称" NextFocusControl=""
                                        Required="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtAddress" runat="server" Text="" Label="地址" NextFocusControl="">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>                            
                            <x:FormRow>
                                <Items>                                    
                                    <x:DropDownList ID="ddlPartnerTypeID" runat="server" Label="伙伴类别" Width="75px">
                                    </x:DropDownList>
                                </Items>
                            </x:FormRow>       
                                                        
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtLinker" runat="server" Text="" Label="联系人" NextFocusControl="">
                                    </x:TextBox>
                                    <x:TextBox ID="txtTel" runat="server" Text="" Label="电话" NextFocusControl="">
                                    </x:TextBox>
                                    <x:TextBox ID="txtFax" runat="server" Text="" Label="传真" NextFocusControl="">
                                    </x:TextBox>                                                                        
                                </Items>
                            </x:FormRow>                             
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPhone" runat="server" Text="" Label="手机" NextFocusControl="">
                                    </x:TextBox>
                                    <x:TextBox ID="txtEmail" runat="server" Text="" Label="邮件地址" NextFocusControl="">
                                    </x:TextBox>
                                    <x:TextBox ID="txtQQ" runat="server" Text="" Label="QQ" NextFocusControl="">
                                    </x:TextBox>                                                                        
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

