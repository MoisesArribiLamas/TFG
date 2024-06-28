<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificarBateria.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.ModificarBateria" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" trace ="True "%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


    <form id="form1" runat="server" >
        
        <asp:Label ID="lblBateria" runat="server" Text="Bateria"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblBateriaTitulo"></asp:Label>

        <div class ="field">
            <span class="label">
                        <asp:Localize ID="Localize1ModificarBateria" runat="server" Text="Numero de serie" meta:resourcekey="Localize1ModificarBateriaResource1"></asp:Localize>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxNSerieModificarBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            </span>
            <span class="entry">
                    <asp:TextBox ID="BoxNSerieModificarBateria" runat="server" meta:resourcekey="BoxNSerieModificarBateriaResource1"></asp:TextBox>
            </span>
        </div>
        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize2ModificarBateria" runat="server" Text="Marca" meta:resourcekey="Localize2ModificarBateriaResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxMarcaModificarBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">  
                <asp:TextBox ID="BoxMarcaModificarBateria" runat="server" meta:resourcekey="BoxMarcaModificarBateriaResource1"></asp:TextBox>
            </span>

        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize3ModificarBateria" runat="server" Text="Modelo" meta:resourcekey="Localize3ModificarBateriaResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator3ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxModeloModificarBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         

            </span>
            <span class="entry">  
                <asp:TextBox ID="BoxModeloModificarBateria" runat="server" meta:resourcekey="BoxModeloModificarBateriaResource1"></asp:TextBox>
            </span>

        </div>

        <div class = "field">
            <span class="label">

                       <asp:Localize ID="Localize4ModificarBateria" runat="server" Text="Capacidad Cargador" meta:resourcekey="Localize4ModificarBateriaResource1"></asp:Localize>
         

            <asp:RequiredFieldValidator ID="RequiredFieldValidator4ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxCapacidadCargadorModificarBateria" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
         
            </span>
            <span class="entry">            
                <asp:TextBox ID="BoxCapacidadCargadorModificarBateria" runat="server" meta:resourcekey="BoxCapacidadCargadorModificarBateriaResource1"></asp:TextBox>
            </span>
        </div>

                  
 
        <div>

            <asp:Label ID="lblErrorModificarBateria" runat="server" ForeColor="Red" Style="position: relative"
                    Visible="False" text="Error Al modificar Bateria" meta:resourcekey="lblErrorModificarBateriaResource1"></asp:Label>
        </div>

        <div class="button"> 

            <asp:Button ID="btModificarBateria" runat="server" Text="Modificar" OnClick="btModificarRatios_Click" meta:resourcekey="btModificarBateriaResource1"  />

        </div>
                    <hr/>
       <div>
           <br />
        </div>

        <div>
            <asp:Label ID="lbRatioCompra" runat="server" Text="Ratio Compra"  Display="Dynamic"  meta:resourcekey="lbRatioCompra" Font-Bold="True"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio" Display="Dynamic"  Font-Italic="True" ForeColor="Red" ControlToValidate="BoxRatioCompra" Text="<%$ Resources:Comunes, campoObligatorio %>" Font-Bold="True"></asp:RequiredFieldValidator>
            <asp:TextBox ID="BoxRatioCompra" runat="server" meta:resourcekey="BoxRatioCompraResource1"  Width="60px" style="text-align: right" ></asp:TextBox>
            &nbsp;

            <asp:Label ID="lblRatioCarga" runat="server" Text="Ratio Carga"  Display="Dynamic"  meta:resourcekey="lblRatioCarga" Font-Bold="True"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio" Font-Italic="True" ForeColor="Red" Display="Dynamic" ControlToValidate="BoxRatioCarga" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            <asp:TextBox ID="BoxRatioCarga" runat="server" meta:resourcekey="BoxRatioCargaResource1" Width="30px" style="text-align: right"></asp:TextBox>
            &nbsp;

            <asp:Label ID="lblRatioUso" runat="server" Text="Ratio Uso"  Display="Dynamic"  meta:resourcekey="lblRatioUso" Font-Bold="True"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7ModificarBateria" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="BoxRatioUso" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            <asp:TextBox ID="BoxRatioUso" runat="server" meta:resourcekey="BoxRatioUsoResource1" Width="60px" style="text-align: right"></asp:TextBox>
            
        </div>
        <div>

            <asp:Label ID="lblErrorModificarRatios" runat="server" ForeColor="Red" Style="position: relative"
                    Visible="False" text="Error Al modificar Ratios" meta:resourcekey="lblErrorModificarBateriaResource1"></asp:Label>
        </div>
        <div class="button"> 

            <asp:Button ID="btModificarRatios" runat="server" Text="Modificar" OnClick="btModificarRatios_Click" meta:resourcekey="btModificarRatiosBateriaResource1"  />

        </div>

        
            <hr/>
       
           <br />

        <div class = "field">
            
            <asp:Label ID="lblAlmacenajeMaximo" runat="server" Text="Almacenaje Máximo (kwh) :" Display="Dynamic" meta:resourcekey="lblAlmacenajeMaximo"  Font-Size="Medium"></asp:Label>
            
             <asp:Label ID="lblAlmacenajeMaximoN" runat="server" Text="Almacenados"  Display="Dynamic"  Font-Bold="True" Font-Size="Medium"></asp:Label>
            &nbsp;
            &nbsp   
            <asp:Label ID="lblKwAlmacenados" runat="server" Text="Almacenados (kwh) :"  Display="Dynamic"  meta:resourcekey="lblKwAlmacenados"  Font-Size="Medium"></asp:Label>   
            <asp:Label ID="lblKwAlmacenadosNumero" runat="server" Text="Almacenados"  Display="Dynamic"  Font-Bold="True" Font-Size="Medium"></asp:Label>
            &nbsp
            &nbsp
            <asp:Label ID="lblPrecioMedio" runat="server" Text="Precio Medio :"  Display="Dynamic"  meta:resourcekey="lblPrecioMedio"  Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblPrecioMedioNumero" runat="server" Text="Precio"  Display="Dynamic"  Font-Bold="True" Font-Size="Medium"></asp:Label>
            
        </div>
        <br />
        <div>
            <asp:Label ID="lblBateriaSuministradora" runat="server" Text="Bateria"  Display="Dynamic"  meta:resourcekey="lblBateriaSuministradora"  Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblValorBateriaSuministradora" runat="server" Text="Estado"  Display="Dynamic" Font-Bold="True"   Font-Size="Medium"></asp:Label>
            &nbsp
            <asp:Label ID="lblEtiquetaEstado" runat="server" Text="Estado :"  Display="Dynamic"  meta:resourcekey="lblEtiquetaEstado" Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblEstado" runat="server" Text="Estado"  Display="Dynamic" Font-Bold="True" Font-Size="Medium"></asp:Label>

        </div>
         <br />

        <hr/>
         <br />
         <br />
        <div>
            <asp:Label ID="lblLimite" runat="server" Text="Porcentaje máximo para cargar"  Display="Dynamic"  meta:resourcekey="lblPorcentajeMaximo" Font-Bold="True"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPorcentajeMaximo" runat="server" ErrorMessage="Este campo es obligatorio"  Font-Italic="True" ForeColor="Red" Display="Dynamic"  ControlToValidate="tbPorcentajeMaximoDeCarga" Text="<%$ Resources:Comunes, campoObligatorio %>"></asp:RequiredFieldValidator>
            <asp:TextBox ID="tbPorcentajeMaximoDeCarga" runat="server" meta:resourcekey="BoxRatioUsoResource1" Width="60px" style="text-align: right"></asp:TextBox>
        
            <asp:Button ID="btnPorcentajeMaximoDeCarga" runat="server" Text="Modificar" OnClick="btModificarRatios_Click" meta:resourcekey="btnPorcentajeMaximoDeCargaResource1"  />


            <br />

            <asp:Label ID="lblErrorPorcentajeMaximoDeCarga" runat="server" ForeColor="Red" Style="position: relative"
                    Visible="False" text="Error Al modificar" meta:resourcekey="lblErrorPorcentajeMaximoDeCargaResource1"></asp:Label>
        
        </div>

         <br />
        <hr/>
        <div class="button"> 

            <asp:Button ID="btnEliminarBateria" runat="server" Text="Eliminar" OnClick="btnEliminarBaterias_Click" meta:resourcekey="btnEliminarBateriaResource1"  />

        </div>
    </form>
        


</asp:Content>
