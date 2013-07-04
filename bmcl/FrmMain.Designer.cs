namespace bmcl
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.VerList = new System.Windows.Forms.ListBox();
            this.labReltime = new System.Windows.Forms.Label();
            this.labTime = new System.Windows.Forms.Label();
            this.labVer = new System.Windows.Forms.Label();
            this.llabRelTime = new System.Windows.Forms.Label();
            this.llabTime = new System.Windows.Forms.Label();
            this.llabVer = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkAutoStart = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtJavaXmx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonJavaw = new System.Windows.Forms.Button();
            this.txtJavaw = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listAuth = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listRemoteVer = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RelTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCheckRes = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonFlush = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.icoBmcl = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuIco = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolShow = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdlgJavaw = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonStart = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.menuIco.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(931, 324);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(923, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "游戏设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.VerList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labReltime);
            this.splitContainer1.Panel2.Controls.Add(this.labTime);
            this.splitContainer1.Panel2.Controls.Add(this.labVer);
            this.splitContainer1.Panel2.Controls.Add(this.llabRelTime);
            this.splitContainer1.Panel2.Controls.Add(this.llabTime);
            this.splitContainer1.Panel2.Controls.Add(this.llabVer);
            this.splitContainer1.Size = new System.Drawing.Size(917, 292);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 0;
            // 
            // VerList
            // 
            this.VerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerList.FormattingEnabled = true;
            this.VerList.ItemHeight = 12;
            this.VerList.Location = new System.Drawing.Point(0, 0);
            this.VerList.Name = "VerList";
            this.VerList.Size = new System.Drawing.Size(178, 292);
            this.VerList.TabIndex = 1;
            this.VerList.SelectedIndexChanged += new System.EventHandler(this.VerList_SelectedIndexChanged);
            // 
            // labReltime
            // 
            this.labReltime.AutoSize = true;
            this.labReltime.Location = new System.Drawing.Point(203, 108);
            this.labReltime.Name = "labReltime";
            this.labReltime.Size = new System.Drawing.Size(0, 12);
            this.labReltime.TabIndex = 5;
            // 
            // labTime
            // 
            this.labTime.AutoSize = true;
            this.labTime.Location = new System.Drawing.Point(203, 77);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(0, 12);
            this.labTime.TabIndex = 4;
            // 
            // labVer
            // 
            this.labVer.AutoSize = true;
            this.labVer.Location = new System.Drawing.Point(203, 46);
            this.labVer.Name = "labVer";
            this.labVer.Size = new System.Drawing.Size(0, 12);
            this.labVer.TabIndex = 3;
            // 
            // llabRelTime
            // 
            this.llabRelTime.AutoSize = true;
            this.llabRelTime.Location = new System.Drawing.Point(100, 108);
            this.llabRelTime.Name = "llabRelTime";
            this.llabRelTime.Size = new System.Drawing.Size(53, 12);
            this.llabRelTime.TabIndex = 2;
            this.llabRelTime.Text = "发布时间";
            // 
            // llabTime
            // 
            this.llabTime.AutoSize = true;
            this.llabTime.Location = new System.Drawing.Point(100, 77);
            this.llabTime.Name = "llabTime";
            this.llabTime.Size = new System.Drawing.Size(77, 12);
            this.llabTime.TabIndex = 1;
            this.llabTime.Text = "上次打开时间";
            // 
            // llabVer
            // 
            this.llabVer.AutoSize = true;
            this.llabVer.Location = new System.Drawing.Point(100, 46);
            this.llabVer.Name = "llabVer";
            this.llabVer.Size = new System.Drawing.Size(53, 12);
            this.llabVer.TabIndex = 0;
            this.llabVer.Text = "游戏版本";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkAutoStart);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtJavaXmx);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.buttonJavaw);
            this.tabPage2.Controls.Add(this.txtJavaw);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtPwd);
            this.tabPage2.Controls.Add(this.txtUserName);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(923, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "启动设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkAutoStart
            // 
            this.checkAutoStart.AutoSize = true;
            this.checkAutoStart.Location = new System.Drawing.Point(341, 131);
            this.checkAutoStart.Name = "checkAutoStart";
            this.checkAutoStart.Size = new System.Drawing.Size(120, 16);
            this.checkAutoStart.TabIndex = 12;
            this.checkAutoStart.Text = "下次直接开始游戏";
            this.checkAutoStart.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(601, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "MB";
            // 
            // txtJavaXmx
            // 
            this.txtJavaXmx.Location = new System.Drawing.Point(449, 89);
            this.txtJavaXmx.Name = "txtJavaXmx";
            this.txtJavaXmx.Size = new System.Drawing.Size(145, 21);
            this.txtJavaXmx.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Java运行内存大小";
            // 
            // buttonJavaw
            // 
            this.buttonJavaw.Location = new System.Drawing.Point(609, 50);
            this.buttonJavaw.Name = "buttonJavaw";
            this.buttonJavaw.Size = new System.Drawing.Size(28, 21);
            this.buttonJavaw.TabIndex = 8;
            this.buttonJavaw.Text = "…";
            this.buttonJavaw.UseVisualStyleBackColor = true;
            this.buttonJavaw.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtJavaw
            // 
            this.txtJavaw.Location = new System.Drawing.Point(428, 51);
            this.txtJavaw.Name = "txtJavaw";
            this.txtJavaw.Size = new System.Drawing.Size(166, 21);
            this.txtJavaw.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(339, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "javaw.exe路径";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(99, 89);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(155, 21);
            this.txtPwd.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(99, 51);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(155, 21);
            this.txtUserName.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listAuth);
            this.groupBox1.Location = new System.Drawing.Point(54, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 164);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录方式";
            // 
            // listAuth
            // 
            this.listAuth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAuth.FormattingEnabled = true;
            this.listAuth.ItemHeight = 12;
            this.listAuth.Location = new System.Drawing.Point(3, 17);
            this.listAuth.Name = "listAuth";
            this.listAuth.Size = new System.Drawing.Size(194, 144);
            this.listAuth.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密  码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(923, 298);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "版本管理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listRemoteVer);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.buttonCheckRes);
            this.splitContainer3.Panel2.Controls.Add(this.buttonDownload);
            this.splitContainer3.Panel2.Controls.Add(this.buttonFlush);
            this.splitContainer3.Size = new System.Drawing.Size(917, 292);
            this.splitContainer3.SplitterDistance = 769;
            this.splitContainer3.TabIndex = 0;
            // 
            // listRemoteVer
            // 
            this.listRemoteVer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.RelTime,
            this.Type});
            this.listRemoteVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRemoteVer.Location = new System.Drawing.Point(0, 0);
            this.listRemoteVer.Name = "listRemoteVer";
            this.listRemoteVer.ShowGroups = false;
            this.listRemoteVer.Size = new System.Drawing.Size(769, 292);
            this.listRemoteVer.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listRemoteVer.TabIndex = 0;
            this.listRemoteVer.UseCompatibleStateImageBehavior = false;
            this.listRemoteVer.View = System.Windows.Forms.View.Details;
            // 
            // Id
            // 
            this.Id.Text = "版本";
            // 
            // RelTime
            // 
            this.RelTime.Text = "发布时间";
            this.RelTime.Width = 232;
            // 
            // Type
            // 
            this.Type.Text = "发布类型";
            // 
            // buttonCheckRes
            // 
            this.buttonCheckRes.Location = new System.Drawing.Point(24, 133);
            this.buttonCheckRes.Name = "buttonCheckRes";
            this.buttonCheckRes.Size = new System.Drawing.Size(101, 37);
            this.buttonCheckRes.TabIndex = 2;
            this.buttonCheckRes.Text = "检查资源文件";
            this.buttonCheckRes.UseVisualStyleBackColor = true;
            this.buttonCheckRes.Click += new System.EventHandler(this.buttonCheckRes_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(24, 75);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(101, 37);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "下载";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonFlush
            // 
            this.buttonFlush.Location = new System.Drawing.Point(24, 18);
            this.buttonFlush.Name = "buttonFlush";
            this.buttonFlush.Size = new System.Drawing.Size(101, 37);
            this.buttonFlush.TabIndex = 0;
            this.buttonFlush.Text = "刷新版本(&R)";
            this.buttonFlush.UseVisualStyleBackColor = true;
            this.buttonFlush.Click += new System.EventHandler(this.buttonFlush_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.webBrowser1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(923, 298);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "更新信息";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(917, 292);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("http://mcupdate.tumblr.com", System.UriKind.Absolute);
            // 
            // icoBmcl
            // 
            this.icoBmcl.ContextMenuStrip = this.menuIco;
            this.icoBmcl.Icon = ((System.Drawing.Icon)(resources.GetObject("icoBmcl.Icon")));
            this.icoBmcl.Text = "BMCL";
            this.icoBmcl.Visible = true;
            this.icoBmcl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.icoBmcl_MouseDoubleClick);
            // 
            // menuIco
            // 
            this.menuIco.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolShow});
            this.menuIco.Name = "menuIco";
            this.menuIco.Size = new System.Drawing.Size(125, 26);
            // 
            // toolShow
            // 
            this.toolShow.Name = "toolShow";
            this.toolShow.Size = new System.Drawing.Size(124, 22);
            this.toolShow.Text = "显示窗口";
            this.toolShow.Click += new System.EventHandler(this.toolShow_Click);
            // 
            // ofdlgJavaw
            // 
            this.ofdlgJavaw.FileName = "javaw.exe";
            this.ofdlgJavaw.Filter = "javaw.exe|javaw.exe";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.buttonStart);
            this.splitContainer2.Size = new System.Drawing.Size(931, 394);
            this.splitContainer2.SplitterDistance = 324;
            this.splitContainer2.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(804, 7);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(115, 47);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "开始游戏！(&S)";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 394);
            this.Controls.Add(this.splitContainer2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "bangbang93\'s Minecraft Launcher";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.VisibleChanged += new System.EventHandler(this.FrmMain_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.menuIco.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NotifyIcon icoBmcl;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox VerList;
        private System.Windows.Forms.Label labReltime;
        private System.Windows.Forms.Label labTime;
        private System.Windows.Forms.Label labVer;
        private System.Windows.Forms.Label llabRelTime;
        private System.Windows.Forms.Label llabTime;
        private System.Windows.Forms.Label llabVer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonJavaw;
        private System.Windows.Forms.TextBox txtJavaw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog ofdlgJavaw;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtJavaXmx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listAuth;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.CheckBox checkAutoStart;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListView listRemoteVer;
        private System.Windows.Forms.ColumnHeader Id;
        private System.Windows.Forms.ColumnHeader RelTime;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.Button buttonFlush;
        private System.Windows.Forms.ContextMenuStrip menuIco;
        private System.Windows.Forms.ToolStripMenuItem toolShow;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonCheckRes;
    }
}

