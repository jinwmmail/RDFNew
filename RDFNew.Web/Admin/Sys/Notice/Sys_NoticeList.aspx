<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_NoticeList.aspx.cs" Inherits="RDFNew.Web.Admin.Sys.Notice.Sys_NoticeList" %>

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
            <x:Region ID="Region1" runat="server" Position="Top" ShowBorder="false" ShowHeader="true" BodyPadding="5px"
                Title="查询条件" Height="60px" EnableCollapse="true">
                <Items>
                    <x:Form ID="FormQuery" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="60px">
                        <Rows>
                            <x:FormRow ColumnWidths="200 200">
                                <Items>
                                    <x:TextBox ID="txtNoticeID" runat="server" Text="" Label="公告编号"
                                        NextFocusControl="">
                                    </x:TextBox>                                    
                                    <x:TextBox ID="txtNoticeTitle" runat="server" Text="" Label="公告标题"
                                        NextFocusControl="">
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
                    <x:Grid ID="Grid1" Title="公告列表" PageSize="50" ShowBorder="false" ShowHeader="true"
                        AutoHeight="true" AllowPaging="true" runat="server" EnableMultiSelect="false"
                        DataKeyNames="NoticeID" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                        OnSort="Grid1_Sort" EnableRowNumber="True" AllowSorting="true" EnableTextSelection="true"
                        OnRowCommand="Grid1_RowCommand">
                        <Columns>
                            <x:LinkButtonField runat="server" HeaderText="公告编号" DataTextField="NoticeID" SortField="Sys_Notice.NoticeID"
                                Width="100" CommandName="View" />                            
                            <x:BoundField Width="500px" DataField="NoticeTitle" SortField="Sys_Notice.NoticeTitle" HeaderText="公告标题" />                            
                            <x:CheckBoxField Width="45px" RenderAsStaticField="true" DataField="Enabled" SortField="Sys_Notice.Enabled" HeaderText="已<br/>发布"
                                TextAlign="Center" />                            
                            <x:BoundField Width="130px" DataField="CrtOn" SortField="Sys_Notice.CrtOn" HeaderText="建档时间" />
                        </Columns>
                    </x:Grid>
                </Items>
            </x:Region>
        </Regions>
    </x:RegionPanel>
    <x:Window ID="Window1" Title="弹出窗体" Popup="false" EnableIFrame="true" IFrameUrl="about:blank"
        EnableMaximize="true" Target="Self" EnableResize="true" runat="server" IsModal="true" 
        WindowPosition="GoldenSection" Width="550px" EnableConfirmOnClose="true" Height="450px" >
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
