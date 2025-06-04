using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace Web.Pages.RacesDrivers
{
    public class CreateModel : PageModel
    {
        private readonly MotorsportApi.Infrastructure.ApplicationDbContext _context;

        public CreateModel(MotorsportApi.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? raceId)
        {
            if (raceId == null) return NotFound();

            var races = _context.Races.Where(r => r.Id == raceId).Select(d => new { d.Id, d.Name }).ToList();

            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Name");
            ViewData["RaceId"] = new SelectList(races, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public DriverRace DriverRace { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DriverRaces.Add(DriverRace);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Races/Edit", new { id = DriverRace.RaceId });
        }
    }
}
