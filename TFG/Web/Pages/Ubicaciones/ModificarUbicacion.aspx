<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificarUbicacion.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.ModificarUbicacion" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" trace="true"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 342px">

    <form id="form1" runat="server" >
        <div class ="field">
            <span class="label">
                        <asp:Localize ID="Localize1CrearUbicacion" runat="server" Text="Etiqueta" meta:resourcekey="Localize1CrearUbicacionResource1"></asp:Localize>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1CrearUbicacion" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxEtiquetaCrearUbicacion" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            </span>
            <span class="entry">
                    <asp:TextBox ID="BoxEtiquetaCrearUbicacion" runat="server" meta:resourcekey="BoxEtiquetaCrearUbicacionResource1"></asp:TextBox>
            </span>
        </div>
        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize2CrearUbicacion" runat="server" Text="Localidad" meta:resourcekey="Localize2CrearUbicacionResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2CrearUbicacion" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxLocalidadCrearUbicacion" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">  
                <asp:TextBox ID="BoxLocalidadCrearUbicacion" runat="server" meta:resourcekey="BoxLocalidadCrearUbicacionResource1"></asp:TextBox>
            </span>

        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize3CrearUbicacion" runat="server" Text="Calle" meta:resourcekey="Localize3CrearUbicacionResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3CrearUbicacion" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxCalleCrearUbicacion" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
            </span>
            <span class="entry">            
                <asp:TextBox ID="BoxCalleCrearUbicacion" runat="server" meta:resourcekey="BoxCalleCrearUbicacionResource1"></asp:TextBox>
            </span>
        </div>

           <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize4CrearUbicacion" runat="server" Text="Numero" meta:resourcekey="Localize4CrearUbicacionResource1"></asp:Localize>
         

               <asp:RequiredFieldValidator ID="RequiredFieldValidator4CrearUbicacion" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxNumeroCrearUbicacion" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">
                <asp:TextBox ID="BoxNumeroCrearUbicacion" runat="server" meta:resourcekey="BoxNumeroCrearUbicacionResource1"></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize5CrearUbicacion" runat="server" Text="Portal" meta:resourcekey="Localize5CrearUbicacionResource1"></asp:Localize>
                 

            </span>
            <span class="entry">
                <asp:TextBox ID="BoxPortalCrearUbicacion" runat="server" meta:resourcekey="BoxPortalCrearUbicacionResource1"></asp:TextBox>
            </span>

            <div class = "field">
                <span class="label">

                    <asp:Localize ID="Localize8CrearUbicacion" runat="server" Text="Codigo Postal" meta:resourcekey="Localize8CrearUbicacionResource1"></asp:Localize>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8CrearUbicacion" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxCodigoPostalCrearUbicacion" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
                </span>
            <span class="entry">            
                <asp:TextBox ID="BoxCodigoPostalCrearUbicacion" runat="server" meta:resourcekey="BoxCodigoPostalCrearUbicacionResource1"></asp:TextBox>
            </span>
        </div>


                  
 
               <div>

                   <asp:Label ID="lblErrorModificarUbicacion" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" text="Error Al crear Ubicación" meta:resourcekey="lblErrorModificarUbicacionResource1"></asp:Label>
               </div>
                <div class="button"> 

            <asp:Button ID="btModificarUbicacion" runat="server" Text="Modificar" OnClick="btModificar_Click" meta:resourcekey="btModificarUbicacionResource1"  />

        </div>
        </div>

       <div>
        </div>

    </form>
        </div>


</asp:Content>
