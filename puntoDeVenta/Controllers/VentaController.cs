using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Extensions;

namespace puntoDeVenta.Controllers
{
    public class VentaController : Controller
    {
        private readonly Contexto _db;
        private readonly IConverter _converter;
        public VentaController(Contexto db, IConverter converter)
        {
            _db = db;
            _converter = converter;
        }
        public IActionResult Index(string? mensaje)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            var listaBlocVenta = _db.blocVentas;
            double total = 0;
            var lista = _db.blocVentas.ToList();
            for(int i=0; i < lista.Count; i++)
            {
                total = total + lista[i].total;
            }
            if (mensaje != null)
            { 
                ViewBag.error = mensaje;
            }
            ViewBag.total = total;
            return View(listaBlocVenta);
        }
        [HttpGet]
        public IActionResult ListaVenta()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            double total = 0;
            var listaVenta = _db.ventas.ToList();
            foreach(var item in listaVenta)
            {
                total=total + item.total;
            }
            ViewBag.total = total;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> obtenerListaVentas()
        {
            var venta = _db.ventas.ToList();
            if (venta.Count > 0)
            {
                foreach (var item in venta)
                {
                    AgregarListaVenta(item);
                }
            }
            var lista = await _db.listaVentas.ToListAsync();
            return Json(new { data = lista });
        }
        public IActionResult BuscarProducto()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpGet]
        public IActionResult Agregar(int?id)
        {
            var listaBlocVenta = _db.blocVentas.ToList();
            if (id==0 || id == null)
            {
                return NotFound();
            }
            var producto = _db.productos.Find(id);
            if(producto == null)
            {
                return NotFound();
            }
            ViewBag.producto = producto.nombre;
            ViewBag.categoria = producto.NombreCategoria;
            ViewBag.precio = producto.precioVenta;
            ViewBag.imagen = producto.imagen;
            ViewBag.stock = producto.stock;
            if (listaBlocVenta.Count > 0)
            {
                foreach (var item in listaBlocVenta)
                {
                    if (item.nombreProducto == producto.nombre && producto.stock == 1)
                    {
                        return RedirectToAction("Index", new { mensaje = "error" });
                    }
                    if (item.nombreProducto == producto.nombre)
                    {
                        item.nombreProducto = producto.nombre;
                        item.nombreCategoria = producto.NombreCategoria;
                        item.precio=producto.precioVenta;
                        item.imagen = producto.imagen;
                        item.fecha = DateTime.Today;
                        item.cantidad = item.cantidad + 1;
                        item.total=item.cantidad*item.precio;
                        item.igv = item.total * 0.18;
                        _db.blocVentas.Update(item);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Agregar(BlocVenta blocVenta,int? stock)
        {
            blocVenta.total = blocVenta.cantidad * blocVenta.precio;
            blocVenta.igv = blocVenta.total * 0.18;
            blocVenta.fecha = DateTime.Today;
            if (ModelState.IsValid)
            {
                if (stock == 0)
                {
                    return RedirectToAction("Index", new {mensaje="error"});
                }
                _db.blocVentas.Add(blocVenta);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blocVenta);
        }
        [HttpGet]
        public IActionResult AgregarUno(int? id)
        {
            var listaBlocVenta = _db.blocVentas.ToList();
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var blocVenta = _db.blocVentas.Find(id);
            if (blocVenta == null)
            {
                return NotFound();
            }
            var producto = _db.productos.Where(p => p.nombre == blocVenta.nombreProducto).FirstOrDefault();
            if (blocVenta.nombreProducto == producto.nombre && producto.stock == 1)
            {
                return RedirectToAction("Index", new { mensaje = "error" });
            }
            if (blocVenta.cantidad>producto.stock)
            {
                return RedirectToAction("Index", new { mensaje = "error" });
            }
            if (listaBlocVenta.Count > 0)
            {
                foreach (var item in listaBlocVenta)
                {
                    if (item.nombreProducto == blocVenta.nombreProducto)
                    {
                        item.nombreProducto = blocVenta.nombreProducto;
                        item.nombreCategoria = blocVenta.nombreCategoria;
                        item.precio = blocVenta.precio;
                        item.imagen = blocVenta.imagen;
                        item.fecha = DateTime.Today;
                        item.cantidad = item.cantidad + 1;
                        item.total = item.cantidad * item.precio;
                        item.igv = item.total * 0.18;
                        _db.blocVentas.Update(item);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult QuitarUno(int? id)
        {
            var listaBlocVenta = _db.blocVentas.ToList();
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var blocVenta = _db.blocVentas.Find(id);
            if (blocVenta == null)
            {
                return NotFound();
            }
            if (listaBlocVenta.Count > 0)
            {
                foreach (var item in listaBlocVenta)
                {
                    if (item.nombreProducto == blocVenta.nombreProducto)
                    {
                        item.nombreProducto = blocVenta.nombreProducto;
                        item.nombreCategoria = blocVenta.nombreCategoria;
                        item.precio = blocVenta.precio;
                        item.imagen = blocVenta.imagen;
                        item.fecha = DateTime.Today;
                        item.cantidad = item.cantidad - 1;
                        item.total = item.cantidad * item.precio;
                        item.igv = item.total * 0.18;
                        _db.blocVentas.Update(item);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> obtenerProductos()
        {
            var lista = await _db.productos.Where(p=>p.estado=="disponible").ToListAsync();
            return Json(new { data = lista });
        }
        [HttpGet]
        public IActionResult EliminarProducto(int?id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var blocVenta = _db.blocVentas.Find(id);
            if (blocVenta == null)
            {
                return NotFound();
            }
            return View(blocVenta);
        }
        [HttpPost]
        public IActionResult EliminarProducto(BlocVenta blocVenta)
        {
            if (blocVenta == null)
            {
                return NotFound();
            }
            else
            {
                _db.blocVentas.Remove(blocVenta);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult BoletaVenta()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            double total = 0;
            var lista = _db.blocVentas.ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                total = total + lista[i].total;
            }
            ViewBag.total = total;
            return View();
        }
        [HttpPost]
        public IActionResult CrearBoletaVenta(string? nombreCliente)
        {
            return RedirectToAction("ConfirmarVenta", new {nombre=nombreCliente});
        }
        public Ventas AgregarBoletaVenta(BlocVenta venta, string numeroVenta,string nombreCliente)
        {
            var user = obtenerUsuario();
            Ventas ventaGenerada = new Ventas();
            ventaGenerada.UsuarioID = user.UsuarioID;
            ventaGenerada.numeroVenta = numeroVenta;
            ventaGenerada.nombreUsuario = user.nombre;
            ventaGenerada.nombreCliente = nombreCliente;
            ventaGenerada.nombreProducto = venta.nombreProducto;
            ventaGenerada.nombreCategoria = venta.nombreCategoria;
            ventaGenerada.cantidad = venta.cantidad;
            ventaGenerada.precio= venta.precio;
            ventaGenerada.total = venta.total;
            ventaGenerada.igv = venta.igv;
            ventaGenerada.tipoVenta = "boleta";
            ventaGenerada.fecha = venta.fecha;
            var producto = _db.productos.Where(p => p.nombre.Equals(ventaGenerada.nombreProducto)).FirstOrDefault();
            producto.stock = producto.stock - ventaGenerada.cantidad;
            _db.ventas.Add(ventaGenerada);
            _db.SaveChanges();
            return ventaGenerada;
        }
        [HttpGet]
        public IActionResult NotaVenta()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            double total = 0;
            var lista = _db.blocVentas.ToList();
            for (int i = 0; i < lista.Count; i++)
            {
                total = total + lista[i].total;
            }
            ViewBag.total = total;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearNotaVenta()
        {
            Ventas ventaGenerada = new Ventas();
            NumeroCorrelativo correlativo = _db.numeroCorrelativos.Where(n => n.Gestion == "venta").First();
            correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
            correlativo.FechaActualizacion = DateTime.Now;
            _db.numeroCorrelativos.Update(correlativo);
            await _db.SaveChangesAsync();
            string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
            string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
            numeroVenta = numeroVenta.Substring(numeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);
            List<BlocVenta> lista = await _db.blocVentas.ToListAsync();
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    AgregarNotaVenta(item, numeroVenta);
                }
                foreach (var item in lista)
                {
                    _db.blocVentas.Remove(item);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public Ventas AgregarNotaVenta(BlocVenta venta, string numeroVenta)
        {
            var user = obtenerUsuario();
            Ventas ventaGenerada = new Ventas();
            ventaGenerada.UsuarioID = user.UsuarioID;
            ventaGenerada.numeroVenta = numeroVenta;
            ventaGenerada.nombreUsuario = user.nombre;
            ventaGenerada.nombreCliente = "null";
            ventaGenerada.nombreProducto = venta.nombreProducto;
            ventaGenerada.nombreCategoria = venta.nombreCategoria;
            ventaGenerada.cantidad = venta.cantidad;
            ventaGenerada.precio = venta.precio;
            ventaGenerada.total = venta.total;
            ventaGenerada.igv = venta.igv;
            ventaGenerada.tipoVenta = "nota";
            ventaGenerada.fecha = venta.fecha;
            var producto = _db.productos.Where(p => p.nombre.Equals(ventaGenerada.nombreProducto)).FirstOrDefault();
            producto.stock = producto.stock - ventaGenerada.cantidad;
            _db.ventas.Add(ventaGenerada);
            _db.SaveChanges();
            return ventaGenerada;
        }
        public void AgregarListaVenta(Ventas ventaGenerada)
        {
            ListaVenta listaVentas = new ListaVenta();
            listaVentas.numeroVenta = ventaGenerada.numeroVenta;
            listaVentas.total = ventaGenerada.total;
            listaVentas.fecha = ventaGenerada.fecha;
            listaVentas.tipoVenta = ventaGenerada.tipoVenta;
            var co = _db.ventas.Where(c => c.numeroVenta.Equals(ventaGenerada.numeroVenta)).FirstOrDefault();
            listaVentas.total = listaVentas.total + co.total;
            if (_db.listaVentas.Count(c => c.numeroVenta.Equals(ventaGenerada.numeroVenta)) == 1)
            {
                var venta = _db.listaVentas.Where(c => c.numeroVenta.Equals(ventaGenerada.numeroVenta)).FirstOrDefault();
                EliminarVentaDeLaLista(venta);
            }
            _db.listaVentas.Add(listaVentas);
            _db.SaveChanges();
        }
        public void EliminarVentaDeLaLista(ListaVenta venta)
        {
            _db.listaVentas.Remove(venta);
            _db.SaveChanges();
        }
        [HttpGet]
        public IActionResult ConfirmarVenta(string? nombre)
        {
            IEnumerable<BlocVenta> listaBlocventa = _db.blocVentas;
            var lista = _db.blocVentas.ToList();
            double total = 0;
            foreach(var item in lista)
            {
                total += item.total;
            }
            ViewBag.total = total;
            ViewBag.nombre = nombre;
            return View(listaBlocventa);
        }
        [HttpPost]
        public async Task<IActionResult> Confirmar(string? nombreCliente)
        {
            Ventas ventaGenerada = new Ventas();
            NumeroCorrelativo correlativo = _db.numeroCorrelativos.Where(n => n.Gestion == "venta").First();
            correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
            correlativo.FechaActualizacion = DateTime.Now;
            _db.numeroCorrelativos.Update(correlativo);
            await _db.SaveChangesAsync();
            string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
            string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
            numeroVenta = numeroVenta.Substring(numeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);
            List<BlocVenta> lista = await _db.blocVentas.ToListAsync();
            if (lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    AgregarBoletaVenta(item, numeroVenta, nombreCliente);
                }
                foreach (var item in lista)
                {
                    _db.blocVentas.Remove(item);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult MostrarPDFenPagina()
        {
            string pagina_actual = HttpContext.Request.Path;
            string urlPagina = HttpContext.Request.GetEncodedUrl();
            urlPagina = urlPagina.Replace(pagina_actual, "");
            urlPagina = $"{urlPagina}/Venta/ConfirmarVenta";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize=PaperKind.A4,
                    Orientation=Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page=urlPagina
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            return File(archivoPDF,"application/pdf");
        }
        public IActionResult DescargarPDFenPagina()
        {
            string pagina_actual = HttpContext.Request.Path;
            string urlPagina = HttpContext.Request.GetEncodedUrl();
            urlPagina = urlPagina.Replace(pagina_actual, "");
            urlPagina = $"{urlPagina}/Venta/ConfirmarVenta";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page=urlPagina
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            string nombrePDF = "boletaVenta" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            return File(archivoPDF, "application/pdf",nombrePDF);
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
            var venta = _db.listaVentas.Find(id);
            if (venta == null)
            {
                return NotFound();
            }
            IEnumerable<Ventas> listaVentas = _db.ventas.Where(c => c.numeroVenta == venta.numeroVenta);
            double total = 0;
            var listVen = listaVentas.ToList();
            foreach(var item in listVen)
            {
                total=total+item.total;
            }
            ViewBag.total = total;
            return View(listaVentas);
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
