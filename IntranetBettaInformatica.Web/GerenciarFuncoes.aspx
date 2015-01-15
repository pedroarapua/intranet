<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GerenciarFuncoes.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.GerenciarFuncoes" Title="Funções" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function selectRow(row) {
			var length = row == null ? 0 : Ext.getCmp('ctl00_cph_body_grdFuncoes').selModel.selections.length;
			var hdfEditarFuncoesValue = Ext.getCmp('ctl00_cph_body_hdfEditarFuncoes').getValue();
			var hdfRemoverFuncoesValue = Ext.getCmp('ctl00_cph_body_hdfRemoverFuncoes').getValue();
			var btnRemover = Ext.getCmp('ctl00_cph_body_btnRemover');
			var btnEditar = Ext.getCmp('ctl00_cph_body_btnEditar');
			btnRemover.setDisabled(length == 0 || hdfRemoverFuncoesValue == '0');
			btnEditar.setDisabled(length == 0 || hdfEditarFuncoesValue == '0');
		}
		function fnRetornoSalvar(a, b, c) {
			if (b.extraParamsResponse && b.extraParamsResponse.contemOrdem == '1') {
				confirm(
					"Atualização de Funções",
					"Já existe uma função com esta ordem, a confirmação desta operação atualizará as funções com ordem superior, deseja fazer isso?",
					function () { Ext.getCmp('ctl00_cph_body_btnSalvar').fireEvent('click'); }, function () { Ext.getCmp('ctl00_cph_body_hdfMensagem').setValue('0'); }
				);
			}
			else {
				alert('Função', 'Função gravada com sucesso', null, null);
			}
		}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strFuncoes" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="Ordem" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%">
                <Items>
                    <ext:GridPanel ID="grdFuncoes" runat="server" StoreID="strFuncoes" Frame="true" AutoExpandColumn="Descricao" AnchorHorizontal="100%" AnchorVertical="100%">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdFuncoes}.getRowsValues({selectedOnly:true}))">
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
                                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdFuncoes}.getRowsValues({selectedOnly:true}))">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                                <Confirmation ConfirmRequest="true" Title="Confirmação" Message="Deseja remover esta função?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
									
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Ordem" DataIndex="Ordem" Width="50" Align="Center" />
                                <ext:Column Header="Nome" DataIndex="Nome" Width="250" />
                                <ext:Column Header="Descrição" DataIndex="Descricao" />
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
							<RowDblClick Handler="return Ext.getCmp('ctl00_cph_body_hdfEditarFuncoes').getValue() == '1';" />
						</Listeners>
                        <DirectEvents>
                            <DblClick OnEvent="btnEditar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                                <ExtraParams>
                                    <ext:Parameter Name="valores" Mode="Raw" Value="Ext.encode(#{grdFuncoes}.getRowsValues({selectedOnly:true}))">
                                    </ext:Parameter>
                                </ExtraParams>
                            </DblClick>
                        </DirectEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strFuncoes" AnchorHorizontal="100%">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
    <ext:Window runat="server" ID="winFuncao" Icon="Theme" Width="450" Height="220"
        Modal="true" Hidden="true" Maximizable="true">
        <Items>
            <ext:AnchorLayout runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%" Vertical="100%">
                        <ext:FormPanel runat="server" Frame="true" ID="frmFuncao" AnchorVertical="100%">
                            <Items>
                                <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false" AnchorHorizontal="92%" FieldLabel="Nome" MsgTarget="Side"></ext:TextField>
                                <ext:TextArea runat="server" ID="txtDescricao" MaxLength="500" AnchorHorizontal="92%" FieldLabel="Descrição"></ext:TextArea>
                                <ext:NumberField runat="server" ID="txtOrdem" AllowDecimals="false" AllowNegative="false" AllowBlank="false" MsgTarget="Side" FieldLabel="Ordem" Width="100" InvalidText="O valor máximo permitido para o campo é" MinValue="1"></ext:NumberField>
                            </Items>
                        </ext:FormPanel>
                    </ext:Anchor>
                </Anchors>
            </ext:AnchorLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Salvar" ID="btnSalvar" Icon="Disk">
                <Listeners>
                    <Click Handler="return #{frmFuncao}.validate();" />
                </Listeners>
                <DirectEvents>
                    <Click OnEvent="Salvar_Click" Success="fnRetornoSalvar">
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winFuncao}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
	<ext:Hidden runat="server" ID="hdfMensagem" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarFuncoes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfAdicionarFuncoes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfEditarFuncoes" Text="0"/>
	<ext:Hidden runat="server" ID="hdfRemoverFuncoes" Text="0"/>
</asp:Content>
