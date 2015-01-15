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
    public partial class GerenciarArquivos : BasePage
    {

        #region propriedades

        public AcaoTela AcaoTelaArquivo
        {
            get
            {
                if (this.ViewState["AcaoTelaArquivo"] == null)
                    return AcaoTela.Inclusao;
                return (AcaoTela)this.ViewState["AcaoTelaArquivo"];
            }
            set { this.ViewState["AcaoTelaArquivo"] = value; }
        }

        private TipoArquivoVO TipoSelecionado
        {
            get
            {
                if (this.ViewState["TipoSelecionado"] == null)
                    return null;
                return (TipoArquivoVO)this.ViewState["TipoSelecionado"];
            }
            set { this.ViewState["TipoSelecionado"] = value; }
        }

        private ArquivoVO ArquivoSelecionado
        {
            get
            {
                if (this.ViewState["ArquivoSelecionado"] == null)
                    return null;
                return (ArquivoVO)this.ViewState["ArquivoSelecionado"];
            }
            set { this.ViewState["ArquivoSelecionado"] = value; }
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
			btnNovo.Disabled = hdfAdicionarPastaBancoArquivos.Value.ToInt32() == 0;
			btnNovoArquivo.Disabled = hdfAdicionarArquivoBancoArquivos.Value.ToInt32() == 0;
			btnDownload.Disabled = hdfDownloadArquivoBancoArquivos.Value.ToInt32() == 0;
	    }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemoverArquivo_Click(object sender, DirectEventArgs e)
        {
            RemoverArquivo(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarTipo(e);
        }

        protected void SalvarArquivo_Click(object sender, DirectEventArgs e)
        {
            SalvarArquivo(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winTipoArquivo.Title = "Cadastrando Pasta";
            LimparCampos();
            winTipoArquivo.Show((Control)sender);
        }

        protected void btnNovoArquivo_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaArquivo = Common.AcaoTela.Inclusao;
            winArquivo.Title = "Cadastrando Arquivo";
            CarregarTipos(null);
            LimparCamposArquivo();
            winArquivo.Show((Control)sender);
        }

        protected void btnEditarArquivo_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaArquivo = Common.AcaoTela.Edicao;
            PreencherCamposArquivo(e);
            winArquivo.Title = "Alterando Arquivo";
            winArquivo.Show((Control)sender);
        }

        protected void btnDowloadArquivo_Click(object sender, DirectEventArgs e)
        {
            DownloadArquivo(e);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strArquivos.DataSource = new ArquivoBO().Select().Where(x=> x.Tipo.Removido == false).ToList();
            strArquivos.DataBind();
        }

        [DirectMethod]
        public void RemoverTipo(String id)
        {
            try
            {
                TipoArquivoVO tipo = new TipoArquivoBO().SelectById(id.ToInt32());
                new TipoArquivoBO(tipo).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover pasta.", "");
            }
        }

        private void RemoverArquivo(DirectEventArgs e)
        {
            try
            {
                ArquivoVO arquivo = new ArquivoBO().SelectById(e.ExtraParams["id"].ToInt32());
                new ArquivoBO(arquivo).DeleteUpdate();
                LoadPagina();
                btnRemover.Disabled = btnEditar.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover arquivo.", "");
            }
        }

        private void SalvarTipo(DirectEventArgs e)
        {
            try
            {
                TipoArquivoVO tipo = new TipoArquivoVO();
                ArquivoVO arquivo = null;
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    tipo = TipoSelecionado;
                else
                {
                    arquivo = new ArquivoVO();
                    if (!fufArquivoTipo.Disabled && !fufArquivoTipo.FileName.IsNullOrEmpty())
                    {
                        arquivo.Extensao = fufArquivoTipo.FileName.Substring(fufArquivoTipo.FileName.LastIndexOf("."));
                        arquivo.NomeOriginal = fufArquivoTipo.FileName.Substring(fufArquivoTipo.FileName.LastIndexOf("\\") + 1);
                    }
                    arquivo.Removido = false;
                }

                tipo.Nome = txtTipoNome.Text;
                tipo.Removido = false;

                tipo = (TipoArquivoVO)new TipoArquivoBO(tipo).Salvar();
                if (arquivo != null)
                {
                    arquivo.Nome = txtNome.Text;
                    arquivo.Descricao = txtDescricao.Text;
                    arquivo.Tipo = tipo;
                    arquivo = (ArquivoVO)new ArquivoBO(arquivo).Salvar();

                    // Grava arquivo no repositorio
                    String pathOriginal = Path.Combine(Server.MapPath("~/BancoArquivos"), arquivo.Id + arquivo.Extensao);
                    fufArquivoTipo.PostedFile.SaveAs(pathOriginal);
                }
                base.MostrarMensagem("Pasta de Arquivo","Pasta e arquivos gravados com sucesso", String.Empty);
                LoadPagina();
                winTipoArquivo.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar pasta e arquivo.";
                e.Success = false;
            }            
        }

        private void SalvarArquivo(DirectEventArgs e)
        {
            try
            {
                ArquivoVO arquivo = new ArquivoVO();
                if (this.AcaoTelaArquivo == Common.AcaoTela.Edicao)
                    arquivo = ArquivoSelecionado;

                arquivo.Nome = txtNomeArquivo.Text;
                arquivo.Descricao = txtDescricaoArquivo.Text;
                if (!fufArquivo.Disabled && !fufArquivo.FileName.IsNullOrEmpty())
                {
                    arquivo.Extensao = fufArquivo.FileName.Substring(fufArquivo.FileName.LastIndexOf("."));
                    arquivo.NomeOriginal = fufArquivo.FileName.Substring(fufArquivo.FileName.LastIndexOf("\\") + 1);
                }
                arquivo.Tipo = new TipoArquivoVO() { Id = cboTipo.Value.ToInt32() };
                arquivo.Removido = false;

                arquivo = (ArquivoVO)new ArquivoBO(arquivo).Salvar();

                if (!fufArquivo.Disabled && !fufArquivo.FileName.IsNullOrEmpty())
                {
                    // Grava arquivo no repositorio
                    String pathOriginal = Path.Combine(Server.MapPath("~/BancoArquivos"), arquivo.Id + arquivo.Extensao);
                    fufArquivo.PostedFile.SaveAs(pathOriginal);
                }

				base.MostrarMensagem("Arquivo","Arquivo gravado com sucesso", String.Empty);

                LoadPagina();
                winArquivo.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar arquivo.";
                e.Success = false;
            }
        }

        [DirectMethod]
        public void AdicionarArquivo(String id)
        {
            LimparCamposArquivo();
            TipoSelecionado = new TipoArquivoBO().SelectById(id.ToInt32());
            this.AcaoTelaArquivo = Common.AcaoTela.Inclusao;
            winArquivo.Title = "Cadastrando Arquivo";
            CarregarTipos(TipoSelecionado);
            cboTipo.SetValue(TipoSelecionado.Id);
            winArquivo.Show();
        }

        [DirectMethod]
        public void EditarTipo(String id)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(id);
            winTipoArquivo.Title = "Alterando Pasta";
            winTipoArquivo.Show();
        }

        private void PreencherCampos(String id)
        {
            TipoSelecionado = new TipoArquivoBO().SelectById(id.ToInt32());
            txtTipoNome.Text = TipoSelecionado.Nome;

            txtNome.Hidden = true;
            txtDescricao.Hidden = true;
            txtNome.AllowBlank = true;

            fufArquivoTipo.Hidden = true;
            fufArquivoTipo.AllowBlank = true;

            winTipoArquivo.Height = 120;
        }

        private void PreencherCamposArquivo(DirectEventArgs e)
        {
            ArquivoSelecionado = new ArquivoBO().SelectById(e.ExtraParams["id"].ToInt32());
            TipoSelecionado = ArquivoSelecionado.Tipo;
            CarregarTipos(TipoSelecionado);
            
            txtNomeArquivo.Text = ArquivoSelecionado.Nome;
            txtDescricaoArquivo.Text = ArquivoSelecionado.Descricao;
            cboTipo.SetValue(TipoSelecionado.Id);
            fufArquivo.Disabled = true;
            fufArquivo.AllowBlank = true;
        }

        private void LimparCampos()
        {
            txtDescricao.Clear();
            txtNome.Clear();
            txtTipoNome.Clear();
            txtNome.Hidden = false;
            txtNome.AllowBlank = false;
            txtDescricao.Hidden = false;
            fufArquivoTipo.Hidden = false;
            fufArquivoTipo.AllowBlank = false;
            fufArquivoTipo.Clear();
            winTipoArquivo.Height = 270;
        }

        private void LimparCamposArquivo()
        {
            cboTipo.Clear();
            txtNomeArquivo.Clear();
            txtDescricaoArquivo.Clear();
            fufArquivo.AllowBlank = false;
            fufArquivo.Disabled = false;
            fufArquivo.Clear();
        }

        private void CarregarTipos(TipoArquivoVO tipo)
        {
            strTipos.DataSource = new TipoArquivoBO().Select().Where(x => x.Removido == false || (tipo != null && x.Id == tipo.Id)).ToList();
            strTipos.DataBind();
        }

        private void DownloadArquivo(DirectEventArgs e)
        {
            ArquivoVO arquivo = new ArquivoBO().SelectById(e.ExtraParams["id"].ToInt32());
            String caminho = Path.Combine(Server.MapPath("~/BancoArquivos"), arquivo.Id + arquivo.Extensao);
            FileInfo file = new FileInfo(caminho);
            if (file.Exists)
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + arquivo.NomeOriginal);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.Flush();
                Response.WriteFile(caminho);
            }
        }

        #endregion
    }
}
