﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panaderia.AccesoDatos.Repositorio.IRepositorio;
using Panaderia.Modelos;
using Panaderia.Modelos.ViewModels;
using Panaderia.Utilidades;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Security.Claims;

namespace Panaderia.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private string _webUrl;

        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public CarroController(IUnidadTrabajo unidadTrabajo, IConfiguration configuration)
        {
            _unidadTrabajo = unidadTrabajo;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Orden = new Modelos.Orden();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                u => u.UsuarioAplicacionId == claim.Value,
                                                incluirPropiedades:"Producto");

            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.UsuarioAplicacionId = claim.Value;

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio; // Siempre mostrar el precio actual del producto
                carroCompraVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }

            return View(carroCompraVM);
        }

        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            carroCompras.Cantidad += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);

            if(carroCompras.Cantidad == 1)
            {
                //Removemos el registro del carro y se actualiza la sesión
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                            c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
                var numeroProductos = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remover(int carroId)
        {
            //Remueve el registro del carro y actualiza la sesion
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                            c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
            var numeroProductos = carroLista.Count();
            _unidadTrabajo.CarroCompra.Remover(carroCompras);
            await _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Proceder()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM()
            {
                Orden = new Modelos.Orden(),
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                            c => c.UsuarioAplicacionId == claim.Value, incluirPropiedades: "Producto"),
                Compania = await _unidadTrabajo.Compania.ObtenerPrimero()
            };

            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.UsuarioAplicacion = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(
                                                                                            u => u.Id == claim.Value);

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio;
                carroCompraVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }
            carroCompraVM.Orden.NombresCliente = carroCompraVM.Orden.UsuarioAplicacion.Nombres + " " +
                                                 carroCompraVM.Orden.UsuarioAplicacion.Apellidos;
            carroCompraVM.Orden.Telefono = carroCompraVM.Orden.UsuarioAplicacion.PhoneNumber;
            carroCompraVM.Orden.Direccion = carroCompraVM.Orden.UsuarioAplicacion.Direccion;
            carroCompraVM.Orden.Pais = carroCompraVM.Orden.UsuarioAplicacion.Pais;
            carroCompraVM.Orden.Ciudad = carroCompraVM.Orden.UsuarioAplicacion.Ciudad;

            //controlar stock
            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                //capturar stock de cada producto
                var producto = await _unidadTrabajo.AlmacenProducto.ObtenerPrimero(b => b.ProductoId == lista.ProductoId &&
                                                                      b.AlmacenId == carroCompraVM.Compania.AlmacenVentaId);
                if(lista.Cantidad > producto.Cantidad)
                {
                    TempData[DS.Error] = "La cantidad del producto " + lista.Producto.Descripcion +
                                         "Excede al stock actual (" + producto.Cantidad + ")";
                    return RedirectToAction("Index");
                }
            }
            return View(carroCompraVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proceder(CarroCompraVM carroCompraVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                c => c.UsuarioAplicacionId == claim.Value,
                                                incluirPropiedades: "Producto");
            carroCompraVM.Compania = await _unidadTrabajo.Compania.ObtenerPrimero();
            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.UsuarioAplicacionId = claim.Value;
            carroCompraVM.Orden.FechaOrden = DateTime.Now;

            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio;
                carroCompraVM.Orden.TotalOrden += (lista.Precio + lista.Cantidad);
            }
            //Controlar stock
            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                var producto = await _unidadTrabajo.AlmacenProducto.ObtenerPrimero(b => b.ProductoId == lista.ProductoId &&
                                                                      b.AlmacenId == carroCompraVM.Compania.AlmacenVentaId);
                if (lista.Cantidad > producto.Cantidad)
                {
                    TempData[DS.Error] = "La cantidad del producto " + lista.Producto.Descripcion +
                                         "Excede al stock actual (" + producto.Cantidad + ")";
                    return RedirectToAction("Index");
                }
            }
            carroCompraVM.Orden.EstadoOrden = DS.EstadoPendiente;
            carroCompraVM.Orden.EstadoPago = DS.PagoEstadoPendiente;
            await _unidadTrabajo.Orden.Agregar(carroCompraVM.Orden);
            await _unidadTrabajo.Guardar();
            //Guardar detalle de orden
            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    ProductoId = lista.ProductoId,
                    OrdenId = carroCompraVM.Orden.Id,
                    Precio = lista.Precio,
                    Cantidad = lista.Cantidad
                };
                await _unidadTrabajo.OrdenDetalle.Agregar(ordenDetalle);
                await _unidadTrabajo.Guardar();
            }
            //Stripe
            var usuario = await _unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == claim.Value);
            var options = new SessionCreateOptions
            {
                SuccessUrl = _webUrl + $"inventario/carro/OrdenConfirmacion?id={carroCompraVM.Orden.Id}",
                CancelUrl = _webUrl + "inventario/carro/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = usuario.Email
            };
            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(lista.Precio * 100), // S/.50 => 5000
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = lista.Producto.Descripcion
                        }
                    },
                    Quantity = lista.Cantidad
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unidadTrabajo.Orden.ActualizarPagoStripeId(carroCompraVM.Orden.Id, session.Id, session.PaymentIntentId);
            await _unidadTrabajo.Guardar();
            Response.Headers.Add("Location", session.Url); //redirecciona a stripe
            return new StatusCodeResult(303);
        
        }


        public async Task<IActionResult> OrdenConfirmacion(int id)
        {
            var orden = await _unidadTrabajo.Orden.ObtenerPrimero(o => o.Id == id, incluirPropiedades:"UsuarioAplicacion");
            var service = new SessionService();
            Session session = service.Get(orden.SessionId);
            var carroCompra = await _unidadTrabajo.CarroCompra.ObtenerTodos(U => U.UsuarioAplicacionId == orden.UsuarioAplicacionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unidadTrabajo.Orden.ActualizarPagoStripeId(id, session.Id, session.PaymentIntentId);
                _unidadTrabajo.Orden.ActualizarEstado(id, DS.EstadoAprobado, DS.PagoEstadoAprobado);
                await _unidadTrabajo.Guardar();

                //Disminuir stock del almacén de venta
                var compania = await _unidadTrabajo.Compania.ObtenerPrimero();
                foreach (var lista in carroCompra)
                {
                    var almacenProducto = new AlmacenProducto();
                    almacenProducto = await _unidadTrabajo.AlmacenProducto.ObtenerPrimero(b => b.ProductoId == lista.ProductoId
                                                                                && b.AlmacenId == compania.AlmacenVentaId);
                    await _unidadTrabajo.KardexInventario.RegistrarKardex(almacenProducto.Id, "Salida",
                                                                          "Venta - Orden# " + id,
                                                                          almacenProducto.Cantidad,
                                                                          lista.Cantidad,
                                                                          orden.UsuarioAplicacionId);
                    almacenProducto.Cantidad -= lista.Cantidad;
                    await _unidadTrabajo.Guardar();
                }

            }
            //Borrar el carro de compra y la sesion del carro de compra
            
            List<CarroCompra> carroCompraLista = carroCompra.ToList();
            _unidadTrabajo.CarroCompra.RemoverRango(carroCompraLista);
            await _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, 0);

            return View(id);
        }
    }
}
