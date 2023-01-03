using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tontonator.Core.Helpers;
using Tontonator.Core.Services;

namespace Tontonator.Core
{
    internal class App
    {
        public static void Init() => States.ShowMainMenu();
        public static void Exit() => Environment.Exit(0);
    }
}
