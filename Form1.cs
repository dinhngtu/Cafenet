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

namespace Cafenet {
    enum CafeModes {
        Deadline,
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
            if (e.Reason == SessionSwitchReason.SessionUnlock && Mode == CafeModes.UntilUnlock) {
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
            notifyIcon1.Icon = Properties.Resources.iconon;
        }

        void TimerStop() {
            Waker.Commands.Add(new WakerStop());
            timer1.Stop();
            notifyIcon1.Icon = Properties.Resources.iconoff;
        }

        private void UpdateTimer(DateTime reference) {
            var timeLeft = Deadline - reference;
            switch (Mode) {
                case CafeModes.Deadline:
                    if (timeLeft > TimeSpan.Zero) {
                        notifyIcon1.Text = timeLeftToolStripMenuItem.Text = $"Time l&eft: {(int)timeLeft.TotalHours,2:00}:{timeLeft.Minutes,2:00}";
                        turnOffToolStripMenuItem.Checked = untilunlockToolStripMenuItem.Checked = keepEnabledToolStripMenuItem.Checked = false;
                        TimerStart();
                    } else {
                        timeLeftToolStripMenuItem.Text = "Keep awak&e";
                        notifyIcon1.Text = "Cafen&et";
                        turnOffToolStripMenuItem.Checked = true;
                        untilunlockToolStripMenuItem.Checked = keepEnabledToolStripMenuItem.Checked = false;
                        TimerStop();
                    }
                    break;
                case CafeModes.UntilUnlock:
                    notifyIcon1.Text = timeLeftToolStripMenuItem.Text = "&Enabled until unlock";
                    untilunlockToolStripMenuItem.Checked = true;
                    turnOffToolStripMenuItem.Checked = keepEnabledToolStripMenuItem.Checked = false;
                    TimerStart();
                    break;
                case CafeModes.Perpetual:
                    notifyIcon1.Text = timeLeftToolStripMenuItem.Text = "&Enabled";
                    keepEnabledToolStripMenuItem.Checked = true;
                    turnOffToolStripMenuItem.Checked = untilunlockToolStripMenuItem.Checked = false;
                    TimerStart();
                    break;
            }
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
