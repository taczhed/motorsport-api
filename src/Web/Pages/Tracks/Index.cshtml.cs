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
    public class IndexModel : PageModel
    {
        private readonly MotorsportApi.Infrastructure.ApplicationDbContext _context;

        public IndexModel(MotorsportApi.Infrastructure.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Track> Track { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Track = await _context.Tracks.ToListAsync();
        }
    }
}
