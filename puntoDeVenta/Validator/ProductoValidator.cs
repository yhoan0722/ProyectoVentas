using puntoDeVenta.Data;
using puntoDeVenta.Models;
namespace puntoDeVenta.Validator
{
    public class ProductoValidator
    {
        public bool NombreUnico(Contexto contexto, Producto producto)
        {
            return contexto.productos.Count(p => p.nombre == producto.nombre) == 0;
        }
        public bool CategoriaProductoEstadoDisponible(Contexto contexto,Producto producto)
        {
            var categoria = contexto.categorias.Where(c => c.nombre == producto.NombreCategoria).Where(c => c.estado == "disponible").FirstOrDefault();
            if(categoria!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PrecioVentaDebeSerMayorPrecioCompra(Producto producto)
        {
            if (producto.precioVenta > producto.precioCompra)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ProductoYaExiste(Contexto contexto,Producto producto) {
            var pro = contexto.productos.Where(p=>p.nombre == producto.nombre).Where(p=>p.NombreCategoria==producto.NombreCategoria).FirstOrDefault();
            if (pro != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool RegistroProducto(Contexto contexto,Contexto contexto2,Producto producto)
        {
            var productoExiste = ProductoYaExiste(contexto,producto);
            var estadoCategoria = CategoriaProductoEstadoDisponible(contexto2, producto);
            var precioVenta = PrecioVentaDebeSerMayorPrecioCompra(producto);
            var nombreProductoUnico = NombreUnico(contexto, producto);
            if (productoExiste)
            {
                if (nombreProductoUnico)
                {
                    if (estadoCategoria)
                    {
                        if (precioVenta)
                        {
                            if (producto.stock > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }
    }
}
