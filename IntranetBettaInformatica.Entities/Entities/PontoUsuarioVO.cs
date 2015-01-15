using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class PontoUsuarioVO:EntidadeBaseVO
    {

        public PontoUsuarioVO()
        {
        
        }

        #region propriedades

        /// <summary>
        /// Data do Ponto
        /// </summary>
        public virtual DateTime Data { get; set; }

        /// <summary>
        /// Hora de Início do Ponto
        /// </summary>
        public virtual DateTime HoraInicio { get; set; }

        /// <summary>
        /// Hora de Término do Ponto
        /// </summary>
        public virtual DateTime? HoraTermino { get; set; }

        /// <summary>
        /// Usuário que registrou o ponto
        /// </summary>
        public virtual UsuarioVO Usuario { get; set; }

        /// <summary>
        /// Justificativa de registro de ponto manual
        /// </summary>
        public virtual String Justificativa { get; set; }

        /// <summary>
        /// Período do registro do ponto
        /// </summary>
        public virtual PeriodoVO Periodo { get; set; }

        #endregion

        #region propriedades nao mapeadas

        public virtual double Tempo
        {
            get
            {
                TimeSpan? time = null;
                if (HoraTermino.HasValue)
                {
                    time = HoraTermino.Value - HoraInicio;
                }
                else
                {
                    time = new TimeSpan(0, 0, 0);
                }
                return time.Value.TotalMinutes;
            }
        }

        #endregion

    }
}
