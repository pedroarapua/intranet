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
    public partial class GerenciarNoticias : BasePage
    {

        #region propriedades

        private NoticiaVO NoticiaSelecionada
        {
            get
            {
                if (this.ViewState["NoticiaSelecionada"] == null)
                    return null;
                return (NoticiaVO)this.ViewState["NoticiaSelecionada"];
            }
            set { this.ViewState["NoticiaSelecionada"] = value; }
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
			btnNovo.Disabled = hdfAdicionarNoticias.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverNoticia(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarNoticia(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winNoticia.Title = "Cadastrando Notícia";
            LimparCampos();
            winNoticia.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winNoticia.Title = "Alterando Notícia";
            winNoticia.Show((Control)sender);
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
            if (!txtDataInicialBusca.Text.IsNullOrEmpty() && txtDataInicialBusca.Text != txtDataInicialBusca.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicialBusca.Text);   
            }
            if (!txtDataFinalBusca.Text.IsNullOrEmpty() && txtDataFinalBusca.Text != txtDataFinalBusca.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinalBusca.Text);   
            }
            //rowExpander.CollapseAll();
            strNoticias.DataSource = new NoticiaBO().Buscar(chkAtiva.Checked, chkIniciada.Checked, chkFinalizada.Checked, dataIni, dataFim).Select(x=> new { HTML = x.HTML, Id = x.Id, Titulo = x.Titulo, Status = x.Status, DataInicial = x.DataInicial, DataFinal = x.DataFinal, DataPeriodo = x.DataInicial.ToString("dd/MM/yyyy") + " até "+ x.DataFinal.ToString("dd/MM/yyyy"), Usuarios = String.Join(", ", x.Usuarios.Select(x1=> x1.Nome)) }).ToList();
            strNoticias.DataBind();
            //rowExpander.ExpandAll();
        }

        private void RemoverNoticia(DirectEventArgs e)
        {
            try
            {
                NoticiaVO noticia = new NoticiaBO().SelectById(e.ExtraParams["id"].ToInt32());
                new NoticiaBO(noticia).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover notícia.", "");
            }
        }

        private void SalvarNoticia(DirectEventArgs e)
        {
            try
            {
                NoticiaVO noticia = new NoticiaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    noticia = NoticiaSelecionada;

				String html = HttpUtility.UrlDecode(e.ExtraParams["html"]);

				if (html.Length > 65535) // maximo suportado pelo mysql
                {
					base.MostrarMensagem("Erro", "Descrição, limite de caracteres excedido (65535).", String.Empty);
                    return;
                }

				noticia.HTML = html;
                noticia.Titulo = txtTitulo.Text;
                noticia.Removido = false;
                noticia.Usuarios = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuarios"]);
                noticia.DataInicial = txtDataInicial.SelectedDate;
                noticia.DataFinal = txtDataFinal.SelectedDate.AddHours(23).AddMinutes(59).AddSeconds(59);

                if (noticia.DataInicial > noticia.DataFinal)
                {
                    tabNoticia.SetActiveTab(0);
                    base.MostrarMensagem("Erro", "Data inicial não pode ser superior a final.", String.Empty);
                    return;
                }

                new NoticiaBO(noticia).Salvar();
                
                LoadPagina();
                winNoticia.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar notícia.";
                e.Success = false;
            }

			base.MostrarMensagem("Notícia", "Notícia gravada com sucesso", String.Empty);
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            NoticiaSelecionada = new NoticiaBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtTitulo.Text = NoticiaSelecionada.Titulo;
            txtDataInicial.Text = NoticiaSelecionada.DataInicial.ToString("dd/MM/yyyy");
            txtDataFinal.Text = NoticiaSelecionada.DataFinal.ToString("dd/MM/yyyy");
			e.ExtraParamsResponse["html"] = NoticiaSelecionada.HTML;
            strUsuarios.DataSource = NoticiaSelecionada.Usuarios.ToList();
            strUsuarios.DataBind();
        }

        private void LimparCampos()
        {
            strUsuarios.DataSource = new List<UsuarioVO>();
            strUsuarios.DataBind();
            txtTitulo.Clear();
            txtDataInicial.Clear();
            txtDataFinal.Clear();
            tabNoticia.SetActiveTab(0);
        }

        private void LimparCamposPesquisaUsuarios()
        {
            btnRemoverUsuarios.Disabled = true;
        }

        private void AdicionarUsuarios(Control sender)
        {
            LimparCamposPesquisaUsuarios();
            strUsuariosPesquisa.DataSource = new UsuarioBO().BuscarUsuariosSistema(false, true).ToList();
            strUsuariosPesquisa.DataBind();
            (grdUsuariosPesquisa.SelectionModel.Primary as CheckboxSelectionModel).ClearSelections();
            winAdicionarUsuarios.Show(sender);
        }

        #endregion
    }
}
