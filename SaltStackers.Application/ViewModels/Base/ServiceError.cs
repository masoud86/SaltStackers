namespace SaltStackers.Application.ViewModels.Base
{
    public class ServiceError
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public ErrorLevel Level { get; set; } = ErrorLevel.Blocker;
    }
}
