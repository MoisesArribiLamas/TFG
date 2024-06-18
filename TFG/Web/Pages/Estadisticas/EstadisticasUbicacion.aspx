<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadisticasUbicacion.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.EstadisticasUbicacion" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" trace="false"%>
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
                
        <asp:Label ID="lblFecha1Error" runat="server" Text="Error Fecha" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblFecha1Error"> </asp:Label>
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
        

        <asp:Label ID="lblFecha2Error" runat="server" Text="Error Fecha" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblFecha2Error"> </asp:Label>
        <asp:TextBox ID="txtFecha2" runat="server" meta:resourcekey="txtFecha2Resource1" Width="70px" ReadOnly="True"></asp:TextBox>

         &nbsp;&nbsp;
        <asp:Label ID="lblCriterio" runat="server" Text="Criterio"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblCriterio"></asp:Label>
       

        <asp:DropDownList ID="ddlListaCriterios" runat="server" AutoPostBack="True"
                            Width="110px" OnSelectedIndexChanged="ddlListaCriterios_SelectedIndexChanged" meta:resourcekey="ddlListaCriteriosResource1" ></asp:DropDownList>

         &nbsp;&nbsp;
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

        <hr/>

        <br/>
       
        <br/>

        <asp:Label ID="lblFechaDia" runat="server" Text="Dia"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblFechaDia"></asp:Label>
        <asp:Button ID="btnCalendario3" runat="server" Text="Calendario3" OnClick="btnCalendario3_Click" />

        <asp:Calendar ID="Calendar3" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar3_SelectionChanged">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>

        <asp:Label ID="lblFecha3Error" runat="server" Text="Error Fecha" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblFecha3Error"> </asp:Label>

        <asp:TextBox ID="txtFecha3" runat="server" meta:resourcekey="txtFecha3Resource1" Width="70px" ReadOnly="True"></asp:TextBox>

         &nbsp;&nbsp;

         <asp:Label ID="lblCriterio2" runat="server" Text="Criterio"  Display="Dynamic" Font-Size="Large" meta:resourcekey="lblCriterio2"></asp:Label>

        <asp:DropDownList ID="ddlListaCriterios2" runat="server" AutoPostBack="True"
                            Width="110px" OnSelectedIndexChanged="ddlListaCriterios2_SelectedIndexChanged" meta:resourcekey="ddlListaCriterios2Resource1" ></asp:DropDownList>

         &nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" Text="Buscar" OnClick="btnBuscar2_Click" />

        <hr/>
        

    </form>
        


</asp:Content>