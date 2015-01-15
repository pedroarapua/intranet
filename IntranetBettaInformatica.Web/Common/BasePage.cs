using IntranetBettaInformatica.Entities.Entities;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ExcelLibrary.SpreadSheet;
using System.IO;
using System.Reflection;
using IntranetBettaInformatica.Business;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Web.Common
{

    public class BasePage : System.Web.UI.Page
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

        public Boolean MostrouLembretes
        {
            get
            {
                if (this.Session["MostrouLembretes"] == null)
                    return false;
                return (Boolean)this.Session["MostrouLembretes"];
            }
            set
            {
                this.Session["MostrouLembretes"] = value;
            }
        }

        public List<UsuarioVO> Aniversariantes
        {
            get
            {
                if (this.Session["Aniversariantes"] == null)
                    return new List<UsuarioVO>();
                return (List<UsuarioVO>)this.Session["Aniversariantes"];
            }
            set
            {
                this.Session["Aniversariantes"] = value;
            }
        }

		public List<AcaoVO> Acoes
		{
			get
			{
				if (this.Session["Acoes"] == null)
					return new AcaoBO().Select();
				return (List<AcaoVO>)this.Session["Acoes"];
			}
			set
			{
				this.Session["Acoes"] = value;
			}
		}

		public List<ExtensaoArquivoVO> ExtensoesArquivo
		{
			get
			{
				if (this.Session["ExtensoesArquivo"] == null)
					return new ExtensaoArquivoBO().Select();
				return (List<ExtensaoArquivoVO>)this.Session["ExtensoesArquivo"];
			}
			set
			{
				this.Session["ExtensoesArquivo"] = value;
			}
		}

        public ConfiguracoesSistemaVO Configuracao
        {
            get
            {
                if (this.Session["Configuracao"] == null)
					this.Session["Configuracao"] = new ConfiguracoesSistemaBO().SelectById(1);
                return (ConfiguracoesSistemaVO)this.Session["Configuracao"];
            }
            set { this.Session["Configuracao"] = value; }
        }

        public AcaoTela AcaoTela
        {
            get
            {
                if (this.ViewState["AcaoTela"] == null)
                    return AcaoTela.Inclusao;
                return (AcaoTela)this.ViewState["AcaoTela"];
            }
            set { this.ViewState["AcaoTela"] = value; }
        }

        public List<String> LstExtensaoImagens
        {
            get { return this.ExtensoesArquivo.Where(x=> x.TipoExtensao == ETipoExtensao.Fotos).Select(x=> x.Extensao).ToList<String>(); }
        }

        public List<String> LstExtensaoVideos
        {
			get { return this.ExtensoesArquivo.Where(x => x.TipoExtensao == ETipoExtensao.Videos).Select(x => x.Extensao).ToList<String>(); }
        }

		public List<String> LstExtensaoCurriculo
		{
			get { return this.ExtensoesArquivo.Where(x => x.TipoExtensao == ETipoExtensao.Curriculum).Select(x => x.Extensao).ToList<String>(); }
		}

        public Boolean EModerador
        {
            get { return UsuarioLogado.PerfilAcesso.Id == 1; }
        }

        public ResourceManager ResourceManager
        {
            get
            {
                return this.Master.FindControl("ResourceManager1") as ResourceManager;
            }
        }

        #endregion

        #region eventos

        protected override void OnLoad(System.EventArgs e)
        {
            String url = Request.RawUrl;
            if (UsuarioLogado == null && !Request.RawUrl.Contains("Login.aspx") && !Request.RawUrl.Contains("Redirecionar.aspx"))
            {
                //AbrirWindowLogin();
                //this.MostrarMensagem("Sessão expirada", "Sua sessão expirou, por favor efetue o login novamente.", "Login.aspx");
                Response.Redirect("Login.aspx");
                return;
            }
            else if (UsuarioLogado != null)
            {
                if (!Request.RawUrl.Contains("Login.aspx") && !Request.RawUrl.Contains("Redirecionar.aspx") && !UsuarioLogado.PerfilAcesso.MenuPaginas.Any(x => x.Url != null && Request.RawUrl.ToLower().Contains(x.Url.Replace("~/", "").ToLower())))
                {
                    this.MostrarMensagem("Mensagem", "Usuario sem permissão de acesso a esta pagina.", "Login.aspx");
                    return;
                }
				else if (!Request.RawUrl.Contains("Login.aspx") && !Request.RawUrl.Contains("Redirecionar.aspx"))
				{
					SetHiddenAcoesPagina();
					InsereMenuPagina(url);
				}
            }
		    base.OnLoad(e);
                           
        }

        #endregion

        #region metodos

        public void MostrarMensagem(String title, String msg, String url)
        {
            String location = String.Empty;
            if (!url.IsNullOrEmpty())
                location = String.Format("window.location = '{0}';", url);
            X.Msg.Alert(title, msg, location).Show();
        }

		/// <summary>
		/// Metodo que verifica se usuario tem determinada permissão
		/// </summary>
		/// <param name="acao"></param>
		/// <returns></returns>
		public Boolean ContemPermissao(ETipoAcao acao)
		{
			return UsuarioLogado.PerfilAcesso.Acoes.Any(x => x.TipoAcao == acao);
		}

        /// <summary>
        ///	Método que exporta o resultado de um List para Excel, com opção de download ou visualização no browser.
        /// </summary>
        /// <param name="list"> Resultado de Dados que será exportado para Excel. </param>
        /// <param name="titulo">Titulo da paleta no excel.</param>
        /// <param name="strNomeArquivo">Nome que será atribuído ao arquivo gerado, com extensão. Ex: nomeDoArquivo.xls. </param>
        /// <param name="propriedadesExibir">Nome das Propriedade das VO. </param>
        /// <param name="header">Titulo do cabeçalho do excel. </param>
        public void ExportaExcel<T>(System.Collections.Generic.List<T> list, string titulo, string strNomeArquivo, string[] propriedadesExibir, string[] header)
        {
            if (list.Count == 0)
            {
                this.MostrarMensagem("Alerta", "Não existem informações para serem exportadas.", String.Empty);
                return;
            }

            RemoverArquivosExistentes();

            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet(titulo);
            
            String path = Path.Combine(Server.MapPath("~/ConfiguracoesSistema"), "1.jpg");
            FileStream stream = null;
            if(File.Exists(path))
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                Picture pic = new Picture();
                System.Drawing.Image imgLogo = new System.Drawing.Bitmap(stream);
                pic.Image = new ExcelLibrary.SpreadSheet.Image(imageToByteArray(imgLogo), 0xF01D);
                //Tamanho da imagem.
                pic.TopLeftCorner = new CellAnchor(0, 0, 10, 10);
                pic.BottomRightCorner = new CellAnchor(4, 2, 10, 10);
                worksheet.AddPicture(pic);
            }

            worksheet.Cells[2, 3] = new ExcelLibrary.SpreadSheet.Cell(titulo);
            worksheet.Cells[2, 7] = new ExcelLibrary.SpreadSheet.Cell(DateTime.Now.ToString("dd/MM/yyyy"));
            // Adiciona a imagem(logo).
            /// Adiciona o worksheet no workbook
            workbook.Worksheets.Add(worksheet);

            Object obj = list[0];
            int index = 0;
            if (header.Length == 0)
            {
                foreach (PropertyInfo propert in obj.GetType().GetProperties())
                {
                    header[index] = propert.Name;
                    index++;
                }
            }

            index = 0;
            foreach (string strHeader in header)
            {
                worksheet.Cells[6, index] = new ExcelLibrary.SpreadSheet.Cell(strHeader);
                index++;
            }

            index = 0;
            int indexLinha = 7;

            foreach (var obj1 in list)
            {
                foreach (String p in propriedadesExibir)
                {
                    object objValor = obj1.GetPropriedadeValor(p);
                    worksheet.Cells[indexLinha, index] = new ExcelLibrary.SpreadSheet.Cell(objValor != null ? objValor.ToString() : "");
                    index++;
                }
                index = 0;
                indexLinha++;
            }

            // Nome do arquivo a ser gerado            
            String nomeAux = Guid.NewGuid().ToString() + "_"+ UsuarioLogado.Id.ToString();
            String caminho = Path.Combine(Path.GetTempPath(), nomeAux + ".xls");
            workbook.Save(caminho);
            
            if(stream != null)
                stream.Close();
            
            FileInfo arquivo = new FileInfo(caminho);
            if (arquivo.Exists)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + strNomeArquivo);
                Response.AddHeader("Content-Length", arquivo.Length.ToString());
                Response.Flush();
                Response.WriteFile(caminho);
            }
        }

        public void RemoverArquivosExistentes()
        {
            foreach (FileInfo f in new DirectoryInfo(Server.MapPath("~/temp")).GetFiles())
            {
                if ((f.Extension.Contains("csv") || f.Extension.Contains("xls")) && f.Name.Contains(UsuarioLogado.Id.ToString()))
                {
                    f.Delete();
                }
            }
        }

        /// <summary>
        /// Métodos que cria um array de imagem(logo da empresa).
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private void AbrirWindowLogin()
        {
            Window winLogin = this.Master.FindControl("winLoginMaster") as Window;
            TextField txtLogin = this.Master.FindControl("txtLoginMaster") as TextField;
            TextField txtSenha = this.Master.FindControl("txtSenhaMaster") as TextField;
            winLogin.Show();
            if (Request.Cookies["login"] == null)
                txtLogin.Focus(false, 50);
            else
            {
                txtLogin.Text = Request.Cookies["login"].Value;
                txtSenha.Focus(false, 50);
            }
        }

		private void SetHiddenAcoesPagina()
		{
			foreach (AcaoVO acao in Acoes)
			{
				Component control = X.GetCmp(String.Format("hdf{0}", acao.TipoAcao.ToString()));
				if (control != null)
				{
					((Hidden)control).Value = UsuarioLogado.PerfilAcesso.Acoes.Any(x => x.TipoAcao == acao.TipoAcao) ? "1" : "0";
				}

			}
		}
        private void InsereMenuPagina(String url)
        {
            String paginaStr = url.Substring(url.LastIndexOf("/") + 1);
			MenuPaginaVO pagina = UsuarioLogado.PerfilAcesso.MenuPaginas.FirstOrDefault(x => !String.IsNullOrEmpty(x.Url) && x.Url.ToUpper() == paginaStr.ToUpper());
            if(pagina != null)
            {
                MenuPaginaUsuarioVO menu = new MenuPaginaUsuarioVO()
                {
                    MenuPagina = pagina,
                    Usuario = UsuarioLogado
                };
                new MenuPaginaUsuarioBO().Inserir(menu);
            }
        }

        public void SetTituloIconePagina(FormPanel frmTitulo)
        {
            if (frmTitulo != null)
            {
                String url = Request.RawUrl.Substring(Request.RawUrl.LastIndexOf("/") + 1);
				MenuPaginaVO pagina = UsuarioLogado.PerfilAcesso.MenuPaginas.FirstOrDefault(x => !x.Url.IsNullOrEmpty() && x.Url.ToUpper().Contains(url.ToUpper()));
                if (pagina != null)
                {
                    frmTitulo.Title = pagina.Descricao;
                    frmTitulo.Icon = pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone);
                }
            }
        }
        [DirectMethod]
        public void Login_Click()
        {
            TextField txtLogin = this.Master.FindControl("txtLoginMaster") as TextField;
            TextField txtSenha = this.Master.FindControl("txtSenhaMaster") as TextField;
            UsuarioLogado = new UsuarioBO().Login(txtLogin.Text, txtSenha.Text);
            if (UsuarioLogado != null)
            {
                HttpCookie cookie = new HttpCookie("login", UsuarioLogado.Login);
                cookie.Expires = DateTime.Now.AddDays(7);
                if (Request.Cookies["login"] == null)
                    Response.Cookies.Add(cookie);
                else
                    Response.SetCookie(cookie);
            }
            else
            {
                this.MostrarMensagem("Erro", "Login ou senha inválidos.", String.Empty);
            }
        }

        #endregion
    }
    public enum AcaoTela
    {
        Inclusao = 0,
        Edicao = 1
    }
}