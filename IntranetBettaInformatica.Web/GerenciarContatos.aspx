<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarContatos.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarContatos" Title="Contatos" %>

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
        function abrirDropDown() {
            Ext.getCmp('ctl00_cph_body_ddfSetor').collapse();
            Ext.getCmp('ctl00_cph_body_treeSetores').initChildren(Ext.getCmp('ctl00_cph_body_treeSetores').getRootNode());
        }
        function selecionarSetor() {
            var ddl = Ext.getCmp('ctl00_cph_body_ddfSetor');
            var treeGrid = Ext.getCmp('ctl00_cph_body_treeSetores');
            var hdfSetor = document.getElementById('ctl00_cph_body_hdfSetor');
            hdfSetor.value = treeGrid.getSelectedNodes().attributes.Id;
            ddl.setValue(treeGrid.getSelectedNodes().attributes.Nome);
        }
        function cancelTelefone(cmp, valido, btn) {
            if (!valido) {
                var grid = Ext.getCmp('ctl00_cph_body_grdTelefones');
                var length = grid.getView().getRows().length - 1;
                alert('Erro', 'Telefone inválido.');
                grid.getRowEditor().startEditing(length);
            }
            else {

                Ext.getCmp('ctl00_cph_body_btnAddTelefone').setDisabled(false);
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
        }
        function validarContato() {
            var frm = Ext.getCmp('ctl00_cph_body_frmContato');
            var retorno = frm.validate();
            if (!retorno) {
                Ext.getCmp('ctl00_cph_body_tabContato').setActiveTab(0);
                alert('Erro', 'Existem campos obrigatórios para preenchimento.');
            }
            return retorno;
        }
        function selectRow(row) {
        	var selModel = Ext.getCmp('ctl00_cph_body_grdContatos').selModel;
        	var hdfEditarContatosValue = Ext.getCmp('ctl00_cph_body_hdfEditarContatos').getValue();
        	var hdfRemoverContatosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverContatos').getValue();
            var length = selModel && selModel.selections.length;
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
            btnRemover.setDisabled(length == 0 || hdfEditarContatosValue == '0');
            btnEditar.setDisabled(length == 0 || hdfRemoverContatosValue == '0');
        }
        function renderSetor(value, p, record) {
            return value ? value : !value && record.data.Empresa ? '[Nenhum]' : '';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strContatos" runat="server" OnRefreshData="OnRefreshData" GroupField="TipoPessoa">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Endereco" />
                    <ext:RecordField Name="Cidade" />
                    <ext:RecordField Name="Telefones" />
                    <ext:RecordField Name="Estado" ServerMapping="Estado.Sigla" />
                    <ext:RecordField Name="Email" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                    <ext:RecordField Name="TipoPessoa" />
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
    <ext:Store ID="strEmpresas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strEmpresasBusca" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
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
    <ext:Store ID="strTiposEmpresa" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strTiposEmpresaBusca" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%" Layout="Border">
                <Items>
                    <ext:FormPanel ID="frmBusca" runat="server" Region="North" Title="Busca" Collapsible="true"
                        ButtonAlign="Left" Collapsed="false" Frame="true" Height="160">
                        <Items>
                            <ext:TextField ID="txtNomeBusca" MaxLengthText="200" FieldLabel="Nome" runat="server" AnchorHorizontal="50%"></ext:TextField>
                            <ext:RadioGroup runat="server" Width="450" FieldLabel="Tipo Pessoa" GroupName="tipoPessoaBusca">
                                <Items>
                                    <ext:Radio runat="server" ID="rdbFisicaBusca" BoxLabel="Física" HideLabel="true" Width="100"></ext:Radio>
                                    <ext:Radio runat="server" ID="rdbJuridicaBusca" BoxLabel="Jurídica" HideLabel="true" Width="100"></ext:Radio>
                                    <ext:Radio runat="server" ID="rdbTodasBusca" Checked="true" BoxLabel="Todas" HideLabel="true" Width="100"></ext:Radio>
                                </Items>
                                <DirectEvents>
                                    <Change OnEvent="rdbTipoPessoaBusca_Click"></Change>    
                                </DirectEvents>
                            </ext:RadioGroup>
                            <ext:ComboBox runat="server" ID="cboEmpresaBusca" Editable="true" Hidden="true" HideMode="Display"
                                AnchorHorizontal="50%" FieldLabel="Empresa" DisplayField="Nome" ValueField="Id"
                                StoreID="strEmpresasBusca" MsgTarget="Side">
                            </ext:ComboBox>
                           <ext:ComboBox runat="server" ID="cboTipoEmpresaBusca" Editable="true" Hidden="true" HideMode="Display"
                                AnchorHorizontal="50%" FieldLabel="Tipo de Empresa" AllowBlank="true"
                                DisplayField="Descricao" ValueField="Id" StoreID="strTiposEmpresaBusca" MsgTarget="Side">
                           </ext:ComboBox>
                        </Items>
                        <Buttons>
                            <ext:Button ID="btnBuscar" Text="Buscar" Icon="Zoom" runat="server" StyleSpec="padding-left:98px;">
                                <Listeners>
                                    <Click Handler="return #{frmBusca}.validate();" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="btnBuscar_Click">
                                        <EventMask Msg="Buscando Contatos..." Target="Page" ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                        <KeyMap>
                            <ext:KeyBinding>
                                <Keys>
                                    <ext:Key Code="ENTER" />
                                </Keys>
                                <Listeners>
                                    <Event Handler="#{btnBuscar}.fireEvent('click');" />
                                </Listeners>
                            </ext:KeyBinding>
                        </KeyMap>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdContatos" runat="server" StoreID="strContatos"
                        Frame="true" Region="Center"  AutoExpandColumn="colNome" AnchorHorizontal="100%"
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                    <ext:Parameter Name="tipoPessoa" Mode="Raw" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].TipoPessoa">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                    <ext:Parameter Name="tipoPessoa" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].TipoPessoa"
                                                        Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este contato?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" />
                                <ext:Column Header="Endereço" DataIndex="Endereco" Width="90" Groupable="false" />
                                <ext:Column Header="Telefones" DataIndex="Telefones" Width="90" Groupable="false" />
                                <ext:Column Header="Email" DataIndex="Email" Width="80" Groupable="false" />
                                <ext:Column Header="Cidade" DataIndex="Cidade" Width="90" Groupable="false" />
                                <ext:Column Header="Estado" DataIndex="Estado" Width="65" Groupable="false" Hidden="true" />
                                <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" Groupable="false" />
                                <ext:Column Header="Setor" DataIndex="Setor" Width="90" Groupable="false" Hidden="true">
                                    <Renderer Fn="renderSetor" />
                                </ext:Column>
                                <ext:Column Header="Tipo" DataIndex="TipoPessoa" Hideable="false" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarContatos').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                    <ext:Parameter Name="tipoPessoa" Mode="Raw" Value="#{grdContatos}.getRowsValues({selectedOnly:true})[0].TipoPessoa">
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
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strContatos" AnchorHorizontal="100%">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winContato" Width="500" Height="370" Modal="true"
        Maximizable="false" Resizable="false" Hidden="true" Icon="Group">
        <Items>
            <ext:FitLayout ID="FitLayout2" runat="server">
                <Items>
                    <ext:TabPanel runat="server" ID="tabContato">
                        <Items>
                            <ext:FormPanel runat="server" Frame="true" ID="frmContato" AnchorVertical="100%"
                                Title="Dados do Contato" Border="false" AutoScroll="true" LabelWidth="130">
                                <Items>
                                    <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false" AnchorHorizontal="92%"
                                        FieldLabel="Nome" MsgTarget="Side">
                                    </ext:TextField>
                                    <ext:RadioGroup runat="server" ID="rdbGroupTipoContato" ColumnsNumber="2" GroupName="tipopessoa"
                                        FieldLabel="Tipo de Pessoa" AnchorHorizontal="92%" Vertical="false">
                                        <Items>
                                            <ext:Radio runat="server" ID="rdbFisica" Checked="true" BoxLabel="Física" HideLabel="true"
                                                OnDirectCheck="rdbCheck_Click">
                                            </ext:Radio>
                                            <ext:Radio runat="server" ID="rdbJuridica" BoxLabel="Jurídica" HideLabel="true" OnDirectCheck="rdbCheck_Click">
                                            </ext:Radio>
                                        </Items>
                                    </ext:RadioGroup>
                                    <ext:ComboBox runat="server" ID="cboEmpresa" Editable="true" EmptyText="Selecione a Empresa..."
                                        AnchorHorizontal="92%" FieldLabel="Empresa" DisplayField="Nome" ValueField="Id"
                                        StoreID="strEmpresas" MsgTarget="Side">
                                        <DirectEvents>
                                            <Select OnEvent="cboEmpresa_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Select>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                    <ext:Panel ID="pnlDropField" runat="server" Frame="false" Border="false" AutoRender="false"
                                        AnchorHorizontal="100%">
                                        <Items>
                                            <ext:DropDownField runat="server" ID="ddfSetor" FieldLabel="Setor" Width="435" Editable="false"
                                                TriggerIcon="SimpleArrowDown">
                                                <Component>
                                                    <ext:TreeGrid runat="server" ID="treeSetores" Height="300" Width="340" Shadow="None"
                                                        UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" AutoRender="false"
                                                        ContainerScroll="true" RootVisible="false" Icon="ApplicationXp" AutoExpandColumn="Nome">
                                                        <Columns>
                                                            <ext:TreeGridColumn DataIndex="Nome" Header="Nome" Width="200">
                                                            </ext:TreeGridColumn>
                                                        </Columns>
                                                        <Buttons>
                                                            <ext:Button ID="Button1" runat="server" Text="Fechar">
                                                                <Listeners>
                                                                    <Click Handler="abrirDropDown();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Buttons>
                                                        <SelectionModel>
                                                            <ext:DefaultSelectionModel ID="DefaultSelectionModel1" runat="server">
                                                                <Listeners>
                                                                    <SelectionChange Fn="selecionarSetor" />
                                                                </Listeners>
                                                            </ext:DefaultSelectionModel>
                                                        </SelectionModel>
                                                    </ext:TreeGrid>
                                                </Component>
                                                <Listeners>
                                                    <Expand Handler="this.component.getRootNode().expand(true);" Single="true" Delay="10" />
                                                </Listeners>
                                            </ext:DropDownField>
                                        </Items>
                                    </ext:Panel>
                                    <ext:ComboBox runat="server" ID="cboTipoEmpresa" Editable="true" EmptyText="Selecione o Tipo..."
                                        Disabled="true" AnchorHorizontal="92%" FieldLabel="Tipo de Empresa" AllowBlank="true"
                                        DisplayField="Descricao" ValueField="Id" StoreID="strTiposEmpresa" MsgTarget="Side">
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
                                        Vtype="email" MsgTarget="Side" FieldLabel="Email">
                                    </ext:TextField>
                                </Items>
                            </ext:FormPanel>
                            <ext:Panel ID="tabTelefones" runat="server" Title="Telefones" Border="false" AutoScroll="true">
                                <Items>
                                    <ext:FitLayout ID="FitLayout1" runat="server">
                                        <Items>
                                            <ext:GridPanel runat="server" ID="grdTelefones" StoreID="strTelefones" AutoExpandColumn="Telefone">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
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
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:RowNumbererColumn />
                                                        <ext:Column Header="Telefone" DataIndex="Telefone" MenuDisabled="true" Sortable="false"
                                                            Groupable="false" Hideable="false">
                                                            <Editor>
                                                                <ext:TextField ID="txtTelefone" runat="server" AllowBlank="false" MaxLength="14">
                                                                </ext:TextField>
                                                            </Editor>
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <Plugins>
                                                    <ext:RowEditor ID="RowEditor1" runat="server" SaveText="Update" ErrorText="Erros">
                                                        <Listeners>
                                                            <CancelEdit Fn="function(cmp, valido){ cancelTelefone(cmp, valido, 'btnCancel');}" />
                                                            <AfterEdit Fn="salvarTelefone" />
                                                            <BeforeEdit Fn="function(){desabilitarBotoesTelefone(true);}" />
                                                        </Listeners>
                                                    </ext:RowEditor>
                                                </Plugins>
                                                <View>
                                                    <ext:GridView ID="GridView1" runat="server" />
                                                </View>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true" />
                                                </SelectionModel>
                                                <Listeners>
                                                    <RowClick Fn="telefoneRowSelect" />
                                                </Listeners>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:FitLayout>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                </Items>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Fn="validarContato" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="tipoPessoa" Value="#{rdbFisica}.checked ? 'Física' : 'Jurídica'"
                                Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="telefones" Value="Ext.encode(#{grdTelefones}.getRowsValues())"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="Button3" runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winContato}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Hidden runat="server" ID="hdfSetor">
    </ext:Hidden>
	<ext:Hidden runat="server" ID="hdfVisualizarContatos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarContatos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarContatos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverContatos" Text="0"/>
</asp:Content>
