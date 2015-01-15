using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de SistemaVO
    /// </summary>
    public class SistemaBO:BaseBO<SistemaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro SistemaVO
        /// </summary>
        /// <param name="user"></param>
        public SistemaBO(SistemaVO sistema)
        {
            base.Object = sistema;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public SistemaBO() { }

        #endregion

        #region metodos

        public List<SistemaVO> Buscar(Boolean removido, List<SistemaVO> sistemasEdit)
        {
            IQueryable<SistemaVO> querySE = sistemasEdit.AsQueryable();
            IQueryable<SistemaVO> query = base.GetQueryLinq;
            IEnumerable<SistemaVO>  ienum = query.AsEnumerable().Where(x => x.Removido == removido || querySE.Any(x1=> x1.Id == x.Id));
            return ienum.ToList();
        }

        #endregion
    }
}
