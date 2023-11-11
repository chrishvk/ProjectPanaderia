using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(60, ErrorMessage = "El nombre debe tener como máximo 60 carateres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(60, ErrorMessage = "La descripción debe tener como máximo 60 carateres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }
    }
}
