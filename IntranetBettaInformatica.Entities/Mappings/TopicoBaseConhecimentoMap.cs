using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class TopicoBaseConhecimentoMap:ClassMap<TopicoBaseConhecimentoVO>
    {
		public TopicoBaseConhecimentoMap()
        {
            Table("topicos_conhecimentos");
            Id(x => x.Id);
            Map(x => x.Titulo).Length(200).Not.Nullable();
			HasMany(x => x.Conhecimentos).KeyColumn("id_topico_conhecimento")
				.Inverse()
				.Cascade.AllDeleteOrphan()
				.LazyLoad();
        }
    }
}
