using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade TemaVO
    /// </summary>
    public static class TemaCarga
    {
        #region atributos

        private static List<TemaVO> lista = new List<TemaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ConfiguracoesSistemaVO
        /// </summary>
        public static List<TemaVO> Lista
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
            TemaVO tema = new TemaVO()
            {
                Nome = "Slate",
                Descricao = "Tema Azul"                
            };
            Lista.Add(tema);

            tema = new TemaVO()
            {
                Nome = "Gray",
                Descricao = "Tema Gray"
            };
            Lista.Add(tema);

            tema = new TemaVO()
            {
                Nome = "Access",
                Descricao = "Tema Acessível"
            };
            Lista.Add(tema);

            tema = new TemaVO()
            {
                Nome = "Default",
                Descricao = "Tema Padrão"
            };
            Lista.Add(tema);
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new TemaBO(obj).Salvar());
        }


        #endregion
    }
}
