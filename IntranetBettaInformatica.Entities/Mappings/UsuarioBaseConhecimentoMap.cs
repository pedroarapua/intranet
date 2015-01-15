using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class UsuarioBaseConhecimentoMap:ClassMap<UsuarioBaseConhecimentoVO>
    {
		public UsuarioBaseConhecimentoMap()
        {
            Table("usuarios_conhecimentos");
            Id(x => x.Id);
            Map(x => x.Comprovavel).Not.Nullable().Default("False");
			Map(x => x.NivelConhecimento).Column("nivel_conhecimento").CustomType<int>().Not.Nullable().Default("1");
			References(x => x.Conhecimento)
			  .Column("id_base_conhecimento")
			  .ForeignKey("fk_usuarios_conhecimentos_base_conhecimentos")
			  .LazyLoad();
			References(x => x.Usuario)
			  .Column("id_usuario")
			  .ForeignKey("fk_usuarios_usuarios_conhecimentos")
			  .LazyLoad();
        }
    }
}
