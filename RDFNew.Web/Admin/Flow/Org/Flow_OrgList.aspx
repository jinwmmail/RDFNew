<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_OrgList.aspx.cs" Inherits="RDFNew.Web.Admin.Flow.Org.Flow_OrgList" %>

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
                                    <x:TextBox ID="txtOrgID" runat="server" Text="" Width="60px" Label="组织编号" NextFocusControl="txtOrgName">
                                    </x:TextBox>
                                    <x:TextBox ID="txtOrgName" runat="server" Text="" Width="160px" Label="组织名称" NextFocusControl="txtOrgID">
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
                    <x:Grid ID="Grid1" Title="组织列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="OrgID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="组织编号" DataTextField="OrgID" Width="95"
                                CommandName="View" />
                            <x:BoundField DataField="OrgName" Hidden="true" /> 
                            <x:BoundField DataField="RID" Hidden="true" /> 
                            <x:BoundField Width="300px" DataField="OrgName" HeaderText="组织名称"
                                DataSimulateTreeLevelField="TreeLevel" DataFormatString="{0}" HtmlEncodeFormatString="false"/>
                            <x:BoundField Width="150px" DataField="RoleName" HeaderText="主管角色" />                                                                
                            <x:BoundField Width="100px" DataField="Manager" HeaderText="主管" />
                            <x:BoundField Width="75px" DataField="JobName" HeaderText="主管职位" />                                
                            <x:BoundField Width="100px" DataField="Remark" HeaderText="备注" />
                            <x:BoundField Width="55px" DataField="TreeLevel" HeaderText="节点<br/>层次" TextAlign="Center" />
                            <x:BoundField DataField="_Type" Hidden="true"/>
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


