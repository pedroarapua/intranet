<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarSetores.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarSetores" Title="Setores de Empresas" %>

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
            Ext.getCmp('ctl00_cph_body_ddfSetorPai').collapse();
            Ext.getCmp('ctl00_cph_body_treeSetoresWin').initChildren(Ext.getCmp('ctl00_cph_body_treeSetoresWin').getRootNode());
        }
        function selecionarSetorPai() {
            var ddl = Ext.getCmp('ctl00_cph_body_ddfSetorPai');
            var treeGrid = Ext.getCmp('ctl00_cph_body_treeSetoresWin');
            var hdfSetorPai = Ext.getCmp('ctl00_cph_body_hdfSetorPai');
            if (treeGrid.getSelectedNodes().attributes.Empresa == '1') {
                alert('Alerta', 'Selecione um Setor e não uma empresa.');
                return;
            }
            hdfSetorPai.value = treeGrid.getSelectedNodes().attributes.Id;
            ddl.setValue(treeGrid.getSelectedNodes().attributes.Nome);
        }
        function fnOpenChartOrganization() {
        	var node = Ext.getCmp('ctl00_cph_body_treeSetores').getSelectedNodes();
        	if (node != null) {
        		Ext.net.DirectMethods.OpenChartOrganization(node.attributes.Id);
        	}
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
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:TreeGrid runat="server" ID="treeSetores" AnchorVertical="100%" AutoScroll="true"
                        NoLeafIcon="true" AnchorHorizontal="100%" AutoExpandColumn="Nome" EnableDD="true">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treeSetores}.getSelectedNodes().attributes.Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover"  Disabled="true" Icon="Delete">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click" Success="refreshTree">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treeSetores}.getSelectedNodes().attributes.Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este setor?" />
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
                        <Columns>
                            <ext:TreeGridColumn DataIndex="Nome" Header="Setor" Width="200">
                            </ext:TreeGridColumn>
                        </Columns>
                        <SelectionModel>
                            <ext:DefaultSelectionModel runat="server">
                                <DirectEvents>
                                    <SelectionChange OnEvent="treeSetores_SelectedRow">
                                        <ExtraParams>
                                            <ext:Parameter Name="node" Value="#{treeSetores}.getSelectedNodes().attributes.Empresa"
                                                Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </SelectionChange>
                                </DirectEvents>
                            </ext:DefaultSelectionModel>
                        </SelectionModel>
						<Listeners>
							<DblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarSetores').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treeSetores}.getSelectedNodes().attributes.Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                    </ext:TreeGrid>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winSetor" Icon="Theme" Width="450" Height="200" Modal="true"
        Maximizable="true" CloseAction="Hide" Hidden="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmSetor" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:ComboBox runat="server" ID="cboEmpresa" AllowBlank="false" FieldLabel="Empresa"
                                    AnchorHorizontal="92%" MsgTarget="Side" DisplayField="Nome" ValueField="Id" EmptyText="Selecione...">
                                    <Store>
                                        <ext:Store runat="server" ID="strEmpresas">
                                            <Reader>
                                                <ext:JsonReader IDProperty="Id">
                                                    <Fields>
                                                        <ext:RecordField Name="Id">
                                                        </ext:RecordField>
                                                        <ext:RecordField Name="Nome">
                                                        </ext:RecordField>
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                </ext:ComboBox>
                                <ext:Panel ID="pnlDropField" runat="server" Frame="false" Border="false" AutoRender="false"
                                    AnchorHorizontal="100%">
                                    <Items>
                                        <ext:DropDownField runat="server" ID="ddfSetorPai" FieldLabel="Setor Pai" Width="390"
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
                                                                <SelectionChange Fn="selecionarSetorPai" />
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
                    <Click Handler="return #{frmSetor}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click" Success="refreshTree">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="setorPai" Mode="Raw" Value="Ext.getCmp('ctl00_cph_body_hdfSetorPai').value">
                            </ext:Parameter>
                        </ExtraParams>
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
    <ext:Hidden runat="server" ID="hdfSetorPai">
    </ext:Hidden>
	<ext:Hidden runat="server" ID="hdfVisualizarSetores" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarSetores" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarSetores" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverSetores" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarChartSetores" Text="0"/>
</asp:Content>
