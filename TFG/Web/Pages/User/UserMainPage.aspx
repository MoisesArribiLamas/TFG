<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMainPage.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.UserMainPage"MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" trace="true" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <form id="form1" runat="server">
    

        <div class="cuadradoRojo" >
    

        <asp:Label ID="lblInicioHighestPrice" runat="server" Text="Precio más alto" meta:resourcekey="lblInicioHighestPriceResource1" Display="Dynamic"></asp:Label>
        
        </div>

        <div class="cuadradoVerde" >
    

        <asp:Label ID="lblInicioLowestPrice" runat="server" Text="Precio más bajo" meta:resourcekey="lblInicioLowestPriceResource1" Display="Dynamic"></asp:Label>
        
        </div>

        <div class="cuadradoMarron" >
    

        <asp:Label ID="lblInicioAveragePrice" runat="server" Text="Precio medio" meta:resourcekey="lblInicioAveragePriceResource1" Display="Dynamic"></asp:Label>
        
        </div>
        

    </form>

</asp:Content>