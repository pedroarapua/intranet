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
    public partial class GerenciarPerfisAcesso : BasePage
    {

        #region propriedades

        private PerfilAcessoVO PerfilSelecionado
        {
            get
            {
                if (this.ViewState["PerfilSelecionado"] == null)
                    return null;
                return (PerfilAcessoVO)this.ViewState["PerfilSelecionado"];
            }
            set { this.ViewState["PerfilSelecionado"] = value; }
        }

        #endregion

        #region eventos

        /// <summary>
        /// Evento disparado para carregar a página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPagina();
                base.SetTituloIconePagina(frmTitulo);
            }
			btnNovo.Disabled = hdfAdicionarPerfisAcesso.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverPerfilAcesso(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarPerfilAcesso(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winPerfilAcesso.Title = "Cadastrando Perfil de Acesso";
            LimparCampos();
			CarregarAcoes();
            winPerfilAcesso.Show((Control)sender);
            tab.SetActiveTab(0);
	    }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
			PreencherCampos(e);
            winPerfilAcesso.Title = "Alterando Perfil de Acesso";
            winPerfilAcesso.Show((Control)sender);
			tab.SetActiveTab(0);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strPerfisAcesso.DataSource = new PerfilAcessoBO().Select().Where(x=> x.Removido == false).ToList();
            strPerfisAcesso.DataBind();
        }

        private void RemoverPerfilAcesso(DirectEventArgs e)
        {
            try
            {
                PerfilAcessoVO perfil = new PerfilAcessoBO().SelectById(JSON.Deserialize<List<PerfilAcessoVO>>(e.ExtraParams["valores"])[0].Id);
                new PerfilAcessoBO(perfil).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover perfil de acesso.", "");
            }
        }

        private void SalvarPerfilAcesso(DirectEventArgs e)
        {
            try
            {
                PerfilAcessoVO perfil = new PerfilAcessoVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    perfil = PerfilSelecionado;
                
                perfil.Descricao = txtDescricao.Text;
                perfil.Nome = txtNome.Text;
                perfil.Removido = false;
				perfil.Acoes = JSON.Deserialize<List<AcaoVO>>(e.ExtraParams["acoes"]);
          
                new PerfilAcessoBO(perfil).Salvar();

				if (perfil.Id == UsuarioLogado.PerfilAcesso.Id)
				{
					UsuarioLogado.PerfilAcesso = new PerfilAcessoBO().SelectById(UsuarioLogado.PerfilAcesso.Id);
					base.MostrarMensagem("Sucesso", "Perfil de Acesso gravado com sucesso.", "GerenciarPerfisAcesso.aspx");
				}
				else
				{
					base.MostrarMensagem("Sucesso", "Perfil de Acesso gravado com sucesso.", "");
					LoadPagina();
				}
                winPerfilAcesso.Hide();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar perfil de acesso.", "");
            }            
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            PerfilSelecionado = JSON.Deserialize<List<PerfilAcessoVO>>(e.ExtraParams["valores"])[0];
            PerfilSelecionado = new PerfilAcessoBO().SelectById(PerfilSelecionado.Id);
			hdfModerador.Value = PerfilSelecionado.Id == 1 ? 1 : 0;
            txtNome.Text = PerfilSelecionado.Nome;
            txtDescricao.Text = PerfilSelecionado.Descricao;
			CarregarAcoes();
        }

        private void LimparCampos()
        {
            PerfilSelecionado = null;
            txtDescricao.Clear();
			hdfModerador.Value = 0;
            txtNome.Clear();
        }

		private void CarregarAcoes()
		{
			List<AcaoVO> lstAcoesTela = base.Acoes;
			lstAcoesTela.ForEach(x => x.Checked = PerfilSelecionado == null ? false : PerfilSelecionado.Acoes.Any(x1 => x1.TipoAcao == x.TipoAcao));
			strAcoes.DataSource = lstAcoesTela;
			strAcoes.DataBind();

		}
        #endregion
    }
}
