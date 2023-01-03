using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TontonatorGameUI.Actions;

namespace TontonatorGameUI.ViewModels
{
    public class LoadingViewModel 
    {
        private DelegateLoadedAction _loadAction = null;
        public DelegateLoadedAction LoadAction
        {
            get
            {
                return _loadAction ?? (_loadAction = new DelegateLoadedAction( () =>
                {
                    
                }));
            }
        }
    }
}
