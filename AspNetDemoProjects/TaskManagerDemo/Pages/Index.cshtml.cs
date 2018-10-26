using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace TaskManagerDemo.Pages
{
    public class IndexModel : PageModel
    {
        public string TaskManagerServiceUrl { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            TaskManagerServiceUrl = configuration["TaskManagerDemoServiceUrl"];
        }

        public void OnGet()
        {
        }
    }
}
