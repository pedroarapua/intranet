using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class ComentarioVideoVO : EntidadeBaseVO
    {
        /// <summary>
        /// Usuario do comentario
        /// </summary>
        public virtual UsuarioVO Usuario { get; set; }

        /// <summary>
        /// Video do comentario
        /// </summary>
        public virtual VideoVO Video { get; set; }

        /// <summary>
        /// Comentario
        /// </summary>
        public virtual String Comentario { get; set; }

        /// <summary>
        /// Data do comentario
        /// </summary>
        public virtual DateTime Data { get; set; }

    }
}
