using Medicare_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Medicare_API.Data
{
      public class DataContext : DbContext
      {
            public DbSet<Utilizador> Utilizadores { get; set; }
            public DbSet<TipoUtilizador> TiposUtilizadores { get; set; }
            public DbSet<TipoParentesco> TiposParentesco { get; set; }
            public DbSet<Responsavel> Responsaveis { get; set; }
            public DbSet<Cuidador> Cuidadores { get; set; }
            public DbSet<Parceiro> Parceiros { get; set; }
            public DbSet<UtilizadorTipoUtilizador> UtilizadoresTiposUtilizadores { get; set; }
            public DbSet<ParceiroUtilizador> ParceirosUtilizadores { get; set; }
            public DbSet<TipoGrandeza> TiposGrandeza { get; set; }
            public DbSet<TipoFarmaceutico> TiposFarmaceutico { get; set; }
            public DbSet<TipoAgendamento> TiposAgendamento { get; set; }
            public DbSet<Remedio> Remedios { get; set; }
            public DbSet<Posologia> Posologias { get; set; }
            public DbSet<Horario> Horarios { get; set; }
            public DbSet<Alarme> Alarmes { get; set; }
            public DbSet<FormaPagamento> FormasPagamento { get; set; }
            public DbSet<Promocao> Promocoes { get; set; }

            public DbSet<SolicitacoesVinculo> SolicitacoesVinculos { get; set; }
            public DbSet<Soneca> Sonecas { get; set; }


            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  #region Utilizadores
                  modelBuilder.Entity<Utilizador>(entity =>
                  {
                  entity.ToTable("Utilizadores");
                  entity.HasKey(e => e.IdUtilizador);

                  entity.Property(e => e.IdUtilizador);

                  entity.Property(e => e.Nome)
                        .HasColumnName("nmUtilizador")
                        .HasMaxLength(40)
                        .IsRequired();

                  entity.Property(e => e.Sobrenome)
                        .HasColumnName("sbUtilizador")
                        .HasMaxLength(40)
                        .IsRequired();

                  entity.Property(e => e.CPF)
                        .HasColumnName("cpfUtilizador")
                        .HasMaxLength(11)
                        .IsFixedLength()
                        .IsRequired();

                  entity.Property(e => e.Email)
                        .HasColumnName("emUtilizador")
                        .HasMaxLength(255)
                        .IsRequired();

                  entity.Property(e => e.Telefone)
                        .HasColumnName("telUtilizador")
                        .HasMaxLength(11)
                        .IsFixedLength()
                        .IsRequired();

                  entity.Property(e => e.SenhaHash)
                        .HasColumnName("senhaHash")
                        .HasMaxLength(64)
                        .IsRequired();

                  entity.Property(e => e.SenhaSalt)
                        .HasColumnName("senhaSalt")
                        .HasMaxLength(128)
                        .IsRequired();

                  entity.Property(e => e.Username)
                        .HasColumnName("userUtilizador")
                        .HasMaxLength(30)
                        .IsRequired();

                  entity.Property(e => e.PasswordResetToken)
                        .HasColumnName("senhaResetToken")
                        .HasColumnType("NVARCHAR(255)");

                  entity.Property(e => e.PasswordResetTokenExpiration)
                        .HasColumnName("senhaResetTokenExpiration")
                        .HasColumnType("datetime");

                        entity.HasIndex(e => e.CPF).IsUnique();
                  entity.HasIndex(e => e.Username).IsUnique();
            });
                  #endregion

                  #region TiposUtilizadores
                  modelBuilder.Entity<TipoUtilizador>(entity =>
                  {
                        entity.ToTable("TiposUtilizadores");
                        entity.HasKey(e => e.IdTipoUtilizador);

                        entity.Property(e => e.IdTipoUtilizador);

                        entity.Property(e => e.Descricao)
                              .HasColumnName("dsTipoUtilizador")
                              .HasMaxLength(14)
                              .IsRequired();
      });
                  #endregion

                  #region TiposParentesco
                  modelBuilder.Entity<TipoParentesco>(entity =>
                  {
                        entity.ToTable("TiposParentesco");
                        entity.HasKey(e => e.IdTipoParentesco);

                        entity.Property(e => e.IdTipoParentesco);

                        entity.Property(e => e.Descricao)
                              .HasColumnName("dsParentesco")
                              .HasMaxLength(20)
                              .IsRequired();
});
#endregion

#region Responsaveis
modelBuilder.Entity<Responsavel>(entity =>
{
      entity.ToTable("Responsaveis");
      entity.HasKey(e => new { e.IdResponsavel, e.IdPaciente });

      entity.Property(e => e.IdTipoParentesco)
            .ValueGeneratedNever();

      entity.Property(e => e.DataCriacao).HasColumnName("dcResponsabilidade").IsRequired();
      entity.Property(e => e.DataAtualizacao).HasColumnName("duResponsabilidade").IsRequired();
      entity.Property(e => e.Status).HasColumnName("stResponsabilidade").HasMaxLength(1).IsRequired();

      entity.HasOne(d => d.ResponsavelUser).WithMany().HasForeignKey(d => d.IdResponsavel).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.IdPaciente).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(d => d.TipoParentesco).WithMany().HasForeignKey(d => d.IdTipoParentesco);
});
#endregion

#region Cuidadores
modelBuilder.Entity<Cuidador>(entity =>
{
      entity.ToTable("Cuidadores");
      entity.HasKey(e => new { e.IdCuidador, e.IdPaciente });

      entity.Property(e => e.IdCuidador)
            .ValueGeneratedNever();

      entity.Property(e => e.IdPaciente)
            .ValueGeneratedNever();

      entity.Property(e => e.DataInicio).HasColumnName("dtInicio").IsRequired();
      entity.Property(e => e.DataFim).HasColumnName("dtFim").IsRequired();
      entity.Property(e => e.DataCriacao).HasColumnName("dcCuidado").IsRequired();
      entity.Property(e => e.DataAtualizacao).HasColumnName("duCuidado").IsRequired();
      entity.Property(e => e.Status).HasColumnName("stCuidado").HasMaxLength(1).IsRequired();

      entity.HasOne(d => d.CuidadorUser).WithMany().HasForeignKey(d => d.IdCuidador).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.IdPaciente).OnDelete(DeleteBehavior.Restrict);
});
#endregion

#region Parceiros
modelBuilder.Entity<Parceiro>(entity =>
{
      entity.ToTable("Parceiros");
      entity.HasKey(e => e.IdParceiro);

      entity.Property(e => e.IdParceiro)
            .ValueGeneratedNever();

      entity.Property(e => e.Nome).HasColumnName("nmParceiro").HasMaxLength(50).IsRequired();
      entity.Property(e => e.Apelido).HasColumnName("apParceiro").HasMaxLength(25).IsRequired();
      entity.Property(e => e.CNPJ).HasColumnName("cnpjParceiro").HasMaxLength(18).IsRequired();
      entity.Property(e => e.Status).HasColumnName("stParceiro").HasMaxLength(1).IsRequired();

      entity.HasIndex(e => e.CNPJ).IsUnique();
});
#endregion

#region Utilizadores_TiposUtilizadores
modelBuilder.Entity<UtilizadorTipoUtilizador>(entity =>
{
      entity.ToTable("Utilizadores_TiposUtilizadores");
      entity.HasKey(e => new { e.IdUtilizador, e.IdTipoUtilizador });

      entity.Property(e => e.IdUtilizador)
            .ValueGeneratedNever();

      entity.Property(e => e.IdTipoUtilizador)
            .ValueGeneratedNever();

      entity.HasOne(e => e.Utilizador)
            .WithMany(u => u.TiposUtilizadores)
            .HasForeignKey(e => e.IdUtilizador);

      entity.HasOne(e => e.TipoUtilizador)
            .WithMany(t => t.Utilizadores)
            .HasForeignKey(e => e.IdTipoUtilizador);


});
#endregion

#region Parceiros_Utilizadores
modelBuilder.Entity<ParceiroUtilizador>(entity =>
{
      entity.ToTable("Parceiros_Utilizadores");
      entity.HasKey(e => new { e.IdParceiro, e.IdUtilizador });

      entity.Property(e => e.IdParceiro)
            .ValueGeneratedNever();

      entity.Property(e => e.IdUtilizador)
            .ValueGeneratedNever();

      entity.HasOne(pu => pu.Parceiro)
            .WithMany(p => p.ParceirosUtilizadores)
            .HasForeignKey(pu => pu.IdParceiro);

      entity.HasOne(pu => pu.Utilizador)
            .WithMany()
            .HasForeignKey(pu => pu.IdUtilizador);
});
#endregion

#region TiposGrandeza
modelBuilder.Entity<TipoGrandeza>(entity =>
{
      entity.ToTable("TiposGrandeza");

      entity.HasKey(e => e.IdTipoGrandeza);

      entity.Property(e => e.IdTipoGrandeza)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.Descricao)
            .HasColumnName("dsGrandeza")
            .HasMaxLength(20)
            .IsRequired();
});
#endregion

#region TiposFarmaceutico
modelBuilder.Entity<TipoFarmaceutico>(entity =>
{
      entity.ToTable("TiposFarmaceutico");

      entity.HasKey(e => e.IdTipoFarmaceutico);

      entity.Property(e => e.IdTipoFarmaceutico)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.Descricao)
            .HasColumnName("decricao")
            .HasMaxLength(40)
            .IsRequired();
});
#endregion

#region TiposAgendamento
modelBuilder.Entity<TipoAgendamento>(entity =>
{
      entity.ToTable("TiposAgendamento");

      entity.HasKey(e => e.IdTipoAgendamento);

      entity.Property(e => e.IdTipoAgendamento)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.Descricao)
            .HasMaxLength(40)
            .IsRequired();
});
#endregion

#region Remedios
modelBuilder.Entity<Remedio>(entity =>
{
      entity.ToTable("Remedios");

      entity.HasKey(e => e.IdRemedio);

      entity.Property(e => e.IdRemedio)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.Nome)
            .HasMaxLength(100)
            .IsRequired();

      entity.Property(e => e.DataCriacao)
            .IsRequired();

      entity.Property(e => e.DataAtualizacao)
            .IsRequired();

      entity.HasMany(e => e.Promocoes)
            .WithOne(p => p.Remedio)
            .HasForeignKey(p => p.IdRemedio)
            .OnDelete(DeleteBehavior.Cascade);
});

#endregion

#region Posologias
modelBuilder.Entity<Posologia>(entity =>
{
      entity.ToTable("Posologias");

      entity.HasKey(e => e.IdPosologia);

      entity.Property(e => e.IdPosologia)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.QtdeDose)
            .HasColumnName("qtdeDose")
            .IsRequired();

      entity.Property(e => e.QtdeConcentracao)
            .HasColumnName("qtdeConcentracao")
            .IsRequired();

      entity.Property(e => e.DataInicio)
            .HasColumnName("dtInicio")
            .IsRequired();

      entity.Property(e => e.DataFim)
            .HasColumnName("dtFim")
            .IsRequired();

      entity.Property(e => e.Intervalo)
            .HasColumnName("intervalo")
            .HasMaxLength(20);

      entity.Property(e => e.DiasSemana)
            .HasColumnName("diasSemana")
            .HasMaxLength(16)
            .IsRequired();

      entity.Property(e => e.DiasUso)
            .HasColumnName("diasUso")
            .IsRequired();

      entity.Property(e => e.DiasPausa)
            .HasColumnName("diasPausa")
            .IsRequired();

      entity.Property(e => e.Observacao)
            .HasColumnName("observacao")
            .HasMaxLength(255);

      // Relacionamentos via FK
      entity.HasOne<Remedio>()
            .WithMany()
            .HasForeignKey(p => p.IdRemedio)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne<Utilizador>()
            .WithMany()
            .HasForeignKey(p => p.IdUtilizador)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne<TipoFarmaceutico>()
            .WithMany()
            .HasForeignKey(p => p.IdTipoFarmaceutico)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne<TipoGrandeza>()
            .WithMany()
            .HasForeignKey(p => p.IdTipoGrandeza)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne<TipoAgendamento>()
            .WithMany()
            .HasForeignKey(p => p.IdTipoAgendamento)
            .OnDelete(DeleteBehavior.Restrict);
});
#endregion

#region Horarios
modelBuilder.Entity<Horario>(entity =>
{
      entity.ToTable("Horarios");

      entity.HasKey(e => new { e.IdPosologia, e.Hora });

      entity.Property(e => e.IdPosologia)
            .IsRequired()
            .ValueGeneratedNever();

      entity.Property(e => e.Hora)
            .HasColumnName("horario")
            .IsRequired()
            .ValueGeneratedNever();

      entity.HasOne(h => h.Posologia)
            .WithMany(p => p.Horarios)
            .HasForeignKey(h => h.IdPosologia)
            .OnDelete(DeleteBehavior.Cascade);
});
#endregion

#region Alarmes

modelBuilder.Entity<Alarme>(entity =>
{
      entity.ToTable("Alarmes");

      entity.HasKey(e => e.IdAlarme);

      entity.Property(e => e.IdAlarme)
            .ValueGeneratedOnAdd();

      entity.Property(e => e.ContadorSoneca)
            .HasColumnName("ContadorSoneca")
            .HasColumnType("char(1)")
            .HasDefaultValue(0)
            .IsRequired();

      entity.Property(e => e.DataHora)
            .HasColumnName("dtHoraAlarme")
            .HasColumnType("datetime")
            .IsRequired();

      entity.Property(e => e.Status)
            .HasColumnName("stAlarme")
            .HasColumnType("char(1)")
            .HasDefaultValue('P')
            .HasMaxLength(1)
            .IsRequired();

      entity.HasOne<Posologia>()
            .WithMany()
            .HasForeignKey(e => e.IdPosologia)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Alarmes_Posologia");
});
#endregion

#region Sonecas
modelBuilder.Entity<Soneca>(entity =>
{
      entity.ToTable("Sonecas");

      entity.HasKey(e => e.IdPosologia); // Se for chave primária

      entity.Property(e => e.StSoneca)
            .HasColumnName("stSoneca")
            .HasColumnType("char(1)")
            .HasDefaultValue('A')
            .IsRequired();

      entity.Property(e => e.IntervaloMinutos)
            .HasColumnName("intervaloMinutos")
            .HasDefaultValue(5)
            .IsRequired();

      entity.Property(e => e.MaxSoneca)
            .HasColumnName("maxSoneca")
            .HasDefaultValue(3);

      entity.Property(e => e.DcSoneca)
            .HasColumnName("dcSoneca")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

      entity.Property(e => e.DuSoneca)
            .HasColumnName("duSoneca")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

      entity.HasOne(e => e.Posologia)
            .WithMany()
            .HasForeignKey(e => e.IdPosologia)
            .HasConstraintName("FK_Sonecas_2")
            .OnDelete(DeleteBehavior.Restrict); // Ou outro comportamento conforme a tua lógica
});
#endregion

#region FormasPagamento
modelBuilder.Entity<FormaPagamento>(entity =>
{
      entity.ToTable("FormasPagamento");
      entity.HasKey(e => e.IdFormaPagamento);

      entity.Property(e => e.IdFormaPagamento);

      entity.Property(e => e.Descricao)
          .HasColumnName("dsFormaPagamento")
          .HasMaxLength(45)
          .IsRequired();

      entity.Property(e => e.QtdeParcelas)
          .HasColumnName("qtdeParcelas")
          .IsRequired();

      entity.Property(e => e.QtdeMinParcelas)
          .HasColumnName("qtdeMinParcelas")
          .IsRequired();

      entity.Property(e => e.QtdeMaxParcelas)
          .HasColumnName("qtdeMaxParcelas")
          .IsRequired();
});
#endregion

#region Promocoes
modelBuilder.Entity<Promocao>(entity =>
{
      entity.ToTable("Promocoes");
      entity.HasKey(e => e.IdPromocao);

      entity.Property(e => e.IdPromocao);

      entity.Property(e => e.Descricao)
          .HasColumnName("dsPromocao")
          .HasMaxLength(50)
          .IsRequired();

      entity.Property(e => e.DataInicio)
          .HasColumnName("dtInicio")
          .IsRequired();

      entity.Property(e => e.DataFim)
          .HasColumnName("dtFim")
          .IsRequired();

      entity.Property(e => e.Valor)
          .HasColumnName("vlrPromocao")
          .HasColumnType("decimal(18,2)")
          .IsRequired();

      entity.HasOne(p => p.FormaPagamento)
          .WithMany()
          .HasForeignKey(p => p.IdFormaPagamento);

      entity.HasOne(p => p.Utilizador)
          .WithMany()
          .HasForeignKey(p => p.IdUtilizador);

      entity.HasOne(p => p.Remedio)
          .WithMany()
          .HasForeignKey(p => p.IdRemedio);
});
#endregion

#region Solicitacoes
modelBuilder.Entity<SolicitacoesVinculo>(entity =>
{
      entity.ToTable("Solicitacoes");
      entity.HasKey(sv => sv.IdSolicitacao);

      entity.HasOne(sv => sv.TipoSolicitante)
          .WithMany()
          .HasForeignKey(sv => new { sv.IdSolicitante, sv.IdTipoSolicitante })
          .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(sv => sv.TipoReceptor)
          .WithMany()
          .HasForeignKey(sv => new { sv.IdReceptor, sv.IdTipoReceptor })
          .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(sv => sv.Solicitante)
          .WithMany()
          .HasForeignKey(sv => sv.IdSolicitante)
          .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(sv => sv.Receptor)
          .WithMany()
          .HasForeignKey(sv => sv.IdReceptor)
          .OnDelete(DeleteBehavior.Restrict);
});
                  #endregion




            }
      }
}


