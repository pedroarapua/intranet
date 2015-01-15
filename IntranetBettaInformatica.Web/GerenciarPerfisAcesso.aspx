<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarPerfisAcesso.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarPerfisAcesso" Title="Perfis de Acesso" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
    		function getAcoes(array) {
    			var args = [];
    			for (var i = 0; i < array.length; i++) {
    				if (array[i].Checked)
    					args[args.length] = { Id: array[i].Id };
    			}
    			return args;
    		}
           function fnCellClick (grid, rowIndex, columnIndex, e) {
           		if (columnIndex == 0) {
           			var array = [1,2,3,4, 103];
           			var hdfModeradorValue = Ext.getCmp('ctl00_cph_body_hdfModerador').getValue();
           			var data = grid.getStore().getAt(rowIndex).data;
           			if (data.Checked && data.Descricao == 'Visualizar') {
           				var possuiAcao = false;
           				for (var i = 0; i <= grid.getStore().data.length - 1; i++) {
           					var data1 = grid.getStore().getAt(i).data;
           					if (data1.PaginaId == data.PaginaId) {
           						if (data1.Checked && data1.Descricao != 'Visualizar') {
           							possuiAcao = true;
           							break;
           						}
           					}
           				}
           				if (!possuiAcao) {
           					grid.getStore().getAt(rowIndex).data.Checked = (hdfModeradorValue == '1' && contains(array, data.Id)) || !data.Checked;
           				}
           			}
           			else {
           				grid.getStore().getAt(rowIndex).data.Checked = (hdfModeradorValue == '1' && contains(array, data.Id)) || !data.Checked;

           				if (grid.getStore().getAt(rowIndex).data.Checked) {
           					for (var i = rowIndex - 1; i > 0; i--) {
           						var data1 = grid.getStore().getAt(i).data;
           						if (data1.PaginaId == data.PaginaId) {
           							if (data1.Descricao == 'Visualizar') {
           								data1.Checked = true;
           								break;
           							}
           						}
           					}
           				}
           			}
					grid.getView().refresh();
				}
			}

			function contains(array, value) {
				for (var i = 0; i < array.length; i++) {
					if (array[i] == value)
						return true;
				}
				return false;
			}
			function selectRow(row) {
				var hdfEditarPerfisAcessoValue = Ext.getCmp('ctl00_cph_body_hdfEditarPerfisAcesso').getValue();
				var hdfRemoverPerfisAcessoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverPerfisAcesso').getValue();
				var data = !row ? null : row.selections.items.length == 0 ? null : row.selections.items[0].data;
				var length = Ext.getCmp('ctl00_cph_body_grdPerfisAcesso').selModel.selections.length;
				var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
				var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
				btnRemover.setDisabled(length == 0 || hdfRemoverPerfisAcessoValue == '0' || data.Id == 1); // id do moderador
				btnEditar.setDisabled(length == 0 || hdfEditarPerfisAcessoValue == '0');
			}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strPerfisAcesso" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Descricao" />
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
	<ext:Store ID="strAcoes" runat="server" GroupField="Pagina">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Pagina" ServerMapping="Pagina.Descricao" />
					<ext:RecordField Name="PaginaId" ServerMapping="Pagina.Id" />
                    <ext:RecordField Name="Descricao" />
					<ext:RecordField Name="Checked" />
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
                    <ext:GridPanel ID="grdPerfisAcesso" runat="server" StoreID="strPerfisAcesso"
                        Height="340" Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%"
                        AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Novo" Icon="Add">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPerfisAcesso}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover"  Disabled="true" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPerfisAcesso}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este perfil de acesso?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Nome" DataIndex="Nome" Width="200" />
                                <ext:Column ColumnID="Descricao" Header="Descrição" Width="300" DataIndex="Descricao" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarPerfisAcesso').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPerfisAcesso}.getRowsValues({selectedOnly:true}))">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strPerfisAcesso">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winPerfilAcesso" Icon="Theme" Width="650" Height="400" Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:TabPanel ID="tab" runat="server" AnchorVertical="100%">
                            <Items>
                                <ext:FormPanel runat="server" Frame="true" ID="frmPerfilAcesso" Title="Dados do Perfil"
                                    AnchorVertical="100%">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtNome" MaxLength="50" AllowBlank="false" AnchorHorizontal="92%"
                                            FieldLabel="Nome" MsgTarget="Side">
                                        </ext:TextField>
                                        <ext:TextArea runat="server" ID="txtDescricao" MaxLength="100" AnchorHorizontal="92%"
                                            Height="200" FieldLabel="Descrição">
                                        </ext:TextArea>
                                    </Items>
                                </ext:FormPanel>
                                <ext:Panel runat="server" ID="pnlAcoes" AnchorHorizontal="100%" AnchorVertical="100%" Layout="FitLayout" Title="Permissões">
                                    <Items>
										<ext:GridPanel runat="server" Layout="FitLayout" ID="grdAcoes" StoreID="strAcoes">
											 <ColumnModel ID="ColumnModel1" runat="server">
												<Columns>
													<ext:CheckColumn Align="Center" DataIndex="Checked" Width="50" Groupable="false" Fixed="false"></ext:CheckColumn>
													<ext:Column ColumnID="Descricao" Header="Descrição" Width="300" DataIndex="Descricao" Groupable="false" />
													<ext:Column Header="Acesso à" DataIndex="Pagina" MenuDisabled="true" Hidden="true">
														<Renderer Handler="return value == null ? 'Outras Permissões' : value;" />
													</ext:Column>
												</Columns>
											</ColumnModel>
											 <View>
												<ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" StartCollapsed="false" EnableRowBody="true" >
												</ext:GroupingView>
											</View>
											<Listeners>
												<CellClick Fn="fnCellClick" />
											</Listeners>
										</ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:TabPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmPerfilAcesso}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="acoes" Value="Ext.encode(getAcoes(#{grdAcoes}.getRowsValues()))"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winPerfilAcesso}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfModerador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarPerfisAcesso" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarPerfisAcesso" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarPerfisAcesso" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverPerfisAcesso" Text="0"/>
</asp:Content>
