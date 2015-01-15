using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
	public class AcaoMap : ClassMap<AcaoVO>
	{
		public AcaoMap()
		{
			Table("acoes_paginas");
			Id(x => x.Id);
			Map(x => x.Descricao).Length(100).Not.Nullable();
			References(x => x.Pagina).Column("id_menu_pagina").ForeignKey("fk_acoes_paginas_paginas").Not.LazyLoad();
		}
	}
}
