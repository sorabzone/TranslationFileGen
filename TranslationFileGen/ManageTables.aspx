<%@ Page Title="Manage Tables" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ManageTables.aspx.cs" Inherits="TranslationFileGen.ManageTables" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <table width="80%" align="center">
            <tr>
                <td>
                    <ul class="nav nav-tabs">
                        <li>
                            <asp:LinkButton Text="Image ID" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                                OnClick="Tab1_Click" /></li>
                        <li>
                            <asp:LinkButton Text="Chinese Name" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
                                OnClick="Tab2_Click" /></li>
                        <li>
                            <asp:LinkButton Text="Meta Data" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
                                OnClick="Tab3_Click" /></li>
                        <li>
                            <asp:LinkButton Text="Category" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
                                OnClick="Tab4_Click" /></li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="box-shadow">
                        <asp:MultiView ID="mvTables" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwImage" runat="server">
                                <table width="90%" style="margin-left: 25px;margin-right: 25px;margin-bottom: 25px;" cellpadding="5px;">
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="h2">
                                                <label>Image ID</label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>SKU</label></td>
                                        <td>
                                            <asp:TextBox ID="txtSku" runat="server" class="form-control" placeholder="SKU"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearch" runat="server" OnClick="btnSearch_Click" class="btn btn-outline-primary my-2" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Image ID</label></td>
                                        <td>
                                            <asp:TextBox ID="txtImage" runat="server" class="form-control" placeholder="Image Id"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Update" ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Add New SKU - Image ID pair</label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNewSku" runat="server" class="form-control" placeholder="SKU"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtNewImage" runat="server" class="form-control" placeholder="Image Id"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Add" ID="btnAdd" runat="server" OnClick="btnAdd_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Import</label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFile" runat="server" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Import" ID="btnImageImport" runat="server" OnClick="btnImageImport_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwChinese" runat="server">
                                <table width="90%" style="margin-left: 25px;margin-right: 25px;margin-bottom: 25px;" cellpadding="5px;">
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="h2">
                                                <label>Chinese Name</label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>SKU</label></td>
                                        <td>
                                            <asp:TextBox ID="txtSkuC" runat="server" class="form-control" placeholder="SKU"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearchC" runat="server" OnClick="btnSearchC_Click" class="btn btn-outline-primary my-2" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Chinese Name</label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseName" runat="server" class="form-control" placeholder="Chinese Name"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Chinese Description</label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseDesc" runat="server" class="form-control" placeholder="Chinese Description"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Update" ID="btnUpdateC" runat="server" OnClick="btnUpdateC_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Add New SKU and Chinese Item pair</label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNewSkuC" runat="server" class="form-control" placeholder="SKU"></asp:TextBox></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseName" runat="server" class="form-control" placeholder="Chinese Name"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseDesc" runat="server" class="form-control" placeholder="Chinese Description"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Add" ID="btnAddC" runat="server" OnClick="btnAddC_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Import</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileChinese" runat="server" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Import" ID="btnChineseImport" runat="server" OnClick="btnChineseImport_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwMeta" runat="server">
                                <table width="90%" style="margin-left: 25px;margin-right: 25px;margin-bottom: 25px;" cellpadding="5px;">
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="h2">
                                                <label>Meta Data</label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>New Meta Data</label></td>
                                        <td>
                                            <asp:TextBox ID="txtMetaData" runat="server" class="form-control" placeholder="Meta Data"></asp:TextBox></td>
                                        <td>
                                            <asp:Button Text="Search" ID="btnSearchM" runat="server" OnClick="btnSearchM_Click" class="btn btn-outline-primary my-2" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Chinese Translation</label></td>
                                        <td>
                                            <asp:TextBox ID="txtChineseTrans" runat="server" class="form-control" placeholder="Chinese Translation"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Update" ID="btnUpdateM" runat="server" OnClick="btnUpdateM_Click" class="btn btn-primary btn-block" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Add New Meta Data Translation pair</label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNewMetaData" runat="server" class="form-control" placeholder="Meta Data"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtNewChineseTrans" runat="server" class="form-control" placeholder="Chinese Translation"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Add" ID="btnAddM" runat="server" OnClick="btnAddM_Click" class="btn btn-primary btn-block" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Import</label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileMetaData" runat="server" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Import" ID="btnMetaDataImport" runat="server" OnClick="btnMetaDataImport_Click" class="btn btn-primary btn-block" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vwCategory" runat="server">
                                <table width="90%" style="margin-left: 25px;margin-right: 25px;margin-bottom: 25px;" cellpadding="5px;">
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="h2">
                                                <label>Categories</label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr class="mb-4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label>Import</label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:FileUpload ID="uploadFileCategory" runat="server" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button Text="Import" ID="btnCategoryImport" runat="server" OnClick="btnCategoryImport_Click" class="btn btn-primary btn-block" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
