using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace FishBot
{
    public partial class Form1 : Form
    {
        CreateSomeFiles createSomeFiles;
        SettingsWindow settingsWindow;
        string delayFile = "Delay.dly",
            closeConfig = "Switch.swt",
            buttonsConfig = "Buttons.btn";
        string usbutton;
        bool reset = true,
            OnOff = false;
        short delayClicks;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Stop();
            button3.Location = new Point((Size.Width - button3.Size.Width), 0);
            panel1.Location = new Point(0, 0);
            panel1.Size = new Size((Size.Width), (27));
            CreateConfigFile();
        }
        private void CreateConfigFile()
        {
            createSomeFiles = new CreateSomeFiles(delayFile, "200");
            createSomeFiles = new CreateSomeFiles(closeConfig, "0");
            createSomeFiles = new CreateSomeFiles(buttonsConfig, "7");
            //if (!File.Exists(delayFile))
            //    using (StreamWriter sw = new StreamWriter(delayFile))
            //        sw.Write(200);
            //if (!File.Exists(closeConfig))
            //    using (StreamWriter sw = new StreamWriter(closeConfig))
            //        sw.Write(0);
            //if (!File.Exists(buttonsConfig))
            //    using (StreamWriter sw = new StreamWriter(buttonsConfig))
            //        sw.Write(7);
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (OnOff == false)
                MessageBox.Show("the program doesn't work", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (OnOff == true)
            {
                pictureBox1.Image = Properties.Resources.OFF_3x;
                timer1.Stop();
                OnOff = false;
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (OnOff == true)
                MessageBox.Show("the program works", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (OnOff == false)
            {
                pictureBox1.Image = Properties.Resources.ON_3x;
                timer1.Start();
                OnOff = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.NumPad9) && reset == true)
            {
                using (StreamReader sr = new StreamReader(delayFile))
                    delayClicks = (short)Convert.ToInt32(sr.ReadToEnd());
                using (StreamReader sr = new StreamReader(buttonsConfig))
                    usbutton = sr.ReadToEnd();
                Click_Mouse();
                Thread.Sleep(delayClicks);
                SendKeys.Send(usbutton);
                reset= false;

            }
            if (Keyboard.IsKeyUp(Key.NumPad9))
                reset = true;
        }

        private void Click_Mouse()
        {
            POINT p = new POINT();
            GetCursorPos(ref p);
            DoMouseRightClick(p.x, p.y);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dsFlags, int dx, int dy, int cButtons, int dsExtraInfo);

        public const int MOUSE_EVENT_F_LEFTDOWN = 0X02;
        public const int MOUSE_EVENT_F_LEFTUP = 0X04;

        public const int MOUSE_EVENT_F_RIGHTDOWN = 0x08;
        public const int MOUSE_EVENT_F_RIGHTUP = 0x10;

        private void DoMouseRightClick(int x, int y)
        {
            mouse_event(MOUSE_EVENT_F_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MOUSE_EVENT_F_RIGHTUP, x, y, 0, 0);
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPoint);


        private void button4_Click(object sender, EventArgs e)
        {
            byte closeValue;
            using (StreamReader sr = new StreamReader(closeConfig))
                closeValue = Convert.ToByte(sr.ReadToEnd());
            if (closeValue == 0)
            {
                settingsWindow = new SettingsWindow();
                settingsWindow.Show();
                using (StreamWriter sw = new StreamWriter(closeConfig))
                    sw.Write(1);
            }
            if (closeValue == 1)
                MessageBox.Show("The window is open", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //--------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(closeConfig))
                sw.Write(0);
            Close();
        }
    }
}
