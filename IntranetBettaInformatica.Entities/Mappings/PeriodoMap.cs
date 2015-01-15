using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class PeriodoMap:ClassMap<PeriodoVO>
    {
        public PeriodoMap()
        {
            Table("periodos");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
        }
    }
}
