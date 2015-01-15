using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class ReuniaoVO:EntidadeBaseVO
    {

        #region construtores

        public ReuniaoVO()
        {
            this.Participantes = new List<UsuarioVO>();
        }

        #endregion

        #region propriedades

        /// <summary>
        /// titulo da reunião
        /// </summary>
        public virtual String Titulo { get; set; }

        /// <summary>
        /// descrição da reunião
        /// </summary>
        public virtual String Descricao { get; set; }

        /// <summary>
        /// data de início da reunião
        /// </summary>
        public virtual DateTime DataInicial { get; set; }

        /// <summary>
        /// data de término da reunião
        /// </summary>
        public virtual DateTime DataFinal { get; set; }

        /// <summary>
        /// usuários participantes da reunião
        /// </summary>
        public virtual IList<UsuarioVO> Participantes { get; set; }

        /// <summary>
        /// ID referente a integração com outlook para modificação e cancelamento da reunião
        /// </summary>
        public virtual String UID { get; set; }

        /// <summary>
        /// local em que será realizado a reunião
        /// </summary>
        public virtual SalaVO SalaReuniao { get; set; }
        
        /// <summary>
        /// Boolean retornando se a reunião foi cancelada
        /// </summary>
        public virtual Boolean ECancelada { get; set; }

        #endregion

        #region propriedades nao mapeadas

        /// <summary>
        /// propriedade que retorna o status da reunião
        /// </summary>
        public virtual StatusReuniao Status
        {
            get
            {
                if (ECancelada)
                    return StatusReuniao.Cancelada;
                else if ((DataInicial >= DateTime.Now && DataFinal <= DateTime.Now) || (DataInicial < DateTime.Now && DataFinal > DateTime.Now))
                    return StatusReuniao.Iniciada;
                else if (DataInicial > DateTime.Now)
                    return StatusReuniao.Aguardando;
                else
                    return StatusReuniao.Finalizada;
            }
        }

        /// <summary>
        /// propriedade que retorna o status da reunião
        /// </summary>
        public virtual String StatusIcon
        {
            get
            {
                if (ECancelada)
                    return "icon-controlrecord";
                else if ((DataInicial >= DateTime.Now && DataFinal <= DateTime.Now) || (DataInicial < DateTime.Now && DataFinal > DateTime.Now))
                    return "icon-controlplayblue";
                else if (DataInicial > DateTime.Now)
                    return "icon-controlpauseblue";
                else
                    return "icon-controlstopblue";
            }
        }

        /// <summary>
        /// Codigo identificador da reunião
        /// </summary>
        public virtual String Codigo
        {
            get { return String.Format("R{0}", Id.ToString().PadLeft(4, '0')); }
        }

        #endregion
    }

    public enum StatusReuniao
    {
        Aguardando = 0,
        Iniciada = 1,
        Finalizada = 2,
        Cancelada = 3
    }
}
