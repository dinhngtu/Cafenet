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
        CafeModes Mode { get; set; }
        DateTime Deadline { get; set; }

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Waker = new Waker();
            WakerThread = new Thread(Waker.ThreadProc);
            WakerThread.Start();
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            UpdateTimer(DateTime.Now);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            TimerStop();
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            UpdateTimer(DateTime.Now);
        }

        private void keepScreenOnToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            Waker.Commands.Add(new WakerSetScreenOn() { Value = keepScreenOnToolStripMenuItem.Checked });
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
                        notifyIcon1.Text = timeLeftToolStripMenuItem.Text = $"Time l&eft: {(int)timeLeft.TotalHours,2:00}:{timeLeft.Minutes,2:00}";
                    } else {
                        timeLeftToolStripMenuItem.Text = "Keep awak&e";
                        notifyIcon1.Text = "Cafen&et";
                    }
                    break;
                case CafeModes.UntilLock:
                    notifyIcon1.Text = timeLeftToolStripMenuItem.Text = "&Enabled until lock";
                    break;
                case CafeModes.UntilUnlock:
                    notifyIcon1.Text = timeLeftToolStripMenuItem.Text = "&Enabled until unlock";
                    break;
                case CafeModes.Perpetual:
                    notifyIcon1.Text = timeLeftToolStripMenuItem.Text = "&Enabled";
                    break;
            }
            var inactive = Mode == CafeModes.Deadline && timeLeft <= TimeSpan.Zero;
            if (inactive) {
                TimerStop();
                notifyIcon1.Icon = Properties.Resources.Asleep;
            } else {
                TimerStart();
                notifyIcon1.Icon = Properties.Resources.Awake;
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
        }

        private void turnOffToolStripMenuItem_Click(object sender, EventArgs e) {
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
            AddMinutes(15);
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
        }

        private void untilunlockToolStripMenuItem_Click(object sender, EventArgs e) {
            Mode = CafeModes.UntilUnlock;
            UpdateTimer(DateTime.Now);
        }

        private void keepEnabledToolStripMenuItem_Click(object sender, EventArgs e) {
            Mode = CafeModes.Perpetual;
            UpdateTimer(DateTime.Now);
        }
    }
}
