using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using download;

namespace bmcl
{
    class launcher
    {

        #region 异常
        public static Exception NoJava = new Exception("找不到java");
        public static Exception NoRam = new Exception("没有足够物理内存");
        public static Exception NoMoreRam = new Exception("没有足够的可用内存");
        public static Exception UnSupportVer = new Exception("启动器不支持这个版本");
        public static Exception FailInLib = new Exception("无法获得所需的依赖");
        #endregion


        #region 属性
        private Process game = new Process();
        private string java = "";
        private string javaxmx = "";
        private string username = "";
        private string version;
        private gameinfo Info;
        long timestamp = DateTime.Now.Ticks;
        downloader downloadJob;


        #endregion 

        #region 委托
        delegate void downThread();
        public delegate void changeHandel(string status);
        public delegate void gameExitDel();
        #endregion


        #region 事件
        public static event changeHandel changeEvent;
        public static event gameExitDel gameexit;
        #endregion


        #region 方法
        /// <summary>
        /// 初始化启动器
        /// </summary>
        /// <param name="JavaPath"></param>
        /// <param name="JavaXmx"></param>
        /// <param name="UserName"></param>
        /// <param name="ver"></param>
        /// <param name="info"></param>
        /// <param name="session"></param>
        public launcher(string JavaPath, string JavaXmx, string UserName,string ver,gameinfo info,string session="no")
        {
            changeEvent("检查Java");
            if (!File.Exists(JavaPath))
            {
                throw NoJava;
            }
            changeEvent("检查内存");
            if (Convert.ToUInt64(JavaXmx) < 512M)
            {
                throw NoRam;
            }
            java = JavaPath;
            javaxmx = JavaXmx;
            username = UserName;
            version = ver;
            game.StartInfo.FileName = java;
            Info = info;
            changeEvent("设置环境变量");
            StringBuilder arg = new StringBuilder("-Xincgc -Xmx");
            arg.Append(javaxmx);
            arg.Append("M ");
            arg.Append(@"-Djava.library.path=");
            arg.Append(Environment.CurrentDirectory).Append(@"\.minecraft\versions\");
            arg.Append(ver).Append("\\").Append(ver).Append("-natives-").Append(timestamp.ToString());
            arg.Append(" -cp ");
            foreach (libraries.libraryies lib in info.libraries)
            {
                changeEvent("处理依赖" + lib.name);
                StringBuilder libp = new StringBuilder(Environment.CurrentDirectory + @"\.minecraft\libraries\");
                string[] split = lib.name.Split(':');//0 包;1 名字；2 版本
                if (split.Count() != 3)
                {
                    throw UnSupportVer;
                }
                libp.Append(split[0].Replace('.','\\'));
                libp.Append("\\");
                libp.Append(split[1]).Append("\\");
                libp.Append(split[2]).Append("\\");
                libp.Append(split[1]).Append("-");
                libp.Append(split[2]).Append(".jar;");
                if (!File.Exists(libp.ToString().Substring(0,libp.Length-1))&&lib.natives==null)
                {
                    if (lib.url != null)
                    {
                        changeEvent("下载依赖" + lib.name);
                        StringBuilder downfile = new StringBuilder(lib.url);
                        string[] split1 = lib.name.Split(':');//0 包;1 名字；2 版本
                        downfile.Append(split1[0].Replace('.', '\\'));
                        downfile.Append("\\");
                        downfile.Append(split1[1]).Append("\\");
                        downfile.Append(split1[2]).Append("\\");
                        downfile.Append(split1[1]).Append("-");
                        downfile.Append(split1[2]).Append(".jar;");
                        downloadJob = new downloader(downfile.ToString());
                        downloadJob.Filename=libp.ToString();
                        downThread downJob = new downThread(startdownload);
                        try
                        {
                            IAsyncResult downResult = downJob.BeginInvoke(null, null);
                        }
                        catch
                        {
                            throw new Exception("依赖下载失败" + lib.name, FailInLib);
                        }
                        

                    }
                    else
                    {
                        throw new Exception("无法获取所需依赖" + lib.name, FailInLib);
                    }
                }
                arg.Append(libp);
            }
            changeEvent("传递MC参数");
            StringBuilder mcpath = new StringBuilder(Environment.CurrentDirectory +  @"\.minecraft\versions\");
            mcpath.Append(ver).Append("\\").Append(ver).Append(".jar ");
            mcpath.Append(info.mainClass);
            arg.Append(mcpath);
            StringBuilder mcarg;
            //" --username ${auth_player_name} --session ${auth_session} --version ${version_name} --gameDir ${game_directory} --assetsDir ${game_assets}"
            mcarg = new StringBuilder(info.minecraftArguments);
            mcarg.Replace("${auth_player_name}", username);
            mcarg.Replace("${auth_session}", session);
            mcarg.Replace("${version_name}", ver);
            mcarg.Replace("${game_directory}", ".minecraft");
            mcarg.Replace("${game_assets}", @".minecraft\assets");
            arg.Append(" ");
            arg.Append(mcarg);
            game.StartInfo.Arguments = arg.ToString();
#if DEBUG
            System.Windows.Forms.MessageBox.Show(game.StartInfo.Arguments);
#endif
        }

        /// <summary>
        /// 释放依赖并运行游戏
        /// </summary>
        /// <returns>true:成功运行；false:失败</returns>
        public bool start()
        {
            changeEvent("创建依赖文件夹");
            StringBuilder NativePath = new StringBuilder(Environment.CurrentDirectory +  @"\.minecraft\versions\");
            NativePath.Append(version).Append("\\");
            DirectoryInfo oldnative = new DirectoryInfo(NativePath.ToString());
            foreach (DirectoryInfo dir in oldnative.GetDirectories())
            {
                if (dir.FullName.Contains("-natives-"))
                {
                    Directory.Delete(dir.FullName, true);
                }
            }
            NativePath.Append(version).Append("-natives-").Append(timestamp);
            if (!Directory.Exists(NativePath.ToString()))
            {
                Directory.CreateDirectory(NativePath.ToString());
            }
            
            StringBuilder file = new StringBuilder(NativePath.ToString());
            foreach (libraries.libraryies lib in Info.libraries)
            {
                changeEvent("解压" + lib.name);
                if (lib.natives == null)
                    continue;
                StringBuilder libp = new StringBuilder(Environment.CurrentDirectory + @"\.minecraft\libraries\");
                string[] split = lib.name.Split(':');//0 包;1 名字；2 版本
                if (split.Count() != 3)
                {
                    throw UnSupportVer;
                }
                libp.Append(split[0].Replace('.', '\\'));
                libp.Append("\\");
                libp.Append(split[1]).Append("\\");
                libp.Append(split[2]).Append("\\");
                libp.Append(split[1]).Append("-").Append(split[2]).Append("-natives-windows");
                libp.Append(".jar");
                ZipInputStream zipfile = new ZipInputStream(System.IO.File.OpenRead(libp.ToString()));
                ZipEntry theEntry;
                while ((theEntry = zipfile.GetNextEntry()) != null)
                {
                    bool exc = false;
                    if (lib.extract.exclude != null)
                    {
                        foreach (string excfile in lib.extract.exclude)
                        {
                            if (theEntry.Name.Contains(excfile))
                            {
                                exc = true;
                                break;
                            }
                        }
                    }
                    if (exc) continue;
                    StringBuilder filepath = new StringBuilder(NativePath.ToString());
                    filepath.Append("\\").Append(theEntry.Name);
                    FileStream fileWriter = File.Create(filepath.ToString());
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = zipfile.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            fileWriter.Write(data, 0, data.Length);
                        }
                        else
                        {
                            break;
                        }
                    }
                    fileWriter.Close();

                }
            }
            changeEvent("走你");
            game.StartInfo.UseShellExecute = false;
            //game.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\.minecraft\\versions\\" + version;
            Environment.SetEnvironmentVariable("APPDATA", Environment.CurrentDirectory);
            game.EnableRaisingEvents = true;
            game.Exited += game_Exited;
            //System.Windows.Forms.MessageBox.Show(System.IO.File.Exists(game.StartInfo.FileName).ToString());
            try
            {
                return game.Start();
            }
            catch
            {
                return false;
            }
        }

        void game_Exited(object sender, EventArgs e)
        {
            if (game.ExitCode != 0)
            {
                System.Windows.Forms.MessageBox.Show(game.ExitCode.ToString());
            }
            gameexit();
        }

        private void startdownload()
        {
            downloadJob.Start();
        }
        #endregion
        
    }
}
