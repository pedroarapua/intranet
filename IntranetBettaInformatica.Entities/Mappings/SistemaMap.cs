using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class SistemaMap:ClassMap<SistemaVO>
    {
        public SistemaMap()
        {
            Table("sistemas");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            Map(x => x.Url).Length(100).Not.Nullable();
            Map(x => x.ExtensaoImagem).Column("extensao_imagem").Length(10);
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
