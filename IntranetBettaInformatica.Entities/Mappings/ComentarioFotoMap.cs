using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ComentarioFotoMap:ClassMap<ComentarioFotoVO>
    {
        public ComentarioFotoMap()
        {
            Table("comentarios_fotos");
            Id(x => x.Id);
            References(x => x.Usuario)
                .Column("id_usuario")
                .LazyLoad()
                .Not.Nullable();
            References(x => x.Foto)
                .Column("id_foto")
                .LazyLoad()
                .Not.Nullable();
            Map(x => x.Comentario).Column("comentario").Length(200).Not.Nullable();
            Map(x => x.Data).Column("data").Not.Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
