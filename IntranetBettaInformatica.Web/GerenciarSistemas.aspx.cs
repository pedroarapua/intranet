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
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarSistemas : BasePage
    {

        #region propriedades

        private SistemaVO SistemaSelecionado
        {
            get
            {
                if (this.ViewState["SistemaSelecionado"] == null)
                    return null;
                return (SistemaVO)this.ViewState["SistemaSelecionado"];
            }
            set { this.ViewState["SistemaSelecionado"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarSistemas.Value.ToInt32().ToBoolean();
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
            SalvarSistema(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winSistema.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winSistema.Title = "Cadastrando Sistema";
            LimparCampos();
            winSistema.Show((Control)sender);
            
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winSistema.Title = "Alterando Sistema";
            winSistema.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strSistemas.DataSource = new SistemaBO().Select().Where(x=> x.Removido == false).ToList();
            strSistemas.DataBind();
        }

        private void RemoverThema(DirectEventArgs e)
        {
            try
            {
                SistemaVO sistema = JSON.Deserialize<List<SistemaVO>>(e.ExtraParams["valores"])[0];
                new SistemaBO(sistema).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover sistema.", "");
            }
        }

        private void SalvarSistema(DirectEventArgs e)
        {
            try
            {
                SistemaVO sistema = new SistemaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    sistema = SistemaSelecionado;
                
                String extensao = String.Empty;
                if (ckbImagem.Checked)
                {
                    if (!fufImagem.FileName.IsNullOrEmpty())
                        extensao = fufImagem.FileName.Substring(fufImagem.FileName.LastIndexOf("."));
                    else if (!imgAtual.ImageUrl.IsNullOrEmpty())
                        extensao = imgAtual.ImageUrl.Substring(imgAtual.ImageUrl.LastIndexOf("."));
                }
                if (ckbImagem.Checked && !extensao.IsNullOrEmpty() && !ValidaExtensaoImagem(extensao))
                {
                    base.MostrarMensagem("Imagem", "Extensão de imagem " + extensao + " não suportada.", "");
                    return;
                }

                sistema.Nome = txtNome.Text;
                sistema.Url = txtUrl.Text;
                sistema.ExtensaoImagem = extensao;

                sistema = (SistemaVO)new SistemaBO(sistema).Salvar();

                if (ckbImagem.Checked && !fufImagem.FileName.IsNullOrEmpty())
                {
                    Bitmap imgOriginal = System.Drawing.Image.FromStream(fufImagem.FileContent) as Bitmap;
                    Bitmap imgModificada = new Bitmap(32, 32);
                    Graphics g = Graphics.FromImage(imgModificada);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgModificada.Width, imgModificada.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    // Grava imagem redimensionada
                    String path = Path.Combine(Server.MapPath("~/Sistemas"), sistema.Id + sistema.ExtensaoImagem);
                    imgModificada.Save(path, imgOriginal.RawFormat);
                }

                base.MostrarMensagem("Sucesso", "Sistema gravado com sucesso.", "");

                LoadPagina();
                winSistema.Hide();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar sistema.", "");
            }            
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            SistemaSelecionado = JSON.Deserialize<List<SistemaVO>>(e.ExtraParams["valores"])[0];
            txtNome.Text = SistemaSelecionado.Nome;
            txtUrl.Text = SistemaSelecionado.Url;
            imgAtual.Hidden = SistemaSelecionado.ExtensaoImagem.IsNullOrEmpty();
            ckbImagem.Checked = !imgAtual.Hidden;
            fufImagem.Disabled = !ckbImagem.Checked;
            fufImagem.Clear();
            if (!imgAtual.Hidden)
                imgAtual.ImageUrl = SistemaSelecionado.CaminhoImagem;
        }

        private void LimparCampos()
        {
            ckbImagem.Checked = false;
            imgAtual.Hidden = true;
            fufImagem.Disabled = true;
            fufImagem.Clear();
            txtNome.Clear();
            txtUrl.Clear();
			SistemaSelecionado = null;
            //fufImagem.Note = "Preferencialmente com tamanho 36x36, ";
            //fufImagem.Note += "caso o tamanho não seja este a imagem será modficiada para este tamanho. ";
            //fufImagem.Note += "Extensões permitidas: " + String.Join(",", base.LstExtensaoImagens); 
        }

        private Boolean ValidaExtensaoImagem(String extensao)
        {
            return base.LstExtensaoImagens.Any(x => x.ToLower() == extensao.ToLower());
        }

        #endregion
    }
}
