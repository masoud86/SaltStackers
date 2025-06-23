using System.Text.Json.Serialization;

namespace SaltStackers.Application.ViewModels.WebRequest
{
    public sealed class Api
    {
        private static Api _instance;
        private static readonly object Lock = new object();

        private Api() { }

        public List<ApiModel> Items { get; set; }

        public static Api Initial()
        {
            lock (Lock)
            {
                _instance ??= new Api
                {
                    Items = new List<ApiModel>
                    {
                        new ApiModel("Title", "Name", "Base_Url" + "/api/action", WebRequestType.Get, "{ }", null, false)
                    }
                };
            }

            return _instance;
        }

        public static ApiModel Get(string name)
        {
            Initial();
            return _instance.Items.FirstOrDefault(p => p.Name == name);
        }

        public static List<ApiModel> GetAll()
        {
            Initial();
            return _instance.Items;
        }
    }

    public class ApiModel
    {
        public ApiModel(string title, string name, string url, WebRequestType type, string requestModel, Type requestModelType, bool isPerOrganization)
        {
            Title = title;
            Name = name;
            Url = url;
            Type = type;
            RequestModel = requestModel;
            RequestModelType = requestModelType;
            IsPerOrganization = isPerOrganization;
        }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        
        public WebRequestType Type { get; set; }

        public string RequestModel { get; set; }

        [JsonIgnore]
        public Type RequestModelType { get; set; }

        public bool IsPerOrganization { get; set; }
    }
}
