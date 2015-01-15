using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class CertificacaoUsuarioMap:ClassMap<CertificacaoUsuarioVO>
    {
		public CertificacaoUsuarioMap()
        {
            Table("certificacoes_usuarios");
            Id(x => x.Id);
            Map(x => x.Certificacao).Length(500).Not.Nullable();
			Map(x => x.Orgao).Length(500).Not.Nullable();
			References(x => x.Usuario)
			  .Column("id_usuario")
			  .ForeignKey("fk_usuarios_certificacoes_usuarios")
			  .LazyLoad();
        }
    }
}
