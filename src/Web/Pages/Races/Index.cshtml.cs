using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Pages.Races
{
    [Authorize(Roles = "Admin,RaceManager")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Race> Race { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Race = await _context.Races
                .Include(r => r.Track).ToListAsync();
        }
    }
}
