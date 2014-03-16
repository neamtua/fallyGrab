using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Security.Permissions;
using fallyToast;
using System.IO;
namespace fallyGrab
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(errorHandler);
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "fallyGrab", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new mainForm());
                }
            } 
        }

        static void errorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            fallyToast.Toaster general = new fallyToast.Toaster();
            general.Show("fallyGrab", "An error has occured. Check error log.", -1, "Fade", "Up", "", "", "error");

            Exception e = (Exception)args.ExceptionObject;
            commonFunctions.writeLog(e.Message,e.StackTrace);
        }
    }
}
