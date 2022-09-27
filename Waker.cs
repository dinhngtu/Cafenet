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

        bool ScreenOn { get; set; } = false;

        public Waker() {
            Commands = new BlockingCollection<WakerCommand>();
        }

        void Start() {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | (ScreenOn ? ES_DISPLAY_REQUIRED : 0));
        }

        void Stop() {
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        void Restart() {
            uint old = SetThreadExecutionState(ES_CONTINUOUS);
            SetThreadExecutionState((old & ~ES_DISPLAY_REQUIRED) | (ScreenOn ? ES_DISPLAY_REQUIRED : 0));
        }

        public void ThreadProc() {
            while (true) {
                var _cmd = Commands.Take();
                switch (_cmd) {
                    case WakerExit _:
                        Stop();
                        return;
                    case WakerStart _:
                        Start();
                        break;
                    case WakerStop _:
                        Stop();
                        break;
                    case WakerSetScreenOn cmd:
                        ScreenOn = cmd.Value;
                        Restart();
                        break;
                }
            }
        }
    }
}
