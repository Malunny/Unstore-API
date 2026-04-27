using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Unstore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ContactNumber = table.Column<string>(type: "varchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Wage = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToolTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ContactNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolToolTags",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolTagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolToolTags", x => new { x.ToolId, x.ToolTagId });
                    table.ForeignKey(
                        name: "FK_ToolToolTags_ToolTags_ToolTagId",
                        column: x => x.ToolTagId,
                        principalTable: "ToolTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolToolTags_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceTool",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTool", x => new { x.ServiceId, x.ToolsId });
                    table.ForeignKey(
                        name: "FK_ServiceTool_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTool_Tools_ToolsId",
                        column: x => x.ToolsId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "ContactNumber", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Av. Paulista, 1000", "1130001000", "contato@solar.com", "Condomínio Solar" },
                    { 2, "Rua das Flores, 50", "1130002000", "adm@harmonia.com", "Residencial Harmonia" },
                    { 3, "Rua Saúde, 200", "1130003000", "atendimento@bemestar.com", "Clínica Médica Bem Estar" },
                    { 4, "Praça da Sé, 10", "1130004000", "pao@central.com", "Padaria Central" },
                    { 5, "Alameda Santos, 450", "1130005000", "legal@advx.com", "Escritório Advocacia X" },
                    { 6, "Rua B, 123", "11988887777", "joao@gmail.com", "João da Silva" },
                    { 7, "Av. Brasil, 99", "11977776666", "maria@yahoo.com", "Maria Oliveira" },
                    { 8, "Rua Gastronomia, 15", "1130008000", "gerencia@sabor.com", "Restaurante Sabor" },
                    { 9, "Rua Educação, 88", "1130009000", "diretoria@prime.com", "Escola Infantil Prime" },
                    { 10, "Rua do Suor, 500", "1130010000", "treino@fit.com", "Academia Fit" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Description", "Name", "Wage" },
                values: new object[,]
                {
                    { 1, "Especialista em persianas motorizadas", "Instalador Sênior", 3500.00m },
                    { 2, "Auxílio em furações e transporte", "Ajudante de Instalação", 1800.00m },
                    { 3, "Atendimento interno e orçamentos", "Consultor de Vendas", 2200.00m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "ServiceId", "Value" },
                values: new object[,]
                {
                    { 1, "Tecido 100% poliéster", "url.com", "Persiana Rolo Blackout", null, 150.00m },
                    { 2, "Tecido sob medida", "url.com", "Cortina de Linho", null, 280.00m },
                    { 3, "Somfy 220v", "url.com", "Motor para Persiana", null, 450.00m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Administrador total", "ADM" },
                    { 2, "Acesso a vendas e clientes", "Vendedor" },
                    { 3, "Acesso a ordens de serviço e ferramentas", "Instalador" }
                });

            migrationBuilder.InsertData(
                table: "ToolTags",
                columns: new[] { "Id", "Description", "TagName" },
                values: new object[,]
                {
                    { 1, "Ferramentas que usam bateria ou cabo", "Elétrica" },
                    { 2, "Precisão e medidas", "Medição" },
                    { 3, "Ferramentas de mão", "Manual" }
                });

            migrationBuilder.InsertData(
                table: "Tools",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Furadeira Makita 18V", "Furadeira de Impacto" },
                    { 2, "Medidor Bosch 50m", "Trena Laser" },
                    { 3, "Nível de alumínio 60cm", "Nível de Bolha" },
                    { 4, "Escada 7 degraus", "Escada Extensível" },
                    { 5, "DeWalt com controle de torque", "Parafusadeira" },
                    { 6, "Jogo de chaves Phillips e Fenda", "Maleta de Chaves" },
                    { 7, "Para limpeza pós-furação", "Aspirador Portátil" },
                    { 8, "Para ajuste de suportes", "Martelo de Borracha" },
                    { 9, "Corte de sobras de tecido", "Estilete Profissional" },
                    { 10, "Para evitar furar canos", "Detector de Metais/Vigas" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Active", "ContactNumber", "Email", "Name", "PositionId" },
                values: new object[,]
                {
                    { 1, true, "11911111111", "roberto@unstore.com", "Roberto Alves", 1 },
                    { 2, true, "11922222222", "felipe@unstore.com", "Felipe Souza", 2 },
                    { 3, true, "11933333333", "mariana@unstore.com", "Mariana Costa", 3 },
                    { 4, true, "11944444444", "lucas@unstore.com", "Lucas Mendes", 1 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "ContactNumber", "Email", "Name", "PositionId" },
                values: new object[] { 5, "11955555555", "beatriz@unstore.com", "Beatriz Rocha", 2 });

            migrationBuilder.InsertData(
                table: "ToolToolTags",
                columns: new[] { "ToolId", "ToolTagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 5, 1 },
                    { 6, 3 },
                    { 8, 3 },
                    { 9, 3 },
                    { 10, 2 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Address", "ClientId", "Cost", "Details", "EmployeeId" },
                values: new object[] { 1, "Av. Paulista, 1000 - Sala 5", 1, 0m, "Instalação de 4 persianas blackout motorizadas", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ServiceId",
                table: "Products",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ClientId",
                table: "Services",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_EmployeeId",
                table: "Services",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTool_ToolsId",
                table: "ServiceTool",
                column: "ToolsId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolToolTags_ToolTagId",
                table: "ToolToolTags",
                column: "ToolTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ServiceTool");

            migrationBuilder.DropTable(
                name: "ToolToolTags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ToolTags");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
