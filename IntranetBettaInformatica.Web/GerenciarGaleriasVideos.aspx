<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarGaleriasVideos.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarGaleriasVideos" Title="Galerias de Videos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function GroupCommandClick(command, value, record) {
        	if (command == "Delete") {
        		if (Ext.getCmp('ctl00_cph_body_hdfAdicionarVideos').getValue() == '1') {
        			confirm('Confirmação', 'Deseja remover esta galeria?', function (btn) {
        				Ext.net.DirectMethods.RemoverGaleria(record[0].data.GaleriaId);
        			});
        		}
        		else
        			alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        	else if (command == "Edit") {
        		if (Ext.getCmp('ctl00_cph_body_hdfEditarGaleriaVideos').getValue() == '1') {
        			Ext.net.DirectMethods.EditarGaleria(record[0].data.GaleriaId);
        		}
        		else
        			alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        	else {
        		if (Ext.getCmp('ctl00_cph_body_hdfRemoverGaleriaVideos').getValue() == '1') {
					Ext.net.DirectMethods.AdicionarVideo(record[0].data.GaleriaId);
				}
				else
					alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        }
        function selectRow(row) {
        	var hdfEditarVideosValue = Ext.getCmp('ctl00_cph_body_hdfEditarVideos').getValue();
        	var hdfRemoverVideosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverVideos').getValue();
           	var length = Ext.getCmp('ctl00_cph_body_grdGalerias').selModel.selections.length;
           	var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
           	var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
           	btnRemover.setDisabled(length == 0 || hdfRemoverVideosValue == '0');
           	btnEditar.setDisabled(length == 0 || hdfEditarVideosValue == '0');
		}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strVideos" runat="server" OnRefreshData="OnRefreshData" GroupField="Galeria">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="NomeOriginal" />
                    <ext:RecordField Name="Galeria" ServerMapping="Galeria.Nome" />
                    <ext:RecordField Name="GaleriaId" ServerMapping="Galeria.Id" />
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
    <ext:Store ID="strGalerias" runat="server">
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
                    <ext:GridPanel ID="grdGalerias" runat="server" StoreID="strVideos"
                        Height="340" Frame="true" AutoExpandColumn="Titulo" AnchorHorizontal="100%"
                        AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Nova Galeria" Icon="Add">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnNovoVideo" Text="Novo Video" Icon="FilmAdd">
                                        <DirectEvents>
                                            <Click OnEvent="btnNovoVideo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="FilmEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditarVideo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="FilmDelete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemoverVideo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este video?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column ColumnID="Titulo" Header="Descrição" DataIndex="Titulo" Groupable="false" />
                                <ext:Column Header="Nome Arquivo" Width="100" DataIndex="NomeOriginal" Groupable="false" />
                                <ext:Column Header="Galeria" Width="200" DataIndex="Galeria" MenuDisabled="true" />
                                <ext:ImageCommandColumn Width="110">
                                    <GroupCommands>
                                        <ext:GroupImageCommand CommandName="Video" Icon="FilmAdd" RightAlign="true" Text="Adicionar Video">
                                            <ToolTip Text="Adicionar Video" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Delete" Icon="Delete" Text="Remover" RightAlign="true">
                                            <ToolTip Text="Delete" />
                                        </ext:GroupImageCommand>
                                        <ext:GroupImageCommand CommandName="Edit" Icon="TableEdit" Text="Editar" RightAlign="true">
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
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="true"
                                EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarVideos').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditarVideo_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <Listeners>
                            <GroupCommand Fn="GroupCommandClick" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strVideos">
                                <Items>
                                    <ext:Button ID="btnToggleGroups" runat="server" Text="Expandir/Recolher Grupos" Icon="TableSort"
                                        Style="margin-left: 6px;" AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{grdGalerias}.getView().toggleAllGroups();" />
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
    <ext:Window runat="server" ID="winGaleria" Icon="FolderFilm" Width="450" Height="200"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmGaleria" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="100" AllowBlank="false" AnchorHorizontal="92%"
                                    FieldLabel="Nome" MsgTarget="Side">
                                </ext:TextField>
                                <ext:TextArea runat="server" ID="txtDescricao" MaxLength="200" AnchorHorizontal="92%"
                                    AnchorVertical="-30" FieldLabel="Descrição">
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
                    <Click Handler="return #{frmGaleria}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winGaleria}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winVideo" Icon="Film" Width="500" Height="220" Modal="true"
        Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmVideo" AnchorVertical="100%">
                            <Items>
                                <ext:ComboBox runat="server" ID="cboGaleria" AllowBlank="false" MsgTarget="Side"
                                    Editable="true" EmptyText="Selecione a Galeria..." AnchorHorizontal="92%" FieldLabel="Galeria"
                                    DisplayField="Nome" ValueField="Id" StoreID="strGalerias">
                                </ext:ComboBox>
                                <ext:FileUploadField runat="server" ID="fufVideo" AnchorHorizontal="92%" FieldLabel="Video"
                                    EmptyText="Selecione uma video...">
                                </ext:FileUploadField>
                                <ext:TextArea runat="server" ID="txtTituloVideo" MaxLength="200" AnchorHorizontal="92%"
                                    AnchorVertical="-60" FieldLabel="Descrição" MsgTarget="Side">
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
                    <Click Handler="return #{frmVideo}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="SalvarVideo_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winVideo}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarGaleriaVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarGaleriaVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverGaleriaVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarVideos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverVideos" Text="0"/>

</asp:Content>
