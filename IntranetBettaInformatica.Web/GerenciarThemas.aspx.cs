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
    public partial class GerenciarThemas : BasePage
    {

        #region propriedades

        private TemaVO TemaSelecionado
        {
            get
            {
                if (this.ViewState["TemaSelecionado"] == null)
                    return null;
                return (TemaVO)this.ViewState["TemaSelecionado"];
            }
            set { this.ViewState["TemaSelecionado"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarTemas.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverThema(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarTema(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winTema.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winTema.Title = "Cadastrando Thema";
            LimparCampos();
            winTema.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winTema.Title = "Alterando Thema";
            winTema.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strTemas.DataSource = new TemaBO().Select().Where(x=> x.Removido == false).ToList();
            strTemas.DataBind();
        }

        private void RemoverThema(DirectEventArgs e)
        {
            try
            {
                TemaVO tema = JSON.Deserialize<List<TemaVO>>(e.ExtraParams["valores"])[0];
                new TemaBO(tema).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover tema.", "");
            }
        }

        private void SalvarTema(DirectEventArgs e)
        {
            try
            {
                TemaVO tema = new TemaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    tema = TemaSelecionado;
                
                tema.Descricao = txtDescricao.Text;
                tema.Nome = txtNome.Text;
                tema.Removido = false;
                
                new TemaBO(tema).Salvar();
                
                LoadPagina();
                winTema.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar tema.";
                e.Success = false;
            }

			base.MostrarMensagem("Tema", "Tema gravado com sucesso", String.Empty);
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            TemaSelecionado = JSON.Deserialize<List<TemaVO>>(e.ExtraParams["valores"])[0];
            txtNome.Text = TemaSelecionado.Nome;
            txtDescricao.Text = TemaSelecionado.Descricao;
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
            txtNome.Clear();
        }

        #endregion
    }
}
