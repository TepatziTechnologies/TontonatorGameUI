using System;
using Tontonator.Core.Data.BaseRepository;
using Tontonator.Models;

namespace Tontonator.Core.Services
{
    public class QuestionsService : EntityBaseRepository<Question>
    {
        public QuestionsService() : base("questions")
        {
            
        }
    }
}

