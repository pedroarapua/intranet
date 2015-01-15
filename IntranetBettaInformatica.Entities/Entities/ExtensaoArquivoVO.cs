using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    public class ExtensaoArquivoVO:EntidadeBaseVO
    {
        #region propriedades mapeadas

        /// <summary>
        /// Extensao do aquivo
        /// </summary>
        public virtual String Extensao { get; set; }

        /// <summary>
        /// Tipo de Extensão
        /// </summary>
		public virtual ETipoExtensao TipoExtensao { get; set; }

        #endregion

        #region propriedades não mapeadas

        /// <summary>
        /// Descrição do tipo de extensão
        /// </summary>
		public virtual String DescricaoTipoExtensao { get { return TipoExtensao.ToText(); } }

        #endregion
    }
}
