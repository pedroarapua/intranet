<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VisualizarAniversariantes.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.VisualizarAniversariantes" Title="Aniversariantes" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <style type="text/css">
        .bold td div
        {
            font-weight:bold !important;
            font-style:italic !important;
        }
    </style>
    <script type="text/javascript">
        function getRowClass(row) {
            return new Date(row.data.DataNascimento).format('d/m') == new Date().format('d/m') ? 'bold' : '';
        }
        function selectRow(row) {
        	var length = Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.length;
        	var hdfEnviarMensagemAniversariantes = Ext.getCmp('ctl00_cph_body_hdfEnviarMensagemAniversariantes').getValue();
        	var data = length > 0 ? Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.items[0].data : null;
            var btnMensagem = Ext.getCmp('ctl00_cph_body_btnMensagem');
            var hdfUsuarioLogado = document.getElementById('ctl00_cph_body_hdfUsuarioLogado');
            btnMensagem.setDisabled(length == 0 || data.Id == parseInt(hdfUsuarioLogado.value) || hdfEnviarMensagemAniversariantes == '0');
        }
        function validarMensagem() {
            return Ext.getCmp('ctl00_cph_body_frmMensagem').validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strUsuarios" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="DataNascimento" Type="Date" />
                    <ext:RecordField Name="Cidade" />
                    <ext:RecordField Name="Estado" ServerMapping="Estado.Sigla" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
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
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%"
                Layout="Border">
                <Items>
                    <ext:FormPanel runat="server" ID="frmBusca" Title="Busca" Collapsed="false" Border="false"
                        Height="70" Collapsible="true" Region="North" Frame="true" ButtonAlign="Left">
                        <Items>
                            <ext:Container ID="Container1" runat="server" Height="30" Layout="Column" AnchorHorizontal="70%">
                                <Items>
                                    <ext:Container ID="Container2" runat="server" Layout="Form" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataInicialBusca" FieldLabel="Período" Editable="true"
                                                Vtype="daterange" Format="dd/MM" AnchorHorizontal="80%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="#{txtDataFinal}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container3" runat="server" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataFinalBusca" Editable="true" Vtype="daterange"
                                                HideLabel="true" Format="dd/MM" AnchorHorizontal="63%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="startDateField" Value="#{txtDataInicial}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container4" runat="server" Layout="Form" ColumnWidth="0.5">
										<Items>
											<ext:Button ID="Button1" Text="Buscar" Icon="Zoom" runat="server">
												<Listeners>
													<Click Handler="return #{frmBusca}.validate();" />
												</Listeners>
												<DirectEvents>
													<Click OnEvent="btnBuscar_Click">
														<EventMask Msg="Buscando Usuários..." Target="Page" ShowMask="true" />
													</Click>
												</DirectEvents>
											</ext:Button>
										</Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios"
                        Frame="true" AutoExpandColumn="Nome" AnchorHorizontal="100%" Layout="Fit"
                        Region="Center">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnMensagem" Text="Enviar Mensagem" Icon="Email" ToolTip="Enviar Mensagem" ToolTipType="Qtip" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnMensagem_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="200" />
                                <ext:Column Header="Data de Nasc." DataIndex="DataNascimento" Width="120">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y')" />
                                </ext:Column>
                                <ext:Column Header="Cidade" DataIndex="Cidade" Width="120" />
                                <ext:Column Header="Estado" DataIndex="Estado" Width="75" />
                                <ext:Column Header="Empresa" DataIndex="Empresa" Width="120" />
                                <ext:Column Header="Setor" DataIndex="Setor" Width="120" />
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
                            <ext:GridView runat="server">
                                <GetRowClass Fn="getRowClass" />
                            </ext:GridView>
                        </View>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strUsuarios"></ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winMensagem" Icon="Email" Width="690" Height="350" AutoScroll="true"
        Modal="true" Hidden="true" Maximizable="true" Layout="fit">
        <Items>
            <ext:FormPanel runat="server" Frame="true" ID="frmMensagem" AnchorVertical="100%">
                <Items>
                    <ext:HtmlEditor runat="server" ID="txtMensagem" AnchorHorizontal="100%" HideLabel="true" AnchorVertical="-40"
                        Note="Máximo de 2000 caracteres no conteúdo HTML.">
                    </ext:HtmlEditor>
                    <ext:Checkbox runat="server" ID="chkConfirmarLeitura" FieldLabel="Confirmar Leitura"
                        LabelSeparator="?">
                    </ext:Checkbox>
                </Items>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="Button2" runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return validarMensagem()" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <ExtraParams>
                            <ext:Parameter Name="usuario" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winMensagem}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <asp:HiddenField runat="server" ID="hdfUsuarioLogado" />
	<ext:Hidden runat="server" ID="hdfVisualizarAniversariantes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEnviarMensagemAniversariantes" Text="0"/>
</asp:Content>
