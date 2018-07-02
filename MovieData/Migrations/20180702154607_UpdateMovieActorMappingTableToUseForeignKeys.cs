using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieData.Migrations
{
    public partial class UpdateMovieActorMappingTableToUseForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MovieActorMappings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ActorId",
                table: "MovieActorMappings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_MovieActorMappings_ActorId",
                table: "MovieActorMappings",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActorMappings_MovieId",
                table: "MovieActorMappings",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActorMappings_Actors_ActorId",
                table: "MovieActorMappings",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActorMappings_Movies_MovieId",
                table: "MovieActorMappings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Actors_ActorId",
                table: "MovieActorMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Movies_MovieId",
                table: "MovieActorMappings");

            migrationBuilder.DropIndex(
                name: "IX_MovieActorMappings_ActorId",
                table: "MovieActorMappings");

            migrationBuilder.DropIndex(
                name: "IX_MovieActorMappings_MovieId",
                table: "MovieActorMappings");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MovieActorMappings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActorId",
                table: "MovieActorMappings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
