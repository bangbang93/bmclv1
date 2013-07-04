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

using bmcl.versions;

namespace bmcl
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
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
        public event statuschange changeEvent;
        #endregion


        #region 委托
        public delegate void statuschange(string status);
        #endregion

        #region 公共方法
        static public void saveconfig()
        {
            StreamWriter wCfg = new StreamWriter(cfgfile);
            Cfg.WriteObject(wCfg.BaseStream, cfg);
            wCfg.Close();
        }
        #endregion
        


        private void FrmMain_Load(object sender, EventArgs e)
        {
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
                                    object Auth = Auths[Auths.Count-1];
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
            if (cfg.passwd!=null)
                txtPwd.Text = Encoding.UTF8.GetString(cfg.passwd);
            txtJavaXmx.Text = cfg.javaxmx;
            #endregion
            #region 列出版本
            if (!Directory.Exists(".minecraft"))
            {
                if (Directory.Exists(@".minecraft\versions\"))
                {
                    MessageBox.Show("无法找到版本文件夹，本启动器只支持1.6以后的目录结构");
                    buttonStart.Enabled = false;
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
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
                buttonStart.Enabled = false;
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
            StringBuilder JsonFilePath=new StringBuilder();
            JsonFilePath.Append(@".minecraft\versions\");
            JsonFilePath.Append(VerList.SelectedItem.ToString());
            JsonFilePath.Append(@"\");
            JsonFilePath.Append(VerList.SelectedItem.ToString());
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
            toolShow.PerformClick();
        }

        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                inscreen = true;
            }
            else
            {
                inscreen = false;
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
                downer.DownloadFile(downurl.ToString(), downpath.ToString());
                downer.DownloadFile(downjsonfile, downjsonpath);
                MessageBox.Show("下载成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\n"+ex.InnerException.Message);
            }
            finally
            {
                buttonDownload.Text = "下载";
                refreshLocalVersion();
            }

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
                    return;
                }
                else
                {
                    MessageBox.Show("无法找到游戏文件夹");
                    buttonStart.Enabled = false;
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
            }
            else
                buttonStart.Enabled = false;
            listAuth.SelectedIndex = 0;
        }
    }
}
