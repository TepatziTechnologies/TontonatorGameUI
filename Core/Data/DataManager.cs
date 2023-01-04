using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TontonatorGameUI.Core.Services;
using TontonatorGameUI.Models;
using TontonatorGameUI.Models.Enums;

namespace TontonatorGameUI.Core.Data
{
    internal class DataManager
    {
        private static QuestionsService _questionsService = new QuestionsService();

        public static List<Question> GetBasicQuestions() => _questionsService.ReadAll(nameof(Question.QuestionCategory), QuestionCategory.Basic);

    }
}
