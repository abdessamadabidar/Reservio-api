﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservio.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomEquipmentToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomEquipment_Equipments_EquipmentId",
                table: "RoomEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomEquipment_Rooms_RoomId",
                table: "RoomEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomEquipment",
                table: "RoomEquipment");

            migrationBuilder.RenameTable(
                name: "RoomEquipment",
                newName: "RoomEquipments");

            migrationBuilder.RenameIndex(
                name: "IX_RoomEquipment_EquipmentId",
                table: "RoomEquipments",
                newName: "IX_RoomEquipments_EquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomEquipments",
                table: "RoomEquipments",
                columns: new[] { "RoomId", "EquipmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipments_Equipments_EquipmentId",
                table: "RoomEquipments",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipments_Rooms_RoomId",
                table: "RoomEquipments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomEquipments_Equipments_EquipmentId",
                table: "RoomEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomEquipments_Rooms_RoomId",
                table: "RoomEquipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomEquipments",
                table: "RoomEquipments");

            migrationBuilder.RenameTable(
                name: "RoomEquipments",
                newName: "RoomEquipment");

            migrationBuilder.RenameIndex(
                name: "IX_RoomEquipments_EquipmentId",
                table: "RoomEquipment",
                newName: "IX_RoomEquipment_EquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomEquipment",
                table: "RoomEquipment",
                columns: new[] { "RoomId", "EquipmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Equipments_EquipmentId",
                table: "RoomEquipment",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Rooms_RoomId",
                table: "RoomEquipment",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
