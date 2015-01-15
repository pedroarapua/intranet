using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class VagaEmpregoCurriculoMap : ClassMap<VagaEmpregoCurriculoVO>
    {
		public VagaEmpregoCurriculoMap()
        {
            Table("curriculos_vagas_empregeo");
            Id(x => x.Id);
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Extensao).Column("extensao").Length(20).Not.Nullable();
			Map(x => x.NomeOriginal).Column("nome_original").Length(50).Not.Nullable();
			References(x => x.VagaEmprego).Column("id_vaga_emprego").LazyLoad().Not.Nullable();
			Map(x => x.Removido).Not.Nullable().Default("False");
			Where("Removido = 'false'");
        }
    }
}
