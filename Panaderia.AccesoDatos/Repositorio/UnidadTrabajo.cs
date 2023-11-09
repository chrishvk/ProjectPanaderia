using Panaderia.Acceso.Datos.Data;
using Panaderia.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IAlmacenRepositorio Almacen {  get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Almacen = new AlmacenRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
