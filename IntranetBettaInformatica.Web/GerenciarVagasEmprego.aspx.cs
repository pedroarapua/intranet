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
using System.IO;

namespace IntranetBettaInformatica.Web
{
	public partial class GerenciarVagasEmprego : BasePage
	{
		#region propriedades

		public VagaEmpregoVO VagaEmpregoSelecionada
		{
			get
			{
				if (this.ViewState["VagaEmpregoSelecionada"] == null)
					this.ViewState["VagaEmpregoSelecionada"] = new VagaEmpregoVO();
				return (VagaEmpregoVO)this.ViewState["VagaEmpregoSelecionada"];
			}
			set
			{
				this.ViewState["VagaEmpregoSelecionada"] = value;
			}
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
			base.ResourceManager.RegisterIcon(Ext.Net.Icon.RecordGreen);
			base.ResourceManager.RegisterIcon(Ext.Net.Icon.BulletShape);
            if (!IsPostBack)
            {
                LoadPagina();
				base.SetTituloIconePagina(frmTitulo);
				btnNovo.Disabled = !hdfAdicionarVagaEmprego.Value.ToInt32().ToBoolean();
            }
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverVagaEmprego(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarVagaEmprego(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winVagaEmprego.Title = "Cadastrando Vaga de Emprego";
            LimparCampos();
            winVagaEmprego.Show();
            
        }

		protected void btnEditar_Click(object sender, DirectEventArgs e)
		{
			PreencherCampos(e);
			winVagaEmprego.Title = "Alterando Vaga de Emprego";
			winVagaEmprego.Show((Control)sender);
		}

		protected void btnCurriculos_Click(object sender, DirectEventArgs e)
		{
			AbrirCurriculos(sender, e);
		}

		protected void btnFecharVaga_Click(object sender, DirectEventArgs e)
		{
			FecharVagaEmprego(e);
		}

		protected void btnIndicarVaga_Click(object sender, DirectEventArgs e)
		{
			LimparCamposIndicar();
			winIndicar.Show((Control)sender);
		}

		protected void btnDowload_Click(object sender, DirectEventArgs e)
		{
			Download(e);
		}

		protected void btnSalvarCurriculo_Click(object sender, DirectEventArgs e)
		{
			SalvarCurriculo(e);
		}
		

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
			strVagasEmprego.DataSource = new VagaEmpregoBO().Select().Where(x => x.Removido == false).OrderByDescending(x=> x.Id).Select(x => new { Codigo = x.Codigo, Descricao = x.Descricao, Id = x.Id, QtdIndicacoes = x.QtdIndicacoes, Status = x.Status, Titulo = x.Titulo, StatusIcon = String.Format("icon-{0}", x.Status.ToInt32() == 1 ? "recordgreen" : "bulletshape"), StatusId = x.Status.ToInt32() }).ToList();
			strVagasEmprego.DataBind();
        }

        private void RemoverVagaEmprego(DirectEventArgs e)
        {
            try
            {
                VagaEmpregoVO vagaEmprego = new VagaEmpregoBO().SelectById(e.ExtraParams["id"].ToInt32());
				new VagaEmpregoBO(vagaEmprego).DeleteUpdate();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover vaga de emprego.", "");
            }
        }

        private void SalvarVagaEmprego(DirectEventArgs e)
        {
            try
            {
				VagaEmpregoSelecionada.Titulo = txtTitulo.Text;
				VagaEmpregoSelecionada.Descricao = txtDescricao.Text;
				VagaEmpregoSelecionada.Status = EStatusVagaEmprego.Ativa;
				VagaEmpregoSelecionada.DataAtualizacao = DateTime.Now;
				new VagaEmpregoBO(VagaEmpregoSelecionada).Salvar();

				base.MostrarMensagem("Sucesso", "Vaga de emprego gravada com sucesso.", String.Empty);
                LoadPagina();
                winVagaEmprego.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar vaga de emprego.";
                e.Success = false;
            }            
        }

        private void LimparCampos()
        {
			VagaEmpregoSelecionada = null;
            txtTitulo.Clear();
			txtDescricao.Clear();
			txtTitulo.ClearInvalid();
			txtDescricao.ClearInvalid();
        }

		private void LimparCamposIndicar()
        {
            txtNome.Clear();
			txtCurriculo.Clear();
			txtNome.ClearInvalid();
			txtCurriculo.ClearInvalid();
        }

		private void PreencherCampos(DirectEventArgs e)
		{
			VagaEmpregoSelecionada = new VagaEmpregoBO().SelectById(e.ExtraParams["id"].ToInt32());
			txtTitulo.Text = VagaEmpregoSelecionada.Titulo;
			txtDescricao.Text = VagaEmpregoSelecionada.Descricao;
		}

		private void FecharVagaEmprego(DirectEventArgs e)
		{
			try
			{
				VagaEmpregoVO vaga = new VagaEmpregoBO().SelectById(e.ExtraParams["id"].ToInt32());
				vaga.Status = EStatusVagaEmprego.Fechada;
				new VagaEmpregoBO(vaga).Salvar();
				RowSelectionModel sm = this.grdVagasEmprego.SelectionModel.Primary as RowSelectionModel;
				sm.ClearSelections();
				LoadPagina();
				base.MostrarMensagem("Sucesso", "Vaga de emprego fechada com sucesso", String.Empty);
			}
			catch (Exception ex)
			{
				base.MostrarMensagem("Erro", "Erro ao tentar fechar a vaga de emprego", String.Empty);
			}

		}

		private void AbrirCurriculos(object sender, DirectEventArgs e)
		{
			try
			{
				VagaEmpregoSelecionada = new VagaEmpregoBO().SelectById(e.ExtraParams["id"].ToInt32());
				strCurriculos.DataSource = VagaEmpregoSelecionada.Curriculos.ToList();
				strCurriculos.DataBind();
				winCurriculos.Show((Control)sender);
			}
			catch (Exception ex)
			{
				base.MostrarMensagem("Erro", "Erro ao buscar curriculos da vaga de emprego", String.Empty);
			}
		}

		private void Download(DirectEventArgs e)
		{
			VagaEmpregoCurriculoVO curriculo = new VagaEmpregoCurriculoBO().SelectById(e.ExtraParams["id"].ToInt32());
			String caminho = Path.Combine(Server.MapPath("~/CurriculosEmprego"), curriculo.Id + curriculo.Extensao);
			FileInfo file = new FileInfo(caminho);
			if (file.Exists)
			{
				Response.Clear();
				Response.ContentType = "application/octet-stream";
				Response.AddHeader("Content-Disposition", "attachment;filename=" + curriculo.NomeOriginal);
				Response.AddHeader("Content-Length", file.Length.ToString());
				Response.Flush();
				Response.WriteFile(caminho);
			}
		}

		private void SalvarCurriculo(DirectEventArgs e)
		{
			try
			{
				VagaEmpregoCurriculoVO curriculo = new VagaEmpregoCurriculoVO();
				curriculo.Nome = txtNome.Text;
				curriculo.VagaEmprego = new VagaEmpregoVO() { Id = e.ExtraParams["id"].ToInt32() }; 
				if (!txtCurriculo.Disabled && !txtCurriculo.FileName.IsNullOrEmpty())
				{
					curriculo.Extensao = txtCurriculo.FileName.Substring(txtCurriculo.FileName.LastIndexOf("."));
					curriculo.NomeOriginal = txtCurriculo.FileName.Substring(txtCurriculo.FileName.LastIndexOf("\\") + 1);
					if (!ValidaExtensaoCurriculo(curriculo.Extensao))
					{
						base.MostrarMensagem("Erro", "Extensão de currículo " + curriculo.Extensao + " não suportada.", "");
						return;
					}
				}

				curriculo = (VagaEmpregoCurriculoVO)new VagaEmpregoCurriculoBO(curriculo).Salvar();
				if (curriculo != null)
				{
					String pathOriginal = Path.Combine(Server.MapPath("~/CurriculosEmprego"), curriculo.Id + curriculo.Extensao);
					txtCurriculo.PostedFile.SaveAs(pathOriginal);
				}

				winIndicar.Hide();
				LoadPagina();
				base.MostrarMensagem("Sucesso", "Currículo enviado com sucesso", String.Empty);
			}
			catch (Exception ex)
			{
				base.MostrarMensagem("Erro", "Erro ao enviar currículo", String.Empty);
			}

		}

		private Boolean ValidaExtensaoCurriculo(String extensao)
		{
			return base.LstExtensaoCurriculo.Any(x => x.ToLower() == extensao.ToLower());
		}

		#endregion
    }
}
