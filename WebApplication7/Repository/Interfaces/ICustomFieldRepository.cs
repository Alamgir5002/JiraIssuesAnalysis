using WebApplication7.Models;

namespace IssueAnalysisExtended.Repository.Interfaces
{
    public interface ICustomFieldRepository
    {
        public Task<CustomField> AddOrUpdateCustomFields(CustomField customFields);

        public Task<CustomField?> GetCustomFieldByKey(string customFieldKey);

        public Task<List<CustomField>> GetAllCustomFields();
    }
}
