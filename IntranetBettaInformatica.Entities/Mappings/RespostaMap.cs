using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class RespostaMap : ClassMap<RespostaVO>
    {
        public RespostaMap()
        {
            Table("resposta");
            Id(x => x.Id);
            Map(x => x.Descricao).Length(200);
            References(x => x.Pesquisa)
                .Column("id_pesquisa_opiniao")
                .ForeignKey("fk_respostas_pesquisas_opiniao")
                .LazyLoad();
            HasManyToMany(x => x.Usuarios).Table("respostas_usuarios")
                .ParentKeyColumn("id_resposta")
                .ChildKeyColumn("id_usuario")
                .ForeignKeyConstraintNames("fk_respostas_usuarios_respostas", "fk_respostas_usuarios_usuarios")
                .LazyLoad();
        }
    }
}
