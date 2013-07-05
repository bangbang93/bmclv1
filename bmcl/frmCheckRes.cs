using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Web;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.WindowsAPICodePack.Taskbar;

using bmcl.ResSer;

namespace bmcl
{
    public partial class frmCheckRes : Form
    {
        public frmCheckRes()
        {
            InitializeComponent();
        }

        delegate string getmd5(string path);
        

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }


        private void frmCheckRes_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.Width - 150;

        }

        private void frmCheckRes_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(FrmMain.URL_RESOURCE_BASE);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                Stream RawXml = res.GetResponseStream();
                XmlDocument doc = new XmlDocument();
                doc.Load(RawXml);
                XmlNodeList nodeLst = doc.GetElementsByTagName("Contents");
                for (int i = 0; i < nodeLst.Count; i++)
                {
                    XmlNode node = nodeLst.Item(i);
                    if (node.GetType() == null)
                        continue;
                    XmlElement element = (XmlElement)node;
                    String key = element.GetElementsByTagName("Key").Item(0).ChildNodes.Item(0).Value;
                    String modtime = element.GetElementsByTagName("LastModified").Item(0).ChildNodes.Item(0).Value;
                    String etag = element.GetElementsByTagName("ETag") == null ? "-" : element.GetElementsByTagName("ETag").Item(0).ChildNodes.Item(0).Value;
                    long size = long.Parse(element.GetElementsByTagName("Size").Item(0).ChildNodes.Item(0).Value);
                    if (size <= 0L)
                        continue;
                    ListViewItem thisitem = listRes.Items.Add(key);
                    thisitem.SubItems.Add(modtime);
                    thisitem.SubItems.Add(size.ToString());
                    thisitem.SubItems.Add("待检查");
                    thisitem.SubItems.Add(etag.Replace("\"","").Trim());
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("与Mojang服务器通信超时，请重试");
                this.Close();
            }
        }

        private void frmCheckRes_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.Width - 150;
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            prs.Maximum = listRes.Items.Count;
            prs.Value = 0;
            TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
            foreach (ListViewItem item in listRes.Items)
            {
                prs.Value++;
                TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
                getmd5 GetMd5 = new getmd5(GetMD5HashFromFile);
                IAsyncResult res = GetMd5.BeginInvoke(@".minecraft/assets/" + item.Text, null, null);
                while (!res.IsCompleted)
                {
                    Thread.Sleep(50);
                    Application.DoEvents();
                }
                string lmd5 = GetMd5.EndInvoke(res);
                if (lmd5.Trim() == item.SubItems[4].Text)
                {
                    item.SubItems[3].Text = "完成";
                }
                else
                {
                    item.SubItems[3].Text = "待同步";
                }
            }
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            WebClient downer = new WebClient();
            prs.Maximum = listRes.Items.Count;
            prs.Value = 0;
            TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
            foreach (ListViewItem item in listRes.Items)
            {
                prs.Value++;
                TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
                if (item.SubItems[3].Text == "待同步")
                {
                    StringBuilder rpath = new StringBuilder(FrmMain.URL_RESOURCE_BASE);
                    StringBuilder lpath = new StringBuilder(Environment.CurrentDirectory + @"\.minecraft\assets\");
                    rpath.Append(item.Text);
                    lpath.Append(item.Text);
                    if (!Directory.Exists(Path.GetDirectoryName(lpath.ToString())))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(lpath.ToString()));
                    }
                    downer.DownloadFile(rpath.ToString(), lpath.ToString());
                    item.SubItems[3].Text = "已同步";
                }
            }
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

    }
}
