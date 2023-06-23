using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;

namespace puntoDeVenta.Controllers
{
    public class AccederController : Controller
    {
        private readonly Contexto _db;
        public AccederController(Contexto db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var user = validarCredenciales(usuario.correo, usuario.clave);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.nombre),
                    new Claim(ClaimTypes.NameIdentifier,user.UsuarioID.ToString()),
                    new Claim(ClaimTypes.Email,user.correo),
                    new Claim(ClaimTypes.Role,user.rol.descripcion),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Home", "Acceder");
            }
            ViewData["Mensaje"] = "El correo y/o la contraseña son incorrectas";
            return View();
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceder");
        }
        public IActionResult Home()
        {
            ClaimsPrincipal claim = HttpContext.User;
            int idUsuario;
            if (claim.Identity.IsAuthenticated)
            {
                string id = claim.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier)
                                            .Select(u => u.Value).SingleOrDefault();
                idUsuario = int.Parse(id);
                var user = _db.Usuarios.Where(u => u.UsuarioID == idUsuario).FirstOrDefault();
                ViewBag.nombre = user.nombre;
                ViewBag.id = user.UsuarioID;
                return View();
            }
            else
            {
                return NotFound();
            }

        }
        private Usuario validarCredenciales(string correo, string password)
        {
            var user = _db.Usuarios.Include(r => r.rol)
                                .Where(u => u.correo == correo)
                                .Where(u => u.clave == password)
                                .FirstOrDefault();
            return user;
        }
    }
}
