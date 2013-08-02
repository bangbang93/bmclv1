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

using bmcl.ResSer;

namespace bmcl
{
    public partial class frmCheckRes : Form
    {
        public frmCheckRes()
        {
            InitializeComponent();
        }

        delegate string getmd5(object obj);
        //string URL_RESOURCE_BASE = "http://file.bangbang93.com/2dmmc.Resources/";
        string URL_RESOURCE_BASE = FrmMain.URL_RESOURCE_BASE;
        int InDownloading = 0;
        int WaitingForSync = 0;

        public void GetMD5HashFromFile(object obj)
        {
            ListViewItem item = obj as ListViewItem;
            string fileName = Environment.CurrentDirectory + @"\.minecraft\assets\" + item.SubItems[0].Text;
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
                string lmd5 = sb.ToString();
                if (lmd5.Trim() == item.SubItems[4].Text)
                {
                    listRes.Invoke(new MethodInvoker(delegate { item.SubItems[3].Text = "完成"; }));
                }
                else
                {
                    listRes.Invoke(new MethodInvoker(delegate { item.SubItems[3].Text = "待同步"; }));
                    WaitingForSync++;
                }
            }
            catch (Exception ex)
            {
                listRes.Invoke(new MethodInvoker(delegate { item.SubItems[3].Text = "待同步"; }));
                WaitingForSync++;
            }
        }


        private void frmCheckRes_Load(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.Width - 150;
        }

        private void frmCheckRes_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            //            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL_RESOURCE_BASE);
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
                    thisitem.SubItems.Add(etag.Replace("\"", "").Trim());
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
            //            TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
            foreach (ListViewItem item in listRes.Items)
            {
                prs.Value++;
                //                TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
                //IAsyncResult res = GetMd5.BeginInvoke(@".minecraft/assets/" + item.Text, null, null);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetMD5HashFromFile), item);
            }
            //            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            prs.Maximum = WaitingForSync;
            prs.Value = 0;
            //            TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
            foreach (ListViewItem item in listRes.Items)
            {
                WebClient downer = new WebClient();
                //                TaskbarManager.Instance.SetProgressValue(prs.Value, prs.Maximum);
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
                    downer.DownloadFileCompleted += downer_DownloadFileCompleted;
                    InDownloading++;
                    downer.DownloadFileAsync(new Uri(rpath.ToString()), lpath.ToString(), item);
                    //downer.DownloadFile(rpath.ToString(), lpath.ToString());

                }
            }
            //            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        void downer_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            InDownloading--;
            listRes.Invoke(new MethodInvoker(delegate { (e.UserState as ListViewItem).SubItems[3].Text = "已同步"; }));
            prs.Value++;
            if (InDownloading == 0)
            {
                MessageBox.Show("同步完成");
                this.Close();
            }
        }

    }
}
