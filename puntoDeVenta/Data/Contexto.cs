using Microsoft.EntityFrameworkCore;
using puntoDeVenta.Models;

namespace puntoDeVenta.Data
{
    public class Contexto:DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Rol> rols { get; set; }
        public virtual DbSet<Proveedor> proveedor { get; set; }
        public virtual DbSet<Categoria> categorias { get; set; }
        public virtual DbSet<Producto> productos { get; set; }
        public virtual DbSet<Compra> compra { get; set; }
        public DbSet<BlocCompra> blocCompras { get; set; }
        public DbSet<ListaCompra> listaCompras { get; set; }
        public DbSet<BlocVenta> blocVentas { get; set; }
        public DbSet<ListaVenta> listaVentas { get; set; }
        public virtual DbSet<Ventas> ventas { get; set; }
        public DbSet<NumeroCorrelativo> numeroCorrelativos { get; set; }
    }
}
