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
    /// Entidade para regras de negocio de ComentarioVideoVO
    /// </summary>
    public class ComentarioVideoBO:BaseBO<ComentarioVideoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ComentarioVideoVO
        /// </summary>
        /// <param name="user"></param>
        public ComentarioVideoBO(ComentarioVideoVO comentario)
        {
            base.Object = comentario;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ComentarioVideoBO() { }

        #endregion

        #region metodos

        public List<ComentarioVideoVO> BuscarPorVideo(VideoVO video)
        {
            return base.Select(base.GetQueryLinq.Where(x => x.Video.Id == video.Id)).OrderByDescending(x => x.Data).ToList();
        }

        #endregion
    }
}
