<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VisualizarNoticias.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.VisualizarNoticias" Title="Notícias" ValidateRequest="false" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<style type="text/css">
        div.item-wrap {
            float : left;
            border : 1px solid transparent;
            margin : 5px 25px 5px 25px;
            width : 100px;
            cursor : pointer;
            height : 120px;
            text-align : center;
        }

        div.item-wrap img {
            margin : 5px 0px 0px 5px;
            width : 61px;
            height : 77px;
        }

        div.item-wrap h6 {
            font-size : 14px;
            color : #3A4B5B;
            font-family : tahoma,arial,san-serif;
        }

        .items-view .x-view-over { border : solid 1px silver; }

        #items-ct { padding : 0px 0px 0px 0px; }

        #items-ct h1 {
            border-bottom : 2px solid #3A4B5B;           
            cursor : pointer;                  
        }

        #items-ct h1 div {
            padding : 4px 4px 4px 0px;
            font-family : tahoma,arial,san-serif;
            font-size : 20px;
            /*color : #3A4B5B;*/
        }

        #items-ct .collapsed h2 div { background-position : 3px 3px; }
        #items-ct dl { margin-left : 2px; }
        #items-ct .collapsed dl { display : none; }
        .group-header { font-size:14px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:Store ID="strNoticias" runat="server" OnRefreshData="OnRefreshData">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                    <ext:RecordField Name="HTML" />
                    <ext:RecordField Name="DataInicial" Type="Date" />
                    <ext:RecordField Name="DataFinal" Type="Date" />
                    <ext:RecordField Name="DataPeriodo" />
                    <ext:RecordField Name="Status" />
                </Fields>
            </ext:JsonReader>
        </Reader>
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
                        Height="130" Collapsible="true" Region="North" Frame="true" ButtonAlign="Left">
                        <Items>
                            <ext:CheckboxGroup ID="chkGroupStatus" runat="server" FieldLabel="Status" Width="450">
                                <Items>
                                    <ext:Checkbox runat="server" ID="chkIniciada" BoxLabel="Iniciada" Checked="true"
                                        HideLabel="true" Width="100">
                                    </ext:Checkbox>
                                    <ext:Checkbox runat="server" ID="chkFinalizada" BoxLabel="Finalizada" HideLabel="true"
                                        Checked="false" Width="100">
                                    </ext:Checkbox>
                                </Items>
                            </ext:CheckboxGroup>
                            <ext:Container ID="Container1" runat="server" Height="50" Layout="Column" AnchorHorizontal="92%">
                                <Items>
                                    <ext:Container ID="Container2" runat="server" Layout="Form" ColumnWidth="0.3">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataInicialBusca" FieldLabel="Período" Editable="true"
                                                Vtype="daterange" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="#{txtDataFinal}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container3" runat="server" Layout="Form" ColumnWidth="0.2">
                                        <Items>
                                            <ext:DateField runat="server" ID="txtDataFinalBusca" Editable="true" Vtype="daterange"
                                                HideLabel="true" Format="dd/MM/yyyy" AnchorHorizontal="92%" MsgTarget="Side">
                                                <CustomConfig>
                                                    <ext:ConfigItem Name="startDateField" Value="#{txtDataInicial}" Mode="Value" />
                                                </CustomConfig>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container ID="Container4" runat="server" Layout="Form" ColumnWidth="0.5">
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Buttons>
                            <ext:Button ID="Button1" Text="Buscar" Icon="Zoom" runat="server" StyleSpec="padding-left:98px;">
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
					<ext:DataView ID="dtNoticias" runat="server" StoreID="strNoticias" AnchorHorizontal="100%" Layout="Fit" Region="Center" EmptyText="Nenhum notícia encontrada." ItemSelector="div.item-wrap" OverClass="x-view-over" >
						 <Template ID="Template2" runat="server">
							 <Html>
								<div id="items-ct">
									<tpl for=".">
										<h1><div>{Titulo}</div></h1>
										<div class="group-header">
											<h3 style="float:left;margin-bottom:10px;">Data:&nbsp;</h3><div style="float:left;margin-bottom:10px;">{DataPeriodo}</div><h3 style="float:left;margin-bottom:10px;">&nbsp;-&nbsp;Status:&nbsp;</h3><div style="margin-bottom:10px;">{Status}</div></span>
										</div>
										<div style="clear:right;">{HTML}</div>
										<hr></hr>
									</tpl>
								</div>
							</Html>
						</Template>
					</ext:DataView>
                    <%--<ext:GridPanel ID="grdNoticias" runat="server" StoreID="strNoticias" Frame="true" DisableSelection="true"
                        AutoExpandColumn="Titulo" AnchorHorizontal="100%" Layout="Fit" Region="Center">
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column Header="Título" ColumnID="Titulo" Width="200" DataIndex="Titulo" />
                                <ext:Column Header="Descrição" Width="200" DataIndex="Descricao" />
                                <ext:Column Header="Data Inicial" Width="120" DataIndex="DataInicial">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Data Final" Width="120" DataIndex="DataFinal">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('d/m/Y H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="Status" Width="100" DataIndex="Status" />
                            </Columns>
                        </ColumnModel>
                        <View>
                            <ext:GridView runat="server" ForceFit="true">
                            </ext:GridView>
                        </View>
                        <Plugins>
                            <ext:RowExpander ID="rowExpander" runat="server">
                                <Template ID="Template1" runat="server">
                                    <Html>
                                        <br />
                                        <table width="100%">
                                            <tr>
                                                <td width="40%">
                                                    <b>Titulo:</b> {Titulo}    
                                                </td>
                                                <td>
                                                    <b>Período:</b> {DataPeriodo}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>Descrição:</b> {Descricao}
                                                </td>
                                                <td>
                                                    <b>Status:</b> {Status}
                                                </td>
                                            </tr>
                                        </table>
                                    </Html>
                                </Template>
                            </ext:RowExpander>
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" PageSize="20" StoreID="strNoticias">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>--%>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
</asp:Content>
