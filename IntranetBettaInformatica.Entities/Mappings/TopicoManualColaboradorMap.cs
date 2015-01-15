using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class TopicoManualColaboradorMap:ClassMap<TopicoManualColaboradorVO>
    {
		public TopicoManualColaboradorMap()
        {
            Table("topicos_manual");
            Id(x => x.Id);
            Map(x => x.Titulo).Length(200).Not.Nullable();
			HasMany(x => x.ItensManualColaborador).KeyColumn("id_topico_manual")
				.Inverse()
				.Cascade.AllDeleteOrphan()
				.LazyLoad();
        }
    }
}
