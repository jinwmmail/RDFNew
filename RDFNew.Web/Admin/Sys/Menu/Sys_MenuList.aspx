<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_MenuList.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Menu.Sys_MenuList" %>

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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="true"
                BodyPadding="5px" Title="查询条件" Height="60px" EnableCollapse="true">
                <Items>
                    <x:Form ID="FormQuery" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="130px 230px">
                                <Items>
                                    <x:TextBox ID="txtMenuID" runat="server" Text="" Width="60px" Label="菜单代码" NextFocusControl="txtMenuName">
                                    </x:TextBox>
                                    <x:TextBox ID="txtMenuName" runat="server" Text="" Width="160px" Label="中文描述" NextFocusControl="txtMenuID">
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
                    <x:Grid ID="Grid1" Title="菜单列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="MenuID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="菜单代码" DataTextField="MenuID"
                                Width="120px" CommandName="View" />      
                            <x:BoundField DataField="MenuName" Hidden="true" /> 
                            <x:BoundField DataField="RID" Hidden="true" />                                                   
                            <x:BoundField  Width="200px" DataField="MenuName" DataSimulateTreeLevelField="TreeLevel" DataFormatString="{0}"
                                HeaderText="菜单名称" ExpandUnusedSpace="false" />                            
                            <x:BoundField Width="120px" DataField="ModuleID" HeaderText="模块代码" />                                                       
                            <x:ImageField Width="40px" DataImageUrlField="IconUrl" DataImageUrlFormatString="{0}" TextAlign="Center"
                                HeaderText="图标"></x:ImageField>
                            <x:CheckBoxField Width="35px" RenderAsStaticField="true" DataField="Enabled" HeaderText="启用"
                                TextAlign="Center" />
                            <x:CheckBoxField Width="35px" RenderAsStaticField="true" DataField="Visibled" HeaderText="可见"
                                TextAlign="Center" />
                            <x:BoundField Width="100px" DataField="MenuID" HeaderText="菜单代码" Hidden="true" />
                            <x:BoundField DataField="Remark"  HeaderText="备注" />
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
