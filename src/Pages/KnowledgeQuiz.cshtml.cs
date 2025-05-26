using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Pages
{
    public class KnowledgeQuizModel : PageModel
    {
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>
        {
            new QuestionModel
            {
                Question = "Who is the leader of the Avengers?",
                Options = new[] { "Iron Man", "Captain America", "Thor", "Hulk" },
                Answer = "Captain America"
            },
            new QuestionModel
            {
                Question = "What is Black Widow's real name?",
                Options = new[] { "Natasha Romanoff", "Wanda Maximoff", "Carol Danvers", "Hope van Dyne" },
                Answer = "Natasha Romanoff"
            },
            new QuestionModel
            {
                Question = "Which Avenger uses a shield?",
                Options = new[] { "Iron Man", "Hawkeye", "Captain America", "Vision" },
                Answer = "Captain America"
            },
            new QuestionModel
            {
                Question = "Who is the God of Thunder?",
                Options = new[] { "Thor", "Loki", "Odin", "Hela" },
                Answer = "Thor"
            },
            new QuestionModel
            {
                Question = "What is Tony Stark's superhero name?",
                Options = new[] { "Iron Man", "War Machine", "Falcon", "Hawkeye" },
                Answer = "Iron Man"
            },
            new QuestionModel
            {
                Question = "Who can shrink in size in the Avengers?",
                Options = new[] { "Spider-Man", "Ant-Man", "Hulk", "Black Panther" },
                Answer = "Ant-Man"
            },
            new QuestionModel
            {
                Question = "Which stone is in Vision's forehead?",
                Options = new[] { "Mind Stone", "Time Stone", "Space Stone", "Power Stone" },
                Answer = "Mind Stone"
            },
            new QuestionModel
            {
                Question = "Who is the sister of Black Panther?",
                Options = new[] { "Okoye", "Shuri", "Nakia", "Ramonda" },
                Answer = "Shuri"
            },
            new QuestionModel
            {
                Question = "Which Avenger is a master archer?",
                Options = new[] { "Hawkeye", "Falcon", "Quicksilver", "Spider-Man" },
                Answer = "Hawkeye"
            },
            new QuestionModel
            {
                Question = "What is Captain Marvel's real name?",
                Options = new[] { "Carol Danvers", "Wanda Maximoff", "Peggy Carter", "Natasha Romanoff" },
                Answer = "Carol Danvers"
            }
        };

        public List<string> CorrectAnswers { get; set; }

        public void OnGet()
        {
            CorrectAnswers = new List<string>();
            foreach (var question in Questions)
            {
                CorrectAnswers.Add(question.Answer);
            }
        }

        public class QuestionModel
        {
            public string Question { get; set; }
            public string[] Options { get; set; }
            public string Answer { get; set; }
        }
    }
}
