<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarPontosUsuarios.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarPontosUsuarios" Title="Registros de Ponto" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
    	function selectRow(row) {
    		var hdfEditarPontosUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfEditarPontosUsuarios').getValue();
    		var hdfRemoverPontosUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfRemoverPontosUsuarios').getValue();
    		var hdfExportarExcelPontosUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfExportarExcelPontosUsuarios').getValue();
    		var hdfGraficoHorasPontosUsuariosValue = Ext.getCmp('ctl00_cph_body_hdfGraficoHorasPontosUsuarios').getValue();
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
            var btnExportarExcel = Ext.getCmp('ctl00_cph_body_btnExportarExcel');
            var btnChartUsuario = Ext.getCmp('ctl00_cph_body_btnChartUsuario');
            var length = Ext.getCmp('ctl00_cph_body_grdPontosUsuarios').selModel.selections.length;

            btnRemover.setDisabled(row == null || length == 0 || hdfRemoverPontosUsuariosValue == '0');
            btnEditar.setDisabled(row == null || length == 0 || hdfEditarPontosUsuariosValue == '0');
            btnExportarExcel.setDisabled(row == null || length == 0 || hdfExportarExcelPontosUsuariosValue == '0');
            btnChartUsuario.setDisabled(row == null || length == 0 || hdfGraficoHorasPontosUsuariosValue == '0');
        }
        function renderTempo(value, p, record) {
            var horas = 0;
            var minutos = parseInt(value);
            if (minutos > 60) {
                minutos = value % 60;
                horas = parseInt(value / 60);
            }

            var strHora = horas.toString().length > 1 ? horas.toString() : '0' + horas;
            var strMinutos = minutos.toString().length > 1 ? minutos.toString() : '0' + minutos;
            return strHora + ':' + strMinutos;
        }
        function renderTotal(value, p, record) {
            var horas = 0;
            var minutos = parseInt(value);
            if (minutos > 60) {
                minutos = value % 60;
                horas = parseInt(value / 60);
            }

            var strHora = horas.toString().length > 1 ? horas.toString() : '0' + horas;
            var strMinutos = minutos.toString().length > 1 ? minutos.toString() : '0' + minutos;
            return strHora + ':' + strMinutos + ' hora(s)';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strPontosUsuarios" runat="server" OnRefreshData="OnRefreshData" GroupField="Usuario">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Data" Type="Date" />
                    <ext:RecordField Name="HoraInicio" Type="Date" />
                    <ext:RecordField Name="HoraTermino" Type="Date" />
                    <ext:RecordField Name="Tempo" Type="Float"/>
                    <ext:RecordField Name="Justificativa" />
                    <ext:RecordField Name="Usuario" ServerMapping="Usuario.Nome" />
                    <ext:RecordField Name="Periodo" ServerMapping="Periodo.Nome" />
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
    <ext:Store ID="strUsuarios" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strUsuariosBusca" runat="server">
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
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%"
                Layout="Border">
                <Items>
                    <ext:FormPanel ID="frmBusca" runat="server" Region="North" Title="Busca" Collapsible="true"
                        ButtonAlign="Left" Collapsed="false" Frame="true" Height="130">
                        <Items>
                            <ext:ComboBox runat="server" ID="cboUsuariosBusca" EmptyText="Selecione o Usuário..."
                                AnchorHorizontal="44.5%" MsgTarget="Side" DisplayField="Nome" StoreID="strUsuariosBusca"
                                ValueField="Id" FieldLabel="Usuário">
                            </ext:ComboBox>
                            <ext:Container runat="server" Height="50" Layout="Column" AnchorHorizontal="92%">
                                <Items>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataInicial" FieldLabel="Período" Editable="true"
                                                Vtype="daterange" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="#{txtDataFinal}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataFinal" Editable="true" Vtype="daterange"
                                                HideLabel="true" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="startDateField" Value="#{txtDataInicial}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.5">
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Buttons>
                            <ext:Button Text="Buscar" Icon="Zoom" runat="server" StyleSpec="padding-left:98px;">
                                <Listeners>
                                    <Click Handler="return #{frmBusca}.validate();" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="btnBuscar_Click">
                                        <EventMask Msg="Buscando Pontos dos Usuários..." Target="Page" ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdPontosUsuarios" runat="server" StoreID="strPontosUsuarios"
                        Region="Center" Frame="true" AutoExpandColumn="Justificativa"
                        AnchorHorizontal="100%" AnchorVertical="100%">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdPontosUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdPontosUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover este registro de ponto?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnExportarExcel" runat="server" Text="Exportar p/ Excel" Icon="PageExcel" AutoPostBack="true" OnClick="btnExportarExcel_Click" Disabled="true"></ext:Button>
                                    <ext:Button ID="btnChartUsuario" runat="server" Text="Horas p/ Usuário" Icon="ChartBar" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnGrafico_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="Usuário" DataIndex="Usuario" Width="200" />
                                <ext:Column Header="Data" Width="50" DataIndex="Data">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y')" />
                                </ext:Column>
                                <ext:Column Header="Início" Width="120" DataIndex="HoraInicio">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('H:i')" />
                                </ext:Column>
                                <ext:Column Header="Término" Width="120" DataIndex="HoraTermino">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('H:i')" />
                                </ext:Column>
                                <ext:Column ColumnID="Justificativa" Header="Justificativa" DataIndex="Justificativa"
                                    Width="150" />
                                <ext:Column Header="Período" DataIndex="Periodo" Width="100" />
                                <ext:GroupingSummaryColumn Width="120" ColumnID="Tempo" Header="Tempo" Sortable="true"
                                    DataIndex="Tempo" SummaryType="Sum">
                                    <Renderer Fn="renderTempo" />
                                    <SummaryRenderer Fn="renderTotal" />
                                </ext:GroupingSummaryColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server">
                                <Listeners>
                                    <SelectionChange Fn="selectRow" />
							    </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
						<Listeners>
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarPontosUsuarios').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdPontosUsuarios}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <View>
                            <ext:GroupingView  runat="server" ForceFit="true" MarkDirty="false" ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true">                   
                            </ext:GroupingView>
                        </View>
                        <Plugins>
                            <ext:GroupingSummary runat="server"></ext:GroupingSummary>
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strPontosUsuarios">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winPontosUsuario" Icon="Clock" Width="550" Height="300"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmPontosUsuario" AnchorVertical="100%">
                            <Items>
                                <ext:ComboBox runat="server" ID="cboUsuario" AllowBlank="false" EmptyText="Selecione o Usuário..."
                                    AnchorHorizontal="92%" MsgTarget="Side" DisplayField="Nome" StoreID="strUsuarios"
                                    ValueField="Id" FieldLabel="Usuário">
                                </ext:ComboBox>
                                <ext:DateField runat="server" ID="txtData" FieldLabel="Data" Editable="true" AllowBlank="false"
                                    Format="dd/MM/yyyy" AnchorHorizontal="50%" MsgTarget="Side">
                                </ext:DateField>
                                <ext:Container runat="server" Layout="Column" Height="25">
                                    <Items>
                                        <ext:Container runat="server" Layout="Form" ColumnWidth=".53">
                                            <Items>
                                                <ext:TimeField runat="server" ID="txtHoraInicial" AllowBlank="false" MsgTarget="Side"
                                                    Width="100" MinTime="00:00" MaxTime="23:59" Editable="true" ForceSelection="false"
                                                    SelectOnFocus="false" Increment="30" Format="HH:mm" FieldLabel="Início">
                                                </ext:TimeField>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="Form" ColumnWidth=".47">
                                            <Items>
                                                <ext:TimeField runat="server" ID="txtHoraFinal" AllowBlank="false" MsgTarget="Side"
                                                    FieldLabel="Término" Width="100" MinTime="00:00" MaxTime="23:59" Editable="true"
                                                    ForceSelection="false" SelectOnFocus="false" Increment="30" Format="HH:mm">
                                                </ext:TimeField>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:Container>
                                <ext:TextArea ID="txtJustificativa" runat="server" FieldLabel="Justificativa" AllowBlank="false"
                                    MsgTarget="Side" AnchorHorizontal="92%" AnchorVertical="-90" MaxLength="200">
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
                    <Click Handler="return #{frmPontosUsuario}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winPontosUsuario}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarPontosUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarPontosUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarPontosUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverPontosUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfExportarExcelPontosUsuarios" Text="0"/>
	<ext:Hidden runat="server" ID="hdfGraficoHorasPontosUsuarios" Text="0"/>
</asp:Content>
