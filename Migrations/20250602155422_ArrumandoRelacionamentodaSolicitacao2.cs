using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medicare_API.Migrations
{
    /// <inheritdoc />
    public partial class ArrumandoRelacionamentodaSolicitacao2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacoesVinculos_TiposUtilizadores_IdTipoReceptor",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacoesVinculos_TiposUtilizadores_IdTipoSolicitante",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdReceptor",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdSolicitante",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdTipoReceptor",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdTipoSolicitante",
                table: "SolicitacoesVinculos");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdReceptor_IdTipoReceptor",
                table: "SolicitacoesVinculos",
                columns: new[] { "IdReceptor", "IdTipoReceptor" });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdSolicitante_IdTipoSolicitante",
                table: "SolicitacoesVinculos",
                columns: new[] { "IdSolicitante", "IdTipoSolicitante" });

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacoesVinculos_Utilizadores_TiposUtilizadores_IdReceptor_IdTipoReceptor",
                table: "SolicitacoesVinculos",
                columns: new[] { "IdReceptor", "IdTipoReceptor" },
                principalTable: "Utilizadores_TiposUtilizadores",
                principalColumns: new[] { "IdUtilizador", "IdTipoUtilizador" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacoesVinculos_Utilizadores_TiposUtilizadores_IdSolicitante_IdTipoSolicitante",
                table: "SolicitacoesVinculos",
                columns: new[] { "IdSolicitante", "IdTipoSolicitante" },
                principalTable: "Utilizadores_TiposUtilizadores",
                principalColumns: new[] { "IdUtilizador", "IdTipoUtilizador" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacoesVinculos_Utilizadores_TiposUtilizadores_IdReceptor_IdTipoReceptor",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacoesVinculos_Utilizadores_TiposUtilizadores_IdSolicitante_IdTipoSolicitante",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdReceptor_IdTipoReceptor",
                table: "SolicitacoesVinculos");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacoesVinculos_IdSolicitante_IdTipoSolicitante",
                table: "SolicitacoesVinculos");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdReceptor",
                table: "SolicitacoesVinculos",
                column: "IdReceptor");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdSolicitante",
                table: "SolicitacoesVinculos",
                column: "IdSolicitante");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdTipoReceptor",
                table: "SolicitacoesVinculos",
                column: "IdTipoReceptor");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesVinculos_IdTipoSolicitante",
                table: "SolicitacoesVinculos",
                column: "IdTipoSolicitante");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacoesVinculos_TiposUtilizadores_IdTipoReceptor",
                table: "SolicitacoesVinculos",
                column: "IdTipoReceptor",
                principalTable: "TiposUtilizadores",
                principalColumn: "IdTipoUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacoesVinculos_TiposUtilizadores_IdTipoSolicitante",
                table: "SolicitacoesVinculos",
                column: "IdTipoSolicitante",
                principalTable: "TiposUtilizadores",
                principalColumn: "IdTipoUtilizador",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
