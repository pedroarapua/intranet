using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class TipoEmpresaMap:ClassMap<TipoEmpresaVO>
    {
        public TipoEmpresaMap()
        {
            Table("tipos_empresa");
            Id(x => x.Id);
            Map(x => x.Descricao).Length(100).Not.Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
