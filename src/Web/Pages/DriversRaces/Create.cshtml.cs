using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Pages.RacesDrivers
{
    [Authorize(Roles = "Admin,RaceManager")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
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
