using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StartUpHandler
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        string processname = "TechnicianClient";

        // Pinvoke declaration for ShowWindow
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



        public Form1()
        {

            InitializeComponent();
            // Get all current running processes into Combo Box1
            comboBox1.Items.Clear();
            Process[] MyProcess = Process.GetProcesses();
            for (int i = 0; i < MyProcess.Length; i++)
                comboBox1.Items.Add(MyProcess[i].ProcessName);
            // comboBox1.Items.Add(MyProcess[i].ProcessName + "-" + MyProcess[i].Id);
            comboBox1.Sorted = true;

            toolStripStatusLabel1.Text = "Wait, login, maximize, close";

        }

        // If NUMARA TechnicianClient is found, login, then quit
        private void button1_Click(object sender, EventArgs e)
        {
            Process ps = Process.GetProcessesByName(comboBox1.SelectedItem.ToString()).FirstOrDefault();
            MessageBox.Show(ps.MainWindowTitle);
            //MessageBox.Show(ps.MainWindowTitle);
            //Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process p = Process.GetProcessesByName(processname).FirstOrDefault();

            if (p != null)
                if (p.MainWindowTitle == "Track-It! Login")
                {
                    IntPtr h = p.MainWindowHandle;

                    SetForegroundWindow(h);
                    SendKeys.SendWait("tlaw");
                    SetForegroundWindow(h);
                    SendKeys.SendWait("{TAB}");
                    SetForegroundWindow(h);
                    SendKeys.SendWait("welcome");
                    SetForegroundWindow(h);
                    SendKeys.SendWait("{ENTER}");

                }
                // better to use MainWindowTitle since it's possible it would detect the program during the ClickOnce prelogin form and shutdown
                else if (p.MainWindowTitle == "Track-It! Technician Client - Tim Law")
                    {
                        IntPtr h = p.MainWindowHandle;
                        // move to second monitor
                        SetWindowPos(h,IntPtr.Zero, 1913, 0, 1024, 768, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
                        ShowWindow(h, SW_SHOWMAXIMIZED);
                   
                        Application.Exit();
                    }
                else
                {
                    MessageBox.Show(p.MainWindowTitle);
                }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process ps = Process.GetProcessesByName(comboBox1.SelectedItem.ToString()).FirstOrDefault();
            toolStripStatusLabel1.Text = ps.MainWindowTitle;
        }
    }
}
