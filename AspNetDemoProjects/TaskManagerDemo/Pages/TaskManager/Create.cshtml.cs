using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TaskManagerDemo.Data;
using TaskManagerDemo.Services;

namespace TaskManagerDemo.Pages.TaskManager
{
    public class CreateModel : PageModel
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        private readonly TaskManagerDemo.Services.ITaskValidation _validation;

        public CreateModel(TaskManagerDemo.Data.ApplicationDbContext context, ITaskValidation validation)
        {
            _context = context;
            _validation = validation;
        }

        [BindProperty]
        public TaskInfo TaskInfo { get; set; }

        public IActionResult OnGet(Guid? parentTaskId)
        {
            TaskInfo parent = null;
            string newTaskName = "Task";
            if (parentTaskId != null)
            {
                parent = _context.Find<TaskInfo>(parentTaskId);
                newTaskName = parent?.Name + "-";
            }

            TaskInfo = new TaskInfo { Name = newTaskName, ParentTaskId = parentTaskId, ParentTask = parent };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _validation.ValidateNameAsync(TaskInfo.Name))
            {
                ModelState.AddModelError("", "Task name validation failed.");
                return Page();
            }

            TaskInfo.UpdateModified(_context, User?.Identity?.Name);

            if (TaskInfo.ParentTaskId != null)
            {
                TaskInfo.ParentTask = await _context.Tasks.FindAsync(TaskInfo.ParentTaskId);
                TaskInfo.ParentTask.UpdateModified(_context, User?.Identity?.Name);
            }

            _context.Tasks.Add(TaskInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}