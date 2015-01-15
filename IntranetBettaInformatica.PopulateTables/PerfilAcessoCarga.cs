using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade PerfilAcessoVO
    /// </summary>
    public static class PerfilAcessoCarga
    {
        #region atributos

        private static List<PerfilAcessoVO> lista = new List<PerfilAcessoVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ConfiguracoesSistemaVO
        /// </summary>
        public static List<PerfilAcessoVO> Lista
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
            PerfilAcessoVO perfil = new PerfilAcessoVO()
            {
                Nome = "Moderador",
                EModerador = true,
                Removido = false,
            };

			List<AcaoVO> lstAcoes = new AcaoBO().Select();

			perfil.Acoes = lstAcoes;
			perfil.Acoes.Add(new AcaoBO().SelectById(49));

            Lista.Add(perfil);

            perfil = new PerfilAcessoVO()
            {
                Nome = "Secretária",
                EModerador = false,
                Removido = false
            };
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(1).Acoes).ToList(); // Default.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(23).Acoes).ToList(); // MeuPerfil.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(2).Acoes).ToList(); // Menu Administração
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(4).Acoes).ToList(); // Menu Colaborador
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(28).Acoes).ToList(); // GerenciarContatos.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(18).Acoes).ToList(); // GerenciarMensagens.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(27).Acoes).ToList(); // VisualizarAniversariantes.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(26).Acoes).ToList(); // VisualizarNoticias.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(20).Acoes).ToList(); // ResponderPesquisasOpiniao.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(32).Acoes).ToList(); // VisualizarOrganizacao.aspx
			perfil.Acoes = perfil.Acoes.Union(new MenuPaginaBO().SelectById(33).Acoes).ToList(); // Colaboradores.aspx
			
			perfil.Acoes.Add(new AcaoBO().SelectById(49)); // Outras Permissões
			perfil.Acoes.Add(new AcaoBO().SelectById(126)); // Visualizar Manual do Colaborador
			perfil.Acoes.Add(new AcaoBO().SelectById(140)); // Visualizar Mural de Vagas
			perfil.Acoes.Add(new AcaoBO().SelectById(147)); // Indicar Vaga

            Lista.Add(perfil);
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new PerfilAcessoBO(obj).Salvar());
        }


        #endregion
    }
}
