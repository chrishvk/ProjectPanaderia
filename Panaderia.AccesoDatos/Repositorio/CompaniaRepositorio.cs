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
    public class CompaniaRepositorio : Repositorio<Compania>, ICompaniaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CompaniaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Compania compania)
        {
            var companiaBD = _db.Companias.FirstOrDefault(b => b.Id == compania.Id);
            if (companiaBD != null)
            {
                companiaBD.Nombre = compania.Nombre;
                companiaBD.Descripcion = compania.Descripcion;
                companiaBD.Pais = compania.Pais;
                companiaBD.Cuidad = compania.Cuidad;
                companiaBD.Direccion = compania.Direccion;
                companiaBD.Telefono = compania.Telefono;
                companiaBD.AlmacenVentaId = compania.AlmacenVentaId;
                companiaBD.ActualizadoPorId = compania.ActualizadoPorId;
                companiaBD.FechaActualizacion = compania.FechaActualizacion;
                _db.SaveChanges();
            }
        }
    }
}
