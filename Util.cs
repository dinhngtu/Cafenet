using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Cafenet.NativeMethods;

namespace Cafenet {
    internal static class Util {
        public static string GetCurrentPackageFullName() {
            // https://github.com/microsoft/Windows-AppConsult-Tools-DesktopBridgeHelpers/blob/master/DesktopBridge.Helpers/Helpers.cs
            if (Environment.OSVersion.Version < new Version(6, 2, 0, 0)) {
                return null;
            }

            uint length = 0;
            int ret;
            ret = NativeMethods.GetCurrentPackageFullName(ref length, null);
            if (ret == APPMODEL_ERROR_NO_PACKAGE || ret != ERROR_INSUFFICIENT_BUFFER) {
                return null;
            }
            var sb = new StringBuilder((int)length);
            ret = NativeMethods.GetCurrentPackageFullName(ref length, sb);
            if (ret == ERROR_SUCCESS) {
                return sb.ToString();
            } else if (ret == APPMODEL_ERROR_NO_PACKAGE) {
                return null;
            } else {
                // ignore error for now
                return null;
            }
        }
    }
}
