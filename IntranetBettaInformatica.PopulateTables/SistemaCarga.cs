using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade SistemaVO
    /// </summary>
    public static class SistemaCarga
    {
        #region atributos

        private static List<SistemaVO> lista = new List<SistemaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ConfiguracoesSistemaVO
        /// </summary>
        public static List<SistemaVO> Lista
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
            SistemaVO sistema = new SistemaVO()
            {
                Nome = "DotProject",
                Url = "dotproject.bettawork.com.br",
                ExtensaoImagem = ".png"
            };
            Lista.Add(sistema);

        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new SistemaBO(obj).Salvar());
        }


        #endregion
    }
}
