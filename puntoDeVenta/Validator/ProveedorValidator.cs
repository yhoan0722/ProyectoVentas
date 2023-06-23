using puntoDeVenta.Data;
using puntoDeVenta.Models;

namespace puntoDeVenta.Validator
{
    public class ProveedorValidator
    {
        public bool NombreUnico(Contexto contexto,Proveedor proveedor)
        {
            return contexto.proveedor.Count(p => p.nombre == proveedor.nombre) == 0;
        }
        public bool RucUnico(Contexto contexto, Proveedor proveedor)
        {
            return contexto.proveedor.Count(p => p.ruc == proveedor.ruc) == 0;
        }
        public bool LongitudRuc(Proveedor proveedor)
        {
            if (proveedor.ruc.Length == 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DireccionObligatoria (Proveedor proveedor)
        {
            if (proveedor.nombre != null)
            {
                if (proveedor.ruc != null)
                {
                    if(proveedor.direccion != null)
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
        public bool RegistroProveedor(Contexto contexto,Proveedor proveedor)
        {
            bool proveedorExiste = ValidarSiYaExisteElProveedor(contexto, proveedor.nombre, proveedor.ruc, proveedor.direccion);
            if (proveedorExiste)
            {
                if (proveedor.nombre != null)
                {
                    if (proveedor.ruc != null && proveedor.ruc.Length == 11)
                    {
                        if (proveedor.direccion != null)
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
        public bool ValidarSiYaExisteElProveedor(Contexto contexto,string nombre,string ruc,string direccion)
        {
            var proveedor = contexto.proveedor.Where(p=>p.nombre==nombre).Where(p=>p.ruc==ruc).Where(p=>p.direccion==direccion).FirstOrDefault();
            if(proveedor != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
