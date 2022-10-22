using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Cafenet.NativeMethods;

namespace Cafenet {
    internal abstract class WakerCommand { }

    internal class WakerStop : WakerCommand { }
    internal class WakerStart : WakerCommand { }
    internal class WakerSetScreenOn : WakerCommand {
        public bool Value { get; set; }
    }
    internal class WakerExit : WakerCommand { }

    internal class Waker {
        public BlockingCollection<WakerCommand> Commands { get; set; }

        bool Enabled { get; set; } = false;

        bool ScreenOn { get; set; } = false;

        public Waker() {
            Commands = new BlockingCollection<WakerCommand>();
        }

        void Update() {
            uint flags = ES_CONTINUOUS;
            if (Enabled) {
                flags |= ES_SYSTEM_REQUIRED;
                if (ScreenOn) {
                    flags |= ES_DISPLAY_REQUIRED;
                }
            }
            SetThreadExecutionState(flags);
        }

        public void ThreadProc() {
            while (true) {
                var _cmd = Commands.Take();
                switch (_cmd) {
                    case WakerExit _:
                        Enabled = false;
                        Update();
                        return;
                    case WakerStart _:
                        Enabled = true;
                        break;
                    case WakerStop _:
                        Enabled = false;
                        break;
                    case WakerSetScreenOn cmd:
                        ScreenOn = cmd.Value;
                        break;
                }
                Update();
            }
        }
    }
}
