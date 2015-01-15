using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade UsuarioVO
    /// </summary>
    public static class UsuarioCarga
    {
        #region atributos

        private static List<UsuarioVO> lista = new List<UsuarioVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade UsuarioVO
        /// </summary>
        public static List<UsuarioVO> Lista
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
            UsuarioVO usuario = new UsuarioVO()
            {
                Cidade = "Arapuã",
                DataNascimento = new DateTime(1987, 7, 13),
                Email = "pedro.dias@bettainformatica.com.br",
                Endereco = "Rua Alberto Schirato Numero: 702, AP. 03",
                Login = "moderador",
                Nome = "Pedro Henrique Fernandes Dias",
                PalavraChave = UsuarioBO.EncriptyPassword("teste"),
                Senha = UsuarioBO.EncriptyPassword("123"),
                UsuarioSistema = true,
                Estado = new EstadoVO() { Id = 16 },
                Tema = new TemaVO() { Id = 1 },
                Empresa = new EmpresaVO() { Id = 1 },
                PaginaInicial = new MenuPaginaVO() { Id = 1 },
                PerfilAcesso = new PerfilAcessoVO() { Id = 1 }
            };
            usuario.Sistemas = new List<SistemaVO>();
            usuario.Sistemas.Add(new SistemaVO(){Id = 1});
            Lista.Add(usuario);

            usuario = new UsuarioVO()
            {
                Cidade = "Franca",
                Login = "daiane",
                Nome = "Daiane",
                PalavraChave = UsuarioBO.EncriptyPassword("123"),
                Senha = UsuarioBO.EncriptyPassword("123"),
                UsuarioSistema = true,
                Estado = new EstadoVO() { Id = 25 },
                Tema = new TemaVO() { Id = 1 },
                Empresa = new EmpresaVO() { Id = 1 },
                PaginaInicial = new MenuPaginaVO(){ Id = 1 },
                PerfilAcesso = new PerfilAcessoVO() { Id = 2 }
            };
            Lista.Add(usuario);

        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new UsuarioBO(obj).Salvar());
        }


        #endregion
    }
}
