using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ReuniaoMap : ClassMap<ReuniaoVO>
    {
        public ReuniaoMap()
        {
            Table("reunioes");
            Id(x => x.Id);
            Map(x => x.Titulo).Length(500).Not.Nullable();
            Map(x => x.Descricao).Length(1000).Nullable();
            Map(x => x.DataInicial).Not.Nullable().Column("data_inicial");
            Map(x => x.DataFinal).Not.Nullable().Column("data_final");
            Map(x => x.UID).Not.Nullable().Column("uid");
            Map(x => x.ECancelada).Not.Nullable().Column("cancelada").Default("False");
            References(x => x.SalaReuniao).Column("id_sala").ForeignKey("fk_reunioes_salas").LazyLoad();
            HasManyToMany(x => x.Participantes).Table("reunioes_usuarios")
                .ParentKeyColumn("id_reunaio")
                .ChildKeyColumn("id_usuario")
                .ForeignKeyConstraintNames("fk_reunioes_usuarios_reunioes", "fk_reunioes_usuarios_reunioes")
                .LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
