﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Panaderia.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarKardexInventarioMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KardexInventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlmacenProductoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockAnterior = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Costo = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    UsuarioAplicacionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KardexInventarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KardexInventarios_AlmacenesProductos_AlmacenProductoId",
                        column: x => x.AlmacenProductoId,
                        principalTable: "AlmacenesProductos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KardexInventarios_AspNetUsers_UsuarioAplicacionId",
                        column: x => x.UsuarioAplicacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventarios_AlmacenProductoId",
                table: "KardexInventarios",
                column: "AlmacenProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventarios_UsuarioAplicacionId",
                table: "KardexInventarios",
                column: "UsuarioAplicacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KardexInventarios");
        }
    }
}
