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
    public partial class GerenciarEnsalamento : BasePage
    {

        #region propriedades

        private SalaVO SalaSelecionada
        {
            get
            {
                if (this.ViewState["SalaSelecionada"] == null)
                    this.ViewState["SalaSelecionada"] = new SalaVO();
                return (SalaVO)this.ViewState["SalaSelecionada"];
            }
            set { this.ViewState["SalaSelecionada"] = value; }
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
			btnNovo.Disabled = hdfAdicionarEnsalamento.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverSala(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarSala(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winSala.Title = "Cadastrando Sala";
            LimparCampos();
            CarregarSetores(treeSetoresWin.Root, null);
            winSala.Show((Control)sender);
            pnlDropField.Render();
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winSala.Title = "Alterando Sala";
            CarregarSetores(treeSetoresWin.Root, SalaSelecionada.Setor);
            winSala.Show((Control)sender);
            pnlDropField.Render();
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar os setores
        /// </summary>
        private void LoadPagina()
        {
            CarregarSalas();
        }

        /// <summary>
        /// metodo que habilita/desabilita os botões da gerencia
        /// </summary>
        /// <param name="habilitar"></param>
        private void HabilitaBotoes(Boolean habilitar)
        {
            btnEditar.Disabled = !habilitar;
            btnRemover.Disabled = !habilitar;
        }

        /// <summary>
        /// metodo que remove uma sala
        /// </summary>
        /// <param name="e"></param>
        private void RemoverSala(DirectEventArgs e)
        {
            try
            {
                SalaVO sala = new SalaBO().SelectById(e.ExtraParams["id"].ToInt32());
                new SalaBO(sala).DeleteUpdate();
                HabilitaBotoes(false);
                CarregarSalas();
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover sala.", "");
            }
        }

        /// <summary>
        /// metodo chamado para inserir ou atualizar uma sala
        /// </summary>
        /// <param name="e"></param>
        private void SalvarSala(DirectEventArgs e)
        {
            try
            {
                SalaSelecionada.Nome = txtNome.Text;
                if (!e.ExtraParams["setor"].ToString().IsNullOrEmpty())
                    SalaSelecionada.Setor = new EmpresaSetorVO(){ Id = e.ExtraParams["setor"].ToInt32() };
                else
                    SalaSelecionada.Setor = null;

                new SalaBO(SalaSelecionada).Salvar();

                CarregarSalas();
                base.MostrarMensagem("Sala", "Sala gravada com sucesso", "");
                winSala.Hide();
                HabilitaBotoes(false);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar Sala.";
                e.Success = false;
            }            
        }

        /// <summary>
        /// metodo que preenche os campos na edição de uma sala
        /// </summary>
        /// <param name="e"></param>
        private void PreencherCampos(DirectEventArgs e)
        {
            SalaSelecionada = new SalaBO().SelectById(e.ExtraParams["id"].ToInt32());
            txtNome.Text = SalaSelecionada.Nome;
            hdfSetor.Value = SalaSelecionada.Setor != null ? SalaSelecionada.Setor.Id.ToString() : "";
            ddfSetor.Text = SalaSelecionada.Setor != null ? SalaSelecionada.Setor.Nome : "[Nenhum]";
        }

        /// <summary>
        /// metodo que carrega os setores no treeview
        /// </summary>
        private Ext.Net.TreeNodeCollection CarregarSetores(Ext.Net.TreeNodeCollection nodes, EmpresaSetorVO sEdit)
        {
            if (nodes == null)
            {
                nodes = new Ext.Net.TreeNodeCollection();
            }

            List<EmpresaSetorVO> setores = new EmpresaSetorBO().BuscarSetoresPai(null);
            //if (sEdit != null)
            //    setores = setores.Where(x => x.Id != sEdit.Id).ToList();

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Setores");
            root.AllowChildren = true;
            root.Expanded = true;

            nodes.Add(root);

            List<EmpresaVO> empresas = setores.Select(x => x.Empresa).Distinct(new PropertyComparer<EmpresaVO>("Id")).ToList();

            Boolean adicionouRemovido = false;

            Ext.Net.TreeNode node = new Ext.Net.TreeNode();
            foreach (EmpresaVO e in empresas)
            {
                Ext.Net.TreeNode nodeE = new Ext.Net.TreeNode();
                nodeE.Icon = Icon.Folder;
                nodeE.CustomAttributes.Add(new ConfigItem("Id", e.Id.ToString(), ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Nome", e.Nome, ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Empresa", "1", ParameterMode.Value));
                nodeE.Text = e.Nome;

                // adiciona setor removido no treeview
                if (sEdit != null && e.Id == sEdit.Empresa.Id && sEdit.Removido)
                {
                    adicionouRemovido = true;
                    
                    node = new Ext.Net.TreeNode();
                    node.NodeID = sEdit.Id.ToString();
					node.Icon = Icon.ApplicationSideBoxes;
                    node.CustomAttributes.Add(new ConfigItem("Id", sEdit.Id.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("Nome", sEdit.Nome, ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("Empresa", "0", ParameterMode.Value));
                    node.Text = sEdit.Nome;
                    node.Leaf = true;
                    nodeE.Nodes.Add(node);
                }

                foreach (EmpresaSetorVO s in setores.Where(x=> x.Empresa.Id == e.Id).ToList())
                {
                    node = new Ext.Net.TreeNode();
                    node.NodeID = s.Id.ToString();
					node.Icon = Icon.ApplicationSideBoxes;
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

            // adiciona setor removido no treeview
            if (sEdit != null && !adicionouRemovido && sEdit.Removido)
            {

                Ext.Net.TreeNode nodeE = new Ext.Net.TreeNode();
                nodeE.Icon = Icon.Folder;
                nodeE.CustomAttributes.Add(new ConfigItem("Id", sEdit.Empresa.Id.ToString(), ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Nome", sEdit.Empresa.Nome, ParameterMode.Value));
                nodeE.CustomAttributes.Add(new ConfigItem("Empresa", sEdit.Empresa.Id.ToString(), ParameterMode.Value));
                nodeE.Text = sEdit.Empresa.Nome;

                node = new Ext.Net.TreeNode();
                node.NodeID = sEdit.Id.ToString();
				node.Icon = Icon.ApplicationSideBoxes;
                node.CustomAttributes.Add(new ConfigItem("Id", sEdit.Id.ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Nome", sEdit.Nome, ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Empresa", "0", ParameterMode.Value));
                node.Text = sEdit.Nome;
                node.Leaf = true;
                nodeE.Nodes.Add(node);

                root.Nodes.Add(nodeE);
            }

            node = new Ext.Net.TreeNode();
			node.Icon = Icon.ApplicationSideBoxes;
            node.CustomAttributes.Add(new ConfigItem("Id", "", ParameterMode.Value));
            node.CustomAttributes.Add(new ConfigItem("Nome", "[Nenhum]", ParameterMode.Value));
            node.Text = "[Nenhum]";
            root.Nodes.Insert(0, node);
            
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
				node1.Icon = Icon.ApplicationSideBoxes;
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
        /// metodo que limpa os campos do form de cadastro de salas
        /// </summary>
        private void LimparCampos()
        {
            SalaSelecionada = null;
            txtNome.Clear();
            hdfSetor.Value = String.Empty;
            ddfSetor.Text = "[Nenhum]";
        }

        /// <summary>
        /// metodo que carrega o grid de salas
        /// </summary>
        private void CarregarSalas()
        {
            strSalas.DataSource = new SalaBO().Select().ToList();
            strSalas.DataBind();
        }

        #endregion
    }
}
