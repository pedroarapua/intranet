<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="IntranetBettaInformatica.Web._Default" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script type="text/javascript" src="http://widgets.twimg.com/j/2/widget.js"></script>
	<script src="Js/Util.js" type="text/javascript"></script>
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
        .custom_twitter { background-image:url(Imagens/icone_twitter.png) !important;height: 16px; width:16ox; }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strUsuarios" runat="server">
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

    <ext:Store ID="strAniversariantes" runat="server">
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

	<ext:Store ID="strNoticias" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
	<ext:Store ID="strReunioes" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Codigo" />
                    <ext:RecordField Name="Titulo" />
					<ext:RecordField Name="Horario"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>

    <ext:FitLayout ID="FitLayout1" runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%"
                Layout="fit">
                <Items>
                    <ext:Portal ID="Portal1" runat="server" Border="false" Layout="Column">
                        <Items>
                            <ext:PortalColumn ID="PortalColumnAniversariantes" runat="server" Cls="x-column-padding" ColumnWidth=".33"
                                Layout="Anchor">
                                <Items>
                                    <ext:Portlet ID="ptlAniversariantes" runat="server" Collapsible="false" Height="275" Icon="Cake" Title="Aniversariantes" BodyStyle="background-color:white;">
                                        <Items>
                                            <ext:Panel ID="pnlAniversariantes" Cls="images-view" Frame="true" Layout="Fit" runat="server" BodyStyle="background-color:white;"
                                                Height="250" Border="false">
												<Items>
                                                    <ext:DataView ID="dtAniversariantes" runat="server" SingleSelect="true" OverClass="x-view-over"
                                                        AutoScroll="true" Height="250" StoreID="strAniversariantes" ItemSelector="div.thumb-wrap" BodyStyle="background-color:white;">
                                                        <Template runat="server">
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
															<DblClick Fn="fnVisualizarPerfil" />
														</Listeners>
                                                    </ext:DataView>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Portlet>
									<ext:Portlet ID="ptlNoticias" runat="server" Title="Notícias" Collapsible="false"  Height="275" Icon="LightningGo" BodyStyle="background-color:white;">
                                        <Items>
											<ext:Label runat="server" ID="lblNenhumaNoticia" HideLabel="true" Hidden="true" Text="&nbsp;&nbsp;Nenhuma notícia."></ext:Label>
											<ext:GridPanel runat="server" ID="grdNoticias" Layout="FitLayout" StoreID="strNoticias" AutoExpandColumn="Titulo" Height="250" HideHeaders="true">
												<ColumnModel>
													<Columns>
														<ext:TemplateColumn DataIndex="Titulo" Hidden="false" Groupable="false">
															<Template runat="server">
																<Html>
																	<span style="font-style: italic;">{Titulo}</span>
																</Html>
															</Template>
														</ext:TemplateColumn>
													</Columns>
												</ColumnModel>
												<SelectionModel>
													<ext:RowSelectionModel ID="RowSelectionModel1" runat="server">
													</ext:RowSelectionModel>
												</SelectionModel>
												<Listeners>
													<DblClick Handler="window.location = 'VisualizarNoticias.aspx';" />
												</Listeners>
											</ext:GridPanel>
										</Items>
                                    </ext:Portlet>
                                </Items>
                            </ext:PortalColumn>
                            <ext:PortalColumn ID="PortalColumnColaboradores" runat="server" Cls="x-column-padding1" ColumnWidth=".33"
                                Layout="Anchor">
                                <Items>
                                    <ext:Portlet ID="ptlUsuarios" runat="server" Title="Colaboradores" Collapsible="false" Height="275" BodyStyle="background-color:white;"
                                        Icon="UserHome">
                                        <Items>
                                            <ext:Panel ID="pnlColaboradores" Cls="images-view" Frame="true" Layout="Fit" runat="server" BodyStyle="background-color:white;"
                                                Height="250" Border="false">
                                                <Items>
                                                    <ext:DataView ID="dtImagens" runat="server" SingleSelect="true" OverClass="x-view-over" BodyStyle="background-color:white;"
                                                        AutoScroll="true" Height="250" StoreID="strUsuarios" ItemSelector="div.thumb-wrap">
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
															<DblClick Handler="if(Ext.getCmp('ctl00_cph_body_hdfVisualizarPerfilColaborador').getValue() == '1') fnVisualizarPerfil(Ext.getCmp('ctl00_cph_body_dtImagens'));" />
														</Listeners>
                                                    </ext:DataView>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Portlet>
									<ext:Portlet ID="ptlReunioes" runat="server" Title="Reuniões" Collapsible="false"  Height="275" Icon="BookOpen" Frame="true" BodyStyle="background-color:white;">
                                        <Items>
											<ext:Label runat="server" ID="lblNenhumaReuniao" HideLabel="true" Hidden="true" Text="&nbsp;&nbsp;Nenhuma reunião."></ext:Label>
											<ext:GridPanel runat="server" ID="grdReunioes" Layout="FitLayout" StoreID="strReunioes" AutoExpandColumn="Titulo" Height="250" HideHeaders="true">
												<ColumnModel>
													<Columns>
														<ext:TemplateColumn DataIndex="Codigo" Hidden="false" Groupable="false" Width="50">
															<Template ID="Template2" runat="server">
																<Html>
																	{Codigo}
																</Html>
															</Template>
														</ext:TemplateColumn>
														<ext:TemplateColumn DataIndex="Titulo" Hidden="false" Groupable="false">
															<Template runat="server">
																<Html>
																	<span style="font-style: italic;">{Titulo}&nbsp;</span>
																</Html>
															</Template>
														</ext:TemplateColumn>
														<ext:TemplateColumn DataIndex="DataInicial" Hidden="false" Groupable="false" Width="50">
															<Template ID="Template3" runat="server">
																<Html>
																	 <span style="font-weight:bold;">{Horario}</span>
																</Html>
															</Template>
														</ext:TemplateColumn>
													</Columns>
												</ColumnModel>
												<SelectionModel>
													<ext:RowSelectionModel ID="RowSelectionModel2" runat="server">
													</ext:RowSelectionModel>
												</SelectionModel>
												<Listeners>
													<DblClick Handler="window.location = 'GerenciarReunioes.aspx';" />
												</Listeners>
											</ext:GridPanel>
										</Items>
                                    </ext:Portlet>
                                </Items>
                            </ext:PortalColumn>
							<ext:PortalColumn ID="PortalColumnTwitter" runat="server" Cls="x-column-padding" ColumnWidth=".33"
                                Layout="Anchor">
                                <Items>
                                    <ext:Portlet ID="ptlTwitter" runat="server" Title="Twitter" Collapsible="false"  Height="275" IconCls="custom_twitter" BodyStyle="background-color:white;">
                                        <Content>
                                            <asp:Literal runat="server" ID="litTwitter"></asp:Literal>
                                        </Content>
                                    </ext:Portlet>
                                </Items>
                            </ext:PortalColumn>
                        </Items>
                    </ext:Portal>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
	<ext:Hidden runat="server" ID="hdfVisualizarPerfilColaborador"></ext:Hidden>
</asp:Content>
