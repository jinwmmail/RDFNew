<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Notice.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Notice.Sys_Notice"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/ueditor/themes/default/ueditor.css" />
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
            <x:Region ID="Region2" runat="server" Position="Top" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="60">
                <Items>
                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" ShowHeader="False" runat="server">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtNoticeID" runat="server" Text="" Width="100px" Label="公告编号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtNoticeTitle" runat="server" Text="" Label="公告标题" NextFocusControl=""
                                        Required="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
                </Items>
            </x:Region>
            <x:Region ID="Region1" runat="server" Position="Center" ShowBorder="false" ShowHeader="false"
                Layout="Fit">
                <Items>
                    <x:TextArea ID="txtNoticeContent" runat="server" Label="公告内容">
                    </x:TextArea>
                </Items>
            </x:Region>
            <x:Region ID="Region3" runat="server" Position="Bottom" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="30">
                <Items>
                    <x:Form ID="Form4" ShowBorder="False" BodyPadding="3px" ShowHeader="False" runat="server">
                        <Rows>
                            <x:FormRow>
                                <Items>
                                    <x:CheckBox ID="ckbEnabled" runat="server" Label="已发布">
                                    </x:CheckBox>
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

<script type="text/javascript" src="/Res/Jscript/PageDetail.js"></script>

<script type="text/javascript">
    window.UEDITOR_HOME_URL = '<%= ResolveUrl("~/ueditor/") %>';
</script>

<script type="text/javascript" src="/ueditor/ueditor.config.js"></script>

<script type="text/javascript" src="/ueditor/ueditor.all.min.js"></script>

<script type="text/javascript">
    var rw = '<%= GetAction() %>' == "VIEW" ? true : false;
    var editor;
    function onReady() {
        editor = new UE.ui.Editor({
            //autoFloatEnabled: false,
            //initialFrameWidth: 650,
            //initialFrameHeight: 250,
            // minFrameHeight: 250,
            autoHeightEnabled: false,
            maximumWords: 8000,
            readonly: rw,
            toolbars: [
            ['fullscreen', 'source', '|', 'undo', 'redo', '|',
                'bold', 'italic', 'underline', 'formatmatch', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
                'rowspacingtop', 'rowspacingbottom', 'lineheight', '|', 'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
                'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
                'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
                'insertimage', 'emotion', '|',
                'horizontal']
            ]

        });
        editor.render('<%= txtNoticeContent.ClientID %>');
    }

    // 提交数据之前同步到表单隐藏字段
    X.util.beforeAjaxPostBackScript = function() {
        editor.sync();
    };

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

