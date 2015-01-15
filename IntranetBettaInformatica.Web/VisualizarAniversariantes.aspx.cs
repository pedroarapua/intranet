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
    public partial class VisualizarAniversariantes : BasePage
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
				txtDataInicialBusca.Value = DateTime.Today;
				txtDataFinalBusca.Value = DateTime.Today;
                LoadPagina();
                hdfUsuarioLogado.Value = base.UsuarioLogado.Id.ToString();
                base.SetTituloIconePagina(frmTitulo);
            }
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnBuscar_Click(object sender, DirectEventArgs e)
        {
            LoadPagina();
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarMensagem(e);
        }

        protected void btnMensagem_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winMensagem.Title = "Enviando Mensagem";
            LimparCampos();
            winMensagem.Show((Control)sender);

        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            if (!txtDataInicialBusca.Text.IsNullOrEmpty() && txtDataInicialBusca.Text != txtDataInicialBusca.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicialBusca.Text);
            }
            if (!txtDataFinalBusca.Text.IsNullOrEmpty() && txtDataFinalBusca.Text != txtDataFinalBusca.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinalBusca.Text);
            }
            strUsuarios.DataSource = new UsuarioBO().BuscarPorDataNascimento(dataIni, dataFim);
            strUsuarios.DataBind();
        }

        private void SalvarMensagem(DirectEventArgs e)
        {
            try
            {
                MensagemVO mensagem = new MensagemVO();
                
                if (txtMensagem.Text.Length > 2000)
                {
                    base.MostrarMensagem("Erro", "Limite de caracteres excedido (2000).", String.Empty);
                    return;
                }

                mensagem.Descricao = txtMensagem.Text;
                mensagem.Removido = false;
                mensagem.UsuarioEnvio = base.UsuarioLogado;
                mensagem.Data = DateTime.Now;
                mensagem.ConfirmarLeitura = chkConfirmarLeitura.Checked;

                mensagem.UsuariosMensagens.Add(new UsuarioMensagemVO() { LidoMensagem = false, Mensagem = mensagem, Removido = false, UsuarioRecMens = new UsuarioVO() { Id = e.ExtraParams["usuario"].ToInt32() } });

                new MensagemBO(mensagem).Salvar();

                LoadPagina();
                winMensagem.Hide();
		    }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar mensagem.";
                e.Success = false;
            }

			base.MostrarMensagem("Mensagem", "Mensagem enviada com sucesso", String.Empty);
        }

        private void LimparCampos()
        {
            txtMensagem.Clear();
            txtMensagem.ClearInvalid();
        }

        #endregion
    }
}
