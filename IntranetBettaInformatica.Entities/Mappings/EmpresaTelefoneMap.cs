using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class EmpresaTelefoneMap:ClassMap<EmpresaTelefoneVO>
    {
        public EmpresaTelefoneMap()
        {
            Table("empresas_telefones");
            Id(x => x.Id);
            Map(x => x.Telefone).Column("telefone").Length(14).Not.Nullable();
            References(x => x.Empresa)
                .Column("id_empresa")
                .LazyLoad()
                .Not.Nullable();
        }
    }
}
