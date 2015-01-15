using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de SalaVO
    /// </summary>
    public class SalaBO:BaseBO<SalaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro SalaVO
        /// </summary>
        /// <param name="user"></param>
        public SalaBO(SalaVO estado)
        {
            base.Object = estado;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public SalaBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// metodo que busca as salas disponíveis para a reunião
        /// </summary>
        /// <param name="reuniaoSelecionada"></param>
        /// <returns></returns>
        public List<SalaVO> BuscarSalasDisponiveis(ReuniaoVO reuniaoSelecionada, DateTime dataInicial, DateTime dataFinal)
        {
            IEnumerable<SalaVO> lstSala = base.GetQueryLinq.AsEnumerable();
            IQueryable<ReuniaoVO> queryReuniao = new BaseBO<ReuniaoVO>().GetQueryLinq.Where(x=> x.ECancelada == false);

            
            if (reuniaoSelecionada.Id != 0)
                queryReuniao = queryReuniao.Where(x => ((dataInicial >= x.DataInicial && dataInicial <= x.DataFinal) || (dataFinal >= x.DataInicial && dataFinal <= x.DataFinal) || (dataInicial <= x.DataInicial && dataFinal >= x.DataFinal)) && x.Id != reuniaoSelecionada.Id);
            else
                queryReuniao = queryReuniao.Where(x => (dataInicial >= x.DataInicial && dataInicial <= x.DataFinal) || (dataFinal >= x.DataInicial && dataFinal <= x.DataFinal) || (dataInicial <= x.DataInicial && dataFinal >= x.DataFinal));

            IEnumerable<ReuniaoVO> lstReuniao = queryReuniao.AsEnumerable();
            return lstSala.Where(x => !lstReuniao.Any(x1 => x1.SalaReuniao.Id == x.Id)).ToList();
        }

        #endregion
    }
}
