using IssueAnalysisExtended.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class CustomFieldRepository: ICustomFieldRepository
    {
        private DatabaseContext databaseContext;
        public CustomFieldRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext; 
        }

        public async Task<CustomField> AddOrUpdateCustomFields(CustomField customFields)
        {
            var customField = await GetCustomFieldByKey(customFields.CustomFieldKey);
            if(customField == null)
            {
                await databaseContext.CustomFields.AddAsync(customFields);
            }
            else
            {
                customField.CustomFieldValue = customFields.CustomFieldValue;
                databaseContext.Entry(customField).State = EntityState.Modified;
            }

            await databaseContext.SaveChangesAsync();
            return customFields;
        }

        public async Task<CustomField?> GetCustomFieldByKey(string customFieldKey)
        {
            return await databaseContext.CustomFields.FirstOrDefaultAsync(cf=> cf.CustomFieldKey.Equals(customFieldKey));
        }

        public async Task<List<CustomField>> GetAllCustomFields()
        {
            return await databaseContext.CustomFields.ToListAsync();
        }
    }
}
