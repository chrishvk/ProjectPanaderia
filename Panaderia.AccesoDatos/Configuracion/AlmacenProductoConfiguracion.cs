using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panaderia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Configuracion
{
    public class AlmacenProductoConfiguracion : IEntityTypeConfiguration<AlmacenProducto>
    {
        public void Configure(EntityTypeBuilder<AlmacenProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.AlmacenId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();

            /* Relaciones */

            builder.HasOne(x => x.Almacen).WithMany()
                .HasForeignKey(x => x.AlmacenId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
