using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

namespace Tibo.fr.SharpMail
{
    public partial class SharpMailApp : Application
    {
        public SharpMailApp()
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var smm = new SharpMailMain();
            smm.Create();
        }
    }
}
