<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.User.ChangePassword" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain"
    runat="server">

     <div id="form" style="height: 206px">
        <form id="ChangePasswordForm" method="post" runat="server">
            <div class="field">
                <span class="label">
                    <asp:Localize ID="lclOldPasswordChangePassword" runat="server" Text="Anterior contraseña" meta:resourcekey="lclOldPasswordChangePasswordResource1" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtOldPasswordChangePassword" TextMode="Password" runat="server" Width="100px" Columns="16" meta:resourcekey="txtOldPasswordChangePasswordResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOldPasswordChangePassword" runat="server" ControlToValidate="txtOldPasswordChangePassword"
                            Display="Dynamic" Text="Campo necesario" meta:resourcekey="rfvOldPasswordChangePasswordResource1"/>
                        <asp:Label ID="lblOldPasswordErrorChangePassword" runat="server" ForeColor="Red" Visible="False"
                            Text="Contraseña anterior incorrecta" meta:resourcekey="lblOldPasswordErrorChangePasswordResource1"></asp:Label>
                    </span>
            </div>
            <div class="field">
                <span class="label">
                    <asp:Localize ID="lclNewPasswordChangePassword" runat="server" Text="Nueva contraseña" meta:resourcekey="lclNewPasswordChangePasswordResource1" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtNewPasswordChangePassword" runat="server" Width="100px" Columns="16" meta:resourcekey="txtNewPasswordChangePasswordResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNewPasswordChangePassword" runat="server" ControlToValidate="txtNewPasswordChangePassword"
                            Display="Dynamic" Text="Campo necesario" meta:resourcekey="rfvNewPasswordChangePasswordResource1"/>
                        <asp:CompareValidator ID="cvCreateNewPasswordChangePassword" runat="server" ControlToCompare="txtOldPasswordChangePassword"
                            ControlToValidate="txtNewPasswordChangePassword" Operator="NotEqual" Text="Las contraseñas no coinciden" meta:resourcekey="cvCreateNewPasswordChangePasswordResource1"></asp:CompareValidator>
                    </span>
            </div>
            <div class="field">
                <span class="label">
                    <asp:Localize ID="lclRetypePasswordChangePassword" runat="server" Text="Repita la contraseña" meta:resourcekey="lclRetypePasswordChangePasswordResource1"/></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtRetypePasswordChangePassword" runat="server" Width="100px"
                            Columns="16" meta:resourcekey="txtRetypePasswordChangePasswordResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRetypePasswordChangePassword" runat="server" ControlToValidate="txtRetypePasswordChangePassword"
                            Display="Dynamic" Text="Campo necesario" meta:resourcekey="rfvRetypePasswordChangePasswordResource1"/>
                        <asp:CompareValidator ID="cvPasswordCheckChangePassword" runat="server" ControlToCompare="txtNewPasswordChangePassword"
                            ControlToValidate="txtRetypePasswordChangePassword" Text="Las contraseñas no coinciden" meta:resourcekey="cvPasswordCheckChangePasswordResource1"></asp:CompareValidator>
                    </span>
            </div>
            <div class="button">
                <asp:Button ID="btnChangePasswordChangePassword" runat="server" OnClick="BtnChangePasswordClick"
                    Text="Confirmar" meta:resourcekey="btnChangePasswordChangePasswordResource1" />
            </div>
        </form>
    </div>

</asp:Content>

