<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BateriasPage.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.Baterias.BateriasPage"MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <form id="form1" runat="server">
    
        <asp:Label ID="lblBaterias" runat="server" Text="Baterias"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblBateriasTitulo"></asp:Label>

        
        <br/>
        <div class="button"> 
            <asp:Button ID="BtnCrearBateria" runat="server" OnClick="ButtonCrearBateria_Click" Text="Crear Batería" Width="130px" Height="35px" meta:resourcekey="lblBateriasBoton"/>
        </div>

        <br/> 
       

        <asp:GridView ID="gvBaterias" runat="server" AutoGenerateColumns="False" onrowcommand="gvBaterias_RowCommand"  OnPageIndexChanging="gvBateriasPageIndexChanging" ShowHeaderWhenEmpty="True"  BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" SelectedRowStyle-HorizontalAlign="Center" SelectedRowStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Left" PagerStyle-VerticalAlign="Bottom" HorizontalAlign="Center" meta:resourcekey="gvBateriasResource1" Height="200px" Width="600px">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />

             <Columns>
                <asp:BoundField headertext="id" DataField="bateriaId" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" meta:resourcekey="BoundFieldResource1" >
                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                </asp:BoundField>
                <asp:hyperlinkfield headertext="nSerie" datatextfield="nSerie" datanavigateurlformatstring="" meta:resourcekey="HyperLinkFieldResource1" />
                <asp:BoundField DataField="etiquetaUbicacion" HeaderText="Ubicacion" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="ratioCarga" HeaderText="ratio Carga" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="ratioCompra" HeaderText="ratio Compra" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="ratioUso" HeaderText="ratio Uso" meta:resourcekey="BoundFieldResource5" />
                <asp:BoundField DataField="precioMedio" HeaderText="precio Medio" meta:resourcekey="BoundFieldResource6" />
                <asp:BoundField DataField="porcentajeCarga" HeaderText="porcentaje Carga" meta:resourcekey="BoundFieldResource7" />




            </Columns>
           
        </asp:GridView>
      
         

    </form>

</asp:Content>