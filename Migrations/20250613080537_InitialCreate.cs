using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medicare_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormasPagamento",
                columns: table => new
                {
                    IdFormaPagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dsFormaPagamento = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    qtdeParcelas = table.Column<int>(type: "int", nullable: false),
                    qtdeMinParcelas = table.Column<int>(type: "int", nullable: false),
                    qtdeMaxParcelas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagamento", x => x.IdFormaPagamento);
                });

            migrationBuilder.CreateTable(
                name: "Parceiros",
                columns: table => new
                {
                    IdParceiro = table.Column<int>(type: "int", nullable: false),
                    nmParceiro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    apParceiro = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    cnpjParceiro = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    stParceiro = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parceiros", x => x.IdParceiro);
                });

            migrationBuilder.CreateTable(
                name: "Remedios",
                columns: table => new
                {
                    IdRemedio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remedios", x => x.IdRemedio);
                });

            migrationBuilder.CreateTable(
                name: "TiposAgendamento",
                columns: table => new
                {
                    IdTipoAgendamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAgendamento", x => x.IdTipoAgendamento);
                });

            migrationBuilder.CreateTable(
                name: "TiposFarmaceutico",
                columns: table => new
                {
                    IdTipoFarmaceutico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    decricao = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposFarmaceutico", x => x.IdTipoFarmaceutico);
                });

            migrationBuilder.CreateTable(
                name: "TiposGrandeza",
                columns: table => new
                {
                    IdTipoGrandeza = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dsGrandeza = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposGrandeza", x => x.IdTipoGrandeza);
                });

            migrationBuilder.CreateTable(
                name: "TiposParentesco",
                columns: table => new
                {
                    IdTipoParentesco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dsParentesco = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposParentesco", x => x.IdTipoParentesco);
                });

            migrationBuilder.CreateTable(
                name: "TiposUtilizadores",
                columns: table => new
                {
                    IdTipoUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dsTipoUtilizador = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUtilizadores", x => x.IdTipoUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    IdUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nmUtilizador = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    sbUtilizador = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    cpfUtilizador = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    DtNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    emUtilizador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    telUtilizador = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    senhaHash = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: false),
                    senhaSalt = table.Column<byte[]>(type: "varbinary(128)", maxLength: 128, nullable: false),
                    userUtilizador = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    senhaResetToken = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    senhaResetTokenExpiration = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.IdUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "Cuidadores",
                columns: table => new
                {
                    IdCuidador = table.Column<int>(type: "int", nullable: false),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    dtInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dtFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dcCuidado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duCuidado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stCuidado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UtilizadorIdUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuidadores", x => new { x.IdCuidador, x.IdPaciente });
                    table.ForeignKey(
                        name: "FK_Cuidadores_Utilizadores_IdCuidador",
                        column: x => x.IdCuidador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cuidadores_Utilizadores_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cuidadores_Utilizadores_UtilizadorIdUtilizador",
                        column: x => x.UtilizadorIdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Parceiros_Utilizadores",
                columns: table => new
                {
                    IdParceiro = table.Column<int>(type: "int", nullable: false),
                    IdUtilizador = table.Column<int>(type: "int", nullable: false),
                    UtilizadorIdUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parceiros_Utilizadores", x => new { x.IdParceiro, x.IdUtilizador });
                    table.ForeignKey(
                        name: "FK_Parceiros_Utilizadores_Parceiros_IdParceiro",
                        column: x => x.IdParceiro,
                        principalTable: "Parceiros",
                        principalColumn: "IdParceiro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parceiros_Utilizadores_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parceiros_Utilizadores_Utilizadores_UtilizadorIdUtilizador",
                        column: x => x.UtilizadorIdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Posologias",
                columns: table => new
                {
                    IdPosologia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUtilizador = table.Column<int>(type: "int", nullable: false),
                    IdRemedio = table.Column<int>(type: "int", nullable: false),
                    qtdeDose = table.Column<int>(type: "int", nullable: false),
                    IdTipoFarmaceutico = table.Column<int>(type: "int", nullable: false),
                    qtdeConcentracao = table.Column<int>(type: "int", nullable: false),
                    IdTipoGrandeza = table.Column<int>(type: "int", nullable: false),
                    observacao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdTipoAgendamento = table.Column<int>(type: "int", nullable: false),
                    dtInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dtFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    intervalo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    diasSemana = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    diasUso = table.Column<int>(type: "int", nullable: false),
                    diasPausa = table.Column<int>(type: "int", nullable: false),
                    UtilizadorIdUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posologias", x => x.IdPosologia);
                    table.ForeignKey(
                        name: "FK_Posologias_Remedios_IdRemedio",
                        column: x => x.IdRemedio,
                        principalTable: "Remedios",
                        principalColumn: "IdRemedio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posologias_TiposAgendamento_IdTipoAgendamento",
                        column: x => x.IdTipoAgendamento,
                        principalTable: "TiposAgendamento",
                        principalColumn: "IdTipoAgendamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posologias_TiposFarmaceutico_IdTipoFarmaceutico",
                        column: x => x.IdTipoFarmaceutico,
                        principalTable: "TiposFarmaceutico",
                        principalColumn: "IdTipoFarmaceutico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posologias_TiposGrandeza_IdTipoGrandeza",
                        column: x => x.IdTipoGrandeza,
                        principalTable: "TiposGrandeza",
                        principalColumn: "IdTipoGrandeza",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posologias_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posologias_Utilizadores_UtilizadorIdUtilizador",
                        column: x => x.UtilizadorIdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Promocoes",
                columns: table => new
                {
                    IdPromocao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFormaPagamento = table.Column<int>(type: "int", nullable: false),
                    IdUtilizador = table.Column<int>(type: "int", nullable: false),
                    IdRemedio = table.Column<int>(type: "int", nullable: false),
                    dsPromocao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dtInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dtFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vlrPromocao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemedioIdRemedio = table.Column<int>(type: "int", nullable: true),
                    UtilizadorIdUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocoes", x => x.IdPromocao);
                    table.ForeignKey(
                        name: "FK_Promocoes_FormasPagamento_IdFormaPagamento",
                        column: x => x.IdFormaPagamento,
                        principalTable: "FormasPagamento",
                        principalColumn: "IdFormaPagamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Promocoes_Remedios_IdRemedio",
                        column: x => x.IdRemedio,
                        principalTable: "Remedios",
                        principalColumn: "IdRemedio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Promocoes_Remedios_RemedioIdRemedio",
                        column: x => x.RemedioIdRemedio,
                        principalTable: "Remedios",
                        principalColumn: "IdRemedio");
                    table.ForeignKey(
                        name: "FK_Promocoes_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Promocoes_Utilizadores_UtilizadorIdUtilizador",
                        column: x => x.UtilizadorIdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Responsaveis",
                columns: table => new
                {
                    IdResponsavel = table.Column<int>(type: "int", nullable: false),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    IdTipoParentesco = table.Column<int>(type: "int", nullable: false),
                    dcResponsabilidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duResponsabilidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stResponsabilidade = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UtilizadorIdUtilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsaveis", x => new { x.IdResponsavel, x.IdPaciente });
                    table.ForeignKey(
                        name: "FK_Responsaveis_TiposParentesco_IdTipoParentesco",
                        column: x => x.IdTipoParentesco,
                        principalTable: "TiposParentesco",
                        principalColumn: "IdTipoParentesco",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Responsaveis_Utilizadores_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Responsaveis_Utilizadores_IdResponsavel",
                        column: x => x.IdResponsavel,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Responsaveis_Utilizadores_UtilizadorIdUtilizador",
                        column: x => x.UtilizadorIdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores_TiposUtilizadores",
                columns: table => new
                {
                    IdUtilizador = table.Column<int>(type: "int", nullable: false),
                    IdTipoUtilizador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores_TiposUtilizadores", x => new { x.IdUtilizador, x.IdTipoUtilizador });
                    table.ForeignKey(
                        name: "FK_Utilizadores_TiposUtilizadores_TiposUtilizadores_IdTipoUtilizador",
                        column: x => x.IdTipoUtilizador,
                        principalTable: "TiposUtilizadores",
                        principalColumn: "IdTipoUtilizador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilizadores_TiposUtilizadores_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alarmes",
                columns: table => new
                {
                    IdAlarme = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPosologia = table.Column<int>(type: "int", nullable: false),
                    dtHoraAlarme = table.Column<DateTime>(type: "datetime", nullable: false),
                    stAlarme = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false, defaultValue: "P"),
                    ContadorSoneca = table.Column<string>(type: "char(1)", nullable: false, defaultValue: "0"),
                    PosologiaIdPosologia = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarmes", x => x.IdAlarme);
                    table.ForeignKey(
                        name: "FK_Alarmes_Posologia",
                        column: x => x.IdPosologia,
                        principalTable: "Posologias",
                        principalColumn: "IdPosologia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alarmes_Posologias_PosologiaIdPosologia",
                        column: x => x.PosologiaIdPosologia,
                        principalTable: "Posologias",
                        principalColumn: "IdPosologia");
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    IdPosologia = table.Column<int>(type: "int", nullable: false),
                    horario = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => new { x.IdPosologia, x.horario });
                    table.ForeignKey(
                        name: "FK_Horarios_Posologias_IdPosologia",
                        column: x => x.IdPosologia,
                        principalTable: "Posologias",
                        principalColumn: "IdPosologia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sonecas",
                columns: table => new
                {
                    IdPosologia = table.Column<int>(type: "int", nullable: false),
                    stSoneca = table.Column<string>(type: "char(1)", nullable: false, defaultValue: "A"),
                    intervaloMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    maxSoneca = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    dcSoneca = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    duSoneca = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sonecas", x => x.IdPosologia);
                    table.ForeignKey(
                        name: "FK_Sonecas_2",
                        column: x => x.IdPosologia,
                        principalTable: "Posologias",
                        principalColumn: "IdPosologia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSolicitante = table.Column<int>(type: "int", nullable: false),
                    IdTipoSolicitante = table.Column<int>(type: "int", nullable: false),
                    IdReceptor = table.Column<int>(type: "int", nullable: false),
                    IdTipoReceptor = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.IdSolicitacao);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Utilizadores_IdReceptor",
                        column: x => x.IdReceptor,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Utilizadores_IdSolicitante",
                        column: x => x.IdSolicitante,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Utilizadores_TiposUtilizadores_IdReceptor_IdTipoReceptor",
                        columns: x => new { x.IdReceptor, x.IdTipoReceptor },
                        principalTable: "Utilizadores_TiposUtilizadores",
                        principalColumns: new[] { "IdUtilizador", "IdTipoUtilizador" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Utilizadores_TiposUtilizadores_IdSolicitante_IdTipoSolicitante",
                        columns: x => new { x.IdSolicitante, x.IdTipoSolicitante },
                        principalTable: "Utilizadores_TiposUtilizadores",
                        principalColumns: new[] { "IdUtilizador", "IdTipoUtilizador" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alarmes_IdPosologia",
                table: "Alarmes",
                column: "IdPosologia");

            migrationBuilder.CreateIndex(
                name: "IX_Alarmes_PosologiaIdPosologia",
                table: "Alarmes",
                column: "PosologiaIdPosologia");

            migrationBuilder.CreateIndex(
                name: "IX_Cuidadores_IdPaciente",
                table: "Cuidadores",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Cuidadores_UtilizadorIdUtilizador",
                table: "Cuidadores",
                column: "UtilizadorIdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Parceiros_cnpjParceiro",
                table: "Parceiros",
                column: "cnpjParceiro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parceiros_Utilizadores_IdUtilizador",
                table: "Parceiros_Utilizadores",
                column: "IdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Parceiros_Utilizadores_UtilizadorIdUtilizador",
                table: "Parceiros_Utilizadores",
                column: "UtilizadorIdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_IdRemedio",
                table: "Posologias",
                column: "IdRemedio");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_IdTipoAgendamento",
                table: "Posologias",
                column: "IdTipoAgendamento");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_IdTipoFarmaceutico",
                table: "Posologias",
                column: "IdTipoFarmaceutico");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_IdTipoGrandeza",
                table: "Posologias",
                column: "IdTipoGrandeza");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_IdUtilizador",
                table: "Posologias",
                column: "IdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Posologias_UtilizadorIdUtilizador",
                table: "Posologias",
                column: "UtilizadorIdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_IdFormaPagamento",
                table: "Promocoes",
                column: "IdFormaPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_IdRemedio",
                table: "Promocoes",
                column: "IdRemedio");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_IdUtilizador",
                table: "Promocoes",
                column: "IdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_RemedioIdRemedio",
                table: "Promocoes",
                column: "RemedioIdRemedio");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_UtilizadorIdUtilizador",
                table: "Promocoes",
                column: "UtilizadorIdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Responsaveis_IdPaciente",
                table: "Responsaveis",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Responsaveis_IdTipoParentesco",
                table: "Responsaveis",
                column: "IdTipoParentesco");

            migrationBuilder.CreateIndex(
                name: "IX_Responsaveis_UtilizadorIdUtilizador",
                table: "Responsaveis",
                column: "UtilizadorIdUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_IdReceptor_IdTipoReceptor",
                table: "Solicitacoes",
                columns: new[] { "IdReceptor", "IdTipoReceptor" });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_IdSolicitante_IdTipoSolicitante",
                table: "Solicitacoes",
                columns: new[] { "IdSolicitante", "IdTipoSolicitante" });

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_cpfUtilizador",
                table: "Utilizadores",
                column: "cpfUtilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_userUtilizador",
                table: "Utilizadores",
                column: "userUtilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_TiposUtilizadores_IdTipoUtilizador",
                table: "Utilizadores_TiposUtilizadores",
                column: "IdTipoUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarmes");

            migrationBuilder.DropTable(
                name: "Cuidadores");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Parceiros_Utilizadores");

            migrationBuilder.DropTable(
                name: "Promocoes");

            migrationBuilder.DropTable(
                name: "Responsaveis");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Sonecas");

            migrationBuilder.DropTable(
                name: "Parceiros");

            migrationBuilder.DropTable(
                name: "FormasPagamento");

            migrationBuilder.DropTable(
                name: "TiposParentesco");

            migrationBuilder.DropTable(
                name: "Utilizadores_TiposUtilizadores");

            migrationBuilder.DropTable(
                name: "Posologias");

            migrationBuilder.DropTable(
                name: "TiposUtilizadores");

            migrationBuilder.DropTable(
                name: "Remedios");

            migrationBuilder.DropTable(
                name: "TiposAgendamento");

            migrationBuilder.DropTable(
                name: "TiposFarmaceutico");

            migrationBuilder.DropTable(
                name: "TiposGrandeza");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
