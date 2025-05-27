using ContosoCrafts.WebSite.Pages;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the KnowledgeQuizModel page model.
    /// </summary>
    public class KnowledgeQuizModelTests
    {
        // The page model instance for each test
        private KnowledgeQuizModel pageModel;

        /// <summary>
        /// Initializes a new instance of KnowledgeQuizModel before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            pageModel = new KnowledgeQuizModel();
        }

        /// <summary>
        /// Verifies that OnGet() populates the CorrectAnswers list with answers from Questions.
        /// </summary>
        [Test]
        public void OnGet_Should_Populate_CorrectAnswers()
        {
            // Act: Call OnGet to initialize CorrectAnswers
            pageModel.OnGet();

            // Assert: CorrectAnswers list should have 10 answers (matching Questions count)
            Assert.AreEqual(10, pageModel.CorrectAnswers.Count);
            Assert.AreEqual("Captain America", pageModel.CorrectAnswers[0]); // Check first answer
        }

        /// <summary>
        /// Verifies that setting the Questions property updates the Questions list.
        /// </summary>
        [Test]
        public void Questions_Setter_Should_Assign_New_Questions()
        {
            // Arrange: Create a new question list
            var newQuestions = new List<KnowledgeQuizModel.QuestionModel>
            {
                new KnowledgeQuizModel.QuestionModel
                {
                    Question = "Test?",
                    Options = new[] { "A", "B", "C" },
                    Answer = "A"
                }
            };

            // Act: Assign the new list to the Questions property
            pageModel.Questions = newQuestions;

            // Assert: Check if Questions was updated correctly
            Assert.AreEqual(1, pageModel.Questions.Count);
            Assert.AreEqual("Test?", pageModel.Questions[0].Question);
        }
    }
}
