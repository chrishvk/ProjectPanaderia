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

        IMarcaRepositorio Marca { get; }
        IProductoRepositorio Producto { get; }

        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }

        IAlmacenProductoRepositorio AlmacenProducto { get; }
        IInventarioRepositorio Inventario { get; }

        IInventarioDetalleRepositorio InventarioDetalle { get; }

        IKardexInventarioRepositorio KardexInventario { get; }

        ICompaniaRepositorio Compania { get; }

        ICarroCompraRepositorio CarroCompra { get; }

        IOrdenRepositorio Orden { get; }

        IOrdenDetalleRepositorio OrdenDetalle { get; }

        Task Guardar();
    }
}
