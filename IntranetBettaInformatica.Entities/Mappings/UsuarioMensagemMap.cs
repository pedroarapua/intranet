using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class UsuarioMensagemMap : ClassMap<UsuarioMensagemVO>
    {
        public UsuarioMensagemMap()
        {
            Table("usuarios_mensagens");
            Id(x => x.Id);
            References(x => x.UsuarioRecMens).Not.Nullable().Column("id_usuario_destino").ForeignKey("fk_usuarios_mensagens_usuarios");
            References(x => x.Mensagem).Not.Nullable().Column("id_mensagem").ForeignKey("fk_usuarios_mensagens_mensagens");
            Map(x => x.LidoMensagem).Column("lido_mensagem").Default("true").Not.Nullable();
            Map(x => x.Removido).Column("removido").Default("false").Not.Nullable();
            Where("Removido = 'false'");
        }
    }
}
