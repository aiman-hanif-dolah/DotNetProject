using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetProject.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Locations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Episodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Episodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Characters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/4/4a/Rachel_Green_Friends.jpg", "https://www.youtube.com/embed?listType=search&list=Rachel%20Green%20Friends%20best%20moments" });

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/e/e2/Monica_Geller_Courteney_Cox.jpg", "https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20Friends%20kitchen%20scenes" });

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/f/f6/Friendsphoebe.jpg", "https://www.youtube.com/embed?listType=search&list=Phoebe%20Buffay%20Smelly%20Cat" });

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/f/f0/Joey_Tribbiani.jpg", "https://www.youtube.com/embed?listType=search&list=Joey%20Tribbiani%20how%20you%20doin" });

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/6/6c/Chandler_Bing_Matthew_Perry.jpg", "https://www.youtube.com/embed?listType=search&list=Chandler%20Bing%20sarcasm" });

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/en/6/6f/Ross_Geller.jpg", "https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20pivot%20scene" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1469474968028-56623f02e42e?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20S01E01%20pilot%20clip" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20sonogram%20episode" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1504674900247-0877df9cc836?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20blackout%20episode" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1504384308090-c894fdcc538d?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20Ross%20finds%20out%20episode" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1508214751196-bcfd4ca60f91?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20prom%20video%20episode" });

            migrationBuilder.UpdateData(
                table: "Episodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.unsplash.com/photo-1478720568477-152d9b164e26?auto=format&fit=crop&w=1200&q=80", "https://www.youtube.com/embed?listType=search&list=Friends%20no%20ones%20ready%20episode" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.pexels.com/photos/304658/pexels-photo-304658.jpeg?auto=compress&cs=tinysrgb&w=1200", "https://www.youtube.com/embed?listType=search&list=Central%20Perk%20set%20tour" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.pexels.com/photos/1571459/pexels-photo-1571459.jpeg?auto=compress&cs=tinysrgb&w=1200", "https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20apartment%20tour" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.pexels.com/photos/1457842/pexels-photo-1457842.jpeg?auto=compress&cs=tinysrgb&w=1200", "https://www.youtube.com/embed?listType=search&list=Joey%20and%20Chandler%20apartment%20tour" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.pexels.com/photos/271816/pexels-photo-271816.jpeg?auto=compress&cs=tinysrgb&w=1200", "https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20apartment%20tour" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageUrl", "VideoUrl" },
                values: new object[] { "https://images.pexels.com/photos/277414/pexels-photo-277414.jpeg?auto=compress&cs=tinysrgb&w=1200", "https://www.youtube.com/embed?listType=search&list=Friends%20museum%20set%20tour" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Characters");

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: null);
        }
    }
}
