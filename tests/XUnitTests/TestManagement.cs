using BusinessLayer.Services.Implementations;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using XUnitTests.Fixtures;

namespace XUnitTests
{
    public class TestManagementServiceTests
    {
        // Repositories mock
        private readonly Mock<ITestRepository> _testRepositoryMock = new();
        private readonly Mock<IQuestionRepository> _questionRepositoryMock = new();
        private readonly Mock<IQuestionChosenOptionRepository> _questionOptionRepositoryMock = new();
        
        // logger mock
        private readonly Mock<ILogger<TestManagementService>> _loggerMock = new();

        // service
        private readonly TestManagementService _testManagementService;

        // mock collections
        private readonly List<TestEntity> _testEntities = new();

        private void configTestRepositoryMock()
        {
            this._testRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            this._testRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync((TestEntity entity) =>
                {
                    this._testEntities.Add(entity);
                    return true;
                });

            this._testRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<TestEntity>()))
                .ReturnsAsync((TestEntity entity) =>
                {
                    this._testEntities.Remove(entity);
                    return true;
                });

            this._testRepositoryMock
                .Setup(x => x.GetAllTestByClassroomAsync(It.IsAny<int>()))
                .ReturnsAsync((int x) => this._testEntities.Where(te => te.ClassroomId == x).ToList());

            this._testRepositoryMock
                .Setup(x => x.FetchAll())
                .ReturnsAsync(this._testEntities);

            this._testRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) =>
                {
                    return this._testEntities.FirstOrDefault(t => t.Id == i);
                });

            this._testRepositoryMock
                .Setup(x => x.GetTestAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int c, int t) =>
                {
                    return this._testEntities.FirstOrDefault(te => te.ClassroomId == c && te.Id == t);
                });
        }


        private void configQuestionRepository()
        {

            this._questionRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            this._questionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<QuestionEntity>()))
                .ReturnsAsync((QuestionEntity entity) =>
                {

                    var testEntity = this._testEntities.Find(t => t.Id == entity.Id);
                    if (testEntity is not null)
                    {
                        testEntity.Questions.Add(entity);
                        return true;
                    }

                    throw new DbUpdateException();
                });

        }

        private void configQuestionOptionRepositoryMock()
        {
            this._questionOptionRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            this._questionOptionRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<QuestionChoiceOptionEntity>()))
                .ReturnsAsync((QuestionChoiceOptionEntity entity) =>
                {

                    var testEntity = this._testEntities.Find(t => t.Questions.Any(q => q.Id == entity.QuestionId));
                    if (testEntity is not null)
                    {
                        var questionEntity = testEntity.Questions.FirstOrDefault(q => q.Id == entity.QuestionId);
                        questionEntity.ChoiceOptions.Add(entity);
                        return true;
                    }

                    throw new DbUpdateException();
                });

            this._questionOptionRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<QuestionChoiceOptionEntity>()))
                .ReturnsAsync((QuestionChoiceOptionEntity qoe) =>
                {
                    var te = this._testEntities.Find(t =>
                        t.Questions.Any(q => q.ChoiceOptions.Any(qo => qo.Id == qoe.Id)));
                    if (te is not null)
                    {
                        var questionEntity =
                            te.Questions.FirstOrDefault(t => t.ChoiceOptions.Any(qo => qo.Id == qoe.Id));
                        questionEntity!.ChoiceOptions.Remove(
                            questionEntity!.ChoiceOptions.FirstOrDefault(qo => qo.Id == qoe.Id)!);
                        return true;
                    }

                    throw new DbUpdateException();
                });
        }


        public TestManagementServiceTests()
        {
            this.configQuestionOptionRepositoryMock();
            this.configTestRepositoryMock();
            this.configQuestionRepository();

            this._testManagementService = new TestManagementService(
                this._testRepositoryMock.Object,
                this._questionRepositoryMock.Object,
                this._questionOptionRepositoryMock.Object,
                this._loggerMock.Object);
        }

        [Fact]
        public async Task AddTestAsync_ReturnsFalse_WhenClassroomPkIsNull()
        {
            // Arrange
            int? classroomPk = null;
            var testData = new TestViewModel();

            // Act
            bool result = await this._testManagementService.AddTestAsync(classroomPk, testData);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddTestAsync_ReturnsFalse_WhenTestIsNotAdded()
        {
            // Arrange
            int? classroomPk = 1;
            var testData = new TestViewModel();

            // Act
            bool result = await this._testManagementService.AddTestAsync(classroomPk, testData);

            // Assert
            Assert.False(result);
            this._testRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TestEntity>()), Times.Once);
            this._questionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<QuestionEntity>()), Times.Never);
        }

        [Fact]
        public async Task AddTestAsync_ReturnsTrue_WhenTestAndQuestionsAreAdded()
        {
            // Arrange
            int? classroomPk = 1;
            var testData = Tests.CapitalsTest;

            // Act
            bool result = await this._testManagementService.AddTestAsync(classroomPk, testData);

            // Assert
            Assert.True(result);
            this._testRepositoryMock.Verify(x => x.AddAsync(It.IsAny<TestEntity>()), Times.Once);
            this._questionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<QuestionEntity>()), Times.Exactly(2));
        }
    }
}