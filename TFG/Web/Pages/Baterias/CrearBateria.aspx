<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrearBateria.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.CrearBateria" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 342px">

    <form id="form1" runat="server" >

        <asp:Label ID="lblCrearBateria" runat="server" Text="CREAR BATERIA"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblCrearBateriaTitulo"></asp:Label>


        <div class ="field">

            <span class="label">
                <asp:Localize ID="Localize1CrearBateria" runat="server" Text="Marca" meta:resourcekey="Localize1CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxMarcaCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            </span>

            <span class="entry">
                    <asp:TextBox ID="BoxMarcaCrearBateria" runat="server" ></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize2CrearBateria" runat="server" Text="Modelo" meta:resourcekey="Localize2CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxModeloCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>

            <span class="entry">  
                <asp:TextBox ID="BoxModeloCrearBateria" runat="server"></asp:TextBox>
            </span>

        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize3CrearBateria" runat="server" Text="N Serie" meta:resourcekey="Localize3CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxNSerieCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
            </span>

            <span class="entry">            
                <asp:TextBox ID="BoxNSerieCrearBateria" runat="server"></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize4CrearBateria" runat="server" Text="Almacenaje Máximo" meta:resourcekey="Localize4CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxAlmacenajeMaximoCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
            </span>

            <span class="entry">
                <asp:TextBox ID="BoxAlmacenajeMaximoCrearBateria" runat="server"></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize5CrearBateria" runat="server" Text="Capacidad Cargador" meta:resourcekey="Localize5CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxCapacidadCargadorCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>


            </span>

            <span class="entry">
                <asp:TextBox ID="BoxCapacidadCargadorCrearBateria" runat="server" ></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize6CrearBateria" runat="server" Text="Ratio Carga" meta:resourcekey="Localize5CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxRatioCargaCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>


            </span>

            <span class="entry">
                <asp:TextBox ID="BoxRatioCargaCrearBateria" runat="server" ></asp:TextBox>
            </span>
        </div>

        <div class = "field">
            <span class="label">

                <asp:Localize ID="Localize7CrearBateria" runat="server" Text="Ratio Compra" meta:resourcekey="Localize6CrearBateriaResource1"></asp:Localize>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxRatioCompraCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>


            </span>

            <span class="entry">
                <asp:TextBox ID="BoxRatioCompraCrearBateria" runat="server" ></asp:TextBox>
            </span>
        </div>

        <div class = "field">
                <span class="label">

                    <asp:Localize ID="Localize8CrearBateria" runat="server" Text="Ratio Uso" meta:resourcekey="Localize8CrearBateriaResource1"></asp:Localize>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8CrearBateria" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxRatioUsoCrearBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
                </span>
            <span class="entry">            
                <asp:TextBox ID="BoxRatioUsoCrearBateria" runat="server" meta:resourcekey="BoxCodigoPostalCrearBateriaResource1"></asp:TextBox>
            </span>
        </div>
 
 
        <div>

            <asp:Label ID="lblErrorCrearBateria" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" text="Error Al crear la Batería" meta:resourcekey="lblErrorCrearBateriaResource1"></asp:Label>
        </div>

        <div class="button"> 

            <asp:Button ID="btCrearBateria" runat="server" Text="Registrar" OnClick="btRegistrar_Click" meta:resourcekey="btRegistrarCreateBateriaResource1"  />

        </div>
        

       <div>
        </div>

    </form>
        </div>


</asp:Content>
