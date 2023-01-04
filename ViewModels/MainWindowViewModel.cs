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

namespace TontonatorGameUI.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public Visibility StartVisible { get; set; }
		public Visibility QuestionVisible { get; set; }
		public Question Question { get; set; }
		private int QuestionIndex { get; set; }
		public ICommand StartCommand { get; set; }
		public ICommand PositiveButton { get; set; }
		public ICommand NegativeButton { get; set; }
		public string QuestionText { 
			get {
				if (Question == null)
				{
					return "";
				}
				else
				{
					return QuestionIndex.ToString() + ". " + Question.QuestionName;
				}
			} 
		}

		public MainWindowViewModel()
		{
			StartVisible = Visibility.Visible;
			QuestionVisible = Visibility.Hidden;
			StartCommand = new RelayCommand(new Action<object>(StartAction));
		}

		private void StartAction(object obj)
		{
			Tontonator.Instance.Init(this);
			StartVisible = Visibility.Hidden;
			QuestionVisible = Visibility.Visible;
			OnPropertyChanged("StartVisible");
			OnPropertyChanged("QuestionVisible");
		}

		public void SetQuestion(Question question, int index)
		{
			this.Question = question;
			OnPropertyChanged("QuestionText");
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
