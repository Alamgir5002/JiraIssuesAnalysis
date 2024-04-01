using WebApplication7.Models;

namespace IssueAnalysisExtended.Models
{
    public class SourceFieldsResponse
    {
        public List<CustomField>? SourceCustomFields { get; set; }
        public CustomField? TeamBoardCustomField { get; set; }
        public CustomField? StoryPointsCustomField { get; set; }
        public List<Project>? SourceProjects { get; set; }
        public Project? UserProject { get; set;}

        public static SourceFieldsResponse processSourceResponse(List<CustomField> sourceCustomFields, 
            CustomField? teamBoardCustomField, CustomField? storyPointsCustomField, 
            List<Project> sourceProjects,
            Project userProject) {

            SourceFieldsResponse sourceFieldsResponse = new SourceFieldsResponse
            {
                SourceCustomFields = sourceCustomFields,
                TeamBoardCustomField = teamBoardCustomField,
                StoryPointsCustomField = storyPointsCustomField,
                SourceProjects = sourceProjects,
                UserProject = userProject
            };
            return sourceFieldsResponse;
        }
    }
}
