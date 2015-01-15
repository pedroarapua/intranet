using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using IntranetBettaInformatica.DataAccess;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de FuncaoVO
    /// </summary>
    public class FuncaoBO:BaseBO<FuncaoVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro FuncaoVO
        /// </summary>
        /// <param name="user"></param>
        public FuncaoBO(FuncaoVO funcao)
        {
            base.Object = funcao;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public FuncaoBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// Metodo que consulta funções que possuam a ordem passada como parametro
        /// </summary>
		/// <param name="func"></param>
        /// <returns></returns>
        public Boolean ContemOrdem(FuncaoVO func, Int32 ordem)
        {
			IQueryable<FuncaoVO> query = base.GetQueryLinq.Where(x => x.Ordem == ordem && x.Removido == false);
			if (func != null)
				query = query.Where(x => x.Id != func.Id);
			return query.Count() > 0;
        }

        /// <summary>
        /// Atualiza todas as funções para uma ordem acima
        /// </summary>
        /// <param name="funcao"></param>
        public void AtualizaFuncaoParaOrdemSuperior(FuncaoVO funcao)
        {
            List<FuncaoVO> funcoes = base.Select(base.GetQueryLinq.Where(x => x.Ordem >= funcao.Ordem && x.Id != funcao.Id && x.Removido == false));
            foreach (FuncaoVO func in funcoes)
            {
                func.Ordem++;
                new FuncaoBO(func).Salvar();
            }
        }

        /// <summary>
        /// Busca último número de ordem para cadastro
        /// </summary>
        /// <param name="funcao"></param>
        public Int32 BuscaUltimaOrdem()
        {
			FuncaoVO func = base.GetQueryLinq.Where(x=> x.Removido == false).AsEnumerable().OrderBy(x=> x.Ordem).LastOrDefault();
			return func == null ? 1 : func.Ordem + 1;
        }

		/// <summary>
		/// Metodo que busca das funções não removidas
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		public List<FuncaoVO> BuscarFuncoes(FuncaoVO func)
		{
			IQueryable<FuncaoVO> query = base.GetQueryLinq;
			if(func == null)
				query = query.Where(x => x.Removido == false);
			else
				query = query.Where(x => x.Removido == false || x.Id == func.Id);
			
			return base.Select(query);
		}

        #endregion
    }
}
