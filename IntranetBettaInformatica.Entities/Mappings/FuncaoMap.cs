using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class FuncaoMap : ClassMap<FuncaoVO>
    {
        public FuncaoMap()
        {
            Table("funcoes");
            Id(x => x.Id);
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Descricao).Length(500);
            Map(x => x.Ordem).Not.Nullable();
			HasMany(x => x.Usuarios).KeyColumn("id_funcao")
				.Inverse()
				.LazyLoad()
				.Not.Cascade.All();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
