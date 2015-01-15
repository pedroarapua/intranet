using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.Business;
using System.Configuration;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade TipoEmpresaVO
    /// </summary>
    public static class TipoEmpresaCarga
    {
        #region atributos

        private static List<TipoEmpresaVO> lista = new List<TipoEmpresaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade TipoEmpresaVO
        /// </summary>
        public static List<TipoEmpresaVO> Lista
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
            TipoEmpresaVO tipoEmpresa = new TipoEmpresaVO()
            {
                Descricao = "[Nenhum]"
            };
            Lista.Add(tipoEmpresa);

            tipoEmpresa = new TipoEmpresaVO()
            {
                Descricao = "Cliente"
            };
            Lista.Add(tipoEmpresa);

            tipoEmpresa = new TipoEmpresaVO()
            {
                Descricao = "Fornecedor"
            };
            Lista.Add(tipoEmpresa);
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new TipoEmpresaBO(obj).Salvar());
        }


        #endregion
    }
}
