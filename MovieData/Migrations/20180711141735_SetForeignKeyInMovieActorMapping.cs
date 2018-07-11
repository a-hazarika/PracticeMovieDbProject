using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieData.Migrations
{
    public partial class SetForeignKeyInMovieActorMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Actors_ActorId",
                table: "MovieActorMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Movies_MovieId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActorMappings_Actors_ActorId",
                table: "MovieActorMappings",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActorMappings_Movies_MovieId",
                table: "MovieActorMappings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Actors_ActorId",
                table: "MovieActorMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieActorMappings_Movies_MovieId",
                table: "MovieActorMappings");

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
    }
}
