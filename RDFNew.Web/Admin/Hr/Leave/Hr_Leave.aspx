<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hr_Leave.aspx.cs" Inherits="RDFNew.Web.Admin.Hr.Leave.Hr_Leave" %>

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
                            <x:FormRow ColumnWidths="168 168">
                                <Items>
                                    <x:TextBox ID="txtLeaveID" runat="server" Text="" Label="请假单号" NextFocusControl=""
                                        EmptyText="为空则自动编号" Regex="^[A-Za-z\-_0-9]+$" RegexMessage="只能输入字母、数字、-和_,不能含有中文字符.">
                                    </x:TextBox>
                                    <x:DatePicker ID="txtLeaveDate" runat="server" Text="" Label="制单日期" NextFocusControl=""
                                        Required="true">
                                    </x:DatePicker>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="168 168">
                                <Items>
                                    <x:TriggerBox ID="txtEmployeeID" runat="server" EnablePostBack="false" TriggerIcon="Search"
                                        Label="员工编号" EnableEdit="false" Required="true">
                                    </x:TriggerBox>
                                    <x:TextBox ID="txtEmployeeName" runat="server" Text="" Label="员工姓名" Readonly="true"
                                        CssStyle="background:#c0c0c0;">
                                    </x:TextBox>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:DropDownList ID="ddlLeaveTypeID" runat="server" Label="假期类别" Width="100px">
                                    </x:DropDownList>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="35% 15% 35% 15%">
                                <Items>
                                    <x:DatePicker ID="txtDTFrom" runat="server" Text="" Label="开始时间" NextFocusControl=""
                                        Required="true">
                                    </x:DatePicker>
                                    <x:TimePicker ID="txtDTFromT" runat="server" ShowLabel="false" Required="true">
                                    </x:TimePicker>
                                    <x:DatePicker ID="txtDTTo" runat="server" Text="" Label="结束时间" NextFocusControl=""
                                        Required="true">
                                    </x:DatePicker>
                                    <x:TimePicker ID="txtDTToT" runat="server" ShowLabel="false" Required="true">
                                    </x:TimePicker>
                                </Items>
                            </x:FormRow>
                            <x:FormRow ColumnWidths="168 100%">
                                <Items>
                                    <x:NumberBox ID="txtTimes" runat="server" Label="请假时间" DecimalPrecision="0" NoNegative="true"
                                        NextFocusControl="" CssStyle="text-align:right;background:#c0c0c0;" Readonly="true"
                                        Width="100">
                                    </x:NumberBox>
                                    <x:Label ID="Label2" runat="server" Text="(分钟)" ShowLabel="false" CssStyle="padding-top:3px;">
                                    </x:Label>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtCause" runat="server" Text="" Height="45px" Label="请假事由">
                                    </x:TextArea>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:TextArea ID="txtRemark" runat="server" Text="" Height="45px" Label="备注">
                                    </x:TextArea>
                                </Items>
                            </x:FormRow>
                            <x:FormRow>
                                <Items>
                                    <x:Label ID="Label1" runat="server" Label="考勤规则" EncodeText="false" CssStyle=" color:blue; "
                                        Text="法定节假日休息;<br/>上 下 班 时 间:8:30-17:00;<br/>午休午 饭时间:12:30-13:00">
                                    </x:Label>
                                </Items>
                            </x:FormRow>
                        </Rows>
                    </x:Form>
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

