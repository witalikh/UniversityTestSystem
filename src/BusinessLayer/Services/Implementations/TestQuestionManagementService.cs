using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementations
{
    public partial class TestManagementService
    {
        private async Task<int> _AddQuestionOptions(QuestionEntity question, List<QuestionChoiceOptionViewModel> options)
        {
            int addedOptions = 0;
            foreach (var optionData in options)
            {
                var optionEntity = QuestionChoiceOptionViewModel.ToEntity(optionData);
                optionEntity.QuestionEntity = question;

                bool addedOption = await this._optionRepository.AddAsync(optionEntity);
                if (addedOption)
                {
                    ++addedOptions;
                };
            }

            int expected = options.Count;
            if (expected != addedOptions)
            {
                this._logger.LogWarning(
                    $"Mismatch: Not all Options were added in _AddQuestionOptions: expected {expected}, fully written {addedOptions}");
            }

            return addedOptions;
        }

        public async Task<bool> AddQuestionAsync(int? testPk, QuestionViewModel questionData)
        {
            try
            {
                if (testPk == null)
                {
                    return false;
                }

                var questionEntity = QuestionViewModel.ToEntity(questionData);
                questionEntity.TestId = testPk.Value;

                bool questionAdded = await this._questionRepository.AddAsync(questionEntity);
                if (questionAdded)
                {
                    if (questionEntity.Type == QuestionType.SingleChoice ||
                        questionEntity.Type == QuestionType.MultipleChoice)
                    {
                        int _ = await this._AddQuestionOptions(questionEntity, questionData.ChoiceOptions);
                    }

                    return true;
                }

                this._logger.LogWarning("Failed to add TestEntity into classroom, but no exception came.");
                return false;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in AddQuestionAsync in TestQuestionManagementService.cs");
                return false;
            }
        }

        public async Task<bool> EditQuestionAsync(int? testPk, QuestionEntity oldQuestionEntity, QuestionViewModel questionData)
        {
            try
            {
                if (testPk == null)
                {
                    return false;
                }

                if (oldQuestionEntity.TestId != testPk)
                {
                    this._logger.LogWarning("Trying to edit questionEntity into other testEntity than declared");
                    return false;
                }

                oldQuestionEntity.CorrectNumber = oldQuestionEntity.CorrectNumber;

                oldQuestionEntity.CorrectShortText = oldQuestionEntity.CorrectShortText;

                oldQuestionEntity.Marks = oldQuestionEntity.Marks;

                oldQuestionEntity.InnerOrder = oldQuestionEntity.InnerOrder;

                oldQuestionEntity.QuestionText = oldQuestionEntity.QuestionText;

                oldQuestionEntity.Type = questionData.Type;

                int deletedOptions = await this._questionRepository.DeleteAllQuestionOptionsAsync(oldQuestionEntity.TestId);
                int addedOptions = await this._AddQuestionOptions(oldQuestionEntity, questionData.ChoiceOptions);

                if (deletedOptions != addedOptions)
                {
                    this._logger.LogInformation($"Changed question count on test {oldQuestionEntity.Id} in EditTestAsync: deleted {deletedOptions}, added {addedOptions}, balance: {-deletedOptions + addedOptions}");
                }

                return await this._questionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in EditQuestionAsync in TestQuestionManagementService.cs");
                return false;
            }
        }

        public async Task<bool> DeleteQuestionAsync(int? testPk, QuestionEntity questionEntity)
        {
            try
            {
                if (testPk == null)
                {
                    return false;
                }

                if (questionEntity.TestId == testPk)
                {
                    return await this._questionRepository.DeleteAsync(questionEntity);
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

        public async Task<List<QuestionEntity>> GetQuestionsByTestAsync(int? testPk)
        {
            try
            {
                if (testPk == null)
                {
                    return new List<QuestionEntity>();
                }

                return await this._questionRepository.GetAllQuestionsByTestAsync((int)testPk);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetTestListByClassroomAsync in TestManagementService.cs");
                return new List<QuestionEntity>();
            }
        }

        public async Task<QuestionEntity?> GetQuestionByTestAndIdAsync(int? testPk, int? questionPk)
        {
            try
            {
                if (testPk == null || questionPk == null)
                {
                    return null;
                }

                return await this._questionRepository.GetQuestionAsync(testPk.Value, questionPk.Value);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetTestByClassroomAndIdAsync in TestManagementService.cs");
                return null;
            }
        }
    }
}
