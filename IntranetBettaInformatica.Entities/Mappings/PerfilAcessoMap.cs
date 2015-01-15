using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class PerfilAcessoMap:ClassMap<PerfilAcessoVO>
    {
        public PerfilAcessoMap()
        {
            Table("perfis_acesso");
            Id(x => x.Id);
            Map(x => x.Nome).Length(50).Not.Nullable();
            Map(x => x.Descricao).Length(100);
            Map(x => x.EModerador).Column("perfil_moderador").Not.Nullable().Default("False");
           HasManyToMany(x => x.Acoes)
				.Table("perfis_acesso_acoes_paginas")
				.ParentKeyColumn("id_perfil_acesso")
				.ChildKeyColumn("id_acao_pagina")
				.AsBag()
				.LazyLoad();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
