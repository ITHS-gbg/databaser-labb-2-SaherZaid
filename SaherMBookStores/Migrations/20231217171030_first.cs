using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Author_id = table.Column<int>(type: "int", nullable: false),
                    First_Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Last_Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Date_of_Birth = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Authors_pk", x => x.Author_id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_id = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    last_name = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    state = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    city = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    street = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    zip_code = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__8CB382B1F07522E7", x => x.Customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Store_iD = table.Column<int>(type: "int", nullable: false),
                    StoreName = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    Address = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    City = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    State = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Stores__A0F0677996872BAB", x => x.Store_iD);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ISBN13 = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Language = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Release_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Author_no = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Books__3BF79E0300E916DC", x => x.ISBN13);
                    table.ForeignKey(
                        name: "FK__Books__Author_no__267ABA7A",
                        column: x => x.Author_no,
                        principalTable: "Authors",
                        principalColumn: "Author_id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    position = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: true),
                    contact_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    Store_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__C52E0BA82528F404", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK__Employees__Store__5BE2A6F2",
                        column: x => x.Store_id,
                        principalTable: "Stores",
                        principalColumn: "Store_iD");
                });

            migrationBuilder.CreateTable(
                name: "AuthorBooks",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__AuthorBoo__autho__6E01572D",
                        column: x => x.author_id,
                        principalTable: "Authors",
                        principalColumn: "Author_id");
                    table.ForeignKey(
                        name: "FK__AuthorBoo__book___6EF57B66",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "ISBN13");
                });

            migrationBuilder.CreateTable(
                name: "InventoryBalance",
                columns: table => new
                {
                    Store_id = table.Column<int>(type: "int", nullable: false),
                    ISBN13 = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    No_Of_Products = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventor__834F1EF94752277B", x => new { x.Store_id, x.ISBN13 });
                    table.ForeignKey(
                        name: "FK__Inventory__ISBN1__3F466844",
                        column: x => x.ISBN13,
                        principalTable: "Books",
                        principalColumn: "ISBN13");
                    table.ForeignKey(
                        name: "FK__Inventory__Store__3E52440B",
                        column: x => x.Store_id,
                        principalTable: "Stores",
                        principalColumn: "Store_iD");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_id = table.Column<int>(type: "int", nullable: false),
                    Order_date = table.Column<DateOnly>(type: "date", nullable: true),
                    Order_time = table.Column<TimeOnly>(type: "time", nullable: true),
                    Order_status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Order_Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Reference_Number = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    Customer_No = table.Column<int>(type: "int", nullable: true),
                    Store_No = table.Column<int>(type: "int", nullable: true),
                    Order_Price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__F1FF8453E1A1211A", x => x.Order_id);
                    table.ForeignKey(
                        name: "Orders_Books_FK",
                        column: x => x.Reference_Number,
                        principalTable: "Books",
                        principalColumn: "ISBN13");
                    table.ForeignKey(
                        name: "Orders_Customers_FK",
                        column: x => x.Customer_No,
                        principalTable: "Customers",
                        principalColumn: "Customer_id");
                    table.ForeignKey(
                        name: "Orders_Orders_FK",
                        column: x => x.Store_No,
                        principalTable: "Stores",
                        principalColumn: "Store_iD");
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    shift_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    shift_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    shift_end = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shifts__7B2672209195BAEE", x => x.shift_id);
                    table.ForeignKey(
                        name: "FK__Shifts__employee__5EBF139D",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBooks_author_id",
                table: "AuthorBooks",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBooks_book_id",
                table: "AuthorBooks",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Author_no",
                table: "Books",
                column: "Author_no");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Store_id",
                table: "Employees",
                column: "Store_id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalance_ISBN13",
                table: "InventoryBalance",
                column: "ISBN13");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_No",
                table: "Orders",
                column: "Customer_No");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Reference_Number",
                table: "Orders",
                column: "Reference_Number");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Store_No",
                table: "Orders",
                column: "Store_No");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_employee_id",
                table: "Shifts",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBooks");

            migrationBuilder.DropTable(
                name: "InventoryBalance");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
