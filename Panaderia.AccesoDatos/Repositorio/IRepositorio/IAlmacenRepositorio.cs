using Panaderia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio.IRepositorio
{
    public interface IAlmacenRepositorio : IRepositorio<Almacen>
    {
        void Actualizar(Almacen almacen);

    }
}
