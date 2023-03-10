using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using TontonatorGameUI.Core.Data.BaseRepository;
using TontonatorGameUI.Core.Helpers;
using TontonatorGameUI.Models.Enums;
using static Grpc.Core.Metadata;

namespace TontonatorGameUI.Models
{
    [FirestoreData]
    public class Question : IQuestion, IEntityBase
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string QuestionName { get; set; }
        [FirestoreProperty]
        public QuestionCategory QuestionCategory { get; set; }
        [FirestoreProperty]
        public Status Status { get; set; }

        // Options disabled. Can be retaken later.
        public string[] QuestionOptions
        {
            get => new string[] {
            "Si",
            "No",
			//"Probablemente",
			//"Probablemente no",
			//"No sé"
		};
        }

        public bool IsCorrect { get; set; }

        [FirestoreProperty]
        public double QuestionRate { get; set; }
        [FirestoreProperty]
        public QuestionOption QuestionOption { get; set; }

        public Question()
        {

        }

        public Question(Question question)
        {
            Id = question.Id;
            QuestionName = question.QuestionName;
            QuestionCategory = question.QuestionCategory;
            Status = question.Status;
            IsCorrect = question.IsCorrect;
            QuestionRate = question.QuestionRate;
            QuestionOption = question.QuestionOption;
        }

        public Question(string questionName, QuestionCategory questionCategory, Status status)
        {
            QuestionName = questionName;
            QuestionCategory = questionCategory;
            IsCorrect = false;
            QuestionRate = 0;
            QuestionOption = QuestionOption.Null;
            Status = status;
        }

        public Question(string questionName, QuestionCategory questionCategory, QuestionOption questionOption, double questionRate, Status status)
        {
            QuestionName = questionName;
            QuestionCategory = questionCategory;
            QuestionOption = questionOption;
            QuestionRate = questionRate;
            Status = status;
        }

        public Question(string questionName, QuestionCategory questionCategory, QuestionOption questionOption, double questionRate)
        {
            QuestionName = questionName;
            QuestionCategory = questionCategory;
            QuestionOption = questionOption;
            QuestionRate = questionRate;
        }

        public void ShowOptions()
        {
            var counter = 1;
            foreach (var option in QuestionOptions)
            {
                Console.WriteLine(counter + ". " + option);
                counter++;
            }
        }

        public void EvaluateOption(string? option)
        {
            var opt = 0;

            if (!string.IsNullOrEmpty(option))
            {
                if (option.Length == 1)
                {
                    if (char.IsDigit(option[0]))
                    {
                        opt = int.Parse(option);

                        Console.Clear();

                        switch (opt)
                        {
                            case 1:
                                QuestionOption = QuestionOption.Si;
                                IsCorrect = true;
                                break;
                            case 2:
                                QuestionOption = QuestionOption.No;
                                IsCorrect = true;
                                break;
                            case 3: // Option disabled
                                QuestionOption = QuestionOption.Probablemente;
                                IsCorrect = true;
                                break;
                            case 4: // Option disabled
                                QuestionOption = QuestionOption.ProbablementeNo;
                                IsCorrect = true;
                                break;
                            case 5: // Option disabled
                                QuestionOption = QuestionOption.Nose;
                                IsCorrect = true;
                                break;
                            default:
                                MessageHelper.WriteError("ERROR: Ingrese un valor valido");
                                break;
                        }

                        EvaluateQuestion();
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

        private void EvaluateQuestion()
        {
            if (QuestionOption == QuestionOption.Si) QuestionRate = 1;
            else if (QuestionOption == QuestionOption.No) QuestionRate = 0;
            else if (QuestionOption == QuestionOption.Probablemente) QuestionRate = 0.75;
            else if (QuestionOption == QuestionOption.ProbablementeNo) QuestionRate = 0.25;
            else if (QuestionOption == QuestionOption.Nose) QuestionRate = 0.50;
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.Add("Id", Id);
            dictionary.Add("QuestionName", QuestionName);
            dictionary.Add("QuestionCategory", QuestionCategory);
            dictionary.Add("Status", Status);

            return dictionary;
        }

        public Dictionary<string, object> ToDictionaryComplete()
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.Add("Id", Id);
            dictionary.Add("QuestionName", QuestionName);
            dictionary.Add("QuestionCategory", QuestionCategory);
            dictionary.Add("QuestionRate", QuestionRate);
            dictionary.Add("QuestionOption", QuestionOption);
            dictionary.Add("Status", Status);

            return dictionary;
        }
    }
}