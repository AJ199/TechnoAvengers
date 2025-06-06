namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Represents the result of a poll with vote counts for "Yes" and "No" responses.
    /// </summary>
    public class PollResultModel
    {
        /// <summary>
        /// Total number of "Yes" votes recorded.
        /// </summary>
        public int YesVotes { get; set; }

        /// <summary>
        /// Total number of "No" votes recorded.
        /// </summary>
        public int NoVotes { get; set; }
    }
}
