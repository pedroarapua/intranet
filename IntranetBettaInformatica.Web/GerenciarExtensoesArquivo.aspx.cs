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
    public partial class GerenciarExtensoesArquivo : BasePage
    {

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
				CarregarExtensoes();
                base.SetTituloIconePagina(frmTitulo);
				btnNovo.Disabled = !hdfAdicionarExtensoesArquivo.Value.ToInt32().ToBoolean();
            }
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverExtensaoArquivo(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarExtensaoArquivo(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winExtensaoArquivo.Title = "Cadastrando Extensão de Arquivo";
            LimparCampos();
            winExtensaoArquivo.Show();
            
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strExtensoesArquivo.DataSource = new ExtensaoArquivoBO().Select().Where(x=> x.Removido == false).ToList();
			strExtensoesArquivo.DataBind();
        }

        private void RemoverExtensaoArquivo(DirectEventArgs e)
        {
            try
            {
                ExtensaoArquivoVO extensaoArquivo = new ExtensaoArquivoBO().SelectById(e.ExtraParams["id"].ToInt32());
                new ExtensaoArquivoBO(extensaoArquivo).Delete();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover tipo empresa.", "");
            }
        }

        private void SalvarExtensaoArquivo(DirectEventArgs e)
        {
            try
            {
                ExtensaoArquivoVO extensaoArquivo = new ExtensaoArquivoVO();
                extensaoArquivo.Extensao = txtExtensao.Text;
				extensaoArquivo.TipoExtensao = (ETipoExtensao)cboTipoExtensao.Value.ToInt32();
                extensaoArquivo.Removido = false;

				if (extensaoArquivo.Extensao[0] != '.')
				{
					base.MostrarMensagem("Erro", String.Format("Extensão deve iniciar com '.'.", extensaoArquivo.Extensao), String.Empty);
					return;
				}
				else if (!new ExtensaoArquivoBO().ValidaExtensao(extensaoArquivo))
				{
					base.MostrarMensagem("Erro", String.Format("Extensão {0} existente.", extensaoArquivo.Extensao), String.Empty);
					return;
				}
				else
				{
					new ExtensaoArquivoBO(extensaoArquivo).Salvar();
				}

				base.MostrarMensagem("Sucesso", "Extensão gravada com sucesso.", String.Empty);
                LoadPagina();
                winExtensaoArquivo.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar exntesão de arquivo.";
                e.Success = false;
            }            
        }

        private void LimparCampos()
        {
            txtExtensao.Clear();
			cboTipoExtensao.Clear();
			cboTipoExtensao.ClearInvalid();
			txtExtensao.ClearInvalid();
        }

		private void CarregarExtensoes()
		{
			List<Object> lstObject = new List<Object>();
			foreach(var enumerator in Enum.GetValues(typeof(ETipoExtensao)))
			{
				lstObject.Add(new { Id = enumerator.ToInt32(), Descricao = enumerator.ToString()});
			}
			strTiposExtensao.DataSource = lstObject;
			strTiposExtensao.DataBind();
		}

        #endregion
    }
}
