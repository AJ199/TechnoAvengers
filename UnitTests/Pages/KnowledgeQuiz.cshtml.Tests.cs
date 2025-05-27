using ContosoCrafts.WebSite.Pages;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Pages
{
    public class KnowledgeQuizModelTests
    {
        private KnowledgeQuizModel pageModel;

        [SetUp]
        public void Setup()
        {
            pageModel = new KnowledgeQuizModel();
        }

        [Test]
        public void OnGet_Should_Populate_CorrectAnswers()
        {
            // Act
            pageModel.OnGet();

            // Assert
            Assert.AreEqual(10, pageModel.CorrectAnswers.Count);
            Assert.AreEqual("Captain America", pageModel.CorrectAnswers[0]);
        }

        [Test]
        public void Questions_Setter_Should_Assign_New_Questions()
        {
            // Arrange
            var newQuestions = new List<KnowledgeQuizModel.QuestionModel>
            {
                new KnowledgeQuizModel.QuestionModel
                {
                    Question = "Test?",
                    Options = new[] { "A", "B", "C" },
                    Answer = "A"
                }
            };

            // Act
            pageModel.Questions = newQuestions;

            // Assert
            Assert.AreEqual(1, pageModel.Questions.Count);
            Assert.AreEqual("Test?", pageModel.Questions[0].Question);
        }
    }
}
