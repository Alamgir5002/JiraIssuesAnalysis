﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class IssueMapperService
    {
        public List<Issue> MapToIssueObject(string responseBody, string sourceUrl)
        {
            JObject jsonObject = JObject.Parse(responseBody);
            var issues = jsonObject["issues"]
                .Select(item => new Issue
                {
                    Id = castValueToGivenType<string>(item["key"]),
                    IssueType = castValueToGivenType<string>(item["fields"]["issuetype"]["name"]),
                    Description = castValueToGivenType<string>(item["fields"]["description"]),
                    IssueEstimatedAndSpentTime = convertTimeToEstimatedAndSpentTime(item["fields"]["aggregatetimespent"], 
                                                    item["fields"]["aggregatetimeoriginalestimate"]),
                    Summary = castValueToGivenType<string>(item["fields"]["summary"]),
                    CreatedDate = getFormattedDate(item["fields"]["created"]),
                    ResolvedDate = getFormattedDate(item["fields"]["resolutiondate"]),
                    Priority = castValueToGivenType<string>(item["fields"]["priority"]["name"]),
                    StoryPoints = castValueToGivenType<int>(item["fields"]["customfield_10024"]),
                    Status = castValueToGivenType<string>(item["fields"]["status"]["name"]),
                    Parent = convertToParent(item["fields"]["parent"], sourceUrl),
                    FixVersions = getReleaseList(item["fields"]["fixVersions"]),
                    TeamBoard = getTeamBoard(item["fields"]?["customfield_10021"]),
                    IssueUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(item["key"])),
                });

            return new List<Issue>(issues);
        }
        

        private List<Release> getReleaseList(JToken jsonObject)
        {
            if (jsonObject.IsNullOrEmpty())
            {
                return null;
            }
            var list = new List<Release>();
            foreach (var item in jsonObject)
            {
                list.Add(new Release
                {
                    Id = castValueToGivenType<string>(item["id"]),
                    Released = castValueToGivenType<bool>(item["released"]),
                    Name = castValueToGivenType<string>(item["name"]),
                    ReleaseDate = getFormattedDate(castValueToGivenType<string>(item["releaseDate"]), "yyyy-mm-dd")
                });
            }

            return list;
        }
        
        private TeamBoard getTeamBoard(JToken jsonObject)
        {
            if (jsonObject.IsNullOrEmpty())
                return null;

            TeamBoard teamBoard = new TeamBoard
            {
                Name = castValueToGivenType<string>(jsonObject["value"]),
                Id = castValueToGivenType<string>(jsonObject["id"])
            };
            return teamBoard;
        }
        

        private Uri prepareIssueUrl(string sourceUrl, string issueId)
        {
            if (String.IsNullOrEmpty(issueId) || String.IsNullOrWhiteSpace(issueId))
            {
                return null;
            }

            return new Uri(new Uri(sourceUrl), $"browse/{issueId}");
        }

        
        private string getFormattedDate(JToken date, string timeFormat = "dd/MM/yyyy HH:mm:ss")
        {
            string formattedDate = date.ToString();
            if (String.IsNullOrEmpty(formattedDate) || String.IsNullOrWhiteSpace(formattedDate))
            {
                return formattedDate;
            }

            DateTime parsedDate = DateTime.ParseExact(formattedDate, timeFormat, 
                System.Globalization.CultureInfo.InvariantCulture);
            return parsedDate.ToString("dd/MM/yyyy");
        }

        private T castValueToGivenType<T>(JToken jToken)
        {
            try
            {
                if (jToken == null || (jToken.Type == JTokenType.String && string.IsNullOrWhiteSpace(jToken.ToString())))
                {
                    return default(T);
                }

                return jToken.ToObject<T>();
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }
        
        private Parent? convertToParent(JToken jToken, string sourceUrl)
        {
            if(jToken.IsNullOrEmpty())
            {
                return null;
            }

            Parent parent = new Parent
            {
                Id = castValueToGivenType<string>(jToken["key"]),
                ParentUrl = prepareIssueUrl(sourceUrl, castValueToGivenType<string>(jToken["key"])),
                Summary = castValueToGivenType<string>(jToken["fields"]["summary"]),
                Status = castValueToGivenType<string>(jToken["fields"]["status"]["name"]),
                Priority = castValueToGivenType<string>(jToken["fields"]["priority"]["name"]),
                IssueType = castValueToGivenType<string>(jToken["fields"]["issuetype"]["name"])
            };
            return parent;
        }
        
        private EstimatedAndSpentTime convertTimeToEstimatedAndSpentTime(JToken jTokenEstimatedTime, JToken jTokenSpentTime)
        {
            int estimatedTime = castValueToGivenType<int>(jTokenEstimatedTime);
           int timeSpent = castValueToGivenType<int>(jTokenSpentTime);


            EstimatedAndSpentTime estimatedAndSpentTime = new EstimatedAndSpentTime
            {
                AggregateTimeEstimate = estimatedTime,
                AggregatedTimeEstimateInDays = calculateTimeInDays(estimatedTime),
                AggregatedTimeSpent = timeSpent,
                AggregatedTimeSpentInDays = calculateTimeInDays(timeSpent)
            };
            return estimatedAndSpentTime;
        }

        private int calculateTimeInDays(int timeInSeconds)
        {
            return timeInSeconds / (3600 * 8);
        }
    }
}
