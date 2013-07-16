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
using System.Net.Sockets;

using bmcl.versions;
using bmcl.util;

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
                StreamReader rCfg = new StreamReader(cfgfile);
                try
                {
                    cfg = Cfg.ReadObject(rCfg.BaseStream) as config;
                    rCfg.Close();
                }
                catch
                {
                    rCfg.Close();
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
            txtExtJArg.Text = cfg.extraJVMArg;
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
            AuthList = listAuth;
            #endregion
        }


        #region 属性
        static int LauncherVer = 3;
        static private string cfgfile = "bmcl.xml";
        public static String URL_DOWNLOAD_BASE = "https://s3.amazonaws.com/Minecraft.Download/";
	    public static String URL_RESOURCE_BASE = "https://s3.amazonaws.com/Minecraft.Resources/";
        private ArrayList Auths=new ArrayList();
        public static config cfg;
        static DataContractSerializer Cfg = new DataContractSerializer(typeof(config));
        static public gameinfo info;
        string session;
        bool startup = true;
        launcher game;
        bool inscreen;
        static public ListBox AuthList;
        static public string portinfo = "Port By BMCL";

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
        static public ListBox GetlistAuth()
        {
            return AuthList;
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
                    btmExportOfficial.Enabled = false;
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    btmExportOfficial.Enabled = false;
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
                btmExportOfficial.Enabled = false;
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
                if (cfg.javaw == "自动寻找")
                {
                    cfg.javaw = config.getjavadir();
                    if (cfg.javaw == null)
                    {
                        MessageBox.Show("自动寻找java失败，请手动寻找");
                        if (ofdlgJavaw.ShowDialog() == DialogResult.OK)
                        {
                            cfg.javaw = ofdlgJavaw.FileName;
                        }
                    }
                }

                frmPrs starter = new frmPrs("正在准备游戏环境及启动游戏");
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
                                game = new launcher(cfg.javaw, cfg.javaxmx, username, cfg.lastPlayVer, info, cfg.extraJVMArg, session);
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
                            game = new launcher(cfg.javaw, cfg.javaxmx, cfg.username, cfg.lastPlayVer, info, cfg.extraJVMArg);
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
            JsonFilePath.Append(VerList.Text);
            JsonFilePath.Append(@"\");
            JsonFilePath.Append(VerList.Text);
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
            labType.Text = info.type;
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
            
            frmPrs starter = new frmPrs("正在准备游戏环境及启动游戏");
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
                        Exception exc = ex;
                        while (exc.InnerException != null)
                        {
                            exc = exc.InnerException;
                        }
                        MessageBox.Show(exc.Message);
                        return;
                    }
                    if (loginans == "True")
                    {
                        cfg.username = txtUserName.Text;
                        cfg.passwd = Encoding.UTF8.GetBytes(txtPwd.Text);
                        cfg.javaxmx = txtJavaXmx.Text;
                        cfg.javaw = txtJavaw.Text;
                        cfg.login = listAuth.SelectedItem.ToString();
                        cfg.lastPlayVer = VerList.Text;
                        cfg.autostart = checkAutoStart.Checked;
                        cfg.extraJVMArg = txtExtJArg.Text;
                        MethodInfo getSession = T.GetMethod("getsession");
                        session = getSession.Invoke(Auth, null).ToString();
                        saveconfig();
                        MethodInfo getPname = T.GetMethod("getPname");
                        string username = getPname.Invoke(Auth, null).ToString();
                        try
                        {
                            game = new launcher(txtJavaw.Text, txtJavaXmx.Text, username, VerList.Text, info, txtExtJArg.Text, session);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("登录失败，用户名或密码错误");
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
                        cfg.lastPlayVer = VerList.Text;
                        cfg.autostart = checkAutoStart.Checked;
                        cfg.extraJVMArg = txtExtJArg.Text;
                        saveconfig();
                        game = new launcher(txtJavaw.Text, txtJavaXmx.Text, txtUserName.Text, VerList.Text.ToString(), info, txtExtJArg.Text);
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
                    icoBmcl.ShowBalloonTip(10000, "BMCL", "启动" + cfg.lastPlayVer + "成功", ToolTipIcon.Info);
                }
                starter.Close();
            }
        }

        private void launcher_gameexit()
        {
            if (!inscreen)
            {
                Application.Exit();
            }
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
                    btmExportOfficial.Enabled = false;
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
                    btmDelete.Enabled = false;
                    btmExportOfficial.Enabled = false;
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
                btmExportOfficial.Enabled = true;
            }
            else
            {
                buttonStart.Enabled = false;
                btmDelete.Enabled = false;
                btmExportOfficial.Enabled = false;
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
            DownloadUrl.Clear();
            treeForgeVer.Nodes.Clear();
            txtInsPath.Text = Environment.CurrentDirectory + "\\.minecraft";
            WebBrowser ForgePageGet = new WebBrowser();
            ForgePageGet.Navigate("http://files.minecraftforge.net/");
            TimeSpan start=new TimeSpan(DateTime.Now.Ticks);
            while (ForgePageGet.ReadyState!=WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
                if (new TimeSpan(DateTime.Now.Ticks) - start>new TimeSpan(0,0,10))
                {
                    if (sender == null)
                        throw new TimeoutException("访问Forge服务器超时");
                    else
                    {
                        MessageBox.Show("访问Forge服务器超时");
                        return;
                    }
                }
            }
            if (ForgePageGet.Url.ToString().IndexOf("114so") != -1)
            {
                if (sender == null)
                    throw new TimeoutException("访问Forge服务器失败");
                else
                {
                    MessageBox.Show("访问Forge服务器失败");
                    return;
                }
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
            DownloadUrl.Clear();
            treeForgeVer.Nodes.Clear();
            txtInsPath.Text = Environment.CurrentDirectory + "\\.minecraft";
            try
            {
                btnReForge_Click(null, null);
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
            this.panelDownload.Visible = false;
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
                    FileStream Isused = File.OpenWrite(".minecraft\\versions\\" + VerList.Text + "\\" + info.id + ".jar");
                    Isused.Close();
                    Directory.Delete(".minecraft\\versions\\" + VerList.Text, true);
                    if (Directory.Exists(".minecraft\\libraries\\" + VerList.Text))
                    {
                        Directory.Delete(".minecraft\\libraries\\" + VerList.Text, true);
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
        
#if RELEASE
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
#endif

        #region 导入导出
        private void btnImportOldVer_Click(object sender, EventArgs e)
        {
            frmPrs prs = new frmPrs("正在导入Minecraft");
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
                prs.Show();
                changeEvent("导入主程序");
                Directory.CreateDirectory(".minecraft\\versions\\" + ImportName);
                File.Copy(ImportFrom + "\\bin\\minecraft.jar", ".minecraft\\versions\\" + ImportName + "\\" + ImportName + ".jar");
                changeEvent("创建Json");
                gameinfo info = new gameinfo();
                info.id = ImportName;
                string timezone = DateTimeOffset.Now.Offset.ToString();
                if (timezone[0] != '-')
                {
                    timezone = "+" + timezone;
                }
                info.time = DateTime.Now.GetDateTimeFormats('s')[0].ToString() + timezone;
                info.releaseTime = DateTime.Now.GetDateTimeFormats('s')[0].ToString() + timezone;
                info.type = portinfo;
                info.minecraftArguments = "${auth_player_name}";
                info.mainClass = "net.minecraft.client.Minecraft";
                changeEvent("处理native");
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
                changeEvent("写入Json");
                FileStream wcfg = new FileStream(".minecraft\\versions\\" + ImportName + "\\" + ImportName + ".json", FileMode.Create);
                DataContractJsonSerializer infojson = new DataContractJsonSerializer(typeof(gameinfo));
                infojson.WriteObject(wcfg, info);
                wcfg.Close();
                changeEvent("处理lib");
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
                changeEvent("处理mods");
                if (Directory.Exists(ImportFrom + "\\mods"))
                    dircopy(ImportFrom + "\\mods", ".minecraft\\versions\\" + ImportName + "\\mods");
                else
                    Directory.CreateDirectory(".minecraft\\versions\\" + ImportName + "\\mods");
                if (Directory.Exists(ImportFrom + "\\coremods"))
                    dircopy(ImportFrom + "\\coremods", ".minecraft\\versions\\" + ImportName + "\\coremods");
                else
                    Directory.CreateDirectory(".minecraft\\versions\\" + ImportName + "\\coremods");
                if (Directory.Exists(ImportFrom + "\\config"))
                    dircopy(ImportFrom + "\\config", ".minecraft\\versions\\" + ImportName + "\\config");
                else
                    Directory.CreateDirectory(".minecraft\\versions\\" + ImportName + "\\configmods");
                prs.Close();
                MessageBox.Show("导入成功，如果这个版本的MC还有MOD在.minecraft下创建了文件夹（例如Flan's mod,Custom NPC等），请点击MOD独立文件夹按钮进行管理");
                refreshLocalVersion();
            }
        }

        private void btmExportOfficial_Click(object sender, EventArgs e)
        {
            string dest = Environment.GetEnvironmentVariable("appdata") + @"\.minecraft\";
            if (Directory.Exists(dest + @"versions\" + info.id))
            {
                if (MessageBox.Show("已存在同名版本，你确定要覆盖吗？","覆盖",MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            if (info.type == "Port By BMCL")
            {
                dircopy(@".minecraft\versions\" + info.id, dest + @"versions\" + info.id);
                dircopy(@".minecraft\libraries\" + info.id, dest + @"libraries\" + info.id);
                dircopy(@".minecraft\lib", dest + @"libs\");
                File.Delete(dest + @"versions\" + info.id + "\\" + info.id + ".json");
                gameinfo oinfo = info;
                oinfo.type = "release";
                FileStream wcfg = new FileStream(dest + @"versions\" + info.id + "\\" + info.id + ".json", FileMode.Create);
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(gameinfo));
                json.WriteObject(wcfg, oinfo);
                wcfg.Close();
            }
            else
            {
                dircopy(@".minecraft\versions\" + info.id, dest + @"versions\" + info.id);
                dircopy(@".minecraft\libraries\", dest + @"libraries\");
            }
            MessageBox.Show("导出成功");

        }
        #endregion

        private void btnPackUp_Click(object sender, EventArgs e)
        {
            frmPackUp frmpackup = new frmPackUp();
            frmpackup.ShowDialog();
        }

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            try
            {
                string rname = Microsoft.VisualBasic.Interaction.InputBox("新名字", "重命名", VerList.Text);
                if (rname == "") return;
                if (rname == VerList.Text) return;
                if (VerList.Items.IndexOf(rname) != -1) throw new Exception("这个版本已经存在");
                Directory.Move(".minecraft\\versions\\" + VerList.Text, ".minecraft\\versions\\" + rname);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("重命名失败，请检查客户端是否开启");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                refreshLocalVersion();
            }
        }

#region mod管理
        private void btnMod_Click(object sender, EventArgs e)
        {
            StringBuilder modpath = new StringBuilder(@".minecraft\versions\");
            modpath.Append(VerList.Text).Append("\\");
            StringBuilder configpath = new StringBuilder(modpath.ToString());
            StringBuilder coremodpath = new StringBuilder(modpath.ToString());
            StringBuilder moddirpath = new StringBuilder(modpath.ToString());
            modpath.Append("mods");
            configpath.Append("config");
            coremodpath.Append("coremods");
            moddirpath.Append("moddir");
            if (!Directory.Exists(modpath.ToString()))
            {
                Directory.CreateDirectory(modpath.ToString());
            }
            if (!Directory.Exists(configpath.ToString()))
            {
                Directory.CreateDirectory(configpath.ToString());
            }
            if (!Directory.Exists(coremodpath.ToString()))
            {
                Directory.CreateDirectory(coremodpath.ToString());
            }
            if (!Directory.Exists(moddirpath.ToString()))
            {
                Directory.CreateDirectory(moddirpath.ToString());
            }
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer.exe";
            explorer.StartInfo.Arguments = modpath.ToString();
            explorer.Start();



        }

        private void btnModConfig_Click(object sender, EventArgs e)
        {
            StringBuilder modpath = new StringBuilder(@".minecraft\versions\");
            modpath.Append(VerList.Text).Append("\\");
            StringBuilder configpath = new StringBuilder(modpath.ToString());
            StringBuilder coremodpath = new StringBuilder(modpath.ToString());
            StringBuilder moddirpath = new StringBuilder(modpath.ToString());
            modpath.Append("mods");
            configpath.Append("config");
            coremodpath.Append("coremods");
            moddirpath.Append("moddir");
            if (!Directory.Exists(modpath.ToString()))
            {
                Directory.CreateDirectory(modpath.ToString());
            }
            if (!Directory.Exists(configpath.ToString()))
            {
                Directory.CreateDirectory(configpath.ToString());
            }
            if (!Directory.Exists(coremodpath.ToString()))
            {
                Directory.CreateDirectory(coremodpath.ToString());
            }
            if (!Directory.Exists(moddirpath.ToString()))
            {
                Directory.CreateDirectory(moddirpath.ToString());
            }
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer.exe";
            explorer.StartInfo.Arguments = configpath.ToString();
            explorer.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StringBuilder modpath = new StringBuilder(@".minecraft\versions\");
            modpath.Append(VerList.Text).Append("\\");
            StringBuilder configpath = new StringBuilder(modpath.ToString());
            StringBuilder coremodpath = new StringBuilder(modpath.ToString());
            StringBuilder moddirpath = new StringBuilder(modpath.ToString());
            modpath.Append("mods");
            configpath.Append("config");
            coremodpath.Append("coremods");
            moddirpath.Append("moddir");
            if (!Directory.Exists(modpath.ToString()))
            {
                Directory.CreateDirectory(modpath.ToString());
            }
            if (!Directory.Exists(configpath.ToString()))
            {
                Directory.CreateDirectory(configpath.ToString());
            }
            if (!Directory.Exists(coremodpath.ToString()))
            {
                Directory.CreateDirectory(coremodpath.ToString());
            }
            if (!Directory.Exists(moddirpath.ToString()))
            {
                Directory.CreateDirectory(moddirpath.ToString());
            }
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer.exe";
            explorer.StartInfo.Arguments = coremodpath.ToString();
            explorer.Start();
        }

        private void btnModDir_Click(object sender, EventArgs e)
        {
            StringBuilder modpath = new StringBuilder(@".minecraft\versions\");
            modpath.Append(VerList.Text).Append("\\");
            StringBuilder configpath = new StringBuilder(modpath.ToString());
            StringBuilder coremodpath = new StringBuilder(modpath.ToString());
            StringBuilder moddirpath = new StringBuilder(modpath.ToString());
            modpath.Append("mods");
            configpath.Append("config");
            coremodpath.Append("coremods");
            moddirpath.Append("moddir");
            if (!Directory.Exists(modpath.ToString()))
            {
                Directory.CreateDirectory(modpath.ToString());
            }
            if (!Directory.Exists(configpath.ToString()))
            {
                Directory.CreateDirectory(configpath.ToString());
            }
            if (!Directory.Exists(coremodpath.ToString()))
            {
                Directory.CreateDirectory(coremodpath.ToString());
            }
            if (!Directory.Exists(moddirpath.ToString()))
            {
                Directory.CreateDirectory(moddirpath.ToString());
            }
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer.exe";
            explorer.StartInfo.Arguments = moddirpath.ToString();
            explorer.Start();
        }

        private void btnModDir_MouseEnter(object sender, EventArgs e)
        {
            tip.Show("如果MOD在.minecraft下创建了文件夹（例如Flan's mod,Custom NPC等），请使用这个管理", btnModDir);
        }
        private void btnModDir_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(btnModDir);
        }
#endregion

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            cfg.autostart = checkAutoStart.Checked;
            cfg.extraJVMArg = txtExtJArg.Text;
            cfg.javaw = txtJavaw.Text;
            cfg.javaxmx = txtJavaXmx.Text;
            cfg.lastPlayVer = cfg.lastPlayVer;
            cfg.login = listAuth.Text;
            cfg.passwd = Encoding.UTF8.GetBytes(txtPwd.Text);
            cfg.username = txtUserName.Text;
            saveconfig();
        }

        #region 服务器列表
        private serverlist.serverlist sl = new serverlist.serverlist();
        private void btnReflushServer_Click(object sender, EventArgs e)
        {
            listServer.Items.Clear();
            if (File.Exists(@".minecraft\servers.dat"))
            {
                foreach (serverlist.serverinfo info in sl.info)
                {
                    DateTime start = DateTime.Now;
                    ListViewItem server = listServer.Items.Add(info.Name);
                    server.SubItems.Add(info.IsHide ? "是" : "否");
                    if (info.IsHide)
                        server.SubItems.Add(string.Empty);
                    else
                        server.SubItems.Add(info.Address);
                    try
                    {
                        Socket con = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        con.ReceiveTimeout = 3000;
                        con.SendTimeout = 3000;
                        if (info.Address.Split(':').Length == 1)
                            con.Connect(Dns.GetHostAddresses(info.Address.Split(':')[0]), 25565);
                        else
                            con.Connect(Dns.GetHostAddresses(info.Address.Split(':')[0]), int.Parse(info.Address.Split(':')[1]));
                        con.Send(new byte[1] { 254 });
                        con.Send(new byte[1] { 1 });
                        byte[] recive = new byte[512];
                        int bytes = con.Receive(recive);
                        if (recive[0] != 255)
                        {
                            throw new Exception("服务器回复无效");
                        }
                        string message = Encoding.UTF8.GetString(recive, 4, bytes - 4);
                        StringBuilder remessage = new StringBuilder(30);
                        for (int i = 0; i <= message.Length; i += 2)
                        {
                            remessage.Append(message[i]);
                        }
                        message = remessage.ToString();
                        con.Shutdown(SocketShutdown.Both);
                        con.Close();
                        DateTime end = DateTime.Now;
                        char[] achar = message.ToCharArray();

                        for (int i = 0; i < achar.Length; ++i)
                        {
                            if (achar[i] != 167 && achar[i] != 0 && char.IsControl(achar[i]))
                            {
                                achar[i] = (char)63;
                            }
                        }
                        message = new String(achar);
                        if (message[0] == (char)253 || message[0] == (char)65533)
                        {
                            message = (char)167 + message.Substring(1);
                        }
                        string[] astring;
                        if (message.StartsWith("\u00a7") && message.Length > 1)
                        {
                            astring = message.Substring(1).Split('\0');
                            if (MathHelper.parseIntWithDefault(astring[0], 0) == 1)
                            {
                                server.SubItems.Add(astring[3]);
                                server.SubItems.Add(astring[2]);
                                int online = MathHelper.parseIntWithDefault(astring[4], 0);
                                int maxplayer = MathHelper.parseIntWithDefault(astring[5], 0);
                                server.SubItems.Add(online + "/" + maxplayer);
                            }
                        }
                        else
                        {
                            server.SubItems.AddRange(new string[] { " ", " " });
                        }
                        server.SubItems.Add((end - start).Milliseconds + " ms");
                        if (((end - start).Milliseconds < 200))
                        {
                            server.SubItems[0].ForeColor = Color.Green;
                        }else
                        if (((end - start).Milliseconds < 500))
                        {
                            server.SubItems[0].ForeColor = Color.Blue;
                        }else
                        if (((end - start).Milliseconds < 1000))
                        {
                            server.SubItems[0].ForeColor = Color.Yellow;
                        }else
                        if (((end - start).Milliseconds < 3000))
                        {
                            server.SubItems[0].ForeColor = Color.Orange;
                        }else
                        if (((end - start).Milliseconds > 3000))
                        {
                            server.SubItems[0].ForeColor = Color.OrangeRed;
                        }
                    }
                    catch (SocketException ex)
                    {
                        server.SubItems.AddRange(new string[] { " ", " ", " "});
                        server.SubItems.Add("连接失败" + ex.Message);
                        server.SubItems[0].ForeColor = Color.Red;
                    }
                    catch (Exception ex)
                    {
                        server.SubItems.AddRange(new string[] { " ", " ", " "});
                        server.SubItems.Add("无法识别的服务器" + ex.Message);
                        server.SubItems[0].ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                if (MessageBox.Show("服务器列表找不到，是否创建？","找不到文件",MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    FileStream serverdat = new FileStream(@".minecraft\servers.dat", FileMode.Create);
                    serverdat.Write(Convert.FromBase64String(resource.ServerDat.Header),0,Convert.FromBase64String(resource.ServerDat.Header).Length);
                    serverdat.WriteByte(0);
                    serverdat.Close();
                }
            }
        }

        private void btnEditServer_Click(object sender, EventArgs e)
        {
            try
            {
                serverlist.AddServer FrmEdit = new serverlist.AddServer(ref sl, this.listServer.SelectedIndices[0]);
                if (FrmEdit.ShowDialog() == DialogResult.OK)
                {
                    sl.Write();
                    btnReflushServer.PerformClick();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("请先选择一个服务器");
            }
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            serverlist.AddServer FrmAdd = new serverlist.AddServer(ref sl);
            if (FrmAdd.ShowDialog() == DialogResult.OK)
            {
                sl.Write();
                btnReflushServer.PerformClick();
            }

        }

        private void btnDeleteServer_Click(object sender, EventArgs e)
        {
            try
            {
                sl.Delete(listServer.SelectedIndices[0]);
                sl.Write();
                btnReflushServer.PerformClick();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("请先选择一个服务器");
            }
        }
        #endregion



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabControl1.TabPages["tabServerList"]) 
            {
                btnReflushServer.PerformClick();
            }
            if (this.tabControl1.SelectedTab == this.tabControl1.TabPages["tabForge"])
            {
                btnReForge.PerformClick();
            }
            if (this.tabControl1.SelectedTab==this.tabControl1.TabPages["tabVersion"])
            {
                buttonFlush.PerformClick();
            }
        }







    }
}
