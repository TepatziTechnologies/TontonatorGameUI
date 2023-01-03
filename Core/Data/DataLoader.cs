using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tontonator.Core.Services;
using Tontonator.Models;
using Tontonator.Models.Enums;

namespace Tontonator.Core.Data
{
	internal class DataLoader
	{
		public static List<Character> characters = new List<Character>();

		public static void InitCharacters()
		{
			characters.Add(new Character("El que siempre dice que si", CharacterCategory.Unassigned, new List<Question>() {
				new Question("¿Su personaje es real?",QuestionCategory.Basic, QuestionOption.Si, 1),
				new Question("¿Su personaje es hombre?", QuestionCategory.Basic, QuestionOption.Si, 1),
                new Question("¿Su personaje es un youtuber famoso?", QuestionCategory.Character, QuestionOption.Si, 1),
                new Question("¿Su personaje vive en mexico?", QuestionCategory.Character, QuestionOption.Si, 1),
				new Question("¿Su personaje habla ingles?", QuestionCategory.Character, QuestionOption.Si, 1),
				new Question("¿Su personaje tiene 3 ojos?", QuestionCategory.Character, QuestionOption.Si, 1),
				new Question("¿Su personaje vive en corea?", QuestionCategory.Character, QuestionOption.Si, 1),
				new Question("¿Su personaje nacio en guatemala?", QuestionCategory.Character, QuestionOption.Si, 1)
			}));
		}

		public static void FeedDatabaseQuestions()
		{
			var db = new QuestionsService();
			var questions = GetQuestions();

			foreach (var question in questions) db.Add(question);
		}

		public static void FeedDatabaseCharacters()
		{
			InitCharacters();
			var db = new CharactersService();
			foreach (var character in characters) db.AddCharacter(character);
		}

		public static List<Question> GetQuestions()
		{
			var questions = new List<Question>();

			questions.Add(new Question("¿Su personaje es real?", QuestionCategory.Basic, Status.Enabled));
			questions.Add(new Question("¿Su personaje es hombre?", QuestionCategory.Basic, Status.Enabled));
			questions.Add(new Question("¿Su personaje es un youtuber famoso?", QuestionCategory.Basic, Status.Enabled));
			questions.Add(new Question("¿Su personaje es un cantante?", QuestionCategory.Basic, Status.Enabled));

			return questions;
		}

	}
}
