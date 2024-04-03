using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace IssueAnalysisExtended.Services
{
    public class TruncationService
    {
        private readonly DatabaseContext _databaseContext;
        public TruncationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Boolean RemoveAllDataFromCircularDependantTables()
        {
            try
            {
                while (true)
                {
                    // Remove all issues
                    var unlinkedIssues = _databaseContext.Issues.ToList();
                    if (unlinkedIssues.Any())
                    {
                        _databaseContext.Issues.RemoveRange(unlinkedIssues);
                        _databaseContext.SaveChanges();
                    }

                    // Remove all ParentIssues
                    var parentIssues = _databaseContext.ParentIssues.ToList();
                    if (parentIssues.Any())
                    {
                        _databaseContext.ParentIssues.RemoveRange(parentIssues);
                        _databaseContext.SaveChanges();
                    }

                    // Remove all IssueTypes
                    var issueTypes = _databaseContext.IssueTypes.ToList();
                    if (issueTypes.Any())
                    {
                        _databaseContext.IssueTypes.RemoveRange(issueTypes);
                        _databaseContext.SaveChanges();
                    }

                    // Remove all teamboards
                    var teamboards = _databaseContext.TeamBoards.ToList();
                    if (teamboards.Any())
                    {
                        _databaseContext.TeamBoards.RemoveRange(teamboards);
                        _databaseContext.SaveChanges();
                    }

                    // Remove all releases
                    var releases = _databaseContext.Releases.ToList();
                    if (releases.Any())
                    {
                        _databaseContext.Releases.RemoveRange(releases);
                        _databaseContext.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
