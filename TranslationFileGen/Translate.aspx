﻿<%@ Page Title="Translate" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Translate.aspx.cs" Inherits="TranslationFileGen.WebForm1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box-shadow bg-white">
        <table width="90%" style="margin-left: 25px; margin-right: 25px; margin-bottom: 25px;" cellpadding="2px;">
            <br />
            <p style="margin-left: 25px;">
                <asp:Label ID="Msg" ForeColor="red" runat="server" />
            </p>
            <tr>
                <td colspan="3">
                    <span class="h2">
                        <label>Translate</label></span>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <hr class="mb-4">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:FileUpload ID="uploadFile" runat="server" /></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button Text="Translate" ID="btnTranslate" runat="server" OnClick="btnTranslate_Click" class="btn btn-primary btn-block" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
