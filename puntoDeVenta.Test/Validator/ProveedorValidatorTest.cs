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
    public class ProveedorValidatorTest
    {
        [Test]
        public void NombreProveedorUnico()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.proveedor).ReturnsDbSet(new List<Proveedor>
            {
                new Proveedor{nombre="DistribuidoraChaliSAC"},
                new Proveedor{nombre="DistribuidoraDonTheoSAC"},
                new Proveedor{nombre="Coca-Cola"},
                new Proveedor{nombre="DistribuidoraRacseSAC"},
            });
            var context = contextMock.Object;
            var validator = new ProveedorValidator();
            var resultado = validator.NombreUnico(context, new Proveedor { nombre = "DistribuidoraChaliSAC" });
            Assert.AreEqual(false,resultado);
        }
        [Test]
        public void NombreProveedorUnico2()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.proveedor).ReturnsDbSet(new List<Proveedor>
            {
                new Proveedor{nombre="DistribuidoraChaliSAC"},
                new Proveedor{nombre="DistribuidoraDonTheoSAC"},
                new Proveedor{nombre="Coca-Cola"},
                new Proveedor{nombre="DistribuidoraRacseSAC"},
            });
            var context = contextMock.Object;
            var validator = new ProveedorValidator();
            var resultado = validator.NombreUnico(context, new Proveedor { nombre = "Coca-Cola" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NombreProveedorUnico3()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.proveedor).ReturnsDbSet(new List<Proveedor>
            {
                new Proveedor{nombre="DistribuidoraChaliSAC"},
                new Proveedor{nombre="DistribuidoraDonTheoSAC"},
                new Proveedor{nombre="Coca-Cola"},
                new Proveedor{nombre="DistribuidoraRacseSAC"},
            });
            var context = contextMock.Object;
            var validator = new ProveedorValidator();
            var resultado = validator.NombreUnico(context, new Proveedor { nombre = "Backus" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RucProveedorUnico()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.proveedor).ReturnsDbSet(new List<Proveedor>
            {
                new Proveedor{nombre="DistribuidoraChaliSAC",ruc="2016735723"},
                new Proveedor{nombre="DistribuidoraDonTheoSAC",ruc="2011235434"},
                new Proveedor{nombre="Coca-Cola",ruc="2023452655"},
                new Proveedor{nombre="DistribuidoraRacseSAC",ruc="2014242357"},
            });
            var context = contextMock.Object;
            var validator = new ProveedorValidator();
            var resultado = validator.RucUnico(context, new Proveedor { nombre = "Backus" ,ruc= "2011235434" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RucProveedorUnico2()
        {
            var contextMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            contextMock.Setup(p => p.proveedor).ReturnsDbSet(new List<Proveedor>
            {
                new Proveedor{nombre="DistribuidoraChaliSAC",ruc="2016735723"},
                new Proveedor{nombre="DistribuidoraDonTheoSAC",ruc="2011235434"},
                new Proveedor{nombre="Coca-Cola",ruc="2023452655"},
                new Proveedor{nombre="DistribuidoraRacseSAC",ruc="2014242357"},
            });
            var context = contextMock.Object;
            var validator = new ProveedorValidator();
            var resultado = validator.RucUnico(context, new Proveedor { nombre = "Backus",ruc="2019324576" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void LongitudRucProveedor()
        {
            var validacion = new ProveedorValidator();
            var resultado = validacion.LongitudRuc(new Proveedor { ruc = "20135343657" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void LongitudRucProveedor2()
        {
            var validacion = new ProveedorValidator();
            var resultado = validacion.LongitudRuc(new Proveedor { ruc = "201353436574" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void DireccionProveedorEsObligatiorio()
        {
            var validacion = new ProveedorValidator();
            var resultado = validacion.DireccionObligatoria(new Proveedor { nombre = "Coca-Cola", ruc = "20135456546", direccion = "Av. San Martin #1321" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void DireccionProveedorEsObligatiorio2()
        {
            var validacion = new ProveedorValidator();
            var resultado = validacion.DireccionObligatoria(new Proveedor { nombre = "Coca-Cola", ruc = "20135456546" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroCorrectamenteElProveedor()
        {
            var validacion = new ProveedorValidator();
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="DistribuidoraChaliSAC",ruc="20135343657",direccion="Av. 12345"},
                new Proveedor {nombre="DistribuidoraDonTheoSAC",ruc="20112354342",direccion="Av. San Nicolas #123"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c=>c.proveedor).ReturnsDbSet(listaProveedor);
            var proveedor = new Proveedor
            {
                nombre="Backus",ruc="20152334367",direccion="Av. San Martin #1536"
            };
            var resultado = validacion.RegistroProveedor(rcMock.Object, proveedor);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RegistroIncorrectoElProveedorYaExiste()
        {
            var validacion = new ProveedorValidator();
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="DistribuidoraChaliSAC",ruc="20135343657",direccion="Av. 12345"},
                new Proveedor {nombre="DistribuidoraDonTheoSAC",ruc="20112354342",direccion="Av. San Nicolas #123"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c => c.proveedor).ReturnsDbSet(listaProveedor);
            var proveedor = new Proveedor
            {
                nombre = "DistribuidoraChaliSAC",
                ruc = "20135343657",
                direccion = "Av. 12345"
            };
            var resultado = validacion.RegistroProveedor(rcMock.Object, proveedor);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoNombreProveedorNulo()
        {
            var validacion = new ProveedorValidator();
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="DistribuidoraChaliSAC",ruc="20135343657",direccion="Av. 12345"},
                new Proveedor {nombre="DistribuidoraDonTheoSAC",ruc="20112354342",direccion="Av. San Nicolas #123"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c => c.proveedor).ReturnsDbSet(listaProveedor);
            var proveedor = new Proveedor
            {
                ruc = "20152334367",
                direccion = "Av. San Martin #1536"
            };
            var resultado = validacion.RegistroProveedor(rcMock.Object, proveedor);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoRucProveedorEsIncorrecto()
        {
            var validacion = new ProveedorValidator();
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="DistribuidoraChaliSAC",ruc="20135343657",direccion="Av. 12345"},
                new Proveedor {nombre="DistribuidoraDonTheoSAC",ruc="20112354342",direccion="Av. San Nicolas #123"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c => c.proveedor).ReturnsDbSet(listaProveedor);
            var proveedor = new Proveedor
            {
                nombre = "Backus",
                ruc = "201523343",
                direccion = "Av. San Martin #1536"
            };
            var resultado = validacion.RegistroProveedor(rcMock.Object, proveedor);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroIncorrectoDireccionProveedorEsNulo()
        {
            var validacion = new ProveedorValidator();
            var listaProveedor = new List<Proveedor>
            {
                new Proveedor {nombre="DistribuidoraChaliSAC",ruc="20135343657",direccion="Av. 12345"},
                new Proveedor {nombre="DistribuidoraDonTheoSAC",ruc="20112354342",direccion="Av. San Nicolas #123"}
            };
            var rcMock = new Mock<Contexto>(new DbContextOptions<Contexto>());
            rcMock.Setup(c => c.proveedor).ReturnsDbSet(listaProveedor);
            var proveedor = new Proveedor
            {
                nombre = "Backus",
                ruc = "20152334367"
            };
            var resultado = validacion.RegistroProveedor(rcMock.Object, proveedor);
            Assert.AreEqual(false, resultado);
        }
    }
}
