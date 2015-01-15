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
using System.Drawing;
using System.IO;

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarGaleriasVideos : BasePage
    {

        #region propriedades

        public AcaoTela AcaoTelaVideo
        {
            get
            {
                if (this.ViewState["AcaoTelaVideo"] == null)
                    return AcaoTela.Inclusao;
                return (AcaoTela)this.ViewState["AcaoTelaVideo"];
            }
            set { this.ViewState["AcaoTelaVideo"] = value; }
        }

        private GaleriaVO GaleriaSelecionado
        {
            get
            {
                if (this.ViewState["GaleriaSelecionado"] == null)
                    return null;
                return (GaleriaVO)this.ViewState["GaleriaSelecionado"];
            }
            set { this.ViewState["GaleriaSelecionado"] = value; }
        }

        private VideoVO VideoSelecionado
        {
            get
            {
                if (this.ViewState["VideoSelecionado"] == null)
                    return null;
                return (VideoVO)this.ViewState["VideoSelecionado"];
            }
            set { this.ViewState["VideoSelecionado"] = value; }
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
			btnNovo.Disabled = hdfAdicionarGaleriaVideos.Value.ToInt32() == 0;
			btnNovoVideo.Disabled = hdfAdicionarVideos.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemoverVideo_Click(object sender, DirectEventArgs e)
        {
            RemoverVideo(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarGaleria(e);
        }

        protected void SalvarVideo_Click(object sender, DirectEventArgs e)
        {
            SalvarVideo(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winGaleria.Title = "Cadastrando Galeria";
            LimparCampos();
            winGaleria.Show((Control)sender);
            
        }

        protected void btnNovoVideo_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaVideo = Common.AcaoTela.Inclusao;
            winVideo.Title = "Cadastrando Video";
            CarregarGalerias(null);
            LimparCamposVideo();
            winVideo.Show((Control)sender);
        }

        protected void btnEditarVideo_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaVideo = Common.AcaoTela.Edicao;
            PreencherCamposVideo(e);
            winVideo.Title = "Alterando Video";
            winVideo.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strVideos.DataSource = new VideoBO().Select().Where(x=> x.Removido == false && x.Galeria.Removido == false).ToList();
            strVideos.DataBind();
        }

        [DirectMethod]
        public void RemoverGaleria(String id)
        {
            try
            {
                GaleriaVO galeria = new GaleriaBO().SelectById(id.ToInt32());
                new GaleriaBO(galeria).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover galeria.", "");
            }
        }

        private void RemoverVideo(DirectEventArgs e)
        {
            try
            {
                VideoVO video = new VideoBO().SelectById(e.ExtraParams["id"].ToInt32());
                new VideoBO(video).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover video.", "");
            }
        }

        private void SalvarGaleria(DirectEventArgs e)
        {
            try
            {
                GaleriaVO galeria = new GaleriaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    galeria = GaleriaSelecionado;
               
                galeria.Descricao = txtDescricao.Text;
                galeria.Nome = txtNome.Text;
                galeria.Removido = false;

                new GaleriaBO(galeria).Salvar();

				base.MostrarMensagem("Galeria", "Galeria gravada com sucesso", String.Empty);

                LoadPagina();
                winGaleria.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar galeria.";
                e.Success = false;
            }            
        }

        private void SalvarVideo(DirectEventArgs e)
        {
            try
            {
                VideoVO video = new VideoVO();
                if (this.AcaoTelaVideo == Common.AcaoTela.Edicao)
                    video = VideoSelecionado;

                video.Titulo = txtTituloVideo.Text;
                if (!fufVideo.Disabled && !fufVideo.FileName.IsNullOrEmpty())
                {
                    video.Extensao = fufVideo.FileName.Substring(fufVideo.FileName.LastIndexOf("."));
                    video.NomeOriginal = fufVideo.FileName.Substring(fufVideo.FileName.LastIndexOf("\\") + 1);
                    if (!ValidaExtensaoVideo(video.Extensao))
                    {
                        base.MostrarMensagem("Video", "Extensão de video " + video.Extensao + " não suportada.", "");
                        e.Success = false;
						e.ErrorMessage = "Extensão de video " + video.Extensao + " não suportada.";
						return;
                    }
                }
                video.Galeria = new GaleriaVO() { Id = cboGaleria.Value.ToInt32() };
                video.Removido = false;

                video = (VideoVO)new VideoBO(video).Salvar();

                if (!fufVideo.Disabled && !fufVideo.FileName.IsNullOrEmpty())
                {
                    String pathOriginal = Path.Combine(Server.MapPath("~/GaleriaVideos"), video.Id + video.Extensao);
                    fufVideo.PostedFile.SaveAs(pathOriginal);
                }

				base.MostrarMensagem("Video", "Video gravado com sucesso", String.Empty);

                LoadPagina();
                winVideo.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar video.";
                e.Success = false;
            }
        }

        [DirectMethod]
        public void AdicionarVideo(String id)
        {
            LimparCamposVideo();
            GaleriaSelecionado = new GaleriaBO().SelectById(id.ToInt32());
            this.AcaoTelaVideo = Common.AcaoTela.Inclusao;
            winVideo.Title = "Cadastrando Video";
            CarregarGalerias(GaleriaSelecionado);
            cboGaleria.SetValue(GaleriaSelecionado.Id);
            winVideo.Show();
        }

        [DirectMethod]
        public void EditarGaleria(String id)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(id);
            winGaleria.Title = "Alterando Galeria";
            winGaleria.Show();
        }

        private void PreencherCampos(String id)
        {
            GaleriaSelecionado = new GaleriaBO().SelectById(id.ToInt32());
            txtNome.Text = GaleriaSelecionado.Nome;
            txtDescricao.Text = GaleriaSelecionado.Descricao;
        }

        private void PreencherCamposVideo(DirectEventArgs e)
        {
            VideoSelecionado = new VideoBO().SelectById(e.ExtraParams["id"].ToInt32());
            GaleriaSelecionado = VideoSelecionado.Galeria;
            CarregarGalerias(GaleriaSelecionado);
            cboGaleria.SetValue(GaleriaSelecionado.Id);
            txtTituloVideo.Text = VideoSelecionado.Titulo;
            fufVideo.Disabled = true;
            fufVideo.AllowBlank = true;
        }

        private void CarregarGalerias(GaleriaVO galeria)
        {
            strGalerias.DataSource = new GaleriaBO().Select().Where(x => x.Removido == false || (galeria != null && x.Id == galeria.Id)).ToList();
            strGalerias.DataBind();
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
            txtNome.Clear();
        }

        private void LimparCamposVideo()
        {
            cboGaleria.Clear();
            txtTituloVideo.Clear();
            fufVideo.AllowBlank = false;
            fufVideo.Disabled = false;
            fufVideo.Clear();
        }

        private Boolean ValidaExtensaoVideo(String extensao)
        {
            return base.LstExtensaoVideos.Any(x => x.ToLower() == extensao.ToLower());
        }

        #endregion
    }
}
