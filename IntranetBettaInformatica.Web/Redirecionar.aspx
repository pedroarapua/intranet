<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Redirecionar.aspx.cs" Inherits="IntranetBettaInformatica.Web.Redirecionar" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
	<script src="Js/Util.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
     <ext:FitLayout ID="FitLayout1" runat="server">
        <Items>
            <ext:Panel runat="server" ID="pnlIFrame" AnchorVertical="100%" AnchorHorizontal="100%">
            </ext:Panel>
        </Items>
    </ext:FitLayout>    
</asp:Content>
