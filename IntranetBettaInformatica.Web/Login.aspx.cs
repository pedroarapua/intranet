using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IntranetBettaInformatica.Entities.Entities;
using System.Collections.Generic;
using IntranetBettaInformatica.Business;
using Ext.Net;
using IntranetBettaInformatica.Web.Common;

namespace IntranetBettaInformatica.Web
{
    public partial class Login : System.Web.UI.Page
	{
		#region propriedades

		public ConfiguracoesSistemaVO Configuracao
		{
			get
			{
				if (this.Session["Configuracao"] == null)
					this.Session["Configuracao"] = new ConfiguracoesSistemaBO().SelectById(1);
				return (ConfiguracoesSistemaVO)this.Session["Configuracao"];
			}
			set { this.Session["Configuracao"] = value; }
		}

		#endregion
		#region eventos

		protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				Session["MostrouLembretes"] = false;
                pnlPaginaEmpresa.AutoLoad.Url = "http://www.bettainformatica.com.br";
                pnlPaginaEmpresa.AutoLoad.Mode = LoadMode.IFrame;
                pnlPaginaEmpresa.AutoLoad.Scripts = true;
                pnlPaginaEmpresa.AutoLoad.ShowMask = true;
                pnlPaginaEmpresa.LoadMask.ShowMask = true;
                pnlPaginaEmpresa.LoadMask.Msg = String.Format("Carregando {0} ...", pnlPaginaEmpresa.AutoLoad.Url);
                pnlPaginaEmpresa.AutoLoad.MaskMsg = String.Format("Carregando {0} ...", pnlPaginaEmpresa.AutoLoad.Url);
                winLogin.Show();
                if (Request.Cookies["login"] == null)
                    txtLogin.Focus(false, 50);
                else
                {
                    txtLogin.Text = Request.Cookies["login"].Value;
                    txtSenha.Focus(false, 50);
                }
            }
        }
        protected void btnLogar_Click(object sender, DirectEventArgs args)
        {
            this.Session["UsuarioLogado"] = new UsuarioBO().Login(txtLogin.Text, txtSenha.Text);
            if (this.Session["UsuarioLogado"] != null)
            {
                UsuarioVO usuarioLogado = (this.Session["UsuarioLogado"] as UsuarioVO);
                HttpCookie cookie = new HttpCookie("login", usuarioLogado.Login);
                cookie.Expires = DateTime.Now.AddDays(7);
                if (Request.Cookies["login"] == null)
                    Response.Cookies.Add(cookie);
                else
                    Response.SetCookie(cookie);
                Response.Redirect(usuarioLogado.PaginaInicial == null ? "Default.aspx" : usuarioLogado.PaginaInicial.Url);
            }
            else
            {
                X.Msg.Alert("Erro", "Login ou senha inválidos.").Show();
            }
        }

		protected void lkbEnviarEmail_Click(object sender, DirectEventArgs e)
		{
			try
			{
				UsuarioVO usuario = new UsuarioBO().BuscarPorLogin(txtLogin.Text);
				if (usuario != null)
				{
					String senha = new Random().Next(99999999).ToString(); // Gera uma nova senha, sorteando um numero
					usuario.Senha = UsuarioBO.EncriptyPassword(senha);
					new UsuarioBO(usuario).Salvar();

					EmailVO email = new EmailVO(this.Configuracao);
					email.Usuarios.Add(usuario);
					email.AddUsuariosAttachmentCollection();
					email.Body = String.Format("Senha:{0}", senha);
					email.Subject = String.Format("{0} - Senha de Acesso", this.Configuracao.Descricao);
					new EmailBO().EnviarEmailSincrono(email);
					X.Msg.Alert("Sucesso", String.Format("Senha enviada para o email {0}.", usuario.Email)).Show();
				}
				else
				{
					X.Msg.Alert("Erro", "Login não encontrado").Show();
				}
			}
			catch(Exception ex){
				X.Msg.Alert("Erro", ex.Message).Show();
			}
		}

		#endregion
	}
}
