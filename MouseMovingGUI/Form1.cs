using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseMovingGUI
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timer = null;

        private bool launched = false;
        private bool running = false;

        private int minutes = 2;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TrayIcon.Icon = Properties.Resources.Red;


            this.TrayIcon.Visible = true;
            this.TrayIcon.ShowBalloonTip(1000, "MouseMoving 1.0已经启动", "我可以定时模拟用户操作鼠标的行为哦,每两分钟运行一次", ToolTipIcon.Info);

            timer = new System.Timers.Timer(minutes * 60000);
            timer.Enabled = false;
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            running = true;
            for (int i = 0; i < 4; i++)
            {
                ExternalWin32Ref.mouse_move(i * 1000, i * 1000);
                Thread.Sleep(1000);
            }
            running = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            launched = false;
            OpeateTimer();
            System.Environment.Exit(0);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            launched = !launched;
            OpeateTimer();
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                startToolStripMenuItem.Enabled = !running;
            }
        }

        private void OpeateTimer()
        {
            if (launched)
            {
                this.TrayIcon.Icon = Properties.Resources.Green;
                timer.Enabled = true;
                timer.Start();
                startToolStripMenuItem.Text = "结束运行";
            }
            else
            {
                this.TrayIcon.Icon = Properties.Resources.Red;
                timer.Enabled = false;
                timer.Stop();
                startToolStripMenuItem.Text = "开始运行";
            }
        }
    }

    class ExternalWin32Ref
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static int mouse_move(int dx, int dy)
        {
            return mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, dx, dy, 0, 0);
        }
        //移动鼠标 
        public const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    }
}
