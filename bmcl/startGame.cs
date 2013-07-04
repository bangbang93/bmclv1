using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bmcl
{
    public partial class startGame : Form
    {
        public startGame()
        {
            InitializeComponent();
        }

        FrmMain frmmain = new FrmMain();

        private void changestatus(string status)
        {
            this.labStatus.Text=status;
        }
        public void setstatus(string status)
        {
            
        }

        private void startGame_Load(object sender, EventArgs e)
        {
            frmmain.changeEvent += frmmain_changeEvent;
            launcher.changeEvent += launcher_changeEvent;
        }

        void launcher_changeEvent(string status)
        {
            this.label1.Text = status;
        }

        void frmmain_changeEvent(string status)
        {
            this.labStatus.Text = status;
        }
    }
}
