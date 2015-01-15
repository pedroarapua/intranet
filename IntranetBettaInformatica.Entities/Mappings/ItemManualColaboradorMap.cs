using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ItemManualColaboradorMap:ClassMap<ItemManualColaboradorVO>
    {
		public ItemManualColaboradorMap()
        {
            Table("itens_manual");
            Id(x => x.Id);
            Map(x => x.Descricao).Length(1000).Not.Nullable();
			References(x => x.Topico)
			  .Column("id_topico_manual")
			  .ForeignKey("fk_itens_manual_topicos_manual")
			  .LazyLoad();
        }
    }
}
