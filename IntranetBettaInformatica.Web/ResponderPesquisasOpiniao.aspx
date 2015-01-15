<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ResponderPesquisasOpiniao.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.ResponderPesquisasOpiniao" Title="Responder Pesquisas de Opinião" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getPesquisasOpiniao() {
            var array = [];
            var index = 0;
            var frmPesquisas = Ext.getCmp('ctl00_cph_body_frmPesquisas');
            for (var i = 0; i < frmPesquisas.items.length; i++) {
                var fds = frmPesquisas.items.items[i];
                for (var j = 0; j < fds.items.length; j++) {
                    var hdf = fds.items.items[0];
                    var group = fds.items.items[1];
                    if (group.disabled)
                        continue;

                    array[index] = { Id: parseInt(hdf.value), Respostas: [] };
                    var indexResposta = 0;
                    for (var x = 0; x < group.items.length; x++) {
                        var radio = group.items.items[x];
                        var idRadio = radio.id.toString();
                        array[index].Respostas[indexResposta] = { Id: parseInt(idRadio.substring(idRadio.lastIndexOf('_') + 1)), Usuarios: [] };
                        if (radio.checked) {
                            array[index].Respostas[indexResposta].Usuarios[0] = { Id: 0 };
                        }
                        indexResposta++;
                    }
                }
                index++;
            }
            return Ext.encode(array);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" ID="frmTitulo" AnchorVertical="100%" AnchorHorizontal="100%" Layout="Border">
                <Items>
                    <ext:FormPanel ID="frmBusca" runat="server" Region="North" Title="Busca" Collapsible="true"
                        ButtonAlign="Left" Collapsed="false" Frame="true" Height="130">
                        <Items>
                            <ext:CheckboxGroup runat="server" FieldLabel="Status" Width="450">
                                <Items>
                                    <ext:Checkbox runat="server" ID="chkAtivo" BoxLabel="Ativa" HideLabel="true" Width="100">
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
                                    <Click Handler="#{frmPesquisas}.removeAll(); return #{frmBusca}.validate();" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="btnBuscar_Click">
                                        <EventMask Msg="Buscando Pesquisas de Opinião..." Target="Page" ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                    <ext:Panel runat="server" Border="false" Frame="true" Region="Center">
                        <Items>
                            <ext:Label ID="lblNenhumaPesquisa" runat="server" Width="400" Hidden="true">
                                Nenhuma pesquisa encontrada para o filtro informado.</ext:Label>
                            <ext:FormPanel ID="frmPesquisas" runat="server" Frame="true" Border="false">
                                <Items>
                                </Items>
                                <Buttons>
                                    <ext:Button runat="server" Text="Salvar" ID="btnSalvar" Icon="Disk">
                                        <Listeners>
                                            <Click Handler="return #{frmPesquisas}.validate();" />
                                        </Listeners>
                                        <DirectEvents>
                                            <Click OnEvent="Salvar_Click">
                                                <EventMask Msg="Salvando respostas..." ShowMask="true" Target="Page" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="questoes" Value="getPesquisasOpiniao()" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Buttons>
                            </ext:FormPanel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:FitLayout>
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
	<ext:Hidden runat="server" ID="hdfVisualizarResponderPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfSalvarResponderPesquisas" Text="0"/>
	<ext:Hidden runat="server" ID="hdfVisualizarGraficoResponderPesquisas" Text="0"/>
</asp:Content>
