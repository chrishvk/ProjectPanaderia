using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AlmacenProductoRepositorio : Repositorio<AlmacenProducto>, IAlmacenProductoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public AlmacenProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(AlmacenProducto almacenProducto)
        {
            var almacenProductoBD = _db.AlmacenesProductos.FirstOrDefault(b => b.Id == almacenProducto.Id);
            if (almacenProductoBD != null)
            {

                almacenProductoBD.Cantidad = almacenProducto.Cantidad;


                _db.SaveChanges();
            }
        }

        
    }
}
