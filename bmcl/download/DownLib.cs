﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bmcl.libraries;
using System.IO;

namespace bmcl.download
{
    class DownLib
    {

        libraryies lib;
        private string urlLib = FrmMain.URL_DOWNLOAD_BASE + "/libraries/";

        public delegate void changeHandel(string status);
        private delegate void downThread();
        public delegate void DownFinEventHandel();

        public event DownFinEventHandel DownFinEvent;

        public static Exception NoJava = new Exception("找不到java");
        public static Exception NoRam = new Exception("没有足够物理内存");
        public static Exception NoMoreRam = new Exception("没有足够的可用内存");
        public static Exception UnSupportVer = new Exception("启动器不支持这个版本");
        public static Exception FailInLib = new Exception("无法获得所需的依赖");

        public DownLib(libraryies Lib)
        {
            lib = Lib;
            launcher.downLibEvent += launcher_downLibEvent;
        }

        void launcher_downLibEvent(libraryies lib)
        {
            startdownload();
        }
        public void startdownload()
        {
            string libp = buildLibPath(lib);
            downloader downer = new downloader(urlLib + buildLibPath(lib).Remove(0, Environment.CurrentDirectory.Length + 1));
            downer.Filename = buildLibPath(lib);
            downer.Start();
            downer.Finished += downer_Finished;
        }

        void downer_Finished(downloader sender)
        {
            DownFinEvent();
        }

        /// <summary>
        /// 获取lib文件相对路径
        /// </summary>
        /// <returns></returns>
        public string buildLibPath(libraryies lib)
        {
            StringBuilder libp = new StringBuilder(@".minecraft\libraries\");
            string[] split = lib.name.Split(':');//0 包;1 名字；2 版本
            if (split.Count() != 3)
            {
                throw UnSupportVer;
            }
            libp.Append(split[0].Replace('.', '\\'));
            libp.Append("\\");
            libp.Append(split[1]).Append("\\");
            libp.Append(split[2]).Append("\\");
            libp.Append(split[1]).Append("-");
            libp.Append(split[2]).Append(".jar;");
            return libp.ToString();
        }

        
    }
}
