using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class MensagemMap : ClassMap<MensagemVO>
    {
        public MensagemMap()
        {
            Table("mensagens");
            Id(x => x.Id);
            Map(x => x.Data).Column("data").Not.Nullable();
            Map(x => x.Descricao).Column("descricao").Length(2000).Not.Nullable();
            Map(x => x.ConfirmarLeitura).Default("false").Not.Nullable();
            Map(x => x.Removido).Column("removido").Default("false").Not.Nullable();
            References(x => x.UsuarioEnvio).Not.Nullable().Column("id_usuario_envio").ForeignKey("fk_mensagens_usuarios");
            HasMany(x => x.UsuariosMensagens).KeyColumn("id_mensagem")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();
        }
    }
}
