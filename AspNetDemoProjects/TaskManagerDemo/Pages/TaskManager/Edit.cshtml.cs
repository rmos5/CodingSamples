using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagerDemo.Data;
using TaskManagerDemo.Services;

namespace TaskManagerDemo.Pages.TaskManager
{
    public class EditModel : PageModel
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        private readonly TaskManagerDemo.Services.ITaskValidation _validation;

        public EditModel(TaskManagerDemo.Data.ApplicationDbContext context, ITaskValidation validation)
        {
            _context = context;
            _validation = validation;
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
                .Include(t => t.ParentTask).Include(o=>o.ModifyInfos).FirstOrDefaultAsync(m => m.Id == id);

            if (TaskInfo == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var existingTaskInfo = await _context.Tasks.Include(o => o.ParentTask).Include(o=>o.ModifyInfos).FirstOrDefaultAsync(o => o.Id == TaskInfo.Id);

                if (existingTaskInfo == null)
                {
                    return NotFound();
                }

                string t1 = BitConverter.ToString(TaskInfo.Timestamp);
                string t2 = BitConverter.ToString(existingTaskInfo.Timestamp);

                if (!string.Equals(t1, t2, StringComparison.Ordinal))
                {
                    TaskInfo = existingTaskInfo;
                    throw new Exception("Task has been updated after you started editing.");
                }

                string name = existingTaskInfo.Name;
                bool isNameValid = true;
                bool updated = await TryUpdateModelAsync(existingTaskInfo, "TaskInfo");

                if (ModelState.IsValid)
                {
                    if (existingTaskInfo.HasChanges(_context))
                    {
                        // model is updated, now validate
                        if (name != existingTaskInfo.Name)
                        {
                            isNameValid = await _validation.ValidateNameAsync(TaskInfo.Name);
                        }

                        if (isNameValid)
                        {
                            existingTaskInfo.UpdateModified(_context, User?.Identity?.Name);
                            _context.Update(existingTaskInfo);
                            await _context.SaveChangesAsync();
                            return RedirectToPage("./Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Task name validation failed.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No changes, nothing to save.");
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Page();
        }
    }
}
