<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMemoWrite.aspx.cs" Inherits="DBhandlingWebApp.FrmMemoWrite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>메모입력</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>메모입력</h3>
            이름 : <asp:TextBox ID="TxtName" runat="server" /> <br />
            이메일 : <asp:TextBox ID="TxtEmail" runat="server" /> <br />
            메모 : <asp:TextBox ID="TxtTitle" runat="server" /> <br />

            <asp:Button ID="BtnWrite" runat="server" Text="저장" OnClick="BtnWrite_Click" />&nbsp;<!--스페이스역할-->
            <asp:Button ID="BinList" runat="server" Text="리스트" OnClick="BinList_Click" />
            <hr />
            <asp:Label ID="LblResult" runat="server" />
        </div>
    </form>
</body>
</html>
