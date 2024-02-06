<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.CreateAccount" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 342px">

    <form id="form1" runat="server" >
        <div class ="field">
            <span class="label">
                        <asp:Localize ID="Localize1CreateAccount" runat="server" Text="Mail" meta:resourcekey="Localize1CreateAccountResource1"></asp:Localize>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxUserMailCreateAccount" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            </span>
            <span class="entry">
                    <asp:TextBox ID="BoxUserMailCreateAccount" runat="server" meta:resourcekey="BoxUserMailCreateAccountResource1"></asp:TextBox>
            </span>
        </div>
        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize2CreateAccount" runat="server" Text="Contraseña" meta:resourcekey="Localize2CreateAccountResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxPasswordCreateAccount" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">  
                <asp:TextBox ID="BoxPasswordCreateAccount" TextMode="Password" runat="server" meta:resourcekey="BoxPasswordCreateAccountResource1"></asp:TextBox>
            </span>

        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize3CreateAccount" runat="server" Text="Nombre" meta:resourcekey="Localize3CreateAccountResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxNombreCreateAccount" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
            </span>
            <span class="entry">            
                <asp:TextBox ID="BoxNombreCreateAccount" runat="server" meta:resourcekey="BoxNombreCreateAccountResource1"></asp:TextBox>
            </span>
        </div>

           <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize4CreateAccount" runat="server" Text="Apellido1" meta:resourcekey="Localize4CreateAccountResource1"></asp:Localize>
         

               <asp:RequiredFieldValidator ID="RequiredFieldValidator4CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxApellido1CreateAccount" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">
                <asp:TextBox ID="BoxApellido1CreateAccount" runat="server" meta:resourcekey="BoxApellido1CreateAccountResource1"></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize5CreateAccount" runat="server" Text="Apellido2" meta:resourcekey="Localize5CreateAccountResource1"></asp:Localize>
                 

            </span>
            <span class="entry">
                <asp:TextBox ID="BoxApellido2CreateAccount" runat="server" meta:resourcekey="BoxApellido2CreateAccountResource1"></asp:TextBox>
            </span>

            <div class = "field">
                <span class="label">

                    <asp:Localize ID="Localize8CreateAccount" runat="server" Text="Telefono" meta:resourcekey="Localize8CreateAccountResource1"></asp:Localize>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8CreateAccount" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxTelefonoCreateAccount" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
                </span>
            <span class="entry">            
                <asp:TextBox ID="BoxTelefonoCreateAccount" runat="server" meta:resourcekey="BoxTelefonoCreateAccountResource1"></asp:TextBox>
            </span>
        </div>


                  <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize6CreateAccount" runat="server" Text="Pais" meta:resourcekey="Localize6CreateAccountResource1"></asp:Localize>
         

            </span>
            <span class="entry">
                      <asp:DropDownList ID="ListaPaisesCreateAccount" runat="server" AutoPostBack="True"
                            Width="100px" OnSelectedIndexChanged="ListaPaises_SelectedIndexChanged" meta:resourcekey="ListaPaisesCreateAccountResource1"></asp:DropDownList>
            </span>
        </div>
 <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize7CreateAccount" runat="server" Text="Idioma" meta:resourcekey="Localize7CreateAccountResource1"></asp:Localize>
         

            </span>
            <span class="entry">
                      <asp:DropDownList ID="ListaIdiomasCreateAccount" runat="server" AutoPostBack="True"
                            Width="100px" OnSelectedIndexChanged="ListaIdiomas_SelectedIndexChanged" meta:resourcekey="ListaIdiomasCreateAccountResource1"></asp:DropDownList>
            </span>
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
