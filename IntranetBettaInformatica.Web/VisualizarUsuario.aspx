<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizarUsuario.aspx.cs" Inherits="IntranetBettaInformatica.Web.VisualizarUsuario" Title="Perfil" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html>
	<head>
		<script src="Js/Util.js" type="text/javascript"></script>
		<style type="text/css">
			.x-form-element
			{
				padding-left: 105px;
				position: relative;
				padding-top:3px;
			}
		</style>
	</head>
	<body style="padding:10px;">
		<form runat="server">
			<ext:ResourceManager ID="ResourceManager1" runat="server">
			</ext:ResourceManager>
			<ext:Viewport runat="server">
				<Items>
					<ext:FitLayout ID="FitLayout1" runat="server">
						<Items>
							<ext:TabPanel runat="server" ID="tabUsuario">
								<Items>
									<ext:Panel runat="server" ID="pnlInfoUsuario" AnchorVertical="100%" Title="Informações Pessoais" Frame="true" Border="false" LabelAlign="Right" Layout="ColumnLayout">
										<Items>
											<ext:Container runat="server" ColumnWidth="0.75">
												<Items>
													<ext:Label runat="server" ID="lblNome" FieldLabel="Nome" Width="450" StyleSpec="padding-top:3px;"></ext:Label>
													<ext:Label runat="server" ID="lblEmpresa" Width="450" FieldLabel="Empresa" EmptyText="&nbsp;"></ext:Label>
													<ext:Label runat="server" ID="lblSetor" Width="450" FieldLabel="Setor" EmptyText="&nbsp;"></ext:Label>
													<ext:Label runat="server" ID="lblDataNascimento" Width="300" FieldLabel="Data de Nasc." EmptyText="&nbsp;"></ext:Label>
													<ext:Label runat="server" ID="lblEndereco" Width="400" FieldLabel="Endereço" EmptyText="&nbsp;"></ext:Label>
													<ext:Label runat="server" ID="lblCidadeEstado" Width="400" FieldLabel="Cidade/Estado" EmptyText="&nbsp;"></ext:Label>
													<ext:Label runat="server" ID="lblEmail" Width="400" FieldLabel="Email" EmptyText="&nbsp;"></ext:Label>
												</Items>
											</ext:Container>
											<ext:Container runat="server" ColumnWidth="0.25">
												<Items>
													<ext:Image runat="server" ID="imgPerfil" Width="150" Height="150">
													</ext:Image>
												</Items>
											</ext:Container>
                                        </Items>
									</ext:Panel>
								</Items>
							</ext:TabPanel>
						</Items>
					</ext:FitLayout>
				</Items>
			</ext:Viewport>
		</form>
	</body>
</html>

