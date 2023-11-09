using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(60, ErrorMessage = "El nombre debe ser  de máximo 60 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es requerido")]
        [MaxLength(100, ErrorMessage = "La descripcion debe ser  de máximo 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }
    }
}
