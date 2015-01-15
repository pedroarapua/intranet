<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VisualizarOrganizacao.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.VisualizarOrganizacao" Title="Estrutura Organizacional" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
     <ext:FitLayout ID="FitLayout1" runat="server">
        <Items>
			<ext:FormPanel runat="server" ID="frmTitulo" AnchorHorizontal="100%" AnchorVertical="100%" Frame="true" Border="false" AutoScroll="true">
				<Content>
					<iframe id="frmChart" runat="server" style="width:99.5%; height:99.3%;" src="ChartOrganizacao.aspx"></iframe>
				</Content>
			</ext:FormPanel>
		</Items>
	</ext:FitLayout>
</asp:Content>
