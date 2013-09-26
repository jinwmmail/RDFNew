<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Flow_InstanceView.aspx.cs"
    Inherits="RDFNew.Web.Admin.Flow.ToDoM.Flow_InstanceView" %>

<%@ Register Assembly="dswbu" Namespace="DS.Web.UI" TagPrefix="x" %>
<%@ Register Assembly="DS.XBPM.Design.Web" Namespace="DS.XBPM.Design.Web" TagPrefix="x" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
		<x:XScriptManager ID="XScriptManager1" runat="server">
		</x:XScriptManager>
		<fieldset>
			<legend>流程走向直观图</legend>
			<x:WorkflowViewer runat="server" ID="WorkflowViewer1" Width="100%" Height="600px">
			</x:WorkflowViewer>
		</fieldset>
		<br />
		<fieldset>
			<legend>流程走向数据项</legend>头记录:
			<br />
			<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
				<Columns>
					<asp:BoundField DataField="Happen" HeaderText="发生时间" HtmlEncode="false" DataFormatString="{0:yyyy-MM-dd HH:mm:ss fff}" />
					<asp:BoundField DataField="Result" HeaderText="状态" />
					<asp:BoundField DataField="Description" HeaderText="描述" />
				</Columns>
			</asp:GridView>
			<br />
			<br />
			明细记录:
			<br />
			<asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridView2_RowDataBound">
				<Columns>
					<asp:BoundField DataField="Happen" HeaderText="发生时间" HtmlEncode="false" 
					    DataFormatString="{0:yyyy-MM-dd HH:mm:ss fff}" />
					<asp:BoundField DataField="Name" HeaderText="元素名称" />
					<asp:BoundField DataField="Type" HeaderText="元素类型" />
					<asp:BoundField DataField="FlowNode" HeaderText="所属流程节点" />
					<asp:BoundField DataField="Result" HeaderText="状态" />
					<asp:BoundField DataField="Description" HeaderText="描述" />
				</Columns>
			</asp:GridView>
		</fieldset>
	</form>
</body>
</html>
