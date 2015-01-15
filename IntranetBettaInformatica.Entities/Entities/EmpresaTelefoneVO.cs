using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetBettaInformatica.Entities.Entities
{
    [Serializable]
    public class EmpresaTelefoneVO : EntidadeBaseVO
    {
        /// <summary>
        /// Empresa do Telefone
        /// </summary>
        public virtual EmpresaVO Empresa { get; set; }

        /// <summary>
        /// Telefone da Empresa
        /// </summary>
        public virtual String Telefone { get; set; }

    }
}
