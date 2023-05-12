using BusinessLayer.ViewModels;
using DataAccessLayer.Models;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITestManagementService {

        public Task<bool> AddTestAsync(int? classroomPk, TestViewModel testData);

        public Task<bool> AddQuestionAsync(int? testPk, QuestionViewModel questionData);

        public Task<bool> EditTestAsync(int? classroomPk, TestEntity oldTestEntity, TestViewModel testData);

        public Task<bool> EditQuestionAsync(int? testPk, QuestionEntity oldQuestionEntity, QuestionViewModel questionData);

        public Task<bool> DeleteTestAsync(int? classroomPk, TestEntity testEntity);

        public Task<bool> DeleteQuestionAsync(int? testPk, QuestionEntity questionEntity);

        public Task<List<TestEntity>> GetTestListByClassroomAsync(int? classroomPk);

        public Task<List<QuestionEntity>> GetQuestionsByTestAsync(int? testPk);

        public Task<TestEntity?> GetTestByClassroomAndIdAsync(int? classroomPk, int? testPk);

        public Task<QuestionEntity?> GetQuestionByTestAndIdAsync(int? testPK, int? questionPk);

    }
}
