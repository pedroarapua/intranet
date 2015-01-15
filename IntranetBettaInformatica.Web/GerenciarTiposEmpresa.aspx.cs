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
    public partial class GerenciarTiposEmpresa : BasePage
    {

        #region propriedades

        private TipoEmpresaVO TipoEmpresaSelecionado
        {
            get
            {
                if (this.ViewState["TipoEmpresaSelecionado"] == null)
                    return null;
                return (TipoEmpresaVO)this.ViewState["TipoEmpresaSelecionado"];
            }
            set { this.ViewState["TipoEmpresaSelecionado"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarTiposEmpresa.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverTipoEmpresa(e);
        }

		protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarTipoEmpresa(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winTipoEmpresa.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winTipoEmpresa.Title = "Cadastrando Tipo Empresa";
            LimparCampos();
            winTipoEmpresa.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winTipoEmpresa.Title = "Alterando Tipo Empresa";
            winTipoEmpresa.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strTiposEmpresa.DataSource = new TipoEmpresaBO().Select().Where(x=> x.Removido == false).ToList();
            strTiposEmpresa.DataBind();
        }

		private void RemoverTipoEmpresa(DirectEventArgs e)
        {
            try
            {
                TipoEmpresaVO tipoEmpresa = JSON.Deserialize<List<TipoEmpresaVO>>(e.ExtraParams["valores"])[0];
                new TipoEmpresaBO(tipoEmpresa).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover tipo empresa.", "");
            }
        }

        private void SalvarTipoEmpresa(DirectEventArgs e)
        {
            try
            {
                TipoEmpresaVO tipoEmpresa = new TipoEmpresaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    tipoEmpresa = TipoEmpresaSelecionado;
                
                tipoEmpresa.Descricao = txtDescricao.Text;
                tipoEmpresa.Removido = false;
                
                new TipoEmpresaBO(tipoEmpresa).Salvar();
                
                LoadPagina();
                winTipoEmpresa.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar tipo empresa.";
                e.Success = false;
            }
			base.MostrarMensagem("Tipo de Empresa", "Tipo de empresa gravado com sucesso", String.Empty);
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            TipoEmpresaSelecionado = JSON.Deserialize<List<TipoEmpresaVO>>(e.ExtraParams["valores"])[0];
            txtDescricao.Text = TipoEmpresaSelecionado.Descricao;
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
        }

        #endregion
    }
}
