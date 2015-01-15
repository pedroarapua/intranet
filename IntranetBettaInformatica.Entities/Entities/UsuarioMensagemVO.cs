using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class UsuarioMensagemVO:EntidadeBaseVO
    {

        public UsuarioMensagemVO()
        {
            
        }

        /// <summary>
        /// Usuario de Destino
        /// </summary>
        public virtual UsuarioVO UsuarioRecMens { get; set; }

        /// <summary>
        /// Mensagem enviada
        /// </summary>
        public virtual MensagemVO Mensagem { get; set; }

        /// <summary>
        /// Mensagem foi lida?
        /// </summary>
        public virtual Boolean LidoMensagem { get; set; }
    }
}
