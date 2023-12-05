using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panaderia.Acceso.Datos.Data;
using Panaderia.AccesoDatos.Repositorio.IRepositorio;
using Panaderia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio
{
    public class KardexInventarioRepositorio : Repositorio<KardexInventario>, IKardexInventarioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public KardexInventarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarKardex(int almacenProdutoId, string tipo, string detalle, int stockAnterior, int cantidad, string usuarioId)
        {
            var almacenProducto = await _db.AlmacenesProductos.Include(b => b.Producto).FirstOrDefaultAsync(b => b.Id == almacenProdutoId);

            if(tipo == "Entrada")
            {
                KardexInventario kardex = new KardexInventario();
                kardex.AlmacenProductoId = almacenProdutoId;
                kardex.Tipo = tipo;
                kardex.Detalle = detalle;
                kardex.StockAnterior = stockAnterior;
                kardex.Cantidad = cantidad;
                kardex.Costo = almacenProducto.Producto.Costo;
                kardex.Stock = stockAnterior + cantidad;
                kardex.Total = kardex.Stock * kardex.Costo;
                kardex.UsuarioAplicacionId = usuarioId;
                kardex.FechaRegistro = DateTime.Now;

                await _db.KardexInventarios.AddAsync(kardex);
                await _db.SaveChangesAsync();
            }
            if (tipo == "Salida")
            {
                KardexInventario kardex = new KardexInventario();
                kardex.AlmacenProductoId = almacenProdutoId;
                kardex.Tipo = tipo;
                kardex.Detalle = detalle;
                kardex.StockAnterior = stockAnterior;
                kardex.Cantidad = cantidad;
                kardex.Costo = almacenProducto.Producto.Costo;
                kardex.Stock = stockAnterior - cantidad;
                kardex.Total = kardex.Stock * kardex.Costo;
                kardex.UsuarioAplicacionId = usuarioId;
                kardex.FechaRegistro = DateTime.Now;

                await _db.KardexInventarios.AddAsync(kardex);
                await _db.SaveChangesAsync();
            }

        }
    }
}
