using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class GaleriaMap:ClassMap<GaleriaVO>
    {
        public GaleriaMap()
        {
            Table("galerias");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            Map(x => x.Descricao).Length(200);
            Map(x => x.Removido).Not.Nullable().Default("False");
            HasMany(x => x.Fotos).KeyColumn("id_galeria")
               .Inverse()
               .LazyLoad()
               .Not.Cascade.All().Where("Removido = 'false'");
            HasMany(x => x.Videos).KeyColumn("id_galeria")
               .Inverse()
               .LazyLoad()
               .Not.Cascade.All().Where("Removido = 'false'");
        }
    }
}
