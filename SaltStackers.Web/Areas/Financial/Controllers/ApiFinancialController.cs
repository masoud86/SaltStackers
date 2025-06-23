using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;

namespace SaltStackers.Web.Areas.Financial.Controllers
{
    [ApiController]
    public class ApiFinancialController : ControllerBase
    {
        private readonly IFinancialService _financialService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;


        public ApiFinancialController(IFinancialService financialService,
            IBackgroundJobClient backgroundJobs, IEmailService emailService,
            IConfiguration configuration)
        {
            _financialService = financialService;
            _backgroundJobs = backgroundJobs;
            _emailService = emailService;
            _configuration = configuration;
        }

        private static List<ServiceError> ModelStateToServiceError(ModelStateDictionary modelstate)
        {
            var errors = new List<ServiceError>();
            foreach (var error in modelstate.Values.SelectMany(v => v.Errors))
            {
                errors.Add(new ServiceError
                {
                    Level = ErrorLevel.Blocker,
                    Description = error.ErrorMessage
                });
            }
            return errors;
        }
    }
}
