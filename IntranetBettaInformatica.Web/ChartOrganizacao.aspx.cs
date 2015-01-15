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
	public partial class ChartOrganizacao : System.Web.UI.Page
	{
		#region propriedades

		public UsuarioVO UsuarioLogado
		{
			get
			{
				if (this.Session["UsuarioLogado"] == null)
					return null;
				return (UsuarioVO)this.Session["UsuarioLogado"];
			}
			set
			{
				this.Session["UsuarioLogado"] = value;
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
			//Faz com que a session expire
			Response.Cache.SetExpires(DateTime.Now.Subtract(new TimeSpan(24, 0, 0)));
			//Desabilita a cache para a página
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			if (!IsPostBack)
			{
				if(!Request.QueryString["empresa"].IsNullOrEmpty())
				{
					EmpresaVO empresa = new EmpresaBO().SelectById(Request.QueryString["empresa"].ToInt32());
					List<EmpresaSetorVO> setoresRetorno = new List<EmpresaSetorVO>();

					setoresRetorno.Add(new EmpresaSetorVO() { Id = 0, Nome = empresa.Nome });
					empresa.Setores.Where(x => x.SetorPai == null).ToList().ForEach(x => setoresRetorno.Add(new EmpresaSetorVO() { Id = x.Id, Nome = x.Nome, SetorPai = new EmpresaSetorVO() { Nome = empresa.Nome, Id = 0 } }));

					foreach (EmpresaSetorVO setor in empresa.Setores)
					{
						if (setor.SetoresFilhos.Count > 0)
							SetSetoresFilhos(setoresRetorno, setor.SetoresFilhos.ToList());
					}
					hdfValores.Value = JSON.Serialize(setoresRetorno);
				}
			}
        }

        #endregion

		#region metodos

		private void SetSetoresFilhos(List<EmpresaSetorVO> setoresRetorno, List<EmpresaSetorVO> setores)
		{
			foreach(EmpresaSetorVO setor in setores)
			{
				setoresRetorno.Add(new EmpresaSetorVO() { Id = setor.Id, Nome = setor.Nome, SetorPai = new EmpresaSetorVO() { Id = setor.SetorPai.Id, Nome = setor.SetorPai.Nome } });
				if (setor.SetoresFilhos.Count > 0)
					SetSetoresFilhos(setoresRetorno, setor.SetoresFilhos.ToList());
			}
		}

		#endregion

	}
}
