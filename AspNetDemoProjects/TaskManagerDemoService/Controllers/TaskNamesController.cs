using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerDemo.Data;

namespace TaskManagerDemoService.Controllers
{
    /// <summary>
    /// Task names service controller.
    /// </summary>
    [Route("rest/v1/[controller]")]
    [ApiController]
    public class TaskNamesController : ControllerBase
    {
        private readonly TaskManagerDemo.Data.ApplicationDbContext _context;

        public TaskNamesController(TaskManagerDemo.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves task names.
        /// </summary>
        /// <param name="filter">Task name search term.</param>
        /// <param name="contains">Task name contains, otherwise starts with <paramref name="filter"/></param>
        /// <param name="pgn">Page number.</param>
        /// <param name="pgsz">Page size.</param>
        /// <returns>Names collection in alphabetical order.</returns>
        // GET rest/names
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(int? pgn = 1, int? pgsz = DataExtensions.PageSizeDefault, string filter = null, bool? contains = false)
        {
            filter = filter?.Trim();
            IQueryable<TaskInfo> result = _context.Tasks
                .ApplyNameFilter(filter, contains.Value)
                .OrderBy(o => o.Name)
                .ApplyPaging(pgn.Value, pgsz.Value);

            return result.Select(o => o.Name).ToList();
        }

        /// <summary>
        /// Counts names.
        /// </summary>
        /// <returns>Existing task names count.</returns>
        // GET rest/names/count
        [HttpGet("Count")]
        public ActionResult<int> Count()
        {
            return _context.Tasks.Count();
        }

        /// <summary>
        /// Validates name.
        /// </summary>
        /// <param name="name">Task name to be validated.</param>
        /// <returns>True if name is valid.</returns>
        // GET rest/names/validate
        [HttpGet("Validate")]
        public ActionResult<bool> Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(false);
            try
            {
                var val = _context.Tasks.FirstOrDefault(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return val == null;
            }
            catch
            {
                return BadRequest(false);
            }
        }

        /// <summary>
        /// Adds name.
        /// </summary>
        /// <param name="name">Stores task name.</param>
        /// <returns><see cref="ActionResult"/></returns>
        // POST rest/names
        [HttpPost]
        public ActionResult Post([FromForm] string name)
        {
            return base.BadRequest("Not implemented.");
        }
    }
}
