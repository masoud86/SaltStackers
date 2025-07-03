namespace SaltStackers.Application.ViewModels.Base;

public class ServiceResult
{
    public ServiceResult()
    {
        Errors ??= [];
    }

    public ServiceResult(bool succeeded, Dictionary<string, string>? info = null)
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
        Info = info;
    }

    public ServiceResult(bool succeeded, List<ServiceError> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; set; }

    public List<ServiceError> Errors { get; set; }

    public Dictionary<string, string>? Info { get; set; }
}
