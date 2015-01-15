using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de RespostaVO
    /// </summary>
    public class RespostaBO:BaseBO<RespostaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro PesquisaOpiniaoVO
        /// </summary>
        /// <param name="user"></param>
        public RespostaBO(RespostaVO resposta)
        {
            base.Object = resposta;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public RespostaBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// Salva as pesquisas com as respostas
        /// </summary>
        /// <param name="pesquisas"></param>
        public void SalvarRespostas(List<RespostaVO> respostas)
        {
            try
            {
                base.IniciaTransacao();
                respostas.ForEach(x => new RespostaBO(x).Salvar());
                base.FinalizaTransacao(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
