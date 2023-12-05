using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.Modelos
{
    public class KardexInventario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AlmacenProductoId { get; set; }

        [ForeignKey("AlmacenProductoId")]
        public AlmacenProducto AlmacenProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; } //Entrada o salida

        [Required]
        public string Detalle { get; set; }

        [Required]
        public int StockAnterior { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public double Costo { get; set; }

        [Required]
        public int Stock { get; set; }

        public double Total { get; set; }

        [Required]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
