using WebApplication7.Models;

namespace IssueAnalysisExtended.Models
{
    public class SourceFieldsResponse
    {
        public List<CustomField>? SourceCustomFields { get; set; }
        public List<CustomField>? UserCustomFields { get; set; }
        public List<Project>? SourceProjects { get; set; }
        public Project? UserProject { get; set;}

        public static SourceFieldsResponse processSourceResponse(List<CustomField> sourceCustomFields, 
            List<CustomField> userCustomFields, 
            List<Project> sourceProjects,
            Project userProject) {

            SourceFieldsResponse sourceFieldsResponse = new SourceFieldsResponse
            {
                SourceCustomFields = sourceCustomFields,
                UserCustomFields = userCustomFields,
                SourceProjects = sourceProjects,
                UserProject = userProject
            };
            return sourceFieldsResponse;
        }
    }
}
