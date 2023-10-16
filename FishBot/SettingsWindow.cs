using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FishBot
{
    public partial class SettingsWindow : Form
    {
        string delayFile = "Delay.dly",
            closeConfig = "Switch.swt",
            buttonsConfig = "Buttons.btn",
            randomModeSettings = "RandomMode.rm";
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(closeConfig))
                sw.Write(0);
            Close();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            button1.Location = new Point((Size.Width - button1.Width), 0);
            panel3.Location = new Point(0, 0);
            panel3.Size = new Size(Size.Width, 34);
            ReadValueDelay();
            SetUsButton();
        }
        private void ReadValueDelay()
        {
            using (StreamReader sr = new StreamReader(delayFile))
                trackBar1.Value = Convert.ToInt32(sr.ReadToEnd());
            textBox1.Text = trackBar1.Value.ToString();
        }
        private void SetUsButton()
        {
            using (StreamReader sr = new StreamReader(buttonsConfig))
                textBox2.Text = sr.ReadToEnd();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(delayFile))
                sw.Write(textBox1.Text);
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void button3_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(buttonsConfig))
                sw.Write(textBox2.Text);
        }
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
