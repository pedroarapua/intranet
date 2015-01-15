<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarArquivos.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarArquivos" Title="Banco de Arquivos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
    	function GroupCommandClick(command, value, record) {
    		if (command == "Delete") {
    			if(Ext.getCmp('ctl00_cph_body_hdfAdicionarPastaBancoArquivos').getValue() == '1')
				{
    				confirm('Confirmação', 'Deseja remover esta pasta?', function (btn) {
						Ext.net.DirectMethods.RemoverTipo(record[0].data.TipoId);
					});
				}
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
            }
            else if (command == "Edit")
			{
				if (Ext.getCmp('ctl00_cph_body_hdfEditarPastaBancoArquivos').getValue() == '1')
					Ext.net.DirectMethods.EditarTipo(record[0].data.TipoId);
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
			}
            else
			{
				if (Ext.getCmp('ctl00_cph_body_hdfAdicionarArquivoBancoArquivos').getValue() == '1')
					Ext.net.DirectMethods.AdicionarArquivo(record[0].data.TipoId);
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
			}
        }
        function selectRow(row) {
        	var hdfEditarArquivoBancoArquivosValue = Ext.getCmp('ctl00_cph_body_hdfEditarArquivoBancoArquivos').getValue();
        	var hdfRemoverArquivoBancoArquivosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverArquivoBancoArquivos').getValue();
        	var hdfDownloadArquivoBancoArquivosValue = Ext.getCmp('ctl00_cph_body_hdfDownloadArquivoBancoArquivos').getValue();
            var length = Ext.getCmp('ctl00_cph_body_grdArquivos').selModel.selections.length;
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
            var btnDownload = Ext.getCmp('ctl00_cph_body_btnDownload');
            btnRemover.setDisabled(length == 0 || hdfRemoverArquivoBancoArquivosValue == '0');
            btnEditar.setDisabled(length == 0 || hdfEditarArquivoBancoArquivosValue == '0');
            btnDownload.setDisabled(length == 0 || hdfDownloadArquivoBancoArquivosValue == '0');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strArquivos" runat="server" OnRefreshData="OnRefreshData" GroupField="Tipo">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="NomeOriginal" />
                    <ext:RecordField Name="Tipo" ServerMapping="Tipo.Nome" />
                    <ext:RecordField Name="TipoId" ServerMapping="Tipo.Id" />
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
    <ext:Store ID="strTipos" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdArquivos" runat="server" StoreID="strArquivos" AutoExpandColumn="Descricao"
                        AnchorHorizontal="100%" AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Nova Pasta" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnNovoArquivo" Text="Novo Arquivo" Icon="DatabaseAdd">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovoArquivo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditarArquivo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdArquivos}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemoverArquivo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdArquivos}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este arquivo?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnDownload" Text="Download" Icon="ArrowDown" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDowloadArquivo_Click" Method="POST">
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdArquivos}.getRowsValues({selectedOnly:true})[0].Id">
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
                                <ext:Column Header="Nome Arquivo" DataIndex="Nome" Groupable="false" />
                                <ext:Column ColumnID="Descricao" Header="Descricao" DataIndex="Descricao" Groupable="false" />
                                <ext:Column Header="Arquivo" Width="100" DataIndex="NomeOriginal" Groupable="false" />
                                <ext:Column Header="Pasta" Width="200" DataIndex="Tipo" MenuDisabled="true" />
                                <ext:ImageCommandColumn Width="110">
									<GroupCommands>
                                        <ext:GroupImageCommand CommandName="Arquivo" Icon="DatabaseAdd" RightAlign="true" HideMode="Visibility"
                                            Text="Adicionar Arquivo">
											<ToolTip Text="Adicionar Arquivo" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Delete" Icon="Delete" Text="Remover" RightAlign="true" HideMode="Visibility">
                                            <ToolTip Text="Delete" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Edit" Icon="NoteEdit" Text="Editar" RightAlign="true" HideMode="Visibility">
                                            <ToolTip Text="Edit" />
                                        </ext:GroupImageCommand>
                                    </GroupCommands>
                                </ext:ImageCommandColumn>
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarArquivoBancoArquivos').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditarArquivo_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdArquivos}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <Listeners>
                            <GroupCommand Fn="GroupCommandClick" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strArquivos">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdArquivos}.getView().toggleAllGroups();" />
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
    <ext:Window runat="server" ID="winTipoArquivo" Width="450" Height="270" Icon="Folder"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmTipo" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtTipoNome" MaxLength="200" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Pasta" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome Arquivo" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextArea runat="server" ID="txtDescricao" MaxLength="500" AnchorHorizontal="92%"
                                    AnchorVertical="-100" FieldLabel="Descrição Arquivo">
                                </ext:TextArea>
                                <ext:FileUploadField runat="server" ID="fufArquivoTipo" AnchorHorizontal="92%" FieldLabel="Arquivo"
                                    AllowBlank="false" EmptyText="Selecione um arquivo...">
                                </ext:FileUploadField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmTipo}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winTipoArquivo}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winArquivo" Icon="DatabaseYellow" Width="500" Height="240"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmArquivo" AnchorVertical="100%">
                            <Items>
                                <ext:ComboBox runat="server" ID="cboTipo" AllowBlank="false" MsgTarget="Side" Editable="true"
                                    EmptyText="Selecione a Pasta..." AnchorHorizontal="92%" FieldLabel="Pasta" DisplayField="Nome"
                                    ValueField="Id" StoreID="strTipos">
                                </ext:ComboBox>
                                <ext:TextField runat="server" ID="txtNomeArquivo" MaxLength="200" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Nome Arquivo" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextArea runat="server" ID="txtDescricaoArquivo" MaxLength="500" AnchorHorizontal="92%"
                                    AnchorVertical="-100" FieldLabel="Descrição Arquivo">
                                </ext:TextArea>
                                <ext:FileUploadField runat="server" ID="fufArquivo" AnchorHorizontal="92%" FieldLabel="Arquivo"
                                    EmptyText="Selecione um arquivo...">
                                </ext:FileUploadField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmArquivo}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="SalvarArquivo_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winArquivo}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarPastaBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarPastaBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverPastaBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarArquivoBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarArquivoBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverArquivoBancoArquivos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfDownloadArquivoBancoArquivos" Text="0"/>

</asp:Content>
