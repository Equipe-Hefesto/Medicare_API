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

                        entity.Property(e => e.IdUtilizador)
                              .ValueGeneratedNever();

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

                        entity.HasIndex(e => e.CPF).IsUnique();
                        entity.HasIndex(e => e.Username).IsUnique();
                  });
                  #endregion

                  #region TiposUtilizadores
                  modelBuilder.Entity<TipoUtilizador>(entity =>
                  {
                        entity.ToTable("TiposUtilizadores");
                        entity.HasKey(e => e.IdTipoUtilizador);

                        entity.Property(e => e.IdTipoUtilizador)
                              .ValueGeneratedNever();

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

                        entity.Property(e => e.IdTipoParentesco)
                              .ValueGeneratedNever();

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
                              .ValueGeneratedNever();

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
                              .ValueGeneratedNever();

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
                              .ValueGeneratedNever();

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
                              .ValueGeneratedNever();

                        entity.Property(e => e.Nome).HasColumnName("nmRemedio").HasMaxLength(40).IsRequired();
                        entity.Property(e => e.DataCriacao).HasColumnName("dcRemedio").IsRequired();
                        entity.Property(e => e.DataAtualizacao).HasColumnName("duRemedio").IsRequired();
                  });

                  #endregion

                  #region Posologias
                  modelBuilder.Entity<Posologia>(entity =>
                  {
                        entity.ToTable("Posologias");
                        entity.HasKey(e => e.IdPosologia);

                        entity.Property(e => e.IdPosologia)
                              .ValueGeneratedNever();

                        entity.Property(e => e.Quantidade).HasColumnName("qtdePosologia").IsRequired();
                        entity.Property(e => e.QuantidadeDose).HasColumnName("qtdeDose").IsRequired();
                        entity.Property(e => e.DataInicio).HasColumnName("diPosologia").IsRequired();
                        entity.Property(e => e.DataFim).HasColumnName("dfPosologia").IsRequired();
                        entity.Property(e => e.Intervalo).HasColumnName("intervalo").IsRequired();
                        entity.Property(e => e.DiasSemana).HasColumnName("diasSemana").HasMaxLength(16).IsRequired();
                        entity.Property(e => e.DiasUso).HasColumnName("diasUso").IsRequired();
                        entity.Property(e => e.DiasPausa).HasColumnName("diasPausa").IsRequired();

                        entity.HasOne(p => p.Remedio).WithMany().HasForeignKey(p => p.IdRemedio);
                        entity.HasOne(p => p.Utilizador).WithMany().HasForeignKey(p => p.IdUtilizador);
                        entity.HasOne(p => p.TipoFarmaceutico).WithMany().HasForeignKey(p => p.IdTipoFarmaceutico);
                        entity.HasOne(p => p.TipoGrandeza).WithMany().HasForeignKey(p => p.IdTipoGrandeza);
                        entity.HasOne(p => p.TipoAgendamento).WithMany().HasForeignKey(p => p.IdTipoAgendamento);
                  });
                  #endregion

                  #region Horarios
                  modelBuilder.Entity<Horario>(entity =>
                  {
                        entity.ToTable("Horarios");

                        entity.HasKey(e => new { e.IdPosologia, e.Hora }); // Corrigido

                        entity.Property(e => e.IdPosologia)
                              .IsRequired()
                              .ValueGeneratedNever();

                        entity.Property(e => e.Hora)
                              .HasColumnName("horario")
                              .IsRequired()
                              .ValueGeneratedNever();

                        entity.HasOne(h => h.Posologia)
                              .WithMany(p => p.Horarios)
                              .HasForeignKey(h => h.IdPosologia);
                  });
                  #endregion

                  #region Alarmes
                  modelBuilder.Entity<Alarme>(entity =>
                  {
                        entity.ToTable("Alarmes");
                        entity.HasKey(e => e.IdAlarme);

                        entity.Property(e => e.IdAlarme)
                              .ValueGeneratedNever();

                        entity.Property(e => e.DataHora).HasColumnName("dtHoraAlarme").IsRequired();
                        entity.Property(e => e.Descricao).HasColumnName("dsAlarme").HasMaxLength(40).IsRequired();
                        entity.Property(e => e.Status).HasColumnName("stAlarme").HasMaxLength(1).IsRequired();

                        entity.HasOne(a => a.Posologia).WithMany().HasForeignKey(a => a.IdPosologia);
                  });
                  #endregion

                  #region FormasPagamento
                  modelBuilder.Entity<FormaPagamento>(entity =>
                  {
                        entity.ToTable("FormasPagamento");
                        entity.HasKey(e => e.IdFormaPagamento);

                        entity.Property(e => e.IdFormaPagamento)
                              .ValueGeneratedNever();

                        entity.Property(e => e.Descricao).HasColumnName("dsFormaPagamento").HasMaxLength(45).IsRequired();
                        entity.Property(e => e.QtdeParcelas).HasColumnName("qtdeParcelas").IsRequired();
                        entity.Property(e => e.QtdeMinParcelas).HasColumnName("qtdeMinParcelas").IsRequired();
                        entity.Property(e => e.QtdeMaxParcelas).HasColumnName("qtdeMaxParcelas").IsRequired();
                  });
                  #endregion

                  #region Promocoes
                  modelBuilder.Entity<Promocao>(entity =>
                  {
                        entity.ToTable("Promocoes");
                        entity.HasKey(e => e.IdPromocao);

                        entity.Property(e => e.IdPromocao)
                              .ValueGeneratedNever();

                        entity.Property(e => e.Descricao).HasColumnName("dsPromocao").HasMaxLength(50).IsRequired();
                        entity.Property(e => e.DataInicio).HasColumnName("dtInicio").IsRequired();
                        entity.Property(e => e.DataFim).HasColumnName("dtFim").IsRequired();
                        entity.Property(e => e.Valor).HasColumnName("vlrPromocao");

                        entity.HasOne(p => p.FormaPagamento).WithMany().HasForeignKey(p => p.IdFormaPagamento);
                        entity.HasOne(p => p.Utilizador).WithMany().HasForeignKey(p => p.IdUtilizador);
                        entity.HasOne(p => p.Remedio).WithMany().HasForeignKey(p => p.IdRemedio);
                  });
                  #endregion
            }
      }
}

