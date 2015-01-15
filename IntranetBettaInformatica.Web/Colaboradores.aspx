<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Colaboradores.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.Colaboradores" Title="Colaboradores" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function selectRow(dv) {
			var hdfVisualizarPerfilColaboradorValue = Ext.getCmp('ctl00_cph_body_hdfVisualizarPerfilColaborador').getValue();
			Ext.getCmp('ctl00_cph_body_btnVisualizarPerfil').setDisabled(dv.getSelectedNodes().length == 0 || hdfVisualizarPerfilColaboradorValue == '0');
		}
	</script>
	 <style type="text/css">
        .x-column-padding
        {
            padding: 10px 0px 10px 10px;
        }
        
        .x-column-padding1
        {
            padding: 10px;
        }
        .images-view .x-panel-body
        {
            background: white !important;
            font: 11px Arial, Helvetica, sans-serif;
        }
        .images-view .thumb
        {
            padding: 0px;
            height: 70px;
            text-align: center;
            vertical-align: middle;
        }
        
        .images-view .thumb-wrap
        {
            float: left;
            margin: 2px;
            height: 105px;
            width: 135px;
            margin-right: 0;
            padding: 5px;
            text-align: center;
        }
        .images-view .thumb-wrap span
        {
            display: block;
            overflow: hidden;
            text-align: center;
        }
        
        .images-view .x-view-over
        {
            border: 1px solid #dddddd;
            background: #efefef url(../../Shared/images/row-over.gif) repeat-x left top;
            padding: 4px;
        }
        
        .images-view .x-view-selected
        {
            background: #eff5fb url(../../Shared/images/selected.gif) no-repeat right bottom;
            border: 1px solid #99bbe8;
            padding: 4px;
        }
        .images-view .x-view-selected .thumb
        {
            background: transparent;
        }
        
        .images-view .loading-indicator
        {
            font-size: 11px;
            background-image: url(../../Shared/images/loading.gif);
            background-repeat: no-repeat;
            background-position: left;
            padding-left: 10px;
            margin: 2px;
        }
        .tip-target
        {
            width: 112px;
            height: 27px;
            text-align: center;
            padding: 2px 5px;
            border: 1px dotted #99bbe8;
            background: #dfe8f6;
            cursor: default;
            margin: 1px 5px;
            font: bold 11px tahoma,arial,sans-serif;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strColaboradores" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="TruncateNome" />
                    <ext:RecordField Name="CaminhoImagemThumbs" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
	<ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%" Cls="images-view" Frame="true">
                <TopBar>
					<ext:Toolbar runat="server">
						<Items>
							<ext:Button runat="server" ID="btnVisualizarPerfil" Icon="ApplicationViewTile" Text="Visualizar Perfil" Disabled="true">
								<Listeners>
									<Click Handler="Ext.net.DirectMethods.OpenPerfilUsuario(Ext.getCmp('ctl00_cph_body_dtImagens').getSelectedNodes()[0].id);" />
								</Listeners>
							</ext:Button>
						</Items>
					</ext:Toolbar>
				</TopBar>
				<Items>
					<ext:DataView ID="dtImagens" runat="server" SingleSelect="true" OverClass="x-view-over" AutoScroll="true" Height="250" StoreID="strColaboradores" ItemSelector="div.thumb-wrap">
                        <Template ID="Template1" runat="server">
                            <Html>
                                <tpl for=".">
									<div class="thumb-wrap" id="{Id}" class="tip-target">
									<div class="thumb"><img src="{CaminhoImagemThumbs}" title="{Nome}" /></div>
									<span class="x-editable tip-target">{TruncateNome}</span></div>
								</tpl>
                                <div class="x-clear"></div>
                            </Html>
                        </Template>
						<Listeners>
							<SelectionChange Fn="selectRow" />
							<DblClick Handler="if(Ext.getCmp('ctl00_cph_body_hdfVisualizarPerfilColaborador').getValue() == '1') fnVisualizarPerfil(Ext.getCmp('ctl00_cph_body_dtImagens'));" />
						</Listeners>
				    </ext:DataView>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Hidden runat="server" ID="hdfVisualizarColaboradores" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarPerfilColaborador" Text="0"/>
</asp:Content>
