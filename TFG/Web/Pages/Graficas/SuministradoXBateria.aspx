<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuministradoXBateria.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.Graficas.SuministradoXBateria" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" Trace="false" %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
      google.charts.load('current', {'packages':['corechart']});
      google.charts.setOnLoadCallback(drawChart);

      function drawChart() {

        var data = google.visualization.arrayToDataTable(<%=obtenerDatosSuministradoXBateria()%>);

        var options = {
          title: 'Suministrou'
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('piechart'));

        chart.draw(data, options);
      }
    </script>

    <form id="form1" runat="server">

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

        <div>
            <div id="piechart" style="width: 900px; height: 380px;"></div>
        </div>
    </form>


</asp:Content>