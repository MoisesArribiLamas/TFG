<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogUser.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.LogUser" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 188px">

    <form id="form1" runat="server">
        <div class ="field">
            <span class="label">
                        <asp:Localize ID="Localize1LogUser" runat="server" Text="User" meta:resourcekey="Localize1LogUserResource1"></asp:Localize>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1LogUser" runat="server" ErrorMessage="Campo obligatorio" ControlToValidate="TBUserNameLogUser" Font-Italic="True" ForeColor="Red" Display="Dynamic" meta:resourcekey="RequiredFieldValidator1LogUserResource1"></asp:RequiredFieldValidator>
            </span>
                    <asp:TextBox ID="TBUserNameLogUser" runat="server" meta:resourcekey="TBUserNameLogUserResource1"></asp:TextBox>
            
        </div>
        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize2LogUser" runat="server" Text="Contraseña" meta:resourcekey="Localize2LogUserResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2LogUser" runat="server" ErrorMessage="Campo obligatorio" Font-Italic="True" ForeColor="Red" ControlToValidate="TBPassLogUser" Display="Dynamic" meta:resourcekey="RequiredFieldValidator2LogUserResource1"></asp:RequiredFieldValidator>
         

            </span>
            <asp:TextBox ID="TBPassLogUser" TextMode="Password" runat="server" meta:resourcekey="TBPassLogUserResource1"></asp:TextBox>

        </div>

        <div>



            <asp:Label ID="Label2LogUser" runat="server" Text="Recordar contraseña?(cookies deben estar activadas)" meta:resourcekey="Label2LogUserResource1"></asp:Label>
            <asp:CheckBox ID="CheckCookiesLogUser" runat="server" meta:resourcekey="CheckCookiesLogUserResource1" />



        </div>

        <div class="button"> 
            <asp:HyperLink ID="lnkCreateAccountLogUser" runat="server" 
                NavigateUrl="~/Pages/User/CreateAccount.aspx"
                Text="¿Aún no tienes cuenta? Regístrate aquí" meta:resourcekey="lnkCreateAccountLogUserResource1"/>
            <asp:Button ID="btLoggingLogUser" runat="server" Text="Iniciar Sesion" OnClick="btLogging_Click" meta:resourcekey="btLoggingLogUserResource1"  />

        </div>
        <div>
            <asp:Label ID="lblPassErrorLogUser" runat="server" Text="Contraseña erronea" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblPassErrorLogUserResource1" ></asp:Label>
        </div>
         <div>
            <asp:Label ID="lblUserErrorLogUser" runat="server" Text="Mail no encontrado" Font-Bold="True" ForeColor="Red" meta:resourcekey="lblUserErrorLogUserResource1" ></asp:Label>
        </div>

    </form>
        </div>


</asp:Content>