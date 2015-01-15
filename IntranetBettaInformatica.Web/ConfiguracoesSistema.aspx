<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ConfiguracoesSistema.aspx.cs"
    Inherits="IntranetBettaInformatica.Web.ConfiguracoesSistema" Title="Configurações do Sistema" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head" runat="server">
    <script src="Js/Util.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validarCampos() {
            var form = Ext.getCmp('ctl00_cph_body_frmTitulo');
            var txtSenha = Ext.getCmp('ctl00_cph_body_txtSenha');
            var txtConfSenha = Ext.getCmp('ctl00_cph_body_txtConfSenha');
            var txtLogin = Ext.getCmp('ctl00_cph_body_txtLogin');

            if (txtSenha.getValue() != txtConfSenha.getValue()) {
                alert('Erro','Senha e confirmação de senha não conferem.');
                return false;
            }
            return form.validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_body" runat="server">
    <ext:FitLayout runat="server">
        <Items>
            <ext:FormPanel runat="server" Frame="true" ID="frmTitulo" AnchorVertical="100%">
                <Items>
                    <ext:TextField runat="server" ID="txtDescricao" MaxLength="100" AllowBlank="false"
                        AnchorHorizontal="40%" FieldLabel="Descrição" MsgTarget="Side">
                    </ext:TextField>
                    <ext:Checkbox FieldLabel="Possui Imagem" LabelSeparator="?" ID="ckbImagem" runat="server">
                        <Listeners>
                            <Check Handler="#{fufImagem}.setDisabled(!this.checked);" />
                        </Listeners>
                    </ext:Checkbox>
                    <ext:FileUploadField runat="server" ID="fufImagem" AnchorHorizontal="40%" FieldLabel="Imagem"
                        EmptyText="Selecione uma imagem...">
                    </ext:FileUploadField>
                    <ext:Image runat="server" ID="imgAtual" FieldLabel="Imagem Atual" Height="85" Width="140">
                    </ext:Image>
                    <ext:FieldSet runat="server" Title="Dados Smtp para Envio de Email" Width="500">
                        <Items>
                            <ext:TextField runat="server" ID="txtSmtp" MaxLength="50" Width="300" FieldLabel="Smtp">
                            </ext:TextField>
                            <ext:TextField runat="server" ID="txtLogin" MaxLength="50" Width="300" FieldLabel="Login">
                            </ext:TextField>
                            <ext:TextField runat="server" ID="txtSenha" MaxLength="20" Width="300" FieldLabel="Senha" InputType="Password">
                            </ext:TextField>
                            <ext:TextField runat="server" ID="txtConfSenha" MaxLength="20" Width="300" FieldLabel="Conf. de Senha" InputType="Password">
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:Button ID="btnSalvar" runat="server" Text="Salvar Configurações" Icon="Disk">
                        <Listeners>
                            <Click Handler="return validarCampos();" />
                        </Listeners>
                        <DirectEvents>
                            <Click OnEvent="Salvar_Click">
                                <EventMask ShowMask="true" Target="Page" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                </Items>
            </ext:FormPanel>
	    </Items>
    </ext:FitLayout>
	<ext:Hidden runat="server" ID="hdfSalvarVisualizarConfiguracoes" Text="0" />
</asp:Content>
