using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagerDemo.Data;

namespace TaskManagerDemo.Pages.TaskManager
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        public DetailsModel(TaskManagerDemo.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public TaskInfo TaskInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaskInfo = await _context.Tasks
                .Include(t => t.ParentTask)
                .Include(t => t.SubTasks).ThenInclude(t => t.ModifyInfos)
                .Include(t => t.ModifyInfos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (TaskInfo == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
