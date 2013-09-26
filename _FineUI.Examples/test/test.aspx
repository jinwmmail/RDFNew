<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="test.aspx.cs"
    Inherits="FineUI.Examples.test.test" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <x:PageManager ID="PageManager1" runat="server" />
    <%--<x:ContentPanel runat="server" ID="Panel1" EnableBackgroundColor="true" Width="600px"
        Height="150px" Title="页面">
        
        <asp:Button 
        ID="Button1" runat="server" Text="Button" OnClick="Button1_Click"></asp:Button>
    
    </x:ContentPanel>--%>
    <x:CheckBoxList ID="cblone" runat="server">
    </x:CheckBoxList>
    <x:Button Text="点击清空CheckBoxList" runat="server" ID="btnHello" OnClick="btnHello_Click">
    </x:Button>
    <x:Form ID="Form2" runat="server" BodyPadding="5px" Title="Form">
    <Rows>
        <x:FormRow ID="FormRow1" runat="server">
        </x:FormRow>
        <x:FormRow ID="FormRow2" runat="server">
        </x:FormRow>
    </Rows>
</x:Form>
    </form>
</body>
</html>
