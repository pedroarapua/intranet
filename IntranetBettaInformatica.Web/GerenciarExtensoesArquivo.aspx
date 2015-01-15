<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarExtensoesArquivo.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarExtensoesArquivo" Title="Extensões de Arquivos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function selectRow(row) {
			var length = Ext.getCmp('ctl00_cph_body_grdExtensaoArquivo').selModel.selections.length;
			var hdfRemoverExtensoesArquivoValue = Ext.getCmp('ctl00_cph_body_hdfRemoverExtensoesArquivo').getValue();
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			btnRemover.setDisabled(length == 0 || hdfRemoverExtensoesArquivoValue == '0');
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strExtensoesArquivo" runat="server" OnRefreshData="OnRefreshData"  GroupField="DescricaoTipoExtensao">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Extensao" />
                    <ext:RecordField Name="DescricaoTipoExtensao" />
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
	<ext:Store ID="strTiposExtensao" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
	
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdExtensaoArquivo" runat="server" StoreID="strExtensoesArquivo"
                        Frame="true" AutoExpandColumn="Extensao" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                    <ext:Button runat="server" ID="btnRemover" Text="Remover" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnRemover_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdExtensaoArquivo}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta extensão?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Tipo" DataIndex="DescricaoTipoExtensao" />
                                <ext:Column Header="Extensão" DataIndex="Extensao" />
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
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winExtensaoArquivo" Icon="Theme" Width="450" Height="150"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmExtensao" AnchorVertical="100%">
                            <Items>
								<ext:ComboBox runat="server" ID="cboTipoExtensao" AllowBlank="false" MsgTarget="Side"
                                    Editable="true" EmptyText="Selecione o Tipo da Extensão..." AnchorHorizontal="92%"
                                    FieldLabel="Tipo de Extensão" DisplayField="Descricao" ValueField="Id" StoreID="strTiposExtensao">
                                </ext:ComboBox>
                                <ext:TextField runat="server" ID="txtExtensao" MaxLength="10" AllowBlank="false"
                                    AnchorHorizontal="92%" FieldLabel="Extensão" MsgTarget="Side">
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
                    <Click Handler="return #{frmExtensao}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winExtensaoArquivo}.hide()" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarExtensoesArquivo" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarExtensoesArquivo" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverExtensoesArquivo" Text="0"/>
</asp:Content>
