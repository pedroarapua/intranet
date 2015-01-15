<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="BuscarPerfilConhecimento.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.BuscarPerfilConhecimento" Title="Buscar Perfil de Conhecimento" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function selectRow(row) {
        	var length = Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.length;
        	var hdfVisualizarConhecimentoUsuarioBuscaPerfilValue = Ext.getCmp('ctl00_cph_body_hdfVisualizarConhecimentoUsuarioBuscaPerfil').getValue();
        	var data = length > 0 ? Ext.getCmp('ctl00_cph_body_grdUsuarios').selModel.selections.items[0].data : null;
        	var btnConhecimentos = Ext.getCmp('ctl00_cph_body_btnConhecimentos');
        	btnConhecimentos.setDisabled(length == 0 || hdfVisualizarConhecimentoUsuarioBuscaPerfilValue == '0');
        }
        var getValues = function (tree) {
        	var msg = [],
                selNodes = tree.getChecked();

        	Ext.each(selNodes, function (node) {
        		if (selNodes.length > 1) {
					if(!node.attributes.hidden)
        				msg.push(node.id);
        		}
        		else {
        			msg.push(node.id);
        		}
        	});

        	return msg.join(",");
        };

        var getText = function (tree) {
        	var msg = [],
                selNodes = tree.getChecked();
        	msg.push("[");

        	var cont = 0;
        	Ext.each(selNodes, function (node) {
        		if (!node.attributes.hidden)
        			cont++;
        	});
        	var index = 0;
        	Ext.each(selNodes, function (node) {
        		index++;
        		if (cont > 1 && msg.length > 1 && !node.attributes.hidden) {
        			msg.push(",");
        		}
        		if (cont > 0) {
        			if (!node.attributes.hidden)
        				msg.push(node.text);
        		}
        		else
        			msg.push(node.text);
        	});

        	msg.push("]");
        	//msg = msg.replace(',]', ']');
        	return msg.join("");
        };

        var getValuesId = function (tree) {
        	var array = [],
            selNodes = tree.getChecked();

        	Ext.each(selNodes, function (node) {
        		if (selNodes.length > 1) {
        			if (!node.attributes.hidden)
        				array.push(node.id);
        		}
        		else {
        			array.push(node.id);
        		}
        	});

        	return array;
        };
            
        var syncValue = function(value){
            var tree = this.component;
                
            if (tree.rendered){
                var ids = value.split(",");
                tree.setChecked({ids: ids, silent: true});
                   
                tree.getSelectionModel().clearSelections();
                Ext.each(ids, function(id){
                    var node = tree.getNodeById(id);      
                       
                    if (node) {
                        node.ensureVisible(function () {
                        tree.getSelectionModel().select(tree.getNodeById(this.id), null, true);
                        }, node);
                    }
                }, this);
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strConhecimentos" runat="server" GroupField="Topico">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo"  ServerMapping="Conhecimento.Titulo"/>
					<ext:RecordField Name="Topico" ServerMapping="Conhecimento.Topico.Titulo" />
                    <ext:RecordField Name="Comprovavel" Type="Boolean" />
					<ext:RecordField Name="NivelConhecimentoId" />
					<ext:RecordField Name="NivelConhecimentoDescricao" />
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
							<ext:Container runat="server" Layout="Column" >
								<Items>
									<ext:Container runat="server" ColumnWidth="0.8" Layout="Form" >
										<Items>
											<ext:DropDownField ID="ddfBusca" runat="server" Editable="false" AnchorHorizontal="98%" TriggerIcon="SimpleArrowDown" Mode="ValueText" FieldLabel="Selecionar Busca" UnderlyingValue="Nenhuma Seleção" Text="[Nenhuma Seleção]">
												<Component>
													<ext:TreePanel ID="treePerfilConhecimento" 
														runat="server" 
														Title="Base de Conhecimentos"
														Icon="Lightbulb"
														Height="500"
														Shadow="None"
														UseArrows="true"
														AutoScroll="true"
														Animate="true"
														EnableDD="true"
														ContainerScroll="true"
														RootVisible="false"
														>
														<Buttons>
															<ext:Button ID="Button1" runat="server" Text="Fechar">
																<Listeners>
																	<Click Handler="#{ddfBusca}.collapse();" />
																</Listeners>
															</ext:Button>
														</Buttons>
														<Listeners>
															<CheckChange Handler="this.dropDownField.setValue(getValues(this), getText(this), false);" />
														</Listeners>
														<SelectionModel>
															<ext:MultiSelectionModel ID="MultiSelectionModel1" runat="server" />
														</SelectionModel>      
													 </ext:TreePanel>
												</Component>
												<Listeners>
													<Expand Handler="this.component.getRootNode().expand(true);" Single="true" Delay="20" />
												</Listeners>
												<SyncValue Fn="syncValue" />
											</ext:DropDownField>
										</Items>
									</ext:Container>
									<ext:Container runat="server" ColumnWidth="0.2" Height="30">
										<Items>
											<ext:Button Text="Buscar" Icon="Zoom" runat="server">
												<DirectEvents>
													<Click OnEvent="btnBuscar_Click">
														<ExtraParams>
															<ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(getValuesId(#{treePerfilConhecimento}))"></ext:Parameter>
														</ExtraParams>
														<EventMask Msg="Buscando Perfil de Conhecimento..." Target="Page" ShowMask="true" />
													</Click>
												</DirectEvents>
											</ext:Button>
										</Items>
									</ext:Container>
								</Items>
							</ext:Container>
						</Items>
                    </ext:FormPanel>
                    <ext:GridPanel ID="grdUsuarios" runat="server" StoreID="strUsuarios" Frame="true" AutoExpandColumn="Nome" AnchorHorizontal="100%" Layout="Fit" Region="Center">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnConhecimentos" Text="Visualizar Conhecimentos" Icon="Vcard" Disabled="true">
										<DirectEvents>
											<Click OnEvent="btnConhecimentos_Click">
												<ExtraParams>
													<ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id"></ext:Parameter>
												</ExtraParams>
											</Click>
										</DirectEvents>
									</ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column ColumnID="colNome" Header="Nome" DataIndex="Nome" Groupable="false" />
                                <ext:Column Header="Cidade" DataIndex="Cidade" Width="120"/>
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
						<DirectEvents>
							<DblClick OnEvent="btnConhecimentos_Click">
								<ExtraParams>
									<ext:Parameter Name="id" Mode="Raw" Value="#{grdUsuarios}.getRowsValues({selectedOnly:true})[0].Id"></ext:Parameter>
								</ExtraParams>
							</DblClick>
						</DirectEvents>
                        <View>
                            <ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false" EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strUsuarios" HideRefresh="true">
							</ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winConhecimentos" Width="800" Height="600" Modal="true"
        Title="Conhecimentos do Usuário" Hidden="true" Icon="Vcard">
        <Items>
            <ext:FitLayout runat="server">
                <Items>
                    <ext:GridPanel runat="server" ID="grdConhecimentos" StoreID="strConhecimentos" DisableSelection="true" AutoExpandColumn="Titulo" AutoScroll="true">
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:RowNumbererColumn />
                                <ext:Column Header="Tópico" DataIndex="Topico"></ext:Column>
								<ext:Column Header="Título" DataIndex="Titulo"></ext:Column>
								<ext:Column Header="Conhecimento" DataIndex="NivelConhecimentoDescricao" Align="Center" Width="30">
								</ext:Column>
								<ext:Column Header="Comprovável?" DataIndex="Comprovavel" Align="Center" Width="30" >
									<Renderer Handler="return value == 1 ? 'Sim' : 'Não';" />
								</ext:Column>
                            </Columns>
                        </ColumnModel>
						<View>
							<ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false" EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Fechar">
                <Listeners>
                    <Click Handler="#{winConhecimentos}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <asp:HiddenField runat="server" ID="hdfUsuarioLogado" />
	<ext:Hidden runat="server" ID="hdfVisualizarBuscaPerfil" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarConhecimentoUsuarioBuscaPerfil" Text="0"/>
</asp:Content>
