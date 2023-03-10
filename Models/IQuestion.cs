using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TontonatorGameUI.Models.Enums;

namespace TontonatorGameUI.Models
{
    public interface IQuestion
    {
        public string QuestionName { get; set; }
        public string[] QuestionOptions { get; }
        public Status Status { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
        public QuestionOption QuestionOption { get; set; }
    }
}