using Newtonsoft.Json.Linq;
using WebApplication7.Models;
using WebApplication7.Repository;

namespace WebApplication7.Services
{
    public class CustomFieldsService
    {
        public const string TEAM_BOARD_CF_KEY = "TEAM_BOARD";
        public const string STORY_POINTS_CF_KEY = "STORY_POINTS";
        public const string CUSTOM_FIELD_KEY_FORMAT = "customfield_{0}";
        private const string CUSTOM_FIELD_ENDPOINT = "/rest/api/3/field";
        private HttpClientService httpClientService;
        private IssueMapperService issueMapperService;

        private CustomFieldRepository customFieldRepository;

        public CustomFieldsService(CustomFieldRepository customFieldRepository, 
            HttpClientService httpClientService,
            IssueMapperService issueMapperService)
        {
            this.customFieldRepository = customFieldRepository;
            this.httpClientService = httpClientService;
            this.issueMapperService = issueMapperService;
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

        public async Task<List<CustomField>> GetAllCustomFieldsFromSource(SourceCredentials sourceCredentials)
        {
            Uri url = new Uri(new Uri(sourceCredentials.SourceURL), CUSTOM_FIELD_ENDPOINT);
            HttpResponseMessage httpResponse = await httpClientService.SendGetRequestWithBasicAuthHeaders(url.ToString(),
                                                        sourceCredentials);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Error code: {httpResponse.StatusCode}, Content: {await httpResponse.Content.ReadAsStringAsync()}");
            }

            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            JArray jsonArray = JArray.Parse(responseBody);


            return issueMapperService.ConvertResponseToCustomFields(jsonArray);
        }
    }
}
