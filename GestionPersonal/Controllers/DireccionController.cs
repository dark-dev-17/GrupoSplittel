using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionPersonal.Controllers
{
    public class DireccionController : Controller
    {
        //private readonly DireccionContext _context;

        //public DireccionController(DireccionContext context)
        //{
        //    _context = context;
        //}

        //// GET: Direccion
        //public async Task<IActionResult> Index()
        //{
        //    var direccionContext = _context.Direccion.Include(d => d.Sociedad).Include(d => d.DireccionPa);
        //    return View(await direccionContext.ToListAsync());
        //}

        //// GET: Direccion/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var direccion = await _context.Direccion
        //        .Include(d => d.Sociedad)
        //        .FirstOrDefaultAsync(m => m.IdDireccion == id);
        //    if (direccion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(direccion);
        //}

        //// GET: Direccion/Create
        //public IActionResult Create()
        //{
        //    ViewData["IdSociedad"] = new SelectList(_context.Set<Sociedad>(), "IdSociedad", "Descripcion");
        //    ViewData["Direcciones"] = new SelectList(_context.Set<Direccion>(), "IdDireccion", "Nombre");
        //    return View();
        //}

        //// POST: Direccion/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdDireccion,Nombre,IdSociedad")] Direccion direccion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(direccion);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdSociedad"] = new SelectList(_context.Set<Sociedad>(), "IdSociedad", "Descripcion", direccion.IdSociedad);
        //    return View(direccion);
        //}

        //// GET: Direccion/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var direccion = await _context.Direccion.FindAsync(id);
        //    if (direccion == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdSociedad"] = new SelectList(_context.Set<Sociedad>(), "IdSociedad", "Descripcion", direccion.IdSociedad);
        //    ViewData["Direcciones"] = new SelectList(_context.Set<Direccion>(), "IdDireccion", "Nombre");
        //    return View(direccion);
        //}

        //// POST: Direccion/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdDireccion,Nombre,IdSociedad")] Direccion direccion)
        //{
        //    if (id != direccion.IdDireccion)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(direccion);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DireccionExists(direccion.IdDireccion))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdSociedad"] = new SelectList(_context.Set<Sociedad>(), "IdSociedad", "Descripcion", direccion.IdSociedad);
        //    ViewData["Direcciones"] = new SelectList(_context.Set<Direccion>(), "IdDireccion", "Nombre");
        //    return View(direccion);
        //}

        //// GET: Direccion/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var direccion = await _context.Direccion
        //        .Include(d => d.Sociedad)
        //        .FirstOrDefaultAsync(m => m.IdDireccion == id);
        //    if (direccion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(direccion);
        //}

        //// POST: Direccion/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var direccion = await _context.Direccion.FindAsync(id);
        //    _context.Direccion.Remove(direccion);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DireccionExists(int id)
        //{
        //    return _context.Direccion.Any(e => e.IdDireccion == id);
        //}
    }
}
