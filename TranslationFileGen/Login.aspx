<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TranslationFileGen.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="stylesheet" href="Content/signin.css" />
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
</head>
<body class="text-center bg-light">
    <form id="form1" runat="server" class="form-signin">
        <h2 class="form-signin-heading">Please sign in</h2>
        <table>
            <tr>
                <td>
                    <label>UserName:</label></td>
                <td>
                    <asp:TextBox ID="UserName" runat="server" class="form-control" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                        ControlToValidate="UserName"
                        Display="Dynamic"
                        ErrorMessage="Cannot be empty."
                        runat="server" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td>
                    <label>Password:</label></td>
                <td>
                    <asp:TextBox ID="UserPass" TextMode="Password"
                        runat="server" class="form-control" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        ControlToValidate="UserPass"
                        ErrorMessage="Cannot be empty."
                        runat="server" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:CheckBox ID="chkboxPersist" runat="server" /> &nbsp; Remember me
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnLogin" OnClick="Login_Click" Text="Log In"
            runat="server" class="btn btn-lg btn-primary btn-block" />
        <p>
            <asp:Label ID="Msg" ForeColor="red" runat="server" />
        </p>
    </form>
</body>
</html>
