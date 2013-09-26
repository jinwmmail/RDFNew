<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_Type.aspx.cs" Inherits="RDFNew.Web.Admin.Bas.Type.Bas_Type" %>

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
                                    <x:TextBox ID="txtTypeID" runat="server" Text="" Width="100px" Label="类别行号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="170 150">
                                <Items>
                                    <x:TextBox ID="txtTypeGroup" runat="server" Text="" Label="类别大类" NextFocusControl=""
                                        Required="true">
                                    </x:TextBox>
                                    <x:DropDownList ID="ddlTypeGroup" runat="server" ShowLabel="false"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTypeGroup_SelectedIndexChanged">
                                    </x:DropDownList>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtSeq" runat="server" Text="" Width="100px" Label="类别编号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtTypeName" runat="server" Text="" Width="160px" Label="类别名称" NextFocusControl=""
                                        Required="true">
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

