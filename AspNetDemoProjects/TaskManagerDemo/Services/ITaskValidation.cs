using System.Threading.Tasks;

namespace TaskManagerDemo.Services
{
    public interface ITaskValidation
    {
        Task<bool> ValidateNameAsync(string taskName);

        //bool ValidateName(string taskName);
    }
}
