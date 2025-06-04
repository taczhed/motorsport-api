using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace Web.Pages.Races
{
    public class DeleteModel : PageModel
    {
        private readonly MotorsportApi.Infrastructure.ApplicationDbContext _context;

        public DeleteModel(MotorsportApi.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Race Race { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FirstOrDefaultAsync(m => m.Id == id);
            var track = await _context.Tracks.FirstOrDefaultAsync(m => m.Id == race.TrackId);

            ViewData["Track"] = track;

            if (race is not null)
            {
                Race = race;

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

            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                Race = race;
                _context.Races.Remove(Race);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
