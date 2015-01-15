<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarGaleriasFotos.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarGaleriasFotos" Title="Galerias de Fotos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function GroupCommandClick(command,value,record) {
        	if (command == "Delete") {
				if (Ext.getCmp('ctl00_cph_body_hdfRemoverGaleriaFotos').getValue() == '1') {
        			confirm('Confirmação', 'Deseja remover esta galeria?', function (btn) {
        				Ext.net.DirectMethods.RemoverGaleria(record[0].data.GaleriaId);
        			});
        		}
        		else
        			alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        	else if (command == "Edit") {
        		if (Ext.getCmp('ctl00_cph_body_hdfEditarGaleriaFotos').getValue() == '1') {
        			Ext.net.DirectMethods.EditarGaleria(record[0].data.GaleriaId);
        		}
        		else
        			alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        	else {
        		if (Ext.getCmp('ctl00_cph_body_hdfAdicionarFotos').getValue() == '1') {
        			Ext.net.DirectMethods.AdicionarFoto(record[0].data.GaleriaId);
        		}
        		else
        			alert('Usuário sem Permissão', 'Você não tem permissão para esta ação.');
        	}
        }
        function selectRow(row) {
        	var hdfEditarFotosValue = Ext.getCmp('ctl00_cph_body_hdfEditarFotos').getValue();
        	var hdfRemoverFotosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverFotos').getValue();
			var length = Ext.getCmp('ctl00_cph_body_grdGalerias').selModel.selections.length;
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			btnRemover.setDisabled(length == 0 || hdfRemoverFotosValue == '0');
			btnEditar.setDisabled(length == 0 || hdfEditarFotosValue == '0');
	    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strFotos" runat="server" OnRefreshData="OnRefreshData" GroupField="Galeria">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="CaminhoImagemThumb" />
                    <ext:RecordField Name="NomeOriginal" />
                    <ext:RecordField Name="CapaAlbum" />
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
            <ext:GridPanel ID="grdGalerias" runat="server" StoreID="strFotos"
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
                            <ext:Button runat="server" ID="btnNovaFoto" Text="Nova Foto" Icon="PhotoAdd">
                                <DirectEvents>
                                    <Click OnEvent="btnNovaFoto_Click">
                                        <EventMask ShowMask="true" Target="Page" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnEditarFoto_Click">
                                        <EventMask ShowMask="true" Target="Page" />
                                        <ExtraParams>
                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnRemoverFoto_Click">
                                        <EventMask ShowMask="true" Target="Page" />
                                        <ExtraParams>
                                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id">
                                            </ext:Parameter>
                                        </ExtraParams>
                                        <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta foto?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:TemplateColumn DataIndex="" MenuDisabled="true" Header="Foto" Width="50" Align="Center">
                            <Template runat="server">
                                <Html>
						            <img src="{CaminhoImagemThumb}"></img>
					            </Html>
                            </Template>
                        </ext:TemplateColumn>
                        <ext:Column ColumnID="Titulo" Header="Descrição" DataIndex="Titulo" Groupable="false" />
                        <ext:Column Header="Nome Arquivo" Width="100" DataIndex="NomeOriginal" Groupable="false" />
                        <ext:Column Header="É Capa do Album?" Width="50" DataIndex="CapaAlbum" Groupable="false">
                            <Renderer Handler="return value == false ? 'Não' : 'Sim';" />
                        </ext:Column>
                        <ext:Column Header="Galeria" Width="200" DataIndex="Galeria" MenuDisabled="true" />
                        <ext:ImageCommandColumn Width="110">
                            <GroupCommands>
                                <ext:GroupImageCommand CommandName="Foto" Icon="PhotoAdd" RightAlign="true" Text="Adicionar Foto">
                                    <ToolTip Text="Adicionar Foto" />
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
					<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarFotos').getValue() == '1';" />
				</Listeners>
                <DirectEvents>
                    <RowDblClick OnEvent="btnEditarFoto_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="id" Mode="Raw" Value="#{grdGalerias}.getRowsValues({selectedOnly:true})[0].Id"></ext:Parameter>
                        </ExtraParams>
                    </RowDblClick>
                </DirectEvents>
                <Listeners>
                    <GroupCommand Fn="GroupCommandClick" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar runat="server" PageSize="20" StoreID="strFotos">
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
    <ext:Window runat="server" ID="winGaleria" Icon="BoxPicture" Width="450" Height="270"
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
                                    AnchorVertical="-120" FieldLabel="Descrição">
                                </ext:TextArea>
                                <ext:FileUploadField runat="server" ID="fufImagemAlbum" AnchorHorizontal="92%" FieldLabel="Capa de Album" AllowBlank="false"
                                    EmptyText="Selecione uma imagem para o Album...">
                                </ext:FileUploadField>
                                <ext:Image runat="server" ID="imgAtual" FieldLabel="Imagem de Capa">
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

    <ext:Window runat="server" ID="winFoto" Icon="Photo" Width="500" Height="240"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmFoto" AnchorVertical="100%">
                            <Items>
                                <ext:ComboBox runat="server" ID="cboGaleria" AllowBlank="false" MsgTarget="Side"
                                    Editable="true" EmptyText="Selecione a Galeria..." AnchorHorizontal="92%"
                                    FieldLabel="Galeria" DisplayField="Nome" ValueField="Id" StoreID="strGalerias">
                                </ext:ComboBox>
                                <ext:FileUploadField runat="server" ID="fufImagem" AnchorHorizontal="92%" FieldLabel="Imagem" 
                                    EmptyText="Selecione uma imagem...">
                                </ext:FileUploadField>
                                <ext:Checkbox runat="server" ID="chkCapaAlbum" FieldLabel="É Capa de Album" LabelSeparator="?"></ext:Checkbox>
                                <ext:TextArea runat="server" ID="txtTituloFoto" MaxLength="200" AnchorHorizontal="92%" AnchorVertical="-80"
                                    FieldLabel="Descrição" MsgTarget="Side">
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
                    <Click Handler="return #{frmFoto}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="SalvarFoto_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winFoto}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Hidden runat="server" ID="hdfVisualizarFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarGaleriaFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarGaleriaFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverGaleriaFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarFotos" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverFotos" Text="0"/>
</asp:Content>
