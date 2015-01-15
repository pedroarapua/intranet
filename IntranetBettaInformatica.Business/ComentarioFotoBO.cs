using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using NHibernate;
using IntranetBettaInformatica.DataAccess;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ComentarioFotoVO
    /// </summary>
    public class ComentarioFotoBO:BaseBO<ComentarioFotoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro CometarioFotoVO
        /// </summary>
        /// <param name="user"></param>
        public ComentarioFotoBO(ComentarioFotoVO comentario)
        {
            base.Object = comentario;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ComentarioFotoBO() { }

        #endregion

        #region metodos

        public List<ComentarioFotoVO> BuscarPorFoto(FotoVO foto)
        {
            return base.Select(base.GetQueryLinq.Where(x => x.Foto.Id == foto.Id)).OrderByDescending(x=> x.Data).ToList();
        }

        #endregion
    }
}
