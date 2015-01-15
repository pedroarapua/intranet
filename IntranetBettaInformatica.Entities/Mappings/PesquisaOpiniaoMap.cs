using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class PesquisaOpiniaoMap : ClassMap<PesquisaOpiniaoVO>
    {
        public PesquisaOpiniaoMap()
        {
            Table("pesquisas_opiniao");
            Id(x => x.Id);
            Map(x => x.Pergunta).Length(500).Not.Nullable();
            Map(x => x.DataInicial).Not.Nullable();
            Map(x => x.DataFinal).Not.Nullable();
            Map(x => x.MostrarResultado).Not.Nullable().Default("False");
            HasManyToMany(x => x.Usuarios).Table("pesquisas_opiniao_usuarios")
                .ParentKeyColumn("id_pesquisa_opiniao")
                .ChildKeyColumn("id_usuario")
                .ForeignKeyConstraintNames("fk_pesquisas_opiniao_usuarios_pesquisas_opiniao", "fk_pesquisas_opiniao_usuarios_usuarios")
                .LazyLoad();
            HasMany(x => x.Respostas).KeyColumn("id_pesquisa_opiniao")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
