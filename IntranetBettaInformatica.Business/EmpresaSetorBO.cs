using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de EmpresaSetorVO
    /// </summary>
    public class EmpresaSetorBO:BaseBO<EmpresaSetorVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TemaVO
        /// </summary>
        /// <param name="user"></param>
        public EmpresaSetorBO(EmpresaSetorVO setor)
        {
            base.Object = setor;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public EmpresaSetorBO() { }

        #endregion

        #region metodos

        public List<EmpresaSetorVO> BuscarSetoresPai(EmpresaVO empresa)
        {
            IQueryable<EmpresaSetorVO> query = base.GetQueryLinq.Where(x => x.SetorPai == null && !x.Removido);
            if (empresa != null)
                query = query.Where(x => x.Empresa.Id == empresa.Id);

            return base.Select(query);
        }

        #endregion
    }
}
