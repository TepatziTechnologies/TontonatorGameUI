using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TontonatorGameUI.Actions
{
    public class DelegateLoadedAction : ILoadedAction
    {
        public Action LoadedActionDelegate { get; set; }

        public DelegateLoadedAction()
        {
        }

        public DelegateLoadedAction(Action action)
        {
            LoadedActionDelegate = action;
        }

        public void WindowLoaded()
        {
            LoadedActionDelegate?.Invoke();
        }
    }
}
