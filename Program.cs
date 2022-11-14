using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cafenet {
    internal class CafenetApplicationContext : ApplicationContext {
        readonly Form1 form1;
        delegate void ToastNotificationManagerCompatActivatedDelegate(ToastNotificationActivatedEventArgsCompat e);

        public CafenetApplicationContext() {
            form1 = new Form1();
            form1.FormClosed += Form1_FormClosed;
            form1.Show();
            form1.Hide();

            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++) {
                if (args[i] == "-restore") {
                    form1.ParseRestartArgs(args[i + 1]);
                    break;
                }
            }

            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e) {
            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) {
                form1.Close();
                return;
            }
            form1.BeginInvoke(new ToastNotificationManagerCompatActivatedDelegate(form1.OnToastActivated), new object[] { e });
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
            using (var m = new Mutex(true, "{87C138DF-39DE-4EE3-9EEC-3C1D6221A8C8}", out var created)) {
                if (!created) {
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try {
                    Application.Run(new CafenetApplicationContext());
                } finally {
                    ToastNotificationManagerCompat.History.Clear();
                    ToastNotificationManagerCompat.Uninstall();
                }
            }
        }
    }
}
