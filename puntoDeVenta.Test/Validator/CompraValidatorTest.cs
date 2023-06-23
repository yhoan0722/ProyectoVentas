using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using puntoDeVenta.Data;
using Moq;
using Moq.EntityFrameworkCore;
using puntoDeVenta.Models;
using puntoDeVenta.Validator;

namespace puntoDeVenta.Test.Validator
{
    public class CompraValidatorTest
    {
        [Test]
        public void RegistroCorrectamenteCompra()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra="000001",
                nombreProveedor="Lays",
                NombreProducto="doritos clasico 40g",
                precioCompra=1.8,
                cantidad=12,
                fechaEntrega= new DateTime(2023, 06, 14, 5, 10, 0),
                importe=12*1.8
            };
            var resultado = validacion.RegistroCompra(rcMock.Object,rcMock2.Object, compra);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RegistroCompraIncorrectoProductoNoExiste()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra = "000001",
                nombreProveedor = "Lays",
                NombreProducto = "doritos queso fuego 40g",
                precioCompra = 1.8,
                cantidad = 12,
                fechaEntrega = new DateTime(2023, 06, 14, 5, 10, 0),
                importe = 12 * 1.8
            };
            var resultado = validacion.RegistroCompra(rcMock.Object, rcMock2.Object, compra);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoCompraEstadoDelProductoInvalido()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="no disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra = "000001",
                nombreProveedor = "Lays",
                NombreProducto = "doritos clasico 40g",
                precioCompra = 1.8,
                cantidad = 12,
                fechaEntrega = new DateTime(2023, 06, 14, 5, 10, 0),
                importe = 12 * 1.8
            };
            var resultado = validacion.RegistroCompra(rcMock.Object, rcMock2.Object, compra);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroCompraIncorrectoProveedorNoExiste()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra = "000001",
                nombreProveedor = "Distribuidora Don Theo",
                NombreProducto = "doritos clasico 40g",
                precioCompra = 1.8,
                cantidad = 12,
                fechaEntrega = new DateTime(2023, 06, 14, 5, 10, 0),
                importe = 12 * 1.8
            };
            var resultado = validacion.RegistroCompra(rcMock.Object, rcMock2.Object, compra);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoCompraCantidadDelProductoInvalido()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra = "000001",
                nombreProveedor = "Lays",
                NombreProducto = "doritos clasico 40g",
                precioCompra = 1.8,
                cantidad = 0,
                fechaEntrega = new DateTime(2023, 06, 14, 5, 10, 0),
                importe = 0 * 1.8
            };
            var resultado = validacion.RegistroCompra(rcMock.Object, rcMock2.Object, compra);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoCompraPrecioCompraDelProductoInvalido()
        {
            var validacion = new CompraValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=12,estado="disponible"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa",precioCompra=2.4,precioVenta=3,stock=16,estado="disponible"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks",precioCompra=1.8,precioVenta=2.5,stock=12,estado="disponible"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza",precioCompra=5.5,precioVenta=6.5,stock=24,estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="Coca Cola",ruc="20143245443",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Backus",ruc="20172436748",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Distribuidora Chali SAC",ruc="20324546547",direccion="Av. San Martin #1354"},
                new Proveedor {nombre="Lays",ruc="20324546547",direccion="Av. San Martin #1354"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(p => p.proveedor).ReturnsDbSet(listaProveedor);
            var compra = new Compra
            {
                numeroCompra = "000001",
                nombreProveedor = "Lays",
                NombreProducto = "doritos clasico 40g",
                precioCompra = 0,
                cantidad = 12,
                fechaEntrega = new DateTime(2023, 06, 14, 5, 10, 0),
                importe = 12 * 0
            };
            var resultado = validacion.RegistroCompra(rcMock.Object, rcMock2.Object, compra);
            Assert.AreEqual(false, resultado);
        }
    }
}
