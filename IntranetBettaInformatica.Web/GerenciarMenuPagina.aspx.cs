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
    public partial class GerenciarMenuPagina : BasePage
    {

        #region propriedades

        private MenuPaginaVO MenuPaginaSelecionado
        {
            get
            {
                if (this.ViewState["MenuPaginaSelecionado"] == null)
                    this.ViewState["MenuPaginaSelecionado"] = new MenuPaginaVO();
                return (MenuPaginaVO)this.ViewState["MenuPaginaSelecionado"];
            }
            set { this.ViewState["MenuPaginaSelecionado"] = value; }
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
		}

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverMenuPagina(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarMenuPagina(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winPagina.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winPagina.Title = "Cadastrando Página";
            LimparCampos();
            CarregarPaginas(treePaginasWin.Root, true, null);
            winPagina.Show((Control)sender);
            pnlDropField.Render();
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winPagina.Title = "Alterando Página";
            CarregarPaginas(treePaginasWin.Root, true, MenuPaginaSelecionado);
            winPagina.Show((Control)sender);
            pnlDropField.Render();
        }

        protected void treePaginas_SelectedRow(object sender, DirectEventArgs e)
        {
            HabilitaBotoes(true);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            CarregarPaginas(treePaginas.Root, false, null);
            CarregarIcons();
            HabilitaBotoes(false);
        }

        /// <summary>
        /// metodo que habilita/desabilita os botões da gerencia
        /// </summary>
        /// <param name="habilitar"></param>
        private void HabilitaBotoes(Boolean habilitar)
        {
			btnEditar.Disabled = !habilitar || hdfEditarPaginas.Value.ToInt32() == 0;
            btnRemover.Disabled = !habilitar ||  hdfRemoverPaginas.Value.ToInt32() == 0;
        }

        /// <summary>
        /// metodo que remove um menupagina
        /// </summary>
        /// <param name="e"></param>
        private void RemoverMenuPagina(DirectEventArgs e)
        {
            try
            {
                MenuPaginaVO menuPagina = new MenuPaginaBO().SelectById(e.ExtraParams["id"].ToInt32());
                SetRemovidoPaginasFilhas(menuPagina.MenuPaginas.ToList());
                new MenuPaginaBO(menuPagina).DeleteUpdate();
                HabilitaBotoes(false);
                ReloadNodes(e);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover pagina.", "");
            }
        }

        /// <summary>
        /// metodo chamado para inserir ou atualizar um menu pagina
        /// </summary>
        /// <param name="e"></param>
        private void SalvarMenuPagina(DirectEventArgs e)
        {
            try
            {
                MenuPaginaSelecionado.Descricao = txtDescricao.Text;
                MenuPaginaSelecionado.EmMenu = ckbEmMenu.Checked;
                MenuPaginaSelecionado.Removido = false;
                MenuPaginaSelecionado.Ordem = txtOrdem.Text.ToInt32();
                MenuPaginaSelecionado.Url = txtUrl.Text;
                MenuPaginaSelecionado.Icone = cboIcon.Value.ToString();
                if (!e.ExtraParams["paginaPai"].ToString().IsNullOrEmpty())
                    MenuPaginaSelecionado.MenuPaginaPai = new MenuPaginaBO().SelectById(e.ExtraParams["paginaPai"].ToInt32());
                else
                    MenuPaginaSelecionado.MenuPaginaPai = null;

                new MenuPaginaBO(MenuPaginaSelecionado).Salvar();

                ReloadNodes(e);
                base.MostrarMensagem("Página", "Pagina gravada com sucesso", "");
                winPagina.Hide();
                HabilitaBotoes(false);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar Página.";
                e.Success = false;
            }            
        }

        /// <summary>
        /// metodo que preenche os campos na edição de um menu pagina
        /// </summary>
        /// <param name="e"></param>
        private void PreencherCampos(DirectEventArgs e)
        {
            MenuPaginaSelecionado = new MenuPaginaBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtUrl.Text = MenuPaginaSelecionado.Url;
            txtDescricao.Text = MenuPaginaSelecionado.Descricao;
            ckbEmMenu.Checked = MenuPaginaSelecionado.EmMenu;
            txtOrdem.Text = MenuPaginaSelecionado.Ordem.ToString();
            hdfPaginaPai.Value = MenuPaginaSelecionado.MenuPaginaPai != null ? MenuPaginaSelecionado.MenuPaginaPai.Id.ToString() : "";
            ddfPaginaPai.Text = MenuPaginaSelecionado.MenuPaginaPai != null ? MenuPaginaSelecionado.MenuPaginaPai.Descricao : "[Nenhuma]";
            cboIcon.SetValue(MenuPaginaSelecionado.Icone.IsNullOrEmpty() ? Icon.None.ToString() : MenuPaginaSelecionado.Icone.ToString());
        }

        /// <summary>
        /// metodo que carrega as páginas pai no treeview
        /// </summary>
        private Ext.Net.TreeNodeCollection CarregarPaginas(Ext.Net.TreeNodeCollection nodes, Boolean isDrop, MenuPaginaVO pEdit)
        {
            if (nodes == null)
            {
                nodes = new Ext.Net.TreeNodeCollection();
            }

            List<MenuPaginaVO> paginas = new MenuPaginaBO().BuscarPaginasPai();
            if (pEdit != null)
                paginas = paginas.Where(x => x.Id != pEdit.Id).ToList();

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Página");
            root.AllowChildren = true;
            root.Expanded = true;

            nodes.Add(root);

            foreach (MenuPaginaVO p in paginas.OrderBy(x => x.Ordem).ToList())
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.NodeID = p.Id.ToString();
                node.Icon = p.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), p.Icone);
                node.CustomAttributes.Add(new ConfigItem("Id", p.Id.ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Descricao", p.Descricao, ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Url", p.Url, ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("EmMenu", p.EmMenu ? "Sim" : "Não", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Ordem", p.Ordem.ToString(), ParameterMode.Value));
                node.Text = p.Descricao;
                if (p.MenuPaginas != null && p.MenuPaginas.Count > 0)
                {
                    node.Expanded = true;
                    CarregarPaginasFilhas(p.MenuPaginas, node, pEdit);
                }
                else
                    node.Leaf = true;
                root.Nodes.Add(node);
            }

            if (isDrop)
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.Icon = Icon.Folder;
                node.CustomAttributes.Add(new ConfigItem("Id", "", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Descricao", "[Nenhuma]", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Url", "", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("EmMenu", "", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Ordem", "", ParameterMode.Value));
                node.Text = "[Nenhuma]";
                root.Nodes.Insert(0, node);
            }

            return nodes;
        }

        /// <summary>
        /// metodo que carrega as paginas filhas de uma pagina pai
        /// </summary>
        /// <param name="paginasFilhas"></param>
        /// <param name="node"></param>
        private void CarregarPaginasFilhas(IList<MenuPaginaVO> paginasFilhas, Ext.Net.TreeNode node, MenuPaginaVO pEdit)
        {
            if (pEdit != null)
                paginasFilhas = paginasFilhas.Where(x => x.Id != pEdit.Id).ToList();
            paginasFilhas = paginasFilhas.Where(x => x.Removido == false).OrderBy(x => x.Ordem).ToList();
            foreach (MenuPaginaVO p in paginasFilhas)
            {
                Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
                node1.NodeID = p.Id.ToString();
                node1.Icon = p.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), p.Icone);
                node1.CustomAttributes.Add(new ConfigItem("Id", p.Id.ToString(), ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("Descricao", p.Descricao, ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("Url", p.Url, ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("EmMenu", p.EmMenu ? "Sim" : "Não", ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("Ordem", p.Ordem.ToString(), ParameterMode.Value));
                node1.Text = p.Descricao;
                if (p.MenuPaginas != null && p.MenuPaginas.Count > 0)
                {
                    node1.Expanded = true;
                    CarregarPaginasFilhas(p.MenuPaginas, node1, pEdit);
                }
                else
                    node1.Leaf = true;
                node.Nodes.Add(node1);
            }
        }

        /// <summary>
        /// altera o objeto para removido = true
        /// </summary>
        /// <param name="paginasFilhas"></param>
        private void SetRemovidoPaginasFilhas(List<MenuPaginaVO> paginasFilhas)
        {
            foreach (MenuPaginaVO p in paginasFilhas)
            {
                p.Removido = true;
                if (p.MenuPaginas != null && p.MenuPaginas.Count > 0)
                    SetRemovidoPaginasFilhas(p.MenuPaginas.ToList());
            }
        }

        /// <summary>
        /// metodo que envia como resultado os nodes a serem atualizados
        /// </summary>
        /// <param name="e"></param>
        private void ReloadNodes(DirectEventArgs e)
        {
            Ext.Net.TreeNodeCollection nodes = CarregarPaginas(null, false,null);
            e.ExtraParamsResponse["nodes"] = nodes.ToJson();
        }

        /// <summary>
        /// metodo que limpa os campos do form de cadastro de pagina
        /// </summary>
        private void LimparCampos()
        {
            MenuPaginaSelecionado = null;
            txtDescricao.Clear();
            txtOrdem.Clear();
            txtUrl.Clear();
            hdfPaginaPai.Value = String.Empty;
            ddfPaginaPai.Text = "[Nenhuma]";
            cboIcon.Clear();
        }

        /// <summary>
        /// metodo que carrega o combobox de icons das paginas
        /// </summary>
        private void CarregarIcons()
        {
            List<Object> lst = new List<Object>();
            foreach(Icon ic in Enum.GetValues(typeof(Icon)))
            {
                lst.Add(new { IconCls = ResourceManager.GetIconClassName(ic), Name = ic.ToString() });
                base.ResourceManager.RegisterIcon(ic);
            }
            strIcons.DataSource = lst;
            strIcons.DataBind();
        }

        #endregion
    }
}
