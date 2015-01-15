using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de ItemManualColaboradorVO
    /// </summary>
    public class ItemManualColaboradorBO:BaseBO<ItemManualColaboradorVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro ItemManualColaboradorVO
        /// </summary>
        /// <param name="user"></param>
        public ItemManualColaboradorBO(ItemManualColaboradorVO item)
        {
            base.Object = item;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
		public ItemManualColaboradorBO() { }

        #endregion
    }
}
