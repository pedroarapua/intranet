<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarTiposEmpresa.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarTiposEmpresa" Title="Tipos de Empresa" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function selectRow(row) {
			var hdfEditarTiposEmpresaValue = Ext.getCmp('ctl00_cph_body_hdfEditarTiposEmpresa').getValue();
			var hdfRemoverTiposEmpresaValue = Ext.getCmp('ctl00_cph_body_hdfRemoverTiposEmpresa').getValue();
			var length = Ext.getCmp('ctl00_cph_body_grdTiposEmpresa').selModel.selections.length;
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			btnRemover.setDisabled(length == 0 || hdfRemoverTiposEmpresaValue == '0');
			btnEditar.setDisabled(length == 0 || hdfEditarTiposEmpresaValue == '0');
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strTiposEmpresa" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
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
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdTiposEmpresa" runat="server" StoreID="strTiposEmpresa"
                        Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdTiposEmpresa}.getRowsValues({selectedOnly:true}))">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdTiposEmpresa}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este tipo de empresa?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Descrição" DataIndex="Descricao" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarTiposEmpresa').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdTiposEmpresa}.getRowsValues({selectedOnly:true}))">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strTiposEmpresa" AnchorHorizontal="100%">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winTipoEmpresa" Icon="Theme" Width="450" Height="150"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmTipoEmpresa" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtDescricao" MaxLength="100" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Descrição" MsgTarget="Side">
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
                    <Click Handler="return #{frmTipoEmpresa}.validate();" />
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
	<ext:Hidden runat="server" ID="hdfVisualizarTiposEmpresa" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarTiposEmpresa" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarTiposEmpresa" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverTiposEmpresa" Text="0"/>
</asp:Content>
