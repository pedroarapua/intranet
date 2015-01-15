using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ComentarioVideoMap:ClassMap<ComentarioVideoVO>
    {
        public ComentarioVideoMap()
        {
            Table("comentarios_videos");
            Id(x => x.Id);
            References(x => x.Usuario)
                .Column("id_usuario")
                .LazyLoad()
                .Not.Nullable();
            References(x => x.Video)
                .Column("id_video")
                .LazyLoad()
                .Not.Nullable();
            Map(x => x.Comentario).Column("comentario").Length(200).Not.Nullable();
            Map(x => x.Data).Column("data").Not.Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
