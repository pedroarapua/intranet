<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarUsuarios.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarUsuarios" Title="Usuários" %>

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
        function checkUsuarioSistema(chk, check) {
            var cboPerfilAcesso = Ext.getCmp('ctl00_cph_body_cboPerfilAcesso');
            var txtLogin = Ext.getCmp('ctl00_cph_body_txtLogin');
            var txtSenha = Ext.getCmp('ctl00_cph_body_txtSenha');
            var txtConfSenha = Ext.getCmp('ctl00_cph_body_txtConfSenha');
            var txtPalavraChave = Ext.getCmp('ctl00_cph_body_txtPalavraChave');
            var txtTwitter = Ext.getCmp('ctl00_cph_body_txtTwitter');
            var hdfAcaoTela = document.getElementById('ctl00_cph_body_hdfAcaoTela');

            cboPerfilAcesso.setDisabled(!chk.checked);
            txtLogin.setDisabled(!chk.checked);
            txtSenha.setDisabled(!chk.checked);
            txtConfSenha.setDisabled(!chk.checked);
            txtPalavraChave.setDisabled(!chk.checked);
            txtTwitter.setDisabled(!chk.checked);

            cboPerfilAcesso.allowBlank = !chk.checked;
            txtLogin.allowBlank = !chk.checked;
            txtSenha.allowBlank = !(chk.checked && hdfAcaoTela.value == '0');
            txtConfSenha.allowBlank = !(chk.checked && hdfAcaoTela.value == '0');
            txtPalavraChave.allowBlank = !(chk.checked && hdfAcaoTela.value == '0');

            cboPerfilAcesso.clearInvalid();
            txtLogin.clearInvalid();
            txtSenha.clearInvalid();
            txtConfSenha.clearInvalid();
            txtPalavraChave.clearInvalid();
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
        function validarUsuario() {
            var frm = Ext.getCmp('ctl00_cph_body_frmUsuario');
            var txtSenha = Ext.getCmp('ctl00_cph_body_txtSenha');
            var txtConfSenha = Ext.getCmp('ctl00_cph_body_txtConfSenha');
            if (txtSenha.getValue() != txtConfSenha.getValue()) {
                alert('Erro', 'Senha e confirmação de senha não conferem.');
                return false;
            }
            return frm.validate();
        }
        function limparSelecaoGrid() {
			var grdSistemas = Ext.getCmp('ctl00_cph_body_grdSistemas');
			if (grdSistemas.getSelectionModel().selections.length > 0)
				grdSistemas.getSelectionModel().clearSelections();
        }
        function selectRow(row) {
        	var hdfEditarUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfEditarUsuarios').getValue();
        	var hdfRemoverUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverUsuarios').getValue();
        	var hdfAdicionarTelefonesUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfAdicionarTelefonesUsuarios').getValue();
        	var length = Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.length;
        	var data = length > 0 ? Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.items[0].data : null;
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			var btnTelefones = Ext.getCmp('ctl00_cph_body_btnTelefones');
			btnRemover.setDisabled(length == 0 || hdfRemoverUsuariosValue == '0' || data.Id == 1);
			btnEditar.setDisabled(length == 0 || hdfEditarUsuariosValue == '0');
			btnTelefones.setDisabled(length == 0 || hdfAdicionarTelefonesUsuariosValue == '0');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <script src="Js/InputTextMask.js" type="text/javascript"></script>
    <ext:Store ID="strUsuarios" runat="server" OnRefreshData="OnRefreshData" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Login" />
                    <ext:RecordField Name="DataNascimento" Type="Date" />
                    <ext:RecordField Name="Cidade" />
                    <ext:RecordField Name="Estado" ServerMapping="Estado.Sigla" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                    <ext:RecordField Name="PerfilAcesso" ServerMapping="PerfilAcesso.Nome" />
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
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strTemas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strPerfisAcesso" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
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
    <ext:Store ID="strSistemas" runat="server" OnRefreshData="OnRefreshDataSistemas">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
	<ext:Store ID="strFuncoes" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
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
                    <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios" Frame="true"
                        AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Novo" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                        <Listeners>
                                            <Click Handler="document.getElementById('ctl00_cph_body_hdfAcaoTela').value = '0';" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditar_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                        <Listeners>
                                            <Click Handler="document.getElementById('ctl00_cph_body_hdfAcaoTela').value = '1';" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover"  Disabled="true" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este usuario?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnTelefones" Text="Adicionar Telefones" Icon="Phone" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnTelefones_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
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
                                <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                                <ext:Column Header="Login" DataIndex="Login" Groupable="false" Width="60" />
                                <ext:Column Header="Data de Nasc." DataIndex="DataNascimento" Width="60">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y')" />
                                </ext:Column>
                                <ext:Column Header="Cidade" DataIndex="Cidade" Width="90" />
                                <ext:Column Header="Estado" DataIndex="Estado" Width="45" />
                                <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                                <ext:Column Header="Setor" DataIndex="Setor" Width="90" />
                                <ext:Column Header="Perfil de Acesso" DataIndex="PerfilAcesso" Width="90" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server">
                                <Listeners>
									<SelectionChange Fn="selectRow" />
							   </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false"
                                EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarUsuarios').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strUsuarios" AnchorHorizontal="100%">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdUsuarios}.getView().toggleAllGroups();" />
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
    <ext:Window runat="server" ID="winUsuario" Width="600" Height="610" Modal="true"
        Maximizable="false" Resizable="false" Hidden="true">
        <Items>
            <ext:FitLayout runat="server">
                <Items>
                    <ext:TabPanel runat="server" ID="tabUsuario" ActiveTabIndex="0" Border="false">
                        <Items>
                            <ext:Panel runat="server" Title="Dados do Usuário" Border="false" Layout="fit">
                                <Items>
                                    <ext:FormPanel runat="server" Frame="true" ID="frmUsuario" AnchorVertical="100%"
                                        AutoScroll="true" LabelWidth="130">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false" AnchorHorizontal="92%"
                                                FieldLabel="Nome" MsgTarget="Side">
                                            </ext:TextField>
                                            <ext:ComboBox runat="server" ID="cboEmpresa" Editable="true" EmptyText="Selecione a Empresa..."
                                                AnchorHorizontal="92%" FieldLabel="Empresa" DisplayField="Nome" ValueField="Id"
                                                AllowBlank="false" StoreID="strEmpresas" MsgTarget="Side">
                                                <DirectEvents>
                                                    <Select OnEvent="cboEmpresa_Click">
                                                        <EventMask ShowMask="true" Target="Page" />
                                                    </Select>
                                                </DirectEvents>
                                            </ext:ComboBox>
                                            <ext:Panel ID="pnlDropField" runat="server" Frame="false" Border="false" AutoRender="false"
                                                AnchorHorizontal="100%">
                                                <Items>
                                                    <ext:DropDownField runat="server" ID="ddfSetor" FieldLabel="Setor" Width="530" Editable="false"
                                                        TriggerIcon="SimpleArrowDown">
                                                        <Component>
                                                            <ext:TreeGrid runat="server" ID="treeSetores" Height="300" Width="550" Shadow="None"
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
                                                                    <ext:DefaultSelectionModel runat="server">
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
											<ext:ComboBox runat="server" ID="cboFuncao" Editable="true" EmptyText="Selecione a Função..."
                                                AnchorHorizontal="92%" FieldLabel="Função" DisplayField="Nome" ValueField="Id" StoreID="strFuncoes" MsgTarget="Side">
                                            </ext:ComboBox>
                                            <ext:DateField runat="server" ID="txtDataNascimento" Width="200" FieldLabel="Data de Nasc."
                                                Format="dd/MM/yyyy">
                                            </ext:DateField>
                                            <ext:TextField runat="server" ID="txtEndereco" MaxLength="200" AnchorHorizontal="92%"
                                                FieldLabel="Endereço">
                                            </ext:TextField>
                                            <ext:TextField runat="server" ID="txtCidade" MaxLength="100" AnchorHorizontal="92%"
                                                FieldLabel="Cidade">
                                            </ext:TextField>
                                            <ext:ComboBox runat="server" ID="cboEstado" Editable="true" EmptyText="Selecione o Estado..."
                                                Width="200" FieldLabel="Estado" DisplayField="Sigla" ValueField="Id" StoreID="strEstados">
                                            </ext:ComboBox>
                                            <ext:TextField runat="server" ID="txtEmail" MaxLength="100" AnchorHorizontal="92%" AllowBlank="false" MsgTarget="Side"
                                                FieldLabel="Email" Vtype="email">
                                            </ext:TextField>
                                            <ext:ComboBox runat="server" ID="cboTema" Editable="true" EmptyText="Selecione o Tema..."
                                                AnchorHorizontal="50%" FieldLabel="Tema" DisplayField="Nome" ValueField="Id"
                                                AllowBlank="false" StoreID="strTemas" MsgTarget="Side">
                                            </ext:ComboBox>
                                            <ext:Checkbox runat="server" ID="chkUsuarioSistema" FieldLabel="É usuário do sistema"
                                                LabelSeparator="?">
                                                <Listeners>
                                                    <Check Fn="checkUsuarioSistema" />
                                                </Listeners>
                                            </ext:Checkbox>
                                            <ext:TextField runat="server" ID="txtTwitter" MaxLength="50" AnchorHorizontal="92%" FieldLabel="Twitter"></ext:TextField>
                                            <ext:FieldSet runat="server" Title="Dados de Acesso" AnchorHorizontal="98%" LabelWidth="120">
                                                <Items>
                                                    <ext:ComboBox runat="server" ID="cboPerfilAcesso" Editable="true" EmptyText="Selecione o Perfil de Acesso..."
                                                        Width="500" FieldLabel="Perfil de Acesso" DisplayField="Nome" ValueField="Id"
                                                        StoreID="strPerfisAcesso" MsgTarget="Side">
                                                    </ext:ComboBox>
                                                    <ext:TextField runat="server" ID="txtLogin" MaxLength="100" Width="300" FieldLabel="Login"
                                                        AllowBlank="false" MsgTarget="Side">
                                                    </ext:TextField>
                                                    <ext:TextField runat="server" ID="txtSenha" MaxLength="20" Width="300" FieldLabel="Senha"
                                                        AllowBlank="false" MsgTarget="Side" InputType="Password">
                                                    </ext:TextField>
                                                    <ext:TextField runat="server" ID="txtConfSenha" MaxLength="20" Width="300" FieldLabel="Conf. de Senha"
                                                        AllowBlank="false" MsgTarget="Side" InputType="Password">
                                                    </ext:TextField>
                                                    <ext:TextField runat="server" ID="txtPalavraChave" MaxLength="20" Width="300" FieldLabel="Palavra Chave"
                                                        AllowBlank="false" MsgTarget="Side" InputType="Password">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:FieldSet>
                                        </Items>
                                    </ext:FormPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="tabSistemas" runat="server" Title="Associar Sistemas" Border="false"
                                AutoRender="false">
                                <Items>
                                    <ext:FitLayout runat="server">
                                        <Items>
                                            <ext:GridPanel ID="grdSistemas" runat="server" StoreID="strSistemas" StripeRows="true"
                                                Title="Sistemas" AutoExpandColumn="Nome">
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="Nome" Header="Sistema" DataIndex="Nome" />
                                                    </Columns>
                                                </ColumnModel>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server" PageSize="15" StoreID="strSistemas" DisplayInfo="false" />
                                                </BottomBar>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel runat="server" />
                                                </SelectionModel>
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
                    <Click Fn="validarUsuario" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="sistemas" Value="Ext.encode(#{grdSistemas}.getRowsValues({selectedOnly:true}))"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winUsuario}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
        <Listeners>
            <Hide Handler="limparSelecaoGrid();" />
        </Listeners>
    </ext:Window>
    <ext:Window runat="server" ID="winTelefones" Width="350" Height="300" Modal="true"
        Title="Telefones do Usuário" Hidden="true" Maximizable="false">
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
                                <ext:Column Header="Telefone" DataIndex="Telefone" MenuDisabled="true" Sortable="false"
                                    Groupable="false" Hideable="false">
                                    <Editor>
                                        <ext:TextField ID="txtTelefone" runat="server" AllowBlank="false" MaxLength="14">
                                            <%-- <CustomConfig>
                                                <ext:ConfigItem Name="plugins" Value="new Ext.ux.InputTextMask('(99) 9999-9999', true)"
                                                    Mode="Raw">
                                                </ext:ConfigItem>
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
    <ext:Hidden runat="server" ID="hdfSetor">
    </ext:Hidden>
    <asp:HiddenField runat="server" ID="hdfAcaoTela"></asp:HiddenField>
	<ext:Hidden runat="server" ID="hdfVisualizarUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarTelefonesUsuarios" Text="0"/>
</asp:Content>
