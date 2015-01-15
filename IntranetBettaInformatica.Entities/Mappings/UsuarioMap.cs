using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using IntranetBettaInformatica.Entities.Entities;

namespace IntranetBettaInformatica.Entities.Mappings
{
    public class UsuarioMap:ClassMap<UsuarioVO>
    {
        public UsuarioMap()
        {
            Table("usuarios");
            Id(x => x.Id);
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Login).Length(100);
            Map(x => x.Senha).Length(100);
            Map(x => x.UsuarioSistema).Column("usuario_sistema").Not.Nullable().Default("False");
            Map(x => x.DataNascimento).Column("data_nascimento");
            Map(x => x.ExtensaoFoto).Length(10).Column("extensao_foto");
            Map(x => x.Endereco).Length(200);
            Map(x => x.Email).Length(100);
            Map(x => x.Cidade).Length(100);
            Map(x => x.PalavraChave).Length(100);
            Map(x => x.Twitter).Length(50);
            Map(x => x.Removido).Not.Nullable().Default("False");
            Map(x => x.RemovidoContato).Column("removido_contato").Not.Nullable().Default("False");
            References(x => x.Tema)
                .Column("id_tema")
                .ForeignKey("fk_usuarios_temas")
                .LazyLoad();
            References(x => x.PaginaInicial)
                .Column("id_pagina_inicial")
                .ForeignKey("fk_usuarios_paginas")
                .LazyLoad();
            References(x => x.Estado)
                .Column("id_estado")
                .ForeignKey("fk_usuarios_estados")
                .LazyLoad();
            References(x => x.Empresa)
                .Column("id_empresa")
                .ForeignKey("fk_usuarios_empresas")
                .LazyLoad();
            References(x => x.Setor)
                .Column("id_setor")
                .ForeignKey("fk_usuarios_setores")
                .LazyLoad();
            References(x => x.PerfilAcesso)
                .Column("id_perfil_acesso")
                .ForeignKey("fk_usuarios_perfis_acesso")
                .LazyLoad();
			References(x => x.Funcao)
				.Column("id_funcao")
				.ForeignKey("fk_usuarios_funcoes")
				.LazyLoad();
            HasManyToMany(x => x.Sistemas).Table("usuarios_sistemas")
                .ParentKeyColumn("id_usuario")
                .ChildKeyColumn("id_sistema")
                .ForeignKeyConstraintNames("fk_usuarios_sistemas_usuarios","fk_usuarios_sistemas_sistemas")
                .Not.LazyLoad();
            HasMany(x => x.Telefones).KeyColumn("id_usuario")
                .Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();
            HasMany(x => x.Paginas).KeyColumn("id_usuario")
                .Inverse()
                .LazyLoad()
                .Cascade.None();
			HasMany(x => x.MensagensEnviadas).KeyColumn("id_usuario_envio")
                .Inverse()
                .LazyLoad()
                .Not.Cascade.All()
                .Where("Removido = 'false'")
                .OrderBy("Data desc");
            HasMany(x => x.MensagensRecebidas).KeyColumn("id_usuario_destino")
                .Inverse()
                .LazyLoad()
                .Where("Removido = 'false'")
                .Not.Cascade.All();
            HasMany(x => x.MensagensRecebidasNaoLidas).KeyColumn("id_usuario_destino")
                .Inverse()
                .LazyLoad()
                .Where("Removido = 'false'")
                .Not.Cascade.All().Where("lido_mensagem = 'false'");
            HasMany(x => x.PontosUsuario).KeyColumn("id_usuario")
               .Inverse()
               .LazyLoad()
               .Where("Data = '"+DateTime.Today+"' and Removido = 'false'")
               .Not.Cascade.All();
        }
    }
}
