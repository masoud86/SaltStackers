using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltStackers.Application.ViewModels.WebRequest
{
    public class WebServiceResult
    {
        public WebServiceResult()
        {
            Errors = new List<Error>();
        }

        public bool Succeeded { get; set; }

        public List<Error> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Error
    {
        public List<ErrorMessage> ErrorMessages { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }

        public string Culture { get; set; }
    }
}
