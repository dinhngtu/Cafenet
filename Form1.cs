using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Windows.ApplicationModel;
using Windows.System;

using static Cafenet.NativeMethods;

namespace Cafenet {
    enum CafeModes {
        Deadline,
        UntilLock,
        UntilUnlock,
        Perpetual,
    }

    public partial class Form1 : Form {
        Waker Waker { get; set; }
        Thread WakerThread { get; set; }
        CafeModes Mode { get; set; } = CafeModes.Deadline;
        DateTime Deadline { get; set; } = DateTime.MinValue;
        bool KeepScreenOn {
            get {
                return keepScreenOnToolStripMenuItem.Checked;
            }
            set {
                keepScreenOnToolStripMenuItem.Checked = value;
            }
        }
        bool KeepScreenOnThisTime { get; set; } = false;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Waker = new Waker();
            WakerThread = new Thread(Waker.ThreadProc);
            WakerThread.Start();
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            UpdateTimer(DateTime.Now);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            TimerStop();
            notifyIcon1.Visible = false;
            Waker.Commands.Add(new WakerExit());
            WakerThread.Join();
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e) {
            if ((e.Reason == SessionSwitchReason.SessionLock && Mode == CafeModes.UntilLock) ||
                (e.Reason == SessionSwitchReason.SessionUnlock && Mode == CafeModes.UntilUnlock)) {
                Mode = CafeModes.Deadline;
                Deadline = DateTime.MinValue;
                UpdateTimer(DateTime.Now);
            }
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e) {
            var restartArgs = new object[] {
                Mode,
                Deadline.Ticks,
                KeepScreenOn,
                KeepScreenOnThisTime,
                keepEnabledToolStripMenuItem.Checked,
            };
            RegisterApplicationRestart($"-restore {string.Join(",", restartArgs)}", RESTART_NO_REBOOT);
            // once SystemEvents_SessionEnding has fired, our main form might already be closing and we might already be past the point of return
            // close ourselves to avoid complicated situations
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            UpdateTimer(DateTime.Now);
        }

        private void keepScreenOnToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            Waker.Commands.Add(new WakerSetScreenOn() { Value = keepScreenOnToolStripMenuItem.Checked });
            if (!keepScreenOnToolStripMenuItem.Checked) {
                KeepScreenOnThisTime = false;
            }
        }

        void TimerStart() {
            Waker.Commands.Add(new WakerStart());
            if (!timer1.Enabled) {
                timer1.Start();
            }
        }

        void TimerStop() {
            Waker.Commands.Add(new WakerStop());
            timer1.Stop();
        }

        private void UpdateTimer(DateTime reference) {
            var timeLeft = Deadline - reference;
            switch (Mode) {
                case CafeModes.Deadline:
                    if (timeLeft > TimeSpan.Zero) {
                        timeLeftToolStripMenuItem.Text = $"Time l&eft: {(int)timeLeft.TotalHours,2:00}:{timeLeft.Minutes,2:00}";
                    } else {
                        timeLeftToolStripMenuItem.Text = "Keep awak&e";
                    }
                    break;
                case CafeModes.UntilLock:
                    timeLeftToolStripMenuItem.Text = "&Enabled until lock";
                    break;
                case CafeModes.UntilUnlock:
                    timeLeftToolStripMenuItem.Text = "&Enabled until unlock";
                    break;
                case CafeModes.Perpetual:
                    timeLeftToolStripMenuItem.Text = "&Enabled";
                    break;
            }
            var inactive = Mode == CafeModes.Deadline && timeLeft <= TimeSpan.Zero;
            if (inactive) {
                TimerStop();
                notifyIcon1.Icon = Properties.Resources.Asleep;
                notifyIcon1.Text = "Cafenet";
                if (KeepScreenOnThisTime) {
                    keepScreenOnToolStripMenuItem.Checked = false;
                }
            } else {
                TimerStart();
                notifyIcon1.Icon = Properties.Resources.Awake;
                notifyIcon1.Text = timeLeftToolStripMenuItem.Text.Replace("&", "");
                if (keepScreenOnToolStripMenuItem.Checked) {
                    notifyIcon1.Text += $" (screen on{(KeepScreenOnThisTime ? " this time" : "")})";
                }
            }
            turnOffToolStripMenuItem.Checked = inactive;
            add15MinutesToolStripMenuItem.Enabled =
                add30MinutesToolStripMenuItem.Enabled =
                add1HourToolStripMenuItem.Enabled =
                add2HoursToolStripMenuItem.Enabled =
                add4HoursToolStripMenuItem.Enabled = Mode == CafeModes.Deadline;
            untillockToolStripMenuItem.Checked = Mode == CafeModes.UntilLock;
            untilunlockToolStripMenuItem.Checked = Mode == CafeModes.UntilUnlock;
            keepEnabledToolStripMenuItem.Checked = Mode == CafeModes.Perpetual;
            if (sleepOnFinishToolStripMenuItem.Checked && inactive) {
                sleepOnFinishToolStripMenuItem.Checked = false;
                try {
                    NativeUtilities.SuspendSystem();
                } catch {
                }
            }
        }

        private void turnOffToolStripMenuItem_Click(object sender, EventArgs e) {
            sleepOnFinishToolStripMenuItem.Checked = false;
            Mode = CafeModes.Deadline;
            Deadline = DateTime.MinValue;
            UpdateTimer(DateTime.Now);
        }

        void AddMinutes(double mins) {
            var now = DateTime.Now;
            Mode = CafeModes.Deadline;
            if (Deadline == DateTime.MaxValue) {
                return;
            } else if (Deadline < now) {
                Deadline = now + TimeSpan.FromMinutes(mins);
            } else {
                Deadline += TimeSpan.FromMinutes(mins);
            }
            UpdateTimer(DateTime.Now);
        }

        private void add15MinutesToolStripMenuItem_Click(object sender, EventArgs e) {
            AddMinutes(1);
        }

        private void add30MinutesToolStripMenuItem_Click(object sender, EventArgs e) {
            AddMinutes(30);
        }

        private void add1HourToolStripMenuItem_Click(object sender, EventArgs e) {
            AddMinutes(60);
        }

        private void add2HoursToolStripMenuItem_Click(object sender, EventArgs e) {
            AddMinutes(120);
        }

        private void add4HoursToolStripMenuItem_Click(object sender, EventArgs e) {
            AddMinutes(240);
        }

        private void untillockToolStripMenuItem_Click(object sender, EventArgs e) {
            Mode = CafeModes.UntilLock;
            UpdateTimer(DateTime.Now);
            if (!keepScreenOnToolStripMenuItem.Checked) {
                var toast = new ToastContentBuilder();
                toast.AddText("Want to keep your screen on just this time?");
                toast.AddButton(new ToastButton().SetContent("Keep screen on").AddArgument("keepScreenOn"));
                toast.AddButton(new ToastButton().SetContent("Dismiss"));
                toast.Show();
            }
        }

        private void untilunlockToolStripMenuItem_Click(object sender, EventArgs e) {
            Mode = CafeModes.UntilUnlock;
            UpdateTimer(DateTime.Now);
        }

        private void keepEnabledToolStripMenuItem_Click(object sender, EventArgs e) {
            Mode = CafeModes.Perpetual;
            UpdateTimer(DateTime.Now);
        }

        async Task<StartupTask> GetStartupTaskAsync() {
            if (Util.GetCurrentPackageFullName() != null) {
                return await StartupTask.GetAsync("{063370BB-B5F6-4DD0-8017-B0527C9DA4D7}");
            }
            return null;
        }

        private async void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            var startupTask = await GetStartupTaskAsync();
            runOnStartupToolStripSeparator.Visible = runOnStartupToolStripMenuItem.Visible = startupTask != null;
            if (startupTask != null) {
                runOnStartupToolStripMenuItem.Enabled = startupTask.State == StartupTaskState.Enabled || startupTask.State == StartupTaskState.Disabled || startupTask.State == StartupTaskState.DisabledByUser;
                runOnStartupToolStripMenuItem.Checked = startupTask.State == StartupTaskState.Enabled || startupTask.State == StartupTaskState.EnabledByPolicy;
            }
        }

        private async void runOnStartupToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            var startupTask = await GetStartupTaskAsync();
            if (startupTask != null) {
                if (runOnStartupToolStripMenuItem.Checked && startupTask.State == StartupTaskState.DisabledByUser) {
                    runOnStartupToolStripMenuItem.Checked = false;
                    await Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
                } else if (runOnStartupToolStripMenuItem.Checked && startupTask.State == StartupTaskState.Disabled) {
                    await startupTask.RequestEnableAsync();
                } else if (!runOnStartupToolStripMenuItem.Checked && startupTask.State == StartupTaskState.Enabled) {
                    startupTask.Disable();
                }
            }
        }

        public void OnToastActivated(ToastNotificationActivatedEventArgsCompat e) {
            var args = ToastArguments.Parse(e.Argument);
            if (args.Contains("keepScreenOn")) {
                KeepScreenOnThisTime = true;
                keepScreenOnToolStripMenuItem.Checked = true;
            }
        }

        public void ParseRestartArgs(string restartArgs) {
            var settings = restartArgs.Split(',');
            if (settings.Length >= 4) {
                try {
                    var mode = (CafeModes)Enum.Parse(typeof(CafeModes), settings[0]);
                    var deadline = new DateTime(long.Parse(settings[1]));
                    var keepScreenOn = bool.Parse(settings[2]);
                    var keepScreenOnThisTime = bool.Parse(settings[3]);
                    bool sleepOnFinish = false;
                    if (settings.Length >= 5) {
                        sleepOnFinish = bool.Parse(settings[4]);
                    }
                    Mode = mode;
                    Deadline = deadline;
                    KeepScreenOn = keepScreenOn;
                    KeepScreenOnThisTime = keepScreenOnThisTime;
                    sleepOnFinishToolStripMenuItem.Checked = sleepOnFinish;
                    UpdateTimer(DateTime.Now);
                } catch {
                }
            }
        }
    }
}
