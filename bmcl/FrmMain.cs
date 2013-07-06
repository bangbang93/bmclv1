using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Configuration;
using System.Reflection;
using System.Collections;
using System.Net;
using System.Diagnostics;
//using Microsoft.WindowsAPICodePack.Taskbar;

using bmcl.versions;

namespace bmcl
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            #region 加载配置
            if (!File.Exists(cfgfile))
            {
                StreamWriter wCfg = new StreamWriter(cfgfile);
                cfg = new config();
                Cfg.WriteObject(wCfg.BaseStream, cfg);
                wCfg.Close();
            }
            else
            {
                try
                {
                    StreamReader rCfg = new StreamReader(cfgfile);
                    cfg = Cfg.ReadObject(rCfg.BaseStream) as config;
                    rCfg.Close();
                }
                catch
                {
                    MessageBox.Show("配置文件无效，加载默认配置");
                    cfg = new config();
                    StreamWriter wCfg = new StreamWriter(cfgfile);
                    Cfg.WriteObject(wCfg.BaseStream, cfg);
                    wCfg.Close();
                }
            }
            txtJavaw.Text = cfg.javaw;
            txtUserName.Text = cfg.username;
            if (cfg.passwd != null)
                txtPwd.Text = Encoding.UTF8.GetString(cfg.passwd);
            txtJavaXmx.Text = cfg.javaxmx;
            #endregion
            #region 加载插件
            listAuth.Items.Add("啥都没有");
            if (Directory.Exists("auths"))
            {
                string[] authplugins = Directory.GetFiles(Application.StartupPath + @"\auths");
                foreach (string auth in authplugins)
                {
                    if (auth.ToLower().EndsWith(".dll"))
                    {
                        try
                        {
                            Assembly AuthMothed = Assembly.LoadFrom(auth);
                            Type[] types = AuthMothed.GetTypes();
                            foreach (Type t in types)
                            {
                                if (t.GetInterface("auth") != null)
                                {
                                    Auths.Add(AuthMothed.CreateInstance(t.FullName));
                                    object Auth = Auths[Auths.Count - 1];
                                    Type T = Auth.GetType();
                                    MethodInfo AuthName = T.GetMethod("getname");
                                    listAuth.Items.Add(AuthName.Invoke(Auth, null).ToString());

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
            }
            #endregion
        }


        #region 属性

        static int LauncherVer = 3;
        static private string cfgfile = "bmcl.xml";
        public static String URL_DOWNLOAD_BASE = "https://s3.amazonaws.com/Minecraft.Download/";
	    public static String URL_RESOURCE_BASE = "https://s3.amazonaws.com/Minecraft.Resources/";
        private ArrayList Auths=new ArrayList();
        static config cfg;
        static DataContractSerializer Cfg = new DataContractSerializer(typeof(config));
        static public gameinfo info;
        string session;
        bool startup = true;
        launcher game;
        bool inscreen;

        #endregion 


        #region 事件
        public static event statuschange changeEvent;
        #endregion


        #region 委托
        public delegate void statuschange(string status);
        #endregion

        #region 公共方法
        /// <summary>
        /// 保存设置
        /// </summary>
        static public void saveconfig()
        {
            StreamWriter wCfg = new StreamWriter(cfgfile);
            Cfg.WriteObject(wCfg.BaseStream, cfg);
            wCfg.Close();
        }
        /// <summary>
        /// 文件夹递归复制
        /// </summary>
        /// <param name="from">源</param>
        /// <param name="to">目标</param>
        static public void dircopy(string from,string to)
        {
            DirectoryInfo dir = new DirectoryInfo(from);
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }
            foreach (DirectoryInfo sondir in dir.GetDirectories())
            {
                dircopy(sondir.FullName, to + "\\" + sondir.Name);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                File.Delete(to + "\\" + file.Name);
                File.Copy(file.FullName, to + "\\" + file.Name);
            }
        }
        /// <summary>
        /// 读取lib信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns>所有lib文件的路径</returns>
        static public string[] readlibpaths(gameinfo info)
        {
            ArrayList libpaths = new ArrayList();
            foreach (libraries.libraryies lib in info.libraries)
            {
                if (lib.natives==null)
                    libpaths.Add(launcher.buildLibPath(lib));
            }
            return (string[])libpaths.ToArray(typeof(string));
        }
        /// <summary>
        /// 读取native信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns>所有navite文件的路径</returns>
        static public string[] readnativepaths(gameinfo info)
        {
            ArrayList nativepaths = new ArrayList();
            foreach (libraries.libraryies lib in info.libraries)
            {
                if (lib.natives!=null)
                    nativepaths.Add(launcher.buildNativePath(lib));
            }
            return (string[])nativepaths.ToArray(typeof(string));
        }
        #endregion
        


        private void FrmMain_Load(object sender, EventArgs e)
        {
            #region 列出版本
            if (!Directory.Exists(".minecraft"))
            {
                if (Directory.Exists(@".minecraft\versions\"))
                {
                    MessageBox.Show("无法找到版本文件夹，本启动器只支持1.6以后的目录结构");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    return;
                }
            }
            DirectoryInfo mcdirinfo =new DirectoryInfo(".minecraft");
            DirectoryInfo[] versions = new DirectoryInfo(@".minecraft\versions").GetDirectories() ;
            foreach (DirectoryInfo version in versions)
            {
                VerList.Items.Add(version.Name);
            }
            VerList.Sorted = true;
            if (VerList.Items.Count != 0)
            {
                VerList.SelectedIndex = 0;
                listAuth.SelectedIndex = listAuth.Items.IndexOf(cfg.login);
            }
            else
            {
                buttonStart.Enabled = false;
                btmDelete.Enabled = false;
            }
            checkAutoStart.Checked = cfg.autostart;
            listAuth.SelectedIndex = 0;
            #endregion 

            #region 自动启动
            if (cfg.autostart&&startup)
            {
                StringBuilder JsonFilePath = new StringBuilder();
                JsonFilePath.Append(@".minecraft\versions\");
                JsonFilePath.Append(cfg.lastPlayVer);
                JsonFilePath.Append(@"\");
                JsonFilePath.Append(cfg.lastPlayVer);
                JsonFilePath.Append(".json");
                if (!File.Exists(JsonFilePath.ToString()))
                {
                    DirectoryInfo mcpath = new DirectoryInfo(Path.GetDirectoryName(JsonFilePath.ToString()));
                    bool find = false;
                    foreach (FileInfo js in mcpath.GetFiles())
                    {
                        if (js.FullName.Contains("json"))
                        {
                            JsonFilePath = new StringBuilder(js.FullName);
                            find = true;
                        }
                    }
                    if (!find)
                    {
                        MessageBox.Show("找不到版本所需的json文件");
                        Environment.Exit(2);
                    }
                }
                StreamReader JsonFile = new StreamReader(JsonFilePath.ToString());
                DataContractJsonSerializer InfoReader = new DataContractJsonSerializer(typeof(gameinfo));
                info = InfoReader.ReadObject(JsonFile.BaseStream) as gameinfo;
                JsonFile.Close();
                startGame starter = new startGame();
                starter.Show();
                if (changeEvent != null)
                    changeEvent("正在登陆");
                try
                {
                    if (listAuth.SelectedIndex != 0)
                    {
                        object Auth = Auths[Auths.Count - 1];
                        Type T = Auth.GetType();
                        MethodInfo Login = T.GetMethod("login");
                        string loginans;
                        try
                        {
                            loginans = Login.Invoke(Auth, new object[] { cfg.username, cfg.passwd }).ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        if (loginans == "True")
                        {
                            MethodInfo getSession = T.GetMethod("getsession");
                            session = getSession.Invoke(Auth, null).ToString();
                            MethodInfo getPname = T.GetMethod("getPname");
                            string username = getPname.Invoke(Auth, null).ToString();
                            try
                            {
                                game = new launcher(cfg.javaw, cfg.javaxmx, username, cfg.lastPlayVer, info, session);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("登录失败");
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            game = new launcher(cfg.javaw, cfg.javaxmx, cfg.username, cfg.lastPlayVer, info);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    try
                    {
                        bool start = game.start();
                        launcher.gameexit += launcher_gameexit;
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                finally
                {
                    starter.Close();
                }
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            #endregion
        }

        private void VerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VerList.SelectedIndex == -1)
            {
                VerList.SelectedIndex = 0;
            }
            StringBuilder JsonFilePath=new StringBuilder();
            JsonFilePath.Append(@".minecraft\versions\");
            JsonFilePath.Append(VerList.Items[VerList.SelectedIndex]);
            JsonFilePath.Append(@"\");
            JsonFilePath.Append(VerList.Items[VerList.SelectedIndex]);
            JsonFilePath.Append(".json");
            if (!File.Exists(JsonFilePath.ToString()))
            {
                DirectoryInfo mcpath=new DirectoryInfo(Path.GetDirectoryName(JsonFilePath.ToString()));
                bool find = false;
                foreach (FileInfo js in mcpath.GetFiles())
                {
                    if (js.FullName.Contains("json"))
                    {
                        JsonFilePath = new StringBuilder(js.FullName);
                        find = true;
                    }
                }
                if (!find)
                {
                    MessageBox.Show("找不到版本所需的json文件");
                    Environment.Exit(2);
                }
            }
            StreamReader JsonFile=new StreamReader(JsonFilePath.ToString());
            DataContractJsonSerializer InfoReader = new DataContractJsonSerializer(typeof(gameinfo));
            info = InfoReader.ReadObject(JsonFile.BaseStream) as gameinfo;
            JsonFile.Close();
            labVer.Text = info.id;
            labTime.Text = info.time;
            labReltime.Text = info.releaseTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofdlgJavaw.ShowDialog() == DialogResult.OK)
            {
                txtJavaw.Text = ofdlgJavaw.FileName;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
            startGame starter = new startGame();
            starter.Show();
            if (changeEvent!=null)
                changeEvent("正在登陆");
            try
            {
                if (listAuth.SelectedIndex != 0)
                {
                    object Auth = Auths[Auths.Count - 1];
                    Type T = Auth.GetType();
                    MethodInfo Login = T.GetMethod("login");
                    string loginans;
                    try
                    {
                        loginans = Login.Invoke(Auth, new object[] { txtUserName.Text, txtPwd.Text }).ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    if (loginans == "True")
                    {
                        cfg.username = txtUserName.Text;
                        cfg.passwd = Encoding.UTF8.GetBytes(txtPwd.Text);
                        cfg.javaxmx = txtJavaXmx.Text;
                        cfg.javaw = txtJavaw.Text;
                        cfg.login = listAuth.SelectedItem.ToString();
                        cfg.lastPlayVer = VerList.Items[VerList.SelectedIndex].ToString();
                        cfg.autostart = checkAutoStart.Checked;
                        MethodInfo getSession = T.GetMethod("getsession");
                        session = getSession.Invoke(Auth, null).ToString();
                        saveconfig();
                        MethodInfo getPname = T.GetMethod("getPname");
                        string username = getPname.Invoke(Auth, null).ToString();
                        try
                        {
                            game = new launcher(txtJavaw.Text, txtJavaXmx.Text, username, VerList.Items[VerList.SelectedIndex].ToString(), info, session);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("登录失败");
                        return;
                    }
                }
                else
                {
                    try
                    {
                        cfg.username = txtUserName.Text;
                        cfg.passwd = null;
                        cfg.javaxmx = txtJavaXmx.Text;
                        cfg.javaw = txtJavaw.Text;
                        cfg.login = listAuth.SelectedItem.ToString();
                        cfg.lastPlayVer = VerList.Items[VerList.SelectedIndex].ToString();
                        cfg.autostart = checkAutoStart.Checked;
                        saveconfig();
                        game = new launcher(txtJavaw.Text, txtJavaXmx.Text, txtUserName.Text, VerList.Items[VerList.SelectedIndex].ToString(), info);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                try
                {
                    bool start = game.start();
                    launcher.gameexit += launcher_gameexit;
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                if (cfg.autostart)
                {
                    icoBmcl.Visible = true;
                    icoBmcl.ShowBalloonTip(5000, "BMCL", "启动" + cfg.lastPlayVer + "成功", ToolTipIcon.Info);
                }
                starter.Close();
            }
        }

        private void launcher_gameexit()
        {
            if (!inscreen)
                Application.Exit();
        }

        private void buttonFlush_Click(object sender, EventArgs e)
        {
            listRemoteVer.Items.Clear();
            try
            {
                DataContractJsonSerializer RawJson = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(versions.RawVersionList));
                HttpWebRequest GetJson = (HttpWebRequest)WebRequest.Create("https://s3.amazonaws.com/Minecraft.Download/versions/versions.json");
                HttpWebResponse GetJsonAns = (HttpWebResponse)GetJson.GetResponse();
                RawVersionList RemoteVersion = RawJson.ReadObject(GetJsonAns.GetResponseStream()) as RawVersionList;
                foreach (RemoteVer RV in RemoteVersion.getVersions())
                {
                    ListViewItem AVer = listRemoteVer.Items.Add(RV.id);
                    AVer.SubItems.Add(RV.time);
                    AVer.SubItems.Add(RV.type);
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (cfg.login != null)
            {
                listAuth.SelectedIndex = listAuth.Items.IndexOf(cfg.login);
            }
            else
            {
                listAuth.SelectedIndex = 0;
            }
            if (cfg.lastPlayVer != null)
            {
                VerList.SelectedIndex = VerList.Items.IndexOf(cfg.lastPlayVer);
            }
            if (cfg.autostart&&startup)
            {
                startup = false;
                this.Hide();
            }
        }

        private void toolShow_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void icoBmcl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                inscreen = true;
                icoBmcl.Visible = false;
            }
            else
            {
                inscreen = false;
                icoBmcl.Visible = true;
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (listRemoteVer.SelectedItems == null)
            {
                MessageBox.Show("请先选择一个版本");
                return;
            }
            StringBuilder downpath = new StringBuilder(Environment.CurrentDirectory + @"\.minecraft\versions\");
            ListView.SelectedListViewItemCollection selectVer = listRemoteVer.SelectedItems;
            string selectver = selectVer[0].Text;
            downpath.Append(selectver).Append("\\");
            downpath.Append(selectver).Append(".jar");
            WebClient downer = new WebClient();
            StringBuilder downurl = new StringBuilder(URL_DOWNLOAD_BASE);
            downurl.Append(@"versions\");
            downurl.Append(selectver).Append("\\");
            downurl.Append(selectver).Append(".jar");
#if DEBUG
            MessageBox.Show(downpath.ToString()+"\n"+downurl.ToString());
#endif
            buttonDownload.Text = "下载中请稍候";
            buttonDownload.Refresh();
            if (!Directory.Exists(Path.GetDirectoryName(downpath.ToString())))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(downpath.ToString()));
            }
            string downjsonfile = downurl.ToString().Substring(0, downurl.Length - 4) + ".json";
            string downjsonpath = downpath.ToString().Substring(0, downpath.Length - 4) + ".json";
            try
            {
                downer.DownloadFileCompleted+= downer_DownloadClientFileCompleted;
                downer.DownloadProgressChanged += downer_DownloadProgressChanged;
                downer.DownloadFile(new Uri(downjsonfile), downjsonpath);
                downer.DownloadFileAsync(new Uri(downurl.ToString()), downpath.ToString());
                downedtime = Environment.TickCount - 1;
//                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                downed = 0;
                panelDownload.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\n"+ex.InnerException.Message);
            }

        }

        private void downer_DownloadClientFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("下载成功");
            buttonDownload.Text = "下载";
            refreshLocalVersion();
//            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            panelDownload.Visible = false;
            tabControl1.SelectTab(0);
        }

        private void refreshLocalVersion()
        {
            VerList.Items.Clear();
            if (!Directory.Exists(".minecraft"))
            {
                if (Directory.Exists(@".minecraft\versions\"))
                {
                    MessageBox.Show("无法找到版本文件夹，本启动器只支持1.6以后的目录结构");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    return;
                }
            }
            DirectoryInfo mcdirinfo = new DirectoryInfo(".minecraft");
            DirectoryInfo[] versions = new DirectoryInfo(@".minecraft\versions").GetDirectories();
            foreach (DirectoryInfo version in versions)
            {
                VerList.Items.Add(version.Name);
            }
            VerList.Sorted = true;
            if (VerList.Items.Count != 0)
            {
                VerList.SelectedIndex = 0;
                buttonStart.Enabled = true;
                btmDelete.Enabled = true;
            }
            else
            {
                buttonStart.Enabled = false;
                btmDelete.Enabled = false;
            }
            listAuth.SelectedIndex = 0;
        }

       
        private void buttonCheckRes_Click(object sender, EventArgs e)
        {
            frmCheckRes frmcheckres = new frmCheckRes();
            frmcheckres.ShowDialog();
        }



        #region Forge
        string lastforge;

        Hashtable DownloadUrl = new Hashtable();
        long downed = 0;
        long downedtime;

        private void buttonCopyInsPath_Click(object sender, EventArgs e)
        {
            txtInsPath.Text = Environment.CurrentDirectory + "\\.minecraft";
            Clipboard.SetText(Environment.CurrentDirectory + "\\.minecraft");
        }
        private void btnReForge_Click(object sender, EventArgs e)
        {
            txtInsPath.Text = Environment.CurrentDirectory + "\\.minecraft";
            WebBrowser ForgePageGet = new WebBrowser();
            ForgePageGet.Navigate("http://files.minecraftforge.net/");
            while (ForgePageGet.ReadyState!=WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            HtmlDocument ForgePage = ForgePageGet.Document;
            HtmlElement ForgePageBody = ForgePage.Body;
            HtmlElementCollection AllDiv = ForgePageBody.GetElementsByTagName("div");
            #region lastforge
            foreach (HtmlElement promotions in AllDiv)
            {
                if (promotions.GetElementsByTagName("h2").Count == 0)
                    continue;
                if (promotions.GetElementsByTagName("h2")[0].InnerText == "Promotions")
                {
                    HtmlElementCollection shortcuts = promotions.GetElementsByTagName("tr");
                    foreach (HtmlElement shortcut in shortcuts)
                    {
                        if (shortcut.GetElementsByTagName("td").Count == 0)
                        {
                            continue;
                        }
                        HtmlElementCollection geturl = shortcut.GetElementsByTagName("td")[4].GetElementsByTagName("a");
                        foreach (HtmlElement url in geturl)
                        {
                            if (url.InnerText == "*" && url.GetAttribute("href").IndexOf("installer") != -1)
                            {
                                TreeNode vernode = treeForgeVer.Nodes.Add(shortcut.GetElementsByTagName("td")[0].InnerText);
                                vernode.Nodes.Add(shortcut.GetElementsByTagName("td")[1].InnerText);
                                DownloadUrl.Add(shortcut.GetElementsByTagName("td")[0].InnerText, url.GetAttribute("href"));
                                if ("Latest" == shortcut.GetElementsByTagName("td")[0].InnerText)
                                {
                                    lastforge = url.GetAttribute("href");
                                    DownloadUrl.Add("lastest", url.GetAttribute("href"));
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            foreach (HtmlElement Build in AllDiv)
            {
                HtmlElementCollection title = Build.GetElementsByTagName("h2");
                foreach (HtmlElement t in title)
                {
                    if (t.InnerText == "All Downloads")
                    {
                        HtmlElementCollection ForgeVer = Build.GetElementsByTagName("tr");
                        for (int i = 1; i < ForgeVer.Count; i++)
                        {
                            TreeNode VerNode;
                            if (ForgeVer[i].GetElementsByTagName("th").Count != 0)
                            {
                                continue;
                            }
                            if (treeForgeVer.Nodes.Find(ForgeVer[i].GetElementsByTagName("td")[1].InnerText.Trim(), false).Length == 0)
                            {
                                VerNode = treeForgeVer.Nodes.Add(ForgeVer[i].GetElementsByTagName("td")[1].InnerText.Trim());
                                VerNode.Name = ForgeVer[i].GetElementsByTagName("td")[1].InnerText.Trim();
                                VerNode.Nodes.Add(ForgeVer[i].GetElementsByTagName("td")[0].InnerText.Trim());
                                HtmlElementCollection geturl = ForgeVer[i].GetElementsByTagName("td")[3].GetElementsByTagName("a");
                                foreach (HtmlElement url in geturl)
                                {
                                    if (url.InnerText == "*" && url.GetAttribute("href").IndexOf("installer") != -1)
                                    {
                                        DownloadUrl.Add(ForgeVer[i].GetElementsByTagName("td")[0].InnerHtml, url.GetAttribute("href"));
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                VerNode = treeForgeVer.Nodes.Find(ForgeVer[i].GetElementsByTagName("td")[1].InnerText, false)[0];
                                VerNode.Nodes.Add(ForgeVer[i].GetElementsByTagName("td")[0].InnerHtml);
                                HtmlElementCollection geturl = ForgeVer[i].GetElementsByTagName("td")[3].GetElementsByTagName("a");
                                foreach (HtmlElement url in geturl)
                                {
                                    if (url.InnerText == "*" && url.GetAttribute("href").IndexOf("installer") != -1)
                                    {
                                        DownloadUrl.Add(ForgeVer[i].GetElementsByTagName("td")[0].InnerHtml, url.GetAttribute("href"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void treeForgeVer_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeForgeVer.SelectedNode == null)
                return;
            try
            {
#if DEBUG
            if (this.treeForgeVer.SelectedNode.Level == 0)
                return;
            MessageBox.Show(DownloadUrl[this.treeForgeVer.SelectedNode.Text].ToString());
#endif  
            
                if (this.treeForgeVer.SelectedNode.Level == 0)
                    return;
                string downurl = DownloadUrl[this.treeForgeVer.SelectedNode.Text].ToString();
                WebClient downer = new WebClient();
                downer.DownloadFileCompleted += downer_DownloadFileCompleted;
                downer.DownloadProgressChanged += downer_DownloadProgressChanged;
                downer.DownloadFileAsync(new Uri(downurl), "forge-ins.jar");
                downedtime = Environment.TickCount - 1;
                downed = 0;
                panelDownload.Visible = true;
//                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

            }
            catch (NullReferenceException )
            {
                MessageBox.Show("目前仅支持带installer的Forge安装。早于1.6.1的Forge没有installer，1.6.1最早的两个版本没有installer。如果你选择的是这两者之外，请报告给我");
            }
        }

        private void buttonLastForge_Click(object sender, EventArgs e)
        {
            txtInsPath.Text = Environment.CurrentDirectory + "\\.minecraft";
            btnReForge.PerformClick();
            string downurl = DownloadUrl["lastest"].ToString();
            WebClient downer = new WebClient();
            downer.DownloadFileCompleted += downer_DownloadFileCompleted;
            downer.DownloadProgressChanged += downer_DownloadProgressChanged;
            downer.DownloadFileAsync(new Uri(downurl), "forge-ins.jar");
            downedtime = Environment.TickCount - 1;
//            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            downed = 0;
            panelDownload.Visible = true;
            //downer.DownloadFile(downurl, "forge-ins.jar");
        }

        void downer_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            prsDown.Maximum = (int)e.TotalBytesToReceive;
            prsDown.Value = (int)e.BytesReceived;
//            TaskbarManager.Instance.SetProgressValue((int)e.BytesReceived, (int)e.TotalBytesToReceive);
            StringBuilder info = new StringBuilder("速度：");
            try
            {
                info.Append(((double)(e.BytesReceived - downed) / (double)((Environment.TickCount - downedtime) / 1000)/1024.0).ToString("F2")).AppendLine("KB/s");
            }
            catch (DivideByZeroException) { info.AppendLine("0B/s"); }
            info.Append(e.ProgressPercentage.ToString()).AppendLine("%");
            labDownInfo.Text = info.ToString();
            
        }

        void downer_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
//            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Paused);
            Clipboard.SetText(txtInsPath.Text);
            MessageBox.Show("接下来弹出来的窗口里请选择路径为启动器这里的.minecraft目录。程序已经将目录复制到了剪贴板，直接在窗口里选择浏览，粘贴路径，确定即可");
            forge.writeprofile();
            Process ForgeIns = new Process();
            ForgeIns.StartInfo.FileName = cfg.javaw;
            ForgeIns.StartInfo.Arguments = "-jar " + Environment.CurrentDirectory + "\\forge-ins.jar";
            ForgeIns.Start();
            ForgeIns.WaitForExit();
            refreshLocalVersion();
            tabControl1.SelectTab(0);
            try
            {
                File.Delete("forge-ins.jar");
            }
            catch { }
//            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            
        }

        #endregion

        private void btmDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除当前版本？这个操作不能恢复，1.5.1之后的版本可去版本管理里重新下载", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) 
            {
                try
                {
                    FileStream Isused = File.OpenWrite(".minecraft\\versions\\" + VerList.Items[VerList.SelectedIndex] + "\\" + VerList.Items[VerList.SelectedIndex] + ".jar");
                    Isused.Close();
                    Directory.Delete(".minecraft\\versions\\" + VerList.Items[VerList.SelectedIndex], true);
                    if (Directory.Exists(".minecraft\\libraries\\"+VerList.Items[VerList.SelectedIndex]))
                    {
                        Directory.Delete(".minecraft\\libraries\\" + VerList.Items[VerList.SelectedIndex], true);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("删除失败，请检查该客户端是否正处于运行状态");
                }
                catch (IOException)
                {
                    MessageBox.Show("删除失败，请检查该客户端是否正处于运行状态");
                }
                finally
                {
                    refreshLocalVersion();
                }
            }
        }
        
        #region   拦截Windows消息
        protected override void WndProc(ref   Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE )
            {
                //捕捉关闭窗体消息      
                //   User   clicked   close   button    
                if (game == null)
                    Application.Exit();
                if (!game.IsRunning())
                    this.Close();
                this.Hide();
                return;
            }
            base.WndProc(ref   m);
        }
        #endregion  


        #region 导入导出
        private void btnImportOldVer_Click(object sender, EventArgs e)
        {
            if (folderImportOldVer.ShowDialog() == DialogResult.OK)
            {
                string ImportFrom = folderImportOldVer.SelectedPath;
                if (!File.Exists(ImportFrom + "\\bin\\minecraft.jar"))
                {
                    MessageBox.Show("未在该目录内发现有效的旧版Minecraft");
                    return;
                }
                string ImportName;
                bool F1 = false, F2 = false;
                ImportName = Microsoft.VisualBasic.Interaction.InputBox("输入导入后的名称", "导入旧版MC", "OldMinecraft");
                do
                {
                    F1 = false;
                    F2 = false;
                    if (ImportName.Length <= 0 || ImportName.IndexOf('.')!=-1)
                        ImportName = Microsoft.VisualBasic.Interaction.InputBox("输入导入后的名称", "输入无效，请不要带\".\"符号", "OldMinecraft");
                    else
                        F1 = true;
                    if (Directory.Exists(".minecraft\\versions\\" + ImportName))
                        ImportName = Microsoft.VisualBasic.Interaction.InputBox("输入导入后的名称", "版本已存在", "OldMinecraft");
                    else 
                        F2 = true;

                } while (!(F1 && F2));
                Directory.CreateDirectory(".minecraft\\versions\\" + ImportName);
                File.Copy(ImportFrom + "\\bin\\minecraft.jar", ".minecraft\\versions\\" + ImportName + "\\" + ImportName + ".jar");
                gameinfo info = new gameinfo();
                info.id = ImportName;
                string timezone = DateTimeOffset.Now.Offset.ToString();
                if (timezone[0] != '-')
                {
                    timezone = "+" + timezone;
                }
                info.time = DateTime.Now.GetDateTimeFormats('s')[0].ToString() + timezone;
                info.releaseTime = DateTime.Now.GetDateTimeFormats('s')[0].ToString() + timezone;
                info.type = "Port By BMCL";
                info.minecraftArguments = "${auth_player_name}";
                info.mainClass = "net.minecraft.client.Minecraft";
                ArrayList libs = new ArrayList();
                DirectoryInfo bin = new DirectoryInfo(ImportFrom + "\\bin");
                foreach (FileInfo file in bin.GetFiles("*.jar"))
                {
                    libraries.libraryies libfile = new libraries.libraryies();
                    if (file.Name == "minecraft.jar")
                        continue;
                    if (!Directory.Exists(".minecraft\\libraries\\" + ImportName + "\\" + file.Name.Substring(0, file.Name.Length - 4) + "\\BMCL\\"))
                    {
                        Directory.CreateDirectory(".minecraft\\libraries\\" + ImportName + "\\" + file.Name.Substring(0, file.Name.Length - 4) + "\\BMCL\\");
                    }
                    File.Copy(file.FullName, ".minecraft\\libraries\\" + ImportName + "\\" + file.Name.Substring(0,file.Name.Length-4) +"\\BMCL\\" + file.Name.Substring(0,file.Name.Length-4) +"-BMCL.jar");
                    libfile.name = ImportName + ":" + file.Name.Substring(0, file.Name.Length - 4) + ":BMCL";
                    libs.Add(libfile);
                }
                ICSharpCode.SharpZipLib.Zip.FastZip nativejar = new ICSharpCode.SharpZipLib.Zip.FastZip();
                if (!Directory.Exists(".minecraft\\libraries\\" + ImportName + "\\BMCL\\"))
                {
                    Directory.CreateDirectory(".minecraft\\libraries\\" + ImportName + "\\native\\BMCL\\");
                }
                nativejar.CreateZip(".minecraft\\libraries\\" + ImportName + "\\native\\BMCL\\native-BMCL-natives-windows.jar", ImportFrom + "\\bin\\natives", false, @"\.dll");
                libraries.libraryies nativefile = new libraries.libraryies();
                nativefile.name = ImportName + ":native:BMCL";
                libraries.OS nativeos = new libraries.OS();
                nativeos.windows = "natives-windows";
                nativefile.natives = nativeos;
                nativefile.extract = new libraries.extract();
                libs.Add(nativefile);
                info.libraries = (libraries.libraryies[])libs.ToArray(typeof(libraries.libraryies));
                FileStream wcfg = new FileStream(".minecraft\\versions\\" + ImportName + "\\" + ImportName + ".json", FileMode.Create);
                DataContractJsonSerializer infojson = new DataContractJsonSerializer(typeof(gameinfo));
                infojson.WriteObject(wcfg, info);
                wcfg.Close();
                if (Directory.Exists(ImportFrom + "\\lib"))
                {
                    if (!Directory.Exists(".minecraft\\lib"))
                    {
                        Directory.CreateDirectory(".minecraft\\lib");
                    }
                    foreach (string libfile in Directory.GetFiles(ImportFrom + "\\lib", "*", SearchOption.AllDirectories))
                    {
                        if (!File.Exists(".minecraft\\lib\\"+ Path.GetFileName(libfile)))
                        {
                            File.Copy(libfile, ".minecraft\\lib\\" + Path.GetFileName(libfile));
                        }
                    }
                }
                refreshLocalVersion();
            }
        }

        private void btmExportOfficial_Click(object sender, EventArgs e)
        {
            string dest = Environment.GetEnvironmentVariable("appdata") + @"\.minecraft\";
            if (Directory.Exists(dest + @"versions\" + VerList.Text))
            {
                if (MessageBox.Show("已存在同名版本，你确定要覆盖吗？","覆盖",MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            if (info.type == "Port By BMCL")
            {
                dircopy(@".minecraft\versions\" + VerList.Text, dest + @"versions\" + VerList.Text);
                dircopy(@".minecraft\libraries\" + VerList.Text, dest + @"libraries\" + VerList.Text);
                dircopy(@".minecraft\lib", dest + @"libs\");
                File.Delete(dest + @"versions\" + VerList.Text + "\\" + VerList.Text + ".json");
                gameinfo oinfo = info;
                oinfo.type = "release";
                FileStream wcfg = new FileStream(dest + @"versions\" + VerList.Text + "\\" + VerList.Text + ".json", FileMode.Create);
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(gameinfo));
                json.WriteObject(wcfg, oinfo);
                wcfg.Close();
            }
            else
            {
                dircopy(@".minecraft\versions\" + VerList.Text, dest + @"versions\" + VerList.Text);
                dircopy(@".minecraft\libraries\", dest + @"libraries\");
            }
            MessageBox.Show("导出成功");

        }
        #endregion

        private void btnPackUp_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            string PackUpFile;
            savePackUp.InitialDirectory = Environment.CurrentDirectory;
            savePackUp.FileName = VerList.Text;
            if (savePackUp.ShowDialog()==DialogResult.Cancel)
            {
                return;
            }
            else
            {
                PackUpFile=savePackUp.FileName;
            }
            string time = DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory("packup" + time + @"\.minecraft\versions\" + VerList.Text);
            dircopy(@".minecraft\versions\" + VerList.Text, "packup" + time + @"\.minecraft\versions\" + VerList.Text);
            Directory.CreateDirectory("packup" + time + @"\.minecraft\libraries\");
            string[] libpaths = readlibpaths(info);
            foreach (string filename in libpaths)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft"))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft")));
                }
                File.Copy(filename, filename.Replace(".minecraft", "packup" + time + @"\.minecraft"));
            }
            string[] nativepaths = readnativepaths(info);
            foreach (string filename in nativepaths)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft"))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft")));
                }
                File.Copy(filename, filename.Replace(".minecraft", "packup" + time + @"\.minecraft"));
            }
            File.Copy(Application.ExecutablePath.ToLower(), "packup" + time + "\\" + Path.GetFileName(Application.ExecutablePath).ToLower());
            FileStream wcfg = new FileStream("packup" + time + "\\bmcl.xml",FileMode.Create);
            DataContractSerializer Cfg = new DataContractSerializer(typeof(config));
            config tempcfg = cfg;
            tempcfg.autostart = true;
            tempcfg.lastPlayVer = info.id;
            Cfg.WriteObject(wcfg, tempcfg);
            wcfg.Close();
            ICSharpCode.SharpZipLib.Zip.FastZip output = new ICSharpCode.SharpZipLib.Zip.FastZip();
            output.CreateZip(PackUpFile, "packup" + time, true, "");
            Directory.Delete("packup" + time, true);
            DateTime end = DateTime.Now;
            StringBuilder mbox = new StringBuilder();
            mbox.AppendLine("打包成功");
            mbox.Append("文件大小：").Append(((new FileInfo(PackUpFile)).Length / 1024.0 / 1024.0).ToString("f2")).AppendLine("MB");
            mbox.Append("耗时").Append((end - start).TotalSeconds.ToString("f2")).AppendLine("秒");
            MessageBox.Show(mbox.ToString());
        }


    }
}
