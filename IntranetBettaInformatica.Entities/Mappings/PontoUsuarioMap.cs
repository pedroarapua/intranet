using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class PontoUsuarioMap:ClassMap<PontoUsuarioVO>
    {
        public PontoUsuarioMap()
        {
            Table("pontos_usuarios");
            Id(x => x.Id);
            Map(x => x.Data).Column("data").Not.Nullable();
            Map(x => x.HoraInicio).Column("hora_inicio").Not.Nullable();
            Map(x => x.HoraTermino).Column("hora_termino").Nullable();
            References(x => x.Usuario).Column("id_usuario").ForeignKey("fk_pontos_usuarios_usuarios").LazyLoad().Not.Nullable();
            References(x => x.Periodo).Column("id_periodo").ForeignKey("fk_pontos_usuarios_periodos").LazyLoad().Not.Nullable();
            Map(x => x.Justificativa).Column("justificativa").Length(200).Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
