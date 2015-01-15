using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade MenuPaginaVO
    /// </summary>
    public static class MenuPaginaCarga
    {
        #region atributos

        private static List<MenuPaginaVO> lista = new List<MenuPaginaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade MenuPaginaVO
        /// </summary>
        public static List<MenuPaginaVO> Lista
        {
            get
            {
                return lista;
            }
            set
            {
                lista = value;
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// Carrega a lista
        /// </summary>
        private static void PreencherLista()
        {

            #region paginas fora do menu e sem pagina pai

            MenuPaginaVO menuPagina = new MenuPaginaVO()
            {
                //Id = 1,
                Descricao = "Home",
                EmMenu = true,
                MenuPaginaPai = null,
                Ordem = 0,
                Icone = Ext.Net.Icon.House.ToString(),
                Url = "Default.aspx"
            };
            Lista.Add(menuPagina);

            #endregion

            #region Menus

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 2,
                Descricao = "Administração",
                EmMenu = true,
                MenuPaginaPai = null,
                Ordem = 1
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 3,
                Descricao = "Empresa",
                EmMenu = true,
                MenuPaginaPai = null,
                Ordem = 3
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 4,
                Descricao = "Colaborador",
                EmMenu = true,
                MenuPaginaPai = null,
                Ordem = 2
            };
            Lista.Add(menuPagina);

            #endregion

            #region SubMenus

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 5,
                Descricao = "Controle de Acesso",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 1
            };
            Lista.Add(menuPagina);

            #endregion

            #region paginas no menu com pagina pai

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 6,
                Descricao = "Temas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 3,
                Icone= Ext.Net.Icon.Theme.ToString(),
                Url = "GerenciarThemas.aspx"
            };

            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 7,
                Descricao = "Sistemas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 4,
                Icone = Ext.Net.Icon.Package.ToString(),
                Url = "GerenciarSistemas.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 8,
                Descricao = "Perfis de Acesso",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 5 },
                Ordem = 1,
                Icone = Ext.Net.Icon.GroupKey.ToString(),
                Url = "GerenciarPerfisAcesso.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 9,
                Descricao = "Paginas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 5 },
                Ordem = 2,
                Icone = Ext.Net.Icon.WorldLink.ToString(),
                Url = "GerenciarMenuPagina.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 10,
                Descricao = "Usuários",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 5 },
                Ordem = 3,
                Url = "GerenciarUsuarios.aspx",
                Icone = Ext.Net.Icon.User.ToString()
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 11,
                Descricao = "Tipos Empresa",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 1,
                Icone = Ext.Net.Icon.Bricks.ToString(),
                Url = "GerenciarTiposEmpresa.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 12,
                Descricao = "Empresas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 2,
                Icone = Ext.Net.Icon.Neighbourhood.ToString(),
                Url = "GerenciarEmpresas.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 13,
                Descricao = "Setores",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 3,
                Icone = Ext.Net.Icon.ApplicationSideBoxes.ToString(),
                Url = "GerenciarSetores.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 14,
                Descricao = "Configurações do Sistema",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 5,
                Icone = Ext.Net.Icon.Cog.ToString(),
                Url = "ConfiguracoesSistema.aspx"
            };
            Lista.Add(menuPagina);

           menuPagina = new MenuPaginaVO()
            {
            //    Id = 15,
                Descricao = "Fotos",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 6,
                Url = "GerenciarGaleriasFotos.aspx",
                Icone = Ext.Net.Icon.BoxPicture.ToString()
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 16,
                Descricao = "Videos",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 7,
                Url = "GerenciarGaleriasVideos.aspx",
                Icone = Ext.Net.Icon.FolderFilm.ToString()
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 17,
                Descricao = "Visualizar Galerias",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 1,
                Icone = Ext.Net.Icon.FolderPicture.ToString(),
                Url = "VisualizarGalerias.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 18,
                Descricao = "Mensagens",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 2,
                Icone = Ext.Net.Icon.Email.ToString(),
                Url = "GerenciarMensagens.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 19,
                Descricao = "Pesquisas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 8,
                Icone = Ext.Net.Icon.Help.ToString(),
                Url = "GerenciarPesquisasOpiniao.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 20,
                Descricao = "Responder Pesquisas",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 3,
                Icone = Ext.Net.Icon.Outline.ToString(),
                Url = "ResponderPesquisasOpiniao.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 21,
                Descricao = "Registrar Ponto",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 4,
                Icone = Ext.Net.Icon.ClockStart.ToString(),
                Url = "RegistrarPonto.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 22,
                Descricao = "Pontos de Usuários",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 9,
                Icone = Ext.Net.Icon.Clock.ToString(),
                Url = "GerenciarPontosUsuarios.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 23,
                Descricao = "Contatos",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 7,
                Icone = Ext.Net.Icon.Group.ToString(),
                Url = "GerenciarContatos.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 24,
                Descricao = "Banco de Arquivos",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 5,
                Icone = Ext.Net.Icon.Database.ToString(),
                Url = "GerenciarArquivos.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 25,
                Descricao = "Notícias",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
                Ordem = 11,
                Icone = Ext.Net.Icon.Lightning.ToString(),
                Url = "GerenciarNoticias.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 26,
                Descricao = "Notícias",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 6,
                Icone = Ext.Net.Icon.LightningGo.ToString(),
                Url = "VisualizarNoticias.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 27,
                Descricao = "Aniversariantes",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 7,
                Icone = Ext.Net.Icon.Cake.ToString(),
                Url = "VisualizarAniversariantes.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //   Id = 28,
                Descricao = "Meu Perfil",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
                Ordem = 7,
                Icone = Ext.Net.Icon.TextfieldKey.ToString(),
                Url = "MeuPerfil.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 29,
                Descricao = "Ensalamento",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 4,
                Icone = Ext.Net.Icon.ApplicationSplit.ToString(),
                Url = "GerenciarEnsalamento.aspx"
            };
            Lista.Add(menuPagina);

            menuPagina = new MenuPaginaVO()
            {
            //    Id = 30,
                Descricao = "Reuniões",
                EmMenu = true,
                MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
                Ordem = 5,
                Icone = Ext.Net.Icon.BookOpen.ToString(),
                Url = "GerenciarReunioes.aspx"
            };
            Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 31,
				Descricao = "Funções",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
				Ordem = 12,
				Icone = Ext.Net.Icon.Wrench.ToString(),
				Url = "GerenciarFuncoes.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 32,
				Descricao = "Estrutura Organizacional",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
				Ordem = 6,
				Icone = Ext.Net.Icon.ChartOrganisation.ToString(),
				Url = "VisualizarOrganizacao.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 33,
				Descricao = "Colaboradores",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 4 },
				Ordem = 8,
				Icone = Ext.Net.Icon.UserHome.ToString(),
				Url = "Colaboradores.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 34,
				Descricao = "Extensões de Arquivos",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 2 },
				Ordem = 12,
				Icone = Ext.Net.Icon.AsteriskYellow.ToString(),
				Url = "GerenciarExtensoesArquivo.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 35,
				Descricao = "Manual do Colaborador",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
				Ordem = 8,
				Icone = Ext.Net.Icon.Book.ToString(),
				Url = "GerenciarManualColaborador.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 36,
				Descricao = "Oportunidades de Emprego",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 3 },
				Ordem = 9
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 37,
				Descricao = "Base de Conhecimento",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 36 },
				Ordem = 1,
				Icone = Ext.Net.Icon.Lightbulb.ToString(),
				Url = "GerenciarBaseConhecimento.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 38,
				Descricao = "Mural de Vagas",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 36 },
				Ordem = 2,
				Icone = Ext.Net.Icon.Layers.ToString(),
				Url = "GerenciarVagasEmprego.aspx"
			};
			Lista.Add(menuPagina);

			menuPagina = new MenuPaginaVO()
			{
				//    Id = 39,
				Descricao = "Buscar Perfil",
				EmMenu = true,
				MenuPaginaPai = new MenuPaginaVO() { Id = 36 },
				Ordem = 2,
				Icone = Ext.Net.Icon.Magnifier.ToString(),
				Url = "BuscarPerfilConhecimento.aspx"
			};
			Lista.Add(menuPagina);


            #endregion

        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new MenuPaginaBO(obj).Salvar());
        }


        #endregion
    }
}
