<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageTables.aspx.cs" Inherits="TranslationFileGen.ManageTables" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="80%" align="center">
                <tr>
                    <td>
                        <asp:Button Text="Tab 1" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                            OnClick="Tab1_Click" />
                        <asp:Button Text="Tab 2" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
                            OnClick="Tab2_Click" />
                        <asp:Button Text="Tab 3" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
                            OnClick="Tab3_Click" />
                        <asp:Button Text="Tab 4" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
                            OnClick="Tab4_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="mvTables" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwImage" runat="server">
                                <table width="90%">
                                    <tr>
                                        <td colspan="3">Update Image ID</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSku" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearch" runat="server" OnClick="btnSearch_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Image ID</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtImage" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Update" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Add New SKU - Image ID pair</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNewSku" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtNewImage" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Add" ID="btnAdd" runat="server" OnClick="btnAdd_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Import</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFile" runat="server" /></td>
                                        <td>
                                            <asp:Button Text="Import" ID="btnImageImport" runat="server" OnClick="btnImageImport_Click" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwChinese" runat="server">
                                <table width="90%">
                                    <tr>
                                        <td colspan="3">Chinese Name</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSkuC" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearchC" runat="server" OnClick="btnSearchC_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Name</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseName" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Description</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseDesc" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Update" ID="btnUpdateC" runat="server" OnClick="btnUpdateC_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Add New SKU and Chinese Item pair</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">SKU</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNewSkuC" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Name</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseName" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Description</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseDesc" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Add" ID="btnAddC" runat="server" OnClick="btnAddC_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Import</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileChinese" runat="server" /></td>
                                        <td>
                                            <asp:Button Text="Import" ID="btnChineseImport" runat="server" OnClick="btnChineseImport_Click" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwMeta" runat="server">
                                <table width="90%">
                                    <tr>
                                        <td colspan="3">Meta Data</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtMetaData" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearchM" runat="server" OnClick="btnSearchM_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Translation</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseTrans" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Update" ID="btnUpdateM" runat="server" OnClick="btnUpdateM_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Add New Meta Data Translation pair</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">New Meta Data</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNewMetaData" runat="server"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server">Chinese Description</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseTrans" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Add" ID="btnAddM" runat="server" OnClick="btnAddM_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Import</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileMetaData" runat="server" /></td>
                                        <td>
                                            <asp:Button Text="Import" ID="btnMetaDataImport" runat="server" OnClick="btnMetaDataImport_Click" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwCategory" runat="server">
                                <table width="90%">
                                    <tr>
                                        <td colspan="3">Categories</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Import</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileCategory" runat="server" /></td>
                                        <td>
                                            <asp:Button Text="Import" ID="btnCategoryImport" runat="server" OnClick="btnCategoryImport_Click" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
