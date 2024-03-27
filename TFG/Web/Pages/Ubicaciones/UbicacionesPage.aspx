<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UbicacionesPage.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.Ubicaciones.UbicacionesPage"MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <form id="form1" runat="server">
    
        <asp:Label ID="lblUbicaciones" runat="server" Text="Ubicaciones"  Display="Dynamic" Font-Size="Large"></asp:Label>

        
        <br/>
        
        <div class="button"> 
            <asp:Button ID="BtnCrearUbicacion" runat="server" OnClick="Button1_Click" Text="Crear Ubicación" Width="130px" Height="35px" />
        </div>
          
        <br/>

        <asp:GridView ID="gvUbicaciones" runat="server" AutoGenerateColumns="False" onrowcommand="gvUbicaciones_RowCommand"  OnPageIndexChanging="gvUbicacionesPageIndexChanging" ShowHeaderWhenEmpty="True"  BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" SelectedRowStyle-HorizontalAlign="Center" SelectedRowStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Left" PagerStyle-VerticalAlign="Bottom" HorizontalAlign="Center" meta:resourcekey="gvUbicacionesResource1" Height="200px" Width="420px">
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
                <asp:BoundField headertext="id" DataField="ubicacionId" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" meta:resourcekey="BoundFieldResource1" >
                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                </asp:BoundField>
                <asp:hyperlinkfield headertext="Etiqueta" datatextfield="etiqueta" datanavigateurlformatstring="" meta:resourcekey="HyperLinkFieldResource1" />
                <asp:BoundField DataField="localidad" HeaderText="Localidad" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="calle" HeaderText="Calle" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="numero" HeaderText="Numero" meta:resourcekey="BoundFieldResource4" />
                <asp:BoundField DataField="portal" HeaderText="Portal" meta:resourcekey="BoundFieldResource5" />


            </Columns>
           
        </asp:GridView>
      
         

    </form>

</asp:Content>