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
    public partial class GerenciarEmpresas : BasePage
    {

        #region propriedades

        private EmpresaVO EmpresaSelecionada
        {
            get
            {
                if (this.ViewState["EmpresaSelecionada"] == null)
                    return null;
                return (EmpresaVO)this.ViewState["EmpresaSelecionada"];
            }
            set { this.ViewState["EmpresaSelecionada"] = value; }
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
			btnNovo.Disabled = hdfAdicionarEmpresas.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverEmpresa(e);
        }

        protected void grdEmpresas_SelectedRow(object sender, DirectEventArgs e)
        {
            HabilitaBotoes();
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarEmpresa(e);
        }

        protected void SalvarTelefones_Click(object sender, DirectEventArgs e)
        {
            SalvarTelefones(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winEmpresa.Hide();
        }

        protected void CancelarTelefones_Click(object sender, DirectEventArgs e)
        {
            winTelefones.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winEmpresa.Title = "Cadastrando Empresa";
            CarregarTiposEmpresa(null);
            LimparCampos();
            winEmpresa.Show((Control)sender);
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winEmpresa.Title = "Alterando Empresa";
            winEmpresa.Show((Control)sender);
        }

        protected void btnTelefones_Click(object sender, DirectEventArgs e)
        {
            CarregarTelefones(e);
            btnRemoveTelefone.Disabled = true;
            btnSalvarTelefones.Disabled = false;
            btnAddTelefone.Disabled = false;
            winTelefones.Show();
        }
        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strEmpresas.DataSource = new EmpresaBO().Select().Where(x => x.Removido == false).ToList();
            strEmpresas.DataBind();
			CarregarEstados();
            HabilitaBotoes();
        }

        private void CarregarTiposEmpresa(TipoEmpresaVO tipoEmpresda)
        {
            strTiposEmpresa.DataSource = new TipoEmpresaBO().Select().Where(x => x.Removido == false || (tipoEmpresda != null && x.Id == tipoEmpresda.Id)).ToList();
            strTiposEmpresa.DataBind();
        }

        private void HabilitaBotoes()
        {
            RowSelectionModel sm = this.grdEmpresas.SelectionModel.Primary as RowSelectionModel;
            Boolean habilitar = sm.SelectedRows.Count > 0;
            btnEditar.Disabled = !habilitar || hdfEditarEmpresas.Value.ToInt32() == 0;
            btnTelefones.Disabled = !habilitar || hdfAdicionarTelefonesEmpresas.Value.ToInt32() == 0;
			btnChartOrganization.Disabled = !habilitar || !hdfVisualizarChartEmpresas.Value.ToInt32().ToBoolean();
            btnRemover.Disabled = !(habilitar && sm.SelectedRows[0].RecordID != "1") || hdfRemoverEmpresas.Value.ToInt32() == 0;
        }

        private void RemoverEmpresa(DirectEventArgs e)
        {
            try
            {
                EmpresaVO empresa = new EmpresaBO().SelectById(e.ExtraParams["id"].ToInt32());
                new EmpresaBO(empresa).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover empresa.", "");
            }
        }

        private void SalvarEmpresa(DirectEventArgs e)
        {
            try
            {
                EmpresaVO empresa = new EmpresaVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    empresa = EmpresaSelecionada;
                
                empresa.Email = txtEmail.Text;
                empresa.Endereco = txtEndereco.Text;
                empresa.Nome = txtNome.Text;
                empresa.Site = txtSite.Text;
                empresa.Cidade = txtCidade.Text;
                empresa.TipoEmpresa = new TipoEmpresaVO(){ Id = cboTipoEmpresa.Value.ToInt32()};
                if (cboEstado.Value != null && !cboEstado.Value.ToString().IsNullOrEmpty())
                    empresa.Estado = new EstadoVO() { Id = cboEstado.Value.ToInt32() };
                else
                    empresa.Estado = null;

                empresa.Removido = false;
                
                new EmpresaBO(empresa).Salvar();

                base.MostrarMensagem("Empresa", "Empresa gravada com sucesso.", "");
                
                LoadPagina();
                winEmpresa.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar empresa.";
                e.Success = false;
            }            
        }

        private void SalvarTelefones(DirectEventArgs e)
        {
            try
            {
                EmpresaSelecionada.Telefones = JSON.Deserialize<List<EmpresaTelefoneVO>>(e.ExtraParams["telefones"]);
                EmpresaSelecionada.Telefones.ToList().ForEach(x => x.Empresa = EmpresaSelecionada);
                new EmpresaBO(EmpresaSelecionada).Salvar();
				base.MostrarMensagem("Telefones","Telefones gravados com sucesso", String.Empty);
                winTelefones.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar empresa.";
                e.Success = false;
            }
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            EmpresaSelecionada = new EmpresaBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtNome.Text = EmpresaSelecionada.Nome;
            txtEmail.Text = EmpresaSelecionada.Email;
            txtEndereco.Text = EmpresaSelecionada.Endereco;
            txtCidade.Text = EmpresaSelecionada.Cidade;
            txtSite.Text = EmpresaSelecionada.Site;
            CarregarTiposEmpresa(EmpresaSelecionada.TipoEmpresa);
            cboTipoEmpresa.SetValue(EmpresaSelecionada.TipoEmpresa.Id.ToString());
            cboTipoEmpresa.Disabled = EmpresaSelecionada.Id == 1;
            if (EmpresaSelecionada.Estado != null)
                cboEstado.SetValue(EmpresaSelecionada.Estado.Id);
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtSite.Clear();
            txtCidade.Clear();
            cboTipoEmpresa.Clear();
            cboEstado.Clear();

            txtCidade.ClearInvalid();
            cboTipoEmpresa.Disabled = false;
        }

        private void CarregarTelefones(DirectEventArgs e)
        {
            EmpresaSelecionada = new EmpresaBO().SelectById(e.ExtraParams["id"].ToInt32());
            strTelefones.DataSource = EmpresaSelecionada.Telefones;
            strTelefones.DataBind();
        }

		private void CarregarEstados()
		{
			strEstados.DataSource = new EstadoBO().Select();
			strEstados.DataBind();
		}

        #endregion
    }
}
