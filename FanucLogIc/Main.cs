using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FanucLogIc
{

    public partial class Main : Form
    {
        short ret;
        ushort h;
        int timeout = 5;
        bool isConnected = false;
        FanucLogIc.Properties.Settings Default = FanucLogIc.Properties.Settings.Default;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label1.Text = Default.Name;
            this.label2.Text = Default.IP;
            this.label3.Text = Default.Port;
            if (Default.IP == "" || Default.Name == "") this.OpenForm();
        }
        private void Form1_FormClosing(object sender, EventArgs e)
        {
            this.F_CloseConn();
        }
        private void OpenForm()
        {
            Form2 form2 = new Form2();
            this.Enabled = false; // Disabilita il form principale
            form2.ShowDialog();
            this.label1.Text = FanucLogIc.Properties.Settings.Default.Name;
            this.label2.Text = FanucLogIc.Properties.Settings.Default.IP;
            this.label3.Text = FanucLogIc.Properties.Settings.Default.Port;
            this.Enabled = true;  // Riabilita il form principale quando il secondo form è chiuso
            this.pictureBox1.BackColor = this.Color();
        }
        private System.Drawing.Color Color()
        {
            FanucLogIc.Properties.Settings Default = FanucLogIc.Properties.Settings.Default;
            if (isConnected) return System.Drawing.Color.Green;
            if (Default.Name != "" || Default.IP != "") return System.Drawing.Color.Orange;
            return System.Drawing.Color.Red;
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            FanucLogIc.Properties.Settings Default = FanucLogIc.Properties.Settings.Default;
            if (Default.Name == "" || Default.IP == "")
            {
                MessageBox.Show("Configure Connection First");
                return;
            }
            bool isConn = this.F_Connect3(Default.IP);
            this.OpenSetting();
        }
        private void OpenConfig(object sender, EventArgs e)
        {
            this.OpenForm();
        }
        
        private void F_CloseConn()
        {
            Focas1.cnc_freelibhndl(h);
        }

        #region Focas Function
        private bool F_Connect3(string ip)
        {
            ret = ret = Focas1.cnc_allclibhndl3(ip, 8193, timeout, out h);
            MessageBox.Show(ret == Focas1.EW_OK ? "Connected" : "Not Connected", "Connection Status");
            return ret == Focas1.EW_OK;
        }
        private bool F_Connect2()
        {
            //ret = Focas1.cnc_allclibhndl2(n_node, out h); //n_node HSSB=9 con NCGuide
            //return ret == Focas1.EW_OK;
            MessageBox.Show("Not Implemented");
            return false;
        }
        
        private void OpenSetting() 
        {
            Functions child = new Functions() { TopLevel = false, TopMost = true };
            child.FormBorderStyle = FormBorderStyle.None;
            this.FuncTable.Controls.Add(child);
            child.Show();
        }
        #endregion
    }
}
