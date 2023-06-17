using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;
using Npgsql.Internal.TypeHandlers;

namespace BusinessLayer.Services.Implementations
{
    public class TestAttemptManagementService: ITestAttemptManagementService
    {
        private readonly IClassroomRepository _classroomRepository;

        private readonly ITestRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionChosenOptionRepository _optionRepository;


        private readonly IAttemptRepository _attemptRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IAnswerChosenOptionRepository _answerChosenOptionRepository;

        private readonly IUserRepository _userRepository;
        private ILogger<TestAttemptManagementService> _logger;

        public TestAttemptManagementService(
            IClassroomRepository classroomRepository,
            
            ITestRepository testRepository,
            IQuestionRepository questionRepository,
            IQuestionChosenOptionRepository optionRepository,

            IAttemptRepository attemptRepository,
            IAnswerRepository answerRepository,
            IAnswerChosenOptionRepository answerChosenOptionRepository,

            IUserRepository userRepository,
            ILogger<TestAttemptManagementService> logger)
        {
            
            this._classroomRepository = classroomRepository;

            this._testRepository = testRepository;
            this._questionRepository = questionRepository;
            this._optionRepository = optionRepository;
            this._attemptRepository = attemptRepository;
            this._answerRepository = answerRepository;
            this._answerChosenOptionRepository = answerChosenOptionRepository;
            this._userRepository = userRepository;
            this._logger = logger;
        }

        public static int EvaluateAnswer(AnswerViewModel data, QuestionEntity question)
        {
            if (question.Type == QuestionType.ShortText)
            {
                return (data.ShortTextValue!.ToLower().Trim() == question.CorrectShortText!.ToLower().Trim())
                    ? question.Marks
                    : 0;
            }
            else if (question.Type == QuestionType.NumericalInput)
            {
                return (data.NumericValue! == question.CorrectNumber!)
                    ? question.Marks
                    : 0;
            }
            else if (question.Type is QuestionType.MultipleChoice or QuestionType.SingleChoice)
            {
                var chosenAnswerOptions = data.ChosenOptions;
                var allOptions = question.ChoiceOptions;
                var correctOptions = allOptions.Where(t => t.IsCorrect);


                int correctlyChosenAnswers = chosenAnswerOptions!.Count(t => correctOptions.Any(tt => tt.Id == t.ChoiceOptionId));
                int wrongChosenOptions = chosenAnswerOptions!.Count - correctlyChosenAnswers;

                int sum = correctlyChosenAnswers - wrongChosenOptions;
                var questionChoiceOptionEntities = correctOptions.ToList();

                if (!questionChoiceOptionEntities!.Any())
                {
                    return (wrongChosenOptions > 0) ? 0 : question.Marks;
                }
                else
                {
                    return (sum > 0) ? (int) Math.Round((double)sum / (double) questionChoiceOptionEntities!.Count() * question.Marks, MidpointRounding.AwayFromZero) : 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public async Task<AttemptViewModel?> StartAttemptAsync(string userPk, int testPk)
        {
            DateTime now = DateTime.UtcNow;
            
            TestEntity? test = await this._testRepository.GetAsync(testPk);
            if (test == null)
            {
                return null;
            }

            if (test.EndDateTime < now)
            {
                return null;
            }

            int userAttempts = await this._attemptRepository.GetUserAttemptsCount(testPk, userPk);
            if (test.AttemptsAllowed is not (null or 0))
            {
                
                if (userAttempts >= test.AttemptsAllowed)
                {
                    return null;
                }
            }

            MembershipEntity? member = await this._userRepository.GetMemberAsync(userPk, test.ClassroomId);

            if (member == null)
            {
                return null;
            }

            AttemptEntity attemptEntity = new AttemptEntity()
            {
                AttemptNumber = userAttempts,
                DateTimeStarted = now,
                DateTimeEnded = null,
                Grade = 0,
                Member = member,
                TestEntity = test
            };

            bool added = await this._attemptRepository.AddAsync(attemptEntity);
            if (added) 
                return new AttemptViewModel(attemptEntity);

            this._logger.LogError("All conditions for attempt are met, but AttemptEntity was not saved, for unknown reason");
            return null;

        }

        public async Task<bool> EndAttemptAsync(int testPk, Guid attemptPk, AttemptViewModel attemptData)
        {
            DateTime now = DateTime.UtcNow;

            // test doesn't exist
            TestEntity? test = await this._testRepository.GetAsync(testPk);
            if (test == null)
            {
                return false;
            }

            // test deadline passed
            if (test.EndDateTime < now)
            {
                return false;
            }

            AttemptEntity? attempt = await this._attemptRepository.GetAsync(attemptPk);

            // attempt doesn't exist
            if (attempt == null)
            {
                return false;
            }

            // attempt is already finished
            if (attempt.DateTimeEnded is not null)
            {
                return false;
            }

            // attempt timeout
            if (test.DurationSeconds is not (null or 0) &&
                attempt.DateTimeStarted + TimeSpan.FromSeconds((double)test.DurationSeconds!) < now)
            {
                return false;
            }

            // if all checks are ok, save
            attempt.DateTimeEnded = now;

            // EVALUATE ANSWERS !!!
            var questions = await this._questionRepository.GetAllQuestionsByTestAsync(testPk);

            double percentages = 0.0;
            int validQuestions = 0;
            foreach (var answerData in attemptData.Answers)
            {
                answerData.AttemptUuid = attemptPk;
                var question = questions.Find(x => x.Id == answerData.QuestionId);
                if (question == null)
                {
                    continue;
                }

                answerData.Marks = EvaluateAnswer(answerData, question);
                percentages += (double)answerData.Marks / (double)question.Marks;
                ++validQuestions;

                attempt.Answers.Add(AnswerViewModel.ToEntity(answerData));
            }

            attempt.Grade = (int)Math.Round((percentages / validQuestions) * test.TotalGrade);
            return await this._attemptRepository.SaveChangesAsync();
        }

        public async Task<List<AttemptEntity>> GetAttemptsByTest(int testPk)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AttemptEntity>> GetAttemptsByTestAndUser(int testPk, string userPk)
        {
            throw new NotImplementedException();
        }
    }
}
