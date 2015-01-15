<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarMensagens.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarMensagens" Title="Mensagens" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <style>
        .rowLido
        {
            background-color: Gray;
        }
    </style>
    <script type="text/javascript">
        function getMensagensSelecionadas() {
            var array = Ext.getCmp('ctl00_cph_body_grdMensagens').getRowsValues({ selectedOnly: true });
            var args = [];
            for (var i = 0; i < array.length; i++)
                args[i] = { Id: array[i].Id };
            return args;
        }
        function selectRow(row) {
        	var hdfEditarMensagensValue = Ext.getCmp('ctl00_cph_body_hdfEditarMensagens').getValue();
        	var hdfRemoverMensagensValue = Ext.getCmp('ctl00_cph_body_hdfRemoverMensagens').getValue();
        	var hdfMarcarComoLidoMensagensValue = Ext.getCmp('ctl00_cph_body_hdfMarcarComoLidoMensagens').getValue();
            var data = !row ? null : row.selections.items.length == 0 ? null : row.selections.items[0].data;
            var disabled = !row || !data;
            if (data && row) {
                var items = row.selections.items;
                var cont = items.length;
                var contLido = 0;
                for (var i = 0; i < cont; i++) {
                    data = items[i].data;
                    if (data.MensagemEnviada || data.MensagemLida) {
                        contLido = 1;
                        break;
                    }
                }
				Ext.getCmp('ctl00_cph_body_btnMarcarLido').setDisabled(contLido > 0 || hdfMarcarComoLidoMensagensValue == '0');
				Ext.getCmp('ctl00_cph_body_btnEditar').setDisabled(cont > 1 || !items[0].data.MensagemEnviada || hdfEditarMensagensValue == '0');
				Ext.getCmp('ctl00_cph_body_btnRemover').setDisabled(cont > 1 || hdfRemoverMensagensValue == '0');
            }
            else {
            	Ext.getCmp('ctl00_cph_body_btnEditar').setDisabled(disabled || !data.MensagemEnviada || hdfEditarMensagensValue == '0');
            	Ext.getCmp('ctl00_cph_body_btnMarcarLido').setDisabled(disabled || data.MensagemEnviada || data.MensagemLida || hdfMarcarComoLidoMensagensValue == '0');
            	Ext.getCmp('ctl00_cph_body_btnRemover').setDisabled(disabled || hdfRemoverMensagensValue == '0');
            }
        }
        function removerUsuarios() {
            Ext.getCmp('ctl00_cph_body_grdUsuarios').deleteSelected();
            rowSelectUsuario(null);
        }
        function rowSelectUsuario(row) {
            var data = !row ? null : row.selections.items.length == 0 ? null : row.selections.items[0].data;
            var disabled = !row || !data;
            Ext.getCmp('ctl00_cph_body_btnRemoverUsuarios').setDisabled(disabled);
        }
        function validarMensagem() {
            var frmMensagem = Ext.getCmp('ctl00_cph_body_frmMensagem');
            var tabMensagem = Ext.getCmp('ctl00_cph_body_tabMensagem');
            var usuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios').getRowsValues();
            if (!frmMensagem.validate()) {
                tabMensagem.setActiveTab(0);
                return false;
            }
            else if (usuarios.length == 0) {
                alert('Usuários', 'Pelo menos 1 usuário deve ser associado a mensagem.');
                tabMensagem.setActiveTab(1);
                return false;
            }
            return true;
        }
        function confirmarUsuarios() {
            var win = Ext.getCmp('ctl00_cph_body_winAdicionarUsuarios');
            var grdUsuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios');
            var grdUsuariosPesq = Ext.getCmp('ctl00_cph_body_grdUsuariosPesquisa');
            var usuariosAdd = grdUsuariosPesq.getRowsValues({ selectedOnly: true });

            for (var i = 0; i < usuariosAdd.length; i++) {
                grdUsuarios.insertRecord(0, usuariosAdd[i]);
            }
            grdUsuarios.selModel.clearSelections();
            rowSelectUsuario(null);
            win.hide();
        }
        function getUsuarios(array) {
            var args = [];
            for (var i = 0; i < array.length; i++)
                args[i] = { Id: array[i].Id };
            return args;
        }
        function rowClass(record) {
            if (!record.data.MensagemEnviada && record.data.ConfirmarLeitura && !record.data.MensagemLida) {
                return 'rowLido';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strMensagens" runat="server" OnRefreshData="OnRefreshData" GroupField="TipoMensagem">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="Data" Type="Date" />
                    <ext:RecordField Name="TipoMensagem" />
                    <ext:RecordField Name="MensagemEnviada" />
                    <ext:RecordField Name="ConfirmarLeitura" />
                    <ext:RecordField Name="MensagemLida" />
                    <ext:RecordField Name="UsuarioEnvio" ServerMapping="UsuarioEnvio.Nome" />
                    <ext:RecordField Name="UsuarioId" ServerMapping="Usuario.Id" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strUsuarios" runat="server" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strUsuariosPesquisa" runat="server" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%"
                Layout="Border">
                <Items>
                    <ext:FormPanel runat="server" ID="frmBusca" Title="Busca" Collapsed="false" Border="false"
                        Height="130" Collapsible="true" Region="North" Frame="true" ButtonAlign="Left">
                        <Items>
                            <ext:RadioGroup runat="server" FieldLabel="Mensagens" Width="400" LabelSeparator="?"
                                GroupName="groupMensagem">
                                <Items>
                                    <ext:Radio runat="server" ID="rdbLido" HideLabel="true" BoxLabel="Lidas" Width="80">
                                    </ext:Radio>
                                    <ext:Radio runat="server" ID="rdbNaoLido" HideLabel="true" BoxLabel="Não Lidas" Width="110">
                                    </ext:Radio>
                                    <ext:Radio runat="server" ID="rdbTodos" Checked="true" HideLabel="true" BoxLabel="Todas"
                                        Width="100">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:Container runat="server" Height="50" Layout="Column" AnchorHorizontal="92%">
                                <Items>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataInicial" FieldLabel="Período" Editable="true"
                                                Vtype="daterange" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="#{txtDataFinal}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataFinal" Editable="true" Vtype="daterange"
                                                HideLabel="true" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="startDateField" Value="#{txtDataInicial}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.5">
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Buttons>
                            <ext:Button Text="Buscar" Icon="Zoom" runat="server" StyleSpec="padding-left:98px;">
                                <Listeners>
                                    <Click Handler="return #{frmBusca}.validate();" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="btnBuscar_Click">
                                        <EventMask Msg="Buscando Mensagens..." Target="Page" ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdMensagens" runat="server" StoreID="strMensagens"
                        Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%"
                        Layout="Fit" Region="Center">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdMensagens}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdMensagens}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                    <ext:Parameter Name="mensagemEnviada" Mode="Raw" Value="#{grdMensagens}.getRowsValues({selectedOnly:true})[0].MensagemEnviada">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta mensagem?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnMarcarLido" Text="Marcar c/ Lido" Disabled="true"
                                        Icon="EmailOpen">
                                        <DirectEvents>
                                            <Click OnEvent="btnMarcarLido_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="mensagens" Mode="Raw" Value="Ext.encode(getMensagensSelecionadas())">
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
                                <ext:Column Header="Mensagem" Width="300" DataIndex="Descricao" />
                                <ext:Column Header="Data" Width="150" DataIndex="Data">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Remetente" Width="200" DataIndex="UsuarioEnvio" />
                                <ext:Column Header="Mensagens" DataIndex="TipoMensagem" Hideable="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server">
                                <Listeners>
                                    <SelectionChange Fn="selectRow" />
                                </Listeners>
                            </ext:CheckboxSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false"
                                EnableRowBody="true">
                                <GetRowClass Fn="rowClass" />
                            </ext:GroupingView>
                        </View>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strMensagens">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdMensagens}.getView().toggleAllGroups();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Label runat="server" HideLabel="true" Text="&nbsp;&nbsp;&nbsp;&nbsp;" Cls="rowLido">
                                    </ext:Label>
                                    <ext:Label runat="server" HideLabel="true" Text="&nbsp;&nbsp;Mensagens não Lidas">
                                    </ext:Label>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winMensagem" Icon="Email" Width="690" Height="350"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:FitLayout runat="server">
                <Items>
                    <ext:TabPanel runat="server" ID="tabMensagem" ActiveTabIndex="0" Border="false">
                        <Items>
                            <ext:Panel runat="server" Title="Mensagem" Border="false" Layout="fit">
                                <Items>
                                    <ext:FormPanel runat="server" Frame="true" ID="frmMensagem" AnchorVertical="100%">
                                        <Items>
                                            <ext:HtmlEditor runat="server" ID="txtMensagem" AnchorHorizontal="100%" HideLabel="true"
                                                AnchorVertical="-40" Note="Máximo de 2000 caracteres no conteúdo HTML.">
                                            </ext:HtmlEditor>
                                            <ext:Checkbox runat="server" ID="chkConfirmarLeitura" FieldLabel="Confirmar Leitura"
                                                LabelSeparator="?">
                                            </ext:Checkbox>
                                        </Items>
                                    </ext:FormPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="tabUsuarios" runat="server" Title="Associar Usuários" Border="false"
                                AutoRender="false">
                                <Items>
                                    <ext:FitLayout runat="server">
                                        <Items>
                                            <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios" Frame="true"
                                                Layout="fit" AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
                                                <TopBar>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:Button runat="server" Text="Adicionar" Icon="Add">
                                                                <DirectEvents>
                                                                    <Click OnEvent="btnAdicionarUsuarios_Click">
                                                                        <EventMask ShowMask="true" Target="Page" />
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="btnRemoverUsuarios" runat="server" Text="Remover" Disabled="true"
                                                                Icon="Delete">
                                                                <Listeners>
                                                                    <Click Fn="removerUsuarios" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                                                        <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                                                        <ext:Column Header="Setor" DataIndex="Setor" Width="90" Hideable="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server" SingleSelect="false">
                                                        <Listeners>
                                                            <SelectionChange Fn="rowSelectUsuario" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <View>
                                                    <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="true"
                                                        EnableRowBody="true">
                                                    </ext:GroupingView>
                                                </View>
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
                    <Click Handler="return validarMensagem()" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <ExtraParams>
                            <ext:Parameter Name="usuarios" Value="Ext.encode(getUsuarios(#{grdUsuarios}.getRowsValues()))"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winMensagem}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winAdicionarUsuarios" Icon="User" Width="550" Height="400"
        Modal="true" Hidden="true" Maximizable="true" Title="Adicionando Usuários a Mensagem"
        Layout="fit">
        <Items>
            <ext:GridPanel ID="grdUsuariosPesquisa" runat="server" StoreID="strUsuariosPesquisa"
                Frame="true" StripeRows="true" AutoExpandColumn="colNome">
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                        <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                        <ext:Column Header="Setor" DataIndex="Setor" Width="90" Hideable="true" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:GridFilters runat="server" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="Nome" />
                            <ext:StringFilter DataIndex="Empresa" />
                            <ext:StringFilter DataIndex="Setor" />
                        </Filters>
                    </ext:GridFilters>
                </Plugins>
                <SelectionModel>
                    <ext:CheckboxSelectionModel runat="server" />
                </SelectionModel>
            </ext:GridPanel>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Confirmar" Icon="BulletTick">
                <Listeners>
                    <Click Fn="confirmarUsuarios" />
                </Listeners>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winAdicionarUsuarios}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarMensagens" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarMensagens" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarMensagens" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverMensagens" Text="0"/>
	<ext:Hidden runat="server" ID="hdfMarcarComoLidoMensagens" Text="0"/>
</asp:Content>
