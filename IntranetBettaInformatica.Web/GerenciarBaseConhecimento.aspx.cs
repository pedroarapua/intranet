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
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Web
{
	public partial class GerenciarBaseConhecimento : BasePage
    {

        #region propriedades

		public AcaoTela AcaoTelaBaseConhecimento
        {
            get
            {
				if (this.ViewState["AcaoTelaBaseConhecimento"] == null)
                    return AcaoTela.Inclusao;
				return (AcaoTela)this.ViewState["AcaoTelaBaseConhecimento"];
            }
			set { this.ViewState["AcaoTelaBaseConhecimento"] = value; }
        }

        private TopicoBaseConhecimentoVO TopicoSelecionado
        {
            get
            {
				if (this.ViewState["TopicoSelecionado"] == null)
                    return null;
				return (TopicoBaseConhecimentoVO)this.ViewState["TopicoSelecionado"];
            }
			set { this.ViewState["TopicoSelecionado"] = value; }
        }

        private BaseConhecimentoVO BaseConhecimentoSelecionado
        {
            get
            {
				if (this.ViewState["BaseConhecimentoSelecionado"] == null)
                    return null;
				return (BaseConhecimentoVO)this.ViewState["BaseConhecimentoSelecionado"];
            }
			set { this.ViewState["BaseConhecimentoSelecionado"] = value; }
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
			btnNovo.Disabled = hdfAdicionarTopicoBaseConhecimento.Value.ToInt32() == 0;
			btnNovoConhecimento.Disabled = hdfAdicionarBaseConhecimento.Value.ToInt32() == 0;

			ImageCommandColumn columnCommand = (grdBaseConhecimento.ColumnModel.Columns[grdBaseConhecimento.ColumnModel.Columns.Count - 1] as ImageCommandColumn);

			columnCommand.GroupCommands[0].Hidden = !hdfAdicionarBaseConhecimento.Value.ToInt32().ToBoolean();
			columnCommand.GroupCommands[1].Hidden = !hdfRemoverTopicoBaseConhecimento.Value.ToInt32().ToBoolean();
			columnCommand.GroupCommands[2].Hidden = !hdfEditarTopicoBaseConhecimento.Value.ToInt32().ToBoolean();
		}

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

		protected void btnRemoverConhecimento_Click(object sender, DirectEventArgs e)
        {
			RemoverConhecimento(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarTopico(e);
        }

		protected void SalvarConhecimento_Click(object sender, DirectEventArgs e)
        {
			SalvarConhecimento(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winTopico.Title = "Cadastrando Tópico";
            LimparCampos();
			winTopico.Show((Control)sender);
        }

        protected void btnNovoConhecimento_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaBaseConhecimento = Common.AcaoTela.Inclusao;
            winBaseConhecimento.Title = "Cadastrando Conhecimento";
            CarregarTopicos();
            LimparCamposConhecimento();
            winBaseConhecimento.Show((Control)sender);
        }

        protected void btnEditarConhecimento_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaBaseConhecimento = Common.AcaoTela.Edicao;
            PreencherCamposConhecimento(e);
            winBaseConhecimento.Title = "Alterando Conhecimento";
			winBaseConhecimento.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strBaseConhecimento.DataSource = new BaseConhecimentoBO().Select();
			strBaseConhecimento.DataBind();
        }

        [DirectMethod]
        public void RemoverTopico(String id)
        {
            try
            {
				TopicoBaseConhecimentoVO topico = new TopicoBaseConhecimentoBO().SelectById(id.ToInt32());
				new TopicoBaseConhecimentoBO(topico).Delete();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover tópico.", "");
            }
        }

        private void RemoverConhecimento(DirectEventArgs e)
        {
            try
            {
                BaseConhecimentoVO item = new BaseConhecimentoBO().SelectById(e.ExtraParams["id"].ToInt32());
				new BaseConhecimentoBO(item).Delete();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover conhecimento.", "");
            }
        }

        private void SalvarTopico(DirectEventArgs e)
        {
            try
            {
				TopicoBaseConhecimentoVO topico = new TopicoBaseConhecimentoVO();
                BaseConhecimentoVO conhecimento = null;
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    topico = TopicoSelecionado;
                else
                {
					conhecimento = new BaseConhecimentoVO();
					conhecimento.Titulo = txtConhecimentoTopico.Text;
                }

                topico.Titulo = txtTitulo.Text;

				topico = (TopicoBaseConhecimentoVO)new TopicoBaseConhecimentoBO(topico).Salvar();
				if (conhecimento != null)
                {
					conhecimento.Topico = topico;
					new BaseConhecimentoBO(conhecimento).Salvar();
                }
                base.MostrarMensagem("Tópico de Conhecimento","Tópico e conhecimento gravados com sucesso", String.Empty);
                LoadPagina();
                winTopico.Hide();
            }
            catch (Exception ex)
            {
				base.MostrarMensagem("Erro", "Erro ao salvar tópico e conhecimento.", String.Empty);
            }            
        }

        private void SalvarConhecimento(DirectEventArgs e)
        {
            try
            {
				BaseConhecimentoVO item = new BaseConhecimentoVO();
                if (this.AcaoTelaBaseConhecimento == Common.AcaoTela.Edicao)
                    item = BaseConhecimentoSelecionado;

				item.Titulo = txtConhecimento.Text;
				item.Topico = new TopicoBaseConhecimentoVO() { Id = cboTopico.Value.ToInt32() };

				new BaseConhecimentoBO(item).Salvar();

                base.MostrarMensagem("Conhecimento","Conhecimento gravado com sucesso", String.Empty);

                LoadPagina();
                winBaseConhecimento.Hide();
            }
            catch (Exception ex)
            {
				base.MostrarMensagem("Erro", "Erro ao salvar conhecimento.", String.Empty);
            }
        }

        [DirectMethod]
        public void AdicionarConhecimento(String id)
        {
            LimparCamposConhecimento();
			TopicoSelecionado = new TopicoBaseConhecimentoBO().SelectById(id.ToInt32());
            this.AcaoTelaBaseConhecimento = Common.AcaoTela.Inclusao;
            winBaseConhecimento.Title = "Cadastrando Conhecimento";
            CarregarTopicos();
            cboTopico.SetValue(TopicoSelecionado.Id);
            winBaseConhecimento.Show();
        }

        [DirectMethod]
        public void EditarTopico(String id)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(id);
			winTopico.Title = "Alterando Tópico";
			winTopico.Show();
        }

        private void PreencherCampos(String id)
        {
			TopicoSelecionado = new TopicoBaseConhecimentoBO().SelectById(id.ToInt32());
            txtTitulo.Text = TopicoSelecionado.Titulo;
			txtConhecimentoTopico.AllowBlank = true;
            txtConhecimentoTopico.Hidden = true;
            winTopico.Height = 120;
        }

        private void PreencherCamposConhecimento(DirectEventArgs e)
        {
			BaseConhecimentoSelecionado = new BaseConhecimentoBO().SelectById(e.ExtraParams["id"].ToInt32());
			TopicoSelecionado = BaseConhecimentoSelecionado.Topico;
            CarregarTopicos();

			txtConhecimento.Text = BaseConhecimentoSelecionado.Titulo;
            cboTopico.SetValue(TopicoSelecionado.Id);
        }

        private void LimparCampos()
        {
            txtTitulo.Clear();
			txtConhecimentoTopico.Clear();
			txtConhecimentoTopico.ClearInvalid();
			txtTitulo.ClearInvalid();
			txtConhecimentoTopico.AllowBlank = false;
			txtConhecimentoTopico.Hidden = false;
            winTopico.Height = 270;
        }

		private void LimparCamposConhecimento()
        {
            cboTopico.Clear();
            txtConhecimento.Clear();
			cboTopico.ClearInvalid();
			txtConhecimento.ClearInvalid();
        }

        private void CarregarTopicos()
        {
            strTopicos.DataSource = new TopicoBaseConhecimentoBO().Select();
			strTopicos.DataBind();
        }

		#endregion
    }
}
