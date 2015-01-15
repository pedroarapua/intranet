using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class EstadoMap:ClassMap<EstadoVO>
    {
        public EstadoMap()
        {
            Table("estados");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            Map(x => x.Sigla).Length(2).Not.Nullable();
        }
    }
}
