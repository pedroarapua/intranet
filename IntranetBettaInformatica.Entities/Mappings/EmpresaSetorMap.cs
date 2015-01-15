using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class EmpresaSetorMap:ClassMap<EmpresaSetorVO>
    {
        public EmpresaSetorMap()
        {
            Table("setores");
            Id(x => x.Id);
            Map(x => x.Nome).Length(100).Not.Nullable();
            References(x => x.Empresa).ForeignKey("fk_empresas_setor").Column("id_empresa").LazyLoad().Not.Nullable();
            References(x => x.SetorPai).ForeignKey("fk_setor_setor").Column("id_setor_pai").LazyLoad();
            HasMany(x => x.SetoresFilhos)
                .KeyColumn("id_setor_pai")
                .LazyLoad()
                .Inverse()
                .Cascade.AllDeleteOrphan();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
