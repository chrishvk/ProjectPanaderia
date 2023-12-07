using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Panaderia.Acceso.Datos.Data;
using Panaderia.Modelos;
using Panaderia.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.AccesoDatos.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInicializador(ApplicationDbContext db, UserManager<IdentityUser> userManager,
                                                                RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager; ;
            _roleManager = roleManager;
        }
        public void Inicializar()
        {
            try
            {   //Count() > 0 , cambiar luego de las pruebas
                if(_db.Database.GetPendingMigrations().Count() > 1)
                {
                    _db.Database.Migrate(); //Ejecutar migraciones pendientes
                }
            }
            catch (Exception)
            {

                throw;
            }

            //DAtos iniciales
            if (_db.Roles.Any(r => r.Name == DS.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Cliente)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Inventario)).GetAwaiter().GetResult();

            //Usuario Christian
            _userManager.CreateAsync(new UsuarioAplicacion
            {
                UserName = "71586554@continental.edu.pe",
                Email = "71586554@continental.edu.pe",
                EmailConfirmed = true,
                Nombres = "Christian",
                Apellidos = "Caso"
            }, "Admin123*").GetAwaiter().GetResult();

            //Usuario Keyla
            _userManager.CreateAsync(new UsuarioAplicacion
            {
                UserName = "73464874@continental.edu.pe",
                Email = "73464874@continental.edu.pe",
                EmailConfirmed = true,
                Nombres = "Keyla",
                Apellidos = "Villayzan"
            }, "Admin123*").GetAwaiter().GetResult();

            //Otorgando rol Admin
            UsuarioAplicacion usuario1 = _db.UsuarioAplicacion.Where(u => u.UserName == "71586554@continental.edu.pe").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario1, DS.Role_Admin).GetAwaiter().GetResult();

            UsuarioAplicacion usuario2 = _db.UsuarioAplicacion.Where(u => u.UserName == "73464874@continental.edu.pe").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario2, DS.Role_Admin).GetAwaiter().GetResult();


        }
    }
}
