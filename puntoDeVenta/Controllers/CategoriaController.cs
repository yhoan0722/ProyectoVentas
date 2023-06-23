using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;

namespace puntoDeVenta.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly Contexto _db;
        public CategoriaController(Contexto db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> ListaCategoria(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            Categoria categoria = new Categoria();
            var listaCategoria = _db.categorias.ToList();
            int contador = listaCategoria.Count();
            ViewBag.contador = contador;
            if (id == null)
            {
                return View(categoria);
            }
            else
            {
                categoria = await _db.categorias.FindAsync(id);
                return View(categoria);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.CategoriaID == 0) //id=0 crear el registro
                {
                    await _db.categorias.AddAsync(categoria);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ListaCategoria));
                }
                else
                {
                    _db.categorias.Update(categoria);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ListaCategoria), new { id = 0 });
                }
            }
            return View(categoria);
        }
        [HttpGet]
        public async Task<IActionResult> obtenerTodos()
        {
            var todos = await _db.categorias.ToListAsync();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var categoria = await _db.categorias.FindAsync(id);
            if (categoria == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.categorias.Remove(categoria);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Categoria borrado exitosamente" });
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
