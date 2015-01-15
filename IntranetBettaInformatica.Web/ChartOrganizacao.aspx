<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartOrganizacao.aspx.cs" Inherits="IntranetBettaInformatica.Web.ChartOrganizacao" Title="Organograma" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html>
	<head>
		<script src="Js/Util.js" type="text/javascript"></script>
		<script type="text/javascript" src="http://www.google.com/jsapi"></script>
		<script type="text/javascript">
			google.load('visualization', '1', { packages: ['orgchart'] });
			google.setOnLoadCallback(drawChart);
			function drawChart() {
				var args = Ext.decode(document.getElementById('hdfValores').value);
				var data = new google.visualization.DataTable();
				data.addColumn('string', 'Name');
				data.addColumn('string', 'Manager');
				data.addColumn('string', 'ToolTip');
				for (var i = 0; i < args.length; i++) {
					data.addRow([{ v: args[i].Nome, f: args[i].Nome + '<div style="color:red; font-style:italic;">' + (args[i].SetorPai == null ? 'Empresa' : '') + '</div>' }, args[i].SetorPai == null ? '' : args[i].SetorPai.Nome, args[i].Nome]);
				}
				var chart = new google.visualization.OrgChart(document.getElementById('chart_div'));
				chart.draw(data, { allowHtml: true });
			}
		</script>
	</head>
	<body style="padding:10px;">
		<form runat="server">
			<ext:ResourceManager ID="ResourceManager1" runat="server">
			</ext:ResourceManager>
			<asp:HiddenField runat="server" ID="hdfValores" />
			<div id="chart_div"></div>
		</form>
	</body>
</html>

