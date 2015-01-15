using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class VagaEmpregoMap : ClassMap<VagaEmpregoVO>
    {
		public VagaEmpregoMap()
        {
            Table("vagas_emprego");
            Id(x => x.Id);
            Map(x => x.Titulo).Column("titulo").Length(200).Not.Nullable();
            Map(x => x.Descricao).Column("descricao").Length(2000).Not.Nullable();
			Map(x => x.DataAtualizacao).Column("data_ultima_atualizacao").Not.Nullable();
            Map(x => x.Status).Column("status").CustomType<int>().Not.Nullable().Default("1");
			HasMany(x => x.Curriculos).KeyColumn("id_vaga_emprego")
				.Inverse()
				.LazyLoad()
				.Not.Cascade.All();
			Map(x => x.Removido).Not.Nullable().Default("False");
			Where("Removido = 'false'");
	    }
    }
}
