<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarReunioes.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarReunioes" Title="Reuniões" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <ext:XScript ID="XScript1" runat="server">
        <script type="text/javascript">
            function validarForm()
            {
                var frm = Ext.getCmp('ctl00_cph_body_frmReuniao');
                var validateForm = frm.validate();
                var usuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios').getRowsValues();
                var tabReuniao = Ext.getCmp('ctl00_cph_body_tabReuniao');
                var cboSala = Ext.getCmp('ctl00_cph_body_cboSala');
                if (!validateForm) {
                    alert('Campos obrigatórios', 'Existem campos obrigatórios a serem preenchidos.');
                    tabReuniao.setActiveTab(0);
                    return false;
                }
                else if (cboSala.getValue() == '') {
                    alert('Campos obrigatórios', 'Selecione a sala, pressione o botão verificar para habilitar o campo.');
                    tabReuniao.setActiveTab(0);
                    return false;
                }
                else if (usuarios.length < 1) {
                    alert('Participantes', 'Pelo menos 1 participante deve fazer parte da reunião.');
                    tabReuniao.setActiveTab(1);
                    return false;
                }
                return true;
            }

            function confirmarUsuarios()
            {
                var win = Ext.getCmp('ctl00_cph_body_winAdicionarUsuarios');
                var grdUsuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios');
                var grdUsuariosReuniao = Ext.getCmp('ctl00_cph_body_grdUsuariosReuniao');
                var usuariosAdd = grdUsuariosReuniao.getRowsValues({selectedOnly:true});

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

            function selectRow(row) {
            	var hdfEditarReunioesValue = Ext.getCmp('ctl00_cph_body_hdfEditarReunioes').getValue();
            	var hdfRemoverReunioesValue = Ext.getCmp('ctl00_cph_body_hdfRemoverReunioes').getValue();
            	var hdfCancelarReunioesValue = Ext.getCmp('ctl00_cph_body_hdfCancelarReunioes').getValue();
                var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
                var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
                var btnCancelar = Ext.getCmp('ctl00_cph_body_btnCancelar');
                var length = Ext.getCmp('ctl00_cph_body_grdReunioes').selModel.selections.length;
                var naoCancelada = length > 0 && !row.selections.items[0].data.ECancelada;
                var status = length > 0 && !row.selections.items[0].data.Status;
                btnEditar.setDisabled(length == 0 || !naoCancelada || hdfEditarReunioesValue == '0');
                btnRemover.setDisabled(length == 0 || !naoCancelada || hdfRemoverReunioesValue == '0');
                btnCancelar.setDisabled(length == 0 || !naoCancelada || hdfCancelarReunioesValue == '0');
            }
            
            function getUsuarios(array)
            {
                var args = [];
                for(var i = 0; i < array.length; i++)
                    args[i] = { Id: array[i].Id, Nome: array[i].Nome, Email: array[i].Email };
                return args;
            }

            function getUsuariosAdicionados() {
                var array = Ext.getCmp('ctl00_cph_body_grdUsuarios').getRowsValues();
                var usuarios = [];
                for (var i = 0; i < array.length; i++)
                    usuarios[usuarios.length] = { Id: array[i].Id, Nome: array[i].Nome, Email: array[i].Email };

                return usuarios;
            }

            function verificaPreenchimentoCamposField() {
                var txtDataInicial = Ext.getCmp('ctl00_cph_body_txtDataInicial');
                var txtDataFinal = Ext.getCmp('ctl00_cph_body_txtDataFinal');
                var txtHoraInicial = Ext.getCmp('ctl00_cph_body_txtHoraInicial');
                var txtHoraFinal = Ext.getCmp('ctl00_cph_body_txtHoraFinal');

                var btnSala = Ext.getCmp('ctl00_cph_body_cboSala');
                btnSala.clearValue();
                btnSala.clearInvalid();
                btnSala.setDisabled(true);

                Ext.getCmp('ctl00_cph_body_btnVerificarSala').setDisabled(txtHoraInicial.getValue() == '' || txtDataFinal.getValue() == '' || txtHoraInicial.getValue() == '' || txtHoraFinal.getValue() == '');
            }
            
        </script>
    </ext:XScript>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strReunioes" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Codigo" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="DataInicial" Type="Date" />
                    <ext:RecordField Name="DataFinal" Type="Date" />
                    <ext:RecordField Name="ECancelada" Type="Boolean" />
                    <ext:RecordField Name="Status" />
                    <ext:RecordField Name="StatusIcon" />
                    <ext:RecordField Name="SalaReuniao" ServerMapping="SalaReuniao.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="SalaReuniao.Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strUsuarios" runat="server" GroupField="Empresa">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Email" />
                    <ext:RecordField Name="Empresa" ServerMapping="Empresa.Nome" />
                    <ext:RecordField Name="Setor" ServerMapping="Setor.Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="strSalas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strUsuariosReuniao" runat="server" GroupField="Setor">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Email" />
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
                    <ext:GridPanel ID="grdReunioes" runat="server" StoreID="strReunioes" Frame="true"
                        AutoExpandColumn="Titulo" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdReunioes}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdReunioes}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta reunião?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button runat="server" ID="btnCancelar" Text="Cancelar" Icon="Cancel"
                                        Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnCancelar_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdReunioes}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja cancelar esta reunião?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:TemplateColumn DataIndex="Status" Header=" " Width="75" Hideable="false" Resizable="false">
                                    <Template ID="Template1" runat="server">
                                        <Html>
                                            <div class="{StatusIcon}" style="height:16px;width:16px;float:left;margin-right:5px;" title="{Status}" alt="{Status}"></div>
                                            <div style="padding-top:2px;">{Codigo}</div>
                                        </Html>
                                    </Template>
                                </ext:TemplateColumn>
                                <ext:Column ColumnID="Titulo" Header="Título" DataIndex="Titulo" Width="200" />
                                <ext:Column Header="Descrição" DataIndex="Descricao" Width="300" />
                                <ext:Column Header="Sala" DataIndex="SalaReuniao" Width="120" />
                                <ext:Column Header="Setor" DataIndex="Setor" Width="120" />
                                <ext:Column Header="Data Inicial" Width="120" DataIndex="DataInicial">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Data Final" Width="120" DataIndex="DataFinal">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarReunioes').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdReunioes}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strReunioes">
                                <Items>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator> 
                                    <ext:Label runat="server" Text="Legenda: " HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container runat="server" Cls="icon-controlplayblue" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label runat="server" Text="Iniciada" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container runat="server" Cls="icon-controlpauseblue" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label runat="server" Text="Aguardando" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container runat="server" Cls="icon-controlstopblue" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label runat="server" Text="Finalizada" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                    <ext:Container runat="server" Cls="icon-controlrecord" Height="16" Width="16">
                                    </ext:Container>
                                    <ext:Label runat="server" Text="Cancelada" HideLabel="true" StyleSpec="padding:0px 5px;color:white;"></ext:Label>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winReuniao" Width="550" Height="400" Icon="BookOpen"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:TabPanel ID="tabReuniao" runat="server" ActiveIndex="0">
                            <Items>
                                <ext:FormPanel runat="server" Frame="true" ID="frmReuniao" Title="Dados da Reunião"
                                    LabelWidth="70" AnchorVertical="100%">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtTitulo" MaxLength="500" AllowBlank="false"
                                            AnchorHorizontal="92%" FieldLabel="Título" MsgTarget="Side">
                                        </ext:TextField>
                                        <ext:TextArea runat="server" ID="txtDescricao" MaxLength="1000"
                                            AnchorHorizontal="92%" AnchorVertical="-120" FieldLabel="Descrição">
                                        </ext:TextArea>
                                        <ext:Container runat="server" Layout="Column" Height="60">
                                            <Items>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth=".7">
                                                    <Items>
                                                        <ext:DateField runat="server" ID="txtDataInicial" FieldLabel="Início" Editable="true"
                                                            AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                            <Listeners>
                                                                <Change Fn="verificaPreenchimentoCamposField" />
                                                            </Listeners>
                                                        </ext:DateField>
                                                        <ext:DateField runat="server" ID="txtDataFinal" FieldLabel="Término" Editable="true"
                                                            AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                            <Listeners>
                                                                <Change Fn="verificaPreenchimentoCamposField" />
                                                            </Listeners>
                                                        </ext:DateField>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth=".3">
                                                    <Items>
                                                        <ext:TimeField runat="server" ID="txtHoraInicial" AllowBlank="false" MsgTarget="Side"
                                                            Width="100" MinTime="00:00" MaxTime="23:59" Editable="true" ForceSelection="false"
                                                            SelectOnFocus="false" Increment="30" Format="HH:mm" HideLabel="true">
                                                            <Listeners>
                                                                <Change Fn="verificaPreenchimentoCamposField" />
                                                            </Listeners>
                                                        </ext:TimeField>
                                                        <ext:TimeField runat="server" ID="txtHoraFinal" AllowBlank="false" MsgTarget="Side"
                                                            HideLabel="true" Width="100" MinTime="00:00" MaxTime="23:59" Editable="true"
                                                            ForceSelection="false" SelectOnFocus="false" Increment="30" Format="HH:mm">
                                                            <Listeners>
                                                                <Change Fn="verificaPreenchimentoCamposField" />
                                                            </Listeners>
                                                        </ext:TimeField>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                        <ext:Container runat="server" Layout="Column" Height="30">
                                            <Items>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth="0.7">
                                                    <Items>
                                                        <ext:ComboBox runat="server" ID="cboSala" Editable="true" Disabled="true" EmptyText="Selecione a Sala de Reunião..." AllowBlank="false" MsgTarget="Side"
                                                            AnchorHorizontal="92%" FieldLabel="Sala" DisplayField="Nome" ValueField="Id" StoreID="strSalas">
                                                        </ext:ComboBox>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Container runat="server" Layout="Form" ColumnWidth="0.3">
                                                    <Items>
                                                        <ext:Button ID="btnVerificarSala" Disabled="true" runat="server" Icon="Accept" Text="Verificar" ToolTip="Verificar">
                                                            <DirectEvents>
                                                                <Click OnEvent="VerificaSalasDisponiveis"></Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Container>
                                            </Items>
                                        </ext:Container>
                                    </Items>
                                </ext:FormPanel>
                                <ext:Panel runat="server" Title="Participantes" Border="false" Layout="fit" Icon="Group">
                                    <Items>
                                        <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuariosReuniao" Frame="true"
                                            Layout="fit" AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
                                            <TopBar>
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <ext:Button runat="server" Text="Adicionar" Icon="UserAdd">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnAdicionarUsuarios_Click">
                                                                    <EventMask ShowMask="true" Target="Page" />
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="usuariosAdicionados" Mode="Raw" Value="Ext.encode(getUsuariosAdicionados())"></ext:Parameter>
                                                                    </ExtraParams>
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
                                                    <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" Hideable="true" />
                                                    <ext:Column Header="Setor" DataIndex="Setor" Width="90">
                                                        <Renderer Handler="return !value || value == '' ? 'Nenhum' : value;" />
                                                    </ext:Column>
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
                            <ext:Parameter Name="usuarios" Value="Ext.encode(getUsuarios(#{grdUsuarios}.getRowsValues()))"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winReuniao}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winAdicionarUsuarios" Icon="User" Width="550" Height="400"
        Modal="true" Hidden="true" Maximizable="true" Title="Adicionando Usuários a Pesquisa"
        Layout="fit">
        <Items>
            <ext:GridPanel ID="grdUsuariosReuniao" runat="server" StoreID="strUsuarios"
                Frame="true" StripeRows="true" AutoExpandColumn="colNome">
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" Width="300" />
                        <ext:Column Header="Empresa" DataIndex="Empresa" Width="90" />
                        <ext:Column Header="Setor" DataIndex="Setor" Width="90" />
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
	<ext:Hidden runat="server" ID="hdfVisualizarReunioes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarReunioes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarReunioes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverReunioes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfCancelarReunioes" Text="0"/>
	<ext:Hidden runat="server" ID="VisualizarTodasReunioes" Text="0"/>
</asp:Content>
