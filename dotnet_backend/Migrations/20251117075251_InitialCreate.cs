using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    permission_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    permission_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    action_key = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.permission_id);
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    promo_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    promo_code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    discount_type = table.Column<string>(type: "TEXT", nullable: false),
                    discount_value = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    end_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    min_order_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    usage_limit = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    used_count = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    status = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.promo_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsPermissionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RolesRoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsPermissionId, x.RolesRoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalTable: "permissions",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "INTEGER", nullable: false),
                    permission_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    role = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_role",
                        column: x => x.role,
                        principalTable: "roles",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category_id = table.Column<int>(type: "INTEGER", nullable: true),
                    supplier_id = table.Column<int>(type: "INTEGER", nullable: true),
                    product_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    barcode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    unit = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true, defaultValue: "pcs"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_products_suppliers_supplier_id",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    promo_id = table.Column<int>(type: "INTEGER", nullable: true),
                    order_date = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    status = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "pending"),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    discount_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_promotions_promo_id",
                        column: x => x.promo_id,
                        principalTable: "promotions",
                        principalColumn: "promo_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    inventory_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.inventory_id);
                    table.ForeignKey(
                        name: "FK_inventory_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    order_item_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    product_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.order_item_id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    payment_method = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "cash"),
                    payment_date = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fk_inventory_products",
                table: "inventory",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "fk_order_items_orders",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "fk_order_items_products",
                table: "order_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "fk_orders_customers",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "fk_orders_promotions",
                table: "orders",
                column: "promo_id");

            migrationBuilder.CreateIndex(
                name: "fk_orders_users",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "fk_payments_orders",
                table: "payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesRoleId",
                table: "PermissionRole",
                column: "RolesRoleId");

            migrationBuilder.CreateIndex(
                name: "action_key",
                table: "permissions",
                column: "action_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "barcode",
                table: "products",
                column: "barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_products_categories",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "fk_products_suppliers",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "promo_code",
                table: "promotions",
                column: "promo_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "role",
                table: "users",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
