using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace Cafenet {
    internal class CafenetApplicationContext : ApplicationContext {
        Form1 form1;
        PackageCatalog packageCatalog;

        public CafenetApplicationContext() {
            form1 = new Form1();
            form1.FormClosed += Form1_FormClosed;
            form1.ShowInTaskbar = false;
            form1.Opacity = 0;
            form1.Show();
            form1.Hide();
            if (Util.GetCurrentPackageFullName() != null) {
                packageCatalog = PackageCatalog.OpenForCurrentPackage();
                packageCatalog.PackageUninstalling += PackageCatalog_PackageUninstalling;
            }
        }

        private void PackageCatalog_PackageUninstalling(PackageCatalog sender, PackageUninstallingEventArgs args) {
            form1.Close();
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
                Application.Run(new CafenetApplicationContext());
            }
        }
    }
}
