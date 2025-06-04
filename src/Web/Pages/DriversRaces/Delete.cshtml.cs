using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Pages.RacesDrivers
{
    [Authorize(Roles = "Admin,RaceManager")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DriverRace DriverRace { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverrace = await _context.DriverRaces.FirstOrDefaultAsync(m => m.DriverId == id);

            if (driverrace is not null)
            {
                DriverRace = driverrace;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverrace = await _context.DriverRaces.FindAsync(id);
            if (driverrace != null)
            {
                DriverRace = driverrace;
                _context.DriverRaces.Remove(DriverRace);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
