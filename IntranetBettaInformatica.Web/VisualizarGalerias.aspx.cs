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
    public partial class VisualizarGalerias : BasePage
    {

        #region propriedades

        private GaleriaVO GaleriaSelecionada
        {
            get
            {
                if (this.ViewState["GaleriaSelecionada"] == null)
                    return null;
                return (GaleriaVO)this.ViewState["GaleriaSelecionada"];
            }
            set { this.ViewState["GaleriaSelecionada"] = value; }
        }

        private FotoVO FotoSelecionada
        {
            get
            {
                if (this.ViewState["FotoSelecionada"] == null)
                    return null;
                return (FotoVO)this.ViewState["FotoSelecionada"];
            }
            set { this.ViewState["FotoSelecionada"] = value; }
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

        private ComentarioFotoVO ComentarioFoto
        {
            get
            {
                if (this.ViewState["ComentarioFoto"] == null)
                {
                    this.ViewState["ComentarioFoto"] = new ComentarioFotoVO() { Foto = this.FotoSelecionada };
                }
                return (ComentarioFotoVO)this.ViewState["ComentarioFoto"];
            }
            set { this.ViewState["ComentarioFoto"] = value; }
        }

        private ComentarioVideoVO ComentarioVideo
        {
            get
            {
                if (this.ViewState["ComentarioVideo"] == null)
                {
                    this.ViewState["ComentarioVideo"] = new ComentarioVideoVO() { Video = this.VideoSelecionado};
                }
                return (ComentarioVideoVO)this.ViewState["ComentarioVideo"];
            }
            set { this.ViewState["ComentarioVideo"] = value; }
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
			btnAdicionarComentarioFoto.Disabled = !hdfAdicionarComentarioFoto.Value.ToInt32().ToBoolean();
			btnAdicionarComentarioVideo.Disabled = !hdfAdicionarComentarioVideo.Value.ToInt32().ToBoolean();
	    }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void OnRefreshDataComentario(object sender, StoreRefreshDataEventArgs e)
        {
            LoadComentarios();
        }

        protected void btnVisualizarFotos_Click(object sender, DirectEventArgs e)
        {
            VisualizarFotos(sender, e);
            
        }

        protected void btnVisualizarVideos_Click(object sender, DirectEventArgs e)
        {
            VisualizarVideos(sender, e);
        }

        protected void dtImagens_SelectionChange(object sender, DirectEventArgs e)
        {
            Ext.Net.DataView dtView = sender as Ext.Net.DataView;
            if (dtView.SelectedIndex < 0)
            {
                pnlFotos.Hidden = true;
                grdComentariosFotos.Hidden = true;
            }
            else
            {
				FotoSelecionada = GaleriaSelecionada.Fotos[dtView.SelectedIndex];
                LoadComentarios();
				System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(Server.MapPath("~/"), FotoSelecionada.CaminhoImagemOriginal));
                Dictionary<Int32, Int32> dicSize = GetSizePhoto(img);
				imgAtual.Width = dicSize.First().Value;
				imgAtual.Height = dicSize.First().Key;
				imgAtual.ImageUrl = FotoSelecionada.CaminhoImagemOriginal;
                
                btnProximo.Disabled = GaleriaSelecionada.Fotos.Count == (dtView.SelectedIndex + 1);
                btnAnterior.Disabled = dtView.SelectedIndex == 0;
                pnlFotos.Hidden = false;
                grdComentariosFotos.Hidden = false;

				pnlImagemPrincipal.DoLayout();

            }
        }

        protected void dtVideos_SelectionChange(object sender, DirectEventArgs e)
        {
            Ext.Net.DataView dtView = sender as Ext.Net.DataView;
            if (dtView.SelectedIndex < 0)
            {
                VideoSelecionado = null;
                pnlVideoFlash.Hidden = true;
                grdComentariosVideos.Hidden = true;
            }
            else
            {
                VideoSelecionado = GaleriaSelecionada.Videos[dtView.SelectedIndex];
                LoadComentarios();
				if (VideoSelecionado.Extensao.ToUpper() == ".SWF")
					pnlVideoFlash.Html = String.Format("<object width=\"100%\" height=\"100%\" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\"><param name=\"SRC\" value=\"GaleriaVideos/{0}\"><embed src=\"GaleriaVideos/{0}\" width=\"100%\" height=\"100%\"></embed></object>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao);
				else if(VideoSelecionado.Extensao.ToUpper() == ".WMV")
					pnlVideoFlash.Html = String.Format("<object width=\"100%\" height=\"100%\" type=\"video/x-ms-asf\" url=\"GaleriaVideos/{0}\" data=\"GaleriaVideos/{0}\" classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\"><param name=\"url\" value=\"GaleriaVideos/{0}\"><param name=\"filename\" value=\"GaleriaVideos/{0}\"><param name=\"autostart\" value=\"1\"><param name=\"uiMode\" value=\"full\" /><param name=\"autosize\" value=\"1\"><param name=\"playcount\" value=\"1\"><embed type=\"application/x-mplayer2\" src=\"GaleriaVideos/{0}\" width=\"100%\" height=\"100%\" autostart=\"true\" showcontrols=\"true\" pluginspage=\"http://www.microsoft.com/Windows/MediaPlayer/\"></embed></object>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao);
				else
					pnlVideoFlash.Html = String.Format("<video width=\"100%\" height=\"100%\" controls=\"controls\" autoplay=\"autoplay\"> <source src=\"GaleriaVideos/{0}\" type=\"video/{1}\" /></video>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao, VideoSelecionado.Extensao.Replace(".", String.Empty));
                pnlVideoFlash.Hidden = false;
                grdComentariosVideos.Hidden = false;
            }
        }

        protected void btnProximo_Click(object sender, DirectEventArgs e)
        {
            Int32 index = dtImagens.SelectedIndex + 1;
            dtImagens.SelectedRows.Clear();
            dtImagens.SelectedRows.Add(new SelectedRow(index));
            dtImagens.UpdateSelection();
            dtImagens_SelectionChange(dtImagens, null);
        }

        protected void btnAnterior_Click(object sender, DirectEventArgs e)
        {
            Int32 index = dtImagens.SelectedIndex - 1;
            dtImagens.SelectedRows.Clear();
            dtImagens.SelectedRows.Add(new SelectedRow(index));
            dtImagens.UpdateSelection();
            dtImagens_SelectionChange(dtImagens, null);
        }

        protected void btnFecharvideo_Click(object sender, DirectEventArgs e)
        {
            winVideos.Hide();
        }
        protected void btnSalvarComentario_Click(object sender, DirectEventArgs e)
        {
            SalvarComentario();
        }

        protected void btnAdicionarComentario_Click(object sender, DirectEventArgs e)
        {
            txtComentario.Clear();
            ComentarioFoto = null;
            ComentarioVideo = null;
            winComentario.Title = "Adicionando Comentário";
            winComentario.Show();
        }

        protected void btnEditarComentario_Click(object sender, DirectEventArgs e)
        {
            PreencherComentario(e);
            winComentario.Title = "Editando Comentário";
            winComentario.Show();
        }

        protected void btnRemoverComentario_Click(object sender, DirectEventArgs e)
        {
            RemoverComentario(e);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strGalerias.DataSource = new GaleriaBO().Select().Where(x=> x.Removido == false).ToList();
            strGalerias.DataBind();
            hdfUsuarioLogado.Value = UsuarioLogado.Id.ToString();
        }

        private void VisualizarFotos(object sender, DirectEventArgs e)
        {
            GaleriaSelecionada = new GaleriaBO().SelectById(e.ExtraParams["id"].ToInt32());
            VideoSelecionado = null;
            strFotos.DataSource = GaleriaSelecionada.Fotos.Where(x => x.Removido == false).ToList();
            strFotos.DataBind();
            winFotos.Show((Control)sender);
            if (GaleriaSelecionada.Fotos.Count > 0)
            {
                dtImagens.SelectedRows.Add(new SelectedRow(0));
                dtImagens.UpdateSelection();
				dtImagens_SelectionChange(dtImagens, null);
            }
            else
            {
                pnlFotos.Hidden = true;
                grdComentariosFotos.Hidden = true;
            }
        }

        private void VisualizarVideos(object sender, DirectEventArgs e)
        {
            GaleriaSelecionada = new GaleriaBO().SelectById(e.ExtraParams["id"].ToInt32());
            FotoSelecionada = null;
            strVideos.DataSource = GaleriaSelecionada.Videos.Where(x => x.Removido == false).ToList();
            strVideos.DataBind();
            if (GaleriaSelecionada.Videos.Count > 0)
            {
                dtVideos.SelectedRows.Add(new SelectedRow(0));
                dtVideos.UpdateSelection();
                VideoSelecionado = GaleriaSelecionada.Videos[0];
                LoadComentarios();
				if (VideoSelecionado.Extensao.ToUpper() == ".SWF")
					pnlVideoFlash.Html = String.Format("<object width=\"100%\" height=\"100%\" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\"><param name=\"SRC\" value=\"GaleriaVideos/{0}\"><embed src=\"GaleriaVideos/{0}\" width=\"100%\" height=\"100%\"></embed></object>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao);
				else if (VideoSelecionado.Extensao.ToUpper() == ".WMV")
					pnlVideoFlash.Html = String.Format("<object width=\"100%\" height=\"100%\" type=\"video/x-ms-asf\" url=\"GaleriaVideos/{0}\" data=\"GaleriaVideos/{0}\" classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\"><param name=\"url\" value=\"GaleriaVideos/{0}\"><param name=\"filename\" value=\"GaleriaVideos/{0}\"><param name=\"autostart\" value=\"1\"><param name=\"uiMode\" value=\"full\" /><param name=\"autosize\" value=\"1\"><param name=\"playcount\" value=\"1\"><embed type=\"application/x-mplayer2\" src=\"GaleriaVideos/{0}\" width=\"100%\" height=\"100%\" autostart=\"true\" showcontrols=\"true\" pluginspage=\"http://www.microsoft.com/Windows/MediaPlayer/\"></embed></object>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao);
				else
					pnlVideoFlash.Html = String.Format("<video width=\"100%\" height=\"100%\" controls=\"controls\"> <source src=\"GaleriaVideos/{0}\" type=\"video/{1}\" autoplay=\"autoplay\"/></video>", VideoSelecionado.Id.ToString() + VideoSelecionado.Extensao, VideoSelecionado.Extensao.Replace(".", String.Empty));
                pnlVideoFlash.Hidden = false;
                grdComentariosVideos.Hidden = false;
            }
            else
            {
                pnlVideoFlash.Hidden = true;
                grdComentariosVideos.Hidden = true;
            }
            winVideos.Show((Control)sender);
        }

        private void LoadComentarios()
        {
            if (FotoSelecionada != null)
            {
                rowExpander.CollapseAll();
                strComentarios.DataSource = new ComentarioFotoBO().BuscarPorFoto(FotoSelecionada);
                strComentarios.DataBind();
                rowExpander.ExpandAll();
            }
            else
            {
                rowExpanderVideo.CollapseAll();
                strComentarios.DataSource = new ComentarioVideoBO().BuscarPorVideo(VideoSelecionado);
                strComentarios.DataBind();
                rowExpanderVideo.ExpandAll();
            }
            
        }

        private void SalvarComentario()
        {
            if (FotoSelecionada != null)
            {
                SalvarComentarioFoto();
            }
            else
            {
                SalvarComentarioVideo();
            }
        }

        private void SalvarComentarioFoto()
        {
            try
            {
                ComentarioFoto.Comentario = txtComentario.Text;
                ComentarioFoto.Removido = false;
                ComentarioFoto.Usuario = UsuarioLogado;
                ComentarioFoto.Data = DateTime.Now;
                new ComentarioFotoBO(ComentarioFoto).Salvar();
                winComentario.Hide();
                LoadComentarios();
                base.MostrarMensagem("Sucesso", "Comentário em foto gravado com sucesso.", String.Empty);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar comentario em foto.", String.Empty);
            }
        }

        private void SalvarComentarioVideo()
        {
            try
            {
                ComentarioVideo.Comentario = txtComentario.Text;
                ComentarioVideo.Removido = false;
                ComentarioVideo.Usuario = UsuarioLogado;
                ComentarioVideo.Data = DateTime.Now;
                new ComentarioVideoBO(ComentarioVideo).Salvar();
                winComentario.Hide();
                LoadComentarios();
                base.MostrarMensagem("Sucesso", "Comentário em video gravado com sucesso.", String.Empty);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar comentario em video.", String.Empty);
            }
        }

        private void PreencherComentario(DirectEventArgs e)
        {
            if (FotoSelecionada != null)
            {
                ComentarioFoto = new ComentarioFotoBO().SelectById(e.ExtraParams["id"].ToInt32());
                txtComentario.Text = ComentarioFoto.Comentario;
            }
            else
            {
                ComentarioVideo = new ComentarioVideoBO().SelectById(e.ExtraParams["id"].ToInt32());
                txtComentario.Text = ComentarioVideo.Comentario;
            }
        }

        private void RemoverComentario(DirectEventArgs e)
        {
            try
            {
                if (FotoSelecionada != null)
                {
                    ComentarioFoto = new ComentarioFotoBO().SelectById(e.ExtraParams["id"].ToInt32()); ;
                    ComentarioFoto.Removido = true;
                    new ComentarioFotoBO(ComentarioFoto).DeleteUpdate();
                }
                else
                {
                    ComentarioVideo = new ComentarioVideoBO().SelectById(e.ExtraParams["id"].ToInt32());
                    ComentarioVideo.Removido = true;
                    new ComentarioVideoBO(ComentarioVideo).DeleteUpdate();
                }
                LoadComentarios();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao remover comentário.", String.Empty);
            }
        }

        private Dictionary<int, int> GetSizePhoto(System.Drawing.Image img)
        {
            int oWidth = img.Width; // largura original
            int oHeight = img.Height; // altura original
            int nWidth = 600;
            int nHeight = 400;
            Dictionary<Int32, Int32> dicSize = new Dictionary<Int32, Int32>();

            // redimensiona se necessario
            if(oWidth > nWidth || oHeight > nHeight)
            {

                if(oWidth > oHeight)
                {
                    // imagem horizontal
                    nHeight = (oHeight * nWidth) / oWidth;
                    nWidth = (oWidth * nHeight) / oHeight;
                }
                else
                {
                    // imagem vertical
                    nWidth = (oWidth * nHeight) / oHeight;
                    nHeight = (oHeight * nWidth) / oWidth;
                }
            }

            dicSize.Add(nHeight, nWidth);
            return dicSize;
        }

        #endregion
    }
}
