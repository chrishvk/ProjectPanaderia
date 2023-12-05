using Microsoft.AspNetCore.Mvc.Rendering;
using Panaderia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio.IRepositorio
{
    public interface IKardexInventarioRepositorio : IRepositorio<KardexInventario>
    {
        Task RegistrarKardex(int almacenProdutoId, string tipo, string detalle, int stockAnterior, int cantidad, string usuarioId);
    }
}
