using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {

        IAlmacenRepositorio Almacen {  get; }
        ICategoriaRepositorio Categoria { get; }

        Task Guardar();
    }
}
