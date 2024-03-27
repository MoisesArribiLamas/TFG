<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMainPage.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.UserMainPage"MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto"  %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <form id="form1" runat="server">
    
        <asp:Label ID="lblFechaTarifas" runat="server" Text="Fecha"  Display="Dynamic" Font-Size="Large"></asp:Label>

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
            <asp:Label ID="lblInicioAveragePricePrecio" runat="server" Text="Precio medio" ForeColor="#990000" Font-Size="X-Large"></asp:Label>

        </div>
        

        <asp:GridView ID="GridView1" runat="server" Height="429px" HorizontalAlign="Right" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" style="margin-left: 70px" Width="271px">
           
        </asp:GridView>
        

    </form>

</asp:Content>