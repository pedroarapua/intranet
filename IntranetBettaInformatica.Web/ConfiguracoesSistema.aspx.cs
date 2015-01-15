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
    public partial class ConfiguracoesSistema : BasePage
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
                base.SetTituloIconePagina(frmTitulo);
            }
			btnSalvar.Disabled = hdfSalvarVisualizarConfiguracoes.Value.ToInt32() == 0;
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarSistema(e);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            txtDescricao.Text = base.Configuracao.Descricao;
            txtSmtp.Text = base.Configuracao.ServidoSmtp;
            txtLogin.Text = base.Configuracao.Login;
            txtSenha.Text = base.Configuracao.Senha;
            txtConfSenha.Text = base.Configuracao.Senha;
            imgAtual.Hidden = base.Configuracao.ExtensaoImagem.IsNullOrEmpty();
            ckbImagem.Checked = !imgAtual.Hidden;
            fufImagem.Disabled = !ckbImagem.Checked;
            fufImagem.Clear();
            if (!imgAtual.Hidden)
                imgAtual.ImageUrl = base.Configuracao.CaminhoImagem;    
        }

        private void SalvarSistema(DirectEventArgs e)
        {
            try
            {
                String extensao = String.Empty;
                if (ckbImagem.Checked)
                {
                    if (!fufImagem.FileName.IsNullOrEmpty())
                        extensao = fufImagem.FileName.Substring(fufImagem.FileName.LastIndexOf("."));
                    else if(!imgAtual.ImageUrl.IsNullOrEmpty())
                        extensao = imgAtual.ImageUrl.Substring(imgAtual.ImageUrl.LastIndexOf("."));
                }
                if (ckbImagem.Checked && !extensao.IsNullOrEmpty() && !fufImagem.FileName.IsNullOrEmpty() && !ValidaExtensaoImagem(extensao))
                {
                    base.MostrarMensagem("Imagem", "Extensão de imagem " + extensao + " não suportada.","");
                    return;
                }

                base.Configuracao.Descricao = txtDescricao.Text;
                base.Configuracao.Login = txtLogin.Text;
                base.Configuracao.Senha = txtSenha.Text;
                base.Configuracao.ServidoSmtp = txtSmtp.Text;
                base.Configuracao.ExtensaoImagem = extensao;

                new ConfiguracoesSistemaBO(base.Configuracao).Salvar();

                if (ckbImagem.Checked && !fufImagem.FileName.IsNullOrEmpty())
                {
                    Bitmap imgOriginal = System.Drawing.Image.FromStream(fufImagem.FileContent) as Bitmap;
                    Bitmap imgModificada = new Bitmap(imgOriginal.Width > 140 ? 140 : imgOriginal.Width, imgOriginal.Height > 85 ? 85 : imgOriginal.Height);
                    Graphics g = Graphics.FromImage(imgModificada);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgModificada.Width, imgModificada.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    // Grava imagem redimensionada
                    String path = Path.Combine(Server.MapPath("~/ConfiguracoesSistema"), base.Configuracao.Id + base.Configuracao.ExtensaoImagem);
                    imgModificada.Save(path, imgOriginal.RawFormat);
                }

				base.MostrarMensagem("Sucesso", "Configurações gravadas com sucesso.", "ConfiguracoesSistema.aspx");
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar configurações do sistema.", "");
            }            
        }

        private Boolean ValidaExtensaoImagem(String extensao)
        {
            return base.LstExtensaoImagens.Any(x => x.ToLower() == extensao.ToLower());
        }

        #endregion
    }
}
