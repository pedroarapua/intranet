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
    public partial class GerenciarFuncoes : BasePage
    {

        #region propriedades

        private FuncaoVO FuncaoSelecionado
        {
            get
            {
                if (this.ViewState["FuncaoSelecionado"] == null)
                    return null;
                return (FuncaoVO)this.ViewState["FuncaoSelecionado"];
            }
            set { this.ViewState["FuncaoSelecionado"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarFuncoes.Value.ToInt32().ToBoolean();
        }

		protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverFuncao(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarFuncao(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winFuncao.Title = "Cadastrando Função";
            LimparCampos();
            txtOrdem.Text = new FuncaoBO().BuscaUltimaOrdem().ToString();
			txtOrdem.MaxValue = Convert.ToDouble(txtOrdem.Text);
            winFuncao.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winFuncao.Title = "Alterando Função";
            winFuncao.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strFuncoes.DataSource = new FuncaoBO().Select().Where(x=> x.Removido == false).OrderBy(x=> x.Ordem).ToList();
            strFuncoes.DataBind();
        }

        private void RemoverFuncao(DirectEventArgs e)
        {
            try
            {
                FuncaoVO funcao = JSON.Deserialize<List<FuncaoVO>>(e.ExtraParams["valores"])[0];
                new FuncaoBO(funcao).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover função.", "");
            }
        }

        private void SalvarFuncao(DirectEventArgs e)
        {
            try
            {
                FuncaoVO funcao = new FuncaoVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    funcao = FuncaoSelecionado;

                funcao.Nome = txtNome.Text;
                funcao.Descricao = txtDescricao.Text;
                funcao.Ordem = Convert.ToInt32(txtOrdem.Text);
                funcao.Removido = false;

				// Validação para não deixar numero de ordem repetido, foi verificado e pode deixar repetir
				//if (Convert.ToInt32(hdfMensagem.Value) == 0)
				//{
				//    if (VerificaOrdem())
				//    {
				//        hdfMensagem.Value = 1;
				//        e.ExtraParamsResponse["contemOrdem"] = "1";
				//        return;
				//    }
				//}
				//else
				//{
				//    new FuncaoBO().AtualizaFuncaoParaOrdemSuperior(funcao);
				//}
                
				new FuncaoBO(funcao).Salvar();
                LoadPagina();
                winFuncao.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar função.";
                e.Success = false;
            }            
        }

		[DirectMethod]
		public Boolean VerificaOrdem()
		{
			Boolean possuiOrdem = new FuncaoBO().ContemOrdem(FuncaoSelecionado, Convert.ToInt32(txtOrdem.Value));
			return possuiOrdem;
		}

        private void PreencherCampos(DirectEventArgs e)
        {
            FuncaoSelecionado = JSON.Deserialize<List<FuncaoVO>>(e.ExtraParams["valores"])[0];
            txtNome.Text = FuncaoSelecionado.Nome;
            txtDescricao.Text = FuncaoSelecionado.Descricao;
            txtOrdem.Text = FuncaoSelecionado.Ordem.ToString();
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
            txtOrdem.Clear();
            txtNome.Clear();
			FuncaoSelecionado = null;
			hdfMensagem.Value = 0;
        }

        #endregion
    }
}
