using WebApplication7.Models;
using WebApplication7.Repository;

namespace WebApplication7.Services
{
    public class CustomFieldsService
    {
        public const string TEAM_BOARD_CF_KEY = "TEAM_BOARD";
        public const string STORY_POINTS_CF_KEY = "STORY_POINTS";
        public const string CUSTOM_FIELD_KEY_FORMAT = "customfield_{0}";

        private CustomFieldRepository customFieldRepository;

        public CustomFieldsService(CustomFieldRepository customFieldRepository)
        {
            this.customFieldRepository = customFieldRepository; 
        }

        public async Task<CustomField> AddNewCustomField(CustomField customFields)
        {
            return await customFieldRepository.AddCustomField(customFields);
        }

        public async Task<string> GetCustomFieldValueAgainstKey(string customFieldKey)
        {
            CustomField? customFields = await customFieldRepository.GetCustomFieldByKey(customFieldKey);   
            if(customFields == null)
            {
                return "";
            }
            return String.Format(CUSTOM_FIELD_KEY_FORMAT, customFields.CustomFieldValue);
        }

        public async Task<List<CustomField>> GetAllCustomFieldsAsync()
        {
            return await customFieldRepository.GetAllCustomFields();
        }
    }
}
