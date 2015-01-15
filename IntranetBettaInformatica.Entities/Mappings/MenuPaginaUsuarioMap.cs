using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class MenuPaginaUsuarioMap:ClassMap<MenuPaginaUsuarioVO>
    {
        public MenuPaginaUsuarioMap()
        {
            Table("usuarios_menu_paginas");
            Id(x => x.Id);
            References(x => x.MenuPagina)
                .Not.Nullable()
                .ForeignKey("fk_usuarios_menu_paginas_paginas")
                .Column("id_menu_pagina");
            References(x => x.Usuario)
                .Not.Nullable()
                .ForeignKey("fk_usuarios_menu_paginas_usuarios")
                .Column("id_usuario");
        }
    }
}
