<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarMenuPagina.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarMenuPagina" Title="Paginas" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function refreshTree(a, b, c) {
            var nodes = eval(b.extraParamsResponse.nodes);
            if (nodes.length > 0) {
                Ext.getCmp('ctl00_cph_body_treePaginas').initChildren(nodes);
            }
            else {
                Ext.getCmp('ctl00_cph_body_treePaginas').getRootNode().removeChildren();
            }
        }
        function abrirDropDown() {
            Ext.getCmp('ctl00_cph_body_ddfPaginaPai').collapse();
            Ext.getCmp('ctl00_cph_body_treePaginasWin').initChildren(Ext.getCmp('ctl00_cph_body_treePaginasWin').getRootNode());
        }
        function selecionarPaginaPai() {
            var ddl = Ext.getCmp('ctl00_cph_body_ddfPaginaPai');
            var treeGrid = Ext.getCmp('ctl00_cph_body_treePaginasWin');
            var hdfPaginaPai = Ext.getCmp('ctl00_cph_body_hdfPaginaPai');
            hdfPaginaPai.value = treeGrid.getSelectedNodes().attributes.Id;
            ddl.setValue(treeGrid.getSelectedNodes().attributes.Descricao);
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
    <ext:Store ID="strIcons" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Name">
                <Fields>
                    <ext:RecordField Name="IconCls" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:TreeGrid runat="server" ID="treePaginas" AnchorVertical="100%" AutoScroll="true"
                        NoLeafIcon="true" AnchorHorizontal="100%" AutoExpandColumn="Descricao"
                        EnableDD="true">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditar_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treePaginas}.getSelectedNodes().attributes.Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click" Success="refreshTree">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treePaginas}.getSelectedNodes().attributes.Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta página?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Columns>
                            <ext:TreeGridColumn DataIndex="Descricao" Header="Página" Width="200">
                            </ext:TreeGridColumn>
                            <ext:TreeGridColumn DataIndex="Url" Header="Url" Width="250">
                            </ext:TreeGridColumn>
                            <ext:TreeGridColumn DataIndex="EmMenu" Header="Em Menu" Width="100">
                            </ext:TreeGridColumn>
                            <ext:TreeGridColumn DataIndex="Ordem" Header="Ordem" Width="50">
                            </ext:TreeGridColumn>
                        </Columns>
                        <SelectionModel>
                            <ext:DefaultSelectionModel runat="server">
                                <DirectEvents>
                                    <SelectionChange OnEvent="treePaginas_SelectedRow">
                                    </SelectionChange>
                                </DirectEvents>
                            </ext:DefaultSelectionModel>
                        </SelectionModel>
						<Listeners>
							<DblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarPaginas').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{treePaginas}.getSelectedNodes().attributes.Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                    </ext:TreeGrid>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winPagina" Icon="Theme" Width="450" Height="250" Modal="true"
        Maximizable="true" CloseAction="Hide" Hidden="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmPagina" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtDescricao" MaxLength="100" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextField runat="server" ID="txtUrl" MaxLength="100" AnchorHorizontal="92%" Disabled="true"
                                    AllowBlank="false" MsgTarget="Side" FieldLabel="Url">
                                </ext:TextField>
                                <ext:Panel ID="pnlDropField" runat="server" Frame="false" Border="false" AutoRender="false"
                                    AnchorHorizontal="100%">
                                    <Items>
                                        <ext:DropDownField runat="server" ID="ddfPaginaPai" FieldLabel="Menu" Width="390"
                                            Editable="false" TriggerIcon="SimpleArrowDown">
                                            <Component>
                                                <ext:TreeGrid runat="server" ID="treePaginasWin" Icon="Accept" Height="300" Width="550"
                                                    Shadow="None" UseArrows="true" AutoScroll="true" Animate="true" EnableDD="true"
                                                    AutoRender="false" ContainerScroll="true" RootVisible="false" AutoExpandColumn="Descricao">
                                                    <Columns>
                                                        <ext:TreeGridColumn DataIndex="Descricao" Header="Página" Width="200">
                                                        </ext:TreeGridColumn>
                                                        <ext:TreeGridColumn DataIndex="Url" Header="Url" Width="150">
                                                        </ext:TreeGridColumn>
                                                        <ext:TreeGridColumn DataIndex="EmMenu" Header="Em Menu" Width="75">
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
                                                                <SelectionChange Fn="selecionarPaginaPai" />
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
                                <ext:ComboBox ID="cboIcon" runat="server" StoreID="strIcons" AnchorHorizontal="92%"
                                    FieldLabel="Icone" DisplayField="Name" ValueField="Name" Mode="Local" TriggerAction="All"
                                    EmptyText="Selecione um icone...">
                                    <Template ID="Template1" runat="server">
                                        <Html>
                                            <tpl for=".">
                                                <div class="x-combo-list-item icon-combo-item {IconCls}">
                                                    {Name}
                                                </div>
                                            </tpl>
                                        </Html>
                                    </Template>
                                    <Listeners>
                                        <Select Handler="this.setIconCls(record.get('IconCls'));" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Checkbox runat="server" ID="ckbEmMenu" FieldLabel="Em Menu" LabelSeparator="?">
                                </ext:Checkbox>
                                <ext:NumberField runat="server" ID="txtOrdem" AllowDecimals="false" AllowNegative="false"
                                    AllowBlank="false" MsgTarget="Side" FieldLabel="Ordem" Width="100">
                                </ext:NumberField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmPagina}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click" Success="refreshTree">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="paginaPai" Mode="Raw" Value="Ext.getCmp('ctl00_cph_body_hdfPaginaPai').value">
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
    <ext:Hidden runat="server" ID="hdfPaginaPai">
    </ext:Hidden>
	<ext:Hidden runat="server" ID="hdfVisualizarPaginas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionar" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarPaginas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverPaginas" Text="0"/>
</asp:Content>
