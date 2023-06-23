using Microsoft.AspNetCore.Components.Forms;
using puntoDeVenta.Data;
using puntoDeVenta.Models;
namespace puntoDeVenta.Validator
{
    public class CompraValidator
    {
        public bool RegistroCompra(Contexto contexto, Contexto contexto2, Compra compra)
        {
            var estadoProducto = estadoDelProducto(contexto, compra);
            var productoExiste = existeProducto(contexto, compra);
            var proveedorExiste = existeProveedor(contexto2, compra);
            if (productoExiste)
            {
                if (estadoProducto)
                {
                    if (proveedorExiste)
                    {
                        if (compra.cantidad > 0)
                        {
                            if (compra.nombreProveedor != null)
                            {
                                if (compra.precioCompra > 0)
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
        public bool estadoDelProducto(Contexto contexto, Compra compra)
        {
            var producto = contexto.productos.Where(p => p.nombre == compra.NombreProducto).Where(p => p.estado == "disponible").FirstOrDefault();
            if (producto != null) {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeProducto (Contexto contexto,Compra compra)
        {
            var producto = contexto.productos.Where(p => p.nombre == compra.NombreProducto).FirstOrDefault();
            if (producto != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool existeProveedor(Contexto contexto,Compra compra)
        {
            var proveedor = contexto.proveedor.Where(p=>p.nombre==compra.nombreProveedor).FirstOrDefault();
            if(proveedor!= null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
