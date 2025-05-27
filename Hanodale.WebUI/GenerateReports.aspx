<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateReports.aspx.cs" Inherits="Hanodale.WebUI.GenerateReports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div><asp:Label runat="server" ID="lbltile" Font-Bold="False" Font-Size="Medium" Font-Names="Arial Black">Report Title : </asp:Label></div>
        <div></div>

        <div>

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <rsweb:ReportViewer ID="RptViewer" runat="server" Height="800px" Width="100%">
            </rsweb:ReportViewer>

            </div>
    </form>
</body>
</html>
