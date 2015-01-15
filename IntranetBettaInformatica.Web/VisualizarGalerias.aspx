<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VisualizarGalerias.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.VisualizarGalerias" Title="Visualizar Galerias" %>

<%@ Register Assembly="Media-Player-ASP.NET-Control" Namespace="Media_Player_ASP.NET_Control"
    TagPrefix="cc1" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <style type="text/css">
        .images-view .x-panel-body
        {
            background: white !important;
            font: 11px Arial, Helvetica, sans-serif;
        }
        .images-view .thumb
        {
            padding: 3px;
            height:60px;
            text-align:center;
            vertical-align:middle;
        }
        
        .images-view .thumb-wrap
        {
            float: left;
            margin: 4px;
            height:95px;
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
            margin: 5px;
        }
        .tip-target
        {
            width: 210px;
            text-align: center;
            padding: 5px 5px 5px 5px;
            border: 1px dotted #99bbe8;
            background: #dfe8f6;
            cursor: default;
            margin: 5px;
            font: bold 11px tahoma,arial,sans-serif;
            float: left;
        }
    </style>
    <script type="text/javascript">
    	function rowCommentSelect(row) {
			var hdfEditarComentarioFotoValue = Ext.getCmp('ctl00_cph_body_hdfEditarComentarioFoto').getValue();
			var hdfEditarComentarioVideoValue = Ext.getCmp('ctl00_cph_body_hdfEditarComentarioVideo').getValue();
			var hdfRemoverComentarioFotoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverComentarioFoto').getValue();
			var hdfRemoverComentarioVideoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverComentarioVideo').getValue();
        	var usuarioLogado = document.getElementById('ctl00_cph_body_hdfUsuarioLogado').value;
        	var data = !row ? null :  row.selections.items.length == 0 ? null : row.selections.items[0].data;
            var disabled = !row || !data || data.UsuarioId != parseInt(usuarioLogado);
            Ext.getCmp('ctl00_cph_body_btnRemoverComentarioFoto').setDisabled(disabled || hdfRemoverComentarioFotoValue == '0');
            Ext.getCmp('ctl00_cph_body_btnEditarComentarioFoto').setDisabled(disabled || hdfEditarComentarioFotoValue == '0');
            Ext.getCmp('ctl00_cph_body_btnRemoverComentarioVideo').setDisabled(disabled || hdfRemoverComentarioVideoValue == '0');
            Ext.getCmp('ctl00_cph_body_btnEditarComentarioVideo').setDisabled(disabled || hdfEditarComentarioVideoValue == '0');
        }
        function selectRow(row) {
        	var length = Ext.getCmp('ctl00_cph_body_grdGalerias').selModel.selections.length;
			var hdfVisualizarFotosGalerias = Ext.getCmp('ctl00_cph_body_hdfVisualizarFotosGalerias').getValue();
			var hdfVisualizarVideosGalerias = Ext.getCmp('ctl00_cph_body_hdfVisualizarVideosGalerias').getValue();
			var btnVisualizarVideos = Ext.getCmp('ctl00_cph_body_btnVisualizarVideos');
			var btnVisualizarFotos = Ext.getCmp('ctl00_cph_body_btnVisualizarFotos');
			btnVisualizarVideos.setDisabled(length == 0 || hdfVisualizarVideosGalerias == '0');
			btnVisualizarFotos.setDisabled(length == 0 || hdfVisualizarFotosGalerias == '0');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strGalerias" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="QuantidadeFotos" />
                    <ext:RecordField Name="QuantidadeVideos" />
                </Fields>
            </ext:JsonReader>
        </Reader>
		<Listeners>
			<Update Fn="selectRow" />
		</Listeners>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strFotos" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="TituloTruncado" />
                    <ext:RecordField Name="CaminhoImagemThumb" />
                    <ext:RecordField Name="NomeOriginal" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <AutoLoadParams>
            <ext:Parameter Name="start" Value="0" Mode="Raw" />
            <ext:Parameter Name="limit" Value="5" Mode="Raw" />
        </AutoLoadParams>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="This" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strVideos" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="TituloTruncado" />
                    <ext:RecordField Name="CaminhoVideoOriginal" />
                    <ext:RecordField Name="NomeOriginal" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <AutoLoadParams>
            <ext:Parameter Name="start" Value="0" Mode="Raw" />
            <ext:Parameter Name="limit" Value="5" Mode="Raw" />
        </AutoLoadParams>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="This" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strComentarios" runat="server" OnRefreshData="OnRefreshDataComentario">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Comentario" />
                    <ext:RecordField Name="Data" Type="Date" />
                    <ext:RecordField Name="UsuarioNome" ServerMapping="Usuario.Nome" />
                    <ext:RecordField Name="UsuarioId" ServerMapping="Usuario.Id" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
            <ext:GridPanel ID="grdGalerias" runat="server" StoreID="strGalerias"
                Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%"
                AnchorVertical="100%">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button runat="server" ID="btnVisualizarFotos" Text="Visualizar Fotos" Icon="BoxPicture" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnVisualizarFotos_Click">
                                        <EventMask ShowMask="true" Target="Page" />
                                        <ExtraParams>
                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnVisualizarVideos" Text="Visualizar Videos" Icon="Film" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnVisualizarVideos_Click" Method="POST">
                                        <EventMask ShowMask="true" Target="Page" />
                                        <ExtraParams>
                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column ColumnID="Nome" Header="Nome" Width="200" DataIndex="Nome" />
                        <ext:Column Header="Descrição" DataIndex="Descricao" />
                        <ext:Column Header="Qtd. Fotos" DataIndex="QuantidadeFotos" Groupable="false">
                            <Renderer Handler="return value == 1 ? (value + ' foto') : (value + ' fotos');" />
                        </ext:Column>
                        <ext:Column Header="Qtd. Videos" DataIndex="QuantidadeVideos" Groupable="false">
                            <Renderer Handler="return value == 1 ? (value + ' video') : (value + ' videos');" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel runat="server">
                        <Listeners>
							<SelectionChange Fn="selectRow" />
						</Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <BottomBar>
                    <ext:PagingToolbar runat="server" PageSize="20" StoreID="strGalerias">
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
        </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winFotos" Icon="Photo" Maximized="true" Title="Fotos da Galeria"
        Width="800" Height="600" Cls="img-chooser-dlg" Border="false" Modal="true" Hidden="true"
        Maximizable="true" Resizable="true">
        <Items>
            <ext:BorderLayout runat="server">
                <Center>
                    <ext:Panel ID="Panel1" runat="server" Height="380" Frame="true" Layout="Fit" AnchorHorizontal="100%"
                        Border="false">
                        <Items>
                            <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="0.03">
                                        <ext:Panel runat="server" Border="false">
                                            <Items>
                                                <ext:CenterLayout runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnAnterior" runat="server" Icon="PreviousGreen" ToolTip="Anterior"
                                                            ToolTipType="Qtip">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnAnterior_Click">
                                                                    <EventMask Target="CustomTarget" ShowMask="true" CustomTarget="#{pnlImagem}" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:CenterLayout>
                                            </Items>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.94">
                                        <ext:Panel ID="pnlImagem" runat="server" Border="false">
                                            <Items>
                                                <ext:ColumnLayout runat="server" FitHeight="true">
                                                    <Columns>
                                                        <ext:LayoutColumn ColumnWidth="0.6">
                                                            <ext:Panel ID="pnlImagemPrincipal" runat="server" DisabledClass="true" AutoScroll="true">
																<LayoutConfig>
																	<ext:HBoxLayoutConfig Align="Middle" Pack="Center" />
																</LayoutConfig>
                                                                <Items>
                                                                    <ext:Image ID="imgAtual" runat="server">
                                                                    </ext:Image>
                                                                </Items>
                                                            </ext:Panel>
                                                            
                                                        </ext:LayoutColumn>
                                                        <ext:LayoutColumn ColumnWidth="0.4">
                                                            <ext:GridPanel ID="grdComentariosFotos" runat="server" StoreID="strComentarios"
                                                                TrackMouseOver="true" Title="Comentários" AnimCollapse="true" Icon="Comment"
                                                                Layout="Fit">
                                                                <TopBar>
                                                                    <ext:Toolbar runat="server">
                                                                        <Items>
                                                                            <ext:Button ID="btnAdicionarComentarioFoto" runat="server" Text="Adicionar" Icon="Add">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="btnAdicionarComentario_Click">
                                                                                        <EventMask Target="Page" ShowMask="true" />
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:Button>
                                                                            <ext:Button runat="server" Text="Editar" Icon="NoteEdit" ID="btnEditarComentarioFoto"
                                                                                Disabled="true">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="btnEditarComentario_Click">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdComentariosFotos}.getRowsValues({selectedOnly:true})[0].Id">
                                                                                            </ext:Parameter>
                                                                                        </ExtraParams>
                                                                                        <EventMask Target="Page" ShowMask="true" />
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:Button>
                                                                            <ext:Button ID="btnRemoverComentarioFoto" runat="server" Text="Remover" Icon="Delete" Disabled="true">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="btnRemoverComentario_Click">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdComentariosFotos}.getRowsValues({selectedOnly:true})[0].Id">
                                                                                            </ext:Parameter>
                                                                                        </ExtraParams>
                                                                                        <EventMask Target="Page" ShowMask="true" />
                                                                                        <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este comentário?" />
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </TopBar>
                                                                <ColumnModel runat="server">
                                                                    <Columns>
                                                                        <ext:Column Header="Usuário" Width="150" DataIndex="UsuarioNome" />
                                                                        <ext:Column Header="Data" Width="100" DataIndex="Data">
                                                                            <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                                                        </ext:Column>
                                                                    </Columns>
                                                                </ColumnModel>
                                                                <View>
                                                                    <ext:GridView runat="server" ForceFit="true">
                                                                    </ext:GridView>
                                                                </View>
                                                                <SelectionModel>
                                                                    <ext:RowSelectionModel runat="server">
                                                                        <Listeners>
                                                                            <SelectionChange Fn="rowCommentSelect" />
                                                                        </Listeners>
                                                                    </ext:RowSelectionModel>
                                                                </SelectionModel>
                                                                <Plugins>
                                                                    <ext:RowExpander ID="rowExpander" runat="server">
                                                                        <Template runat="server">
                                                                            <Html>
                                                                                <p style="margin-top: 10px; margin-left: 5px;"><b>Comentário:</b> <i>{Comentario}</i></p>
                                                                            </Html>
                                                                        </Template>
                                                                    </ext:RowExpander>
                                                                </Plugins>
                                                                <BottomBar>
                                                                    <ext:PagingToolbar runat="server" PageSize="10"></ext:PagingToolbar>
                                                                </BottomBar>
                                                            </ext:GridPanel>
                                                        </ext:LayoutColumn>
                                                    </Columns>
                                                </ext:ColumnLayout>
                                            </Items>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.03">
                                        <ext:Panel runat="server" Border="false" ButtonAlign="Center">
                                            <Items>
                                                <ext:CenterLayout runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnProximo" runat="server" Icon="NextGreen" ToolTip="Próxima" ToolTipType="Qtip">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnProximo_Click">
                                                                    <EventMask Target="CustomTarget" ShowMask="true" CustomTarget="#{pnlImagem}" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:CenterLayout>
                                            </Items>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                        </Items>
                    </ext:Panel>
                </Center>
                <South Collapsible="True">
                    <ext:Panel ID="pnlFotos" Cls="images-view" Frame="true" Layout="Fit" runat="server"
                        Height="180" Border="false">
                        <Items>
                            <ext:DataView ID="dtImagens" runat="server" SingleSelect="true" OverClass="x-view-over"
                                AutoScroll="true" Height="125" StoreID="strFotos" ItemSelector="div.thumb-wrap">
                                <Template ID="Template1" runat="server">
                                    <Html>
                                        <tpl for=".">
										        <div class="thumb-wrap" id="{Id}" class="tip-target">
										        <div class="thumb"><img src="{CaminhoImagemThumb}" title="{Titulo}" /></div>
										        <span class="x-editable tip-target">{TituloTruncado}</span></div>
								</tpl>
                                        <div class="x-clear"></div>
                                    </Html>
                                </Template>
                                <DirectEvents>
                                    <SelectionChange OnEvent="dtImagens_SelectionChange">
                                        <EventMask Target="CustomTarget" ShowMask="true" CustomTarget="#{pnlImagem}" />
                                    </SelectionChange>
                                </DirectEvents>
                            </ext:DataView>
                        </Items>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pgToolbarFoto" StoreID="strFotos" PageSize="5" HideRefresh="true">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:Panel>
                </South>
            </ext:BorderLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Fechar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winFotos}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winVideos" Icon="Film" Maximized="true" Title="Videos da Galeria"
        Width="800" Height="600" Cls="img-chooser-dlg" Border="false" Modal="true" Hidden="true" AutoShow="true"
        Resizable="false">
        <Items>
            <ext:BorderLayout runat="server">
                <Center>
                    <ext:Panel ID="pnlVideo" runat="server" Height="380" Frame="true" Layout="Fit" AnchorHorizontal="100%"
                        Border="false">
                        <Items>
                            <ext:ColumnLayout runat="server" FitHeight="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="0.6">
                                        <ext:Panel ID="pnlVideoFlash" runat="server" Border="false" MinWidth="500" AutoScroll="true"></ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.4">
                                        <ext:GridPanel ID="grdComentariosVideos" runat="server" StoreID="strComentarios" TrackMouseOver="true"
                                            Title="Comentários" AnimCollapse="true" Icon="Comment" Layout="Fit">
                                            <TopBar>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnAdicionarComentarioVideo" runat="server" Text="Adicionar" Icon="Add">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnAdicionarComentario_Click">
                                                                    <EventMask Target="Page" ShowMask="true" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button runat="server" Text="Editar" Icon="NoteEdit" ID="btnEditarComentarioVideo" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnEditarComentario_Click">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="id" Mode="Raw" Value="#{grdComentariosVideos}.getRowsValues({selectedOnly:true})[0].Id">
                                                                        </ext:Parameter>
                                                                    </ExtraParams>
                                                                    <EventMask Target="Page" ShowMask="true" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="btnRemoverComentarioVideo" runat="server" Text="Remover" Icon="Delete" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnRemoverComentario_Click">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="id" Mode="Raw" Value="#{grdComentariosVideos}.getRowsValues({selectedOnly:true})[0].Id">
                                                                        </ext:Parameter>
                                                                    </ExtraParams>
                                                                    <EventMask Target="Page" ShowMask="true" />
                                                                    <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este comentário?" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel runat="server">
                                                <Columns>
                                                    <ext:Column Header="Usuário" Width="150" DataIndex="UsuarioNome" />
                                                    <ext:Column Header="Data" Width="100" DataIndex="Data">
                                                        <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <View>
                                                <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                                            </View>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server">
                                                    <Listeners>
                                                        <SelectionChange Fn="rowCommentSelect" />
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            <Plugins>
                                                <ext:RowExpander ID="rowExpanderVideo" runat="server">
                                                    <Template runat="server">
                                                        <Html>
                                                            <p style="margin-top: 10px; margin-left: 5px;"><b>Comentário:</b> <i>{Comentario}</i></p>
                                                        </Html>
                                                    </Template>
                                                </ext:RowExpander>
                                            </Plugins>
                                            <BottomBar>
                                                <ext:PagingToolbar runat="server" PageSize="10"></ext:PagingToolbar>
                                            </BottomBar>
                                        </ext:GridPanel>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                        </Items>
                    </ext:Panel>
                </Center>
                <South Collapsible="True">
                    <ext:Panel Cls="images-view" Frame="true" Layout="Fit" runat="server" AnchorVertical="100%"
                        Height="180" Border="false">
                        <Items>
                            <ext:DataView ID="dtVideos" runat="server" SingleSelect="true" OverClass="x-view-over"
                                AutoScroll="true" Height="125" StoreID="strVideos" ItemSelector="div.thumb-wrap">
                                <Template runat="server">
                                    <Html>
                                        <tpl for=".">
										        <div class="thumb-wrap" id="{Id}" class="tip-target">
										        <div class="thumb"><img src="Imagens/video.gif" title="{Titulo}" /></div>
										        <span class="x-editable tip-target">{TituloTruncado}</span></div>
								</tpl>
                                        <div class="x-clear"></div>
                                    </Html>
                                </Template>
                                <DirectEvents>
                                    <SelectionChange OnEvent="dtVideos_SelectionChange" Method="POST">
                                        <EventMask Target="CustomTarget" ShowMask="true" CustomTarget="#{pnlVideo}" />
                                    </SelectionChange>
                                </DirectEvents>
                            </ext:DataView>
                        </Items>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pgToolbarVideo" StoreID="strVideos" PageSize="5" HideRefresh="true">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:Panel>
                </South>
            </ext:BorderLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Fechar" Icon="Cancel">
                <DirectEvents>
                    <Click OnEvent="btnFecharvideo_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="winComentario" runat="server" Icon="Comment" Width="500" Height="230"
        Border="false" Modal="true" Hidden="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" ID="frmComentario" Frame="true">
                            <Items>
                                <ext:TextArea runat="server" ID="txtComentario" FieldLabel="Comentário" MaxLength="200"
                                    AnchorHorizontal="92%" AnchorVertical="92%" AllowBlank="false" MsgTarget="Side">
                                </ext:TextArea>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmComentario}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="btnSalvarComentario_Click">
                        <EventMask Target="Page" ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winComentario}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <asp:HiddenField runat="server" ID="hdfUsuarioLogado" />
	<ext:Hidden runat="server" ID="hdfVisualizarGalerias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarFotosGalerias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarVideosGalerias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarComentarioFoto" Text="0" />
	<ext:Hidden runat="server" ID="hdfAdicionarComentarioVideo" Text="0" />
	<ext:Hidden runat="server" ID="hdfEditarComentarioFoto" Text="0" />
	<ext:Hidden runat="server" ID="hdfEditarComentarioVideo" Text="0" />
	<ext:Hidden runat="server" ID="hdfRemoverComentarioFoto" Text="0" />
	<ext:Hidden runat="server" ID="hdfRemoverComentarioVideo" Text="0" />
</asp:Content>
