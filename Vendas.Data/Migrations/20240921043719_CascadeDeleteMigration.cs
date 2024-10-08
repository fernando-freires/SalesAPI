﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendas.Data.Migrations
{
    public partial class CascadeDeleteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda");

            migrationBuilder.AlterColumn<Guid>(
                name: "VendaId",
                table: "ItemVenda",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda");

            migrationBuilder.AlterColumn<Guid>(
                name: "VendaId",
                table: "ItemVenda",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVenda_Vendas_VendaId",
                table: "ItemVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id");
        }
    }
}
