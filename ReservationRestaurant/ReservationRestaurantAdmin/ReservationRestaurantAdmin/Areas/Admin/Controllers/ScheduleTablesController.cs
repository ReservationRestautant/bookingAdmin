using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationRestaurantAdmin.Models;

namespace ReservationRestaurantAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleTablesController : Controller
    {
        private readonly BookingRestaurantContext _context;
        public INotyfService _notifyService { get; }
        public ScheduleTablesController(BookingRestaurantContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/ScheduleTables
        public async Task<IActionResult> Index()
        {
            var bookingRestaurantContext = _context.ScheduleTables.Include(s => s.Table);
            return View(await bookingRestaurantContext.ToListAsync());
        }

        // GET: Admin/ScheduleTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleTable = await _context.ScheduleTables
                .Include(s => s.Table)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleTable == null)
            {
                return NotFound();
            }

            return View(scheduleTable);
        }

        // GET: Admin/ScheduleTables/Create
        public IActionResult Create()
        {
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Name");
            return View();
        }

        // POST: Admin/ScheduleTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,StartTime,EndTime,TableId")] ScheduleTable scheduleTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleTable);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Name", scheduleTable.TableId);
            return View(scheduleTable);
        }

        // GET: Admin/ScheduleTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleTable = await _context.ScheduleTables.FindAsync(id);
            if (scheduleTable == null)
            {
                return NotFound();
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Name", scheduleTable.TableId);
            return View(scheduleTable);
        }

        // POST: Admin/ScheduleTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StartTime,EndTime,TableId")] ScheduleTable scheduleTable)
        {
            if (id != scheduleTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleTable);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleTableExists(scheduleTable.Id))
                    {

                        _notifyService.Success("Có lỗi xãy ra");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Name", scheduleTable.TableId);
            return View(scheduleTable);
        }

        // GET: Admin/ScheduleTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleTable = await _context.ScheduleTables
                .Include(s => s.Table)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleTable == null)
            {
                return NotFound();
            }

            return View(scheduleTable);
        }

        // POST: Admin/ScheduleTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scheduleTable = await _context.ScheduleTables.FindAsync(id);
            _context.ScheduleTables.Remove(scheduleTable);
            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleTableExists(int id)
        {
            return _context.ScheduleTables.Any(e => e.Id == id);
        }
    }
}
