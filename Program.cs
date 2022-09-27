using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafenet {
    internal class CafenetApplicationContext : ApplicationContext {
        Form1 form1;

        public CafenetApplicationContext() {
            form1 = new Form1();
            form1.FormClosed += Form1_FormClosed;
            form1.ShowInTaskbar = false;
            form1.Opacity = 0;
            form1.Show();
            form1.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            ExitThread();
        }
    }

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CafenetApplicationContext());
        }
    }
}
