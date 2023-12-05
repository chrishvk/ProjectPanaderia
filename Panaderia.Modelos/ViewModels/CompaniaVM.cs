using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos.ViewModels
{
    public class CompaniaVM
    {
        public Compania Compania { get; set; }

        public IEnumerable<SelectListItem> AlmacenLista { get; set; }


    }
}
