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
    public partial class GerenciarManualColaborador : BasePage
    {

        #region propriedades

		public AcaoTela AcaoTelaItemManual
        {
            get
            {
                if (this.ViewState["AcaoTelaItemManual"] == null)
                    return AcaoTela.Inclusao;
				return (AcaoTela)this.ViewState["AcaoTelaItemManual"];
            }
			set { this.ViewState["AcaoTelaItemManual"] = value; }
        }

        private TopicoManualColaboradorVO TopicoSelecionado
        {
            get
            {
				if (this.ViewState["TopicoSelecionado"] == null)
                    return null;
				return (TopicoManualColaboradorVO)this.ViewState["TopicoSelecionado"];
            }
			set { this.ViewState["TopicoSelecionado"] = value; }
        }

        private ItemManualColaboradorVO ItemManualSelecionado
        {
            get
            {
				if (this.ViewState["ItemManualSelecionado"] == null)
                    return null;
				return (ItemManualColaboradorVO)this.ViewState["ItemManualSelecionado"];
            }
			set { this.ViewState["ItemManualSelecionado"] = value; }
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
			btnNovo.Disabled = hdfAdicionarTopicoManualColaborador.Value.ToInt32() == 0;
			btnNovoItemManual.Disabled = hdfAdicionarItemManualColaborador.Value.ToInt32() == 0;

			ImageCommandColumn columnCommand = (grdItensManual.ColumnModel.Columns[grdItensManual.ColumnModel.Columns.Count - 1] as ImageCommandColumn);

			columnCommand.GroupCommands[0].Hidden = !hdfAdicionarItemManualColaborador.Value.ToInt32().ToBoolean();
			columnCommand.GroupCommands[1].Hidden = !hdfRemoverTopicoManualColaborador.Value.ToInt32().ToBoolean();
			columnCommand.GroupCommands[2].Hidden = !hdfEditarTopicoManualColaborador.Value.ToInt32().ToBoolean();
			grdItensManual.TopBar.Toolbar.Hidden = !this.ContemAlgumaPermissao();

	    }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemoverItemManual_Click(object sender, DirectEventArgs e)
        {
			RemoverItemManual(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarTopico(e);
        }

		protected void SalvarItemManual_Click(object sender, DirectEventArgs e)
        {
			SalvarItemManual(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winTopico.Title = "Cadastrando Tópico";
            LimparCampos();
			winTopico.Show((Control)sender);
        }

        protected void btnNovoItemManual_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaItemManual = Common.AcaoTela.Inclusao;
            winItemManual.Title = "Cadastrando Item Manual";
            CarregarTopicos();
            LimparCamposItemManual();
            winItemManual.Show((Control)sender);
        }

        protected void btnEditarItemManual_Click(object sender, DirectEventArgs e)
        {
            this.AcaoTelaItemManual = Common.AcaoTela.Edicao;
            PreencherCamposItemManual(e);
            winItemManual.Title = "Alterando Item Manual";
            winItemManual.Show((Control)sender);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strItensManual.DataSource = new ItemManualColaboradorBO().Select();
            strItensManual.DataBind();
        }

        [DirectMethod]
        public void RemoverTopico(String id)
        {
            try
            {
                TopicoManualColaboradorVO topico = new TopicoManualColaboradorBO().SelectById(id.ToInt32());
				new TopicoManualColaboradorBO(topico).Delete();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover tópico.", "");
            }
        }

        private void RemoverItemManual(DirectEventArgs e)
        {
            try
            {
                ItemManualColaboradorVO item = new ItemManualColaboradorBO().SelectById(e.ExtraParams["id"].ToInt32());
				new ItemManualColaboradorBO(item).Delete();
                LoadPagina();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover item do manual.", "");
            }
        }

        private void SalvarTopico(DirectEventArgs e)
        {
            try
            {
				TopicoManualColaboradorVO topico = new TopicoManualColaboradorVO();
                ItemManualColaboradorVO item = null;
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    topico = TopicoSelecionado;
                else
                {
                    item = new ItemManualColaboradorVO();
					item.Descricao = txtItemManual.Text;
                }

                topico.Titulo = txtTitulo.Text;

				topico = (TopicoManualColaboradorVO)new TopicoManualColaboradorBO(topico).Salvar();
                if (item != null)
                {
                    item.Topico= topico;
                    new ItemManualColaboradorBO(item).Salvar();
                }
                base.MostrarMensagem("Tópico de Manual","Tópico e item gravados com sucesso", String.Empty);
                LoadPagina();
                winTopico.Hide();
            }
            catch (Exception ex)
            {
				base.MostrarMensagem("Erro", "Erro ao salvar tópico e item.", String.Empty);
            }            
        }

        private void SalvarItemManual(DirectEventArgs e)
        {
            try
            {
                ItemManualColaboradorVO item = new ItemManualColaboradorVO();
                if (this.AcaoTelaItemManual == Common.AcaoTela.Edicao)
                    item = ItemManualSelecionado;

				item.Descricao = txtItemManualItem.Text;
				item.Topico = new TopicoManualColaboradorVO() { Id = cboTopico.Value.ToInt32() };
                
                new ItemManualColaboradorBO(item).Salvar();

                base.MostrarMensagem("Item","Item gravado com sucesso", String.Empty);

                LoadPagina();
                winItemManual.Hide();
            }
            catch (Exception ex)
            {
				base.MostrarMensagem("Erro", "Erro ao salvar item.", String.Empty);
            }
        }

        [DirectMethod]
        public void AdicionarItemManual(String id)
        {
            LimparCamposItemManual();
            TopicoSelecionado = new TopicoManualColaboradorBO().SelectById(id.ToInt32());
            this.AcaoTelaItemManual = Common.AcaoTela.Inclusao;
            winItemManual.Title = "Cadastrando Item";
            CarregarTopicos();
            cboTopico.SetValue(TopicoSelecionado.Id);
            winItemManual.Show();
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
            TopicoSelecionado = new TopicoManualColaboradorBO().SelectById(id.ToInt32());
            txtTitulo.Text = TopicoSelecionado.Titulo;
			txtItemManual.AllowBlank = true;
            txtItemManual.Hidden = true;
            winTopico.Height = 120;
        }

        private void PreencherCamposItemManual(DirectEventArgs e)
        {
            ItemManualSelecionado = new ItemManualColaboradorBO().SelectById(e.ExtraParams["id"].ToInt32());
            TopicoSelecionado = ItemManualSelecionado.Topico;
            CarregarTopicos();
            
            txtItemManualItem.Text = ItemManualSelecionado.Descricao;
            cboTopico.SetValue(TopicoSelecionado.Id);
        }

        private void LimparCampos()
        {
            txtTitulo.Clear();
			txtItemManual.Clear();
			txtItemManual.ClearInvalid();
			txtTitulo.ClearInvalid();
            txtItemManual.AllowBlank = false;
			txtItemManual.Hidden = false;
            winTopico.Height = 270;
        }

        private void LimparCamposItemManual()
        {
            cboTopico.Clear();
            txtItemManualItem.Clear();
			cboTopico.ClearInvalid();
			txtItemManualItem.ClearInvalid();
        }

        private void CarregarTopicos()
        {
            strTopicos.DataSource = new TopicoManualColaboradorBO().Select();
			strTopicos.DataBind();
        }

		private Boolean ContemAlgumaPermissao()
		{
			if (base.ContemPermissao(ETipoAcao.AdicionarTopicoManualColaborador))
				return true;
			if (base.ContemPermissao(ETipoAcao.EditarTopicoManualColaborador))
				return true;
			if (base.ContemPermissao(ETipoAcao.RemoverTopicoManualColaborador))
				return true;
			if (base.ContemPermissao(ETipoAcao.AdicionarItemManualColaborador))
				return true;
			if (base.ContemPermissao(ETipoAcao.EditarItemManualColaborador))
				return true;
			if (base.ContemPermissao(ETipoAcao.RemoverItemManualColaborador))
				return true;
			return false;
		}

        #endregion
    }
}
