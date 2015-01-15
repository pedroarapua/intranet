using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ArquivoMap : ClassMap<ArquivoVO>
    {
        public ArquivoMap()
        {
            Table("arquivos");
            Id(x => x.Id);
            References(x => x.Tipo)
                .Column("id_tipo_arquivo")
                .LazyLoad()
                .Not.Nullable();
            Map(x => x.Nome).Column("nome").Length(200).Not.Nullable();
            Map(x => x.Descricao).Column("descricao").Length(500).Nullable();
            Map(x => x.NomeOriginal).Column("nome_original").Length(50).Not.Nullable();
            Map(x => x.Extensao).Column("extensao").Length(20).Not.Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
            Where("Removido = 'false'");
        }
    }
}
