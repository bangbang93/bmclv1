using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bmcl.serverlist
{
    public partial class AddServer : Form
    {
        serverlist list;
        serverinfo ServerInfo;
        int num = -1;
        public AddServer(ref serverlist list)
        {
            InitializeComponent();
            this.list = list;
        }

        public AddServer(ref serverlist list, int num)
        {
            InitializeComponent();
            this.list = list;
            txtServerName.Text = list.info[num].Name;
            txtAddress.Text = list.info[num].Address;
            checkIsHide.Checked = list.info[num].IsHide;
            this.num = num;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (num == -1)
            {
                if (txtServerName.Text.Trim() == string.Empty || txtServerName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("输入有误，请检查");
                    return;
                }
                list.Add(txtServerName.Text, txtAddress.Text, checkIsHide.Checked);
            }
            else
            {
                if (txtServerName.Text.Trim() == string.Empty || txtServerName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("输入有误，请检查");
                    return;
                }
                list.info[num] = new serverinfo(txtServerName.Text, checkIsHide.Checked, txtAddress.Text);
            }
            ServerInfo = new serverinfo(txtServerName.Text, checkIsHide.Checked, txtAddress.Text);
        }
        public serverinfo getEdit()
        {
            return ServerInfo;
        }
    }
}
