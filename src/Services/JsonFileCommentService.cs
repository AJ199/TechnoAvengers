using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service to handle comment data stored in a JSON file
    /// </summary>
    public class JsonFileCommentService
    {
        // Provides access to the web root path
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Constructor injecting the hosting environment
        /// </summary>
        /// <param name="webHostEnvironment">Hosting environment</param>
        public JsonFileCommentService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Path to the comments JSON file
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "comments.json");
            }
        }

        /// <summary>
        /// Retrieves all comments for a specific superhero by ID
        /// </summary>
        /// <param name="superheroId">The ID of the superhero</param>
        /// <returns>List of matching CommentModel objects</returns>
        public List<CommentModel> GetComments(string superheroId)
        {
            // Read all text from the comments file
            var fileContent = File.ReadAllText(JsonFileName);

            // Deserialize JSON into list
            var allComments = JsonSerializer.Deserialize<List<CommentModel>>(fileContent);

            if (allComments == null)
            {
                return new List<CommentModel>();
            }

            // Return only comments matching the superhero ID
            return allComments.Where(c => c.SuperheroId == superheroId).ToList();
        }

        /// <summary>
        /// Adds a new comment to the JSON file
        /// </summary>
        /// <param name="comment">The comment to add</param>
        public void AddComment(CommentModel comment)
        {
            // Deserialize into comment list
            var comments = JsonSerializer.Deserialize<List<CommentModel>>(File.ReadAllText(JsonFileName)) ?? new List<CommentModel>();

            comments.Add(comment);

            // Write back the updated list
            File.WriteAllText(JsonFileName, JsonSerializer.Serialize(comments));
        }

        /// <summary>
        /// Updates the number of likes (or dislikes) for a comment and returns the updated comment.
        /// </summary>
        /// <param name="commentId">ID of the comment</param>
        /// <param name="delta">+1 for upvote, -1 for downvote</param>
        /// <returns>The updated comment or null if not found</returns>
        public CommentModel? UpdateLikes(string commentId, int delta)
        {
            // Deserialize comments
            var comments = JsonSerializer.Deserialize<List<CommentModel>>(File.ReadAllText(JsonFileName)) ?? new List<CommentModel>();

            // Find the target comment
            var comment = comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return null;
            }

            comment.Likes += delta;

            // Write the updated comments back to file
            File.WriteAllText(JsonFileName, JsonSerializer.Serialize(comments));

            return comment;
        }
    }
}