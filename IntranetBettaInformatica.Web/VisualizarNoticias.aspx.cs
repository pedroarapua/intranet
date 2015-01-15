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
    public partial class VisualizarNoticias : BasePage
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
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
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
            if (!txtDataInicialBusca.Text.IsNullOrEmpty() && txtDataInicialBusca.Text != txtDataInicialBusca.EmptyValue.ToString())
            {
                dataIni = Convert.ToDateTime(txtDataInicialBusca.Text);
            }
            if (!txtDataFinalBusca.Text.IsNullOrEmpty() && txtDataFinalBusca.Text != txtDataFinalBusca.EmptyValue.ToString())
            {
                dataFim = Convert.ToDateTime(txtDataFinalBusca.Text);
            }
            //rowExpander.CollapseAll();
			strNoticias.DataSource = new NoticiaBO().Buscar(UsuarioLogado, false, chkIniciada.Checked, chkFinalizada.Checked, dataIni, dataFim).Select(x => new { HTML = x.HTML, Id = x.Id, Titulo = x.Titulo, Status = x.Status, DataInicial = x.DataInicial, DataFinal = x.DataFinal, DataPeriodo = x.DataInicial.ToString("dd/MM/yyyy") + " até " + x.DataFinal.ToString("dd/MM/yyyy") }).OrderByDescending(x=> x.DataInicial).ToList();
            strNoticias.DataBind();
            //rowExpander.ExpandAll();
        }

        #endregion
    }
}
