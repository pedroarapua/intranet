using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class UsuarioTelefoneVO : EntidadeBaseVO
    {
        /// <summary>
        /// Usuario do Telefone
        /// </summary>
        public virtual UsuarioVO Usuario { get; set; }

        /// <summary>
        /// Telefone do Usuario
        /// </summary>
        public virtual String Telefone { get; set; }

    }
}
