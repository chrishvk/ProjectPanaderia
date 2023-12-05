﻿using Panaderia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        void Actualizar(Orden orden);

    }
}
