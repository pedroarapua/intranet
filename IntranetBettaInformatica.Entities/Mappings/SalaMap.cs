using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class SalaMap:ClassMap<SalaVO>
    {
        public SalaMap()
        {
            Table("salas");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            References(x => x.Setor).ForeignKey("fk_salas_setores").Column("id_setor").LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
