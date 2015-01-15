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
    public partial class GerenciarContatos : BasePage
    {

        #region propriedades

        private UsuarioVO UsuarioSelecionado
        {
            get
            {
                if (this.ViewState["UsuarioSelecionado"] == null)
                    return null;
                return (UsuarioVO)this.ViewState["UsuarioSelecionado"];
            }
            set { this.ViewState["UsuarioSelecionado"] = value; }
        }

        private EmpresaVO EmpresaSelecionada
        {
            get
            {
                if (this.ViewState["EmpresaSelecionada"] == null)
                    return null;
                return (EmpresaVO)this.ViewState["EmpresaSelecionada"];
            }
            set { this.ViewState["EmpresaSelecionada"] = value; }
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
                CarregarEstados();
                CarregarTiposEmpresaBusca();
                CarregarEmpresasBusca();
                LoadPagina();
                base.SetTituloIconePagina(frmTitulo);
            }
			btnNovo.Disabled = hdfAdicionarContatos.Value.ToInt32() == 0;
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverContato(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarContato(e);
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winContato.Title = "Cadastrando Contato";
            UsuarioSelecionado = null;
            EmpresaSelecionada = null;
            CarregarEmpresas(null);
            CarregarSetores(treeSetores.Root, null, null);
            CarregarTiposEmpresa(null);
            LimparCampos();
            strTelefones.DataSource = new List<Object>();
            strTelefones.DataBind();
            winContato.Show((Control)sender);
            pnlDropField.Render(frmContato, 3, RenderMode.InsertTo);
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winContato.Title = "Alterando Contato";
            winContato.Show((Control)sender);
            pnlDropField.Render(frmContato, 3, RenderMode.InsertTo);
        }

        protected void cboEmpresa_Click(object sender, DirectEventArgs e)
        {
            EmpresaVO empresa = cboEmpresa.Value.ToInt32() == 0 ? null : new EmpresaVO(){ Id = cboEmpresa.Value.ToInt32() };
            CarregarSetores(treeSetores.Root, empresa, null);
            hdfSetor.Value = "0";
            ddfSetor.Text = "[Nenhum]";
            pnlDropField.Render(frmContato, 3, RenderMode.InsertTo);
        }

        protected void rdbCheck_Click(object sender, DirectEventArgs e)
        {
            Radio radio = sender as Radio;
            if(radio.Checked)
            {
                hdfSetor.Value = "0";
                ddfSetor.Text = "[Nenhum]";

                cboEmpresa.SetValue(0);
                cboEmpresa.Disabled = radio.ID != "rdbFisica";
                ddfSetor.Disabled = radio.ID != "rdbFisica";
                cboTipoEmpresa.Disabled = radio.ID == "rdbFisica";
                cboTipoEmpresa.AllowBlank = radio.ID == "rdbFisica";
            }
        }

        protected void btnBuscar_Click(object sender, DirectEventArgs e)
        {
            LoadPagina();
        }

        protected void rdbTipoPessoaBusca_Click(object sender, DirectEventArgs e)
        {
            Radio radio = (sender as RadioGroup).CheckedItems[0];
            if (radio.Checked)
            {
                cboEmpresaBusca.Hidden = !(radio.ID == "rdbFisicaBusca");
                cboTipoEmpresaBusca.Hidden = !(radio.ID == "rdbJuridicaBusca");
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            List<UsuarioVO> usuarios = new List<UsuarioVO>();
            List<EmpresaVO> empresas = new List<EmpresaVO>();
            String nome = txtNomeBusca.Text;
            EmpresaVO empresa = null;
            TipoEmpresaVO tipo = null;

            if (rdbFisicaBusca.Checked || rdbTodasBusca.Checked)
            {
                empresa = cboEmpresaBusca.SelectedIndex == 0 ? null : new EmpresaVO() { Id = cboEmpresaBusca.Value.ToInt32() };
                usuarios = new UsuarioBO().BuscarContatos(nome, empresa, false);
            }
            if (rdbJuridicaBusca.Checked || rdbTodasBusca.Checked)
            {
                tipo = cboTipoEmpresaBusca.SelectedIndex == 0 ? null : new TipoEmpresaVO() { Id = cboTipoEmpresaBusca.Value.ToInt32() };
                empresas = new EmpresaBO().BuscarContatos(nome, tipo, false);
            }

            List<object> lstFinal = usuarios.Select<UsuarioVO, object>(
                x => new { 
                    Id = x.Id, 
                    Nome = x.Nome, 
                    Endereco = x.Endereco, 
                    Cidade = x.Cidade, 
                    Estado = x.Estado, 
                    Email = x.Email,
                    Empresa = x.Empresa,
                    Setor = x.Setor,
                    TipoPessoa = "Física",
                    Telefones = String.Join(", ", x.Telefones.Select(x1 => x1.Telefone))
                }
            ).ToList();

            lstFinal = lstFinal.Union(
                empresas.Select<EmpresaVO, object>(
                    x=> new {
                        Id = x.Id,
                        Nome = x.Nome,
                        Endereco = x.Endereco,
                        Cidade = x.Cidade,
                        Estado = x.Estado,
                        Email = x.Email,
                        Empresa = new EmpresaVO(),
                        Setor = new EmpresaSetorVO(),
                        TipoPessoa = "Jurídica",
                        Telefones = String.Join(", ", x.Telefones.Select(x1=> x1.Telefone))
                    }
                )
            ).ToList();

            strContatos.DataSource = lstFinal;
            strContatos.DataBind();
        }

        private void CarregarEmpresas(EmpresaVO empresa)
        {
            List<EmpresaVO> empresas = new EmpresaBO().Select().Where(x => x.RemovidoContato == false || (empresa != null && x.Id == empresa.Id)).ToList();
            empresas.Insert(0, new EmpresaVO(){ Nome="[Nenhuma]", Id = 0});
            strEmpresas.DataSource = empresas;
            strEmpresas.DataBind();

            cboEmpresa.SetValue(0);
        }

        private void CarregarEmpresasBusca()
        {
            strEmpresasBusca.DataSource = new EmpresaBO().Select().Where(x => x.RemovidoContato == false).ToList();
            strEmpresasBusca.DataBind();
            cboEmpresaBusca.Items.Insert(0, new Ext.Net.ListItem("[Todas]", ""));
            cboEmpresaBusca.SelectedIndex = 0;
        }

        private void CarregarTiposEmpresaBusca()
        {
            strTiposEmpresaBusca.DataSource = new TipoEmpresaBO().Select().Where(x => x.Removido == false).ToList();
            strTiposEmpresaBusca.DataBind();
            cboTipoEmpresaBusca.Items.Insert(0, new Ext.Net.ListItem("[Todos]", ""));
            cboTipoEmpresaBusca.SelectedIndex = 0;
        }

        private void CarregarTiposEmpresa(TipoEmpresaVO tipo)
        {
            strTiposEmpresa.DataSource = new TipoEmpresaBO().Select().Where(x => x.Removido == false || (tipo != null && x.Id == tipo.Id)).ToList();
            strTiposEmpresa.DataBind();
        }

        private void CarregarEstados()
        {
            strEstados.DataSource = new EstadoBO().Select();
            strEstados.DataBind();
        }

        private void RemoverContato(DirectEventArgs e)
        {
            try
            {
                if (e.ExtraParams["tipoPessoa"] == "Física")
                {
                    UsuarioVO usuario = new UsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
                    new UsuarioBO().DeleteContato(usuario);
                }
                else
                {
                    EmpresaVO empresa = new EmpresaBO().SelectById(e.ExtraParams["id"].ToInt32());
                    new EmpresaBO().DeleteContato(empresa);
                }
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover contato.", "");
            }
        }

        private void SalvarContato(DirectEventArgs e)
        {
            try
            {
                if (e.ExtraParams["tipoPessoa"] == "Física")
                {
                    UsuarioVO usuario = new UsuarioVO();
                    if (base.AcaoTela == Common.AcaoTela.Edicao)
                        usuario = UsuarioSelecionado;

                    usuario.Email = txtEmail.Text;
                    usuario.Endereco = txtEndereco.Text;
                    usuario.Nome = txtNome.Text;
                    usuario.Cidade = txtCidade.Text;
                    if (cboEmpresa.Value.ToInt32() != 0)
                        usuario.Empresa = new EmpresaVO() { Id = cboEmpresa.Value.ToInt32() };
                    else
                        usuario.Empresa = null;

                    if (cboEstado.Value != null && !cboEstado.Value.ToString().IsNullOrEmpty())
                        usuario.Estado = new EstadoVO() { Id = cboEstado.Value.ToInt32() };
                    else
                        usuario.Estado = null;

                    if (hdfSetor.Value.ToString() != "0")
                        usuario.Setor = new EmpresaSetorVO() { Id = hdfSetor.Value.ToInt32() };
                    else
                        usuario.Setor = null;

                    usuario.Telefones = JSON.Deserialize<List<UsuarioTelefoneVO>>(e.ExtraParams["telefones"]);
                    usuario.Telefones.ToList().ForEach(x => x.Usuario = usuario);

                    if (base.AcaoTela == Common.AcaoTela.Inclusao)
                    {
                        usuario.Removido = true;
                        usuario.UsuarioSistema = false;
                    }

                    usuario.RemovidoContato = false;
                    
                    new UsuarioBO(usuario).Salvar();
                }
                else
                {
                    EmpresaVO empresa = new EmpresaVO();
                    if (base.AcaoTela == Common.AcaoTela.Edicao)
                        empresa = EmpresaSelecionada;

                    empresa.Email = txtEmail.Text;
                    empresa.Endereco = txtEndereco.Text;
                    empresa.Nome = txtNome.Text;
                    empresa.Cidade = txtCidade.Text;
                    empresa.TipoEmpresa = new TipoEmpresaVO() { Id = cboTipoEmpresa.Value.ToInt32() };
                    if (cboEstado.Value != null && !cboEstado.Value.ToString().IsNullOrEmpty())
                        empresa.Estado = new EstadoVO() { Id = cboEstado.Value.ToInt32() };
                    else
                        empresa.Estado = null;

                    empresa.Telefones = JSON.Deserialize<List<EmpresaTelefoneVO>>(e.ExtraParams["telefones"]);
                    empresa.Telefones.ToList().ForEach(x => x.Empresa = empresa);

                    if(base.AcaoTela == Common.AcaoTela.Inclusao)
                        empresa.Removido = true;
                    
                    empresa.RemovidoContato = false;
                    
                    new EmpresaBO(empresa).Salvar();
                }

                base.MostrarMensagem("Contato", "Contato gravado com sucesso.", "");
                
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
                winContato.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar contato.";
                e.Success = false;
            }            
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            if (e.ExtraParams["tipoPessoa"] == "Física")
            {
                UsuarioSelecionado = new UsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
                CarregarEmpresas(UsuarioSelecionado.Empresa);
                CarregarSetores(treeSetores.Root, UsuarioSelecionado.Empresa, UsuarioSelecionado.Setor);
                CarregarTiposEmpresa(null);
                rdbFisica.Checked = true;
                
                txtNome.Text = UsuarioSelecionado.Nome;
                txtEmail.Text = UsuarioSelecionado.Email;
                txtEndereco.Text = UsuarioSelecionado.Endereco;
                txtCidade.Text = UsuarioSelecionado.Cidade;
                if (UsuarioSelecionado.Empresa != null)
                    cboEmpresa.SetValue(UsuarioSelecionado.Empresa.Id);
                else
                    cboEmpresa.SetValue(0);
                hdfSetor.Value = UsuarioSelecionado.Setor != null ? UsuarioSelecionado.Setor.Id.ToString() : "0";
                ddfSetor.Text = UsuarioSelecionado.Setor != null ? UsuarioSelecionado.Setor.Nome : "[Nenhum]";
                
                cboTipoEmpresa.AllowBlank = true;
                cboTipoEmpresa.Disabled = true;

                if (UsuarioSelecionado.Estado != null)
                    cboEstado.SetValue(UsuarioSelecionado.Estado.Id);
            }
            else
            {
                EmpresaSelecionada = new EmpresaBO().SelectById(e.ExtraParams["id"].ToInt32());
                CarregarTiposEmpresa(EmpresaSelecionada.TipoEmpresa);
                CarregarEmpresas(null);
                rdbJuridica.Checked = true;

                txtNome.Text = EmpresaSelecionada.Nome;
                txtEmail.Text = EmpresaSelecionada.Email;
                txtEndereco.Text = EmpresaSelecionada.Endereco;
                txtCidade.Text = EmpresaSelecionada.Cidade;

                if (EmpresaSelecionada.Estado != null)
                    cboEstado.SetValue(EmpresaSelecionada.Estado.Id);

                cboEmpresa.SetValue(0);
                cboTipoEmpresa.Disabled = false;
                cboTipoEmpresa.SetValue(EmpresaSelecionada.TipoEmpresa.Id);
                cboTipoEmpresa.AllowBlank = false;
            }
            rdbGroupTipoContato.Disabled = true;
            CarregarTelefones();
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtCidade.Clear();
            cboTipoEmpresa.Clear();
            cboEstado.Clear();
            
            cboTipoEmpresa.AllowBlank = true;
            cboTipoEmpresa.Disabled = true;
			rdbFisica.Checked = true;
			rdbJuridica.Checked = false;
			rdbGroupTipoContato.Disabled = false;

            txtNome.ClearInvalid();
            txtEmail.ClearInvalid();
            txtEndereco.ClearInvalid();
            txtCidade.ClearInvalid();
            cboTipoEmpresa.ClearInvalid();
            
            cboEmpresa.ClearInvalid();
            cboEstado.ClearInvalid();
            
            hdfSetor.Value = "0";
            ddfSetor.SetRawValue("[Nenhum]");

            rdbFisica.Checked = true;
            btnRemoveTelefone.Disabled = true;
            btnAddTelefone.Disabled = false;

            tabContato.ActiveTabIndex = 0;
        }

        private void CarregarTelefones()
        {
            if(UsuarioSelecionado != null)
                strTelefones.DataSource = UsuarioSelecionado.Telefones;
            else
                strTelefones.DataSource = EmpresaSelecionada.Telefones;
            strTelefones.DataBind();
        }

        /// <summary>
        /// metodo que carrega as páginas pai no treeview
        /// </summary>
        private Ext.Net.TreeNodeCollection CarregarSetores(Ext.Net.TreeNodeCollection nodes, EmpresaVO empresa, EmpresaSetorVO sEdit)
        {
            if (nodes == null)
            {
                nodes = new Ext.Net.TreeNodeCollection();
            }

            List<EmpresaSetorVO> setores = empresa == null ? new List<EmpresaSetorVO>() : new EmpresaSetorBO().BuscarSetoresPai(empresa);
            if (sEdit != null && sEdit.Removido && sEdit.SetorPai == null)
            {
                sEdit.SetoresFilhos = new List<EmpresaSetorVO>();
                setores.Add(sEdit);
            }

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Setores");
            root.AllowChildren = true;
            root.Expanded = true;

            nodes.Add(root);

            foreach (EmpresaSetorVO s in setores)
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.NodeID = s.Id.ToString();
                node.Icon = treeSetores.Icon;
                node.CustomAttributes.Add(new ConfigItem("Id", s.Id.ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("Nome", s.Nome, ParameterMode.Value));
                node.Text = s.Nome;
                
                if (s.SetoresFilhos != null && s.SetoresFilhos.Count > 0)
                {
                    node.Expanded = true;
                    CarregarSetoresFilhos(s.SetoresFilhos, node, sEdit);
                }
                else
                    node.Leaf = true;
                root.Nodes.Add(node);
            }
            
            Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
            node1.Icon = treeSetores.Icon;
            node1.CustomAttributes.Add(new ConfigItem("Id", "0", ParameterMode.Value));
            node1.CustomAttributes.Add(new ConfigItem("Nome", "[Nenhum]", ParameterMode.Value));
            node1.Text = "[Nenhum]";
            root.Nodes.Insert(0, node1);
            
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

        #endregion
    }
}
