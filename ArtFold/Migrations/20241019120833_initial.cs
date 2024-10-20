using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtFold.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckOuts",
                columns: table => new
                {
                    CheckOutID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOuts", x => x.CheckOutID);
                    table.ForeignKey(
                        name: "FK_CheckOuts_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintPaperType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartProducts",
                columns: table => new
                {
                    CartID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCartQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => new { x.CartID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_CartProducts_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "CartID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckOutProducts",
                columns: table => new
                {
                    CheckOutProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckOutID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOutProducts", x => x.CheckOutProductID);
                    table.ForeignKey(
                        name: "FK_CheckOutProducts_CheckOuts_CheckOutID",
                        column: x => x.CheckOutID,
                        principalTable: "CheckOuts",
                        principalColumn: "CheckOutID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckOutProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ProductImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ProductImageID);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentsImage",
                columns: table => new
                {
                    CommentImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsImage", x => x.CommentImageID);
                    table.ForeignKey(
                        name: "FK_CommentsImage_Comments_CommentID",
                        column: x => x.CommentID,
                        principalTable: "Comments",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", null, "Customer", "CUSTOMER" },
                    { "ca56d541-391a-4d35-9814-5b823922c37d", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "CreatedAt", "District", "Email", "EmailConfirmed", "FullName", "HouseAddress", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Ward" },
                values: new object[,]
                {
                    { "3b7a6789-7551-4c71-9138-9cc50ce7df36", 0, null, "26b10cd1-0f0f-4eef-96c7-8f37cc41837a", new DateTime(2024, 10, 19, 19, 8, 31, 929, DateTimeKind.Local).AddTicks(412), null, "dinhuynhminhthu@gmail.com", true, "Đinh Huỳnh Minh Thư", null, false, null, null, null, "AQAAAAIAAYagAAAAEHo6b0D4d5IHrsHEE1y6pTlPjPdUzqNPEmE2KjLm1aNRHFKBxipWIJMyq80lqUqYQA==", "0123456789", false, "8a57c535-c9e2-44cc-a1ab-4e5ef7b9170b", false, "MinhThu", null },
                    { "77b5473f-4397-40c2-8c21-750d3e407317", 0, null, "e1605e7a-5bfd-4ac0-b5a8-73af0454301b", new DateTime(2024, 10, 19, 19, 8, 31, 807, DateTimeKind.Local).AddTicks(5936), null, "bluegameming292003@gmail.com", true, "Trần Minh Quốc Khánh", null, false, null, null, null, "AQAAAAIAAYagAAAAEHP0HhWLUvlRhAP8TsB9O0NpjmmmedAZflMFazgZB2bAR1nKw+ZSe0htydA7I1o08w==", "0934763210", false, "24cfe60e-3f71-42d1-b1cc-4524c6e8059f", false, "Admin", null },
                    { "9c72a140-883f-4318-ba86-7eab60208c49", 0, null, "cbfd767e-9758-400c-b7c3-ffe57f0169c7", new DateTime(2024, 10, 19, 19, 8, 31, 988, DateTimeKind.Local).AddTicks(8557), null, "ngocha@gmail.com", true, "Ngọc Hà", null, false, null, null, null, "AQAAAAIAAYagAAAAEG5z5RE85sUzzQfkhIvgKRH3n0FxFjQKkzQ8ykDs/p53og+c/0unsC08p5fH8UQ0DQ==", "0123456789", false, "41f6a228-4b67-4567-b7bd-1495c348db50", false, "NgocHa", null },
                    { "bba66c5f-2628-4559-8cb0-a2882839b04e", 0, null, "47abfa9d-f229-4ce0-8000-cbaad9d9599e", new DateTime(2024, 10, 19, 19, 8, 32, 48, DateTimeKind.Local).AddTicks(159), null, "nguyenvungochan@gmail.com", true, "Nguyễn Vũ Ngọc Hân", null, false, null, null, null, "AQAAAAIAAYagAAAAEMInea+iBXZAga9GjDWou5o896kfAsiO0D5FxKfBHU1OZIq1vQ8Uvjyi956vfAEP8g==", "0123456789", false, "b5749fc2-90a2-4fbb-b73e-4cf8b1df3d6b", false, "NgocHan", null },
                    { "c3b81fe4-df66-46b7-94dc-46bfcfccba4b", 0, null, "aca9538d-8d39-433b-a18b-ae30e9a46d71", new DateTime(2024, 10, 19, 19, 8, 32, 113, DateTimeKind.Local).AddTicks(6162), null, "thuIT@gmail.com", true, "Thư AI", null, false, null, null, null, "AQAAAAIAAYagAAAAELkwJz/6jDkcsekPjEgzNnPZtQYwL/WKI/UVpq0X+s9NlChlOxtW0UjyIYMKODcOfg==", "0123456789", false, "f5fec51c-eb16-4b9e-b3a4-3551d05f9aad", false, "ThuIT", null },
                    { "c6d9c36f-09bf-4cd2-8fe9-8ee49e690d54", 0, null, "eb31586e-2650-4b3b-a183-7d48f693f450", new DateTime(2024, 10, 19, 19, 8, 31, 867, DateTimeKind.Local).AddTicks(7700), null, "taimodel@gmail.com", true, "Nguyễn Lương Tài", null, false, null, null, null, "AQAAAAIAAYagAAAAEMMB65QKof/Tex/Mil3CDM5sAV7CF45xaznh3vpmATIW8MZLGCRixeEcs/GPX1nsvQ==", "0123456789", false, "d1a3b317-3f6a-4aa3-8697-4f7b53c6b723", false, "TaiModel", null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), "Marvel" },
                    { new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), "Anime" },
                    { new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), "Architecture" },
                    { new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), "Vehicle" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", "3b7a6789-7551-4c71-9138-9cc50ce7df36" },
                    { "ca56d541-391a-4d35-9814-5b823922c37d", "77b5473f-4397-40c2-8c21-750d3e407317" },
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", "9c72a140-883f-4318-ba86-7eab60208c49" },
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", "bba66c5f-2628-4559-8cb0-a2882839b04e" },
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", "c3b81fe4-df66-46b7-94dc-46bfcfccba4b" },
                    { "5d04d8f2-0688-450f-826f-cf0b5c43dc12", "c6d9c36f-09bf-4cd2-8fe9-8ee49e690d54" }
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "CartID", "UserID" },
                values: new object[,]
                {
                    { new Guid("1473e255-5124-42a0-b67a-ffccafdce7b5"), "77b5473f-4397-40c2-8c21-750d3e407317" },
                    { new Guid("1c38f7ee-9739-4de7-9693-3c7824d862a0"), "bba66c5f-2628-4559-8cb0-a2882839b04e" },
                    { new Guid("2fbbbfe1-c499-424b-ba15-d9860c945be9"), "3b7a6789-7551-4c71-9138-9cc50ce7df36" },
                    { new Guid("89f21c2c-570e-4e5c-82b1-7ba5b7973ab8"), "c6d9c36f-09bf-4cd2-8fe9-8ee49e690d54" },
                    { new Guid("a7c93407-71af-48bd-9654-4301aa6cbb3e"), "c3b81fe4-df66-46b7-94dc-46bfcfccba4b" },
                    { new Guid("f73e4336-0ee3-4e0d-91d2-1ce04a0f4757"), "9c72a140-883f-4318-ba86-7eab60208c49" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductID", "CategoryID", "CreatedAt", "Description", "ImgUrl", "Name", "Price", "PrintPaperType", "ProductQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0d1607f4-a568-456f-9e45-3724c129ce62"), new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7248), "Bộ sản phẩm Mô hình giấy Anime Game Uzumaki Naruto ver 3 bao gồm:\r\n- 6 tờ kit mô hình.\r\n(Mặc định bản kit sẽ được in bản có line, nếu bạn muốn in bản ko line trong đơn hàng bạn ghi chú là \"in bản ko line\" để shop cho in nhé)\r\n- Kích thước A4: Cao: 17cm x Rộng: 20,1cm x Sâu: 28,3cm.\r\nXuất xứ: Việt Nam", "https://down-vn.img.susercontent.com/file/sg-11134201-22110-igsmlbzefhkvf0.webp", "Uzumaki Naruto", 42000.0, "A4", 30, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7249) },
                    { new Guid("192c7a8f-6a81-4283-971c-9d114b4e813c"), new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7490), "Bộ sản phẩm Mô hình giấy kiến trúc Tháp Luân Đôn Tower of London – England bao gồm:\r\n- 10 tờ kit mô hình.\r\n- 2 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/5e96e9613e2fd22d255d9d90159d19ce.webp", "Tower of London – England", 65000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7491) },
                    { new Guid("2d370af7-83bc-4b7c-a735-e60afa600f27"), new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7456), "Bộ sản phẩm Mô hình giấy Marvel Avengers Iron Spider bao gồm:\r\n- 15 tờ kit mô hình.\r\n- Kích thước: Cao: 38cm x Rộng: 30,7cm x Sâu: 34,5cm", "https://down-vn.img.susercontent.com/file/4b925257b8c606d8ba5549860b146ad1.webp", "Marvel Avengers Iron Spider", 100000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7466) },
                    { new Guid("4c9f7f8f-08b0-4f5a-bb4b-9670660c6564"), new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7276), "Bộ sản phẩm Mô hình giấy Anime Chibi Levi Ackerman ver 3 – Attack on Titan bao gồm:\r\n- 6 tờ kit mô hình.\r\n- Kích thước: Cao: 20,3cm x Rộng: 11,1cm x Sâu: 18cm", "https://down-vn.img.susercontent.com/file/a6da3b4677bd9309784051610617a5e7@resize_w450_nl.webp", "Chibi Levi Ackerman", 14000.0, "A4", 80, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7276) },
                    { new Guid("4cae498c-f91d-41c8-818f-8aac9f48b4c5"), new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7221), "Bộ sản phẩm Mô hình giấy Anime Goku SSJ HD – Dragon Ball bao gồm:\r\n- 25 tờ kit mô hình.\r\n- Kích thước: Cao: 55,5cm x Rộng: 13,4cm x Sâu: 23,9cm", "https://down-vn.img.susercontent.com/file/ea93877ccd8d3700b6b9ede4220df541.webp", "Son Goku", 50000.0, "A4", 50, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7225) },
                    { new Guid("56563249-1ba3-466e-bb08-e4ab96ca4ece"), new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7513), "Bộ sản phẩm Mô hình giấy kiến trúc Nhà thờ chính Siena Cathedral - Italy bao gồm:\r\n- 19 tờ kit mô hình.\r\n- 4 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/e7ac1e43b3160334e9ca1fc66da7f34a.webp", "Siena Cathedral - Italy", 124000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7514) },
                    { new Guid("5867c5a3-2eed-4e7d-89b9-3259fb1e33b5"), new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7429), "Bộ sản phẩm Mô hình giấy Marvel Avenger Robot Iron Man Mark VII bao gồm:\r\n- 16 tờ kit mô hình.\r\n- 3 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/5fc4fc6d877bc7c905b6f92eeb951a94.webp", "Robot Iron Man Mark VII", 105000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7430) },
                    { new Guid("5b4f83b6-8f1d-4912-b61d-f42eb89bdfa2"), new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7483), "Bộ sản phẩm Mô hình giấy kiến trúc lâu đài Đức Neuschwanstein Castle - Germany bao gồm:\r\n- 8 tờ kit mô hình.\r\n- 2 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/d50b7f9c059c8cb8e7c0654954a08ab1.webp", "Neuschwanstein Castle - Germany", 55000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7483) },
                    { new Guid("65d04395-587b-4325-a4b0-59d9fe099183"), new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7401), "Bộ sản phẩm Mô hình giấy xe ô tô Prototype Technology Group BMW bao gồm:\r\n- 6 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/9fb112bf0fe8b6b773c0aa7411a2392c.webp", "Prototype Technology Group BMW", 79000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7401) },
                    { new Guid("67f2ac12-41c8-42c2-aa09-f6e4f31dbaf1"), new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7435), "Bộ sản phẩm Mô hình giấy Anime Game Chibi Thor mập - Marvel bao gồm:\r\n- 8 tờ kit mô hình in mực Dầu trên giấy Màu.\r\n- 2 tờ hướng dẫn lắp ráp.\r\n- Kích thước : Cao 15,5cm x Rộng 13cm x Sâu 9cm.\r\nXuất xứ: Việt Nam", "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lmua3ev8pza778.webp", "Chibi Thor ", 50000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7436) },
                    { new Guid("6cf406b9-0ffa-45ca-94c5-fd6302e3bd4f"), new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7383), "Bộ sản phẩm Mô hình giấy phi thuyền không gian vũ trụ tàu con thoi Space Shuttle Atlantis bao gồm:\r\n- 11 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/4ed6a6e35f435d28286762c02db7f911.webp", "Space Shuttle Atlantis", 72000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7384) },
                    { new Guid("74a9338e-fdb6-4c06-a219-eb3de7c12b07"), new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7497), "Bộ sản phẩm Mô hình giấy kiến trúc Pháp tháp Eiffel Tower bao gồm:\r\n- 9 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/a077c0d85e3866a441e4b1e76ab69dbb.webp", "Eiffel Tower", 60000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7498) },
                    { new Guid("9d711647-d878-4915-ba5d-dfa3bb2c65e2"), new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7239), "Bộ sản phẩm Mô hình giấy Anime Chibi Monkey D Luffy - One Piece bao gồm:\r\n- 18 tờ kit mô hình.\r\n- Kích thước: Cao: 40cm x Rộng: 23,4cm x Sâu: 21,6cm", "https://down-vn.img.susercontent.com/file/e82a586f3d146ea83a3b6303b4668914.webp", "Monkey D. Luffy", 55000.0, "A4", 100, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7239) },
                    { new Guid("9ee99784-2643-4204-b45d-40bb9b688d5f"), new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7476), "Bộ sản phẩm Mô hình giấy Anime Game Chibi Doctor Strange - Marvel bao gồm:\r\n- 2 tờ kit mô hình + kèm scan code xem video hướng dẫn lắp ráp.\r\n* Xuất xứ: Việt Nam", "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lzad737x2krla7@resize_w450_nl.webp", "Chibi Doctor Strange", 25000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7477) },
                    { new Guid("9efdfeed-3495-4d67-8e42-5a4eb82ee62a"), new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7418), "Bộ sản phẩm Mô hình giấy máy bay Boeing 777-200 British Airways bao gồm:\r\n- 8 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/a09cfa936019a5e6c493acafbd4a13e1.webp", "Boeing 777-200 British Airways", 58000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7419) },
                    { new Guid("c27f87ab-a1cf-44ff-b696-1e03ba52e903"), new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7408), "Bộ sản phẩm Mô hình giấy xe máy Mille Miglia Custom Chopper bao gồm:\r\n- 24 tờ kit mô hình.\r\n- 8 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/2fbbe89ee72a717b7f2bed3a84d8b259.webp", "Mille Miglia Custom Chopper Bike", 149000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7408) },
                    { new Guid("c80cc419-9cdc-4619-9d1b-fa5c9e539632"), new Guid("a0223bc3-9ffc-478e-8268-c418decf70b6"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7390), "Bộ sản phẩm Mô hình giấy xe ô tô Lamborghini Sesto Elemento bao gồm:\r\n- 3 tờ kit mô hình.\r\n- Kích thước: Cao: 4,9cm x Rộng: 8,6cm x Sâu: 18,1cm", "https://down-vn.img.susercontent.com/file/966ca26a8de1b2f34c66449cc74e48bd.webp", "Lamborghini Sesto Elemento", 69000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7391) },
                    { new Guid("dc8fce56-18e2-468c-bc60-675f9f59711c"), new Guid("5ad1f1bf-0bb0-411b-8742-7e957ebb1ac3"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7254), "Bộ sản phẩm Mô hình giấy Anime Game Pokemon Pikachu Polygon ver 2 bao gồm:\r\n- 9 tờ kit mô hình in mực Dầu trên giấy Màu.\r\n- 4 tờ hướng dẫn lắp ráp.\r\n- Kích thước A4: Cao: 33cm x Rộng: 30cm x Sâu: 34cm.\r\nXuất xứ: Việt Nam", "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-ls9lvceatuah97@resize_w450_nl.webp", "Pikachu Polygon ver 2", 59000.0, "A4", 30, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7254) },
                    { new Guid("ec63fa84-95f7-40f6-a6d9-bf139bedf0a5"), new Guid("0a843071-4ee3-402a-ab7e-dd8874bcfd6f"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7441), "Bộ sản phẩm Mô hình giấy Anime Game Marvel Hulk Treo tường ver 3 bao gồm:\r\n– 17 tờ kit mô hình in trên giấy A4 Ford màu định lượng 180gsm (so với giấy photo là 70gsm) + scan code xem hướng dẫn.\r\n- Kích thước: Cao: khoảng 40cm", "https://down-vn.img.susercontent.com/file/8aedf29f64c9de9ac7ec2b3f48182f7b.webp", "Marvel Hulk Wall Hanging", 83000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7442) },
                    { new Guid("f17b9c9e-db3d-42b7-adf1-3876b7c552df"), new Guid("744bb7a2-05ec-4e10-aa48-608d5fc6125e"), new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7507), "Bộ sản phẩm Mô hình giấy kiến trúc Cambuchia Angkor Wat bao gồm:\r\n- 24 tờ kit mô hình.\r\n- 3 tờ hướng dẫn lắp ráp.", "https://down-vn.img.susercontent.com/file/edb6286c7abf2d62a36a911b5d0983d4.webp", "Cambuchia Angkor Wat", 156000.0, "A4", 10, new DateTime(2024, 10, 19, 19, 8, 32, 172, DateTimeKind.Local).AddTicks(7508) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_ProductID",
                table: "CartProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserID",
                table: "Carts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutProducts_CheckOutID",
                table: "CheckOutProducts",
                column: "CheckOutID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutProducts_ProductID",
                table: "CheckOutProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_UserID",
                table: "CheckOuts",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductID",
                table: "Comments",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsImage_CommentID",
                table: "CommentsImage",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CartProducts");

            migrationBuilder.DropTable(
                name: "CheckOutProducts");

            migrationBuilder.DropTable(
                name: "CommentsImage");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CheckOuts");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
