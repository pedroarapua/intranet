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
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace IntranetBettaInformatica.Web
{
    public partial class MeuPerfil : BasePage
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

        protected void Salvar_Click(object sender, DirectEventArgs e)
        {
            SalvarPerfil(e);
        }

		protected void SalvarConhecimentos_Click(object sender, DirectEventArgs e)
		{
			SalvarConhecimentos(e);
		}

		protected void btnConhecimentos_Click(object sender, DirectEventArgs e)
		{
			CarregarConhecimentos();
			winConhecimentos.Show((Control)sender);
		}

        #endregion

        #region metodos

        /// <summary>
        /// metoco para carregar a página
        /// </summary>
        private void LoadPagina()
        {
            UsuarioLogado = new UsuarioBO().SelectById(UsuarioLogado.Id);
            txtNome.Text = UsuarioLogado.Nome;
            txtEmail.Text = UsuarioLogado.Email;
            txtEndereco.Text = UsuarioLogado.Endereco;
            txtCidade.Text = UsuarioLogado.Cidade;
            txtDataNascimento.Text = UsuarioLogado.DataNascimento != null ? UsuarioLogado.DataNascimento.Value.ToString("dd/MM/yyyy") : String.Empty;
            txtTwitter.Text = UsuarioLogado.Twitter;
			txtTwitter.Disabled = !UsuarioLogado.PerfilAcesso.Acoes.Any(x => x.TipoAcao == Entities.Enumertators.ETipoAcao.VisualizarTwitter);
            lblEmpresa.Text = UsuarioLogado.Empresa != null ? UsuarioLogado.Empresa.Nome : String.Empty;
            lblSetor.Text = UsuarioLogado.Setor != null ? UsuarioLogado.Setor.Nome : "[Nenhum]";

            CarregarEstados();
            CarregarPaginas();
            CarregarTemas();
			CarregarNiveisConhecimento();

            cboTema.SetValue(UsuarioLogado.Tema.Id);
            if (UsuarioLogado.PaginaInicial == null)
            {
                if(cboPaginas.Items.FirstOrDefault(x=> x.Value == "1") != null)
                    cboPaginas.SetValue(1);
            }
            else
            {
                cboPaginas.SetValue(UsuarioLogado.PaginaInicial.Id);
            }

            txtLogin.Text = UsuarioLogado.Login;

            if (UsuarioLogado.Estado != null)
                cboEstado.SetValue(UsuarioLogado.Estado.Id);

            imgAtual.Hidden = UsuarioLogado.ExtensaoFoto.IsNullOrEmpty();
            
            fufImagem.Clear();
            if (!imgAtual.Hidden)
                imgAtual.ImageUrl = UsuarioLogado.CaminhoImagemOriginal;    
        }

        private void SalvarPerfil(DirectEventArgs e)
        {
            try
            {
                String extensao = String.Empty;
                if (!fufImagem.FileName.IsNullOrEmpty())
                    extensao = fufImagem.FileName.Substring(fufImagem.FileName.LastIndexOf("."));
                else if(!imgAtual.ImageUrl.IsNullOrEmpty())
                    extensao = imgAtual.ImageUrl.Substring(imgAtual.ImageUrl.LastIndexOf("."));
                
                if (!extensao.IsNullOrEmpty() && !ValidaExtensaoImagem(extensao))
                {
                    base.MostrarMensagem("Imagem", "Extensão de imagem " + extensao + " não suportada.", "");
                    return;
                }

                UsuarioLogado.Cidade = txtCidade.Text;
                UsuarioLogado.Login = txtLogin.Text;
                UsuarioLogado.Twitter = txtTwitter.Text;
                if(!txtSenha.Text.IsNullOrEmpty())
                    UsuarioLogado.Senha = UsuarioBO.EncriptyPassword(txtSenha.Text);

                if (!txtDataNascimento.Text.IsNullOrEmpty())
                    UsuarioLogado.DataNascimento = Convert.ToDateTime(txtDataNascimento.Text);
                else
                    UsuarioLogado.DataNascimento = null;

                UsuarioLogado.Email = txtEmail.Text;
                UsuarioLogado.Endereco = txtEndereco.Text;
                if (cboEstado.Value != null && !cboEstado.Value.ToString().IsNullOrEmpty())
                    UsuarioLogado.Estado = new EstadoVO() { Id = cboEstado.Value.ToInt32(), Nome = cboEstado.Text };
                else
                    UsuarioLogado.Estado = null;

                UsuarioLogado.Nome = txtNome.Text;
                UsuarioLogado.PaginaInicial = new MenuPaginaBO().SelectById(cboPaginas.Value.ToInt32());
                if (!txtPalavraChave.Text.IsNullOrEmpty())
                    UsuarioLogado.PalavraChave = UsuarioBO.EncriptyPassword(txtPalavraChave.Text);

                UsuarioLogado.Tema = new TemaVO() { Id = cboTema.Value.ToInt32(), Nome = cboTema.Text };
                UsuarioLogado.ExtensaoFoto = extensao;
                
                new UsuarioBO(UsuarioLogado).Salvar();

                if (!fufImagem.FileName.IsNullOrEmpty())
                {
                    Bitmap imgOriginal = System.Drawing.Image.FromStream(fufImagem.FileContent) as Bitmap;

                    Dictionary<Int32, Int32> dicSize = GetSizePhoto(imgOriginal, 150, 150);
                    Int32 width = dicSize.FirstOrDefault().Value;
                    Int32 heigth = dicSize.FirstOrDefault().Key;
                    Bitmap imgModificada = new Bitmap(width, heigth);
                    Graphics g = Graphics.FromImage(imgModificada);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgModificada.Width, imgModificada.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    
                    // Grava imagem redimensionada
                    String pathOriginal = Path.Combine(Server.MapPath("~/FotosUsuarios/FotosOriginais"), UsuarioLogado.Id + UsuarioLogado.ExtensaoFoto);
                    imgModificada.Save(pathOriginal, imgOriginal.RawFormat);

                    imgModificada = new Bitmap(imgOriginal.Width > 150 ? 150 : imgOriginal.Width, imgOriginal.Height > 150 ? 150 : imgOriginal.Height);
                    dicSize = GetSizePhoto(imgOriginal, 70, 70);
                    width = dicSize.FirstOrDefault().Value;
                    heigth = dicSize.FirstOrDefault().Key;
                    imgModificada = new Bitmap(width, heigth);
                    g = Graphics.FromImage(imgModificada);
                    g.DrawImage(imgOriginal, new Rectangle(0, 0, imgModificada.Width, imgModificada.Height), 0, 0, imgOriginal.Width, imgOriginal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                    
                    String pathThumbs = Path.Combine(Server.MapPath("~/FotosUsuarios/FotosThumbs"), UsuarioLogado.Id + UsuarioLogado.ExtensaoFoto);
                    imgModificada.Save(pathThumbs, imgOriginal.RawFormat);
                }

                base.MostrarMensagem("Sucesso", "Perfil atualizado com sucesso.", "MeuPerfil.aspx");
            }
            catch (Exception ex)
            {
                base.MostrarMensagem("Erro", "Erro ao atualizar perfil.","");
            }            
        }

		private void SalvarConhecimentos(DirectEventArgs e)
		{
			try
			{
				List<UsuarioBaseConhecimentoVO> lstBaseConhecimento = JSON.Deserialize<List<UsuarioBaseConhecimentoVO>>(e.ExtraParams["conhecimentos"]);
				foreach (UsuarioBaseConhecimentoVO conhecimento in lstBaseConhecimento)
				{
					conhecimento.Usuario = base.UsuarioLogado;
					new UsuarioBaseConhecimentoBO(conhecimento).Salvar();
				}
				winConhecimentos.Hide();
			}
			catch (Exception ex)
			{
				base.MostrarMensagem("Erro", "Erro ao salvar os conhecimentos", String.Empty);
			}
		}

        private void CarregarPaginas()
        {
			List<MenuPaginaVO> paginas = UsuarioLogado.PerfilAcesso.MenuPaginas.ToList();
            List<MenuPaginaVO> paginasRetorno = new List<MenuPaginaVO>();
            GetPaginas(paginas, paginasRetorno);
            
            foreach (MenuPaginaVO p in paginasRetorno)
            {
                Ext.Net.Icon icon = p.Icone.IsNullOrEmpty() ? Ext.Net.Icon.None : (Ext.Net.Icon)Enum.Parse(typeof(Ext.Net.Icon), p.Icone);
                base.ResourceManager.RegisterIcon(icon);
                p.IconeCls = ResourceManager.GetIconClassName(icon);
            }
            strPaginas.DataSource = paginasRetorno.OrderBy(x=> x.Descricao).ToList();
            strPaginas.DataBind();
        }

        private List<MenuPaginaVO> GetPaginas(List<MenuPaginaVO> paginas, List<MenuPaginaVO> paginasRetorno)
        {
            paginas = paginas.Where(x=> ((x.MenuPaginaPai != null && x.MenuPaginas.Count == 0) || (x.MenuPaginaPai == null && x.EmMenu == true)) ).ToList();
            foreach (MenuPaginaVO p1 in paginas)
            {
                paginasRetorno.Add(p1);
                if (p1.MenuPaginas.Count > 0)
                {
                    GetPaginas(p1.MenuPaginas.ToList(), paginasRetorno);
                }
            }
            return paginasRetorno;
        }

        private void CarregarEstados()
        {
            strEstados.DataSource = new EstadoBO().Select();
            strEstados.DataBind();
        }

        private void CarregarTemas()
        {
            strTemas.DataSource = new TemaBO().Select().Where(x => x.Removido == false || (UsuarioLogado.PaginaInicial != null && x.Id == UsuarioLogado.PaginaInicial.Id)).ToList();
            strTemas.DataBind();
        }

		private void CarregarConhecimentos()
		{
			List<UsuarioBaseConhecimentoVO> lstConhecimentosUsuario = new UsuarioBaseConhecimentoBO().BuscarPorUsuario(UsuarioLogado.Id);
			List<BaseConhecimentoVO> lstConhecimentos = new BaseConhecimentoBO().Select();
			Int32 id = -1;
			foreach (BaseConhecimentoVO conhecimento in lstConhecimentos)
			{
				if (!lstConhecimentosUsuario.Any(x => x.Conhecimento.Id == conhecimento.Id))
				{
					lstConhecimentosUsuario.Add(new UsuarioBaseConhecimentoVO() { Comprovavel = false, Conhecimento = conhecimento, NivelConhecimento = ENivelConhecimento.Nenhum, Id = id });
					id--;
				}
			}

			strConhecimentos.DataSource = lstConhecimentosUsuario.Select(x => new { Id = x.Id, NivelConhecimentoId = x.NivelConhecimento.ToInt32(), NivelConhecimentoDescricao = x.NivelConhecimento.ToText() , Comprovavel = x.Comprovavel, Conhecimento = x.Conhecimento });
			strConhecimentos.DataBind();
		}

		private void CarregarNiveisConhecimento()
		{
			List<Object> lstObject = new List<Object>();
			foreach (var enumerator in Enum.GetValues(typeof(ENivelConhecimento)))
			{
				lstObject.Add(new { Id = enumerator.ToInt32(), Titulo = enumerator.ToString() });
			}
			strNiveisConhecimento.DataSource = lstObject;
			strNiveisConhecimento.DataBind();
		}

        private Boolean ValidaExtensaoImagem(String extensao)
        {
            return base.LstExtensaoImagens.Any(x => x.ToLower() == extensao.ToLower());
        }

        private Dictionary<int, int> GetSizePhoto(System.Drawing.Bitmap img, int nWidth, int nHeight)
        {
            int oWidth = img.Width; // largura original
            int oHeight = img.Height; // altura original
            Dictionary<Int32, Int32> dicSize = new Dictionary<Int32, Int32>();

            // redimensiona se necessario
            if (oWidth > nWidth || oHeight > nHeight)
            {

                if (oWidth > oHeight)
                {
                    // imagem horizontal
                    nHeight = (oHeight * nWidth) / oWidth;
                    nWidth = (oWidth * nHeight) / oHeight;
                }
                else
                {
                    // imagem vertical
                    nWidth = (oWidth * nHeight) / oHeight;
                    nHeight = (oHeight * nWidth) / oWidth;
                }
            }

            dicSize.Add(nHeight, nWidth);
            return dicSize;
        }

        #endregion
    }
}
