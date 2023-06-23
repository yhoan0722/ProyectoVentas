using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace puntoDeVenta.Controllers
{
    public class CompraController : Controller
    {
        private readonly Contexto _db;
        public CompraController(Contexto db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult ListaCompra()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> obtenerListaCompras()
        {
            var compra = _db.compra.ToList();
            if (compra.Count > 0)
            {
                foreach (var item in compra)
                {
                    AgregarListaCompra(item);
                }
            }
            var lista = await _db.listaCompras.ToListAsync();
            return Json(new { data = lista });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var listaCompra = await _db.listaCompras.FindAsync(id);
            var listCompras = await _db.compra.ToListAsync();
            if (listaCompra == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            foreach(var item in listCompras)
            {
                if (item.numeroCompra == listaCompra.numeroCompra)
                {
                    Delete(item);
                }
            }
            _db.listaCompras.Remove(listaCompra);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Lista de compra fue borrado exitosamente" });
        }
        public void Delete(Compra compra)
        {
            _db.compra.Remove(compra);
            _db.SaveChanges();
        }
        [HttpGet]
        public IActionResult RegistrarCompra(string? mensaje) {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            var listaBlocCompra = _db.blocCompras;
            IEnumerable<SelectListItem> proveedor = _db.proveedor
                .Select(c => new SelectListItem
                {
                    Text = c.nombre,
                    Value = c.nombre
                });
            ViewBag.proveedores = proveedor;
            if (mensaje != null)
            {
                ViewBag.error = "Debe de ingresar el nombre del Proveedor";
            }
            return View(listaBlocCompra);
        }
        [HttpGet]
        public async Task<IActionResult> obtenerProductos()
        {
            var lista = await _db.productos.ToListAsync();
            return Json(new { data = lista });
        }
        [HttpGet]
        public async Task<IActionResult> obtenerBlocCompra()
        {
            var listaBlocCompra = await _db.blocCompras.ToListAsync();
            return Json(new {data= listaBlocCompra });
        }
        [HttpGet]
        public IActionResult AgregarCompra(int?id)
        {
            var producto = _db.productos.Find(id);
            var listaBlocCompra = _db.blocCompras.ToList();
            if (producto == null)
            {
                return NotFound();
            }
            if (listaBlocCompra.Count > 0)
            {
                foreach(var item in listaBlocCompra)
                {
                    if (item.NombreProducto == producto.nombre)
                    {
                        return RedirectToAction("Advertencia", "Compra");
                    }
                }
            }
            ViewBag.producto = producto.nombre;
            return View();
        }
        [HttpGet]
        public IActionResult Advertencia()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpPost]
        public IActionResult AgregarCompra(BlocCompra blocCompra)
        {
            blocCompra.importe = blocCompra.cantidad * blocCompra.precioCompra;
            if (ModelState.IsValid)
            {
                _db.blocCompras.Add(blocCompra);
                _db.SaveChanges();
                return RedirectToAction("RegistrarCompra");
            }
            return View(blocCompra);
        }
        [HttpGet]
        public IActionResult EliminarCompra(int?id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var blocCompra = _db.blocCompras.Find(id);
            if (blocCompra == null)
            {
                return NotFound();
            }
            return View(blocCompra);
        }
        [HttpPost]
        public IActionResult EliminarCompra(BlocCompra blocCompra)
        {
            if (blocCompra == null)
            {
                return NotFound();
            }
            else
            {
                _db.blocCompras.Remove(blocCompra);
                _db.SaveChanges();
                return RedirectToAction("RegistrarCompra");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CrearCompra(string? nombreProveedor)
        {
            if(nombreProveedor== null)
            {
                return RedirectToAction("RegistrarCompra", "Compra",new {mensaje="error"});
            }
            else
            {
                Compra compraGenerada = new Compra();
                NumeroCorrelativo correlativo = _db.numeroCorrelativos.Where(n => n.Gestion == "compra").First();
                correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                correlativo.FechaActualizacion = DateTime.Now;
                _db.numeroCorrelativos.Update(correlativo);
                await _db.SaveChangesAsync();
                string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
                string numeroCompra = ceros + correlativo.UltimoNumero.ToString();
                numeroCompra = numeroCompra.Substring(numeroCompra.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);
                List<BlocCompra> lista = await _db.blocCompras.ToListAsync();
                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        AgregarCompra(item, numeroCompra, nombreProveedor);
                    }
                    foreach (var item in lista)
                    {
                        _db.blocCompras.Remove(item);
                        await _db.SaveChangesAsync();
                    }
                    return RedirectToAction("ListaCompra", "Compra");
                }
                else
                {
                    return NotFound();
                }
            }
        }
        public Compra AgregarCompra (BlocCompra compra,string numeroCompra,string nombreProveedor)
        {
            Compra compraGenerada = new Compra();
            compraGenerada.NombreProducto = compra.NombreProducto;
            compraGenerada.nombreProveedor = nombreProveedor;
            compraGenerada.precioCompra = (double)compra.precioCompra;
            compraGenerada.cantidad = (int)compra.cantidad;
            compraGenerada.fechaEntrega = DateTime.Today;
            compraGenerada.importe = compraGenerada.precioCompra * compraGenerada.cantidad;
            compraGenerada.numeroCompra = numeroCompra;
            var producto = _db.productos.Where(p => p.nombre.Equals(compraGenerada.NombreProducto)).FirstOrDefault();
            producto.stock = producto.stock + compraGenerada.cantidad;
            producto.precioCompra=compraGenerada.precioCompra;
            _db.compra.Add(compraGenerada);
            _db.SaveChanges();
            return compraGenerada;
        }
        public void AgregarListaCompra(Compra compraGenerada)
        {
            ListaCompra listaCompra = new ListaCompra();
            listaCompra.numeroCompra = compraGenerada.numeroCompra;
            listaCompra.total = compraGenerada.importe;
            listaCompra.fecha = compraGenerada.fechaEntrega;
            var co = _db.compra.Where(c => c.numeroCompra.Equals(compraGenerada.numeroCompra)).FirstOrDefault();
            listaCompra.total = listaCompra.total + co.importe;
            if (_db.listaCompras.Count(c => c.numeroCompra.Equals(compraGenerada.numeroCompra)) == 1)
            {
                var compra = _db.listaCompras.Where(c => c.numeroCompra.Equals(compraGenerada.numeroCompra)).FirstOrDefault();
                EliminarCompraDeLaLista(compra);
            }
            _db.listaCompras.Add(listaCompra);
            _db.SaveChanges();
        }
        public void EliminarCompraDeLaLista(ListaCompra compra)
        {
            _db.listaCompras.Remove(compra);
            _db.SaveChanges();
        }
        [HttpGet]
        public IActionResult Detalle(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var compra = _db.listaCompras.Find(id);
            if(compra == null)
            {
                return NotFound();
            }
            IEnumerable<Compra> listaCompra = _db.compra.Where(c => c.numeroCompra==compra.numeroCompra);
            return View(listaCompra);
        }
        public Usuario obtenerUsuario()
        {
            ClaimsPrincipal claim = HttpContext.User;
            int idUsuario;
            Usuario user = new Usuario();
            if (claim.Identity.IsAuthenticated)
            {
                string id = claim.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier)
                                            .Select(u => u.Value).SingleOrDefault();
                idUsuario = int.Parse(id);
                user = _db.Usuarios.Where(u => u.UsuarioID == idUsuario).FirstOrDefault();
                return user;
            }
            else
            {
                return user;
            }
        }
    }
}
