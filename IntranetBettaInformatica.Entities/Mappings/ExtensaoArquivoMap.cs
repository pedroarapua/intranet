using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class ExtensaoArquivoMap:ClassMap<ExtensaoArquivoVO>
    {
        public ExtensaoArquivoMap()
        {
            Table("extensoes_arquivos");
            Id(x => x.Id);
            Map(x => x.Extensao).Length(10).Not.Nullable();
            Map(x=> x.TipoExtensao).CustomType<int>().Not.Nullable();
        }
    }
}
