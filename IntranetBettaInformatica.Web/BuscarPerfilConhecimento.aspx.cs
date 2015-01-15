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
	public partial class BuscarPerfilConhecimento : BasePage
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

		protected void btnBuscar_Click(object sender, DirectEventArgs e)
		{
			List<Int32> lstIdsConhecimentos = JSON.Deserialize<List<Int32>>(e.ExtraParams["valores"]);
			strUsuarios.DataSource = new UsuarioBaseConhecimentoBO().BuscarPorConhecimentos(lstIdsConhecimentos).Select(x => x.Usuario);
			strUsuarios.DataBind();
		}

		protected void btnConhecimentos_Click(object sender, DirectEventArgs e)
		{
			CarregarConhecimentos(e);
			winConhecimentos.Show((Control)sender);
		}

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
			List<TopicoBaseConhecimentoVO> lstTopicosBaseConhecimento = new TopicoBaseConhecimentoBO().Select();

			Ext.Net.TreeNode root = new Ext.Net.TreeNode("Conhecimentos");
			root.AllowChildren = true;
			root.Expanded = true;

			treePerfilConhecimento.Root.Add(root);

			foreach (TopicoBaseConhecimentoVO topico in lstTopicosBaseConhecimento)
			{
				Ext.Net.TreeNode nodeE = new Ext.Net.TreeNode();
				nodeE.Icon = Icon.Folder;
				nodeE.Text = topico.Titulo;
				nodeE.Expanded = true;

				foreach (BaseConhecimentoVO conhecimento in topico.Conhecimentos)
				{
					Ext.Net.TreeNode node = new Ext.Net.TreeNode();
					node.NodeID = conhecimento.Id.ToString();
					node.Leaf = true;
					node.Checked = ThreeStateBool.False;
					node.Text = conhecimento.Titulo;
					nodeE.Nodes.Add(node);
				}

				root.Nodes.Add(nodeE);
			}
			Ext.Net.TreeNode nodeEAux = new Ext.Net.TreeNode();
			nodeEAux.Icon = Icon.Folder;
			nodeEAux.Text = "Nenhuma Seleção";
			nodeEAux.Expanded = true;
			nodeEAux.Leaf = true;
			nodeEAux.NodeID = "Nenhuma Seleção";
			nodeEAux.Checked = ThreeStateBool.True;
			nodeEAux.Hidden = true;
			root.Nodes.Add(nodeEAux);
		}

		private void CarregarConhecimentos(DirectEventArgs e)
		{
			strConhecimentos.DataSource = new UsuarioBaseConhecimentoBO().BuscarPorUsuario(e.ExtraParams["id"].ToInt32()).Select(x => new { Id = x.Id, NivelConhecimentoId = x.NivelConhecimento.ToInt32(), NivelConhecimentoDescricao = x.NivelConhecimento.ToText(), Comprovavel = x.Comprovavel, Conhecimento = x.Conhecimento });
			strConhecimentos.DataBind();
		}

        #endregion
    }
}
