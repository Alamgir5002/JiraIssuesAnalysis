using Quartz;
using System.Text.Json;
using WebApplication7.Services;

namespace WebApplication7.Quartz.Jobs
{
    public class JiraIssuesSyncJob : IJob
    {
        private IssuesService issuesService;
        private readonly ILogger<JiraIssuesSyncJob> _logger;

        public JiraIssuesSyncJob(IssuesService issuesService, ILogger<JiraIssuesSyncJob> logger)
        {
            this.issuesService = issuesService;
            _logger = logger;
        }
    
        public async Task Execute(IJobExecutionContext context)
        {
            string fixVersion = "1.9.6.20";
            try
            {
                var response = await issuesService.FetchIssuesAgainstRelease(fixVersion);
                issuesService.processIssuesList(response);

                string jsonString = JsonSerializer.Serialize(response);

                string filePath = Guid.NewGuid().ToString() + ".json";
                writeToFile(filePath, jsonString);
                //file.writealltext(filepath, jsonstring);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void writeToFile(string fileName, string fileContent)
        {
            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Define your application's subfolder name
            string userContentFolderName = "jira-responses";

            // Combine paths to create the user content folder path
            string userContentPath = Path.Combine(applicationDataPath, userContentFolderName);

            // Create the folder if it doesn't exist
            if (!Directory.Exists(userContentPath))
            {
                Directory.CreateDirectory(userContentPath);
            }

            // Save user-generated file to the user content folder
            //string fileName = "user_generated_file.txt"; // Replace with actual filename
            string filePath = Path.Combine(userContentPath, fileName);
            File.WriteAllText(filePath, fileContent);

            _logger.LogInformation(filePath + " saved at: " + DateTime.UtcNow);
        }
    }
}
