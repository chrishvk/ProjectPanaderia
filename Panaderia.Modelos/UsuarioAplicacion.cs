using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        [Required(ErrorMessage ="El nombre es requerido")]
        [MaxLength(80)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La direccion es requerida")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida")]
        [MaxLength(60)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El país es requerido")]
        [MaxLength(60)]
        public string Pais { get; set; }

        [NotMapped] //No se agrega a la tabla
        public string Role { get; set; }


    }
}
