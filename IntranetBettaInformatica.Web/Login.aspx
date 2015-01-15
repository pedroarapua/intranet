<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IntranetBettaInformatica.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login</title>
	<script src="Js/Util.js" type="text/javascript"></script>
	<script type="text/javascript">
		function validaLogin() {
			if (Ext.getCmp('txtLogin').getValue() == '') {
				alert('Erro', 'Informe o login do usuário.');
				return false;
			}
			return true;
		}
	</script>
	<style type="text/css">
		.lkbEsqueceuSenha .x-form-item-label
		{
			font-size:11px !important;
			padding:0px !important;
		}
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:resourcemanager ID="Resourcemanager1" runat="server" />
        <ext:viewport runat="server">
            <Items>
                <ext:AnchorLayout runat="server">
                    <Anchors>
                        <ext:Anchor Horizontal="100%" Vertical="100%">
                            <ext:Panel ID="pnlPaginaEmpresa" runat="server" Frame="true">
                                <TopBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server"/>
                                            <ext:Button runat="server" Text="Login" Icon="Key">
                                                <Listeners>
                                                    <Click Handler="#{winLogin}.show(this);"></Click>
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                            </ext:Panel>                
                        </ext:Anchor>
                    </Anchors>
                </ext:AnchorLayout>
            </Items>
        </ext:viewport>
        <ext:Window runat="server" ID="winLogin" Layout="Form" Border="false" Title="Login" Icon="Key" Hidden="true" Width="350" Height="160" Padding="5" Modal="true" Closable="false">
            <Items>
                <ext:FormPanel runat="server" ID="frmPanel" Border="false" Frame="true" AnchorHorizontal="100%" AnchorVertical="100%">
                    <Items>
                        <ext:TextField runat="server" ID="txtLogin" FieldLabel="Login" AllowBlank="false" AnchorHorizontal="92%" MsgTarget="Side"></ext:TextField>
                        <ext:TextField runat="server" ID="txtSenha" FieldLabel="Senha" InputType="Password" AllowBlank="false" AnchorHorizontal="92%" MsgTarget="Side"></ext:TextField>        
						<ext:Container runat="server" AnchorHorizontal="100%" StyleSpec="font-size:11px;" Cls="lkbEsqueceuSenha">
							<Items>
								<ext:LinkButton FieldLabel="Esqueceu a senha" LabelWidth="100" LabelSeparator="?" Text="Enviar uma nova senha por email" ID="lkbEsqueceuSenha" runat="server" StyleSpec="font-size:11px;">
									<Listeners>
										<Click Fn="validaLogin" />
									</Listeners>
									<DirectEvents>
										<Click OnEvent="lkbEnviarEmail_Click">
											<EventMask Msg="Enviando email..." ShowMask="true" />
										</Click>
									</DirectEvents>
								</ext:LinkButton>
							</Items>
						</ext:Container>
				    </Items>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnLogin" runat="server" Text="Logar" Icon="KeyGo" FormBind="true">
                    <Listeners>
                        <Click Handler="return #{frmPanel}.validate();" />
                    </Listeners>
                    <DirectEvents>
                        <Click OnEvent="btnLogar_Click">
                            <EventMask ShowMask="true" Msg="Efetuando Login..." />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                 <ext:Button runat="server" Text="Cancelar">
                    <Listeners>
                        <Click Handler="#{winLogin}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <KeyMap>
                <ext:KeyBinding>
                    <Keys>
                        <ext:Key Code="ENTER" />
                    </Keys>
                    <Listeners>
                        <Event Handler="#{btnLogin}.fireEvent('click');" />
                    </Listeners>
                </ext:KeyBinding>
            </KeyMap>
            <Listeners>
                <BeforeShow Handler="#{txtSenha}.reset(); #{txtLogin}.reset();"></BeforeShow>
            </Listeners>
        </ext:Window>
    </div>
    </form>
</body>
</html>
