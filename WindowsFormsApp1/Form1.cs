using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //Import Win32 APIs to smoothen screen grab
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        public Form1()
        {
            InitializeComponent();

            // Attachment to drag handle (panelDrag)
            panel1.MouseDown += PanelDrag_MouseDown;

            // Load the saved path if it exists
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastSelectedPath))
            {
                this.textBox1.Text = Properties.Settings.Default.LastSelectedPath;
            }
            else
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string customPath = Path.Combine(appDataPath, "RedM");
                this.textBox1.Text = customPath;
            }

            // Attach FormClosing event
            this.FormClosing += Form1_FormClosing;
        }
        private void PanelDrag_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void fileButton_Click(object sender, EventArgs e)
        {
            //grab local app data
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string customPath = Path.Combine(appDataPath, "RedM");

            //create folder dialog
            FolderBrowserDialog diag = new FolderBrowserDialog();
            diag.SelectedPath = customPath; //set the pre folder to the redM location

            if (diag.ShowDialog() == DialogResult.OK) //open diag and set the text
            {
                string folderPath = diag.SelectedPath;
                this.textBox1.Text = folderPath;

                // Save the selected path
                Properties.Settings.Default.LastSelectedPath = folderPath;
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the current path when the form is closing
            Properties.Settings.Default.LastSelectedPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            //Modify to restore the last used path
            string exePath = this.textBox1.Text + "\\RedM.exe";
            //MessageBox.Show("Selected exe path: " + exePath); test
            try
            {
                Process.Start(exePath);
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error Launching .exe: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cacheButton_Click(object sender, EventArgs e)
        {
            string exePath = this.textBox1.Text + "\\RedM.exe";
            string cacheGeneral = this.textBox1.Text + "\\RedM.app\\data\\cache";
            string cacheServer = this.textBox1.Text + "\\RedM.app\\data\\server-cache";
            string cacheServerPriv = this.textBox1.Text + "\\RedM.app\\data\\server-cache-priv";

            try //Delete the directory (folders) and start then close this app
            {

                Directory.Delete(cacheGeneral, true); // delete cache 1
                Directory.Delete(cacheServer, true); // delete cache 2
                Directory.Delete(cacheServerPriv, true); // delete cache 3
                Process.Start(exePath);
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
