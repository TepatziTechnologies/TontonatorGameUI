using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TontonatorGameUI.Core.Data.BaseRepository;
using TontonatorGameUI.Models.Enums;

namespace TontonatorGameUI.Models
{
    public class Character : IEntityBase
    {
        public string Id { get; set; }
        public string CharacterName { get; set; }
        public CharacterCategory CharacterCategory { get; set; }
        public List<Question> Questions { get; set; }
        public string[] IdQuestions { get; set; }

        public Character()
        {

        }

        public Character(string characterName, CharacterCategory characterCategory, List<Question> questions)
        {
            CharacterName = characterName;
            CharacterCategory = characterCategory;
            Questions = questions;
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.Add("Id", Id);
            dictionary.Add("CharacterName", CharacterName);
            dictionary.Add("CharacterCategory", CharacterCategory);
            dictionary.Add("IdQuestions", QuestionsToArray());

            return dictionary;
        }

        /// <summary>
        /// This method should be used only when storing data into the database,
        /// </summary>
        /// <returns></returns>
        private string[] QuestionsToArray()
        {
            string[] questions = new string[Questions.Count()];

            var counter = 0;

            foreach (var question in Questions)
            {
                questions[counter] = question.Id;
                counter++;
            }

            return questions;
        }
    }
}