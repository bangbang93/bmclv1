using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Reflection;

namespace bmcl
{
    public partial class frmPackUp : Form
    {
        public frmPackUp()
        {
            InitializeComponent();
            info = FrmMain.info.clone();
            cfg = FrmMain.cfg.clone();
        }
        readonly gameinfo info = new gameinfo();
        readonly config cfg = new config();
        private ArrayList Auths = new ArrayList();

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
                if (lib.natives == null)
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
                if (lib.natives != null)
                    nativepaths.Add(launcher.buildNativePath(lib));
            }
            return (string[])nativepaths.ToArray(typeof(string));
        }
        /// <summary>
        /// 文件夹递归复制
        /// </summary>
        /// <param name="from">源</param>
        /// <param name="to">目标</param>
        static public void dircopy(string from, string to)
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


        private void btnStart_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            string PackUpFile;
            savePackUp.InitialDirectory = Environment.CurrentDirectory;
            savePackUp.FileName = info.id;
            if (savePackUp.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                PackUpFile = savePackUp.FileName;
            }
            string time = DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory("packup" + time + @"\.minecraft\versions\" + info.id);
            dircopy(@".minecraft\versions\" + info.id, "packup" + time + @"\.minecraft\versions\" + info.id);//主程序和MOD
            if (checkLibNat.Checked)
            {
                Directory.CreateDirectory("packup" + time + @"\.minecraft\libraries\");
                string[] libpaths = readlibpaths(info);
                try
                {
                    foreach (string filename in libpaths)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft"))))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft")));
                        }
                        File.Copy(filename, filename.Replace(".minecraft", "packup" + time + @"\.minecraft"));
                    }//lib
                    string[] nativepaths = readnativepaths(info);
                    foreach (string filename in nativepaths)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft"))))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filename.Replace(".minecraft", "packup" + time + @"\.minecraft")));
                        }
                        File.Copy(filename, filename.Replace(".minecraft", "packup" + time + @"\.minecraft"));
                    }//native
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show("依赖文件未找到，请先运行游戏，以确保依赖文件完整性"+ex.Message);
                    Directory.Delete("packup" + time, true);
                    return;
                }
            }
            File.Copy(Application.ExecutablePath.ToLower(), "packup" + time + "\\" + Path.GetFileName(Application.ExecutablePath).ToLower());
            File.Copy("AuthMothed.dll", "packup" + time + "\\AuthMothed.dll");
            dircopy("auths", "packup" + time + "\\auths");
            //loginAuthMethod
            if (checkRes.Checked)
            {
                if (!Directory.Exists(".minecraft\\assets"))
                {
                    MessageBox.Show("请先去版本管理页面下载资源文件");
                    Directory.Delete("packup" + time, true);
                    return;
                }
                dircopy(".minecraft\\assets","packup" + time + "\\.minecraft\\assets");
            }
            //Resource
            FileStream wcfg = new FileStream("packup" + time + "\\bmcl.xml", FileMode.Create);
            DataContractSerializer Cfg = new DataContractSerializer(typeof(config));
            config tempcfg = cfg.clone(); ;
            tempcfg.autostart = true;
            tempcfg.lastPlayVer = info.id;
            Cfg.WriteObject(wcfg, tempcfg);
            wcfg.Close();
            //config
            ICSharpCode.SharpZipLib.Zip.FastZip output = new ICSharpCode.SharpZipLib.Zip.FastZip();
            output.CreateZip(PackUpFile, "packup" + time, true, "");
            Directory.Delete("packup" + time, true);
            DateTime end = DateTime.Now;
            StringBuilder mbox = new StringBuilder();
            mbox.AppendLine("打包成功");
            mbox.Append("文件大小：").Append(((new FileInfo(PackUpFile)).Length / 1024.0 / 1024.0).ToString("f2")).AppendLine("MB");
            mbox.Append("耗时").Append((end - start).TotalSeconds.ToString("f2")).AppendLine("秒");
            MessageBox.Show(mbox.ToString());
            this.Close();
        }

        private void frmPackUp_Load(object sender, EventArgs e)
        {
            txtJavaw.Text = cfg.javaw;
            txtUserName.Text = cfg.username;
            if (info.type == FrmMain.portinfo)
            {
                checkLibNat.Checked = true;
                checkLibNat.Enabled = false;
                checkRes.Checked = true;
                checkRes.Enabled = false;
                labINFO.Visible = true;
            }
            if (cfg.passwd != null)
                txtPwd.Text = Encoding.UTF8.GetString(cfg.passwd);
            txtJavaXmx.Text = cfg.javaxmx;
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

        private void checkAutoJava_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkAutoJava.Checked == true)
            {
                txtJavaw.Text = "自动寻找";
                cfg.javaw = "自动寻找";
                txtJavaw.Enabled = false;
                buttonJavaw.Enabled = false;
            }
            else
            {
                txtJavaw.Text = FrmMain.cfg.javaw;
                cfg.javaw = FrmMain.cfg.javaw;
                txtJavaw.Enabled = true;
                buttonJavaw.Enabled = true;
            }
        }
        #region 配置变更
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            cfg.username = this.txtUserName.Text;
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            cfg.passwd = Encoding.UTF8.GetBytes(txtPwd.Text);
        }

        private void txtJavaw_TextChanged(object sender, EventArgs e)
        {
            cfg.javaw = this.txtJavaw.Text;
        }

        private void txtJavaXmx_TextChanged(object sender, EventArgs e)
        {
            cfg.javaxmx = txtJavaXmx.Text;
        }

        private void checkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            cfg.autostart = checkAutoStart.Checked;
        }
        #endregion 
        private void frmPackUp_Shown(object sender, EventArgs e)
        {
            listAuth.Text = cfg.login;
        }
    }
}
