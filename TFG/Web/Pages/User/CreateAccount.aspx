<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.CreateAccount" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 342px">

    <form id="form1" runat="server">
        <div class ="field">
            <span class="label">
                        <asp:Localize ID="Localize1CreateAccount" runat="server" Text="Mail" meta:resourcekey="Localize1CreateAccountResource1"></asp:Localize>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxUserMailCreateAccount" meta:resourcekey="RequiredFieldValidator1CreateAccountResource1"></asp:RequiredFieldValidator>
            </span>
                    <asp:TextBox ID="BoxUserMailCreateAccount" runat="server" meta:resourcekey="BoxUserMailCreateAccountResource1"></asp:TextBox>
            
        </div>
        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize2CreateAccount" runat="server" Text="Contraseña" meta:resourcekey="Localize2CreateAccountResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxPasswordCreateAccount" meta:resourcekey="RequiredFieldValidator2CreateAccountResource1"></asp:RequiredFieldValidator>
         

            </span>
            <asp:TextBox ID="BoxPasswordCreateAccount" TextMode="Password" runat="server" meta:resourcekey="BoxPasswordCreateAccountResource1"></asp:TextBox>

        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize3CreateAccount" runat="server" Text="Nombre" meta:resourcekey="Localize3CreateAccountResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxNombreCreateAccount" meta:resourcekey="RequiredFieldValidator3CreateAccountResource1"></asp:RequiredFieldValidator>
         

            </span>
            <asp:TextBox ID="BoxNombreCreateAccount" runat="server" meta:resourcekey="BoxNombreCreateAccountResource1"></asp:TextBox>

        </div>

           <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize4CreateAccount" runat="server" Text="Apellidos" meta:resourcekey="Localize4CreateAccountResource1"></asp:Localize>
         

               <asp:RequiredFieldValidator ID="RequiredFieldValidator4CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxApellidosCreateAccount" meta:resourcekey="RequiredFieldValidator4CreateAccountResource1"></asp:RequiredFieldValidator>
         

            </span>
            <asp:TextBox ID="BoxApellidosCreateAccount" runat="server" meta:resourcekey="BoxApellidosCreateAccountResource1"></asp:TextBox>

        </div>

           <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize5CreateAccount" runat="server" Text="Direccion Postal" meta:resourcekey="Localize5CreateAccountResource1"></asp:Localize>
         

               <asp:RequiredFieldValidator ID="RequiredFieldValidator5CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxCPCreateAccount" meta:resourcekey="RequiredFieldValidator5CreateAccountResource1"></asp:RequiredFieldValidator>
         

            </span>
            <asp:TextBox ID="BoxCPCreateAccount" runat="server" meta:resourcekey="BoxCPCreateAccountResource1"></asp:TextBox>
              
                  <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize6CreateAccount" runat="server" Text="Pais" meta:resourcekey="Localize6CreateAccountResource1"></asp:Localize>
         

            </span>
                      <asp:DropDownList ID="ListaPaisesCreateAccount" runat="server" AutoPostBack="True"
                            Width="100px" OnSelectedIndexChanged="ListaPaises_SelectedIndexChanged" meta:resourcekey="ListaPaisesCreateAccountResource1"></asp:DropDownList>

        </div>
 <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize7CreateAccount" runat="server" Text="Idioma" meta:resourcekey="Localize7CreateAccountResource1"></asp:Localize>
         

            </span>
                      <asp:DropDownList ID="ListaIdiomasCreateAccount" runat="server" AutoPostBack="True"
                            Width="100px" OnSelectedIndexChanged="ListaIdiomas_SelectedIndexChanged" meta:resourcekey="ListaIdiomasCreateAccountResource1"></asp:DropDownList>

        </div>
               <div>

                   <asp:Label ID="lblLoginErrorCreateAccount" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" text="Error Al registrar User" meta:resourcekey="lblLoginErrorCreateAccountResource1"></asp:Label>
               </div>
                <div class="button"> 

            <asp:Button ID="btRegistrarCreateAccount" runat="server" Text="Registrar" OnClick="btRegistrar_Click" meta:resourcekey="btRegistrarCreateAccountResource1"  />

        </div>
        </div>

       <div>
        </div>

    </form>
        </div>


</asp:Content>