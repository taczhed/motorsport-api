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
    public class DetailsModel : PageModel
    {
        private readonly MotorsportApi.Infrastructure.ApplicationDbContext _context;

        public DetailsModel(MotorsportApi.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
