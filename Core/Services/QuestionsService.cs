using System;
using TontonatorGameUI.Core.Data.BaseRepository;
using TontonatorGameUI.Models;

namespace TontonatorGameUI.Core.Services
{
    public class QuestionsService : EntityBaseRepository<Question>
    {
        public QuestionsService() : base("questions")
        {

        }
    }
}

