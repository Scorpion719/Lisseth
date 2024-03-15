using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lisseth.Models;

namespace Lisseth.Controllers
{
    public class ClientesController : Controller
    {
        private readonly LissethContext _context;

        public ClientesController(LissethContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
              return _context.Cliente != null ? 
                          View(await _context.Cliente.ToListAsync()) :
                          Problem("Entity set 'LissethContext.Cliente'  is null.");
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(s => s.Telefono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Details";
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            var cliente = new Cliente();
            cliente.Telefono = new List<Telefono>();
            cliente.Telefono.Add(new Telefono
            {
                
            });
            ViewBag.Accion = "Create";
            return View(cliente);
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Fecha,Correo,Telefono")] Cliente cliente)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult AgregarDetalles([Bind("Id,Nombre,Direccion,Fecha,Correo,Telefono")] Cliente cliente, string accion)
        {
            cliente.Telefono.Add(new Telefono { });
            ViewBag.Accion = accion;
            return View(accion, cliente);
        }
        public ActionResult EliminarDetalles([Bind("Id,Nombre,Direccion,Fecha,Correo,Telefono")] Cliente cliente, int index, string accion)
        {
            var det = cliente.Telefono[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                cliente.Telefono.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(s => s.Telefono)
                .FirstAsync(s => s.Id==id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,Fecha,Correo,Telefono")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            try
            {

                var facturaUpdate = await _context.Cliente
                      .Include(s => s.Telefono)
                      .FirstAsync(s => s.Id == cliente.Id);
                facturaUpdate.Nombre = cliente.Nombre;
                facturaUpdate.Direccion = cliente.Direccion;
                facturaUpdate.Fecha = cliente.Fecha;
                facturaUpdate.Correo = cliente.Correo;
                //facturaUpdate.Estado = proyecto.Estado;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = cliente.Telefono.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    facturaUpdate.Telefono.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = cliente.Telefono.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = facturaUpdate.Telefono.FirstOrDefault(s => s.Id == d.Id);
                    det.NumeroTelefono = d.NumeroTelefono;
                    det.Compania = d.Compania;
                    det.Tipo = d.Tipo;
                    
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDet = cliente.Telefono.Where(s => s.Id < 0).ToList();
                if (delDet != null && delDet.Count > 0)
                {
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.Telefono.FirstOrDefault(s => s.Id == d.Id);
                        _context.Remove(det);
                        // facturaUpdate.DetFacturaVenta.Remove(det);
                    }
                }
                // Aplicar esos cambios a la base de datos


                _context.Update(facturaUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .Include(s=> s.Telefono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cliente == null)
            {
                return Problem("Entity set 'LissethContext.Cliente'  is null.");
            }
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Cliente?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
