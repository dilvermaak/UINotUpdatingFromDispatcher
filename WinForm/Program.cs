using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WinForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*while (!Debugger.IsAttached)
            {
                // loop
            }*/

            if (Debugger.IsAttached) Debugger.Break();

            Mutex mutex = null;
            if (!Mutex.TryOpenExisting("NotifyIconsLogicMutex", out mutex))
            {
                mutex = new Mutex(false, "NotifyIconsLogicMutex");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.Close();
            }
        }
    }
}
