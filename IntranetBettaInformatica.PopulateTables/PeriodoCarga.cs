using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade PeriodoVO
    /// </summary>
    public static class PeriodoCarga
    {
        #region atributos

        private static List<PeriodoVO> lista = new List<PeriodoVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade PeriodoVO
        /// </summary>
        public static List<PeriodoVO> Lista
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
            PeriodoVO periodo = new PeriodoVO()
            {
                Nome = "Matutino"
            };
            Lista.Add(periodo);

            periodo = new PeriodoVO()
            {
                Nome = "Vespertino"
            };
            Lista.Add(periodo);

            periodo = new PeriodoVO()
            {
                Nome = "Noturno"
            };
            Lista.Add(periodo);

        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new PeriodoBO(obj).Salvar());
        }


        #endregion
    }
}
