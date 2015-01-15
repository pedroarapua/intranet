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
    /// Entidade responsavel por dar carga no banco de dados na entidade EmpresaVO
    /// </summary>
    public static class EmpresaCarga
    {
        #region atributos

        private static List<EmpresaVO> lista = new List<EmpresaVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade EmpresaVO
        /// </summary>
        public static List<EmpresaVO> Lista
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
            EmpresaVO empresa = new EmpresaVO()
            {
                Nome = ConfigurationSettings.AppSettings["EmpresaSistema"],
                Endereco = "",
                Email = "",
                Site = "www.bettainformatica.com.br",
                TipoEmpresa = new TipoEmpresaVO() { Id = 1 }                
            };
            Lista.Add(empresa);

        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new EmpresaBO(obj).Salvar());
        }


        #endregion
    }
}
