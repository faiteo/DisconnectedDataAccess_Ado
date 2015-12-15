<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductInfo.aspx.cs" Inherits="DisconnectedDataAccess_Ado.ProductInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family:Arial">
        <asp:Button ID="btnGetProductsFromDB" runat="server" Text="Get Product List from Database" OnClick="btnGetProductsFromDB_Click" />
        <br />
        <br />

        <asp:GridView ID="gdViewProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID" OnRowCancelingEdit="gdViewProducts_RowCancelingEdit" OnRowDeleting="gdViewProducts_RowDeleting" OnRowEditing="gdViewProducts_RowEditing" OnRowUpdating="gdViewProducts_RowUpdating">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="ProductID" HeaderText="ProductID" InsertVisible="False" ReadOnly="True" SortExpression="ProductID" />
                <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
                <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
            </Columns>
        </asp:GridView>
       
        <br />

        <asp:Button ID="btnUpdateProductsInDB" runat="server" Text="Update Product data in Database" OnClick="btnUpdateProductsInDB_Click" Visible="False" />
        <asp:Label ID="lblMessages" runat="server"  ></asp:Label>
  
    
    </div>
    </form>
</body>
</html>
