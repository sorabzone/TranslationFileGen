<%@ Page Title="Translate" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Translate.aspx.cs" Inherits="TranslationFileGen.WebForm1" %>

<asp:Content id="BodyContent" contentplaceholderid="MainContent" runat="server">
        <div>
            <table width="90%">
                <tr>
                    <td colspan="3">Translate</td>
                </tr>
                <tr>
                    <td colspan="3">Import</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:FileUpload ID="uploadFile" runat="server" /></td>
                    <td>
                        <asp:Button Text="Import" ID="btnTranslate" runat="server" OnClick="btnTranslate_Click" /></td>
                </tr>
            </table>
        </div>
</asp:Content>
