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
    public class ProductoValidatorTest
    {
        [Test]
        public void NombreProductoUnico()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.productos).ReturnsDbSet(new List<Producto>
            {
                new Producto{nombre="cocal cola 600ml",NombreCategoria="gaseosa"},
                new Producto{nombre="inca kola 600ml",NombreCategoria="gaseosa"},
                new Producto{nombre="cocal cola 3l sa",NombreCategoria="gaseosa"},
                new Producto{nombre="cocal cola 3l",NombreCategoria="gaseosa"},
            });
            var context = contextMock.Object;
            var validator = new ProductoValidator();
            var resultado = validator.NombreUnico(context, new Producto { nombre = "cocal cola 600ml", NombreCategoria = "gaseosa"});
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NombreProductoUnico2()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.productos).ReturnsDbSet(new List<Producto>
            {
                new Producto{nombre="cocal cola 600ml",NombreCategoria="gaseosa"},
                new Producto{nombre="inca kola 600ml",NombreCategoria="gaseosa"},
                new Producto{nombre="cocal cola 3l sa",NombreCategoria="gaseosa"},
                new Producto{nombre="cocal cola 3l",NombreCategoria="gaseosa"},
            });
            var context = contextMock.Object;
            var validator = new ProductoValidator();
            var resultado = validator.NombreUnico(context, new Producto { nombre = "fanta naranja 500ml", NombreCategoria = "gaseosa" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ValidarSiElEstadoDeLaCategoriaDelProductoEstaDisponible()
        {
            var validacion = new ProductoValidator();
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c=>c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "coca cola 600ml",
                NombreCategoria = "gaseosa"
            };
            var resultado = validacion.CategoriaProductoEstadoDisponible(rcMock.Object, producto);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ValidarSiElEstadoDeLaCategoriaDelProductoEstaDisponible2()
        {
            var validacion = new ProductoValidator();
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="no disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "coca cola 600ml",
                NombreCategoria = "gaseosa"
            };
            var resultado = validacion.CategoriaProductoEstadoDisponible(rcMock.Object, producto);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void ValidarSiElPrecioVentaEsMayorAlPrecioCompraDelProducto()
        {
            var validacion = new ProductoValidator();
            var resultado = validacion.PrecioVentaDebeSerMayorPrecioCompra(new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa", precioCompra = 2.4, precioVenta=3 });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ValidarSiElPrecioVentaEsMayorAlPrecioCompraDelProducto2()
        {
            var validacion = new ProductoValidator();
            var resultado = validacion.PrecioVentaDebeSerMayorPrecioCompra(new Producto { nombre = "coca-cola 600ml", NombreCategoria = "gaseosa", precioCompra = 3, precioVenta = 2.4 });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void ValidarSiElProductoYaEstaRegistrado()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p=>p.productos).ReturnsDbSet(listaProducto);
            var producto = new Producto
            {
                nombre = "coca-cola 600ml",
                NombreCategoria = "gaseosa"
            };
            var resultado = validacion.ProductoYaExiste(rcMock.Object, producto);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ValidarSiElProductoYaEstaRegistrado2()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var producto = new Producto
            {
                nombre = "papa lays clasica 32g",
                NombreCategoria = "snacks"
            };
            var resultado = validacion.ProductoYaExiste(rcMock.Object, producto);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroCorrectamenteDelProducto()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "papa lays clasica 32g",
                NombreCategoria = "snacks",
                precioCompra = 1.8,
                precioVenta = 2.5,
                stock = 12
            };
            var resultado = validacion.RegistroProducto(rcMock.Object,rcMock2.Object,producto);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RegistroIncorrectoProductoYaEstaRegistrado()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "coca-cola 600ml",
                NombreCategoria = "gaseosa",
                precioCompra = 2.4,
                precioVenta = 3,
                stock = 12
            };
            var resultado = validacion.RegistroProducto(rcMock.Object, rcMock2.Object, producto);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RegistroIncorrectoEstadoInvalidoCategoriaDelProducto()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="no disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "papa lays clasica 32g",
                NombreCategoria = "snacks",
                precioCompra = 1.8,
                precioVenta = 2.5,
                stock = 12
            };
            var resultado = validacion.RegistroProducto(rcMock.Object, rcMock2.Object, producto);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoPrecioVentaEsMayorPrecioCompraDelProducto()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "papa lays clasica 32g",
                NombreCategoria = "snacks",
                precioCompra = 2.5,
                precioVenta = 1.8,
                stock = 12
            };
            var resultado = validacion.RegistroProducto(rcMock.Object, rcMock2.Object, producto);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoStockInvalidoDelProducto()
        {
            var validacion = new ProductoValidator();
            var listaProducto = new List<Producto>
            {
                new Producto {nombre="coca-cola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="inca-kola 600ml",NombreCategoria="gaseosa"},
                new Producto {nombre="doritos clasico 40g",NombreCategoria="snacks"},
                new Producto {nombre="cristal 650ml",NombreCategoria="cerveza"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(p => p.productos).ReturnsDbSet(listaProducto);
            var listaCategoria = new List<Categoria>
            {
                new Categoria {nombre="gaseosa",estado="disponible"},
                new Categoria {nombre="galleta",estado="disponible"},
                new Categoria {nombre="snacks",estado="disponible"},
                new Categoria {nombre="vino",estado="disponible"}
            };
            var rcMock2 = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock2.Setup(c => c.categorias).ReturnsDbSet(listaCategoria);
            var producto = new Producto
            {
                nombre = "papa lays clasica 32g",
                NombreCategoria = "snacks",
                precioCompra = 1.8,
                precioVenta = 2.5,
                stock = 0
            };
            var resultado = validacion.RegistroProducto(rcMock.Object, rcMock2.Object, producto);
            Assert.AreEqual(false, resultado);
        }
    }
}
