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
    public partial class GerenciarUsuarios : BasePage
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
			btnNovo.Disabled = !hdfAdicionarUsuarios.Value.ToInt32().ToBoolean();
        }

        protected void OnRefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            LoadPagina();
        }

        protected void OnRefreshDataSistemas(object sender, StoreRefreshDataEventArgs e)
        {
            CarregarSistemas();
        }

        protected void btnRemover_Click(object sender, DirectEventArgs e)
        {
            RemoverUsuario(e);
        }

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarUsuario(e);
        }

        protected void SalvarTelefones_Click(object sender, DirectEventArgs e)
        {
            SalvarTelefones(e);
        }

        protected void Cancelar_Click(object sender, DirectEventArgs e)
        {
            winUsuario.Hide();
        }

        protected void CancelarTelefones_Click(object sender, DirectEventArgs e)
        {
            winTelefones.Hide();
        }

        protected void btnNovo_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Inclusao;
            winUsuario.Title = "Cadastrando Usuario";
            hdfAcaoTela.Value = "0";
            LimparCampos();
            CarregarEmpresas(null);
            CarregarPerfisAcesso(null);
            CarregarTemas(null);
            CarregarSistemas();
			CarregarFuncoes(null);
            winUsuario.Show((Control)sender);
            pnlDropField.Render(frmUsuario, 2, RenderMode.InsertTo);
            tabSistemas.Render();
        }

        protected void btnEditar_Click(object sender, DirectEventArgs e)
        {
            base.AcaoTela = Common.AcaoTela.Edicao;
            LimparCampos();
            PreencherCampos(e);
            winUsuario.Title = "Alterando Usuario";
            winUsuario.Show((Control)sender);
            pnlDropField.Render(frmUsuario, 2, RenderMode.InsertTo);
            tabSistemas.Render();
        }

        protected void btnTelefones_Click(object sender, DirectEventArgs e)
        {
            CarregarTelefones(e);
            btnRemoveTelefone.Disabled = true;
            btnSalvarTelefones.Disabled = false;
            btnAddTelefone.Disabled = false;
            winTelefones.Show();
        }

        protected void cboEmpresa_Click(object sender, DirectEventArgs e)
        {
            EmpresaVO empresa = new EmpresaVO(){ Id = cboEmpresa.Value.ToInt32() };
            CarregarSetores(treeSetores.Root, empresa, null);
            hdfSetor.Value = "";
            ddfSetor.Text = "[Nenhum]";
            pnlDropField.Render(frmUsuario, 2, RenderMode.InsertTo);
        }

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            List<UsuarioVO> usuarios = new UsuarioBO().BuscarUsuarios(false);
            strUsuarios.DataSource = usuarios;
            strUsuarios.DataBind();

            CarregarEstados();
        }

        private void CarregarTemas(TemaVO tema)
        {
            strTemas.DataSource = new TemaBO().Select().Where(x => x.Removido == false || (tema != null && x.Id == tema.Id)).ToList();
            strTemas.DataBind();
        }

        private void CarregarEmpresas(EmpresaVO empresa)
        {
            strEmpresas.DataSource = new EmpresaBO().Select().Where(x => x.Removido == false || (empresa != null && x.Id == empresa.Id)).ToList();
            strEmpresas.DataBind();
        }

        private void CarregarPerfisAcesso(PerfilAcessoVO perfil)
        {
            List<PerfilAcessoVO> perfis = new PerfilAcessoBO().Select().Where(x => x.Removido == false || (perfil != null && x.Id == perfil.Id)).ToList();
            if(base.AcaoTela == Common.AcaoTela.Inclusao && !base.EModerador)
                perfis.RemoveAt(0);
            strPerfisAcesso.DataSource = perfis;
            strPerfisAcesso.DataBind();
        }

        private void CarregarSistemas()
        {
            List<SistemaVO> sistemasEdit = base.AcaoTela == Common.AcaoTela.Inclusao ? new List<SistemaVO>() : UsuarioSelecionado.Sistemas.ToList() ;
            List<SistemaVO> sistemas = new SistemaBO().Buscar(false, sistemasEdit);
            strSistemas.DataSource = sistemas;
            strSistemas.DataBind();
        }

        private void CarregarEstados()
        {
            strEstados.DataSource = new EstadoBO().Select();
            strEstados.DataBind();
        }

		private void CarregarFuncoes(FuncaoVO func)
		{
			strFuncoes.DataSource = new FuncaoBO().BuscarFuncoes(func);
			strFuncoes.DataBind();
		}

        private void RemoverUsuario(DirectEventArgs e)
        {
            try
            {
                UsuarioVO usuario = new UsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
                new UsuarioBO(usuario).DeleteUpdate();
                LoadPagina();
                btnEditar.Disabled = true;
                btnRemover.Disabled = true;
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao tentar remover usuario.", "");
            }
        }

        private void SalvarUsuario(DirectEventArgs e)
        {
            try
            {
                UsuarioVO usuario = new UsuarioVO();
                if (base.AcaoTela == Common.AcaoTela.Edicao)
                    usuario = UsuarioSelecionado;

                usuario.Email = txtEmail.Text;
                usuario.Endereco = txtEndereco.Text;
                usuario.Nome = txtNome.Text;
                usuario.Cidade = txtCidade.Text;
                usuario.Empresa = new EmpresaVO() { Id = cboEmpresa.Value.ToInt32() };
                usuario.Tema = new TemaVO() { Id = cboTema.Value.ToInt32() };
                usuario.UsuarioSistema = chkUsuarioSistema.Checked;
                
                usuario.Sistemas = JSON.Deserialize<List<SistemaVO>>(e.ExtraParams["sistemas"]);
                
                if (!txtDataNascimento.Text.IsNullOrEmpty())
                    usuario.DataNascimento = Convert.ToDateTime(txtDataNascimento.Text);
                else
                    usuario.DataNascimento = null;

                if (cboEstado.Value != null && !cboEstado.Value.ToString().IsNullOrEmpty())
                    usuario.Estado = new EstadoVO() { Id = cboEstado.Value.ToInt32() };
                else
                    usuario.Estado = null;

				if (cboFuncao.Value != null && !cboFuncao.Value.ToString().IsNullOrEmpty())
				{
					usuario.Funcao = new FuncaoVO() { Id = cboFuncao.Value.ToInt32() };
				}
				else
					usuario.Funcao = null;
                
                if (!hdfSetor.Value.ToString().IsNullOrEmpty())
                    usuario.Setor = new EmpresaSetorVO() { Id = hdfSetor.Value.ToInt32() };
                else
                    usuario.Setor = null;

                if (chkUsuarioSistema.Checked)
                {
                    usuario.PerfilAcesso = new PerfilAcessoVO() { Id = cboPerfilAcesso.Value.ToInt32() };
                    usuario.Login = txtLogin.Text;
                    usuario.Twitter = txtTwitter.Text;

                    Boolean validaLogin = new UsuarioBO().ValidarLogin(usuario, base.AcaoTela == Common.AcaoTela.Edicao);
                    if (!validaLogin)
                    {
                        base.MostrarMensagem("Erro", "Login existente.", "");
                        return;
                    }

                    if(!txtSenha.Text.IsNullOrEmpty())
                        usuario.Senha = UsuarioBO.EncriptyPassword(txtSenha.Text);

                    if (!txtPalavraChave.Text.IsNullOrEmpty())
                        usuario.PalavraChave = UsuarioBO.EncriptyPassword(txtPalavraChave.Text);
                }
                else
                {
                    usuario.PerfilAcesso = null;
                    usuario.Login = String.Empty;
                    usuario.Senha = String.Empty;
                    usuario.PalavraChave = String.Empty;
                    usuario.Twitter = String.Empty;
                }
                
                usuario.Removido = false;
                
                new UsuarioBO(usuario).Salvar();

                base.MostrarMensagem("Usuario", "Usuario gravado com sucesso.", "");
                
                LoadPagina();
                winUsuario.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar usuario.";
                e.Success = false;
            }            
        }

        private void SalvarTelefones(DirectEventArgs e)
        {
            try
            {
                UsuarioSelecionado.Telefones = JSON.Deserialize<List<UsuarioTelefoneVO>>(e.ExtraParams["telefones"]);
                UsuarioSelecionado.Telefones.ToList().ForEach(x => x.Usuario = UsuarioSelecionado);
                new UsuarioBO(UsuarioSelecionado).Salvar();
                winTelefones.Hide();
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Erro ao salvar telefones do usuario.";
                e.Success = false;
            }
			base.MostrarMensagem("Telefones", "Telefones gravados com sucesso", String.Empty);
        }

        private void PreencherCampos(DirectEventArgs e)
        {
            UsuarioSelecionado = new UsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
            CarregarEmpresas(UsuarioSelecionado.Empresa);
            CarregarPerfisAcesso(UsuarioSelecionado.PerfilAcesso);
            CarregarTemas(UsuarioSelecionado.Tema);
			CarregarFuncoes(UsuarioSelecionado.Funcao);

            txtNome.Text = UsuarioSelecionado.Nome;
            txtEmail.Text = UsuarioSelecionado.Email;
            txtEndereco.Text = UsuarioSelecionado.Endereco;
            txtCidade.Text = UsuarioSelecionado.Cidade;
            txtTwitter.Text = UsuarioSelecionado.Twitter;
            txtDataNascimento.Text = UsuarioSelecionado.DataNascimento != null ? UsuarioSelecionado.DataNascimento.Value.ToString("dd/MM/yyyy") : String.Empty;
            chkUsuarioSistema.Checked = UsuarioSelecionado.UsuarioSistema;

            if(UsuarioSelecionado.Empresa != null)
                cboEmpresa.SetValue(UsuarioSelecionado.Empresa.Id);
			if (UsuarioSelecionado.Funcao != null)
				cboFuncao.SetValue(UsuarioSelecionado.Funcao.Id);

            cboTema.SetValue(UsuarioSelecionado.Tema.Id);

            CarregarSetores(treeSetores.Root, UsuarioSelecionado.Empresa, UsuarioSelecionado.Setor);

            hdfSetor.Value = UsuarioSelecionado.Setor != null ? UsuarioSelecionado.Setor.Id.ToString() : "";
            ddfSetor.Text = UsuarioSelecionado.Setor != null ? UsuarioSelecionado.Setor.Nome : "[Nenhum]";
            
            txtSenha.AllowBlank = true;
            txtConfSenha.AllowBlank = true;
            txtPalavraChave.AllowBlank = true;
            cboPerfilAcesso.AllowBlank = !UsuarioSelecionado.UsuarioSistema;
            txtLogin.AllowBlank = !UsuarioSelecionado.UsuarioSistema;

            if(UsuarioSelecionado.Estado != null)
                cboEstado.SetValue(UsuarioSelecionado.Estado.Id);
            if (UsuarioSelecionado.UsuarioSistema)
            {
                cboPerfilAcesso.SetValue(UsuarioSelecionado.PerfilAcesso.Id);
				txtLogin.Text = UsuarioSelecionado.Login;
            }

            chkUsuarioSistema.Disabled = !base.EModerador && UsuarioSelecionado.PerfilAcesso.Id == 1;
            if (!base.EModerador && UsuarioSelecionado.PerfilAcesso.Id == 1)
            {
                txtLogin.Disabled = true;
                txtSenha.Disabled = true;
                txtConfSenha.Disabled = true;
                txtPalavraChave.Disabled = true;
                cboPerfilAcesso.Disabled = true;
            }

			if (UsuarioSelecionado.Id == 1)
			{
				cboPerfilAcesso.Disabled = true;
			}

            CarregarSistemas();

            RowSelectionModel sm = this.grdSistemas.SelectionModel.Primary as RowSelectionModel;
            UsuarioSelecionado.Sistemas.ToList().ForEach(x=> sm.SelectedRows.Add(new SelectedRow(0)));
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtCidade.Clear();
            txtConfSenha.Clear();
            txtSenha.Clear();
            txtDataNascimento.Clear();
            txtLogin.Clear();
            txtPalavraChave.Clear();
            txtTelefone.Clear();
            cboEmpresa.Clear();
            cboEstado.Clear();
            cboPerfilAcesso.Clear();
            cboTema.Clear();
			cboFuncao.Clear();
            txtTwitter.Clear();
			
            txtNome.ClearInvalid();
            txtEmail.ClearInvalid();
            txtEndereco.ClearInvalid();
            txtCidade.ClearInvalid();
            txtConfSenha.ClearInvalid();
            txtSenha.ClearInvalid();
            txtDataNascimento.ClearInvalid();
            txtLogin.ClearInvalid();
            txtPalavraChave.ClearInvalid();
            txtTelefone.ClearInvalid();
            cboEmpresa.ClearInvalid();
            cboEstado.ClearInvalid();
            cboPerfilAcesso.ClearInvalid();
            cboTema.ClearInvalid();
			cboFuncao.ClearInvalid();

            txtConfSenha.AllowBlank = false;
            txtSenha.AllowBlank = false;
            txtPalavraChave.AllowBlank = false;

            chkUsuarioSistema.Disabled = false;
			cboPerfilAcesso.Disabled = false;

            hdfSetor.Value = "";
            ddfSetor.Text = "[Nenhum]";                

            chkUsuarioSistema.Checked = true;
        }

        private void CarregarTelefones(DirectEventArgs e)
        {
            UsuarioSelecionado = new UsuarioBO().SelectById(e.ExtraParams["id"].ToInt32());
            strTelefones.DataSource = UsuarioSelecionado.Telefones;
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

            List<EmpresaSetorVO> setores = new EmpresaSetorBO().BuscarSetoresPai(empresa);
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
            node1.CustomAttributes.Add(new ConfigItem("Id", "", ParameterMode.Value));
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
