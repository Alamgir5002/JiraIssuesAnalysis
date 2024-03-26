using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Repository
{
    public class CustomFieldRepository
    {
        private DatabaseContext databaseContext;
        public CustomFieldRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext; 
        }

        public async Task<CustomField> AddCustomField(CustomField customFields)
        {
            await databaseContext.CustomFields.AddAsync(customFields);
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
