using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;

namespace puntoDeVenta.Controllers
{
    public class ProductoController : Controller
    {
        private readonly Contexto _db;
        public ProductoController(Contexto db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            IEnumerable<Producto> productos = _db.productos;
            int contador = productos.Count();
            ViewBag.contador = contador;
            return View(productos);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            IEnumerable<SelectListItem> categoria = _db.categorias.Where(c => c.estado.Equals("disponible"))
                .Select(c => new SelectListItem
                {
                    Text = c.nombre,
                    Value = c.nombre
                });
            ViewBag.categorias = categoria;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto, IFormFile img)
        {
            if(img == null)
            {
                if (ModelState.IsValid)
                {
                    producto.imagen = "";
                    _db.productos.Add(producto);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Stream image = img.OpenReadStream();
                string imagen = await SubirStorage(image, img.FileName);
                if (ModelState.IsValid)
                {
                    producto.imagen = imagen;
                    _db.productos.Add(producto);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(producto);
        }
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var producto = _db.productos.Find(id);
            if(producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _db.productos.Update(producto);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }
        [HttpGet]
        public IActionResult Eliminar(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var producto = _db.productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
        [HttpPost]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            else
            {
                _db.productos.Remove(producto);
                _db.SaveChanges();
                return RedirectToAction("Index", "Producto");
            }
        }
        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            //INGRESA AQUÍ TUS PROPIAS CREDENCIALES
            string email = "josuecalvanapon.2010@gmail.com";
            string clave = "Omar123";
            string ruta = "profinaldiars.appspot.com";
            string api_key = "AIzaSyDj5UzbNqRQ2Vwi-6D5CiobWsUcBCtCDZ0";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("Fotos_Perfil")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);
            var downloadURL = await task;
            return downloadURL;
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
