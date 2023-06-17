using BusinessLayer.ViewModels;
using DataAccessLayer.Models;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITestAttemptManagementService
    {
        public Task<AttemptViewModel?> StartAttemptAsync(string userPk, int testPk);

        public Task<bool> EndAttemptAsync(int testPk, Guid attemptPk, AttemptViewModel attemptData);

        public Task<List<AttemptEntity>> GetAttemptsByTest(int testPk);

        public Task<List<AttemptEntity>> GetAttemptsByTestAndUser(int testPk, string userPk);


        //public Task<bool> DeleteTestAsync(int? classroomPk, TestEntity testEntity);

        //public Task<bool> DeleteQuestionAsync(int? testPk, QuestionEntity questionEntity);

        //public Task<List<TestEntity>> GetTestListByClassroomAsync(int? classroomPk);

        //public Task<List<QuestionEntity>> GetQuestionsByTestAsync(int? testPk);

        //public Task<TestEntity?> GetTestByClassroomAndIdAsync(int? classroomPk, int? testPk);

        //public Task<QuestionEntity?> GetQuestionByTestAndIdAsync(int? testPK, int? questionPk);

    }
}
