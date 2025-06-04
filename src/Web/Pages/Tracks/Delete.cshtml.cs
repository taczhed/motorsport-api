using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace Web.Pages.Tracks
{
    public class DeleteModel : PageModel
    {
        private readonly MotorsportApi.Infrastructure.ApplicationDbContext _context;

        public DeleteModel(MotorsportApi.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Track Track { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks.FirstOrDefaultAsync(m => m.Id == id);

            if (track is not null)
            {
                Track = track;

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

            var track = await _context.Tracks.FindAsync(id);
            if (track != null)
            {
                Track = track;
                _context.Tracks.Remove(Track);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
