<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadisticasUbicacion.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.EstadisticasUbicacion" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" trace="true"%>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


    <form id="form1" runat="server" >
        
        <asp:Label ID="lblUbicaciones" runat="server" Text="UBICACIÓN"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblUbicacionesTitulo"></asp:Label>
        <asp:Label ID="lblSerapacion" runat="server" Text=" : "  Display="Dynamic" Font-Size="Large" ></asp:Label>
        <asp:Label ID="lblNombreUbicacion" runat="server" Text="UBICACIÓN"  Display="Dynamic" Font-Size="Large" ></asp:Label>

        
        <br/>
       
        <br/>
        

        


        
        <asp:Label ID="lblFechaIni" runat="server" Text="Fecha Inicio"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblFechaIni"></asp:Label>

        <asp:Button ID="btnCalendario" runat="server" Text="Calendario" OnClick="btnCalendario_Click" />

        <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar1_SelectionChanged">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
                
        <asp:TextBox ID="txtFecha"  runat="server" meta:resourcekey="txtFechaResource1" Width="70px" ReadOnly="True"></asp:TextBox>
               &nbsp;&nbsp;
        <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Fin"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblFechaFin"></asp:Label>
        <asp:Button ID="btnCalendario2" runat="server" Text="Calendario2" OnClick="btnCalendario2_Click" />

        <asp:Calendar ID="Calendar2" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar2_SelectionChanged">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
        
        <asp:TextBox ID="txtFecha2" runat="server" meta:resourcekey="txtFecha2Resource1" Width="70px" ReadOnly="True"></asp:TextBox>

         &nbsp;&nbsp;
        <asp:Label ID="lblCriterio" runat="server" Text="Criterio"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblCriterio"></asp:Label>
       

        <asp:DropDownList ID="ddlListaCriterios" runat="server" AutoPostBack="True"
                            Width="110px" OnSelectedIndexChanged="ddlListaCriterios_SelectedIndexChanged" meta:resourcekey="ListaBateriasUbicacionResource1" ></asp:DropDownList>

         &nbsp;&nbsp;
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar2_Click" />

        <hr/>

        <asp:GridView ID="gvUbicacionesEstadisticas" runat="server" AutoGenerateColumns="False" onrowcommand="gvUbicacionesEstadisticas_RowCommand"  OnPageIndexChanging="gvUbicacionesPageIndexChanging" ShowHeaderWhenEmpty="True"  BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" SelectedRowStyle-HorizontalAlign="Center" SelectedRowStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Middle" PagerStyle-HorizontalAlign="Left" PagerStyle-VerticalAlign="Bottom" HorizontalAlign="Center" meta:resourcekey="gvUbicacionesEstadisticasResource1" Height="200px" Width="420px">
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
                <asp:BoundField headertext="id" DataField="consumoId" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" meta:resourcekey="BoundFieldResource1" >
                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>

                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                </asp:BoundField>
                <asp:hyperlinkfield headertext="Criterio" datatextfield="criterio" datanavigateurlformatstring="" meta:resourcekey="HyperLinkFieldResource1" />
                <asp:BoundField DataField="fecha" HeaderText="Fecha" meta:resourcekey="BoundFieldResource2" />
                <asp:BoundField DataField="horaIni" HeaderText="Hora inicio" meta:resourcekey="BoundFieldResource3" />
                <asp:BoundField DataField="horaFin" HeaderText="Hora fin" meta:resourcekey="BoundFieldResource4" />


            </Columns>
           
        </asp:GridView>

    </form>
        


</asp:Content>