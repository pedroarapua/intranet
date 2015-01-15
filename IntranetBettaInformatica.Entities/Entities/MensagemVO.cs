using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class MensagemVO:EntidadeBaseVO
    {

        public MensagemVO()
        {
            this.UsuariosMensagens = new List<UsuarioMensagemVO>();    
        }

        /// <summary>
        /// Descrição da mensagem
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Data de envio da mensagem
        /// </summary>
        public virtual DateTime Data { get; set; }

        /// <summary>
        /// Usuario que enviou a mensagem
        /// </summary>
        public virtual UsuarioVO UsuarioEnvio { get; set; }

        /// <summary>
        /// Precisa de confirmação de leitura da mensagem
        /// </summary>
        public virtual Boolean ConfirmarLeitura { get; set; }

        /// <summary>
        /// Usuarios que receberam a mensagem
        /// </summary>
        public virtual IList<UsuarioMensagemVO> UsuariosMensagens { get; set; }

        #region propriedades nao mapeadas

        /// <summary>
        /// propriedade utilizada na gerencia de mensagens para agrupar as mensagens lidas e recebidas
        /// </summary>
        public virtual Boolean MensagemEnviada { get; set; }

        /// <summary>
        /// propriedade para verificar se a mensagem foi lida
        /// </summary>
        public virtual Boolean MensagemLida { get; set; }

        /// <summary>
        /// descrição do tipo da mensagem
        /// </summary>
        public virtual String TipoMensagem { 
            get {
                return MensagemEnviada ? "Enviadas" : "Recebidas";
            } 
        }

        #endregion

    }
}
