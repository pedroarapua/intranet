using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class BaseConhecimentoMap:ClassMap<BaseConhecimentoVO>
    {
		public BaseConhecimentoMap()
        {
            Table("conhecimentos");
            Id(x => x.Id);
            Map(x => x.Titulo).Length(500).Not.Nullable();
			HasMany(x => x.UsuariosConhecimentos).Table("usuarios_conhecimentos")
				.KeyColumn("id_base_conhecimento")
				.ForeignKeyConstraintName("fk_conhecimentos_usuarios_conhecimentos")
				.LazyLoad()
				.Cascade.AllDeleteOrphan();
			References(x => x.Topico)
			  .Column("id_topico_conhecimento")
			  .ForeignKey("fk_conhecimentos_topicos_conhecimentos")
			  .LazyLoad();
        }
    }
}
