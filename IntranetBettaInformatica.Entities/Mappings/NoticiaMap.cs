using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class NoticiaMap : ClassMap<NoticiaVO>
    {
        public NoticiaMap()
        {
            Table("noticias");
            Id(x => x.Id);
            Map(x => x.Titulo).Length(1000).Not.Nullable();
			Map(x => x.HTML).Length(65535).Nullable();
            Map(x => x.DataInicial).Not.Nullable();
            Map(x => x.DataFinal).Not.Nullable();
            HasManyToMany(x => x.Usuarios).Table("noticias_usuarios")
                .ParentKeyColumn("id_noticia")
                .ChildKeyColumn("id_usuario")
                .ForeignKeyConstraintNames("fk_noticias_usuarios_noticias", "fk_noticias_usuarios_usuarios")
                .LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
