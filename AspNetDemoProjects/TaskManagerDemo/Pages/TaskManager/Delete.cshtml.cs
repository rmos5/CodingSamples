using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagerDemo.Data;

namespace TaskManagerDemo.Pages.TaskManager
{
    public class DeleteModel : PageModel
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        public DeleteModel(TaskManagerDemo.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskInfo TaskInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaskInfo = await _context.Tasks
                .Include(t => t.ParentTask).FirstOrDefaultAsync(m => m.Id == id);

            if (TaskInfo == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaskInfo = await _context.Tasks
               .Include(t => t.ParentTask).FirstOrDefaultAsync(m => m.Id == id);

            if (TaskInfo != null)
            {
                _context.Tasks.Remove(TaskInfo);
                TaskInfo.ParentTask?.UpdateModified(_context, User?.Identity?.Name);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
