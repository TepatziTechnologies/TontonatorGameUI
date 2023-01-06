using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TontonatorGameUI.Commands;
using TontonatorGameUI.Core;
using TontonatorGameUI.Models;
using TontonatorGameUI.Models.Enums;

namespace TontonatorGameUI.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public Visibility StartVisible { get; set; }
		public Visibility QuestionVisible { get; set; }
		public Visibility IsItCharacterVisible { get; set; }
		public Visibility NewCharacterVisible { get; set; }
		public Visibility QuestionFormVisible { get; set; }
		public Question Question { get; set; }
		public Character Character { get; set; }
		private int QuestionIndex { get; set; }
		public string FirstQuestion { get; set; }
		public string SecondQuestion { get; set; }
		public string ThirdQuestion { get; set; }
		public string FourthQuestion { get; set; }
		public string FifthQuestion { get; set; }
		public string UserCharacerName { get; set; }
		public bool FirstQuestionBool { get; set; }
		public bool SecondQuestionBool { get; set; }
		public bool ThirdQuestionBool { get; set; }
		public bool FourthQuestionBool { get; set; }
		public bool FifthQuestionBool { get; set; }
		public ICommand StartCommand { get; set; }
		public ICommand PositiveCommand { get; set; }
		public ICommand NegativeCommand { get; set; }
		public ICommand SaveCharacterCommand { get; set; }
		public ICommand QuitCommand { get; set; }

		public string QuestionText { 
			get {
				if (Question == null) return "";
				else return (QuestionIndex + 1).ToString() + ". " + Question.QuestionName;
			}
		}

		public string CharacterName
		{
			get {
				if (Character == null) return "";
				else return "Su personaje es: " + Character.CharacterName;
			}
		}

		public MainWindowViewModel()
		{
			StartVisible = Visibility.Visible;
			QuestionVisible = Visibility.Hidden;
			IsItCharacterVisible = Visibility.Hidden;
			NewCharacterVisible = Visibility.Hidden;
			QuestionFormVisible = Visibility.Hidden;
			UserCharacerName = "";
			FirstQuestion = "";
			SecondQuestion = "";
			ThirdQuestion = "";
			FourthQuestion = "";
			FifthQuestion = "";
			FirstQuestionBool = false;
			SecondQuestionBool = false;
			ThirdQuestionBool = false;
			FourthQuestionBool = false;
			FifthQuestionBool = false;
			StartCommand = new RelayCommand(new Action<object>(StartAction));
			PositiveCommand = new RelayCommand(new Action<object>(PositiveAction));
			NegativeCommand = new RelayCommand(new Action<object>(NegativeAction));
			SaveCharacterCommand = new RelayCommand(new Action<object>(SaveForm));
			QuitCommand = new RelayCommand(new Action<object>(QuitWindow));
		}

		private void QuitWindow(object obj)
		{
			Application.Current.Shutdown();
		}

		private void StartAction(object obj)
		{
			Tontonator.Instance.Init(this);
			StartVisible = Visibility.Hidden;
			QuestionVisible = Visibility.Visible;
			OnPropertyChanged("StartVisible");
			OnPropertyChanged("QuestionVisible");
		}

		private void PositiveAction(object obj)
		{
			var identifier = int.Parse(obj as string);

			if (identifier == 1)
			{
				Question.QuestionOption = QuestionOption.Si;
				Tontonator.Instance.ThinkOnCharacter(this.Question);
			}
			else if (identifier == 2)
			{
				Tontonator.Instance.Dispose();
			}
			else if (identifier == 3)
			{
				Tontonator.Instance.EnableDatabase();
				NewCharacterVisible = Visibility.Hidden;
				OnPropertyChanged("NewCharacterVisible");
				QuestionFormVisible = Visibility.Visible;
				OnPropertyChanged("QuestionFormVisible");
			}
		}

		private void NegativeAction(object obj)
		{
			var identifier = int.Parse(obj as string);

			if (identifier == 1)
			{
				Question.QuestionOption = QuestionOption.No;
				Tontonator.Instance.ThinkOnCharacter(this.Question);
			}
			else if (identifier == 2)
			{
				if (Tontonator.Instance.CanRerol)
				{
					// Here does something
				}
				else
				{
					NewCharacterVisible = Visibility.Visible;
					StartVisible = Visibility.Hidden;
					QuestionVisible = Visibility.Hidden;
					IsItCharacterVisible = Visibility.Hidden;
					OnPropertyChanged("NewCharacterVisible");
					OnPropertyChanged("StartVisible");
					OnPropertyChanged("QuestionVisible");
					OnPropertyChanged("IsItCharacterVisible");
					//CreateNewCharacterMenu(Tontonator.Instance.QuestionsRequired);
				}
			}
			else if (identifier == 3)
			{
				QuitWindow(0);
			}
		}

		public void CallNewCharacter()
		{
			NewCharacterVisible = Visibility.Visible;
			StartVisible = Visibility.Hidden;
			QuestionVisible = Visibility.Hidden;
			IsItCharacterVisible = Visibility.Hidden;
			OnPropertyChanged("NewCharacterVisible");
			OnPropertyChanged("StartVisible");
			OnPropertyChanged("QuestionVisible");
			OnPropertyChanged("IsItCharacterVisible");
		}

		public void SetQuestion(Question question, int index)
		{
			Tontonator.Instance.IncreaseCurrentIndex();
			QuestionIndex = index;
			this.Question = question;
			OnPropertyChanged("QuestionText");
		}

		public void SetCharacter(Character character)
		{
			this.Character = character;
			IsItCharacterVisible = Visibility.Visible;
			QuestionVisible = Visibility.Hidden;
			OnPropertyChanged("QuestionVisible");
			OnPropertyChanged("IsItCharacterVisible");
			OnPropertyChanged("CharacterName");
		}

		private void SaveForm(object obj)
		{
			var questions = Tontonator.Instance.GetAskedQuestions();
			var character = new Character();

			var question1 = new Question(FirstQuestion, QuestionCategory.Character, Status.Disabled);
			var question2 = new Question(SecondQuestion, QuestionCategory.Character, Status.Disabled);
			var question3 = new Question(ThirdQuestion, QuestionCategory.Character, Status.Disabled);
			var question4 = new Question(FourthQuestion, QuestionCategory.Character, Status.Disabled);
			var question5 = new Question(FifthQuestion, QuestionCategory.Character, Status.Disabled);

			var localQuestions = new List<Question>() { question1, question2, question3, question4, question5 };

			for (int i = 0; i < 5; i++)
			{
				var questionByName = Tontonator.Instance.GetQuestionByName(localQuestions[i].QuestionName);

				if (string.IsNullOrEmpty(questionByName.Id)) localQuestions[i] = Tontonator.Instance.AddQuestion(localQuestions[i]);

				if (localQuestions[i] != null) questions.Add(localQuestions[i]);
				
			}

			if (questions.Count > 0 && !string.IsNullOrEmpty(UserCharacerName))
			{
				character = new Character(UserCharacerName, CharacterCategory.Unassigned, questions);
				character = Tontonator.Instance.AddCharacter(character);
			}

			if (string.IsNullOrEmpty(character.Id)) Console.WriteLine("Su personaje se ha enviado para revisión.");
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
