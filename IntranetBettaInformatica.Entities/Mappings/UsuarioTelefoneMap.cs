using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class UsuarioTelefoneMap:ClassMap<UsuarioTelefoneVO>
    {
        public UsuarioTelefoneMap()
        {
            Table("usuarios_telefones");
            Id(x => x.Id);
            Map(x => x.Telefone).Column("telefone").Length(14).Not.Nullable();
            References(x => x.Usuario)
                .Column("id_usuario")
                .LazyLoad()
                .Not.Nullable();
        }
    }
}
