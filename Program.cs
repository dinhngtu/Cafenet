using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace Cafenet {
    internal class CafenetApplicationContext : ApplicationContext {
        readonly Form1 form1;
        readonly PackageCatalog packageCatalog;
        delegate void ToastNotificationManagerCompatActivatedDelegate(ToastNotificationActivatedEventArgsCompat e);

        public CafenetApplicationContext() {
            form1 = new Form1();
            form1.FormClosed += Form1_FormClosed;
            form1.Show();
            form1.Hide();
            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;
            if (Util.GetCurrentPackageFullName() != null) {
                packageCatalog = PackageCatalog.OpenForCurrentPackage();
                packageCatalog.PackageUninstalling += PackageCatalog_PackageUninstalling;
            }
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e) {
            form1.BeginInvoke(new ToastNotificationManagerCompatActivatedDelegate(form1.OnToastActivated), new object[] { e });
        }

        private void PackageCatalog_PackageUninstalling(PackageCatalog sender, PackageUninstallingEventArgs args) {
            form1.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            ToastNotificationManagerCompat.Uninstall();
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
                Application.Run(new CafenetApplicationContext());
            }
        }
    }
}
