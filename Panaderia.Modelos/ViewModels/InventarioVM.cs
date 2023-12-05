using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos.ViewModels
{
    public class InventarioVM
    {
        public Inventario Inventario { get; set; }

        public InventarioDetalle InventarioDetalle { get; set; }

        public IEnumerable<InventarioDetalle> inventarioDetalles { get; set; }

        public IEnumerable<SelectListItem> AlmacenLista { get; set; }


    }
}
