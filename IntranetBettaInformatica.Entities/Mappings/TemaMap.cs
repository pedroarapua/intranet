using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class TemaMap:ClassMap<TemaVO>
    {
        public TemaMap()
        {
            Table("temas");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            Map(x => x.Descricao).Length(200);
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
