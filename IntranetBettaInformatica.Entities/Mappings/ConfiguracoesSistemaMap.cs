using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ConfiguracoesSistemaMap:ClassMap<ConfiguracoesSistemaVO>
    {
        public ConfiguracoesSistemaMap()
        {
            Table("configuracoes_sistema");
            Id(x => x.Id);
            Map(x => x.ExtensaoImagem).Column("extensao_imagem").Length(10);
            Map(x => x.Descricao).Column("descricao_logo").Length(100).Not.Nullable();
            Map(x => x.ServidoSmtp).Column("servidor_smtp").Length(50);
            Map(x => x.Login).Length(50);
            Map(x => x.Senha).Length(100);
        }
    }
}
