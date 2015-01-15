<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarNoticias.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarNoticias" Title="Notícias" ValidateRequest="false" %>

<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
    	function selectRow(row) {
    		var hdfEditarNoticiasValue = Ext.getCmp('ctl00_cph_body_hdfEditarNoticias').getValue();
    		var hdfVisualizarHtmlNoticiasValue = Ext.getCmp('ctl00_cph_body_hdfVisualizarHtmlNoticias').getValue();
    		var hdfRemoverNoticiasValue = Ext.getCmp('ctl00_cph_body_hdfRemoverNoticias').getValue();
            var length = Ext.getCmp('ctl00_cph_body_grdNoticias').selModel.selections.length;
            var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
            var btnViewHtml = Ext.getCmp('ctl00_cph_body_btnViewHtml');
            var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
            btnRemover.setDisabled(length == 0 || hdfRemoverNoticiasValue == '0');
            btnEditar.setDisabled(length == 0 || hdfEditarNoticiasValue == '0');
            btnViewHtml.setDisabled(length == 0 || hdfVisualizarHtmlNoticiasValue == '0');
        }
        function removerUsuarios() {
            Ext.getCmp('ctl00_cph_body_grdUsuarios').deleteSelected();
            rowSelectUsuario(null);
        }
        function rowSelectUsuario(row) {
            var data = !row ? null : row.selections.items.length == 0 ? null : row.selections.items[0].data;
            var disabled = !row || !data;
            Ext.getCmp('ctl00_cph_body_btnRemoverUsuarios').setDisabled(disabled);
        }
        function validarNoticia() {
        	var frmNoticia = Ext.getCmp('ctl00_cph_body_frmNoticia');
            var tabNoticia = Ext.getCmp('ctl00_cph_body_tabNoticia');
            var usuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios').getRowsValues();
            if (!frmNoticia.validate()) {
                tabNoticia.setActiveTab(0);
                return false;
            }
            else if (usuarios.length == 0) {
                alert('Usuários', 'Pelo menos 1 usuário deve ser associado a notícia.');
                tabNoticia.setActiveTab(1);
                return false;
            }
            return true;
        }
        function confirmarUsuarios() {
            var win = Ext.getCmp('ctl00_cph_body_winAdicionarUsuarios');
            var grdUsuarios = Ext.getCmp('ctl00_cph_body_grdUsuarios');
            var grdUsuariosPesq = Ext.getCmp('ctl00_cph_body_grdUsuariosPesquisa');
            var usuariosAdd = grdUsuariosPesq.getRowsValues({ selectedOnly: true });

            for (var i = 0; i < usuariosAdd.length; i++) {
                grdUsuarios.insertRecord(0, usuariosAdd[i]);
            }
            grdUsuarios.selModel.clearSelections();
            rowSelectUsuario(null);
            win.hide();
        }
        function getUsuarios(array) {
            var args = [];
            for (var i = 0; i < array.length; i++)
                args[i] = { Id: array[i].Id };
            return args;
           }
		function fnEditarClick(a,b,c)
		{
			if(b.extraParamsResponse && b.extraParamsResponse.html)
				CKEDITOR.instances.ctl00_cph_body_txtHtml.setData(b.extraParamsResponse.html); 
		}
    </script>
	<style type="text/css">
		#ctl00_cph_body_Container2_Content{
			height: 95% !important;
		}
		span.cke_wrapper cke_ltr,table.cke_editor, td.cke_contents, span.cke_skin_kama, span.cke_wrapper, span.cke_browser_webkit{
			height: 98%!important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strNoticias" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="DataInicial" Type="Date" />
                    <ext:RecordField Name="DataFinal" Type="Date" />
                    <ext:RecordField Name="DataPeriodo" />
                    <ext:RecordField Name="Usuarios" />
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
             <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%"
                Layout="Border">
                <Items>
                    <ext:FormPanel runat="server" ID="frmBusca" Title="Busca" Collapsed="false" Border="false"
                        Height="130" Collapsible="true" Region="North" Frame="true" ButtonAlign="Left">
                        <Items>
                            <ext:CheckboxGroup ID="chkGroupStatus" runat="server" FieldLabel="Status" Width="450">
                                <Items>
                                    <ext:Checkbox runat="server" ID="chkAtiva" BoxLabel="Aguardando" HideLabel="true" Width="100" Checked="true">
                                    </ext:Checkbox>
                                    <ext:Checkbox runat="server" ID="chkIniciada" BoxLabel="Iniciada" Checked="true"
                                        HideLabel="true" Width="100">
                                    </ext:Checkbox>
                                    <ext:Checkbox runat="server" ID="chkFinalizada" BoxLabel="Finalizada" HideLabel="true"
                                        Width="100">
                                    </ext:Checkbox>
                                </Items>
                            </ext:CheckboxGroup>
                            <ext:Container runat="server" Height="50" Layout="Column" AnchorHorizontal="92%">
                                <Items>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataInicialBusca" FieldLabel="Período" Editable="true"
                                                Vtype="daterange" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="#{txtDataFinal}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataFinalBusca" Editable="true" Vtype="daterange"
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
                                        <EventMask Msg="Buscando Notícias..." Target="Page" ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdNoticias" runat="server" StoreID="strNoticias"
                        Frame="true" AnchorHorizontal="100%" Layout="Fit"
                        Region="Center">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnNovo" Text="Nova" Icon="Add">
										<Listeners>
											<Click Handler="CKEDITOR.instances.ctl00_cph_body_txtHtml.setData('');" />
										</Listeners>
                                        <DirectEvents>
                                            <Click OnEvent="btnNovo_Click">
                                                <EventMask ShowMask="true" Target="Page" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnEditar" Text="Editar" Icon="NoteEdit" Disabled="true">
                                        <Listeners>
											<Click Handler="CKEDITOR.instances.ctl00_cph_body_txtHtml.setData('');" />
										</Listeners>
										<DirectEvents>
											<Click OnEvent="btnEditar_Click" Success="fnEditarClick">
                                                <EventMask ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdNoticias}.getRowsValues({selectedOnly:true})[0].Id">
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
                                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdNoticias}.getRowsValues({selectedOnly:true})[0].Id">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta notícia?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									<ext:Button runat="server" ID="btnViewHtml" Text="HTML" Icon="Html" Disabled="true">
										<Listeners>
											<Click Handler="Ext.net.DirectMethods.OpenNoticia(Ext.getCmp('ctl00_cph_body_grdNoticias').getRowsValues({selectedOnly:true})[0].Id);" />
										</Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
								<ext:Column Header="Título" Width="200" DataIndex="Titulo" />
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
                        <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true"></ext:GridView>
                        </View>
                     	<Listeners>
							<RowDblClick Handler="CKEDITOR.instances.ctl00_cph_body_txtHtml.setData(''); return Ext.getCmp('ctl00_cph_body_hdfEditarNoticias').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <RowDblClick OnEvent="btnEditar_Click" Success="fnEditarClick">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="id" Mode="Raw" Value="#{grdNoticias}.getRowsValues({selectedOnly:true})[0].Id">
                                    </ext:Parameter>
                                </ExtraParams>
                            </RowDblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strNoticias"></ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winNoticia" Icon="Lightning" Maximized="true"
        Modal="true" Hidden="true" Maximizable="false">
        <Items>
            <ext:FitLayout runat="server">
                <Items>
                    <ext:TabPanel runat="server" ID="tabNoticia" ActiveTabIndex="0" Border="false">
                        <Items>
                            <ext:Panel runat="server" Title="Notícia" Border="false" Layout="fit">
                                <Items>
                                    <ext:FormPanel runat="server" Frame="true" ID="frmNoticia" AnchorVertical="100%">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtTitulo" AnchorHorizontal="96%" FieldLabel="Título" AllowBlank="false" MaxLength="1000" MsgTarget="Side">
                                            </ext:TextField>
                                           <ext:Container ID="Container1" runat="server" Height="40" Width="500" Layout="ColumnLayout">
                                                <Items>
                                                    <ext:Container runat="server" Layout="Form" Width="240">
                                                        <Items>
                                                            <ext:DateField runat="server" ID="txtDataInicial" FieldLabel="Início" Editable="true"
                                                                AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                            </ext:DateField>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container runat="server" Layout="Form" Width="240">
                                                        <Items>
                                                            <ext:DateField runat="server" ID="txtDataFinal" FieldLabel="Término" Editable="true"
                                                                AllowBlank="false" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                            </ext:DateField>
                                                        </Items>
                                                    </ext:Container>
                                                </Items>
                                            </ext:Container>
											<ext:Container ID="Container2" runat="server" AnchorHorizontal="100%" AnchorVertical="100%">
												<Content>
													<CKEditor:CKEditorControl ID="txtHtml" BasePath="~/ckeditor" runat="server"></CKEditor:CKEditorControl>
												</Content>
											</ext:Container>
                                        </Items>
                                    </ext:FormPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="tabUsuarios" runat="server" Title="Associar Usuários" Border="false"
                                AutoRender="false">
                                <Items>
                                    <ext:FitLayout runat="server">
                                        <Items>
                                            <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios" Frame="true"
                                                Layout="fit" AutoExpandColumn="colNome" AnchorHorizontal="100%" AnchorVertical="100%">
                                                <TopBar>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:Button runat="server" Text="Adicionar" Icon="Add">
                                                                <DirectEvents>
                                                                    <Click OnEvent="btnAdicionarUsuarios_Click">
                                                                        <EventMask ShowMask="true" Target="Page" />
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="btnRemoverUsuarios" runat="server" Text="Remover" Disabled="true"
                                                                Icon="Delete">
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
                                                            <SelectionChange Fn="rowSelectUsuario" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <View>
                                                    <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="true"
                                                        EnableRowBody="true">
                                                    </ext:GroupingView>
                                                </View>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:FitLayout>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                </Items>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return validarNoticia()" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click">
                        <ExtraParams>
                            <ext:Parameter Name="usuarios" Value="Ext.encode(getUsuarios(#{grdUsuarios}.getRowsValues()))"
                                Mode="Raw">
                            </ext:Parameter>
							<ext:Parameter Name="html" Value="escape(CKEDITOR.instances.ctl00_cph_body_txtHtml.getData())" Mode="Raw"></ext:Parameter>
                        </ExtraParams>
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winNoticia}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window runat="server" ID="winAdicionarUsuarios" Icon="User" Width="550" Height="400"
        Modal="true" Hidden="true" Maximizable="true" Title="Adicionando Usuários a Notícia"
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
	<ext:Hidden runat="server" ID="hdfVisualizarNoticias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarNoticias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarNoticias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverNoticias" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarHtmlNoticias" Text="0"/>
</asp:Content>
