using System;
using System.Diagnostics.Metrics;
using Tontonator.Core.Helpers;
using Tontonator.Models;
using Tontonator.Models.Enums;

namespace Tontonator.Core
{
    public class States
    {

        /// <summary>
        /// This method shows the main menu. Should be called on App.Init() only.
        /// </summary>
        public static void ShowMainMenu()
        {
            var opt = 0;

            while (opt != 2)
            {
                if (Tontonator.Instance.IsActive)
                {
                    Console.WriteLine("===== Bienvenido a tontonator =====");
                    Console.WriteLine("Seleccione una opción");
                    Console.WriteLine("1. Jugar");
                    Console.WriteLine("2. Salir");

                    string? aux = Console.ReadLine();

                    if (!string.IsNullOrEmpty(aux))
                    {
                        if (aux.Length == 1)
                        {
                            if (char.IsDigit(aux[0]))
                            {
                                opt = int.Parse(aux);

                                Console.Clear();

                                switch (opt)
                                {
                                    case 1:
                                        Tontonator.Instance.Init();
                                        break;
                                    case 2:
                                        Console.WriteLine("Saliendo...");
                                        break;
                                    default:
                                        MessageHelper.WriteError("ERROR: Ingrese un valor valido");
                                        break;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                MessageHelper.WriteError("ERROR: Ingrese un valor númerico");
                            }

                        }
                        else
                        {
                            Console.Clear();
                            MessageHelper.WriteError("ERROR: Ingrese un valor valido");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        MessageHelper.WriteError("ERROR: El campo no puede estar vacio.");
                    }
                }
            }
        }

        /// <summary>
        /// This state shows the question and its options. It also waits for user's response and evaluate the question.
        /// </summary>
        /// <param name="question">The question to be shown.</param>
        /// <param name="index">This index is only for message purposes.</param>
        /// <returns>Returns a question with all its properties filled.</returns>
        public static Question ShowQuestion(Question question, int index)
        {
            if (Tontonator.Instance.IsActive)
            {
                if (IsQuestionReady(question))
                {
                    Tontonator.Instance.IncreaseCurrentIndex();
                    while (!question.IsCorrect)
                    {
                        Console.Clear();
                        Console.WriteLine(index++ + ". " + question.QuestionName);
                        question.ShowOptions();
                        var opt = Console.ReadLine();
                        question.EvaluateOption(opt);
                        Tontonator.Instance.ThinkOnCharacter(question);
                    }
                }
            }

            return question;
        }

        public static Question ShowQuestion(Question question)
        {
            if (Tontonator.Instance.IsActive)
            {
                if (IsQuestionReady(question))
                {
                    Tontonator.Instance.IncreaseCurrentIndex();
                    while (!question.IsCorrect)
                    {
                        Console.Clear();
                        Console.WriteLine(question.QuestionName);
                        question.ShowOptions();
                        var opt = Console.ReadLine();
                        question.EvaluateOption(opt);
                    }
                }
            }

            return question;
        }

        /// <summary>
        /// This shows the character, must pass a character as parameter.
        /// </summary>
        /// <param name="character">The character to display.</param>
        public static void ShowCharacter(Character character)
        {
            Console.WriteLine("Su personaje es: " + character.CharacterName);
            Console.WriteLine("1. Si");
            Console.WriteLine("2. No");

            var opt = Console.ReadLine();

            if (!string.IsNullOrEmpty(opt))
            {
                if (char.IsDigit(opt[0]))
                {
                    if (int.Parse(opt) == 1)
                    {
                        Tontonator.Instance.Dispose();
                        MessageHelper.WriteSuccess("He adivinado su personaje. Presione cualquier tecla para salir.");
                        Console.ReadKey();
                        App.Exit();
                    }
                    else if (int.Parse(opt) == 2)
                    {
                        if (Tontonator.Instance.CanRerol)
                        {
                            // Here does something
                        }
                        else
                        {
                            CreateNewCharacterMenu(Tontonator.Instance.QuestionsRequired);
                        }
                    }
                }
            }
        }

        public static void CreateNewCharacterMenu(bool questionsRequired)
        {
            List<Question> questions = new List<Question>();

            var characterName = "";
            var character = new Character();

            Console.WriteLine("No pude adivinar su personaje, ¿Desea añadirlo?");
            Console.WriteLine("1. Si");
            Console.WriteLine("2. No");

            var opt = Console.ReadLine();

            if (!string.IsNullOrEmpty(opt))
            {
                if (char.IsDigit(opt[0]))
                {
                    if (int.Parse(opt) == 1)
                    {
                        Tontonator.Instance.EnableDatabase();

                        if (questionsRequired)
                        {
                            if (Tontonator.Instance.GetAskedQuestions().Count > 0) questions = Tontonator.Instance.GetAskedQuestions();
                        }
                        else
                        {
                            questions = Tontonator.Instance.GetAskedQuestions();

                            for (int i = 0; i < 5; i++)
                            {
                                var question = FormQuestion();
                                var questionByName = Tontonator.Instance.GetQuestionByName(question.QuestionName);

                                if (string.IsNullOrEmpty(questionByName.Id)) question = Tontonator.Instance.AddQuestion(question);

                                if (question != null) questions.Add(question);
                            }

                            if(questions.Count > 0)
                            {
                                Console.WriteLine("Ingrese el nombre para su personaje: ");
                                characterName = Console.ReadLine();
                            }

                            if (questions.Count > 0 && !string.IsNullOrEmpty(characterName))
                            {
                                character = new Character(characterName, CharacterCategory.Unassigned, questions);
                                character = Tontonator.Instance.AddCharacter(character);
                            }

                            if (string.IsNullOrEmpty(character.Id)) Console.WriteLine("Su personaje se ha enviado para revisión.");
                        }
                    }
                    else if (int.Parse(opt) == 2) 
                    {
                        Tontonator.Instance.Dispose();
                        Console.Clear();
                        MessageHelper.WriteSuccess("Presiona cualquier tecla para terminar.");
                        Console.ReadKey();
                        App.Exit();
                    }
                }
            }
        }

        private static Question FormQuestion()
        {
            var question = new Question();

            Console.Clear();
            Console.WriteLine("Ingrese una pregunta");

            var questionTxt = Console.ReadLine();

            if (!string.IsNullOrEmpty(questionTxt))
            {
                //if (questionTxt.StartsWith('¿') && questionTxt.EndsWith('?'))
                //{
                    question = new Question(questionTxt, QuestionCategory.Character, Status.Disabled);
                    ShowQuestion(question);
                /*}
                else
                {
                    MessageHelper.WriteError("Intentalo de nuevo.");
                }*/
            }

            return question;
        }

        /// <summary>
        /// This method checks if  the question is ready to be showed. This means when doing a call to the database should not return a null object.
        /// So this method should be used first before checking the question.
        /// </summary>
        /// <param name="question">The question to be checked.</param>
        /// <returns>Returns either true if the question is not null or false if it is null.</returns>
        private static bool IsQuestionReady(Question question) => question != null ? true : false;   
    }
}