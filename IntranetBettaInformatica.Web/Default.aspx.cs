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
using System.Collections.Generic;
using IntranetBettaInformatica.Business;
using winUI = IntranetBettaInformatica.Entities;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Web.Common;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Web
{
    public partial class _Default : BasePage
    {

        #region eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadPagina();
                base.SetTituloIconePagina(frmTitulo);
            }
        }

        #endregion

        #region metodos
            
        private void LoadPagina()
        {
            // Carrega as informacoes do twitter do usuário
            CarregarTwitter();
            CarregarAniversariantes();
            CarregarUsuarios();
			CarregarNoticias();
			CarregarReunioes();
        }

        private void CarregarTwitter()
        {
            if(base.ContemPermissao(ETipoAcao.VisualizarTwitter))
            {
				if (!UsuarioLogado.Twitter.IsNullOrEmpty())
				{
					String script = "<script type=\"text/javascript\">";
					script +=
						"if(TWTR){ new TWTR.Widget({ version: 2, type: 'profile', rpp: 30, interval: 30000, width: '100%', height: '100%', ";
					script +=
						"theme: { shell: { background: '#333333', color: '#ffffff' }, tweets: { background: '#000000', color: '#ffffff', links: '#4aed05' } }, ";
					script +=
						"features: { scrollbar: true, loop: false, live: true, hashtags: true, timestamp: true, avatars: true, behavior: 'all' } ";
					script += "}).render().setUser('" + UsuarioLogado.Twitter + "').start();} </script>";
					litTwitter.Text = script;
				}
				else
				{
					litTwitter.Text = "<label style=\"margin:5px;\">Twitter não configurado.</label>";
				}
            }
			else
			{
				PortalColumnTwitter.Visible = false;
			}
        }

        /// <summary>
        /// Carrega os usuarios do sistema
        /// </summary>
        private void CarregarUsuarios()
        {
			if (base.ContemPermissao(ETipoAcao.VisualizarColaboradores))
			{
				List<UsuarioVO> lst = new UsuarioBO().BuscarUsuariosSistema(false, true);
				lst = lst.FindAll(x => x.Id != UsuarioLogado.Id);
				if (lst.Count == 0)
				{
					ptlUsuarios.Remove(ptlUsuarios.Items[0]);
					ptlUsuarios.Html = "&nbsp;&nbsp;Nenhum usuário.";
				}
				else
				{
					strUsuarios.DataSource = lst.Select(x => new { Id = x.Id, Nome = x.Nome, CaminhoImagemThumbs = x.CaminhoImagemThumbs, TruncateNome = x.Nome.Truncate(30) });
					strUsuarios.DataBind();
				}
			}
			else
			{
				PortalColumnColaboradores.Visible = false;
			}
        }

        /// <summary>
        /// Carrega os aniversariantes do mês
        /// </summary>
        private void CarregarAniversariantes()
        {
			if (base.ContemPermissao(ETipoAcao.VisualizarAniversariantes))
			{
				if (!MostrouLembretes && base.Aniversariantes.Count == 0)
					base.Aniversariantes = new UsuarioBO().BuscarAniversariantes(UsuarioLogado);

				if (base.Aniversariantes.Count == 0)
				{
					ptlAniversariantes.Remove(ptlAniversariantes.Items[0]);
					ptlAniversariantes.Html = "&nbsp;&nbsp;Nenhum aniversariante.";
				}
				else
				{
					strAniversariantes.DataSource = base.Aniversariantes.Select(x => new { Id = x.Id, Nome = x.Nome, CaminhoImagemThumbs = x.CaminhoImagemThumbs, TruncateNome = x.Nome.Truncate(30) });
					strAniversariantes.DataBind();
				}
			}
			else
			{
				PortalColumnAniversariantes.Visible = false;
			}
        }

		/// <summary>
		/// Metodo que carrega as noticias ativas
		/// </summary>
		private void CarregarNoticias()
		{
			List<NoticiaVO> noticias = new NoticiaBO().Buscar(UsuarioLogado, false, true, false, null, null).OrderByDescending(x => x.DataInicial).ToList();
			if (noticias.Count > 0)
			{
				strNoticias.DataSource = noticias;
				strNoticias.DataBind();
				lblNenhumaNoticia.Hidden = true;
			}
			else
			{
				lblNenhumaNoticia.Hidden = false;
			}
		}

		/// <summary>
		/// Metodo que carrega as reuniões do dia
		/// </summary>
		private void CarregarReunioes()
		{
			var reuniao = new ReuniaoBO().BuscarReunioesParaDia(UsuarioLogado).Select(x=> new { Id = x.Id, Codigo = x.Codigo, Titulo = x.Titulo, Horario = x.DataInicial.ToString("hh:mm")}).ToList();
			if (reuniao.Count > 0)
			{
				strReunioes.DataSource = reuniao;
				strReunioes.DataBind();
				lblNenhumaReuniao.Hidden = true;
			}
			else
			{
				lblNenhumaReuniao.Hidden = false;
			}
		}

        #endregion

    }
}
