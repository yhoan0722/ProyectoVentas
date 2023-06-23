using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
using System.Security.Claims;

namespace puntoDeVenta.Controllers
{
    [Authorize]
    public class ProveedorController : Controller
    {
        private readonly Contexto _db;
        public ProveedorController(Contexto db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> ListaProveedor(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre;
            ViewBag.id = user.UsuarioID;
            var listaProveedor = _db.proveedor.ToList();
            int contador = listaProveedor.Count();
            ViewBag.contador= contador;
            Proveedor Proveedor = new Proveedor();
            if (id == null)
            {
                return View(Proveedor);
            }
            else
            {
                Proveedor = await _db.proveedor.FindAsync(id);
                return View(Proveedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaProveedor(Proveedor Proveedor)
        {
            if (ModelState.IsValid)
            {
                if (Proveedor.ProveedorID == 0) //id=0 crear el registro
                {
                    await _db.proveedor.AddAsync(Proveedor);
                    await _db.SaveChangesAsync();
                    ViewData["Mensaje"] = "Proveedor creado exitosamente!!";
                    return RedirectToAction(nameof(ListaProveedor));
                }
                else
                {
                    _db.proveedor.Update(Proveedor);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ListaProveedor), new { id = 0 });
                }
            }
            return View(Proveedor);
        }
        [HttpGet]
        public async Task<IActionResult> obtenerTodos()
        {
            var todos = await _db.proveedor.ToListAsync();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var Proveedor = await _db.proveedor.FindAsync(id);
            if (Proveedor == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.proveedor.Remove(Proveedor);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Proveedor borrado exitosamente" });
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
