<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogUser.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.LogUser" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 198px">

    <form id="form1" runat="server">
        <div class ="field">
            <span class="label">
                <asp:Localize ID="Localize1LogUser" runat="server" Text="Usuario" meta:resourcekey="Localize1LogUserResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1LogUser" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="TBUserNameLogUser" Font-Italic="True" ForeColor="Red" Display="Dynamic" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>

            </span>
            <span class="entry">
                <asp:TextBox ID="TBUserNameLogUser" runat="server" meta:resourcekey="TBUserNameLogUserResource1"></asp:TextBox>
                <asp:Label ID="lblLoginError" runat="server" Text="Error Usuario" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblLoginError"> </asp:Label>
            </span>
            
            
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize2LogUser" runat="server" Text="Contraseña" meta:resourcekey="Localize2LogUserResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2LogUser" runat="server" ErrorMessage="Campo obligatorio" Font-Italic="True" ForeColor="Red" ControlToValidate="TBPassLogUser" Display="Dynamic" Text="<%$ Resources:Comunes, campoObligatorio %>"> </asp:RequiredFieldValidator>

            </span>
            <span class="entry">
                <asp:TextBox ID="TBPassLogUser" TextMode="Password" runat="server" meta:resourcekey="TBPassLogUserResource1"></asp:TextBox>
                <asp:Label ID="lblPasswordError" runat="server" Text="Contraseña Incorrecta" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblPasswordError">       
                </asp:Label>
            </span>

        </div>

        <div style="height: 10px; width: 1022px">
        </div>

        <div >



            <asp:Label ID="Label2LogUser" runat="server" Text="Recordar contraseña?(cookies deben estar activadas)" meta:resourcekey="Label2LogUserResource1"></asp:Label>
            <asp:CheckBox ID="CheckCookiesLogUser" runat="server" meta:resourcekey="CheckCookiesLogUserResource1" />



        </div>

        <div style="height: 10px; width: 1022px">
        </div>

        <div>
            <asp:HyperLink ID="HyperLink1" runat="server" 
                NavigateUrl="~/Pages/User/CreateAccount.aspx"
                Text="¿Aún no tienes cuenta? Regístrate aquí" meta:resourcekey="lnkCreateAccountLogUserResource1"/>
        </div>
        <div class="button"> 
            <asp:Button ID="btLoggingLogUser" runat="server" Text="Iniciar Sesion" OnClick="btLogging_Click" meta:resourcekey="btLoggingLogUserResource1"  />

        </div>
        <div>
            <asp:Label ID="lblPassErrorLogUser" runat="server" Text="Contraseña erronea" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblPassErrorLogUserResource1" ></asp:Label>
        </div>
         <div>
            <asp:Label ID="lblUserErrorLogUser" runat="server" Text="Usuario no encontrado" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblUserErrorLogUserResource1" ></asp:Label>
        </div>

    </form>
        </div>


</asp:Content>