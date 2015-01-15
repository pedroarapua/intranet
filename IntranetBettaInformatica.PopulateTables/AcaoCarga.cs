using IntranetBettaInformatica.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Entities.Enumertators;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
	/// Entidade responsavel por dar carga no banco de dados na entidade AcaoCargaVO
    /// </summary>
	public class AcaoCarga
    {
        #region atributos

        private static List<AcaoVO> lista = new List<AcaoVO>();

        #endregion

        #region propriedades

        /// <summary>
		/// Lista de objetos a serem persistidos da entidade AcaoVO
        /// </summary>
		public static List<AcaoVO> Lista
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
			foreach (ETipoAcao acao in Enum.GetValues(typeof(ETipoAcao)))
			{
				if (new List<Int32>(new Int32[] { 103, 114 }).Any(x => x == acao.ToInt32())) // Home
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(1)});
				else if(new List<Int32>(new Int32[]{1,2,3,4}).Any(x=> x == acao.ToInt32())) // Perfis Acesso
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(8)});
				else if (new List<Int32>(new Int32[] { 5, 6, 7, 8 }).Any(x => x == acao.ToInt32())) // Paginas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(9)});
				else if (new List<Int32>(new Int32[] { 9, 10, 11, 12, 13 }).Any(x => x == acao.ToInt32())) // Usuarios
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(10)});
				else if (new List<Int32>(new Int32[] { 14, 15, 16, 17 }).Any(x => x == acao.ToInt32())) // Temas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(6)});
				else if (new List<Int32>(new Int32[] { 18, 19, 20, 21 }).Any(x => x == acao.ToInt32())) // Sistemas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(7)});
				else if (new List<Int32>(new Int32[] { 22, 23 }).Any(x => x == acao.ToInt32())) // Configurações
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(14)});
				else if (new List<Int32>(new Int32[] { 24, 25, 26, 27, 28, 29, 30 }).Any(x => x == acao.ToInt32())) // Fotos
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(15)});
				else if (new List<Int32>(new Int32[] { 31, 32, 33, 34, 35, 36, 37 }).Any(x => x == acao.ToInt32())) // Videos
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(16)});
				else if (new List<Int32>(new Int32[] { 38, 39, 40, 41, 42 }).Any(x => x == acao.ToInt32())) // Pesquisas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(19)});
				else if (new List<Int32>(new Int32[] { 43, 44, 45, 46, 47, 48 }).Any(x => x == acao.ToInt32())) // Pontos de Usuários
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(22)});
				else if(new List<Int32>(new Int32[] { 49 }).Any(x => x == acao.ToInt32())) // Registro de Ponto Eletrônico
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = null });
				else if (new List<Int32>(new Int32[] { 50, 51, 52, 53 }).Any(x => x == acao.ToInt32())) // Contatos
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(23)});
				else if (new List<Int32>(new Int32[] { 54, 55, 56, 57, 121 }).Any(x => x == acao.ToInt32())) // Noticias
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(25)});
				else if (new List<Int32>(new Int32[] { 58, 59, 60, 104, 105, 106, 107, 108, 109 }).Any(x => x == acao.ToInt32())) // Visualizar Galerias
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(17)});
				else if (new List<Int32>(new Int32[] { 61, 62, 63, 64, 65 }).Any(x => x == acao.ToInt32())) // Mensagens
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(18)});
				else if (new List<Int32>(new Int32[] { 66, 67, 68 }).Any(x => x == acao.ToInt32())) // Responder Pesquisas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(20)});
				else if (new List<Int32>(new Int32[] { 69, 70, 71, 72, 73, 74, 75, 76 }).Any(x => x == acao.ToInt32())) // Banco de Arquivos
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(24)});
				else if (new List<Int32>(new Int32[] { 77 }).Any(x => x == acao.ToInt32())) // Visualizar Notícias
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(26)});
				else if (new List<Int32>(new Int32[] { 78, 79 }).Any(x => x == acao.ToInt32())) // Aniversariantes
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(27)});
				else if (new List<Int32>(new Int32[] { 80, 81 }).Any(x => x == acao.ToInt32())) // Meu Perfil
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(28)});
				else if (new List<Int32>(new Int32[] { 82, 83, 84, 85 }).Any(x => x == acao.ToInt32())) // Tipos Empresa
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(11)});
				else if (new List<Int32>(new Int32[] { 86, 87, 88, 89, 90, 117 }).Any(x => x == acao.ToInt32())) // Empresas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(12)});
				else if (new List<Int32>(new Int32[] { 91, 92, 93, 94, 115 }).Any(x => x == acao.ToInt32())) // Setores
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(13)});
				else if (new List<Int32>(new Int32[] { 95, 96, 97, 98 }).Any(x => x == acao.ToInt32())) // Ensalamento
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(29)});
				else if (new List<Int32>(new Int32[] { 99, 100, 101, 102, 118, 125 }).Any(x => x == acao.ToInt32())) // Reuniões
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(30)});
				else if (new List<Int32>(new Int32[] { 110, 111, 112, 113 }).Any(x => x == acao.ToInt32())) // Funções
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(31) });
				else if(new List<Int32>(new Int32[] { 116 }).Any(x => x == acao.ToInt32())) // Visualizar Organização
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(32) });
				else if (new List<Int32>(new Int32[] { 119, 120 }).Any(x => x == acao.ToInt32())) // Colaboradores
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(33) });
				else if (new List<Int32>(new Int32[] { 122, 123, 124 }).Any(x => x == acao.ToInt32())) // Extensoes
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(34) });
				else if (new List<Int32>(new Int32[] { 126, 127, 128, 129, 130, 131, 132 }).Any(x => x == acao.ToInt32())) // Manual dos colaboradores
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(35) });
				else if (new List<Int32>(new Int32[] { 133, 134, 135, 136, 137, 138, 139 }).Any(x => x == acao.ToInt32())) // Gerenciar Base de Conhecimento
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(37) });
				else if (new List<Int32>(new Int32[] { 140, 141, 142, 143, 144, 145, 146, 147 }).Any(x => x == acao.ToInt32())) // Mural de Vagas
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(38) });
				else if (new List<Int32>(new Int32[] { 148, 149 }).Any(x => x == acao.ToInt32())) // Buscar Perfil de Conhecimento
					Lista.Add(new AcaoVO() { Descricao = acao.ToText(), Pagina = new MenuPaginaBO().SelectById(39) });
			}
	    }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new AcaoBO(obj).Salvar());
        }


        #endregion
    }
}
