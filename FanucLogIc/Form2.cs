using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanucLogIc
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Config_Click(object sender, EventArgs e)
        {
            FanucLogIc.Properties.Settings.Default.IP =   this.ipbox.Text;
            FanucLogIc.Properties.Settings.Default.Name = this.nameTbox.Text;
            FanucLogIc.Properties.Settings.Default.Port = this.portbox.Text;
            FanucLogIc.Properties.Settings.Default.Save();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ipbox.Text = FanucLogIc.Properties.Settings.Default.IP;
            this.nameTbox.Text = FanucLogIc.Properties.Settings.Default.Name;
            this.portbox.Text = FanucLogIc.Properties.Settings.Default.Port;
        }
    }
}
