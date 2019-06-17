using Microsoft.EntityFrameworkCore.Migrations;

namespace MyIMDB.Data.Migrations
{
    public partial class MergeWatchListwithRatesintoUserMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "WatchLaterMovie");

            migrationBuilder.DropColumn(
                name: "AverageRate",
                table: "Movie");

            migrationBuilder.AddColumn<long>(
                name: "RatesCount",
                table: "Movie",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RatesSum",
                table: "Movie",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "UserMovie",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    MovieId = table.Column<long>(nullable: false),
                    Rate = table.Column<int>(nullable: true),
                    IsInWatchlist = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovie", x => new { x.MovieId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMovie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMovie_UserId",
                table: "UserMovie",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMovie");

            migrationBuilder.DropColumn(
                name: "RatesCount",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "RatesSum",
                table: "Movie");

            migrationBuilder.AddColumn<double>(
                name: "AverageRate",
                table: "Movie",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    MovieId = table.Column<long>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => new { x.MovieId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_Rate_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rate_User_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchLaterMovie",
                columns: table => new
                {
                    UsereId = table.Column<long>(nullable: false),
                    MovieId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLaterMovie", x => new { x.UsereId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_WatchLaterMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchLaterMovie_User_UsereId",
                        column: x => x.UsereId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rate_ProfileId",
                table: "Rate",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchLaterMovie_MovieId",
                table: "WatchLaterMovie",
                column: "MovieId");
        }
    }
}
