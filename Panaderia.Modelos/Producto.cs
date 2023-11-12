using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Panaderia.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de serie es requerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(60)]
        public string Descripcion {  get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El costo es requerido")]
        public double Costo { get; set; }

        public string ImagegUrl { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set;}

        [Required(ErrorMessage = "La marca es requerida")]
        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        public int? PadreId { get; set; }

        public virtual Producto Padre { get; set; }
    }
}
