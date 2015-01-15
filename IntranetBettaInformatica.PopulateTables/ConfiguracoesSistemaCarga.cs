using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade ConfiguracoesSistemaVO
    /// </summary>
    public static class ConfiguracoesSistemaCarga
    {
        #region atributos

        private static List<ConfiguracoesSistemaVO> lista = new List<ConfiguracoesSistemaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ConfiguracoesSistemaVO
        /// </summary>
        public static List<ConfiguracoesSistemaVO> Lista { get { return lista;} set { lista = value;} }
        
        #endregion

        #region metodos

        /// <summary>
        /// Carrega a lista
        /// </summary>
        private static void PreencherLista()
        {
            ConfiguracoesSistemaVO conf = new ConfiguracoesSistemaVO()
            {
                Descricao = "Intranet",
                ExtensaoImagem = ".png"
            };
            Lista.Add(conf);
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new ConfiguracoesSistemaBO(obj).Salvar());
        }


        #endregion
    }
}
