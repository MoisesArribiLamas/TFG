<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMainPage.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.UserMainPage"MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" Trace="false"%>


<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
	
	<form id="form1" runat="server">

        <div class="ubicacion">
			<span class="precio">
				<asp:Localize ID="Localize1Precio" runat="server" Text="Precio Actual" meta:resourcekey="Localize1PrecioResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:Label ID="lblPrecioActual" runat="server" Text="Precio Actual" Display="Dynamic" Font-Size="X-Large" ></asp:Label>
			</span>

			<span class="ubicacion">
                <asp:hyperlink id="hlUbicacion" Text="ubicacion" runat="server" meta:resourcekey="HyperLinkFieldResource1" />
				<br/>
                <br/>
                <asp:DropDownList ID="ListaUbicaciones" runat="server" AutoPostBack="True"
					Width="100px" OnSelectedIndexChanged="ListaUbicaciones_SelectedIndexChanged" meta:resourcekey="ListaUbicacionesResource1"></asp:DropDownList>
		
            </span>
				
            <span class="consumo">
				<asp:Localize ID="Localize1Consumo" runat="server" Text="Consumo" meta:resourcekey="Localize1ConsumoResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:Label ID="lblConsumo" runat="server" Text="Consumo" Display="Dynamic" ></asp:Label>
            </span>

            <span class="coste">
				<asp:Localize ID="Localize1Suministrando" runat="server" Text="Suministrando" meta:resourcekey="Localize1CosteResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:hyperlink id="hlsuministrador" Text="suministrador" runat="server" meta:resourcekey="HyperLinksuministradorResource1" />

            </span>

            <span class="bateria">
				<asp:Localize ID="Localize1EstadoBateria" runat="server" Text="Estado Bateria" meta:resourcekey="Localize1EstadoBateriaResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:Label ID="lblEstado" runat="server" Text="Estado" Display="Dynamic" ></asp:Label>

            </span>

            <span class="ratioCompra">
				<asp:Localize ID="Localize1ratioCompra" runat="server" Text="R. Compra" meta:resourcekey="Localize1ratioCompraResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:TextBox ID="BoxRatioCompra" runat="server" meta:resourcekey="BoxRatioCompraResource1"  Width="60px" style="text-align: right" ></asp:TextBox>
            </span>

            <span class="ratioCarga">
				<asp:Localize ID="Localize1ratioCarga" runat="server" Text="R. Carga" meta:resourcekey="Localize1ratioCargaResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:TextBox ID="BoxRatioCarga" runat="server" meta:resourcekey="BoxRatioCargaResource1" Width="50px" style="text-align: right"></asp:TextBox>
                
            </span>


            <span class="ratioUso">
				<asp:Localize ID="Localize1ratioUso" runat="server" Text="R. Uso" meta:resourcekey="Localize1ratioUsoResource1"></asp:Localize>
				<br/>
                <br/>
                <asp:TextBox ID="BoxRatioUso" runat="server" meta:resourcekey="BoxRatioUsoResource1" Width="60px" style="text-align: right"></asp:TextBox>
            </span>

            <asp:Label ID="lblhora" runat="server" Text="hora" Display="Dynamic" ></asp:Label>
            <br/>
            <br/>
            <asp:Button ID="btModificarRatios" runat="server" Text="Modificar Ratios" OnClick="btModificarRatios_Click" meta:resourcekey="btModificarRatiosBateriaResource1"  />

		</div>

        <div>
            <br/>
            <asp:Label ID="lblErrorModificarRatios" runat="server" ForeColor="Red" Style="position: relative"
                    Visible="False" text="Error Al modificar Ratios" meta:resourcekey="lblErrorModificarBateriaResource1"></asp:Label>
        </div>
        
        <div class="field">
		    <hr/>
		    <br/>
        </div>
        <br/>
		<asp:Label ID="lblFechaTarifas" runat="server" Text="Fecha"  Display="Dynamic" Font-Size="Large"></asp:Label>
        
        <div class="datosWeb">
		    <asp:Localize ID="Localize1Datos" runat="server" Text="Datos obtenidos de :" meta:resourcekey="Localize1DatosResource1"></asp:Localize>

            <asp:hyperlink id="Hyperlink1" Text="tarifaluzhora" runat="server" meta:resourcekey="HyperLinksuministradorResource1" NavigateUrl="https://tarifaluzhora.es/" />
            <br/>
        </div>

		<div class="cuadradoRojo" >
	
			<br/>
			<asp:Label ID="lblInicioHighestPrice" runat="server" Text="Precio más alto" meta:resourcekey="lblInicioHighestPriceResource1" Font-Size="Large" ></asp:Label>
			<br/>
			<asp:Label ID="lblInicioHighestPriceHora" runat="server" Text="Precio más alto hora" Font-Bold="True" Font-Size="Large"></asp:Label>
			<br/>
			<asp:Label ID="lblInicioHighestPricePrecio" runat="server" Text="Precio más alto" Font-Size="X-Large" Font-Bold="True" ForeColor="Red"></asp:Label>

		</div>

		<div class="cuadradoVerde" >
	
			<br/>
			<asp:Label ID="lblInicioLowestPrice" runat="server" Text="Precio más bajo" meta:resourcekey="lblInicioLowestPriceResource1" Font-Size="Large" ></asp:Label>
			<br/>
			<asp:Label ID="lblInicioLowestPriceHora" runat="server" Text="Precio más bajo Hora"  Font-Bold="True" Font-Size="Large"></asp:Label>
			<br/>
			<asp:Label ID="lblInicioLowestPricePrecio" runat="server" Text="Precio más bajo" Font-Size="X-Large" Font-Bold="True" ForeColor="#00CC00"></asp:Label>

		</div>

		<div class="cuadradoMarron" >
	
			<br/>
			<asp:Label ID="lblInicioAveragePrice" runat="server" Text="Precio medio" meta:resourcekey="lblInicioAveragePriceResource1" Font-Size="Large" ></asp:Label>
			<br/>
			<br/>
			<asp:Label ID="lblInicioAveragePricePrecio" runat="server" Text="Precio medio" Font-Bold="True" ForeColor="#F7DC06" Font-Size="X-Large"></asp:Label>

		</div>
 
		<asp:GridView ID="GridView1" runat="server" Height="429px" HorizontalAlign="Right" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" style="margin-left: 70px" Width="271px">
		   
		</asp:GridView>


        



	</form>

</asp:Content>