using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using IntranetBettaInformatica.Web.Common;

namespace IntranetBettaInformatica.Web
{
    public partial class Redirecionar : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Ext.Net.Panel pnlPerfilCollapse = this.Master.FindControl("pnlPerfilCollapse") as Ext.Net.Panel;
                Ext.Net.Panel pnlNorth = this.Master.FindControl("pnlNorth") as Ext.Net.Panel;
                pnlPerfilCollapse.Collapse();
                pnlNorth.Collapse();
                pnlIFrame.Title = Request.QueryString["url"];
                pnlIFrame.AutoLoad.Url = Request.QueryString["url"];
                pnlIFrame.AutoLoad.Mode = LoadMode.IFrame;
                pnlIFrame.AutoLoad.ShowMask = true;
                pnlIFrame.LoadMask.ShowMask = true;
                pnlIFrame.LoadMask.Msg = String.Format("Carregando {0} ...", pnlIFrame.AutoLoad.Url);
                pnlIFrame.AutoLoad.MaskMsg = String.Format("Carregando {0} ...", pnlIFrame.AutoLoad.Url);
            }
                
        }
    }
}