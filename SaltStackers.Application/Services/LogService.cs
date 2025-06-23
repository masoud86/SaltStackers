using AutoMapper;
using LinqKit;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Log;
using System.Dynamic;

namespace SaltStackers.Application.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _iMapper;

        public LogService(ILogRepository logRepository)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _iMapper = config.CreateMapper();
            _logRepository = logRepository;
        }

        public async Task AddUserLogAsync(UserActivityLogDto model)
        {
            await _logRepository.AddUserActivityLogAsync(_iMapper.Map<UserActivityLog>(model));
        }

        public async Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, ClientInformation client, string receiptNumber = null, string requestNumber = null)
        {
            var model = _iMapper.Map<UserActivityLogDto>(client);
            model.UserId = userId;
            model.DescriptionResourceKey = descriptionKey;
            model.DescriptionParameters = descriptionParameters;
            model.Type = type;
            model.ReceiptNumber = receiptNumber;
            model.RequestNumber = requestNumber;
            await AddUserLogAsync(model);
        }

        public async Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, string actionRelatedId, ClientInformation client, string receiptNumber = null, string requestNumber = null)
        {
            var model = _iMapper.Map<UserActivityLogDto>(client);
            model.UserId = userId;
            model.DescriptionResourceKey = descriptionKey;
            model.DescriptionParameters = descriptionParameters;
            model.Type = type;
            model.ActionRelatedId = actionRelatedId;
            model.ReceiptNumber = receiptNumber;
            model.RequestNumber = requestNumber;
            await AddUserLogAsync(model);
        }

        private static dynamic RemoveExtraInformation(object content)
        {
            if (content is ExpandoObject)
                return ((IDictionary<string, object>)content).ContainsKey("LogInfo");

            var editedContent = content as dynamic;

            if (content.GetType().GetProperty("LogInfo") != null)
            {
                editedContent.LogInfo = null;
            }

            return editedContent;
        }

        public async Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, object content, ClientInformation client, string receiptNumber = null, string requestNumber = null)
        {
            var model = _iMapper.Map<UserActivityLogDto>(client);
            model.UserId = userId;
            model.DescriptionResourceKey = descriptionKey;
            model.DescriptionParameters = descriptionParameters;
            model.Type = type;
            model.Content = RemoveExtraInformation(content);
            model.ReceiptNumber = receiptNumber;
            model.RequestNumber = requestNumber;
            await AddUserLogAsync(model);
        }

        public async Task AddUserLogAsync(string userId, string type, string descriptionKey, string descriptionParameters, string actionRelatedId, object content,
            ClientInformation client, string receiptNumber = null, string requestNumber = null)
        {
            var model = _iMapper.Map<UserActivityLogDto>(client);
            model.UserId = userId;
            model.DescriptionResourceKey = descriptionKey;
            model.DescriptionParameters = descriptionParameters;
            model.Type = type;
            model.ActionRelatedId = actionRelatedId;
            model.Content = RemoveExtraInformation(content);
            model.ReceiptNumber = receiptNumber;
            model.RequestNumber = requestNumber;
            await AddUserLogAsync(model);
        }

        private ExpressionStarter<UserActivityLog> FilterToExpression(UserActivityLogFilters filter)
        {
            var predicate = PredicateBuilder.New<UserActivityLog>(_ => true);

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                predicate.And(p => p.UserId == filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    p.IpAddress.ToLower().Contains(filter.Query));
            }

            return predicate;
        }

        public async Task<List<UserActivityLogDto>> GetUserActivityLogAsync(UserActivityLogFilters filter)
        {
            var predicate = FilterToExpression(filter);

            var model = await _logRepository.GetUserActivitiesLogAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<UserActivityLog>, List<UserActivityLogDto>>(model);
        }

        public async Task<UserActivityLogs> GetUserActivityLogModelAsync(UserActivityLogFilters filter)
        {
            var predicate = FilterToExpression(filter);

            var recordTotal = await _logRepository.GetUserActivityLogsCount();

            var recordsFilters = await _logRepository.GetUserActivityLogsCount(predicate);

            return new UserActivityLogs
            {
                Items = await GetUserActivityLogAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<UserActivityLogDto> GetUserActivityLogById(Guid id)
        {
            var log = await _logRepository.GetUserActivityLogByIdAsync(id);
            return _iMapper.Map<UserActivityLogDto>(log);
        }

        private ExpressionStarter<ApplicationLog> FilterToExpression(ApplicationLogFilters filter)
        {
            var predicate = PredicateBuilder.New<ApplicationLog>(_ => true);

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                predicate.And(p => p.UserId == filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.RequestNumber))
            {
                predicate.And(p => p.RequestNumber == filter.RequestNumber);
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    p.Message.ToLower().Contains(filter.Query) ||
                    p.ReceiptNumber.ToLower().Contains(filter.Query) ||
                    p.RequestNumber.ToLower().Contains(filter.Query) ||
                    p.Logger.ToLower().Contains(filter.Query));
            }

            return predicate;
        }

        public async Task<List<ApplicationLogDto>> GetApplicationLogAsync(ApplicationLogFilters filter)
        {
            var predicate = FilterToExpression(filter);

            var model = await _logRepository.GetApplicationLogsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<ApplicationLog>, List<ApplicationLogDto>>(model);
        }

        public async Task<ApplicationLogs> GetApplicationLogModelAsync(ApplicationLogFilters filter)
        {
            var predicate = FilterToExpression(filter);

            var recordTotal = await _logRepository.GetApplicationLogsCountAsync(
                FilterToExpression(new ApplicationLogFilters
                {
                    UserId = filter.UserId,
                    RequestNumber = filter.RequestNumber
                }
            ));

            var recordsFilters = await _logRepository.GetApplicationLogsCountAsync(predicate);

            return new ApplicationLogs
            {
                Items = await GetApplicationLogAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
    }
}
