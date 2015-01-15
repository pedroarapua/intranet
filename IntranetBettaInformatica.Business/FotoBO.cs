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
    /// Entidade para regras de negocio de FotoVO
    /// </summary>
    public class FotoBO:BaseBO<FotoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro GaleriaVO
        /// </summary>
        /// <param name="user"></param>
        public FotoBO(FotoVO foto)
        {
            base.Object = foto;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public FotoBO() { }

        #endregion

        #region metodos

        public void AtualizarCapaAlbum(GaleriaVO galeria)
        {
            galeria = new GaleriaBO().SelectById(galeria.Id);
            foreach (FotoVO foto in galeria.Fotos)
            {
                foto.CapaAlbum = false;
                new FotoBO(foto).Salvar();
            }
        }

        #endregion
    }
}
