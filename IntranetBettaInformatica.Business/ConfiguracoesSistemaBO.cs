using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ConfiguracoesSistemaVO
    /// </summary>
    public class ConfiguracoesSistemaBO:BaseBO<ConfiguracoesSistemaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ConfiguracoesSistemaVO
        /// </summary>
        /// <param name="user"></param>
        public ConfiguracoesSistemaBO(ConfiguracoesSistemaVO conf)
        {
            base.Object = conf;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public ConfiguracoesSistemaBO() { }

        #endregion
    }
}
