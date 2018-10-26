using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TaskManagerDemoService.Client;

namespace TaskManagerDemo.Services
{
    internal class TaskNameUniqueValidationService : ITaskValidation
    {
        private readonly string _serviceUrl;

        private readonly ILogger<ITaskValidation> _logger;

        public TaskNameUniqueValidationService(IConfiguration configuration, ILogger<ITaskValidation> logger)
        {
            _serviceUrl = configuration["TaskManagerDemoServiceUrl"];
            _logger = logger;
        }
        
        public async Task<bool> ValidateNameAsync(string taskName)
        {
            bool result = false;
            Client client = new Client(_serviceUrl);
            try
            {
                result = await client.RestV1TaskNamesValidateGetAsync(taskName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Task name validation failed. {ex.Message}");
            }

            return result;
        }
    }
}
