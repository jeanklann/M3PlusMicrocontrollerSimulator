using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IDE {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try {
                Application.Run(SplashScreen.OpenMainForm(null));
            } catch(Exception e) {
                UiStatics.ShowExceptionMessage(e);
            }
            
        }
    }
}
