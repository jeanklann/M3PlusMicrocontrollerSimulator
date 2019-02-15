using System;
using System.Threading;
using System.Windows.Forms;

namespace IDE {
    public partial class SplashScreen : Form {
        private SplashScreen() {
            InitializeComponent();
            label1.Text = string.Format(label1.Text, Application.ProductVersion);
        }

        
        public static FormularioPrincipal OpenMainForm(IWin32Window parent) {
            var splash = new SplashScreen();
            splash.Show();
            FormularioPrincipal form;
            Application.DoEvents();
            form = new FormularioPrincipal();
            if (parent != null)
                form.Show(parent);
            else
                form.Show();
            splash.Close();
            return form;
        }
        public static FormRamMemory OpenRAM(IWin32Window parent) {
            var splash = new SplashScreen();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(10);
            var form = new FormRamMemory();
            form.Build(FormRamType.RAM);
            if(parent != null)
                form.Show(parent);
            else
                form.Show();
            splash.Close();
            return form;
        }
        public static FormRamMemory OpenStack(IWin32Window parent) {
            var splash = new SplashScreen();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(10);
            var form = new FormRamMemory();
            form.Build(FormRamType.Stack);
            if (parent != null)
                form.Show(parent);
            else
                form.Show();
            splash.Close();
            return form;
        }
        public static FormRomMemory OpenROM(IWin32Window parent) {
            var splash = new SplashScreen();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(10);
            var form = new FormRomMemory();
            if (parent != null)
                form.Show(parent);
            else
                form.Show();
            splash.Close();
            return form;
        }


        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
