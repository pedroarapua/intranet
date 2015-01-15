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
    public partial class Colaboradores : BasePage
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

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
			List<UsuarioVO> lst = new UsuarioBO().BuscarUsuariosSistema(false, true);
			lst = lst.FindAll(x => x.Id != UsuarioLogado.Id);
			strColaboradores.DataSource = lst.Select(x => new { Id = x.Id, Nome = x.Nome, CaminhoImagemThumbs = x.CaminhoImagemThumbs, TruncateNome = x.Nome.Truncate(30) });
			strColaboradores.DataBind();
		}

        #endregion
    }
}
