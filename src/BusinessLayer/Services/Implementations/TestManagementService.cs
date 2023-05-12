using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using BusinessLayer.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementations
{
    public partial class TestManagementService : ITestManagementService
    {
        private readonly ITestRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionChosenOptionRepository _optionRepository;
        private readonly ILogger<TestManagementService> _logger;

        public TestManagementService(
            ITestRepository testRepository,
            IQuestionRepository questionRepository,
            IQuestionChosenOptionRepository optionRepository,
            ILogger<TestManagementService> logger)
        {
            this._testRepository = testRepository;
            this._questionRepository = questionRepository;
            this._optionRepository = optionRepository;
            this._logger = logger;
        }

        private async Task<int> _AddQuestions(
            TestEntity testEntity,
            List<QuestionViewModel> questionDataList)
        {
            int addedQuestions = 0;

            foreach (var questionData in questionDataList)
            {
                var questionEntity = QuestionViewModel.ToEntity(questionData);
                questionEntity.TestEntity = testEntity;

                bool addedQuestion = await this._questionRepository.AddAsync(questionEntity);

                if (!addedQuestion)
                {
                    continue;
                }

                bool addedAllOptions = true;
                if (questionEntity.Type == QuestionType.SingleChoice ||
                    questionEntity.Type == QuestionType.MultipleChoice)
                {
                    int optionsCount = await this._AddQuestionOptions(questionEntity, questionData.ChoiceOptions);
                    addedAllOptions = questionData.ChoiceOptions.Count == optionsCount;
                }

                if (addedAllOptions)
                {
                    ++addedQuestions;
                }
            }

            return addedQuestions;
        }

        public async Task<bool> AddTestAsync(
            int? classroomPk,
            TestViewModel testData)
        {
            try
            {
                if (classroomPk == null)
                {
                    return false;
                }

                var testEntity = TestViewModel.ToEntity(testData);
                testEntity.ClassroomId = (int)classroomPk;

                bool addedTest = await this._testRepository.AddAsync(testEntity);

                if (addedTest)
                {
                    int addedQuestions = await this._AddQuestions(testEntity, testData.Questions);
                    int expected = testData.Questions.Count;
                    if (addedQuestions != expected)
                    {
                        this._logger.LogWarning($"Mismatch: Not all Questions were added in AddTestAsync: expected {expected}, fully written {addedQuestions}");
                    }

                    return true;
                }

                this._logger.LogWarning("Failed to add TestEntity into classroom, but no exception came.");
                return false;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in AddClassroomAsync in TestManagementService.cs");
                return false;
            }
        }

        public async Task<bool> EditTestAsync(int? classroomPk, TestEntity oldTestEntity, TestViewModel testData)
        {
            try
            {
                if (classroomPk == null)
                {
                    return false;
                }

                if (oldTestEntity.ClassroomId != classroomPk)
                {
                    this._logger.LogWarning("Trying to edit testEntity into other classroomEntity than declared");
                    return false;
                }

                oldTestEntity.AttemptsAllowed = testData.AttemptsAllowed;

                oldTestEntity.DurationSeconds = testData.DurationSeconds;

                oldTestEntity.StartDateTime = testData.StartDateTime;

                oldTestEntity.EndDateTime = testData.EndDateTime;

                oldTestEntity.TotalGrade = testData.TotalGrade;

                int deletedQuestions = await this._testRepository.DeleteAllQuestionsAsync(oldTestEntity.Id);
                int addedQuestions = await this._AddQuestions(oldTestEntity, testData.Questions);

                if (deletedQuestions != addedQuestions)
                {
                    this._logger.LogInformation($"Changed question count on test {oldTestEntity.Id} in EditTestAsync: deleted {deletedQuestions}, added {addedQuestions}, balance: {-deletedQuestions + addedQuestions}");
                }


                return await this._testRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in EditClassroomAsync in TestManagementService.cs");
                return false;
            }
        }

        public async Task<bool> DeleteTestAsync(int? classroomPk, TestEntity testEntity)
        {
            try
            {
                if (classroomPk == null)
                {
                    return false;
                }

                if (testEntity.ClassroomId == classroomPk)
                {
                    await this._testRepository.DeleteAllQuestionsAsync(testEntity.Id);
                    return await this._testRepository.DeleteAsync(testEntity);
                }

                this._logger.LogWarning("Trying to delete testEntity into other classroomEntity than declared");
                return false;

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in DeleteClassroomAsync in TestManagementService.cs");
                return false;
            }
        }

        public async Task<List<TestEntity>> GetTestListByClassroomAsync(int? classroomPk)
        {
            try
            {
                if (classroomPk == null)
                {
                    return new List<TestEntity>();
                }

                return await this._testRepository.GetAllTestByClassroomAsync((int)classroomPk);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetTestListByClassroomAsync in TestManagementService.cs");
                return new List<TestEntity>();
            }
        }

        public async Task<TestEntity?> GetTestByClassroomAndIdAsync(int? classroomPk, int? testPk)
        {
            try
            {
                if (classroomPk == null || testPk == null)
                {
                    return null;
                }

                return await this._testRepository.GetTestAsync((int)classroomPk, (int)testPk);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetTestByClassroomAndIdAsync in TestManagementService.cs");
                return null;
            }
        }
    }
}
