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
    public partial class GerenciarGaleriasFotos : BasePage
    {

        #region propriedades

        public AcaoTela AcaoTelaFoto
        {
            get
            {
                if (this.ViewState["AcaoTelaFoto"] == null)
                    return AcaoTela.Inclusao;
                return (AcaoTela)this.ViewState["AcaoTelaFoto"];
            }
            set { this.ViewState["AcaoTelaFoto"] = value; }
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
			btnNovaFoto.Disabled = hdfAdicionarFotos.Value.ToInt32() == 0;
			btnNovo.Disabled = hdfAdicionarGaleriaFotos.Value.ToInt32() == 0;
	    }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemoverFoto_Click(object sender, DirectEventArgs e)
        {
            RemoverFoto(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarGaleria(e);
        }

        protected void SalvarFoto_Click(object sender, DirectEventArgs e)
        {
            SalvarFoto(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winGaleria.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winGaleria.Title = "Cadastrando Galeria";
            LimparCampos();
            winGaleria.Show((Control)sender);
        }

        protected void btnNovaFoto_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaFoto = Common.AcaoTela.Inclusao;
            winFoto.Title = "Cadastrando Foto";
            CarregarGalerias(null);
            LimparCamposFoto();
            winFoto.Show((Control)sender);
        }

        protected void btnEditarFoto_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaFoto = Common.AcaoTela.Edicao;
            PreencherCamposFoto(e);
            winFoto.Title = "Alterando Foto";
            winFoto.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strFotos.DataSource = new FotoBO().Select().Where(x=> x.Removido == false && x.Galeria.Removido == false).ToList();
            strFotos.DataBind();
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

        private void RemoverFoto(DirectEventArgs e)
        {
            try
            {
                FotoVO foto = new FotoBO().SelectById(e.ExtraParams["id"].ToInt32());
                new FotoBO(foto).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover foto.", "");
            }
        }

        private void SalvarGaleria(DirectEventArgs e)
        {
            try
            {
                GaleriaVO galeria = new GaleriaVO();
                FotoVO foto = null;
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    galeria = GaleriaSelecionado;
                else
                {
                    foto = new FotoVO();
                    if (!fufImagemAlbum.Disabled && !fufImagemAlbum.FileName.IsNullOrEmpty())
                    {
                        foto.Extensao = fufImagemAlbum.FileName.Substring(fufImagemAlbum.FileName.LastIndexOf("."));
                        foto.NomeOriginal = fufImagemAlbum.FileName.Substring(fufImagemAlbum.FileName.LastIndexOf("\\")+1);
                        if (!ValidaExtensaoImagem(foto.Extensao))
                        {
                            base.MostrarMensagem("Capa de Album", "Extensão de imagem " + foto.Extensao + " não suportada.", "");
                            return;
                        }
                    }
                    foto.Removido = false;
                }

                galeria.Descricao = txtDescricao.Text;
                galeria.Nome = txtNome.Text;
                galeria.Removido = false;

                galeria = (GaleriaVO)new GaleriaBO(galeria).Salvar();
                if (foto != null)
                {
                    foto.Titulo = "Foto de Album da Galeria.";
                    foto.Galeria = galeria;
                    foto.CapaAlbum = true;
                    foto = (FotoVO)new FotoBO(foto).Salvar();

                    Bitmap imgOriginal = System.Drawing.Image.FromStream(fufImagemAlbum.FileContent) as Bitmap;
                    Int32 width = imgOriginal.Width > imgOriginal.Height ? 80 : 60;
                    Int32 heigth = imgOriginal.Height > imgOriginal.Width ? 80 : 60;
                    Bitmap imgThumb = new Bitmap(width, heigth);
                    Graphics g = Graphics.FromImage(imgThumb);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgThumb.Width, imgThumb.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    // Grava imagem redimensionada
                    String pathThumb = Path.Combine(Server.MapPath("~/GaleriaImagens/FotosThumbs"), foto.Id + foto.Extensao);
                    String pathOriginal = Path.Combine(Server.MapPath("~/GaleriaImagens/FotosOriginais"), foto.Id + foto.Extensao);
                    imgThumb.Save(pathThumb, imgOriginal.RawFormat);
                    fufImagemAlbum.PostedFile.SaveAs(pathOriginal);
                }

				base.MostrarMensagem("Galeria", "Galeria gravada com sucesso.", String.Empty);
                LoadPagina();
                winGaleria.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar galeria.";
                e.Success = false;
            }            
        }

        private void SalvarFoto(DirectEventArgs e)
        {
            try
            {
                FotoVO foto = new FotoVO();
                if (this.AcaoTelaFoto == Common.AcaoTela.Edicao)
                    foto = FotoSelecionada;

                foto.Titulo = txtTituloFoto.Text;
                foto.CapaAlbum = chkCapaAlbum.Checked;
                if (!fufImagem.Disabled && !fufImagem.FileName.IsNullOrEmpty())
                {
                    foto.Extensao = fufImagem.FileName.Substring(fufImagem.FileName.LastIndexOf("."));
                    foto.NomeOriginal = fufImagem.FileName.Substring(fufImagem.FileName.LastIndexOf("\\")+1);
                    if (!ValidaExtensaoImagem(foto.Extensao))
                    {
                        base.MostrarMensagem("Imagem", "Extensão de imagem " + foto.Extensao + " não suportada.", "");
                        return;
                    }
                }
                foto.Galeria = new GaleriaVO() { Id = cboGaleria.Value.ToInt32() };
                foto.Removido = false;

                if (foto.CapaAlbum)
                {
                    new FotoBO().AtualizarCapaAlbum(foto.Galeria);
                }
                
                foto = (FotoVO)new FotoBO(foto).Salvar();

                if (!fufImagem.Disabled && !fufImagem.FileName.IsNullOrEmpty())
                {
                    Bitmap imgOriginal = System.Drawing.Image.FromStream(fufImagem.FileContent) as Bitmap;
                    Dictionary<Int32, Int32> dicSize = GetSizePhoto(imgOriginal);
                    Int32 width = dicSize.FirstOrDefault().Value;
                    Int32 heigth = dicSize.FirstOrDefault().Key; 
                    Bitmap imgThumb = new Bitmap(width,heigth);
                    Graphics g = Graphics.FromImage(imgThumb);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgThumb.Width, imgThumb.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    // Grava imagem redimensionada
                    String pathThumb = Path.Combine(Server.MapPath("~/GaleriaImagens/FotosThumbs"), foto.Id + foto.Extensao);
                    String pathOriginal = Path.Combine(Server.MapPath("~/GaleriaImagens/FotosOriginais"), foto.Id + foto.Extensao);
                    imgThumb.Save(pathThumb, imgOriginal.RawFormat);
                    fufImagem.PostedFile.SaveAs(pathOriginal);
                }

				base.MostrarMensagem("Foto", "Foto gravada com sucesso", String.Empty);

                LoadPagina();
                winFoto.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar foto.";
                e.Success = false;
            }
        }

        [DirectMethod]
        public void AdicionarFoto(String id)
        {
            LimparCamposFoto();
            GaleriaSelecionado = new GaleriaBO().SelectById(id.ToInt32());
            this.AcaoTelaFoto = Common.AcaoTela.Inclusao;
            winFoto.Title = "Cadastrando Foto";
            CarregarGalerias(GaleriaSelecionado);
            cboGaleria.SetValue(GaleriaSelecionado.Id);
            winFoto.Show();
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
            FotoVO fotoAlbum = GaleriaSelecionado.Fotos.FirstOrDefault(x => x.CapaAlbum == true);
            txtNome.Text = GaleriaSelecionado.Nome;
            txtDescricao.Text = GaleriaSelecionado.Descricao;
            imgAtual.Hidden = fotoAlbum == null;
            fufImagemAlbum.Hidden = true;
            fufImagemAlbum.AllowBlank = true;
            if(fotoAlbum != null)
                imgAtual.ImageUrl = fotoAlbum.CaminhoImagemThumb;
        }

        private void PreencherCamposFoto(DirectEventArgs e)
        {
            FotoSelecionada = new FotoBO().SelectById(e.ExtraParams["id"].ToInt32());
            CarregarGalerias(GaleriaSelecionado);
            GaleriaSelecionado = FotoSelecionada.Galeria;
            txtTituloFoto.Text = FotoSelecionada.Titulo;
            cboGaleria.SetValue(GaleriaSelecionado.Id);
            fufImagem.Disabled = true;
            fufImagem.AllowBlank = true;
            chkCapaAlbum.Checked = FotoSelecionada.CapaAlbum;
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
            txtNome.Clear();
            imgAtual.Hidden = true;
            fufImagemAlbum.Hidden = false;
            fufImagemAlbum.AllowBlank = false;
            fufImagemAlbum.Clear();
        }

        private void LimparCamposFoto()
        {
            cboGaleria.Clear();
            txtTituloFoto.Clear();
            fufImagem.AllowBlank = false;
            fufImagem.Disabled = false;
            fufImagem.Clear();
            chkCapaAlbum.Clear();
        }

        private void CarregarGalerias(GaleriaVO galeria)
        {
            strGalerias.DataSource = new GaleriaBO().Select().Where(x => x.Removido == false || (galeria != null && x.Id == galeria.Id)).ToList();
            strGalerias.DataBind();
        }

        private Boolean ValidaExtensaoImagem(String extensao)
        {
            return base.LstExtensaoImagens.Any(x => x.ToLower() == extensao.ToLower());
        }

        private Dictionary<int, int> GetSizePhoto(System.Drawing.Bitmap img)
        {
            int oWidth = img.Width; // largura original
            int oHeight = img.Height; // altura original
            int nWidth = 80;
            int nHeight = 60;
            Dictionary<Int32, Int32> dicSize = new Dictionary<Int32, Int32>();

            // redimensiona se necessario
            if (oWidth > nWidth || oHeight > nHeight)
            {

                if (oWidth > oHeight)
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
