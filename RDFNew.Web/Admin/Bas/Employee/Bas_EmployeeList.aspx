<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bas_EmployeeList.aspx.cs"
    Inherits="RDFNew.Web.Admin.Bas.Employee.Bas_EmployeeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">

    </script>

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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                Title="查询条件" Height="60px" EnableCollapse="true">
                <Items>
                    <x:Form ID="FormQuery" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="130px 230px 230px">
                                <Items>
                                    <x:TextBox ID="txtEmployeeID" runat="server" Text="" Width="60px" Label="雇员工号"
                                        NextFocusControl="txtEmployeeName">
                                    </x:TextBox>
                                    <x:TextBox ID="txtEmployeeName" runat="server" Text="" Width="160px" Label="中文姓名" NextFocusControl="txtNameE">
                                    </x:TextBox>
                                    <x:TextBox ID="txtNameE" runat="server" Text="" Width="160px" Label="外文名字" NextFocusControl="txtEmployeeID">
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
                    <x:Grid ID="Grid1" Title="雇员列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="EmployeeID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="雇员工号" DataTextField="EmployeeID" SortField="Bas_Employee.EmployeeID"
                                Width="75" CommandName="View" />
                            <x:BoundField Width="120px" DataField="EmployeeName"  SortField="Bas_Employee.EmployeeName" HeaderText="中文姓名" />
                            <x:BoundField Width="100px" DataField="NameE"  SortField="Bas_Employee.NameE" HeaderText="英文名称" />
                            <x:BoundField Width="100px" DataField="DeptName"  SortField="b.DeptName" HeaderText="所在部门" />
                            <x:TemplateField Width="55px" HeaderText="头像">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImageS")%>' Width="50px" Height="50px"
                                        Visible='<%# GetImageVisible(Eval("ImageS")) %>' Style="cursor: pointer;" ToolTip="点击查看高清图片."
                                        onclick='PageList.showItemImage(this);'></asp:Image>
                                </ItemTemplate>
                            </x:TemplateField>                                                          
                            <x:CheckBoxField Width="35px" RenderAsStaticField="true" DataField="Enabled"  SortField="Bas_Employee.Enabled" HeaderText="在职"
                                TextAlign="Center" />
                            <x:CheckBoxField Width="35px" RenderAsStaticField="true" DataField="IsUser"  SortField="Bas_Employee.IsUser" HeaderText="为<br/>用户"
                                TextAlign="Center" />                                
                            <x:BoundField DataField="Remark" HeaderText="备注" />
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

    function btnPrint_onclick() {
        var p1 = Ext.getCmp('<%= txtEmployeeID.ClientID %>');
        var p2 = Ext.getCmp('<%= txtEmployeeName.ClientID %>');
        var p3 = Ext.getCmp('<%= txtNameE.ClientID %>');
        var win = top.Ext.getCmp('<%= Window1.ClientID %>');
        win.box_show("Bas/Employee/Bas_EmployeeListPrint.aspx?action=print&pm1=" +
            p1.getValue() + "&pm2=" + p2.getValue() + "&pm3=" + p3.getValue(), "打印-[雇员]");
    }

    //以下必须放在最后面
    if (self == top) {
        top.location.href = window.location.protocol + "//" + window.location.host + '<%= System.Web.Security.FormsAuthentication.DefaultUrl %>' + "#" + window.location.pathname;
    }   
    
</script>
