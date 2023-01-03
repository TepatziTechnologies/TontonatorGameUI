using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tontonator.Core.Data;
using Tontonator.Core.Services;
using Tontonator.Models;
using Tontonator.Models.Enums;

namespace Tontonator.Core
{
	public class Tontonator
	{
        #region VarDeclaration

        // To be used to connect with the database.
        // This var is the connection to the  character's service.
        private readonly CharactersService _charactersService;

        // This var is the connection to the  question's service.
        private readonly QuestionsService _questionsService;

		// This var must be used in a newer version. To be more exact.
		private double _average;

		// This var is used to show the current number of question. Does nothing else.
		private int currentIndex = 1;

		// This var is used to disable or enable the connection with the database
		// to avoid excesive calls to the database.
		public bool DATABASE_OFF;

		// This var is used when the state requires to know if it can rerol.
		public bool CanRerol { get; set; }

		// This var checks if the program is either active or inactive (disposed).
        public bool IsActive { get; set; }

        // This var checks if the state (Create new character) needs to ask the user more questions.
        public bool QuestionsRequired { get; set; }

		// This var stores the value to let know if there are questions to be asked.
		private bool alreadySet;

		// This var contains the current character.
		// Questions of Character.Questions are asked and compared with the response.
		private Character currentCharacter = new Character();

		// This var stores all the possible Characters.
		private List<Character> possibleCharacters;

		// Tis var stores all the next possible Characters.
		private List<Character> nextPossibleCharacters;

		// This var stores all the deleted Characters to be filtered on the next call.
		private List<Character> deletedCharacters;

		// This var stores all the questions that are going to be asked.
		public List<Question> questions;

		// This var stores all the questions that were already asked.
		private List<Question> checkedQuestions;

		// This var stores all the questions that are currently asked.
		private List<Question> liveQuestions;

		// This var stores all the questions that were already asked.
		private List<Question> alreadyAskedQuestions;

		// This var stores all the questions that were inherited from a Character.Questions.
		private List<Question> charactersInheritedQuestions;

        #endregion

        #region SingletonDeclaration
		// We create the instance of the same class.
        // his to create a singleton.
        private readonly static Tontonator _instance = new Tontonator();

		// A private constructor need to be a singleton class.
		private Tontonator()
		{
            // We enable the database.
            EnableDatabase();

            // We start the services to connect with the database.
            _charactersService = new CharactersService();
            _questionsService = new QuestionsService();

            questions = DataManager.GetBasicQuestions();

            // We initialize the question lists that we are going to use.
            checkedQuestions = new List<Question>();
            alreadyAskedQuestions = new List<Question>();
            liveQuestions = new List<Question>();
            charactersInheritedQuestions = new List<Question>();

            // We initialize the character lists that we are going to use.
            possibleCharacters = new List<Character>();
            nextPossibleCharacters = new List<Character>();
            deletedCharacters = new List<Character>();

            // This var is going to be used on a newer version.
            _average = 0d;

            // This var when init is set as true since it stores the current status of the program.
            // It is considered active because it is just initialized.
            IsActive = true;

            // This var when init is set as true because this class on method
            alreadySet = false;
        }

		/// <summary>
		/// This property is used to access to Tontonator instance since it is a Singleton.
		/// </summary>

		public static Tontonator Instance
		{
			get
			{
				return _instance;
			}
		}

        #endregion

		/// <summary>
        /// Initialize the game.
        /// </summary>
        public void Init()
		{
			// The way we initialize is doing a call to this method.
			// Since here starts to do all the process.
            ChangeQuestion();
		}

		/// <summary>
		/// This method prints the info of the given question as parameter and it thinks on the new character. Does most of things.
		/// </summary>
		/// <param name="currentQuestion">The question to be shown.</param>
		public void ThinkOnCharacter(Question currentQuestion)
		{
			// Method.
			// UpdateAvg();

			// We check if it has not been disposed.
			if (IsActive)
			{
				// We add the current questions, to the live sesion history.
				liveQuestions.Add(currentQuestion);

				// We add the current quesiton to the already asked question.
				// This because we consider it as completed.
				alreadyAskedQuestions.Add(currentQuestion);

				// Then we check if the question has been answered
				if (!checkedQuestions.Exists(n => n.Id == currentQuestion.Id)) this.checkedQuestions.Add(currentQuestion);

				// We make a call to retrieve more characters for each question.
				// This just because we consider every question changes everything.
				RetrieveMoreCharacters(currentQuestion);

				// We make a call to remove the already asked questions from questions list.
				RemoveAlreadyAskedQuestions();

				// We make a call to calculate all the possible characters.
				CalculatePossibleCharacters();

				// We check if there are any possible characters. If don't we consider this
				// as the end of the execution. We show the user the menu to create a new character.
				// Here stops this execution if no errors found.
				if (possibleCharacters.Count == 0) States.CreateNewCharacterMenu(QuestionsRequired);

				// We check if theres a character that matches the questions.
				CheckCharacter();

				// Either yes or not we make a call.
				// Because this takes or not the questions of the current character.
				ChangeQuestion();
			}
		}

		/// <summary>
        /// This methods updates the average number for every call.
        /// </summary>
        /// <remarks>
        /// This should not be used until working on the newer version.
        /// </remarks>
        [Obsolete]
		private void UpdateAvg()
		{
			double aux = 0d;
			foreach (var question in questions)
				aux += question.QuestionRate;

			aux = aux / questions.Count;
			_average = aux;
		}

		/// <summary>
        /// Method to retrieve more characters based on the question.
        /// </summary>
        /// <param name="currentQuestion">To be used on the query</param>
		private void RetrieveMoreCharacters(Question currentQuestion)
		{
			// We put all the characters as possible.
			nextPossibleCharacters = _charactersService.ReadByQuestion(currentQuestion);

			// We make a call to this method.
			SetCurrentCharacter();
		}

		/// <summary>
        /// This method changes the question.
        /// </summary>
		private void ChangeQuestion()
		{
			// We check if the program is not disposed.
			if (IsActive)
			{
				// If there are any question's character then we show those first.
				if (charactersInheritedQuestions.Count > 0) States.ShowQuestion(charactersInheritedQuestions[0], currentIndex);
				// If there are questions reamining then we ask it.
				else if (questions.Count > 0) States.ShowQuestion(questions[0], currentIndex);
				// If there's nothing left then that means no character matched. We asked the user for it.
				else if (questions.Count == 0) States.CreateNewCharacterMenu(QuestionsRequired);
				//else if (questions.Count == 0) FillQuestions();
			}
        }

		/// <summary>
        /// This method checks if theres a character that matches the asked questions.
        /// </summary>
		private void CheckCharacter()
		{
			// This var stores the total of matches of asked question and character's questions.
			var hits = 0;

			// This var stores the total of not matching answers.
			var breaks = 0;

			// This var stores the reaiming number of questions.
			var remaining = 0;

			// This var stores the total number of matching questions between the list and the character.
			var count = 0;

			// This iteration checks every question that is currently be asked. 
			foreach (var question in liveQuestions)
			{
				// It checks if there are any question that matches the same name as the current iteration.
				if (currentCharacter.Questions.Find(val => val.QuestionName == question.QuestionName) != null) count++;
            }

			// We calculate the reaiming questions to be checked.
			remaining = currentCharacter.Questions.Count - count;

            // This iteration checks every question that is currently be asked. 
            foreach (var question in liveQuestions)
			{
				// Now we iterate on the current character's questions.
				foreach (var cQuestion in currentCharacter.Questions)
				{
					// If there's a match on the question and the answer we add 1 to var hits.
					if (question.QuestionName == cQuestion.QuestionName && question.QuestionOption == cQuestion.QuestionOption) hits++;
					// If not then we mark it as a break.
					else if (question.QuestionName == cQuestion.QuestionName && question.QuestionOption != cQuestion.QuestionOption) breaks++;
				}
			}

			// We check if there are any remaining questions and if there were any breaks.
			// If so, then that means it won't match. So
			// For the next state we are going to disable the database first.
			if (remaining > 0 && breaks == 0) DisableDatabase();
			// Otherwise, we keep the database enabled.
			else if (DATABASE_OFF && breaks > 0) EnableDatabase();

			// If there are more than one break, we delete this character.
			if (breaks > 0)
			{
				// We add this character to the deleted list.
				deletedCharacters.Add(currentCharacter);

				// We clear the inherited questions.
				charactersInheritedQuestions.Clear();

				possibleCharacters.RemoveAll(e => e.Id == currentCharacter.Id);

                SetCurrentCharacter();
			}

			// We check if the amount of hits is the same as the amount of the character's question.
			if (hits == currentCharacter.Questions.Count)
			{
				// If so we asked if it is the current character.
				States.ShowCharacter(currentCharacter);
			}
			else
			{


			}
		}

		// This method changes the character.
		private void ChangeCharacter(){
			// We clear the inherited questions if there were any previously.
			charactersInheritedQuestions.Clear();

			// This var stores the max value that is equal to Questions.Count.
			var max = 0;

			// We iterate on our possible characters.
            foreach (var character in nextPossibleCharacters)
            {
				// We set the max value as the character's questions count.
				max = character.Questions.Count;

				// We iterate on our character's questions
                foreach (var question in character.Questions)
                {
                    foreach (var questionn in liveQuestions)
                    {

                    }
                }
            }
		}

		/// <summary>
        /// This method sets a new current character to work with.
        /// </summary>
		private void SetCurrentCharacter()
		{
			// We check for the hits.
			var hits = 0;

			// If there is any possible character on the list.
			if (nextPossibleCharacters.Count > 0)
			{
				// We iterate on them.
				foreach (var character in nextPossibleCharacters)
				{
					// We check every current question with the character questions.
					foreach (var question in liveQuestions)
					{
						// If it exists then we increment hits by 1.
						if (character.Questions.Exists(q => q.QuestionName == question.QuestionName)) hits++;

						// If hits are equal to th
						if (hits == liveQuestions.Count)
						{
							// We set the character that has more hits as current.
							currentCharacter = character;

							// We call this method to set the questions.
							SetCharacterQuestions(currentCharacter);

							// We disable the database to avoid excesive calls to the database.
							DisableDatabase();

							// We set this value as true to let the program know it has questions to ask.
							alreadySet = true;
						}
                        else
                        {
							// If it is false.
                            if(!alreadySet)
                            {
								// We check if any possible character exist.
								if (possibleCharacters.Count > 0)
								{
									// If it exist then we set the position 0 as the current.
									currentCharacter = possibleCharacters[0];

                                    // We disable the database to avoid excesive calls to the database.
                                    DisableDatabase();

                                    // We call this method to set the questions.
                                    SetCharacterQuestions(currentCharacter);
								}
                            }
                        }
					}
				}



				alreadySet = false;
			}

            // If there is any possible character on the list.
            if (possibleCharacters.Count > 0)
            {
                // We iterate on them.
                foreach (var character in possibleCharacters)
                {
                    // We check every current question with the character questions.
                    foreach (var question in liveQuestions)
                    {
                        // If it exists then we increment hits by 1.
                        if (character.Questions.Exists(q => q.QuestionName == question.QuestionName)) hits++;

                        // If hits are equal to th
                        if (hits == liveQuestions.Count)
                        {
                            // We set the character that has more hits as current.
                            currentCharacter = character;

                            // We call this method to set the questions.
                            SetCharacterQuestions(currentCharacter);

                            // We disable the database to avoid excesive calls to the database.
                            DisableDatabase();

                            // We set this value as true to let the program know it has questions to ask.
                            alreadySet = true;
                        }
                        else
                        {
                            // If it is false.
                            if (!alreadySet)
                            {
                                // We check if any possible character exist.
                                if (possibleCharacters.Count > 0)
                                {
                                    // If it exist then we set the position 0 as the current.
                                    currentCharacter = possibleCharacters[0];

                                    // We disable the database to avoid excesive calls to the database.
                                    DisableDatabase();

                                    // We call this method to set the questions.
                                    SetCharacterQuestions(currentCharacter);
                                }
                            }
                        }
                    }
                }

                alreadySet = false;
            }
        }

		/// <summary>
        /// Method to calculate all the possible characters.
        /// </summary>
		private void CalculatePossibleCharacters()
        {
			// We init a new list of characters.
			var characters = new List<Character>();

			// We iterate the current asked questions.
            foreach (var question in liveQuestions)
            {
				// We set the list of characters.
                characters = nextPossibleCharacters.FindAll(n => n.Questions.FindAll(q => q.Id == question.Id).Count > 0);
            }

			// If there are more than 0 character.
			if (characters.Count > 0)
			{
				// We set the characters.
				this.possibleCharacters = characters;

				// We clean the history.
				nextPossibleCharacters.Clear();
			}
        }

		/// <summary>
        /// Method to remove the already asked questions.
        /// </summary>
		private void RemoveAlreadyAskedQuestions()
		{
            // We iterate the asked questions.
            foreach (var question in alreadyAskedQuestions)
			{
				// If there are any question processed that match the id saved on characters data.
				if (questions.Exists(q => q.Id == question.Id)) questions.RemoveAll(qq => qq.Id == question.Id);

				// If there are any questions that were inherited we remove the one that were asked.
				if (charactersInheritedQuestions.Exists(q => q.Id == question.Id)) charactersInheritedQuestions.RemoveAll(qq => qq.Id == question.Id);
			}
        }

		/// <summary>
        /// Method to check if there are any duplicated questions.
        /// </summary>
		private void CheckDuplicatedQuestions()
        {
			// If there is any duplicated question on the same list it removes them.
            foreach (var question in questions) if (questions.Exists(q => q.Id == question.Id)) questions.RemoveAll(q => q.Id == question.Id);
        }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
		private void SetCharacterQuestions(Character character)
		{
            // We init a new list of characters.
            var list = new List<Question>();

            // We iterate on the character questions.
            foreach (var question in character.Questions)
            {
				// We add them to the list.
				list.Add(new Question(question));
            }

            foreach (var question in alreadyAskedQuestions)
            {
                if (list.Exists(q => q.Id == question.Id)) list.RemoveAll(q => q.Id == question.Id);
            }

            // We set the current list on this var.
            charactersInheritedQuestions = list;
		}

		/// <summary>
        /// This method disposes the program.
        /// </summary>
		public void Dispose	() => IsActive = false;

		/// <summary>
        /// Method to increase the current index that will be shown when asking a question.
        /// </summary>
        public void IncreaseCurrentIndex() => currentIndex++;

		/// <summary>
        /// Method to disable the database.
        /// </summary>
        public void DisableDatabase() => DATABASE_OFF = true;

		/// <summary>
        /// Method to enable the database.
        /// </summary>
        public void EnableDatabase() => DATABASE_OFF = false;

		/// <summary>
        /// This method adds a new question to the database.
        /// </summary>
        /// <param name="question">Question to be added.</param>
        /// <returns>Question with some fields added.</returns>
        public Question AddQuestion(Question question) => _questionsService.Add(question);

		/// <summary>
        /// Method to get a question by a name provided.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>Question if found.</returns>
        public Question GetQuestionByName(string name) => _questionsService.Read(nameof(Question.QuestionName), name);

		/// <summary>
        /// Method to add a character to the database.
        /// </summary>
        /// <param name="character">Character to be added</param>
        /// <returns>The character.</returns>
		public Character AddCharacter(Character character) => _charactersService.AddCharacter(character);

        /// <summary>
        /// Method to return the var alreadyAskedQuestions. <seealso cref="alreadyAskedQuestions"/>
        /// </summary>
        /// <returns>List of questions.</returns>
        public List<Question> GetAskedQuestions() => alreadyAskedQuestions;
    }
}