using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ComputadorMap:ClassMap<ComputadorVO>
    {
        public ComputadorMap()
        {
            Table("computadores");
            Id(x => x.Id);
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Ip).Length(20);
            References(x => x.Usuario).Column("id_usuario").ForeignKey("fk_computadores_usuarios");
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
