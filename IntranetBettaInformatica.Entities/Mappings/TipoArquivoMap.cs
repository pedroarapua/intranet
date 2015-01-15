using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class TipoArquivoMap : ClassMap<TipoArquivoVO>
    {
        public TipoArquivoMap()
        {
            Table("tipos_arquivo");
            Id(x => x.Id);
            Map(x => x.Nome).Column("nome").Length(200).Not.Nullable();
            Map(x => x.Removido).Not.Nullable().Default("False");
        }
    }
}
