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
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarReunioes : BasePage
    {

        #region propriedades

        private ReuniaoVO ReuniaoSelecionada
        {
            get
            {
                if (this.ViewState["ReuniaoSelecionada"] == null)
                    this.ViewState["ReuniaoSelecionada"] = new ReuniaoVO() { UID = Guid.NewGuid().ToString("B") };
                return (ReuniaoVO)this.ViewState["ReuniaoSelecionada"];
            }
            set { this.ViewState["ReuniaoSelecionada"] = value; }
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
            base.ResourceManager.RegisterIcon(Ext.Net.Icon.ControlPauseBlue);
            base.ResourceManager.RegisterIcon(Ext.Net.Icon.ControlStopBlue);
            base.ResourceManager.RegisterIcon(Ext.Net.Icon.ControlPlayBlue);
            base.ResourceManager.RegisterIcon(Ext.Net.Icon.ControlRecord);
            if (!IsPostBack)
            {
                LoadPagina();
                base.SetTituloIconePagina(frmTitulo);
            }
			btnNovo.Disabled = !hdfAdicionarReunioes.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverReuniao(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarReuniao(e);
        }

       protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winReuniao.Title = "Cadastrando Reunião";
            LimparCampos();
            winReuniao.Show((Control)sender);
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winReuniao.Title = "Alterando Reunião";
            winReuniao.Show((Control)sender);
            tabReuniao.SetActiveTab(0);
        }

        protected void btnCancelar_Click(object sender, DirectEventArgs e)
        {
            CancelarReuniao(e);
        }

        protected void btnAdicionarUsuarios_Click(object sender, DirectEventArgs e)
        {
            AdicionarUsuarios((Control)sender, e);
        }

        protected void VerificaSalasDisponiveis(object sender, DirectEventArgs e)
        {
            DateTime dataInicial = txtDataInicial.SelectedDate.Add(txtHoraInicial.SelectedTime);
            DateTime dataFinal = txtDataFinal.SelectedDate.Add(txtHoraFinal.SelectedTime);

            if (dataInicial > dataFinal)
            {
                base.MostrarMensagem("Erro", "Data e horário inicial não pode ser superior ao final.", String.Empty);
            }
            else
            {
                LoadSalas(dataInicial, dataFinal);
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strReunioes.DataSource = new ReuniaoBO().BuscarReunioesUsuario(UsuarioLogado, base.ContemPermissao(ETipoAcao.VisualizarTodasReunioes));
            strReunioes.DataBind();
        }

        private void LoadSalas(DateTime dataInicial, DateTime dataFinal)
        {
            List<SalaVO> lstSalas = new SalaBO().BuscarSalasDisponiveis(ReuniaoSelecionada, dataInicial, dataFinal);
            strSalas.DataSource = lstSalas;
            strSalas.DataBind();
            if (lstSalas.Count == 0)
            {
                cboSala.ClearValue();
                cboSala.ClearInvalid();
            }
            cboSala.Disabled = false;
        }

        private void RemoverReuniao(DirectEventArgs e)
        {
            try
            {
                ReuniaoVO reuniao = new ReuniaoBO().SelectById(e.ExtraParams["id"].ToInt32());
                new ReuniaoBO(reuniao).DeleteUpdate();

				EmailVO email = new EmailVO(base.Configuracao, true, reuniao);
				email.Usuarios = reuniao.Participantes.ToList();
                email.AddUsuariosAttachmentCollection();
                email.RemovidoReuniao();
                new EmailBO().EnviarEmailAssincrono(email);

                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
                btnCancelar.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover reunião.", "");
            }
        }

        private void SalvarReuniao(DirectEventArgs e)
        {
            try
            {
                List<UsuarioVO> usuariosRemovidos = new List<UsuarioVO>();
                List<UsuarioVO> usuariosAdicionados = new List<UsuarioVO>();
                List<UsuarioVO> usuariosReuniao = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuarios"]);
                if(base.AcaoTela == Common.AcaoTela.Edicao)
                {
                    usuariosRemovidos = ReuniaoSelecionada.Participantes.Where(x=> !usuariosReuniao.Any(x1=> x1.Id == x.Id)).ToList();
                    usuariosAdicionados = usuariosReuniao.Where(x => !ReuniaoSelecionada.Participantes.Any(x1 => x1.Id == x.Id)).ToList();
                }
                ReuniaoSelecionada.Participantes = usuariosReuniao;
                
                ReuniaoSelecionada.DataInicial = txtDataInicial.SelectedDate.Add(txtHoraInicial.SelectedTime);
                ReuniaoSelecionada.DataFinal = txtDataFinal.SelectedDate.Add(txtHoraFinal.SelectedTime);

                if (ReuniaoSelecionada.DataInicial > ReuniaoSelecionada.DataFinal)
                {
                    base.MostrarMensagem("Erro", "Data e horário inicial não pode ser superior ao final.", String.Empty);
                    return;
                }

                ReuniaoSelecionada.Titulo = txtTitulo.Text;
                ReuniaoSelecionada.Descricao = txtDescricao.Text;
                ReuniaoSelecionada.SalaReuniao = new SalaBO().SelectById(cboSala.Value.ToInt32());

                new ReuniaoBO(ReuniaoSelecionada).Salvar();

                EmailVO email = null;
                if (base.AcaoTela == Common.AcaoTela.Inclusao)
                {
                    email = new EmailVO(base.Configuracao, true, ReuniaoSelecionada);
                    email.Usuarios = ReuniaoSelecionada.Participantes.ToList();
                    email.AddUsuariosAttachmentCollection();
                    email.AddAttachment();
                }
                else
                {
                    email = new EmailVO(base.Configuracao, true, ReuniaoSelecionada);
                    email.Usuarios = usuariosRemovidos;
                    email.AddUsuariosAttachmentCollection();
                    email.RemoverUsuariosReuniao();

                    new EmailBO().EnviarEmailAssincrono(email);

                    email = new EmailVO(base.Configuracao, true, ReuniaoSelecionada);
                    email.Usuarios = usuariosAdicionados;
                    email.AddUsuariosAttachmentCollection();
                    email.AddAttachment();
                }
                new EmailBO().EnviarEmailAssincrono(email);
                base.MostrarMensagem("Sucesso", "Reunião gravada com sucesso.", String.Empty);
                
                LoadPagina();
                winReuniao.Hide();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar reunião.", String.Empty);
            }            
        }

        private void CancelarReuniao(DirectEventArgs e)
        {
            try
            {
                ReuniaoVO reuniao = new ReuniaoBO().SelectById(e.ExtraParams["id"].ToInt32());
                reuniao.ECancelada = true;
                new ReuniaoBO(reuniao).Salvar();
				try
				{
					EmailVO email = new EmailVO(base.Configuracao, true, reuniao);
					email.Usuarios = reuniao.Participantes.ToList();
					email.AddUsuariosAttachmentCollection();
					email.CancelamentoReuniao();
					new EmailBO().EnviarEmailAssincrono(email);
				}
				catch { }
				LoadPagina();
                base.MostrarMensagem("Sucesso", "Reunião cancelada com sucesso.", String.Empty);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao cancelar reunião.", String.Empty);
            }
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            ReuniaoSelecionada = new ReuniaoBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtTitulo.Text = ReuniaoSelecionada.Titulo;
            txtDescricao.Text = ReuniaoSelecionada.Descricao;
            txtDataInicial.Text = ReuniaoSelecionada.DataInicial.ToString("dd/MM/yyyy");
            txtHoraInicial.Text = ReuniaoSelecionada.DataInicial.ToString("HH:mm");
            txtDataFinal.Text = ReuniaoSelecionada.DataFinal.ToString("dd/MM/yyyy");
            txtHoraFinal.Text = ReuniaoSelecionada.DataFinal.ToString("HH:mm");
            LoadSalas(ReuniaoSelecionada.DataInicial, ReuniaoSelecionada.DataFinal);
            cboSala.SetValue(ReuniaoSelecionada.SalaReuniao.Id);
            btnVerificarSala.Disabled = false;
            strUsuariosReuniao.DataSource = ReuniaoSelecionada.Participantes;
            strUsuariosReuniao.DataBind();
        }

        private void LimparCampos()
        {
            txtTitulo.Clear();
            txtTitulo.ClearInvalid();
            txtDescricao.Clear();
            txtDescricao.ClearInvalid();
            txtDataInicial.Clear();
            txtDataInicial.ClearInvalid();
            txtDataFinal.Clear();
            txtDataFinal.ClearInvalid();
            txtHoraInicial.Clear();
            txtHoraInicial.ClearInvalid();
            txtHoraFinal.Clear();
            txtHoraFinal.ClearInvalid();
            cboSala.ClearValue();
            cboSala.ClearInvalid();
            strUsuariosReuniao.DataSource = new List<UsuarioVO>();
            strUsuariosReuniao.DataBind();
            btnVerificarSala.Disabled = true;
            
            tabReuniao.SetActiveTab(0);

            ReuniaoSelecionada = null;
        }

        private void LimparCamposPesquisaUsuarios()
        {
            btnRemoverUsuarios.Disabled = true;
        }

        private void AdicionarUsuarios(Control sender, DirectEventArgs e)
        {
            LimparCamposPesquisaUsuarios();
            List<UsuarioVO> usuarios = new UsuarioBO().BuscarUsuariosSistema(false, null);
            List<UsuarioVO> usuariosAdicionados = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuariosAdicionados"]);

            usuarios = usuarios = usuarios.Where(x => !usuariosAdicionados.Any(x1 => x1.Id == x.Id)).ToList();
            if (base.AcaoTela == Common.AcaoTela.Edicao)
            {
                usuarios = usuarios.Where(x => !ReuniaoSelecionada.Participantes.Any(x1 => x1.Id == x.Id)).ToList();
            }
            strUsuarios.DataSource = usuarios;
            strUsuarios.DataBind();
            (grdUsuariosReuniao.SelectionModel.Primary as CheckboxSelectionModel).ClearSelections();
            winAdicionarUsuarios.Show(sender);
        }

        #endregion
    }
}
