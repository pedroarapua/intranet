<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarEmpresas.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarEmpresas" Title="Empresas" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <ext:XScript ID="XScript1" runat="server">
        <script type="text/javascript">
            var addTelefone = function () {
                var grid = #{grdTelefones};
                grid.getRowEditor().stopEditing();
                
                grid.addRecord(0, {
                    Telefone : "",
                    Id: 0
                });
                
                grid.getView().refresh();
                var length = grid.getView().getRows().length - 1;
                grid.getSelectionModel().selectRow(length);
                grid.getRowEditor().startEditing(length);
            }
            
            var removeTelefone = function () {
                var grid = #{grdTelefones};
                grid.getRowEditor().stopEditing();
                
                var s = grid.getSelectionModel().getSelections();
                
                for (var i = 0, r; r = s[i]; i++) {
                    #{strTelefones}.remove(r);
                }
                Ext.getCmp('ctl00_cph_body_btnAddTelefone').setDisabled(false);
                Ext.getCmp('ctl00_cph_body_btnRemoveTelefone').setDisabled(true);
            }
        </script>
    </ext:XScript>
    <script type="text/javascript">
        function cancelTelefone(cmp, valido, btn) {
            if (!valido) {
                var grid = Ext.getCmp('ctl00_cph_body_grdTelefones');
                var length = grid.getView().getRows().length - 1;
                alert('Erro', 'Telefone inválido.');
                grid.getRowEditor().startEditing(length);
            }
            else {

                Ext.getCmp('ctl00_cph_body_btnAddTelefone').setDisabled(false);
                Ext.getCmp('ctl00_cph_body_btnSalvarTelefones').setDisabled(false);
                if (cmp.record.data.Telefone == '' && btn == 'btnCancel') {
                    removeTelefone();
                }
                else {
                    Ext.getCmp('ctl00_cph_body_btnRemoveTelefone').setDisabled(false);
                }
            }
        }
        function salvarTelefone() {
            desabilitarBotoesTelefone(false);
        }
        function telefoneRowSelect(a) {
            Ext.getCmp('ctl00_cph_body_btnRemoveTelefone').setDisabled(a == null);
        }
        function desabilitarBotoesTelefone(disabled) {
            Ext.getCmp('ctl00_cph_body_btnAddTelefone').setDisabled(disabled);
            Ext.getCmp('ctl00_cph_body_btnRemoveTelefone').setDisabled(disabled);
            Ext.getCmp('ctl00_cph_body_btnSalvarTelefones').setDisabled(disabled);
           }
        function fnOpenChartOrganization() {
        	var rows = Ext.getCmp('ctl00_cph_body_grdEmpresas').getRowsValues({ selectedOnly: true });
        	if (rows.length > 0) {
        		Ext.net.DirectMethods.OpenChartOrganization(rows[0].Id);
			}
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <script src="Js/InputTextMask.js" type="text/javascript"></script>
    <ext:Store ID="strEmpresas" runat="server" OnRefreshData="OnRefreshData" GroupField="TipoEmpresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Endereco" />
                    <ext:RecordField Name="Email" />
                    <ext:RecordField Name="Site" />
                    <ext:RecordField Name="TipoEmpresa" ServerMapping="TipoEmpresa.Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strTiposEmpresa" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strTelefones" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Telefone" />
                    <ext:RecordField Name="Id" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strEstados" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Sigla" />
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
                    <ext:GridPanel ID="grdEmpresas" runat="server" StoreID="strEmpresas"
                        Frame="true" AutoExpandColumn="Nome" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdEmpresas}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdEmpresas}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta empresa?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnTelefones" Text="Adicionar Telefones" Icon="Phone">
                                        <DirectEvents>
                                            <Click OnEvent="btnTelefones_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdEmpresas}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:Button ID="btnChartOrganization" runat="server" Text="Gráfico Organizacional" Icon="ChartOrganisation" Disabled="true">
										<Listeners>
											<Click Fn="fnOpenChartOrganization" />
										</Listeners>
									</ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Nome" DataIndex="Nome" Groupable="false" />
                                <ext:Column Header="Email" DataIndex="Email" Groupable="false" />
                                <ext:Column Header="Endereço" DataIndex="Endereco" Groupable="false" />
                                <ext:Column Header="Site" DataIndex="Site" Groupable="false" />
                                <ext:Column Header="Tipo de Empresa" DataIndex="TipoEmpresa" Hideable="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server">
                                <DirectEvents>
                                    <BeforeRowSelect OnEvent="grdEmpresas_SelectedRow">
                                    </BeforeRowSelect>
                                </DirectEvents>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false"
                                EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarEmpresas').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdEmpresas}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strEmpresas" AnchorHorizontal="100%">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdEmpresas}.getView().toggleAllGroups();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winEmpresa" Width="450" Height="270" Modal="true"
        Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmEmpresa" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:ComboBox runat="server" ID="cboTipoEmpresa" AllowBlank="false" MsgTarget="Side"
                                    Editable="true" EmptyText="Selecione o Tipo da Empresa..." AnchorHorizontal="92%"
                                    FieldLabel="Tipo de Empresa" DisplayField="Descricao" ValueField="Id" StoreID="strTiposEmpresa">
                                </ext:ComboBox>
                                <ext:TextField runat="server" ID="txtEndereco" MaxLength="200" AnchorHorizontal="92%"
                                    FieldLabel="Endereço">
                                </ext:TextField>
                                <ext:TextField runat="server" ID="txtCidade" MaxLength="100" AnchorHorizontal="92%"
                                    FieldLabel="Cidade">
                                </ext:TextField>
                                <ext:ComboBox runat="server" ID="cboEstado" Editable="true" EmptyText="Selecione o Estado..."
                                    Width="200" FieldLabel="Estado" DisplayField="Sigla" ValueField="Id" StoreID="strEstados">
                                </ext:ComboBox>
                                <ext:TextField runat="server" ID="txtEmail" MaxLength="100" AnchorHorizontal="92%"
                                    FieldLabel="Email">
                                </ext:TextField>
                                <ext:TextField runat="server" ID="txtSite" MaxLength="100" AnchorHorizontal="92%"
                                    FieldLabel="Site">
                                </ext:TextField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmEmpresa}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <DirectEvents>
                    <Click OnEvent="Cancelar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winTelefones" Width="350" Height="300" Modal="true"
        Title="Telefones da Empresa" Hidden="true" Maximizable="true">
        <Items>
            <ext:FitLayout runat="server">
                <Items>
                    <ext:GridPanel runat="server" ID="grdTelefones" StoreID="strTelefones" AutoExpandColumn="Telefone">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button ID="btnAddTelefone" runat="server" Text="Adicionar" Icon="PhoneAdd">
                                        <Listeners>
                                            <Click Fn="addTelefone" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="btnRemoveTelefone" runat="server" Text="Remover" Icon="PhoneDelete"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Fn="removeTelefone" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:RowNumbererColumn />
                                <ext:Column Header="Telefone" DataIndex="Telefone">
                                    <Editor>
                                        <ext:TextField ID="txtTelefone" runat="server" AllowBlank="false" MaxLength="14">
                                            <%-- <CustomConfig>
                                                <ext:ConfigItem Name="plugins" Value="new Ext.ux.InputTextMask('(99) 9999-9999', true)" Mode="Raw"></ext:ConfigItem>
                                            </CustomConfig>--%>
                                        </ext:TextField>
                                    </Editor>
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <Plugins>
                            <ext:RowEditor runat="server" SaveText="Update" ErrorText="Erros">
                                <Listeners>
                                    <CancelEdit Fn="function(cmp, valido){ cancelTelefone(cmp, valido, 'btnCancel');}" />
                                    <AfterEdit Fn="salvarTelefone" />
                                    <BeforeEdit Fn="function(){desabilitarBotoesTelefone(true);}" />
                                </Listeners>
                            </ext:RowEditor>
                        </Plugins>
                        <View>
                            <ext:GridView runat="server" />
                        </View>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <Listeners>
                            <RowClick Fn="telefoneRowSelect" />
                        </Listeners>
                    </ext:GridPanel>
                </Items>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button ID="btnSalvarTelefones" runat="server" Text="Salvar" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="SalvarTelefones_Click">
                        <ExtraParams>
                            <ext:Parameter Name="telefones" Value="Ext.encode(#{grdTelefones}.getRowsValues())"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winTelefones}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarEmpresas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarEmpresas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarEmpresas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverEmpresas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarTelefonesEmpresas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarChartEmpresas" Text="0"/>
	
</asp:Content>
