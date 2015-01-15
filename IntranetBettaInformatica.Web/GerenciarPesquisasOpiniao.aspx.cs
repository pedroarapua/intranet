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

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarPesquisasOpiniao : BasePage
    {

        #region propriedades

        private PesquisaOpiniaoVO PesquisaSelecionada
        {
            get
            {
                if (this.ViewState["PesquisaSelecionada"] == null)
                    return null;
                return (PesquisaOpiniaoVO)this.ViewState["PesquisaSelecionada"];
            }
            set { this.ViewState["PesquisaSelecionada"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarPesquisas.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverPesquisa(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarPesquisa(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winPesquisa.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winPesquisa.Title = "Cadastrando Pesquisa de Opinião";
            LimparCampos();
            winPesquisa.Show((Control)sender);
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            PreencherCampos(e);
            winPesquisa.Title = "Alterando Pesquisa de Opinião";
            winPesquisa.Show((Control)sender);
            tabPesquisa.SetActiveTab(0);
        }

        protected void btnAdicionarUsuarios_Click(object sender, DirectEventArgs e)
        {
            AdicionarUsuarios((Control)sender);
        }

        /// <summary>
        /// evento para abrir gráfico de pesquisas
        /// </summary>
        /// <param name="button"></param>
        protected void btnVisualizarGrafico_Click(object sender, DirectEventArgs e)
        {
            PesquisaOpiniaoVO p = new PesquisaOpiniaoBO().SelectById(e.ExtraParams["id"].ToInt32());
            setChart(p.Respostas.ToList());
            winGrafico.Render();
            winGrafico.Show();
        }

        /// <summary>
        /// metodo que carrega as informações no gráfico
        /// </summary>
        /// <param name="respostas"></param>
        private void setChart(List<RespostaVO> respostas)
        {
            var lstSeries = new List<Series>();
            foreach (RespostaVO r in respostas)
            {
                double[] valor = new double[1];
                valor[0] = (double)r.Usuarios.Count;
                Chart1.Series["Series1"].Points.Add(new DataPoint() { YValues = valor, LegendText = r.Descricao });
            }
            Chart1.Series["Series1"].Font = new Font("Trebuchet MS", 8, FontStyle.Bold);
            Chart1.Series["Series1"]["CollectedToolTip"] = "Other";
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            strPesquisas.DataSource = new PesquisaOpiniaoBO().Select().Where(x=> x.Removido == false).ToList();
            strPesquisas.DataBind();
        }

        private void RemoverPesquisa(DirectEventArgs e)
        {
            try
            {
                PesquisaOpiniaoVO pesquisa = JSON.Deserialize<List<PesquisaOpiniaoVO>>(e.ExtraParams["valores"])[0];
                pesquisa = new PesquisaOpiniaoBO().SelectById(pesquisa.Id);
                new PesquisaOpiniaoBO(pesquisa).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover pesquisa de opinião.", "");
            }
        }

        private void SalvarPesquisa(DirectEventArgs e)
        {
            try
            {
                PesquisaOpiniaoVO pesquisa = new PesquisaOpiniaoVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                {
                    pesquisa = PesquisaSelecionada;
                    List<RespostaVO> respostas = JSON.Deserialize<List<RespostaVO>>(e.ExtraParams["respostas"]);
                    List<UsuarioVO> usuarios = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuarios"]);
                    foreach (RespostaVO r in respostas)
                    {
                        if (!pesquisa.Respostas.Any(x => x.Id == r.Id))
                        {
                            r.Id = 0;
                            pesquisa.Respostas.Add(r);
                        }
                        else
                        {
                            r.Usuarios = r.Usuarios.Where(x => usuarios.Any(x1 => x1.Id == x.Id)).ToList();
                        }
                    }
                    pesquisa.Respostas = pesquisa.Respostas.Where(x => respostas.Any(x1 => x1.Id == x.Id)).ToList();
                }
                else
                    pesquisa.Respostas = JSON.Deserialize<List<RespostaVO>>(e.ExtraParams["respostas"]);
                
                pesquisa.Usuarios = JSON.Deserialize<List<UsuarioVO>>(e.ExtraParams["usuarios"]);
                pesquisa.DataInicial = txtDataInicial.SelectedDate.Add(txtHoraInicial.SelectedTime);
                pesquisa.DataFinal = txtDataFinal.SelectedDate.Add(txtHoraFinal.SelectedTime);
                pesquisa.Respostas.ToList().ForEach(x => x.Pesquisa = pesquisa);

                if (pesquisa.DataInicial > pesquisa.DataFinal)
                {
                    base.MostrarMensagem("Erro", "Data e horário inicial não pode ser superior ao final.", String.Empty);
                    return;
                }

                pesquisa.MostrarResultado = chkMostrarResultado.Checked;
                pesquisa.Pergunta = txtPergunta.Text;
                pesquisa.Removido = false;

                new PesquisaOpiniaoBO(pesquisa).Salvar();

                base.MostrarMensagem("Sucesso", "Pesquisa de opinião gravada com sucesso.", String.Empty);
                
                LoadPagina();
                winPesquisa.Hide();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar pesquisa de opinião.", String.Empty);
            }            
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            PesquisaSelecionada = new PesquisaOpiniaoBO().SelectById(JSON.Deserialize<List<PesquisaOpiniaoVO>>(e.ExtraParams["valores"])[0].Id);
            txtPergunta.Text = PesquisaSelecionada.Pergunta;
            txtDataInicial.Text = PesquisaSelecionada.DataInicial.ToString("dd/MM/yyyy");
            txtHoraInicial.Text = PesquisaSelecionada.DataInicial.ToString("HH:mm");
            txtDataFinal.Text = PesquisaSelecionada.DataFinal.ToString("dd/MM/yyyy");
            txtHoraFinal.Text = PesquisaSelecionada.DataFinal.ToString("HH:mm");
            chkMostrarResultado.Checked = PesquisaSelecionada.MostrarResultado;
            strRespostas.DataSource = PesquisaSelecionada.Respostas;
            strRespostas.DataBind();
            strUsuarios.DataSource = PesquisaSelecionada.Usuarios;
            strUsuarios.DataBind();
        }

        private void LimparCampos()
        {
            txtPergunta.Clear();
            txtDataInicial.Clear();
            txtDataFinal.Clear();
            txtHoraInicial.Clear();
            txtHoraFinal.Clear();
            chkMostrarResultado.Checked = false;
            strRespostas.DataSource = new List<RespostaVO>();
            strRespostas.DataBind();
            strUsuarios.DataSource = new List<UsuarioVO>();
            strUsuarios.DataBind();
            tabPesquisa.SetActiveTab(0);
        }

        private void LimparCamposPesquisaUsuarios()
        {
            btnRemoverUsuarios.Disabled = true;
        }

        private void AdicionarUsuarios(Control sender)
        {
            LimparCamposPesquisaUsuarios();
            strUsuariosPesquisa.DataSource = new UsuarioBO().BuscarUsuariosSistema(false,true);
            strUsuariosPesquisa.DataBind();
            (grdUsuariosPesquisa.SelectionModel.Primary as CheckboxSelectionModel).ClearSelections();
            winAdicionarUsuarios.Show(sender);
        }

        #endregion
    }
}
