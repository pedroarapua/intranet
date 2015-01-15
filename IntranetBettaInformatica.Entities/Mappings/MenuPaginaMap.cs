using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class MenuPaginaMap:ClassMap<MenuPaginaVO>
    {
        public MenuPaginaMap()
        {
            Table("menu_paginas");
            Id(x => x.Id);
            Map(x => x.Descricao).Length(100).Not.Nullable();
            Map(x => x.Url).Length(100);
            Map(x => x.EmMenu).Column("em_menu").Not.Nullable().Default("True");
            Map(x => x.Ordem).Not.Nullable();
            Map(x => x.Icone).Length(50);
            References(x => x.MenuPaginaPai)
                .ForeignKey("fk_menu_paginas_menu_paginas")
                .Column("id_menu_pagina_pai")
                .LazyLoad();
            HasMany(x => x.MenuPaginas)
                .KeyColumn("id_menu_pagina_pai")
                .LazyLoad()
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .OrderBy("Ordem");
			HasMany(x => x.Acoes)
				.KeyColumn("id_menu_pagina")
				.LazyLoad()
				.Not.Cascade.All();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
