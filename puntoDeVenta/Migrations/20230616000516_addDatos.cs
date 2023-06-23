using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace puntoDeVenta.Migrations
{
    public partial class addDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blocCompras",
                columns: table => new
                {
                    BlocCompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precioCompra = table.Column<double>(type: "float", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: true),
                    importe = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocCompras", x => x.BlocCompraID);
                });

            migrationBuilder.CreateTable(
                name: "blocVentas",
                columns: table => new
                {
                    BlocVentaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroVenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<double>(type: "float", nullable: false),
                    total = table.Column<double>(type: "float", nullable: false),
                    igv = table.Column<double>(type: "float", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocVentas", x => x.BlocVentaID);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "numeroCorrelativos",
                columns: table => new
                {
                    NumeroCorrelativoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UltimoNumero = table.Column<int>(type: "int", nullable: true),
                    CantidadDigitos = table.Column<int>(type: "int", nullable: true),
                    Gestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeroCorrelativos", x => x.NumeroCorrelativoID);
                });

            migrationBuilder.CreateTable(
                name: "proveedor",
                columns: table => new
                {
                    ProveedorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ruc = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proveedor", x => x.ProveedorID);
                });

            migrationBuilder.CreateTable(
                name: "rols",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rols", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    ProductoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    precioCompra = table.Column<double>(type: "float", nullable: false),
                    precioVenta = table.Column<double>(type: "float", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaID = table.Column<int>(type: "int", nullable: true),
                    ProveedorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.ProductoID);
                    table.ForeignKey(
                        name: "FK_productos_categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "categorias",
                        principalColumn: "CategoriaID");
                    table.ForeignKey(
                        name: "FK_productos_proveedor_ProveedorID",
                        column: x => x.ProveedorID,
                        principalTable: "proveedor",
                        principalColumn: "ProveedorID");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolID = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numeroDni = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    telefono = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK_Usuarios_rols_RolID",
                        column: x => x.RolID,
                        principalTable: "rols",
                        principalColumn: "RolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    CompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroCompra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreProveedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precioCompra = table.Column<double>(type: "float", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    fechaEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    importe = table.Column<double>(type: "float", nullable: false),
                    ProveedorID = table.Column<int>(type: "int", nullable: true),
                    ProductoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compra", x => x.CompraID);
                    table.ForeignKey(
                        name: "FK_compra_productos_ProductoID",
                        column: x => x.ProductoID,
                        principalTable: "productos",
                        principalColumn: "ProductoID");
                    table.ForeignKey(
                        name: "FK_compra_proveedor_ProveedorID",
                        column: x => x.ProveedorID,
                        principalTable: "proveedor",
                        principalColumn: "ProveedorID");
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    VentasID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    numeroVenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombreCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<double>(type: "float", nullable: false),
                    total = table.Column<double>(type: "float", nullable: false),
                    igv = table.Column<double>(type: "float", nullable: false),
                    tipoVenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ventas", x => x.VentasID);
                    table.ForeignKey(
                        name: "FK_ventas_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listaCompras",
                columns: table => new
                {
                    ListaCompraID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroCompra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompraID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listaCompras", x => x.ListaCompraID);
                    table.ForeignKey(
                        name: "FK_listaCompras_compra_CompraID",
                        column: x => x.CompraID,
                        principalTable: "compra",
                        principalColumn: "CompraID");
                });

            migrationBuilder.CreateTable(
                name: "listaVentas",
                columns: table => new
                {
                    ListaVentaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroVenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipoVenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<double>(type: "float", nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VentasID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listaVentas", x => x.ListaVentaID);
                    table.ForeignKey(
                        name: "FK_listaVentas_ventas_VentasID",
                        column: x => x.VentasID,
                        principalTable: "ventas",
                        principalColumn: "VentasID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_compra_ProductoID",
                table: "compra",
                column: "ProductoID");

            migrationBuilder.CreateIndex(
                name: "IX_compra_ProveedorID",
                table: "compra",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_listaCompras_CompraID",
                table: "listaCompras",
                column: "CompraID");

            migrationBuilder.CreateIndex(
                name: "IX_listaVentas_VentasID",
                table: "listaVentas",
                column: "VentasID");

            migrationBuilder.CreateIndex(
                name: "IX_productos_CategoriaID",
                table: "productos",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_productos_ProveedorID",
                table: "productos",
                column: "ProveedorID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolID",
                table: "Usuarios",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_UsuarioID",
                table: "ventas",
                column: "UsuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocCompras");

            migrationBuilder.DropTable(
                name: "blocVentas");

            migrationBuilder.DropTable(
                name: "listaCompras");

            migrationBuilder.DropTable(
                name: "listaVentas");

            migrationBuilder.DropTable(
                name: "numeroCorrelativos");

            migrationBuilder.DropTable(
                name: "compra");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "proveedor");

            migrationBuilder.DropTable(
                name: "rols");
        }
    }
}
