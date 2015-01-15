using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class EmpresaMap:ClassMap<EmpresaVO>
    {
        public EmpresaMap()
        {
            Table("empresas");
            Id(x => x.Id);
            Map(x => x.Nome).Length(200).Column("nome").Not.Nullable();
            Map(x => x.Endereco).Column("endereco").Length(200);
            Map(x => x.Cidade).Length(100);
            Map(x => x.Email).Column("email").Length(100);
            Map(x => x.Site).Column("site").Length(100);
            References(x => x.Estado)
               .Column("id_estado")
               .ForeignKey("fk_empresas_estados")
               .LazyLoad();
            References(x => x.TipoEmpresa)
                .Column("id_tipo_empresa")
                .LazyLoad()
                .Not.Nullable();
            HasMany(x => x.Telefones).KeyColumn("id_empresa")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();
            HasMany(x => x.Setores)
                .KeyColumn("id_empresa")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Map(x => x.RemovidoContato).Column("removido_contato").Not.Nullable().Default("False");
        }
    }
}
