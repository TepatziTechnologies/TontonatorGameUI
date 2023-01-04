using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TontonatorGameUI.Actions;
using TontonatorGameUI.Helpers;
using TontonatorGameUI.Core;
using TontonatorGameUI.Views;
using System.Threading;

namespace TontonatorGameUI.ViewModels
{
    public class LoadingViewModel 
    {
        private DelegateLoadedAction _loadAction = null;
        public DelegateLoadedAction LoadAction
        {
            get
            {
                return _loadAction ?? (_loadAction = new DelegateLoadedAction( async () =>
                {
                    if (InternetConnectionHelper.CheckInternetConnection())
                    {
						//Tontonator.Instance.Init();

						await Task.Delay(5000);

						var mainWindow = new Views.MainWindow();
						
						Application.Current.Windows[0].Close();

						mainWindow.Show();
						
						
                    }
                    else
                    {
                        var result = MessageBox.Show("No tienes conexión a internet.", "No hay internet.", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (result == MessageBoxResult.OK) Application.Current.Shutdown();
                    }
                }));
            }
        }
    }
}
