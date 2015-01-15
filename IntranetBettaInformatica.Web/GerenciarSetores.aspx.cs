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
using IntranetBettaInformatica.Entities.Util;

namespace IntranetBettaInformatica.Web
{
    public partial class GerenciarSetores : BasePage
    {

        #region propriedades

        private EmpresaSetorVO SetorSelecionado
        {
            get
            {
                if (this.ViewState["SetorSelecionado"] == null)
                    this.ViewState["SetorSelecionado"] = new EmpresaSetorVO();
                return (EmpresaSetorVO)this.ViewState["SetorSelecionado"];
            }
            set { this.ViewState["SetorSelecionado"] = value; }
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
			btnNovo.Disabled = !hdfAdicionarSetores.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverSetor(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarSetor(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winSetor.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winSetor.Title = "Cadastrando Setor";
            LimparCampos();
            CarregarSetores(treeSetoresWin.Root, true, null);
            winSetor.Show((Control)sender);
            pnlDropField.Render();
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winSetor.Title = "Alterando Setor";
            CarregarSetores(treeSetoresWin.Root, true, SetorSelecionado);
            winSetor.Show((Control)sender);
            pnlDropField.Render();
        }

        protected void treeSetores_SelectedRow(object sender, DirectEventArgs e)
        {
            HabilitaBotoes(!e.ExtraParams["node"].ToInt32().ToBoolean());
        }

		#endregion

        #region metodos

        /// <summary>
        /// metoco para carregar os setores
        /// </summary>
        private void LoadPagina()
        {
            CarregarSetores(treeSetores.Root, false, null);
            CarregarEmpresas();
            HabilitaBotoes(false);
			btnChartOrganization.Disabled = true;
        }

        /// <summary>
        /// metodo que habilita/desabilita os botões da gerencia
        /// </summary>
        /// <param name="habilitar"></param>
        private void HabilitaBotoes(Boolean habilitar)
        {
            btnEditar.Disabled = !habilitar || !hdfEditarSetores.Value.ToInt32().ToBoolean();
            btnRemover.Disabled = !habilitar || !hdfRemoverSetores.Value.ToInt32().ToBoolean();
			btnChartOrganization.Disabled = habilitar || !hdfVisualizarChartSetores.Value.ToInt32().ToBoolean();
        }

        /// <summary>
        /// metodo que remove um setor
        /// </summary>
        /// <param name="e"></param>
        private void RemoverSetor(DirectEventArgs e)
        {
            try
            {
                EmpresaSetorVO setor = new EmpresaSetorBO().SelectById(e.ExtraParams["id"].ToInt32());
                SetRemovidoSetoresFilhos(setor.SetoresFilhos.ToList());
                new EmpresaSetorBO(setor).DeleteUpdate();
                HabilitaBotoes(false);
                ReloadNodes(e);
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover setor.", "");
            }
        }

        /// <summary>
        /// metodo chamado para inserir ou atualizar um menu setor
        /// </summary>
        /// <param name="e"></param>
        private void SalvarSetor(DirectEventArgs e)
        {
            try
            {
                SetorSelecionado.Empresa = new EmpresaVO() { Id = cboEmpresa.Value.ToInt32() };
                SetorSelecionado.Nome = txtNome.Text;
                if (!e.ExtraParams["setorPai"].ToString().IsNullOrEmpty())
                    SetorSelecionado.SetorPai = new EmpresaSetorBO().SelectById(e.ExtraParams["setorPai"].ToInt32());
                else
                    SetorSelecionado.SetorPai = null;

                new EmpresaSetorBO(SetorSelecionado).Salvar();

                ReloadNodes(e);
                base.MostrarMensagem("Setor", "Setor gravado com sucesso", "");
                winSetor.Hide();
                HabilitaBotoes(false);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar Setor.";
                e.Success = false;
            }            
        }

        /// <summary>
        /// metodo que preenche os campos na edição de um menu pagina
        /// </summary>
        /// <param name="e"></param>
        private void PreencherCampos(DirectEventArgs e)
        {
            SetorSelecionado = new EmpresaSetorBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtNome.Text = SetorSelecionado.Nome;
            hdfSetorPai.Value = SetorSelecionado.SetorPai != null ? SetorSelecionado.SetorPai.Id.ToString() : "";
            ddfSetorPai.Text = SetorSelecionado.SetorPai != null ? SetorSelecionado.SetorPai.Nome : "[Nenhuma]";
            cboEmpresa.SetValue(SetorSelecionado.Empresa.Id);
        }

        /// <summary>
        /// metodo que carrega as páginas pai no treeview
        /// </summary>
        private Ext.Net.TreeNodeCollection CarregarSetores(Ext.Net.TreeNodeCollection nodes, Boolean isDrop, EmpresaSetorVO sEdit)
        {
            if (nodes == null)
            {
                nodes = new Ext.Net.TreeNodeCollection();
            }

            List<EmpresaSetorVO> setores = new EmpresaSetorBO().BuscarSetoresPai(null);
            if (sEdit != null)
                setores = setores.Where(x => x.Id != sEdit.Id).ToList();

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Setores");
            root.AllowChildren = true;
            root.Expanded = true;

            nodes.Add(root);

            List<EmpresaVO> empresas = setores.Select(x => x.Empresa).Distinct(new PropertyComparer<EmpresaVO>("Id")).ToList();

            foreach (EmpresaVO e in empresas)
            {
                Ext.Net.TreeNode nodeE = new Ext.Net.TreeNode();
                nodeE.Icon = Icon.Folder;
                nodeE.CustomAttributes.Add(new ConfigItem("Id", e.Id.ToString(), ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Nome", e.Nome, ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Empresa", "1", ParameterMode.Value));
                nodeE.Text = e.Nome;

                foreach (EmpresaSetorVO s in setores.Where(x=> x.Empresa.Id == e.Id).ToList())
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.NodeID = s.Id.ToString();
                    node.Icon = treeSetores.Icon;
                    node.CustomAttributes.Add(new ConfigItem("Id", s.Id.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("Nome", s.Nome, ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("Empresa", "0", ParameterMode.Value));
                    node.Text = s.Nome;
                    if (s.SetoresFilhos != null && s.SetoresFilhos.Count > 0)
                    {
                        node.Expanded = true;
                        CarregarSetoresFilhos(s.SetoresFilhos, node, sEdit);
                    }
                    else
                        node.Leaf = true;
                    nodeE.Nodes.Add(node);
                }
                root.Nodes.Add(nodeE);
            }

            if (isDrop)
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.Icon = treeSetores.Icon;
                node.CustomAttributes.Add(new ConfigItem("Id", "", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Nome", "[Nenhum]", ParameterMode.Value));
                node.Text = "[Nenhum]";
                root.Nodes.Insert(0, node);
            }

            return nodes;
        }

        /// <summary>
        /// metodo que carrega as paginas filhas de uma pagina pai
        /// </summary>
        /// <param name="paginasFilhas"></param>
        /// <param name="node"></param>
        private void CarregarSetoresFilhos(IList<EmpresaSetorVO> setoresFilhos, Ext.Net.TreeNode node, EmpresaSetorVO sEdit)
        {
            if (sEdit != null)
                setoresFilhos = setoresFilhos.Where(x => x.Id != sEdit.Id).ToList();
            setoresFilhos = setoresFilhos.Where(x => x.Removido == false).ToList();
            foreach (EmpresaSetorVO s in setoresFilhos)
            {
                Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
                node1.NodeID = s.Id.ToString();
                node1.Icon = treeSetores.Icon;
                node1.CustomAttributes.Add(new ConfigItem("Id", s.Id.ToString(), ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("Nome", s.Nome, ParameterMode.Value));
                node1.CustomAttributes.Add(new ConfigItem("Empresa", "0", ParameterMode.Value));
                node1.Text = s.Nome;
                if (s.SetoresFilhos != null && s.SetoresFilhos.Count > 0)
                {
                    node1.Expanded = true;
                    CarregarSetoresFilhos(s.SetoresFilhos, node1, sEdit);
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
        private void SetRemovidoSetoresFilhos(List<EmpresaSetorVO> setoresFilhos)
        {
            foreach (EmpresaSetorVO s in setoresFilhos)
            {
                s.Removido = true;
                if (s.SetoresFilhos != null && s.SetoresFilhos.Count > 0)
                    SetRemovidoSetoresFilhos(s.SetoresFilhos.ToList());
            }
        }

        /// <summary>
        /// metodo que envia como resultado os nodes a serem atualizados
        /// </summary>
        /// <param name="e"></param>
        private void ReloadNodes(DirectEventArgs e)
        {
            Ext.Net.TreeNodeCollection nodes = CarregarSetores(null, false,null);
            e.ExtraParamsResponse["nodes"] = nodes.ToJson();
        }

        /// <summary>
        /// metodo que limpa os campos do form de cadastro de setor
        /// </summary>
        private void LimparCampos()
        {
            SetorSelecionado = null;
            txtNome.Clear();
            hdfSetorPai.Value = String.Empty;
            ddfSetorPai.Text = "[Nenhum]";
            cboEmpresa.Clear();
        }

        /// <summary>
        /// metodo que carrega o combobox de empresas
        /// </summary>
        private void CarregarEmpresas()
        {
            strEmpresas.DataSource = new EmpresaBO().Select().Where(x=> !x.Removido).ToList();
            strEmpresas.DataBind();
        }

        #endregion
    }
}
