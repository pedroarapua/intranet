using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class RespostaVO:EntidadeBaseVO
    {

        public RespostaVO()
        {
            this.Usuarios = new List<UsuarioVO>();
        }

        /// <summary>
        /// Resposta da Pergunta
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// Pesquisa da Resposta
        /// </summary>
        public virtual PesquisaOpiniaoVO Pesquisa { get; set; }

        /// <summary>
        /// Usuários que resopnderam essa resposta
        /// </summary>
        public virtual IList<UsuarioVO> Usuarios { get; set; }

    }
}
