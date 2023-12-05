using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia.AccesoDatos.Repositorio.IRepositorio;
using Panaderia.Modelos;
using Panaderia.Utilidades;

namespace Panaderia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class AlmacenController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public AlmacenController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Upsert(int? id)
        {
            Almacen almacen = new Almacen();

            if (id == null)
            {
                //crear un nuevo almacen
                almacen.Estado = true;
                return View(almacen);
            }
            //Actualizar almacen
            almacen = await _unidadTrabajo.Almacen.Obtener(id.GetValueOrDefault());
            if(almacen == null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Almacen almacen)
        {
            if(ModelState.IsValid)
            {
                if(almacen.Id == 0)
                {
                    await _unidadTrabajo.Almacen.Agregar(almacen);
                    TempData[DS.Exitosa] = "Almacén creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.Almacen.Actualizar(almacen);
                    TempData[DS.Exitosa] = "Almacén actualizado existosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar almacén";
            return View(almacen);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Almacen.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var almacenDb = await _unidadTrabajo.Almacen.Obtener(id);
            if (almacenDb == null)
            {
                return Json(new { success = false, message = "Error al borrar alamcén" });
            }
            _unidadTrabajo.Almacen.Remover(almacenDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Almacén eliminado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Almacen.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if(valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
