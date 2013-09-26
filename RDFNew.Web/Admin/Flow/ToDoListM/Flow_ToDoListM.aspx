<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_ToDoListM.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.ToDoListM.Flow_ToDoListM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    </style>
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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="false"
                Layout="Fit" Height="140">
                <Items>
                    <x:TabStrip ID="TabStrip1" runat="server" ShowBorder="false">
                        <Tabs>
                            <x:Tab ID="Tab1" runat="server" Layout="Fit" Title="主表信息">
                                <Items>
                                    <x:Form ID="Form2" ShowBorder="False" BodyPadding="3px" EnableBackgroundColor="false"
                                        ShowHeader="False" runat="server" LabelWidth="60px">
                                        <Rows>
                                            <x:FormRow >
                                                <Items>
                                                    <x:TextBox ID="txtToDoListMID" runat="server" Text="" Label="流程编号" NextFocusControl=""
                                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                                    </x:TextBox>                                                    
                                                </Items>
                                            </x:FormRow>                                         
                                            <x:FormRow>
                                                <Items>
                                                    <x:TextArea ID="txtDescription" runat="server" Text="" Height="45px" Label="描述" NextFocusControl="">
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
                                        AllowPaging="false" runat="server" EnableMultiSelect="false" DataKeyNames="ToDoListDID"
                                        EnableRowNumber="True" EnableTextSelection="true" OnRowCommand="Grid1_RowCommand"
                                        OnRowDataBound="Grid1_RowDataBound">
                                        <Columns>
                                            <x:LinkButtonField ID="LinkButtonField1" runat="server" HeaderText="序号" DataTextField="Seq"
                                                Width="45" CommandName="View" />                                            
                                            <x:BoundField DataField="Description" HeaderText="描述" />
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
    <x:Window ID="Window1" Title="选择" Popup="false" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Parent" IsModal="true" Width="520px"
        Height="450px" WindowPosition="Center">
    </x:Window>
    </form>
</body>
</html>

<script src="/Res/Jscript/PageDetail.js" type="text/javascript"></script>

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
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#";
    }          
</script>

