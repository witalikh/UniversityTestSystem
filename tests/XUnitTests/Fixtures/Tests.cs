using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;

namespace XUnitTests.Fixtures
{
    public static class Tests
    {
        public static TestViewModel CapitalsTest = new TestViewModel
        {
            AttemptsAllowed = 5,
            Questions = new List<QuestionViewModel>
            {
                new ()
                {
                    QuestionText = "Capital of Poland",
                    Type = QuestionType.ShortText,
                    CorrectShortText = "Warsaw"
                },
                new ()
                {
                    QuestionText = "What is the capital of France?",
                    Type = QuestionType.SingleChoice,
                    ChoiceOptions = new List<QuestionChoiceOptionViewModel>
                    {
                        new QuestionChoiceOptionViewModel()
                        {
                            InnerOrder = 1,
                            IsCorrect = false,
                            OptionText = "Amsterdam",
                        },
                        new QuestionChoiceOptionViewModel()
                        {
                            InnerOrder = 2,
                            IsCorrect = false,
                            OptionText = "Boryslav",
                        },
                        new QuestionChoiceOptionViewModel()
                        {
                            InnerOrder = 3,
                            IsCorrect = true,
                            OptionText = "Paris",
                        },
                        new QuestionChoiceOptionViewModel()
                        {
                            InnerOrder = 4,
                            IsCorrect = false,
                            OptionText = "Berlin",
                        }
                    }
                }
            }
        };
    }
}
