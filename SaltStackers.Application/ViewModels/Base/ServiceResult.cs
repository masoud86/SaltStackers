using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Base
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            Errors ??= new List<ServiceError>();
        }

        public ServiceResult(bool succeeded)
        {
            var errors = new List<ServiceError>();
            if (!succeeded)
            {
                errors.Add(new ServiceError
                {
                    Code = "UnknownError",
                    Description = Resources.Error.UnknownError
                });
            }
            Succeeded = succeeded;
            Errors = errors;
        }

        public ServiceResult(bool succeeded, List<ServiceError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public List<ServiceError> Errors { get; set; }
    }
}
