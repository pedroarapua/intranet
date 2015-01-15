using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Expressions;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para regras de negocio de EmpresaVO
    /// </summary>
    public class EmpresaBO:BaseBO<EmpresaVO>
    {
        #region contrutores

        /// <summary>
        /// contrutor com parametro TipoEmpresaVO
        /// </summary>
        /// <param name="user"></param>
        public EmpresaBO(EmpresaVO empresa)
        {
            base.Object = empresa;
        }

        /// <summary>
        /// construtor padrao
        /// </summary>
        public EmpresaBO() { }

        #endregion

        #region metodos

        /// <summary>
        /// metodo que busca os contatos da tabela de empresas
        /// </summary>
        /// <param name="removido"></param>
        /// <returns></returns>
        public List<EmpresaVO> BuscarContatos(String nome, TipoEmpresaVO tipo, Boolean? removido)
        {
            IQueryable<EmpresaVO> query = base.GetQueryLinq;
            if (removido.HasValue)
            {
                query = query.Where(x => x.RemovidoContato == removido);
            }
            if (!nome.IsNullOrEmpty())
            {
                query = query.Where(x => x.Nome.Contains(nome));
            }
            if (tipo != null)
            {
                query = query.Where(x => x.TipoEmpresa.Id == tipo.Id);
            }
            return base.Select(query);
        }

        /// <summary>
        /// metodo que remove a empresa da gerencia de contatos
        /// </summary>
        /// <returns></returns>
        public virtual Boolean DeleteContato(EmpresaVO empresa)
        {
            empresa.RemovidoContato = true;
            base.Object = empresa;
            try
            {
                base.Salvar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion
    }
}
