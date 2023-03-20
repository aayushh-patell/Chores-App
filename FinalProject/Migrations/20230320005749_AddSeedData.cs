using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8h7cb3u4-8375-0384-d625-nd73hz5h91gw", 0, "e3c82046-1376-42fb-9b50-43577323c1e6", "alex.gilmer@mitt.ca", true, "Alex", "Gilmer", false, null, "ALEX.GILMER@MITT.CA", "ALEX.GILMER@MITT.CA", "AQAAAAEAACcQAAAAEG4p0sObEod0dbR50BRAQc10plMRVHkR500GTg0GFUxswy5980SwyC7HFJq6pyXMbw==", null, false, "8d67829b-6972-49ca-942f-379cd5c22f28", false, "alex.gilmer@mitt.ca" },
                    { "92n6dhf7-7254-0265-h265-8ch25zmp6hst", 0, "a2ffc717-5246-4393-af47-cd9d4ead29ae", "chris.macdonald@mitt.ca", true, "Chris", "MacDonald", false, null, "CHRIS.MACDONALD@MITT.CA", "CHRIS.MACDONALD@MITT.CA", "AQAAAAEAACcQAAAAEFYIwx8hVeuwZc0iIyX9LvFMBwzu2AW64cbLsMHfn8cNRjx1lgQeLZ6WDVr8jlO/ow==", null, false, "f3224e71-cbd6-42af-82b8-e9dc22ba7ed4", false, "chris.macdonald@mitt.ca" },
                    { "hs73mcu2-9264-0276-h827-js82hcbza04h", 0, "4d18b654-6c02-49d4-89f6-3408ba2ab73a", "aayushptl2005@gmail.com", true, "Aayush", "Patel", false, null, "AAYUSHPTL2005@GMAIL.COM", "AAYUSHPTL2005@GMAIL.COM", "AQAAAAEAACcQAAAAEDNa8DFw9NB3mkyztJ8zHvwc2L+OsB5vhZLnURbHTARVkegrNXZ/z+9Km/ONKIVwVw==", null, false, "b8165ab6-3f00-482a-b48f-fa4cb9a071a0", false, "aayushptl2005@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Cleaning" },
                    { 2, "Finance" },
                    { 3, "Shopping" },
                    { 4, "Groceries" },
                    { 5, "Other" }
                });

            migrationBuilder.InsertData(
                table: "Chores",
                columns: new[] { "Id", "CategoryId", "Completed", "DueDate", "Name", "Recurrence", "UserId" },
                values: new object[,]
                {
                    { 1, 5, false, new DateTime(2023, 3, 20, 15, 0, 0, 0, DateTimeKind.Unspecified), "Get a Haircut", 3, "8h7cb3u4-8375-0384-d625-nd73hz5h91gw" },
                    { 2, 5, false, new DateTime(2023, 3, 20, 18, 30, 0, 0, DateTimeKind.Unspecified), "Walk the Dog", 1, "hs73mcu2-9264-0276-h827-js82hcbza04h" },
                    { 3, 4, false, new DateTime(2023, 3, 25, 20, 0, 0, 0, DateTimeKind.Unspecified), "Buy Groceries", 2, "92n6dhf7-7254-0265-h265-8ch25zmp6hst" },
                    { 4, 5, false, new DateTime(2023, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Dentist Appointment", 4, "8h7cb3u4-8375-0384-d625-nd73hz5h91gw" },
                    { 5, 5, false, new DateTime(2023, 6, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), "Aayush's Birthday Party", 0, null },
                    { 6, null, false, new DateTime(2023, 3, 31, 9, 0, 0, 0, DateTimeKind.Unspecified), "Buy New Phone", 0, "hs73mcu2-9264-0276-h827-js82hcbza04h" },
                    { 7, null, false, new DateTime(2023, 3, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), "Do the Laundry", 2, "8h7cb3u4-8375-0384-d625-nd73hz5h91gw" }
                });

            migrationBuilder.InsertData(
                table: "ChoreMonths",
                columns: new[] { "ChoreId", "Month" },
                values: new object[] { 4, "April" });

            migrationBuilder.InsertData(
                table: "ChoreMonths",
                columns: new[] { "ChoreId", "Month" },
                values: new object[] { 4, "October" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ChoreMonths",
                keyColumns: new[] { "ChoreId", "Month" },
                keyValues: new object[] { 4, "April" });

            migrationBuilder.DeleteData(
                table: "ChoreMonths",
                keyColumns: new[] { "ChoreId", "Month" },
                keyValues: new object[] { 4, "October" });

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "92n6dhf7-7254-0265-h265-8ch25zmp6hst");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "hs73mcu2-9264-0276-h827-js82hcbza04h");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Chores",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8h7cb3u4-8375-0384-d625-nd73hz5h91gw");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
