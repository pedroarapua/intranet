<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarEnsalamento.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarEnsalamento" Title="Ensalamento" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function refreshTree(a, b, c) {
            var nodes = eval(b.extraParamsResponse.nodes);
            if (nodes.length > 0) {
                Ext.getCmp('ctl00_cph_body_treeSetores').initChildren(nodes);
            }
            else {
                Ext.getCmp('ctl00_cph_body_treeSetores').getRootNode().removeChildren();
            }
        }
        function abrirDropDown() {
            Ext.getCmp('ctl00_cph_body_ddfSetor').collapse();
            Ext.getCmp('ctl00_cph_body_treeSetoresWin').initChildren(Ext.getCmp('ctl00_cph_body_treeSetoresWin').getRootNode());
        }
        function selecionarSetor() {
            var ddl = Ext.getCmp('ctl00_cph_body_ddfSetor');
            var treeGrid = Ext.getCmp('ctl00_cph_body_treeSetoresWin');
            var hdfSetor = Ext.getCmp('ctl00_cph_body_hdfSetor');
            if (treeGrid.getSelectedNodes().attributes.Empresa == '1') {
                alert('Alerta', 'Selecione um Setor.');
                return;
            }
            hdfSetor.value = treeGrid.getSelectedNodes().attributes.Id;
            ddl.setValue(treeGrid.getSelectedNodes().attributes.Nome);
        }
        function selectRow(row) {
        	var length = Ext.getCmp('ctl00_cph_body_grdSalas').selModel.selections.length;
        	var hdfEditarEnsalamentoValue = Ext.getCmp('ctl00_cph_body_hdfEditarEnsalamento').getValue();
        	var hdfRemoverEnsalamentoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverEnsalamento').getValue();
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
            btnRemover.setDisabled(length == 0 || hdfRemoverEnsalamentoValue == '0');
            btnEditar.setDisabled(length == 0 || hdfEditarEnsalamentoValue == '0');
        }
        function renderSetor(value, p, record) {
            if (value == null)
                return 'Nenhum';
            return value;
        }
    </script>
    <style type="text/css">
        .icon-combo-item
        {
            background-repeat: no-repeat !important;
            background-position: 3px 50% !important;
            padding-left: 24px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strSalas" runat="server" OnRefreshData="OnRefreshData" GroupField="Setor">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Empresa" ServerMapping="Setor.Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.NomeRemovido" />
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
                    <ext:GridPanel ID="grdSalas" runat="server" StoreID="strSalas" Frame="true"
                        AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdSalas}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdSalas}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta sala?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                                <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                                <ext:Column Header="Setor" DataIndex="Setor" Width="90">
                                    <Renderer Fn="renderSetor" />
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
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false"
                                EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarEnsalamento').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdSalas}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strSalas" AnchorHorizontal="100%">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdSalas}.getView().toggleAllGroups();" />
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
    <ext:Window runat="server" ID="winSala" Icon="ApplicationSplit" Width="450" Height="200" Modal="true"
        Maximizable="true" CloseAction="Hide" Hidden="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmSala" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:Panel ID="pnlDropField" runat="server" Frame="false" Border="false" AutoRender="false"
                                    AnchorHorizontal="100%">
                                    <Items>
                                        <ext:DropDownField runat="server" ID="ddfSetor" FieldLabel="Setor" Width="390"
                                            Editable="false" TriggerIcon="SimpleArrowDown">
                                            <Component>
                                                <ext:TreeGrid runat="server" ID="treeSetoresWin" Height="300" Width="550" Shadow="None"
                                                    UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true" AutoRender="false"
                                                    ContainerScroll="true" RootVisible="false" AutoExpandColumn="Nome">
                                                    <Columns>
                                                        <ext:TreeGridColumn DataIndex="Nome" Header="Nome" Width="200">
                                                        </ext:TreeGridColumn>
                                                    </Columns>
                                                    <Buttons>
                                                        <ext:Button runat="server" Text="Fechar">
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
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmSala}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="setor" Mode="Raw" Value="Ext.getCmp('ctl00_cph_body_hdfSetor').value">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winSala}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Hidden runat="server" ID="hdfSetor">
    </ext:Hidden>
	<ext:Hidden runat="server" ID="hdfVisualizarEnsalamento" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarEnsalamento" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarEnsalamento" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverEnsalamento" Text="0"/>
</asp:Content>
