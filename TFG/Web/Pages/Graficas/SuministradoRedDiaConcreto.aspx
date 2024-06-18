<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuministradoRedDiaConcreto.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.Graficas.SuministradoRedDiaConcreto" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" trace="false"%>



<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
      google.charts.load('current', {'packages':['corechart']});
      google.charts.setOnLoadCallback(drawChart);

      function drawChart() {

        var data = google.visualization.arrayToDataTable(<%=obtenerDatosXRed()%>);

        var options = {
          title:  '<%=titulo()%>'
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('piechart'));

        chart.draw(data, options);
      }
    </script>

    <form id="form1" runat="server" >
        

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

        <div>
            <div id="piechart" style="width: 900px; height: 380px;"></div>
        </div>
        

    </form>
        


</asp:Content>