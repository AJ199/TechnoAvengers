using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for the KnowledgeQuizModel page model.
    /// </summary>
    public class KnowledgeQuizModelTests
    {
        #region Setup
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
        #endregion

        #region OnGet
        /// <summary>
        /// Verifies that OnGet() populates the CorrectAnswers list with answers from Questions.
        /// </summary>
        [Test]
        public void OnGet_Valid_Model_State_Should_Populate_Correct_Answers()
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
        public void OnGet_Valid_Questions_Setter_Should_Assign_New_Questions()
        {
            // Arrange
            // Create a new question list
            var data = new List<QuestionModel>
            {
                new QuestionModel
                {
                    Question = "Test?",
                    Options = new[] { "A", "B", "C" },
                    Answer = "A"
                }
            };

            // Act
            // Assign the new list to the Questions property
            pageModel.Questions = data;

            // Assert: Check if Questions was updated correctly
            Assert.AreEqual(1, pageModel.Questions.Count);
            Assert.AreEqual("Test?", pageModel.Questions[0].Question);
        }

        /// <summary>
        /// Validates that OnGet correctly populates the Questions list
        /// </summary>
        [Test]
        public void OnGet_Valid_Options_Getter_Should_Return_Expected_Values()
        {
            // Arrange
            pageModel.OnGet();

            var data = pageModel.Questions[0];

            // Act
            var result = data.Options;

            // Assert
            Assert.AreEqual(true, result.Contains(data.Answer));
        }
        #endregion
    }
}