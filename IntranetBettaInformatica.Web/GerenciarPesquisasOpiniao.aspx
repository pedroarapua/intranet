<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarPesquisasOpiniao.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarPesquisasOpiniao" Title="Pesquisas de Opinião" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <ext:XScript ID="XScript1" runat="server">
		<script type="text/javascript">
			var addResposta = function () {
				var grid = #{grdRespostas};
				grid.getRowEditor().stopEditing();
                
				grid.addRecord(0, {
					Descricao : "",
					Id: 0
				});
                
				grid.getView().refresh();
				var length = grid.getView().getRows().length - 1;
				grid.getSelectionModel().selectRow(length);
				grid.getRowEditor().startEditing(length);
			}
            
			var removeResposta = function () {
				var grid = #{grdRespostas};
				grid.getRowEditor().stopEditing();
                
				var s = grid.getSelectionModel().getSelections();
                
				for (var i = 0, r; r = s[i]; i++) {
					#{strRespostas}.remove(r);
				}
				Ext.getCmp('ctl00_cph_body_btnAddResposta').setDisabled(false);
				Ext.getCmp('ctl00_cph_body_btnRemoveResposta').setDisabled(true);
			}
		</script>
    </ext:XScript>
	<script type="text/javascript">
            function cancelResposta(cmp, valido, btn) {
                if (!valido) {
                    var grid = Ext.getCmp('ctl00_cph_body_grdRespostas');
                    var length = grid.getView().getRows().length - 1;
                    alert('Erro', 'Resposta inválida.');
                    grid.getRowEditor().startEditing(length);
                }
                else {

                    Ext.getCmp('ctl00_cph_body_btnAddResposta').setDisabled(false);
                    if (cmp.record.data.Descricao == '' && btn == 'btnCancel') {
                        removeResposta();
                    }
                    else {
                        Ext.getCmp('ctl00_cph_body_btnRemoveResposta').setDisabled(false);
                    }
                }
            }
            function salvarResposta() {
                desabilitarBotoesResposta(false);
            }
            function respostaRowSelect(a) {
               Ext.getCmp('ctl00_cph_body_btnRemoveResposta').setDisabled(a == null);
            }
            function desabilitarBotoesResposta(disabled) {
                Ext.getCmp('ctl00_cph_body_btnAddResposta').setDisabled(disabled);
                Ext.getCmp('ctl00_cph_body_btnRemoveResposta').setDisabled(disabled);
            }
            function validarForm()
            {
                var frm = Ext.getCmp('ctl00_cph_body_frmPesquisa');
                var validateForm = frm.validate();
                var respostas = Ext.getCmp('ctl00_cph_body_grdRespostas').getRowsValues();
                var tabPesquisa = Ext.getCmp('ctl00_cph_body_tabPesquisa');
                if(!validateForm)
                {
                    alert('Campos obrigatórios', 'Existem campos obrigatórios a serem preenchidos.');
                    tabPesquisa.setActiveTab(0);
                    return false;
                }
                else if(respostas.length <= 1)
                {
                    alert('Resposta', 'Pelo menos 2 respostas devem ser informadas para a pesquisa.');
                    tabPesquisa.setActiveTab(1);
                    return false;
                }
                return true;
            }

            function confirmarUsuarios()
            {
                var win = Ext.getCmp('ctl00_cph_body_winAdicionarUsuarios');
                var grdUsuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios');
                var grdUsuariosPesq = Ext.getCmp('ctl00_cph_body_grdUsuariosPesquisa');
                var usuariosAdd = grdUsuariosPesq.getRowsValues({selectedOnly:true});

                for(var i=0; i < usuariosAdd.length;i++)
                {
                    grdUsuarios.insertRecord(0, usuariosAdd[i]);
                }
                grdUsuarios.selModel.clearSelections();
                rowSelect(null);
                win.hide();
            }

            function removerUsuarios()
            {
                Ext.getCmp('ctl00_cph_body_grdUsuarios').deleteSelected();
                rowSelect(null);
            }

            function rowSelect(row)
            {
                var btnRemoverUsuarios = Ext.getCmp('ctl00_cph_body_btnRemoverUsuarios');
                btnRemoverUsuarios.setDisabled(!row);
            }  

            function selectRow(row)
            {
				var hdfEditarPesquisasValue = Ext.getCmp('ctl00_cph_body_hdfEditarPesquisas').getValue();
				var hdfRemoverPesquisasValue = Ext.getCmp('ctl00_cph_body_hdfRemoverPesquisas').getValue();
				var hdfVisualizarResultadosPesquisasValue = Ext.getCmp('ctl00_cph_body_hdfVisualizarResultadosPesquisas').getValue();
                var btnVisualizarGrafico = Ext.getCmp('ctl00_cph_body_btnVisualizarGrafico');
                var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
                var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
                var length = Ext.getCmp('ctl00_cph_body_grdPesquisas').selModel.selections.length;
                btnVisualizarGrafico.setDisabled(length == 0 || hdfVisualizarResultadosPesquisasValue == '0');
                btnEditar.setDisabled(length == 0 || hdfEditarPesquisasValue == '0');
                btnRemover.setDisabled(length == 0 || hdfRemoverPesquisasValue == '0');
            }
            
            function getUsuarios(array)
            {
                var args = [];
                for(var i = 0; i < array.length; i++)
                    args[i] = { Id: array[i].Id };
                return args;
            } 
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strPesquisas" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Pergunta" />
                    <ext:RecordField Name="DataInicial" Type="Date" />
                    <ext:RecordField Name="DataFinal" Type="Date" />
                    <ext:RecordField Name="Status" />
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
    <ext:Store ID="strRespostas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strUsuarios" runat="server" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strUsuariosPesquisa" runat="server" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdPesquisas" runat="server" StoreID="strPesquisas" Frame="true"
                        AutoExpandColumn="Pergunta" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditar_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPesquisas}.getRowsValues({selectedOnly:true}))">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPesquisas}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta pesquisa de opinião?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnVisualizarGrafico" Text="Resultado" Icon="ChartBar"
                                        Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnVisualizarGrafico_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdPesquisas}.getRowsValues({selectedOnly:true})[0].Id">
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
                                <ext:Column ColumnID="Pergunta" Header="Pergunta" DataIndex="Pergunta" Width="200" />
                                <ext:Column Header="Data Inicial" Width="120" DataIndex="DataInicial">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Data Final" Width="120" DataIndex="DataFinal">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Status" Width="100" DataIndex="Status" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarPesquisas').getValue() == '1';" />
						</Listeners>
						<DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdPesquisas}.getRowsValues({selectedOnly:true}))">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strPesquisas">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winPesquisa" Icon="Help" Width="550" Height="400"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:TabPanel ID="tabPesquisa" runat="server" ActiveIndex="0">
                            <Items>
                                <ext:FormPanel runat="server" Frame="true" ID="frmPesquisa" Title="Dados da Pesquisa"
                                    LabelWidth="120" AnchorVertical="100%">
                                    <Items>
                                        <ext:TextArea runat="server" ID="txtPergunta" MaxLength="500" AllowBlank="false"
                                            AnchorHorizontal="92%" AnchorVertical="-150" FieldLabel="Pergunta" MsgTarget="Side">
                                        </ext:TextArea>
                                        <ext:Checkbox runat="server" ID="chkMostrarResultado" FieldLabel="Mostrar Resultado"
                                            LabelSeparator="?">
                                        </ext:Checkbox>
                                        <ext:Container runat="server" Layout="Column" Height="100">
                                            <Items>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth=".7">
                                                    <Items>
                                                        <ext:DateField runat="server" ID="txtDataInicial" FieldLabel="Início" Editable="true"
                                                            AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                        </ext:DateField>
                                                        <ext:DateField runat="server" ID="txtDataFinal" FieldLabel="Término" Editable="true"
                                                            AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth=".3">
                                                    <Items>
                                                        <ext:TimeField runat="server" ID="txtHoraInicial" AllowBlank="false" MsgTarget="Side"
                                                            Width="100" MinTime="00:00" MaxTime="23:59" Editable="true" ForceSelection="false"
                                                            SelectOnFocus="false" Increment="30" Format="HH:mm" HideLabel="true">
                                                        </ext:TimeField>
                                                        <ext:TimeField runat="server" ID="txtHoraFinal" AllowBlank="false" MsgTarget="Side"
                                                            HideLabel="true" Width="100" MinTime="00:00" MaxTime="23:59" Editable="true"
                                                            ForceSelection="false" SelectOnFocus="false" Increment="30" Format="HH:mm">
                                                        </ext:TimeField>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:FormPanel>
                                <ext:Panel runat="server" Title="Respostas" Border="false" Layout="fit">
                                    <Items>
                                        <ext:GridPanel runat="server" ID="grdRespostas" StoreID="strRespostas" Frame="true"
                                            AutoExpandColumn="Descricao">
                                            <TopBar>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnAddResposta" runat="server" Text="Adicionar" Icon="Add">
                                                            <Listeners>
                                                                <Click Fn="addResposta" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button ID="btnRemoveResposta" runat="server" Text="Remover" Icon="Delete" Disabled="true">
                                                            <Listeners>
                                                                <Click Fn="removeResposta" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel runat="server">
                                                <Columns>
                                                    <ext:RowNumbererColumn />
                                                    <ext:Column Header="Resposta" DataIndex="Descricao" MenuDisabled="true" Sortable="false"
                                                        Groupable="false" Hideable="false">
                                                        <Editor>
                                                            <ext:TextField ID="txtResposta" runat="server" AllowBlank="false" MaxLength="200">
                                                            </ext:TextField>
                                                        </Editor>
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <Plugins>
                                                <ext:RowEditor runat="server" SaveText="Update" ErrorText="Erros">
                                                    <Listeners>
                                                        <CancelEdit Fn="function(cmp, valido){ cancelResposta(cmp, valido, 'btnCancel');}" />
                                                        <AfterEdit Fn="salvarResposta" />
                                                        <BeforeEdit Fn="function(){desabilitarBotoesResposta(true);}" />
                                                    </Listeners>
                                                </ext:RowEditor>
                                            </Plugins>
                                            <View>
                                                <ext:GridView runat="server" />
                                            </View>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server" SingleSelect="true" />
                                            </SelectionModel>
                                            <Listeners>
                                                <RowClick Fn="respostaRowSelect" />
                                            </Listeners>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel runat="server" Title="Usuários" Border="false" Layout="fit">
                                    <Items>
                                        <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios" Frame="true"
                                            Layout="fit" AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
                                            <TopBar>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:Button runat="server" Text="Adicionar" Icon="UserAdd">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnAdicionarUsuarios_Click">
                                                                    <EventMask ShowMask="true" Target="Page" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:Button ID="btnRemoverUsuarios" runat="server" Text="Remover" Disabled="true"
                                                            Icon="UserDelete">
                                                            <Listeners>
                                                                <Click Fn="removerUsuarios" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                                                    <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                                                    <ext:Column Header="Setor" DataIndex="Setor" Width="90" Hideable="true" />
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server" SingleSelect="false">
                                                    <Listeners>
                                                        <SelectionChange Fn="rowSelect" />
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
                                </ext:Panel>
                            </Items>
                        </ext:TabPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return validarForm();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <EventMask ShowMask="true" Target="Page" />
                        <ExtraParams>
                            <ext:Parameter Name="respostas" Value="Ext.encode(#{grdRespostas}.getRowsValues())"
                                Mode="Raw">
                            </ext:Parameter>
                            <ext:Parameter Name="usuarios" Value="Ext.encode(getUsuarios(#{grdUsuarios}.getRowsValues()))"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winPesquisa}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winAdicionarUsuarios" Icon="User" Width="550" Height="400"
        Modal="true" Hidden="true" Maximizable="true" Title="Adicionando Usuários a Pesquisa"
        Layout="fit">
        <Items>
            <ext:GridPanel ID="grdUsuariosPesquisa" runat="server" StoreID="strUsuariosPesquisa"
                Frame="true" StripeRows="true" AutoExpandColumn="colNome">
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                        <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                        <ext:Column Header="Setor" DataIndex="Setor" Width="90" Hideable="true" />
                    </Columns>
                </ColumnModel>
                <Plugins>
                    <ext:GridFilters runat="server" Local="true">
                        <Filters>
                            <ext:StringFilter DataIndex="Nome" />
                            <ext:StringFilter DataIndex="Empresa" />
                            <ext:StringFilter DataIndex="Setor" />
                        </Filters>
                    </ext:GridFilters>
                </Plugins>
                <SelectionModel>
                    <ext:CheckboxSelectionModel runat="server" />
                </SelectionModel>
            </ext:GridPanel>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Confirmar" Icon="BulletTick">
                <Listeners>
                    <Click Fn="confirmarUsuarios" />
                </Listeners>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winAdicionarUsuarios}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winGrafico" Icon="ChartBar" Width="650" Height="480"
        Title="Gráfico com Resultado da Pesquisa de Opinião" AutoRender="false" Modal="true"
        Hidden="true" Resizable="false">
        <Items>
            <ext:FitLayout runat="server">
                <Content>
                    <asp:Chart ID="Chart1" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                        Height="420px" Width="640px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom"
                        BorderWidth="2" BorderColor="26, 59, 105" IsSoftShadows="False">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                Text="Gráfico de Respostas da Pesquisa" Name="Title1" ForeColor="26, 59, 105">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                                Name="Default">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                            <asp:Series ChartArea="Area1" XValueType="Double" Name="Series1" ChartType="Pie"
                                Font="Trebuchet MS, 8.25pt, style=Bold" CustomProperties="DoughnutRadius=25, PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20"
                                MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                                Label="#PERCENT{P1}">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="Area1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                BackColor="Transparent" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY2>
                                    <MajorGrid Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisY2>
                                <AxisX2>
                                    <MajorGrid Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisX2>
                                <Area3DStyle PointGapDepth="900" Rotation="162" IsRightAngleAxes="False" WallWidth="25"
                                    IsClustered="False" />
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </Content>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Fechar">
                <Listeners>
                    <Click Handler="#{winGrafico}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfVisualizarPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarResultadosPesquisas" Text="0"/>
</asp:Content>
