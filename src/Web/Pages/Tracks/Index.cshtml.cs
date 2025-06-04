using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.Web.Pages.Tracks
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
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
