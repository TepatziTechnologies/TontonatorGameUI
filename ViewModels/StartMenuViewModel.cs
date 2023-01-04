using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TontonatorGameUI.Actions;

namespace TontonatorGameUI.ViewModels
{
    public class StartMenuViewModel
    {
        private DelegateLoadedAction _loadAction = null;
        public DelegateLoadedAction LoadAction
        {
            get
            {
                return _loadAction ?? (_loadAction = new DelegateLoadedAction(() =>
                {
                    
                }));
            }
        }
    }
}
