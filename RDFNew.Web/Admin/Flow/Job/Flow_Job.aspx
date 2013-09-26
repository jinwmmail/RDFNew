<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_Job.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.Job.Flow_Job" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                                    <x:TextBox ID="txtJobID" runat="server" Text="" Width="100px" Label="职位代码"
                                        NextFocusControl="txtJobName" EmptyText="只能输入数字" Regex="^[0-9]+$" RegexMessage="只能输入数字." Required="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>                            
                            <x:FormRow>
                                <Items>
                                    <x:TextBox ID="txtJobName" runat="server" Text="" Width="160px" Label="职位名称" NextFocusControl="txtRemark"
                                        Required="true" FocusOnPageLoad="true">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>                            
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注" NextFocusControl="txtJobID">
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
