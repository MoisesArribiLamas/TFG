<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.Graficas.WebForm2" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" Trace="false"%>


<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
      google.charts.load('current', {'packages':['bar']});
      google.charts.setOnLoadCallback(drawChart);

      function drawChart() {
        var data = google.visualization.arrayToDataTable(<%=suministradoCargadoyRed()%>);

        var options = {
          chart: {
            title: 'Company Performance',
            subtitle: 'Sales, Expenses, and Profit: 2014-2017',
            }, series: {
                0: { axis: 'distance' }
            }, axes: {
                y: {
                    distance: { label: 'parsecs' }
                }
            }
        };

        var chart = new google.charts.Bar(document.getElementById('columnchart_material'));

        chart.draw(data, google.charts.Bar.convertOptions(options));
      }
    </script>
    <form id="form1" runat="server">
        <div>
            <div id="columnchart_material" style="width: 800px; height: 400px;"></div>
        </div>
    </form>

</asp:Content>

