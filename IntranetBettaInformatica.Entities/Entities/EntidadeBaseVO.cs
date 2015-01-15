using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    /// <summary>
    /// Entidade Base das Entidades
    /// </summary>
    public class EntidadeBaseVO
    {
        /// <summary>
        /// Codigo da Entidade
        /// </summary>
        public virtual Int32 Id { get; set; }

        /// <summary>
        /// Removido da Tabela
        /// </summary>
        public virtual Boolean Removido { get; set; }
    }
}
