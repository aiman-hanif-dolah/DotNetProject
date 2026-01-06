using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Season = table.Column<int>(type: "int", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "int", nullable: false),
                    AirDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Context = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    EpisodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotes_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "ActorName", "Description", "ImageUrl", "Name", "Occupation" },
                values: new object[,]
                {
                    { 1, "Jennifer Aniston", "Initially a waitress at Central Perk, later becomes a fashion executive at Ralph Lauren.", null, "Rachel Green", "Fashion Executive" },
                    { 2, "Courteney Cox", "A perfectionist chef known for her cleanliness obsession and competitive nature.", null, "Monica Geller", "Head Chef" },
                    { 3, "Lisa Kudrow", "A quirky masseuse and musician known for her song 'Smelly Cat'.", null, "Phoebe Buffay", "Masseuse/Musician" },
                    { 4, "Matt LeBlanc", "A lovable but dim-witted aspiring actor best known for his role as Dr. Drake Ramoray.", null, "Joey Tribbiani", "Actor" },
                    { 5, "Matthew Perry", "Known for his sarcastic wit and comedic timing. Later transitions to advertising.", null, "Chandler Bing", "IT Procurement/Advertising" },
                    { 6, "David Schwimmer", "Monica's older brother, a paleontologist who has been married three times.", null, "Ross Geller", "Paleontologist" }
                });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "AirDate", "Description", "EpisodeNumber", "Season", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(1994, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rachel runs away from her wedding and moves in with Monica.", 1, 1, "The One Where Monica Gets a Roommate" },
                    { 2, new DateTime(1994, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ross finds out his ex-wife Carol is pregnant with his baby.", 2, 1, "The One with the Sonogram at the End" },
                    { 3, new DateTime(1994, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "A power outage in New York City leads to adventures for the group.", 7, 1, "The One with the Blackout" },
                    { 4, new DateTime(1995, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ross discovers Rachel's feelings for him through a drunken voicemail.", 7, 2, "The One Where Ross Finds Out" },
                    { 5, new DateTime(1996, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The gang watches Rachel and Monica's prom video, revealing Ross's long-time love for Rachel.", 14, 2, "The One with the Prom Video" },
                    { 6, new DateTime(1996, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ross struggles to get everyone ready for a museum event.", 2, 3, "The One Where No One's Ready" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Description", "ImageUrl", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "90 Bedford Street, New York", "The iconic coffee house where the friends spend most of their time.", null, "Central Perk", "Coffee Shop" },
                    { 2, "90 Bedford Street, Apt 20, New York", "Monica's purple-walled apartment where most scenes take place.", null, "Monica's Apartment", "Apartment" },
                    { 3, "90 Bedford Street, Apt 19, New York", "The apartment across the hall from Monica's, featuring the famous foosball table.", null, "Joey & Chandler's Apartment", "Apartment" },
                    { 4, "New York", "Ross's various apartments throughout the series.", null, "Ross's Apartment", "Apartment" },
                    { 5, "New York", "Where Ross works as a paleontologist.", null, "Museum of Prehistoric History", "Workplace" }
                });

            migrationBuilder.InsertData(
                table: "Quotes",
                columns: new[] { "Id", "CharacterId", "Context", "EpisodeId", "Text" },
                values: new object[,]
                {
                    { 1, 6, "Ross's famous catchphrase about his relationship with Rachel", 1, "We were on a break!" },
                    { 2, 4, "Joey's signature pickup line", 1, "How you doin'?" },
                    { 3, 5, "Chandler's sarcastic observation", 1, "Could this BE any more awkward?" },
                    { 4, 2, "Janice's iconic catchphrase, often imitated by others", 2, "Oh. My. God!" },
                    { 5, 3, "Phoebe explaining Ross and Rachel's relationship", 5, "See, he's her lobster!" },
                    { 6, 6, "Ross trying to move a couch up the stairs", 6, "PIVOT! PIVOT! PIVOT!" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CharacterId",
                table: "Quotes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_EpisodeId",
                table: "Quotes",
                column: "EpisodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Episodes");
        }
    }
}
