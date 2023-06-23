
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;

namespace puntoDeVenta.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Contexto _db;
        public UsuarioController(Contexto db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> ListaUsuario(int? id)
        {
            Usuario usuario = new Usuario();
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            IEnumerable<SelectListItem> roles = _db.rols.
                Select(r => new SelectListItem
                {
                    Text = r.descripcion,
                    Value = r.RolID.ToString()
                });
            ViewBag.roles = roles;
            if (id == null)
            {
                return View(usuario);
            }
            else
            {
                usuario = await _db.Usuarios.FindAsync(id);
                return View(usuario);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.UsuarioID == 0) //id=0 crear el registro
                {
                   await _db.Usuarios.AddAsync(usuario);
                   await _db.SaveChangesAsync();
                   return RedirectToAction(nameof(ListaUsuario));
                }
                else
                {
                    _db.Usuarios.Update(usuario);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ListaUsuario), new {id=0});
                }
            }
            return View(usuario);
        }
        [HttpGet]
        public async Task<IActionResult> obtenerTodos()
        {
            var todos = await _db.Usuarios.ToListAsync();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var usuario = await _db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Usuario borrado exitosamente" });
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
