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
    public partial class ResponderPesquisasOpiniao : BasePage
    {

        #region propriedades

        private List<PesquisaOpiniaoVO> Pesquisas
        {
            get
            {
                if (this.ViewState["Pesquisas"] == null)
                    return null;
                return (List<PesquisaOpiniaoVO>)this.ViewState["Pesquisas"];
            }
            set { this.ViewState["Pesquisas"] = value; }
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
			btnSalvar.Disabled = !hdfSalvarResponderPesquisas.Value.ToInt32().ToBoolean();
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarPesquisa(e);
        }

        protected void btnBuscar_Click(object sender, DirectEventArgs e)
        {
            LoadPagina();
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            if (!txtDataInicial.Text.IsNullOrEmpty() && txtDataInicial.Text != txtDataInicial.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicial.Text);
            }
            if (!txtDataFinal.Text.IsNullOrEmpty() && txtDataFinal.Text != txtDataFinal.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinal.Text);
            }

            Pesquisas = new PesquisaOpiniaoBO().BuscarPorUsuario(UsuarioLogado, chkAtivo.Checked, chkIniciada.Checked, chkFinalizada.Checked, dataIni, dataFim);
            lblNenhumaPesquisa.Hidden = Pesquisas.Count > 0;
            CarregarPesquisas();
            frmPesquisas.Hidden = Pesquisas.Count == 0;
        }

        /// <summary>
        /// metodo que carrega as pesquisas existentes
        /// </summary>
        private void CarregarPesquisas()
        {
            foreach (PesquisaOpiniaoVO p in Pesquisas)
            {
                FieldSet fds = new FieldSet() { AutoWidth = true, AutoHeight = true, Title = p.Pergunta, TitleCollapse = true, Collapsible = true, Collapsed = false, AnimCollapse = true };
                Hidden hdf = new Hidden() { ID = "hdf_" + p.Id, Value = p.Id.ToString() };
                Ext.Net.RadioGroup group = new RadioGroup() { AutoWidth=true, ColumnsNumber = 1, GroupName = "group_" + p.Id, ID="group_"+p.Id, InvalidText="Selecione uma resposta.", AllowBlank = !(p.Status == StatusPesquisa.Iniciada), MsgTarget = MessageTarget.Side };
                foreach (RespostaVO r in p.Respostas)
                {
                    Radio radio = new Radio() { BoxLabel = r.Descricao, HideLabel = true, AutoWidth = true, MinWidth = 150, ID = "radio_"+r.Id, Checked = r.Usuarios.Any(x=> x.Id == UsuarioLogado.Id) };
                    group.Items.Add(radio);
                }
                fds.Items.Add(hdf);
                fds.Items.Add(group);
                if (p.Status == StatusPesquisa.Finalizada && p.MostrarResultado)
                {
                    Ext.Net.Button btnGrafico = new Ext.Net.Button("Resultado");
                    //btnGrafico.DirectEvents.Click.EventMask = new EventMask() { Msg = "Abrindo gráfico...", ShowMask = true, Target = MaskTarget.Page };
                    btnGrafico.ID = "btnGrafico" + p.Id;
                    btnGrafico.Listeners.Click.Handler = "Ext.net.DirectMethods.VisualizarGrafico('" + p.Id + "');";
                    btnGrafico.Icon = Ext.Net.Icon.ChartBar;
					btnGrafico.Disabled = !hdfVisualizarGraficoResponderPesquisas.Value.ToInt32().ToBoolean();
                    Toolbar toolbar = new Toolbar();
                    toolbar.Add(btnGrafico);
                    fds.TopBar.Add(toolbar);
                }
                group.Disabled = !(p.Status == StatusPesquisa.Iniciada);
                fds.AddTo(frmPesquisas);
            }
        }

        /// <summary>
        /// metodo que salva as respostas das pesquisas
        /// </summary>
        /// <param name="e"></param>
        private void SalvarPesquisa(DirectEventArgs e)
        {
            try
            {
                List<PesquisaOpiniaoVO> pesquisas = JSON.Deserialize<List<PesquisaOpiniaoVO>>(e.ExtraParams["questoes"]);
                List<RespostaVO> respostas = new List<RespostaVO>();
                foreach (PesquisaOpiniaoVO p in pesquisas)
                {
                    if (p.Respostas.Any(x => x.Usuarios.Count > 0))
                    {
                        foreach (RespostaVO r in p.Respostas)
                        {
                            RespostaVO rAux = new RespostaBO().SelectById(r.Id);
                            if (r.Usuarios.Count > 0 && !rAux.Usuarios.Any(x => x.Id == UsuarioLogado.Id))
                            {
                                rAux.Usuarios.Add(UsuarioLogado);
                                respostas.Add(rAux);
                            }
                            else if (rAux.Usuarios.Any(x => x.Id == UsuarioLogado.Id))
                            {
                                rAux.Usuarios.Remove(UsuarioLogado);
                                respostas.Add(rAux);
                            }
                        }
                    }
                }
                new RespostaBO().SalvarRespostas(respostas);
                base.MostrarMensagem("Sucesso", "Pesquisas respondidas com sucesso.", String.Empty);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao salvar pesquisa(s) de opinião.", String.Empty);
            }            
        }

        /// <summary>
        /// metodo que abri o gráfico do resultado da pesquisa
        /// </summary>
        /// <param name="button"></param>
        [DirectMethod]
        public void VisualizarGrafico(String id)
        {
            PesquisaOpiniaoVO p = Pesquisas.Find(x=> x.Id == id.ToInt32());
            setChart(p.Respostas.ToList());
            winGrafico.Render();
            winGrafico.Show();
        }

        void setChart(List<RespostaVO> respostas)
        {
            var lstSeries = new List<Series>();
            foreach (RespostaVO r in respostas)
            {
                //var s = getGanttSeries(r.Descricao);
                //s.LegendText = r.Descricao;
                double[] valor = new double[1];
                valor[0] = (double)r.Usuarios.Count;
                Chart1.Series["Series1"].Points.Add(new DataPoint() { YValues = valor, LegendText=r.Descricao });
            }
            Chart1.Series["Series1"].Font = new Font("Trebuchet MS", 8, FontStyle.Bold);
            Chart1.Series["Series1"]["CollectedToolTip"] = "Other";
        }
        #endregion

    }
}
