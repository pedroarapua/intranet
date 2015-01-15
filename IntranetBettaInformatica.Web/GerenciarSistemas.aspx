<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarSistemas.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarSistemas" Title="Sistemas" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
	<script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function selectRow(row) {
			var hdfEditarSistemasValue = Ext.getCmp('ctl00_cph_body_hdfEditarSistemas').getValue();
			var hdfRemoverSistemasValue = Ext.getCmp('ctl00_cph_body_hdfRemoverSistemas').getValue();
			var length = Ext.getCmp('ctl00_cph_body_grdSistemas').selModel.selections.length;
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			btnRemover.setDisabled(length == 0 || hdfRemoverSistemasValue == '0');
			btnEditar.setDisabled(length == 0 || hdfEditarSistemasValue == '0');
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strSistemas" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Url" />
                    <ext:RecordField Name="ExtensaoImagem" />
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
    <ext:FitLayout ID="FitLayout1" runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdSistemas" runat="server" StoreID="strSistemas"
                        Frame="true" AutoExpandColumn="Url" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdSistemas}.getRowsValues({selectedOnly:true}))">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdSistemas}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este sistema?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Nome" DataIndex="Nome" Width="200" />
                                <ext:Column ColumnID="Url" Header="Url" Width="250" DataIndex="Url" />
                                <ext:Column Header="Extensao" Width="75" DataIndex="ExtensaoImagem" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarSistemas').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdSistemas}.getRowsValues({selectedOnly:true}))">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strSistemas">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winSistema" Icon="Package" Width="450" Height="250"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmSistema" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextField runat="server" ID="txtUrl" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Url" MsgTarget="Side">
                                </ext:TextField>
                                <ext:Checkbox FieldLabel="Possui Imagem" LabelSeparator="?" ID="ckbImagem" runat="server">
                                    <Listeners>
                                        <Check Handler="#{fufImagem}.setDisabled(!this.checked);" />
                                    </Listeners>
                                </ext:Checkbox>
                                <ext:FileUploadField runat="server" ID="fufImagem" AnchorHorizontal="92%" FieldLabel="Imagem"
                                    EmptyText="Selecione uma imagem...">
                                </ext:FileUploadField>
                                <ext:Image runat="server" ID="imgAtual" FieldLabel="Imagem Atual" Height="36" Width="36">
                                </ext:Image>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmSistema}.validate();" />
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
	<ext:Hidden runat="server" ID="hdfVisualizarSistemas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarSistemas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarSistemas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverSistemas" Text="0"/>
</asp:Content>
