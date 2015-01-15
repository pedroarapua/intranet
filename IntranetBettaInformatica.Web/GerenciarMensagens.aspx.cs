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
    public partial class GerenciarMensagens : BasePage
    {

        #region propriedades

        private MensagemVO MensagemSelecionada
        {
            get
            {
                if (this.ViewState["MensagemSelecionada"] == null)
                    return null;
                return (MensagemVO)this.ViewState["MensagemSelecionada"];
            }
            set { this.ViewState["MensagemSelecionada"] = value; }
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
			btnNovo.Disabled = hdfAdicionarMensagens.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverMensagem(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarMensagem(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winMensagem.Title = "Enviando Mensagem";
            LimparCampos();
            winMensagem.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winMensagem.Title = "Alterando Mensagem";
            winMensagem.Show((Control)sender);
        }

        protected void btnMarcarLido_Click(object sender, DirectEventArgs e)
        {
            MarcarMensagemLida(e);
        }

        protected void btnAdicionarUsuarios_Click(object sender, DirectEventArgs e)
        {
            AdicionarUsuarios((Control)sender);
        }

        protected void btnBuscar_Click(object sender, DirectEventArgs e)
        {
            LoadPagina();
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
            if(!txtDataInicial.Text.IsNullOrEmpty() && txtDataInicial.Text != txtDataInicial.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicial.Text);   
            }
            if (!txtDataFinal.Text.IsNullOrEmpty() && txtDataFinal.Text != txtDataFinal.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinal.Text);   
            }

            Boolean? msgLidas = null;
            if (rdbLido.Checked)
                msgLidas = true;
            else if (rdbNaoLido.Checked)
                msgLidas = false;

            UsuarioLogado.MensagensEnviadas = new MensagemBO().BuscarMensagensEnviadas(UsuarioLogado, dataIni, dataFim);
            UsuarioLogado.MensagensRecebidas = new UsuarioMensagemBO().BuscarMensagensRecebidas(UsuarioLogado, msgLidas, dataIni, dataFim);
            UsuarioLogado.MensagensEnviadas.ToList().ForEach(x => x.MensagemEnviada = true);
            foreach(UsuarioMensagemVO um in UsuarioLogado.MensagensRecebidas)
            {
                um.Mensagem.MensagemEnviada = false;
                um.Mensagem.MensagemLida = um.LidoMensagem;
            }
            List<MensagemVO> mensEnvRec = UsuarioLogado.MensagensEnviadas.Union(UsuarioLogado.MensagensRecebidas.Select(x=> x.Mensagem).ToList()).ToList();
            strMensagens.DataSource = mensEnvRec;
            strMensagens.DataBind();
            
            // Limpa selecao do grid
            CheckboxSelectionModel sm = this.grdMensagens.SelectionModel.Primary as CheckboxSelectionModel;
            sm.ClearSelections();
        }

        private void RemoverMensagem(DirectEventArgs e)
        {
            try
            {
                Boolean MensagemEnviada = Convert.ToBoolean(e.ExtraParams["mensagemEnviada"]);
                if (MensagemEnviada)
                {
                    MensagemVO mensagem = new MensagemBO().SelectById(e.ExtraParams["id"].ToInt32());
                    new MensagemBO(mensagem).DeleteUpdate();
                }
                else
                {
                    UsuarioMensagemVO usuarioM = new UsuarioMensagemBO().BuscarPorMensagemUsuario(e.ExtraParams["id"].ToInt32(), UsuarioLogado);
                    new UsuarioMensagemBO(usuarioM).DeleteUpdate();
                }
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover mensagem.", "");
            }
        }

        private void SalvarMensagem(DirectEventArgs e)
        {
            try
            {
                MensagemVO mensagem = new MensagemVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    mensagem = MensagemSelecionada;

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

                List<UsuarioVO> usuarioM = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuarios"]);
                List<UsuarioVO> usuarioAdd = usuarioM.Where(x=> !mensagem.UsuariosMensagens.Any(x1=> x1.UsuarioRecMens.Id == x.Id)).ToList();
                usuarioAdd.ForEach(
                    x=> mensagem.UsuariosMensagens.Add(
                        new UsuarioMensagemVO(){ 
                            LidoMensagem = !mensagem.ConfirmarLeitura,
                            Mensagem = mensagem,
                            UsuarioRecMens = x,
                            Removido = false
                        }
                    )
                );
                mensagem.UsuariosMensagens = mensagem.UsuariosMensagens.Where(x => usuarioM.Any(x1 => x1.Id == x.UsuarioRecMens.Id)).ToList();
                
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

        private void PreencherCampos(DirectEventArgs e)
        {
            MensagemSelecionada = new MensagemBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtMensagem.Text = MensagemSelecionada.Descricao;
            strUsuarios.DataSource = MensagemSelecionada.UsuariosMensagens.ToList().Select(x => x.UsuarioRecMens).ToList();
            strUsuarios.DataBind();
        }

        private void LimparCampos()
        {
            strUsuarios.DataSource = new List<UsuarioVO>();
            strUsuarios.DataBind();
            txtMensagem.Clear();
        }

        private void LimparCamposPesquisaUsuarios()
        {
            btnRemoverUsuarios.Disabled = true;
        }

        private void AdicionarUsuarios(Control sender)
        {
            LimparCamposPesquisaUsuarios();
            strUsuariosPesquisa.DataSource = new UsuarioBO().BuscarUsuariosSistema(false, true).Where(x=> x.Id != UsuarioLogado.Id).ToList();
            strUsuariosPesquisa.DataBind();
            (grdUsuariosPesquisa.SelectionModel.Primary as CheckboxSelectionModel).ClearSelections();
            winAdicionarUsuarios.Show(sender);
        }

        private void MarcarMensagemLida(DirectEventArgs e)
        {
            try
            {
                List<MensagemVO> mensagens = JSON.Deserialize<List<MensagemVO>>(e.ExtraParams["mensagens"]);
                List<UsuarioMensagemVO> lstUM = new UsuarioMensagemBO().BuscarPorMensagensUsuario(mensagens, UsuarioLogado);
                new UsuarioMensagemBO().Salvar(lstUM);
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar marcar como lida a mensagem.", String.Empty);
            }
        }

        #endregion
    }
}
