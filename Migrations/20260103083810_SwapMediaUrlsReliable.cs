using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetProject.Migrations
{
    /// <inheritdoc />
    public partial class SwapMediaUrlsReliable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/4/4a/Rachel_Green_Friends.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/e/e2/Monica_Geller_Courteney_Cox.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/f/f6/Friendsphoebe.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/f/f0/Joey_Tribbiani.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/6/6c/Chandler_Bing_Matthew_Perry.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/6/6f/Ross_Geller.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/d/d6/Friends_Season_1_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/d/d6/Friends_Season_1_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/d/d6/Friends_Season_1_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/f/f6/Friends_Season_2_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/f/f6/Friends_Season_2_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/en/1/16/Friends_Season_3_DVD.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/7/7e/Central_Perk_at_Warner_Bros_Studio_Tour_Hollywood.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/48/90_Bedford_Street%2C_New_York_City.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/48/90_Bedford_Street%2C_New_York_City.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/48/90_Bedford_Street%2C_New_York_City.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/3/3d/AMNH_entrance.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/1/1c/Rachel_Green.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/6/6c/Monica_Geller.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/2/26/PhoebeBuffay.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/5/5d/JoeyTribbiani.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/8/8e/Chandler_Bing.jpg");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/f/f0/RossGeller.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/3/3b/101.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/a/a1/102.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/9/99/107.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/1/19/207.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/b/bc/214.jpg");

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/6/6b/302.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/7/7e/Central_Perk.png");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/8/89/Monicas_apartment_living_room.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/4/45/Joey_Chandler_apartment.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/d/d2/Ross_apartment.jpg");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://static.wikia.nocookie.net/friends/images/3/3f/Museum_lobby.jpg");
        }
    }
}
