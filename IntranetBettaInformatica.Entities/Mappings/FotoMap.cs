using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class FotoMap : ClassMap<FotoVO>
    {
        public FotoMap()
        {
            Table("fotos");
            Id(x => x.Id);
            Map(x => x.Extensao).Column("extensao").Length(10).Not.Nullable();
            Map(x => x.Titulo).Column("titulo").Length(200);
            Map(x => x.NomeOriginal).Column("nome_original").Length(50);
            Map(x => x.CapaAlbum).Column("capa_album").Not.Nullable().Default("False");
            Map(x => x.Removido).Not.Nullable().Default("False");
            References(x => x.Galeria).Not.Nullable().Column("id_galeria").ForeignKey("fk_galerias_fotos");
            HasMany(x => x.Comentarios).KeyColumn("id_foto")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .OrderBy("Data desc")
                .LazyLoad();
        }
    }
}
