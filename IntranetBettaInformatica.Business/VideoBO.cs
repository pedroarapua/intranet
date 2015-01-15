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
    /// Entidade para regras de negocio de VideoVO
    /// </summary>
    public class VideoBO:BaseBO<VideoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro VideoVO
        /// </summary>
        /// <param name="user"></param>
        public VideoBO(VideoVO video)
        {
            base.Object = video;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public VideoBO() { }

        #endregion

    }
}
