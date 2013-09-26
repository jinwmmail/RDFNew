<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_Employee.aspx.cs" Inherits="RDFNew.Web.Admin.Bas.Employee.Bas_Employee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../../../Res/ImageView/CJL.0.1.min.js"></script>

    <script type="text/javascript" src="../../../Res/ImageView/QuickUpload.js"></script>

    <script type="text/javascript" src="../../../Res/ImageView/ImagePreview.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <x:HiddenField ID="hidTreeLevel" runat="server">
    </x:HiddenField>
    <x:HiddenField ID="hidPID" runat="server">
    </x:HiddenField>
    <x:Panel ID="Panel1" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
        ShowBorder="false" ShowHeader="false" Title="Panel" Layout="Row">
        <Toolbars>
            <x:Toolbar ID="Toolbar1" runat="server">
            </x:Toolbar>
        </Toolbars>
        <Items>
            <x:Panel ID="Panel2" runat="server" BodyPadding="0px" EnableBackgroundColor="false"
                Layout="HBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start" ShowBorder="false"
                ShowHeader="false" Height="260px">
                <Items>
                    <x:Form ID="Form2" runat="server" BoxFlex="1" ShowBorder="False" BodyPadding="3px"
                        EnableBackgroundColor="false" ShowHeader="False" LabelWidth="60px">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtEmployeeID" runat="server" Text="" Width="100px" Label="雇员工号" NextFocusControl="txtEmployeeName"
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtEmployeeName" runat="server" Text="" Label="中文姓名" NextFocusControl="txtNameE"
                                        Required="true" FocusOnPageLoad="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtNameE" runat="server" Text="" Label="外文名字" NextFocusControl="txtPhone">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtPhone" runat="server" Text="" Label="联系电话" NextFocusControl="txtEmail">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtEmail" runat="server" Text="" Label="邮件地址" NextFocusControl="txtRemark">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TriggerBox ID="txtDeptID" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                        Label="部门编号" EnableEdit="false">
                                    </x:TriggerBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtDeptName" runat="server" Text="" Label="部门名称" Readonly="true" CssStyle="background:#c0c0c0;">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <x:CheckBox ID="ckbEnabled" runat="server" Label="在职">
                                    </x:CheckBox>
                                    <x:CheckBox ID="ckbIsUser" runat="server" Label="为用户">
                                    </x:CheckBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtEmployeeID">
                                    </x:TextArea>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                    <x:Form ID="Form3" runat="server" BoxFlex="1" ShowBorder="false" ShowHeader="false"
                        LabelWidth="5px" BodyStyle="padding-left:5px;padding-top:5px;">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:Label runat="server" ShowLabel="false" Text="雇员头像">
                                    </x:Label>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:ContentPanel ID="ContentPanel1" runat="server" ShowBorder="false" ShowHeader="false"
                                        EnableBackgroundColor="true" BodyStyle="margin-left:10px;" Width="210px">
                                        <asp:Image ID="idImg" runat="server" Height="150px" />
                                    </x:ContentPanel>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:ContentPanel ID="ContentPanel2" runat="server" ShowBorder="false" ShowHeader="false"
                                        BodyStyle="padding:3px; padding-left:10px;">
                                        <asp:FileUpload ID="idFile" name="pic" runat="server" />
                                    </x:ContentPanel>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:CheckBox ID="ckbDeleteImg" runat="server" Text="删除头像">
                                    </x:CheckBox>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Panel>
        </Items>
    </x:Panel>
    <x:Window ID="Window1" Title="选择" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent" IsModal="true" Width="550px"
        Height="450px" WindowPosition="GoldenSection">
    </x:Window>
    </form>
</body>
</html>
<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

<script type="text/javascript">
    var ip = new ImagePreview($$("<%=this.idFile.ClientID %>"), $$("<%=this.idImg.ClientID %>"), {
        maxWidth: 200, maxHeight: 150,
        action: "../../Res/ImageView/ImagePreview.ashx",
        onCheck: CheckPreview
    });
    var exts = "jpeg|jpg|png|gif|bmp", paths = "|";
    if (!RegExp("\.(?:" + exts + ")$$", "i").test(ip.img.src))
        ip.img.src = ImagePreview.TRANSPARENT;
    ip.file.onchange = function() { ip.preview(); };

    //检测程序
    function CheckPreview() {
        var value = this.file.value, check = true;
        if (!value) {
            check = false; alert("请先选择文件！");
        } else if (!RegExp("\.(?:" + exts + ")$$", "i").test(value)) {
            check = false; alert("只能上传以下类型：" + exts);
        } else if (paths.indexOf("|" + value + "|") >= 0) {
            check = false; alert("已经有相同文件！");
        }
        return check;
    }
</script>

<script type="text/javascript">
    function onReady() {
        var win = Ext.getCmp('<%= Window1.ClientID %>');
        var L = parent.Ext.getBody().getSize().width - win.getWidth();
        win.x_property_left = L;
        win.x_property_top = 0;
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }     
</script>

