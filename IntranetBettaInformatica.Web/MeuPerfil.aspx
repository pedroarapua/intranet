<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MeuPerfil.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.MeuPerfil" Title="Meu Perfil" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
	<style type="text/css">
		.x-form-check-wrap
		{
			white-space:nowrap;
			text-align:center;
			zoom:1;
		}
	</style>
    <script type="text/javascript">
        function validarUsuario() {
            var frm = Ext.getCmp('ctl00_cph_body_frmTitulo');
            var txtSenha = Ext.getCmp('ctl00_cph_body_txtSenha');
            var txtConfSenha = Ext.getCmp('ctl00_cph_body_txtConfSenha');
            if (txtSenha.getValue() != txtConfSenha.getValue()) {
                alert('Erro', 'Senha e confirmação de senha não conferem.');
                return false;
            }
            return frm.validate();
        }
        function fnNivelConhecimento(value, col, item) {
        	var strRetorno = '';
        	switch (item.data.NivelConhecimentoId) {
        		case 1: strRetorno = 'Nenhum'; break;
        		case 2: strRetorno = 'Básico'; break;
        		case 3: strRetorno = 'Intermediario'; break;
        		case 4: strRetorno = 'Avancado'; break;
        	}
        	return strRetorno;
        }
        function getConhecimentos(array) {
        	var args = [];
        	for (var i = 0; i < array.length; i++) {
        		args[args.length] = { Comprovavel : array[i].Comprovavel, Conhecimento :{ Id: array[i].ConhecimentoId }, NivelConhecimento : array[i].NivelConhecimentoId, Id : array[i].Id };
        	}
        	return args;
        }
    </script>
    <style type="text/css">
        .icon-combo-item
        {
            background-repeat: no-repeat !important;
            background-position: 3px 50% !important;
            padding-left: 24px !important;
        }
        .x-form-file-btn
        {
            z-index:1000 !important;
        }
        .x-form-file
        {
            z-index:1001 !important;
        }
            
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
	<ext:Store ID="strConhecimentos" runat="server" GroupField="Topico">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo"  ServerMapping="Conhecimento.Titulo"/>
					<ext:RecordField Name="ConhecimentoId"  ServerMapping="Conhecimento.Id"/>
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
    <ext:Store ID="strPaginas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Descricao" />
                    <ext:RecordField Name="IconeCls" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <DirectEventConfig>
            <EventMask ShowMask="true" Target="Page" Msg="Carregando..." />
        </DirectEventConfig>
    </ext:Store>
    <ext:Store ID="strTemas" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Nome" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
	<ext:Store ID="strNiveisConhecimento" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Titulo" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
	<ext:Store ID="strEstados" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Sigla" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" Frame="true" ID="frmTitulo" AnchorVertical="100%" BodyStyle="padding:10px;"
                AutoScroll="true" LabelWidth="130">
				<TopBar>
					<ext:Toolbar runat="server">
						<Items>
							<ext:Button ID="Button1" runat="server" Text="Salvar Informações" Icon="Disk">
								<Listeners>
									<Click Handler="return validarUsuario();" />
								</Listeners>
								<DirectEvents>
									<Click OnEvent="Salvar_Click">
										<EventMask ShowMask="true" Target="Page" />
									</Click>
								</DirectEvents>
							</ext:Button>
							<ext:Button runat="server" ID="btnConhecimentos" Text="Meus Conhecimentos" Icon="Vcard">
								<DirectEvents>
									<Click OnEvent="btnConhecimentos_Click"></Click>
								</DirectEvents>
							</ext:Button>
						</Items>
					</ext:Toolbar>
				</TopBar>
                <Items>
                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true" AnchorHorizontal="100%">
                        <Columns>
                            <ext:LayoutColumn ColumnWidth="0.5">
                                <ext:Panel ID="Panel1" runat="server">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtNome" MaxLength="200" AllowBlank="false" FieldLabel="Nome"
                                            MsgTarget="Side" Width="450">
                                        </ext:TextField>
                                        <ext:Label runat="server" ID="lblEmpresa" Width="450" FieldLabel="Empresa">
                                        </ext:Label>
                                        <ext:Label runat="server" ID="lblSetor" Width="450" FieldLabel="Setor">
                                        </ext:Label>
                                        <ext:DateField runat="server" ID="txtDataNascimento" Width="300" FieldLabel="Data de Nasc."
                                            Format="dd/MM/yyyy">
                                        </ext:DateField>
                                        <ext:TextField runat="server" ID="txtTwitter" FieldLabel="Twitter" Width="300"></ext:TextField>
                                        <ext:ComboBox runat="server" ID="cboTema" Editable="true" EmptyText="Selecione o Tema..."
                                            Width="450" FieldLabel="Tema" DisplayField="Nome" ValueField="Id" AllowBlank="false"
                                            StoreID="strTemas" MsgTarget="Side">
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="cboPaginas" runat="server" Editable="true" EmptyText="Selecione a Pagina Inicial..."
                                            Width="450" AllowBlank="false" MsgTarget="Side" FieldLabel="Página Inicial" DisplayField="Descricao"
                                            ValueField="Id" StoreID="strPaginas" Mode="Local" TriggerAction="All">
                                            <Template ID="Template1" runat="server">
                                                <Html>
                                                    <tpl for=".">
                                                    <div class="x-combo-list-item icon-combo-item {IconeCls}">
                                                        {Descricao}
                                                    </div>
                                                </tpl>
                                                </Html>
                                            </Template>
                                            <Listeners>
                                                <Select Handler="this.setIconCls(record.get('IconeCls'));" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:FieldSet ID="FieldSet1" runat="server" Title="Dados de Acesso" Width="500" LabelWidth="120">
                                            <Items>
                                                <ext:Label runat="server" ID="lblPerfilAcesso" FieldLabel="Perfil de Acesso" Width="400">
                                                </ext:Label>
                                                <ext:TextField runat="server" ID="txtLogin" MaxLength="100" Width="300" FieldLabel="Login"
                                                    AllowBlank="false" MsgTarget="Side">
                                                </ext:TextField>
                                                <ext:TextField runat="server" ID="txtSenha" MaxLength="20" Width="300" FieldLabel="Senha"
                                                    MsgTarget="Side" InputType="Password">
                                                </ext:TextField>
                                                <ext:TextField runat="server" ID="txtConfSenha" MaxLength="20" Width="300" FieldLabel="Conf. de Senha"
                                                    MsgTarget="Side" InputType="Password">
                                                </ext:TextField>
                                                <ext:TextField runat="server" ID="txtPalavraChave" MaxLength="20" Width="300" FieldLabel="Palavra Chave"
                                                    MsgTarget="Side" InputType="Password">
                                                </ext:TextField>
                                            </Items>
                                        </ext:FieldSet>
                                    </Items>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.5">
                                <ext:Panel ID="Panel2" runat="server">
                                    <Items>
                                        <ext:TextField runat="server" ID="txtEndereco" MaxLength="200" Width="400" FieldLabel="Endereço">
                                        </ext:TextField>
                                        <ext:TextField runat="server" ID="txtCidade" MaxLength="100" Width="400" FieldLabel="Cidade">
                                        </ext:TextField>
                                        <ext:ComboBox runat="server" ID="cboEstado" Editable="true" EmptyText="Selecione o Estado..."
                                            Width="300" FieldLabel="Estado" DisplayField="Sigla" ValueField="Id" StoreID="strEstados">
                                        </ext:ComboBox>
                                        <ext:TextField runat="server" ID="txtEmail" MaxLength="100" Width="400" FieldLabel="Email" Vtype="email" AllowBlank="false" MsgTarget="Side">
                                        </ext:TextField>
                                        <ext:FileUploadField runat="server" ID="fufImagem" ButtonOnly="false" FieldLabel="Foto" Width="400" EmptyText="Selecione uma imagem..."
                                            Icon="ImageAdd" ButtonText="">
                                        </ext:FileUploadField>
                                        <ext:Image runat="server" ID="imgAtual" FieldLabel="Imagem Atual" Width="250" StyleSpec="height:120px;">
                                        </ext:Image>
                                    </Items>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </Columns>
                    </ext:ColumnLayout>
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
                                <ext:Column Header="Tópico" DataIndex="Topico"	 MenuDisabled="true" Sortable="false" Hideable="false"></ext:Column>
								<ext:Column Header="Título" DataIndex="Titulo" MenuDisabled="true" Sortable="false" Hideable="false"></ext:Column>
								<ext:Column Header="Conhecimento" DataIndex="NivelConhecimentoId" MenuDisabled="true" Sortable="false" Hideable="false" Align="Center" Width="30">
									<Renderer Fn="fnNivelConhecimento" />
									<Editor>
                                        <ext:ComboBox ID="cboNivelConhecimento" runat="server" AllowBlank="false" Editable="false" SelectOnFocus="true" DisplayField="Titulo" ValueField="Id" HideLabel="true" StoreID="strNiveisConhecimento">
                                        </ext:ComboBox>
                                    </Editor>
								</ext:Column>
								<ext:Column Header="Comprovável?" DataIndex="Comprovavel" MenuDisabled="true" Sortable="false" Hideable="false" Align="Center" Width="30" >
									<Renderer Handler="return value == 1 ? 'Sim' : 'Não';" />
									<Editor>
										<ext:Checkbox ID="ckbComprovavel" runat="server" HideLabel="true"></ext:Checkbox>
									</Editor>
								</ext:Column>
                            </Columns>
                        </ColumnModel>
						<Plugins>
                            <ext:EditableGrid runat="server" />
                        </Plugins>
                        <View>
							<ext:GroupingView HideGroupedColumn="true" runat="server" ForceFit="true" StartCollapsed="false" EnableRowBody="true">
                            </ext:GroupingView>
                        </View>
                    </ext:GridPanel>
                </Items>
            </ext:FitLayout>
        </Items>
        <Buttons>
            <ext:Button ID="btnSalvarConhecimentos" runat="server" Text="Salvar" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="SalvarConhecimentos_Click">
                        <ExtraParams>
                            <ext:Parameter Name="conhecimentos" Value="Ext.encode(getConhecimentos(#{grdConhecimentos}.getRowsValues()))" Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                        <EventMask ShowMask="true" Target="Page" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button runat="server" Text="Cancelar" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{winConhecimentos}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
</asp:Content>
