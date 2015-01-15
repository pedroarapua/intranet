<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarManualColaborador.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarManualColaborador" Title="Manual do Colaborador" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
    	function GroupCommandClick(command, value, record) {
    		if (command == "Delete") {
    			if(Ext.getCmp('ctl00_cph_body_hdfAdicionarTopicoManualColaborador').getValue() == '1')
				{
    				confirm('Confirmação', 'Deseja remover este tópico do manual e os itens do mesmo?', function (btn) {
						Ext.net.DirectMethods.RemoverTopico(record[0].data.TopicoId);
					});
				}
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
            }
            else if (command == "Edit")
			{
				if (Ext.getCmp('ctl00_cph_body_hdfEditarTopicoManualColaborador').getValue() == '1')
					Ext.net.DirectMethods.EditarTopico(record[0].data.TopicoId);
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
			}
            else
			{
				if (Ext.getCmp('ctl00_cph_body_hdfAdicionarItemManualColaborador').getValue() == '1')
					Ext.net.DirectMethods.AdicionarItemManual(record[0].data.TopicoId);
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
			}
        }
        function selectRow(row) {
        	var hdfEditarItemManualColaboradorValue = Ext.getCmp('ctl00_cph_body_hdfEditarItemManualColaborador').getValue();
        	var hdfRemoverItemManualColaboradorValue = Ext.getCmp('ctl00_cph_body_hdfRemoverItemManualColaborador').getValue();
        	var length = Ext.getCmp('ctl00_cph_body_grdItensManual').selModel.selections.length;
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			if(btnRemover)
				btnRemover.setDisabled(length == 0 || hdfRemoverItemManualColaboradorValue == '0');
			if(btnEditar)
				btnEditar.setDisabled(length == 0 || hdfEditarItemManualColaboradorValue == '0');
        }
    </script>
	<style type="text/css">
		.x-grid3-cell-inner {
			white-space: normal;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strItensManual" runat="server" OnRefreshData="OnRefreshData" GroupField="Topico">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="Topico" ServerMapping="Topico.Titulo" />
                    <ext:RecordField Name="TopicoId" ServerMapping="Topico.Id" />
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
    <ext:Store ID="strTopicos" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdItensManual" runat="server" StoreID="strItensManual" AutoExpandColumn="Descricao"
                        AnchorHorizontal="100%" AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Novo Tópico" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnNovoItemManual" Text="Novo Item Manual" Icon="BookAdd">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovoItemManual_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditarItemManual_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdItensManual}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemoverItemManual_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdItensManual}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este item do manual?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Item Manual" DataIndex="Descricao" Groupable="false" />
                                <ext:Column Header="Tópico" Width="200" DataIndex="Topico" MenuDisabled="true" />
                                <ext:ImageCommandColumn Width="110" Hidden="true">
									<GroupCommands>
                                        <ext:GroupImageCommand CommandName="ItemManual" Icon="BookAdd" RightAlign="true" HideMode="Visibility"
                                            Text="Adicionar Item Manual">
											<ToolTip Text="Adicionar Item Manual" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Delete" Icon="Delete" Text="Remover" RightAlign="true" HideMode="Visibility">
                                            <ToolTip Text="Remover" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Edit" Icon="NoteEdit" Text="Editar" RightAlign="true" HideMode="Visibility">
                                            <ToolTip Text="Editar" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarItemManualColaborador').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditarItemManual_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdItensManual}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <Listeners>
                            <GroupCommand Fn="GroupCommandClick" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strItensManual">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdItensManual}.getView().toggleAllGroups();" />
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
    <ext:Window runat="server" ID="winTopico" Width="450" Height="270" Icon="Folder"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmTopico" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtTitulo" MaxLength="200" AllowBlank="false" AnchorHorizontal="92%" FieldLabel="Título" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextArea runat="server" ID="txtItemManual" MaxLength="1000" AllowBlank="false" AnchorHorizontal="92%" AnchorVertical="-30"
                                    FieldLabel="Item Manual" MsgTarget="Side">
                                </ext:TextArea>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmTopico}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winTopico}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winItemManual" Icon="DatabaseYellow" Width="500" Height="240"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmItemManual" AnchorVertical="100%">
                            <Items>
                                <ext:ComboBox runat="server" ID="cboTopico" AllowBlank="false" MsgTarget="Side" Editable="true"
                                    EmptyText="Selecione o Topico..." AnchorHorizontal="92%" FieldLabel="Topico" DisplayField="Titulo"
                                    ValueField="Id" StoreID="strTopicos">
                                </ext:ComboBox>
                                <ext:TextArea runat="server" ID="txtItemManualItem" MaxLength="1000" AllowBlank="false" AnchorHorizontal="92%" AnchorVertical="-30" FieldLabel="Nome Arquivo" MsgTarget="Side">
                                </ext:TextArea>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmItemManual}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="SalvarItemManual_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winItemManual}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarItemManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarTopicoManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarTopicoManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverTopicoManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarItemManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarItemManualColaborador" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverItemManualColaborador" Text="0"/>
</asp:Content>
