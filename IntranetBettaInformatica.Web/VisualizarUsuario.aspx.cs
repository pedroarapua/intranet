using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Ext.Net;
using IntranetBettaInformatica.Business;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Web.Common;
using System.Collections.Generic;

namespace IntranetBettaInformatica.Web
{
	public partial class VisualizarUsuario : System.Web.UI.Page
	{
		#region eventos

		/// <summary>
        /// Evento disparado para carregar a página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
			//Faz com que a session expire
			Response.Cache.SetExpires(DateTime.Now.Subtract(new TimeSpan(24, 0, 0)));
			//Desabilita a cache para a página
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			if (!IsPostBack)
			{
				if(!Request.QueryString["id"].IsNullOrEmpty())
				{
					UsuarioVO usuario = new UsuarioBO().SelectById(Request.QueryString["id"].ToInt32());
					
					if (usuario.Estado != null)
						lblCidadeEstado.Text = String.Format("{0}/{1}", usuario.Cidade, usuario.Estado.Sigla);
					else
						lblCidadeEstado.Text = usuario.Cidade;

					lblDataNascimento.Text = usuario.DataNascimento.HasValue ? usuario.DataNascimento.Value.ToString("dd/MM/yyyy") : String.Empty;
					lblEmail.Text = usuario.Email;
					lblEmpresa.Text = usuario.Empresa.Nome;
					lblEndereco.Text = usuario.Endereco;
					lblNome.Text = usuario.Nome;
					lblSetor.Text = usuario.Setor != null ? usuario.Setor.Nome : String.Empty;
					imgPerfil.ImageUrl = usuario.CaminhoImagemOriginal;
				}
			}
        }

        #endregion

	}
}
