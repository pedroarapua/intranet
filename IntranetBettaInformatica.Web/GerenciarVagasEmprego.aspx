<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarVagasEmprego.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarVagasEmprego" Title="Mural de Vagas" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function selectRow(row) {
			var length = Ext.getCmp('ctl00_cph_body_grdVagasEmprego').selModel.selections.length;
			var data = length > 0 ? Ext.getCmp('ctl00_cph_body_grdVagasEmprego').selModel.selections.items[0].data : null;
			var hdfEditarVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfEditarVagaEmprego').getValue(); 
			var hdfRemoverVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverVagaEmprego').getValue(); 
			var hdfFecharVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfFecharVagaEmprego').getValue();		
			var hdfVisualizarCurriculosVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfVisualizarCurriculosVagaEmprego').getValue();	
			var hdfIndicarVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfIndicarVagaEmprego').getValue();

			Ext.getCmp('ctl00_cph_body_btnIndicarVagaEmprego').setDisabled(length == 0 || hdfIndicarVagaEmpregoValue == '0' || (data != null && data.StatusId == 2));
			Ext.getCmp('ctl00_cph_body_btnVisualizarCurriculos').setDisabled(length == 0 || hdfVisualizarCurriculosVagaEmpregoValue == '0');
			Ext.getCmp('ctl00_cph_body_btnFecharVagaEmprego').setDisabled(length == 0 || hdfFecharVagaEmpregoValue == '0' || (data != null && data.StatusId == 2));
			Ext.getCmp('ctl00_cph_body_btnEditar').setDisabled(length == 0 || hdfEditarVagaEmpregoValue == '0' || (data != null && data.StatusId == 2));
			Ext.getCmp('ctl00_cph_body_btnRemover').setDisabled(length == 0 || hdfRemoverVagaEmpregoValue == '0' || (data != null && data.StatusId == 2));
		}
		function selectRowCurriculo(row) {
			var length = Ext.getCmp('ctl00_cph_body_grdCurriculos').selModel.selections.length;
			var hdfDownloadCurriculoVagaEmpregoValue = Ext.getCmp('ctl00_cph_body_hdfDownloadCurriculoVagaEmprego').getValue();
			var btnDownload = Ext.getCmp('ctl00_cph_body_btnDownload');
			btnDownload.setDisabled(length == 0 || hdfDownloadCurriculoVagaEmpregoValue == '0');
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strVagasEmprego" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
					<ext:RecordField Name="Codigo" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Descricao" />
					<ext:RecordField Name="Status" />
					<ext:RecordField Name="StatusIcon" />
					<ext:RecordField Name="StatusId" />
					<ext:RecordField Name="QtdIndicacoes" />
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
	<ext:Store ID="strCurriculos" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
	
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdVagasEmprego" runat="server" StoreID="strVagasEmprego"
                        Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%" AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Nova" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditar_Click">
                                                <EventMask ShowMask="true" Target="Page" />
												<ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta vaga de emprego?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:ToolbarSeparator></ext:ToolbarSeparator>
									<ext:Button runat="server" ID="btnVisualizarCurriculos" Text="Currículos" Icon="Vcard" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnCurriculos_Click">
                                                <EventMask ShowMask="true" Target="Page" />
												<ExtraParams>
													<ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
													</ext:Parameter>
												</ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:Button runat="server" ID="btnFecharVagaEmprego" Text="Fechar Vaga" Icon="StopRed" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnFecharVaga_Click">
                                                <EventMask ShowMask="true" Target="Page" />
												<ExtraParams>
													<ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
													</ext:Parameter>
												</ExtraParams>
												<Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja fechar esta vaga de emprego?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:Button runat="server" ID="btnIndicarVagaEmprego" Text="Indicar" Icon="VcardAdd" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnIndicarVaga_Click">
                                                <EventMask ShowMask="true" Target="Page" />
												<ExtraParams>
													<ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
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
								<ext:TemplateColumn DataIndex="Status" Header=" " Width="30" Hideable="false" Resizable="false" Align="Center">
                                    <Template runat="server">
                                        <Html>
                                            <div class="{StatusIcon}" style="height:16px;width:16px;float:left;margin-right:5px;" title="{Status}" alt="{Status}"></div>
                                            <div style="padding-top:2px;">{Codigo}</div>
                                        </Html>
                                    </Template>
                                </ext:TemplateColumn>
								<ext:Column Header="Título" DataIndex="Titulo" Width="150" />
                                <ext:Column Header="Descrição" DataIndex="Descricao" Width="250" />
								<ext:Column Header="Qtd. de Indicações" DataIndex="QtdIndicacoes" Width="40" Align="Center" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server">
								<Listeners>
									<SelectionChange Fn="selectRow" />
								</Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarVagaEmprego').getValue() == '1';" />
						</Listeners>
						<DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
								<ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false"
                                EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
						<BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strVagasEmprego">
                                <Items>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator> 
                                    <ext:Label ID="Label1" runat="server" Text="Legenda: " HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container ID="Container1" runat="server" Cls="icon-recordgreen" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label ID="Label2" runat="server" Text="Aberta" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container ID="Container2" runat="server" Cls="icon-bulletshape" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label ID="Label3" runat="server" Text="Fechada" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winVagaEmprego" Icon="Layers" Width="600" Height="400"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmVagaEmprego" AnchorVertical="100%">
                            <Items>
								<ext:TextField runat="server" ID="txtTitulo" MaxLength="200" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Título" MsgTarget="Side">
                                </ext:TextField>
								<ext:TextArea runat="server" ID="txtDescricao" MaxLength="2000" AllowBlank="false"
                                    AnchorHorizontal="92%" AnchorVertical="-30" FieldLabel="Descrição" MsgTarget="Side">
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
                    <Click Handler="return #{frmVagaEmprego}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winVagaEmprego}.hide()" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Window runat="server" ID="winCurriculos" Icon="Vcard" Width="500" Height="350" Title="Currículos da Vaga"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
			<ext:FitLayout runat="server">
                <Items>
					<ext:GridPanel runat="server" ID="grdCurriculos" StoreID="strCurriculos" Frame="false" Layout="FitLayout" AutoExpandColumn="Nome">
						<TopBar>
							<ext:Toolbar runat="server">
								<Items>
									<ext:Button ID="btnDownload" Text="Download" Icon="ArrowDown" Disabled="true" runat="server">
										<DirectEvents>
											<Click OnEvent="btnDowload_Click" Method="POST">
												<ExtraParams>
													<ext:Parameter Name="id" Mode="Raw" Value="#{grdCurriculos}.getRowsValues({selectedOnly:true})[0].Id">
													</ext:Parameter>
												</ExtraParams>
											</Click>
										</DirectEvents>
									</ext:Button>
								</Items>
							</ext:Toolbar>
						</TopBar>
						<ColumnModel>
							<Columns>
								<ext:Column DataIndex="Nome" Header="Nome" Hideable="false"></ext:Column>
							</Columns>
						</ColumnModel>
						<SelectionModel>
							<ext:RowSelectionModel runat="server">
								<Listeners>
									<SelectionChange Fn="selectRowCurriculo"/>
								</Listeners>
							</ext:RowSelectionModel>
						</SelectionModel>
					</ext:GridPanel>
				</Items>
			</ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Fechar">
                <Listeners>
                    <Click Handler="#{winCurriculos}.hide()" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>

	<ext:Window runat="server" ID="winIndicar" Icon="VcardAdd" Width="450" Height="150" Title="Indicar Vaga"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
           <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmIndicar" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:FileUploadField runat="server" ID="txtCurriculo" AnchorHorizontal="92%" FieldLabel="Currículo" MsgTarget="Side"
                                    AllowBlank="false" EmptyText="Selecione o currículo...">
                                </ext:FileUploadField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
			<ext:Button runat="server" Text="Salvar" Icon="Disk">
				<Listeners>
					<Click Handler="return #{frmIndicar}.validate();" />
				</Listeners>
                <DirectEvents>
					<Click OnEvent="btnSalvarCurriculo_Click">
						<ExtraParams>
							<ext:Parameter Name="id" Mode="Raw" Value="#{grdVagasEmprego}.getRowsValues({selectedOnly:true})[0].Id">
							</ext:Parameter>
						</ExtraParams>
					</Click>
				</DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winIndicar}.hide()" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>

	<ext:Hidden runat="server" ID="hdfVisualizarVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfFecharVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarCurriculosVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfDownloadCurriculoVagaEmprego" Text="0"/>
	<ext:Hidden runat="server" ID="hdfIndicarVagaEmprego" Text="0"/>
</asp:Content>
