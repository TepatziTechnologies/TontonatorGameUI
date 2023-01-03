using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TontonatorGameUI.Actions
{
    public class LoadedBindings
    {
        public static readonly DependencyProperty LoadedEnabledProperty =
            DependencyProperty.RegisterAttached(
                "LoadedEnabled",
                typeof(bool),
                typeof(LoadedBindings),
                new PropertyMetadata(false, new PropertyChangedCallback(OnLoadedEnabledPropertyChanged)));

        public static bool GetLoadedEnabled(DependencyObject sender) => (bool)sender.GetValue(LoadedEnabledProperty);
        public static void SetLoadedEnabled(DependencyObject sender, bool value) => sender.SetValue(LoadedEnabledProperty, value);

        private static void OnLoadedEnabledPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window w)
            {
                bool newEnabled = (bool)e.NewValue;
                bool oldEnabled = (bool)e.OldValue;

                if (oldEnabled && !newEnabled)
                    w.Loaded -= MyWindowLoaded;
                else if (!oldEnabled && newEnabled)
                    w.Loaded += MyWindowLoaded;
            }
        }

        private static void MyWindowLoaded(object sender, RoutedEventArgs e)
        {
            ILoadedAction loadedAction = GetLoadedAction((Window)sender);
            loadedAction?.WindowLoaded();
        }


        public static readonly DependencyProperty LoadedActionProperty =
            DependencyProperty.RegisterAttached(
                "LoadedAction",
                typeof(ILoadedAction),
                typeof(LoadedBindings),
                new PropertyMetadata(null));

        public static ILoadedAction GetLoadedAction(DependencyObject sender) => (ILoadedAction)sender.GetValue(LoadedActionProperty);
        public static void SetLoadedAction(DependencyObject sender, ILoadedAction value) => sender.SetValue(LoadedActionProperty, value);
    }
}
