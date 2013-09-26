<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_DeployM.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.DeployM.Flow_DeployM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1"></x:PageManager>
    <x:HiddenField ID="hidDeployKey" runat="server">
    </x:HiddenField>
    <x:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
        <Toolbars>
            <x:Toolbar ID="Toolbar1" runat="server">
                <Items>
                </Items>
            </x:Toolbar>
        </Toolbars>
        <Regions>
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="100">
                <Items>
                    <x:TabStrip ID="TabStrip1" runat="server" ShowBorder="false">
                        <Tabs>
                            <x:Tab ID="Tab1" runat="server" Layout="Fit" Title="主表信息">
                                <Items>
                                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                                        ShowHeader="False" runat="server" LabelWidth="60px">
                                        <Rows>
                                            <x:FormRow ColumnWidths="160 200 200">
                                                <Items>
                                                    <x:TextBox ID="txtDeployMID" runat="server" Text="" Label="部署编号" NextFocusControl="txtDeployName"
                                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符."
                                                        Required="true">
                                                    </x:TextBox>
                                                    <x:TextBox ID="txtDeployName" runat="server" Text="" Label="部署名称" NextFocusControl="txtFileName"
                                                        Required="true" FocusOnPageLoad="true">
                                                    </x:TextBox>
                                                    <x:TextBox ID="txtFileName" runat="server" Text="" Label="文件名称" NextFocusControl="txtDeployMID"
                                                        Required="true" FocusOnPageLoad="true">
                                                    </x:TextBox>
                                                </Items>
                                            </x:FormRow>
                                            <x:FormRow>
                                                <Items>
                                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="30px" Label="备注">
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
                                </Toolbars>
                                <Items>
                                    <x:Grid ID="Grid1" ShowHeader="false" PageSize="50" ShowBorder="false" AutoHeight="true"
                                        AllowPaging="false" runat="server" EnableMultiSelect="false" DataKeyNames=""
                                        EnableRowNumber="True" EnableTextSelection="true" OnPreRowDataBound="Grid1_PreRowDataBound">
                                        <Columns>
                                            <x:BoundField DataField="ProcessName" HeaderText="流程名称" Width="120" />
                                            <x:BoundField DataField="Name" HeaderText="活动名称" Width="120" />
                                            <x:BoundField DataField="Description" HeaderText="活动描述" Width="150" />
                                            <x:TemplateField HeaderText="操作人员" Width="75px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOperator" runat="server" Width="95%"></asp:TextBox>
                                                </ItemTemplate>
                                            </x:TemplateField>                                            
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

