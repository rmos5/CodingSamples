using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerDemo.Data;

namespace TaskManagerDemo.Pages.TasksManager
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Contains { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; } = 12;

        [BindProperty(SupportsGet = true)]
        public int? PageNum { get; set; } = 1;

        public IEnumerable<TaskInfo> TaskInfos { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            IQueryable<TaskInfo> result = _context.Tasks
                .Include(t => t.ParentTask)
                .Include(t => t.SubTasks)
                .Include(t => t.ModifyInfos)
                .ApplyNameFilter(Filter, Contains)
                .OrderByDescending(t => t.Timestamp)
                .ApplyPaging(PageNum.Value, PageSize.Value);

            TaskInfos = await result.ToListAsync();
        }
    }
}
