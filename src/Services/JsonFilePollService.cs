using System.IO;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service to handle poll voting data stored in a JSON file
    /// </summary>
    public class JsonFilePollService
    {
        /// <summary>
        /// Provides access to the web root path via the hosting environment
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Constructor that sets the hosting environment for path resolution
        /// </summary>
        /// <param name="webHostEnvironment">Injected hosting environment</param>
        public JsonFilePollService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Path to the JSON file storing poll results
        /// </summary>
        private string JsonFileName =>
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "poll.json");

        /// <summary>
        /// Retrieves the current poll results from the JSON file
        /// </summary>
        /// <returns>A PollResultModel with current vote counts</returns>
        public PollResultModel GetPollResult()
        {
            if (!File.Exists(JsonFileName))
            {
                return new PollResultModel { YesVotes = 0, NoVotes = 0 };
            }

            var json = File.ReadAllText(JsonFileName);
            return JsonSerializer.Deserialize<PollResultModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new PollResultModel();
        }

        /// <summary>
        /// Saves the updated poll result to the JSON file
        /// </summary>
        /// <param name="result">The updated poll result</param>
        public void SavePollResult(PollResultModel result)
        {
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(JsonFileName, json);
        }

        /// <summary>
        /// Updates the vote count based on the user's response
        /// </summary>
        /// <param name="response">"yes" or "no" string</param>
        /// <returns>The updated poll result</returns>
        public PollResultModel UpdatePoll(string response)
        {
            var poll = GetPollResult();

            if (response == "yes") poll.YesVotes++;
            else if (response == "no") poll.NoVotes++;

            SavePollResult(poll);
            return poll;
        }
    }
}
