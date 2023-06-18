using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace XUnitTests.Services
{
    public class TestManagementServiceTests
    {
        private Mock<IQuestionRepository> _questionRepositoryMock;
        private Mock<IQuestionOptionRepository> _optionRepositoryMock;
        private Mock<ILogger<TestManagementService>> _loggerMock;

        public TestManagementServiceTests()
        {
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _optionRepositoryMock = new Mock<IQuestionOptionRepository>();
            _loggerMock = new Mock<ILogger<TestManagementService>>();
        }

        [Fact]
        public async void AddQuestionAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var testPk = 1;
            var questionData = new QuestionViewModel
            {
                QuestionText = "Sample question",
                Type = QuestionType.SingleChoice,
                ChoiceOptions = new List<QuestionChoiceOptionViewModel>
                {
                    new QuestionChoiceOptionViewModel { OptionText = "Option 1", IsCorrect = true },
                    new QuestionChoiceOptionViewModel { OptionText = "Option 2", IsCorrect = false }
                }
            };

            var service = new TestManagementService(
                _questionRepositoryMock.Object,
                _optionRepositoryMock.Object,
                _loggerMock.Object
            );

            _questionRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<QuestionEntity>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddQuestionAsync(testPk, questionData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void EditQuestionAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var testPk = 1;
            var oldQuestionEntity = new QuestionEntity
            {
                Id = 1,
                TestId = testPk,
                QuestionText = "Old question",
                Type = QuestionType.SingleChoice,
                ChoiceOptions = new List<QuestionChoiceOptionEntity>
                {
                    new QuestionChoiceOptionEntity { OptionText = "Option 1", IsCorrect = true },
                    new QuestionChoiceOptionEntity { OptionText = "Option 2", IsCorrect = false }
                }
            };

            var questionData = new QuestionViewModel
            {
                QuestionText = "Updated question",
                Type = QuestionType.SingleChoice,
                ChoiceOptions = new List<QuestionChoiceOptionViewModel>
                {
                    new QuestionChoiceOptionViewModel { OptionText = "Option 1", IsCorrect = true },
                    new QuestionChoiceOptionViewModel { OptionText = "Option 2", IsCorrect = false },
                    new QuestionChoiceOptionViewModel { OptionText = "Option 3", IsCorrect = false }
                }
            };

            var service = new TestManagementService(
                _questionRepositoryMock.Object,
                _optionRepositoryMock.Object,
                _loggerMock.Object
            );

            _questionRepositoryMock
                .Setup(repo => repo.DeleteAllQuestionOptionsAsync(testPk))
                .ReturnsAsync(oldQuestionEntity.ChoiceOptions.Count);

            _optionRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<QuestionChoiceOptionEntity>()))
                .ReturnsAsync(true);

            _questionRepositoryMock
                .Setup(repo => repo.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            var result = await service.EditQuestionAsync(testPk, oldQuestionEntity, questionData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void DeleteQuestionAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var testPk = 1;
            var questionEntity = new QuestionEntity
            {
                Id = 1,
                TestId = testPk,
                QuestionText = "Sample question",
                Type = QuestionType.SingleChoice
            };

            var service = new TestManagementService(
                _questionRepositoryMock.Object,
                _optionRepositoryMock.Object,
                _loggerMock.Object
            );

            _questionRepositoryMock
                .Setup(repo => repo.DeleteAsync(questionEntity))
                .ReturnsAsync(true);

            // Act
            var result = await service.DeleteQuestionAsync(testPk, questionEntity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void GetQuestionsByTestAsync_ValidData_ReturnsListOfQuestions()
        {
            // Arrange
            var testPk = 1;
            var questions = new List<QuestionEntity>
            {
                new QuestionEntity { Id = 1, TestId = testPk, QuestionText = "Question 1", Type = QuestionType.SingleChoice },
                new QuestionEntity { Id = 2, TestId = testPk, QuestionText = "Question 2", Type = QuestionType.MultipleChoice }
            };

            var service = new TestManagementService(
                _questionRepositoryMock.Object,
                _optionRepositoryMock.Object,
                _loggerMock.Object
            );

            _questionRepositoryMock
                .Setup(repo => repo.GetAllQuestionsByTestAsync(testPk))
                .ReturnsAsync(questions);

            // Act
            var result = await service.GetQuestionsByTestAsync(testPk);

            // Assert
            Assert.Equal(questions, result);
        }

        [Fact]
        public async void GetQuestionByTestAndIdAsync_ValidData_ReturnsQuestion()
        {
            // Arrange
            var testPk = 1;
            var questionPk = 1;
            var question = new QuestionEntity { Id = questionPk, TestId = testPk, QuestionText = "Sample question", Type = QuestionType.SingleChoice };

            var service = new TestManagementService(
                _questionRepositoryMock.Object,
                _optionRepositoryMock.Object,
                _loggerMock.Object
            );

            _questionRepositoryMock
                .Setup(repo => repo.GetQuestionAsync(testPk, questionPk))
                .ReturnsAsync(question);

            // Act
            var result = await service.GetQuestionByTestAndIdAsync(testPk, questionPk);

            // Assert
            Assert.Equal(question, result);
        }

        [Fact]
        public async Task AddQuestionAsync_ReturnsTrue_WhenQuestionAddedSuccessfully()
        {
            // Arrange
            var service = new TestManagementService(_testRepositoryMock.Object, _questionRepositoryMock.Object, _logger);
            int testPk = 1;
            var questionData = new QuestionViewModel
            {
                Type = QuestionType.SingleChoice,
                Marks = 1,
                QuestionText = "What is the capital of France?",
                ChoiceOptions = new List<QuestionChoiceOptionViewModel>
                {
                    new QuestionChoiceOptionViewModel { OptionText = "Paris", IsCorrect = true },
                    new QuestionChoiceOptionViewModel { OptionText = "London", IsCorrect = false },
                    new QuestionChoiceOptionViewModel { OptionText = "Berlin", IsCorrect = false }
                }
            };

            _questionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<QuestionEntity>()))
                .ReturnsAsync(true);

            // Act
            bool result = await service.AddQuestionAsync(testPk, questionData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditQuestionAsync_ReturnsTrue_WhenQuestionEditedSuccessfully()
        {
            // Arrange
            var service = new TestManagementService(_testRepositoryMock.Object, _questionRepositoryMock.Object, _logger);
            int testPk = 1;
            int questionPk = 1;
            var oldQuestionEntity = new QuestionEntity
            {
                Id = questionPk,
                TestId = testPk,
                Type = QuestionType.SingleChoice,
                Marks = 1,
                QuestionText = "What is the capital of France?"
                // Set other properties as needed
            };
            var questionData = new QuestionViewModel
            {
                Type = QuestionType.SingleChoice,
                Marks = 2,
                QuestionText = "What is the capital of Italy?",
                ChoiceOptions = new List<QuestionChoiceOptionViewModel>
                {
                    new QuestionChoiceOptionViewModel { OptionText = "Rome", IsCorrect = true },
                    new QuestionChoiceOptionViewModel { OptionText = "Paris", IsCorrect = false },
                    new QuestionChoiceOptionViewModel { OptionText = "Madrid", IsCorrect = false }
                }
            };

            _questionRepositoryMock.Setup(repo => repo.DeleteAllQuestionOptionsAsync(testPk))
                .ReturnsAsync(3); // Assuming 3 options were deleted

            _questionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<QuestionEntity>()))
                .ReturnsAsync(true);

            _questionRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            bool result = await service.EditQuestionAsync(testPk, oldQuestionEntity, questionData);

            // Assert
            Assert.True(result);
        }
    }
}
