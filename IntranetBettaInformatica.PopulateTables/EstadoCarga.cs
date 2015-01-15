using IntranetBettaInformatica.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.PopulateTables
{
    /// <summary>
    /// Entidade responsavel por dar carga no banco de dados na entidade ConfiguracoesSistemaVO
    /// </summary>
    public class EstadoCarga
    {
        #region atributos

        private static List<EstadoVO> lista = new List<EstadoVO>();

        #endregion

        #region propriedades

        /// <summary>
        /// Lista de objetos a serem persistidos da entidade ConfiguracoesSistemaVO
        /// </summary>
        public static List<EstadoVO> Lista
        {
            get
            {
                return lista;
            }
            set
            {
                lista = value;
            }
        }

        #endregion

        #region metodos

        /// <summary>
        /// Carrega a lista
        /// </summary>
        private static void PreencherLista()
        {
            EstadoVO conf = new EstadoVO()
            {
                Nome = "Acre",
                Sigla = "AC"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Alagoas",
                Sigla = "AL"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Amapá",
                Sigla = "AP"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Amazonas",
                Sigla = "AM"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Bahia",
                Sigla = "BA"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Ceará",
                Sigla = "CE"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Distrito Federal",
                Sigla = "DF"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Espírito Santo",
                Sigla = "ES"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Goiás",
                Sigla = "GO"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Maranhão",
                Sigla = "MA"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Mato Grosso",
                Sigla = "MT"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Mato Grosso do Sul",
                Sigla = "MS"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Minas Gerais",
                Sigla = "MG"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Pará",
                Sigla = "PA"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Paraíba",
                Sigla = "PB"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Paraná",
                Sigla = "PR"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Pernambuco",
                Sigla = "PE"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Piauí",
                Sigla = "PI"
            };
            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Rio de Janeiro",
                Sigla = "RJ"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Rio Grande do Norte",
                Sigla = "RN"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Rio Grande do Sul",
                Sigla = "RS"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Rondônia",
                Sigla = "RO"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Roraima",
                Sigla = "RR"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Santa Catarina",
                Sigla = "SC"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "São Paulo",
                Sigla = "SP"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Sergipe",
                Sigla = "SE"
            };

            Lista.Add(conf);

            conf = new EstadoVO()
            {
                Nome = "Tocantins",
                Sigla = "TO"
            };

            Lista.Add(conf);
        }

        public static void DispararCargaBanco()
        {
            PreencherLista();
            Lista.ForEach(obj => new EstadoBO(obj).Salvar());
        }


        #endregion
    }
}
