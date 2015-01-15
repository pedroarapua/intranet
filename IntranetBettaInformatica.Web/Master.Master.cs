using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ext.Net;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;
using System.Collections.Generic;
using IntranetBettaInformatica.Entities.Util;
using IntranetBettaInformatica.Entities;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.Web
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public partial class Master : System.Web.UI.MasterPage
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

        public List<UsuarioVO> Aniversariantes
        {
            get
            {
                if (this.Session["Aniversariantes"] == null)
                    return null;
                return (List<UsuarioVO>)this.Session["Aniversariantes"];
            }
            set
            {
                this.Session["Aniversariantes"] = value;
            }
        }

        //public List<NoticiaVO> Noticias
        //{
        //    get
        //    {
        //        if (this.Session["Noticias"] == null)
        //            return null;
        //        return (List<NoticiaVO>)this.Session["Noticias"];
        //    }
        //    set
        //    {
        //        this.Session["Noticias"] = value;
        //    }
        //}

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

        public Boolean PerfilCollapsed
        {
            get
            {
                if (this.Session["PerfilCollapsed"] == null)
                    return false;
                return (Boolean)this.Session["PerfilCollapsed"];
            }
            set
            {
                this.Session["PerfilCollapsed"] = value;
            }
        }

        #endregion

        #region eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMasterPage();
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// Metodo chamado para alterar o tema do usuario
        /// </summary>
        /// <param name="nomeTema"></param>
        /// <param name="idTema"></param>
        [DirectMethod]
        public void AlterarTema(String nomeTema, String idTema)
        {
            Theme myTheme = (Theme)Enum.Parse(typeof(Theme), nomeTema);
            UsuarioLogado.Tema = new TemaVO() { Id = idTema.ToInt32(), Nome = nomeTema };
            new UsuarioBO(UsuarioLogado).Salvar();
            ResourceManager1.SetTheme(myTheme);
        }

        /// <summary>
        /// Metodo utilizado para redirecionar o usuario para o projeto que ele clicou
        /// </summary>
        /// <param name="nomeTema"></param>
        /// <param name="idTema"></param>
        [DirectMethod]
        public void RedirecionarProjeto(String url)
        {
            url = url.Length > 7 && url.Substring(0, 7).ToUpper() != "HTTP://" ? ("http://" + url) : url;
            Response.Redirect(String.Format("Redirecionar.aspx?url={0}", url));
        }

		/// <summary>
		/// Metodo que abre o gráfico organizacional da empresa
		/// </summary>
		/// <param name="empresa"></param>
		[DirectMethod]
		public void OpenChartOrganization(String empresa)
		{
			winChartOrganizacao.AutoLoad.Url = String.Format("ChartOrganizacao.aspx?empresa={0}&date={1}", empresa, DateTime.Today.ToString());
			winChartOrganizacao.Render();
			winChartOrganizacao.Show();
		}

		/// <summary>
		/// Metodo que abre o perfil do usuario
		/// </summary>
		/// <param name="empresa"></param>
		[DirectMethod]
		public void OpenPerfilUsuario(String id)
		{
			winPerfilUsuario.AutoLoad.Url = String.Format("VisualizarUsuario.aspx?id={0}&date={1}", id, DateTime.Today.ToString());
			winPerfilUsuario.Render();
			winPerfilUsuario.Show();
		}

		/// <summary>
		/// Metodo que abre a notícia
		/// </summary>
		/// <param name="empresa"></param>
		[DirectMethod]
		public void OpenNoticia(String noticia)
		{
			winHtmlNoticia.Html = new NoticiaBO().SelectById(noticia.ToInt32()).HTML;
			winHtmlNoticia.Show();
		}

        [DirectMethod]
        public void RegistrarPonto(Boolean entrada)
        {
            PontoUsuarioVO pontoU = null;
            DateTime dataNow = DateTime.Now;
            if (UsuarioLogado.PontosUsuario.Count == 0 || UsuarioLogado.PontosUsuario[UsuarioLogado.PontosUsuario.Count - 1].HoraTermino.HasValue)
            {
                pontoU = new PontoUsuarioVO();
                pontoU.HoraInicio = dataNow.AddSeconds(-dataNow.Second);
                pontoU.HoraTermino = null;
            }
            else
            {
                pontoU = UsuarioLogado.PontosUsuario[UsuarioLogado.PontosUsuario.Count - 1];
                pontoU.HoraTermino = dataNow.AddSeconds(-dataNow.Second);
                if (pontoU.HoraTermino.Value.Date < pontoU.HoraInicio.Date)
                {
                    this.MostrarMensagem("Erro", "Não é possível dar saída, pois a data de saída é superior a de entrada. Entre em contato com algum administrador.", String.Empty);
                    return;
                }
            }
            
            DateTime dataMatu = DateTime.Today.AddHours(12);
            DateTime dataVesp = DateTime.Today.AddHours(18);
            PeriodoVO p = new PeriodoVO() { Id = pontoU.HoraInicio <= dataMatu ? 1 : pontoU.HoraInicio <= dataVesp ? 2 : 3 };
            pontoU.Periodo = p;
            pontoU.Data = DateTime.Today;
            pontoU.Usuario = UsuarioLogado;
            pontoU.Removido = false;

            new PontoUsuarioBO(pontoU).Salvar();
            UsuarioLogado.PontosUsuario = new PontoUsuarioBO().BuscarPontosDoDia(UsuarioLogado);
            CarregarInformacoesPonto();
            this.MostrarMensagem("Sucesso", "Ponto registrado com sucesso.", this.Page.Request.RawUrl);
        }

        
        /// <summary>
        /// metodo chamado para carregar as informacoes da master page
        /// </summary>
        private void LoadMasterPage()
        {
            if (UsuarioLogado != null)
            {
                AdicionaStylesSistemas();
                Ext.Net.ResourceManager.GetInstance(HttpContext.Current).SetTheme((Theme)Enum.Parse(typeof(Theme), UsuarioLogado.Tema.Nome));
                CarregarTemas();
                CarregarLogo();
                CarregarMenu();
                CarregarProjetos();
                CarregarFavoritos();
                CarregarInformacoesUsuario();
				if(this.ContemPermissao(ETipoAcao.RegistrarPontosUsuarios))
					CarregarInformacoesPonto();
                AbrirNotificacoes();
                CarregarBotoesPerfil();
                pnlPerfilCollapse.Collapsed = PerfilCollapsed;
            }
        }

        [DirectMethod]
        public void Collapse_Click(Boolean collapse)
        {
            PerfilCollapsed = collapse;
        }

        /// <summary>
        /// metodo chamado para carregar os temas existentes
        /// </summary>
        private void CarregarTemas()
        {
            List<TemaVO> lstTemas = new TemaBO().Select();
            btnTemas.Visible = lstTemas.Count > 0;
            if (lstTemas.Count > 0)
            {
                Ext.Net.Menu menu = new Ext.Net.Menu();
                btnTemas.Menu.Add(menu);
                foreach (TemaVO t in lstTemas)
                {
                    Ext.Net.MenuItem menuItem = new Ext.Net.MenuItem(t.Nome);
                    menuItem.Listeners.Click.Handler = "Ext.net.DirectMethods.AlterarTema('" + t.Nome + "', '" + t.Id + "');";
                    btnTemas.Menu.Primary.Items.Add(menuItem);
                }
            }
        }

        /// <summary>
        /// Carrega o logo a ser utilizado na master page
        /// </summary>
        private void CarregarLogo()
        {
            ConfiguracoesSistemaVO conf = new ConfiguracoesSistemaBO().SelectById(1);
            imgLogo.Hidden = conf.CaminhoImagem.IsNullOrEmpty();
            imgLogo.ImageUrl = conf.CaminhoImagem;
        }

        /// <summary>
        /// Carrega o menu do projeto
        /// </summary>
        private void CarregarMenu()
        {
			List<MenuPaginaVO> lstMenuPai = new List<MenuPaginaVO>();
			SetMenuPaginaPai(lstMenuPai, null);
			lstMenuPai = lstMenuPai.OrderByDescending(x=> x.Ordem).Distinct(new KeyEqualityComparer<MenuPaginaVO>(x => x.Id)).ToList();
			foreach (MenuPaginaVO mp in lstMenuPai)
            {
                if (!mp.EmMenu)
                    continue;
                if (mp.MenuPaginas.Count == 0)
                {
                    Ext.Net.Button btn = new Ext.Net.Button();
                    btn.Text = mp.Descricao;
                    btn.Icon = mp.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), mp.Icone);
                    if (!String.IsNullOrEmpty(mp.Url))
                        btn.Listeners.Click.Handler = "window.location = '" + mp.Url + "';";
                    toolbarMenu.Items.Insert(0, btn);
                }
                else
                {
                    Ext.Net.SplitButton btn = new Ext.Net.SplitButton();
                    btn.Text = mp.Descricao;
					if (mp.MenuPaginas.Count > 0)
                    {
                        Ext.Net.Menu menu = new Ext.Net.Menu();
						CarregarSubMenu(menu, mp.MenuPaginas);
                        btn.Menu.Add(menu);
                    }
                    btn.Icon = mp.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), mp.Icone);
                    if (!String.IsNullOrEmpty(mp.Url))
                        btn.Listeners.Click.Handler = "window.location = '" + mp.Url + "';";
                    toolbarMenu.Items.Insert(0, btn);
                }
                AdicionarIcones(mp);
            }
        }

		private void SetMenuPaginaPai(List<MenuPaginaVO> lstMenuPagina, MenuPaginaVO menuPaginaPai)
		{
			if (menuPaginaPai == null)
			{
				foreach (MenuPaginaVO menuPagina in UsuarioLogado.PerfilAcesso.MenuPaginas)
				{
					if (menuPagina.MenuPaginaPai != null) 
					{
						SetMenuPaginaPai(lstMenuPagina, menuPagina.MenuPaginaPai);
					}
					else if (menuPagina.Url.Contains("Default.aspx"))// Pagina Home também é considerado um menu
					{
						lstMenuPagina.Add(menuPagina);
					}
				}
			}
			else
			{
				if (menuPaginaPai.MenuPaginaPai != null)
				{
					SetMenuPaginaPai(lstMenuPagina, menuPaginaPai.MenuPaginaPai);
				}
				else
				{
					lstMenuPagina.Add(menuPaginaPai);
				}
			}
		}

        private void CarregarSubMenu(Ext.Net.Menu menu, IList<MenuPaginaVO> sub)
        {
            foreach (MenuPaginaVO mp in sub.OrderBy(x => x.Ordem))
            {
				if (!mp.EmMenu || (!String.IsNullOrEmpty(mp.Url) && !UsuarioLogado.PerfilAcesso.MenuPaginas.Any(x => x.Id == mp.Id)) || (String.IsNullOrEmpty(mp.Url) && !UsuarioLogado.PerfilAcesso.MenuPaginas.Any(x=> mp.MenuPaginas.Any(x1=> x1.Id == x.Id))))
                    continue;
                Ext.Net.MenuItem item = new Ext.Net.MenuItem(mp.Descricao);
                if (!String.IsNullOrEmpty(mp.Url))
                    item.Listeners.Click.Handler = "window.location = '" + mp.Url + "';";
                item.Icon = mp.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), mp.Icone);
                if (mp.MenuPaginas.Count > 0)
                {
                    Ext.Net.Menu menu1 = new Ext.Net.Menu();
                    CarregarSubMenu(menu1, mp.MenuPaginas);
                    item.Menu.Add(menu1);
                }

                menu.Items.Add(item);
                AdicionarIcones(mp);
            }

			//if (UsuarioLogado.PerfilAcesso.Acoes.Any(x => x.TipoAcao == Entities.Enumertators.ETipoAcao.RegistrarPontosUsuarios))
			//{
			//    Ext.Net.MenuItem item = new Ext.Net.MenuItem("Registrar Ponto");
			//    item.Icon = Ext.Net.Icon.ClockStart;
			//    menu.Items.Add(item);
			//    CarregarMenuPonto(item);
			//}
        }

        /// <summary>
        /// Carrega os projetos que o usuario tem permissao
        /// </summary>
        private void CarregarProjetos()
        {
            if (UsuarioLogado.Sistemas.ToList().Count > 0)
            {
                TableLayout tableProjetos = new TableLayout();
                tableProjetos.Columns = UsuarioLogado.Sistemas.Count;
                foreach(SistemaVO s in UsuarioLogado.Sistemas)
                {
                    Cell cell = new Cell();
                    Ext.Net.Button btn = new Ext.Net.Button(s.Nome);
                    btn.EnableToggle = true;
                    btn.IconAlign = IconAlign.Top;
                    btn.Scale = ButtonScale.Large;
                    btn.IconCls = "custom_"+s.Id;
                    btn.AutoPostBack = true;
                    btn.AutoWidth = true;
                    btn.Listeners.Click.Handler = "Ext.net.DirectMethods.RedirecionarProjeto('"+ s.Url + "');";
                    cell.Items.Add(btn);
                    tableProjetos.Cells.Add(cell);
                }
                pnlProjetos.Items.Add(tableProjetos);
            }
        }

        /// <summary>
        /// Carrega os favoritos do usuario
        /// </summary>
        private void CarregarFavoritos()
        {
            if (UsuarioLogado.Paginas.Count > 0)
            {
                UsuarioLogado.Paginas.ToList().ForEach(x=> x.QtdAcessos = UsuarioLogado.Paginas.Count(x1=> x1.MenuPagina.Id == x.MenuPagina.Id));
                UsuarioLogado.Paginas = UsuarioLogado.Paginas.OrderByDescending(x => x.QtdAcessos).ToList();
                UsuarioLogado.Paginas = UsuarioLogado.Paginas.Distinct(new KeyEqualityComparer<MenuPaginaUsuarioVO>(x => x.MenuPagina.Id)).ToList();
                UsuarioLogado.Paginas = UsuarioLogado.Paginas.Take(6).ToList();
                List<MenuPaginaVO> paginas = UsuarioLogado.Paginas.Select(x => x.MenuPagina).ToList();
                TableLayout tableFavoritos = new TableLayout();
                tableFavoritos.Columns = paginas.Count;
                foreach (MenuPaginaVO p in paginas)
                {
                    Cell cell = new Cell();
                    Ext.Net.Button btn = new Ext.Net.Button(p.Descricao);
                    btn.Height = 30;
                    btn.MinWidth = 80;
                    btn.Icon = p.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), p.Icone);
                    btn.Listeners.Click.Handler = "window.location = '" + p.Url + "';";
                    cell.Items.Add(btn);
                    tableFavoritos.Cells.Add(cell);
                }
                btnGroupFavoritos.Items.Add(tableFavoritos);
                btnGroupFavoritos.Visible = true;
            }
            else
                btnGroupFavoritos.Visible = false;
        }

        /// <summary>
        /// metodo que mostra notificações do dia
        /// </summary>
        private void AbrirNotificacoes()
        {
			if(this.ContemPermissao(ETipoAcao.VisualizarAniversariantes)) // Somente será mostrado se tiver permissão de visualizar aniversariantes
				AbrirNotificacoesAniversariantes();
			if (this.ContemPermissao(ETipoAcao.VisualizarNoticiasColaborador)) // Somente será mostrado se tiver permissão de visualizar noticias
				AbrirNotificacoesNoticias();
			if (this.ContemPermissao(ETipoAcao.VisualizarMensagens)) // Somente será mostrado se tiver permissão de visualizar mensagens
				AbrirNotificacoesMensagens();
			if (this.ContemPermissao(ETipoAcao.VisualizarResponderPesquisas)) // Somente será mostrado se tiver permissão de visualizar responder pesquisas
				AbrirNotificacoesPesquisas();
			if (this.ContemPermissao(ETipoAcao.VisualizarReunioes)) // Somente será mostrado se tiver permissão de visualizar reuniões
				AbrirNotificacoesReunioes();
            MostrouLembretes = true;
        }

        /// <summary>
        /// metodo que mostra as notificações dos aniversariantes
        /// </summary>
        private void AbrirNotificacoesAniversariantes()
        {
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "VISUALIZARANIVERSARIANTES.ASPX");
            if (!MostrouLembretes && Aniversariantes == null)
                Aniversariantes = new UsuarioBO().BuscarAniversariantes(UsuarioLogado);
            if (!MostrouLembretes && Aniversariantes.Count != 0 && pagina != null)
            {
                String plural = Aniversariantes.Count > 1 ? "estão" : "está";
                Notification.Show(new NotificationConfig
                {
                    ID = "ntfAniversariantes",
                    Title = "Aniversariantes",
                    Icon = pagina == null || pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone),
                    AutoHide = false,
                    Html = String.Format("<br/>{0} {1} fazendo aniversário.", String.Join(", ", Aniversariantes.Select(x=> x.Nome).ToArray()), plural)
                });
                String script = "var ntfAniversariantes = document.getElementById(\"ntfAniversariantes\"); ntfAniversariantes.style.cursor = \"pointer\"; ntfAniversariantes.onclick = function(){ window.location =  '" + pagina.Url + "'; };";
                this.ResourceManager1.RegisterOnReadyScript(script);
            }
        }

        /// <summary>
        /// metodo que mostra as notificações dos aniversariantes
        /// </summary>
        private void AbrirNotificacoesMensagens()
        {
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "GERENCIARMENSAGENS.ASPX");
            UsuarioLogado.MensagensRecebidasNaoLidas = UsuarioLogado.MensagensRecebidasNaoLidas.Where(x => x.Mensagem.Removido == false).ToList();
            if (!MostrouLembretes && UsuarioLogado.MensagensRecebidasNaoLidas.Count != 0 && pagina != null)
            {
                Notification.Show(new NotificationConfig
                {
                    ID = "ntfMensagens",
                    Title = "Mensagens",
                    Icon = pagina == null || pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone),
                    AutoHide = false,
                    Html = String.Format("<br/>Você possui {0} mensagens não lida(s).", UsuarioLogado.MensagensRecebidasNaoLidas.Count)
                });

                String script = "var ntfMensagens = document.getElementById(\"ntfMensagens\"); ntfMensagens.style.cursor = \"pointer\"; ntfMensagens.onclick = function(){ window.location =  '" + pagina.Url + "'; };";
                this.ResourceManager1.RegisterOnReadyScript(script);
            }
        }

        /// <summary>
        /// metodo que mostra as notificações de notícias
        /// </summary>
        private void AbrirNotificacoesNoticias()
        {
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "VISUALIZARNOTICIAS.ASPX");
            if (!MostrouLembretes && UsuarioLogado.Noticias.Count == 0)
                UsuarioLogado.Noticias = new NoticiaBO().Buscar(UsuarioLogado, true, true, true, null, null);

            List<NoticiaVO> lstNoticiasAtivas = new NoticiaBO().GetNoticias(UsuarioLogado.Noticias.ToList(), false, true,
                                                                            false);
            if (!MostrouLembretes && lstNoticiasAtivas.Count != 0 && pagina != null)
            {
                Notification.Show(new NotificationConfig
                {
                    ID = "ntfNoticias",
                    Title = "Notícias",
                    Icon = pagina == null || pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone),
                    AutoHide = false,
                    Html = String.Format("<br/>{0} notícia(s).", lstNoticiasAtivas.Count)
                });
                String script = "var ntfNoticias = document.getElementById(\"ntfNoticias\"); ntfNoticias.style.cursor = \"pointer\"; ntfNoticias.onclick = function(){ window.location =  '" + pagina.Url + "'; };";
                this.ResourceManager1.RegisterOnReadyScript(script);
            }
        }

		/// <summary>
		/// metodo que mostra as notificações de reuniões
		/// </summary>
		private void AbrirNotificacoesReunioes()
		{
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "GERENCIARREUNIOES.ASPX");
			if (!MostrouLembretes && UsuarioLogado.Reunioes.Count == 0)
				UsuarioLogado.Reunioes = new ReuniaoBO().BuscarReunioesParaDia(UsuarioLogado);

			if (!MostrouLembretes && UsuarioLogado.Reunioes.Count != 0 && pagina != null)
			{
				Notification.Show(new NotificationConfig
				{
					ID = "ntfReunioes",
					Title = "Reuniões",
					Icon = pagina == null || pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone),
					AutoHide = false,
					Html = String.Format("<br/>{0} reuni{0}.", UsuarioLogado.Reunioes.Count, UsuarioLogado.Reunioes.Count > 1 ? "ões" : "ão")
				});
				String script = "var ntfNoticias = document.getElementById(\"ntfNoticias\"); ntfNoticias.style.cursor = \"pointer\"; ntfNoticias.onclick = function(){ window.location =  '" + pagina.Url + "'; };";
				this.ResourceManager1.RegisterOnReadyScript(script);
			}
		}

        /// <summary>
        /// metodo que mostra as notificações de pesquisas
        /// </summary>
        private void AbrirNotificacoesPesquisas()
        {
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "RESPONDERPESQUISASOPINIAO.ASPX");
            if (!MostrouLembretes && UsuarioLogado.Pesquisas.Count == 0)
                UsuarioLogado.Pesquisas = new PesquisaOpiniaoBO().BuscarPorUsuario(UsuarioLogado, true, true, true, null, null);

            List<PesquisaOpiniaoVO> lstPesquisasAtivas = new PesquisaOpiniaoBO().GetPesquisas(UsuarioLogado.Pesquisas.ToList(), false, true,
                                                                            false);
            if (!MostrouLembretes && lstPesquisasAtivas.Count != 0 && pagina != null)
            {
                Notification.Show(new NotificationConfig
                {
                    ID = "ntfPesquisas",
                    Title = "Pesquisas",
                    Icon = pagina == null || pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone),
                    AutoHide = false,
                    Html = String.Format("<br/>{0} pesquisa(s) à responder.", lstPesquisasAtivas.Count)
                });
                String script = "var ntfPesquisas = document.getElementById(\"ntfPesquisas\"); ntfPesquisas.style.cursor = \"pointer\"; ntfPesquisas.onclick = function(){ window.location =  '" + pagina.Url + "'; };";
                this.ResourceManager1.RegisterOnReadyScript(script);
            }
        }

        private void CarregarInformacoesUsuario()
        {
            imgPerfil.ImageUrl = UsuarioLogado.CaminhoImagemOriginal;
            btnUsuarioLogado.Visible = true;
            btnUsuarioLogado.Text = UsuarioLogado.Nome + " [" + UsuarioLogado.PerfilAcesso.Nome + "]";
        }

        private void AdicionaStylesSistemas()
        {
            foreach(SistemaVO s in UsuarioLogado.Sistemas)
            {
                ResourceManager1.RegisterClientStyleBlock(".custom_"+s.Id, ".custom_"+s.Id+"{ background-image:url(Sistemas/"+s.Id+s.ExtensaoImagem+") !important;height: 46px; width:auto; }");
            }
        }

        private void CarregarInformacoesPonto()
        {
            List<PontoUsuarioVO> pontosDia = UsuarioLogado.PontosUsuario.ToList();
			btnPonto.Visible = this.ContemPermissao(ETipoAcao.RegistrarPontosUsuarios);
            Boolean entrada = pontosDia.Count == 0 || pontosDia[pontosDia.Count - 1].HoraTermino.HasValue;
            btnPonto.Icon = entrada ? Icon.ClockStart : Icon.ClockStop;
            btnPonto.Text = entrada ? "Registrar Ponto (Entrada)" : "Registrar Ponto (Saída)";
            String msg = entrada ? "Deseja registrar entrada?" : "Deseja registrar saída?";
            btnPonto.Listeners.Click.Handler = "return confirm('" + btnPonto.Text + "','" + msg + "', function(){Ext.net.DirectMethods.RegistrarPonto(" + (entrada ? 1 : 0) + ");});";
        }

		//private void CarregarMenuPonto(Ext.Net.MenuItem item)
		//{
		//    List<PontoUsuarioVO> pontosDia = UsuarioLogado.PontosUsuario.ToList();
		//    Boolean entrada = pontosDia.Count == 0 || pontosDia[pontosDia.Count - 1].HoraTermino.HasValue;
		//    item.Icon = entrada ? Icon.ClockStart : Icon.ClockStop;
		//    item.Text = entrada ? "Registrar Ponto (Entrada)" : "Registrar Ponto (Saída)";
		//    String msg = entrada ? "Deseja registrar entrada?" : "Deseja registrar saída?";
		//    item.Listeners.Click.Handler = "return confirm('" + item.Text + "','" + msg + "',function(){Ext.net.DirectMethods.RegistrarPonto(" + (entrada ? 1 : 0) + ");});";
		//}

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

        private void AdicionarIcones(MenuPaginaVO pagina)
        {
            Boolean eAniversariantes = !String.IsNullOrEmpty(pagina.Url) ? pagina.Url.ToUpper().Contains("VISUALIZARANIVERSARIANTES.ASPX") : false;
            Boolean eNoticias = !String.IsNullOrEmpty(pagina.Url) ? pagina.Url.ToUpper().Contains("VISUALIZARNOTICIAS.ASPX") : false;
            if (eAniversariantes || eNoticias)
                this.ResourceManager1.RegisterIcon(pagina.Icone.IsNullOrEmpty() ? Icon.None : (Icon)Enum.Parse(typeof(Icon), pagina.Icone));
        }

        private  MenuPaginaVO GetMenuPagina(List<MenuPaginaVO> lst, String url)
        {
            foreach (MenuPaginaVO p1 in lst)
            {
                if (!String.IsNullOrEmpty(p1.Url) && p1.Url.ToUpper().Contains(url))
                    return p1;
                else if (p1.MenuPaginas.Count > 0)
                    GetMenuPagina(p1.MenuPaginas.ToList(), url);
            }
            return null;
        }

        private void CarregarBotoesPerfil()
        {
			MenuPaginaVO pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "GERENCIARMENSAGENS.ASPX");

            if(pagina != null)
            {
                btnMensagensPerfil.Text += String.Format(" - {0}", UsuarioLogado.MensagensRecebidas.Count);
                btnMensagensPerfil.ToolTips[0].Html = UsuarioLogado.MensagensRecebidasNaoLidas.Count.ToString();
                btnMensagensPerfil.Listeners.Click.Handler = "window.location = '" + pagina.Url + "'; ";
            }
            else
            {
                btnMensagensPerfil.Visible = false;
            }

			pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "VISUALIZARNOTICIAS.ASPX");

            if(pagina != null)
            {
                List<NoticiaVO> lstNoticiasAtivas = new NoticiaBO().GetNoticias(UsuarioLogado.Noticias.ToList(), false,
                                                                                true,
                                                                                false);
                btnNoticiasPerfil.Text += String.Format(" - {0}", UsuarioLogado.Noticias.Count);
                btnNoticiasPerfil.ToolTips[0].Html = lstNoticiasAtivas.Count.ToString();
                btnNoticiasPerfil.Listeners.Click.Handler = "window.location = '"+ pagina.Url+"'; ";
            }
            else
            {
                btnNoticiasPerfil.Visible = false;
            }

			pagina = GetMenuPagina(UsuarioLogado.PerfilAcesso.MenuPaginas.ToList(), "RESPONDERPESQUISASOPINIAO.ASPX");

            if (pagina != null)
            {
                List<PesquisaOpiniaoVO> lstPesquisas =
                    new PesquisaOpiniaoBO().GetPesquisas(UsuarioLogado.Pesquisas.ToList(), false, true,
                                                         false);
                btnPesquisasPerfil.Text += String.Format(" - {0}", UsuarioLogado.Pesquisas.Count);
                btnPesquisasPerfil.ToolTips[0].Html = lstPesquisas.Count.ToString();
                btnPesquisasPerfil.Listeners.Click.Handler = "window.location = '" + pagina.Url + "'; ";
            }
            else
            {
                btnPesquisasPerfil.Visible = false;
            }
        }

      
        #endregion
    }
}
