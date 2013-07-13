namespace bmcl
{
    partial class frmPackUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.savePackUp = new System.Windows.Forms.SaveFileDialog();
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
            this.checkAutoJava = new System.Windows.Forms.CheckBox();
            this.checkRes = new System.Windows.Forms.CheckBox();
            this.checkLibNat = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labINFO = new System.Windows.Forms.Label();
            this.txtExtJArg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(664, 249);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(127, 53);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始(&S)";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // savePackUp
            // 
            this.savePackUp.DefaultExt = "zip";
            this.savePackUp.Filter = "zip|*.zip";
            // 
            // checkAutoStart
            // 
            this.checkAutoStart.AutoSize = true;
            this.checkAutoStart.Location = new System.Drawing.Point(288, 144);
            this.checkAutoStart.Name = "checkAutoStart";
            this.checkAutoStart.Size = new System.Drawing.Size(96, 16);
            this.checkAutoStart.TabIndex = 24;
            this.checkAutoStart.Text = "直接开始游戏";
            this.checkAutoStart.UseVisualStyleBackColor = true;
            this.checkAutoStart.CheckedChanged += new System.EventHandler(this.checkAutoStart_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(559, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "MB";
            // 
            // txtJavaXmx
            // 
            this.txtJavaXmx.Location = new System.Drawing.Point(407, 44);
            this.txtJavaXmx.Name = "txtJavaXmx";
            this.txtJavaXmx.Size = new System.Drawing.Size(145, 21);
            this.txtJavaXmx.TabIndex = 22;
            this.txtJavaXmx.TextChanged += new System.EventHandler(this.txtJavaXmx_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(297, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "Java运行内存大小";
            // 
            // buttonJavaw
            // 
            this.buttonJavaw.Location = new System.Drawing.Point(567, 5);
            this.buttonJavaw.Name = "buttonJavaw";
            this.buttonJavaw.Size = new System.Drawing.Size(28, 21);
            this.buttonJavaw.TabIndex = 20;
            this.buttonJavaw.Text = "…";
            this.buttonJavaw.UseVisualStyleBackColor = true;
            // 
            // txtJavaw
            // 
            this.txtJavaw.Location = new System.Drawing.Point(386, 6);
            this.txtJavaw.Name = "txtJavaw";
            this.txtJavaw.Size = new System.Drawing.Size(166, 21);
            this.txtJavaw.TabIndex = 19;
            this.txtJavaw.TextChanged += new System.EventHandler(this.txtJavaw_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "javaw.exe路径";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(57, 44);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(155, 21);
            this.txtPwd.TabIndex = 17;
            this.txtPwd.TextChanged += new System.EventHandler(this.txtPwd_TextChanged);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(57, 6);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(155, 21);
            this.txtUserName.TabIndex = 16;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listAuth);
            this.groupBox1.Location = new System.Drawing.Point(12, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 164);
            this.groupBox1.TabIndex = 15;
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
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "密  码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "用户名";
            // 
            // checkAutoJava
            // 
            this.checkAutoJava.AutoSize = true;
            this.checkAutoJava.Location = new System.Drawing.Point(602, 9);
            this.checkAutoJava.Name = "checkAutoJava";
            this.checkAutoJava.Size = new System.Drawing.Size(72, 16);
            this.checkAutoJava.TabIndex = 25;
            this.checkAutoJava.Text = "自动搜寻";
            this.checkAutoJava.UseVisualStyleBackColor = true;
            this.checkAutoJava.CheckedChanged += new System.EventHandler(this.checkAutoJava_CheckedChanged);
            // 
            // checkRes
            // 
            this.checkRes.AutoSize = true;
            this.checkRes.Location = new System.Drawing.Point(288, 167);
            this.checkRes.Name = "checkRes";
            this.checkRes.Size = new System.Drawing.Size(96, 16);
            this.checkRes.TabIndex = 26;
            this.checkRes.Text = "包括资源文件";
            this.checkRes.UseVisualStyleBackColor = true;
            // 
            // checkLibNat
            // 
            this.checkLibNat.AutoSize = true;
            this.checkLibNat.Location = new System.Drawing.Point(288, 190);
            this.checkLibNat.Name = "checkLibNat";
            this.checkLibNat.Size = new System.Drawing.Size(96, 16);
            this.checkLibNat.TabIndex = 27;
            this.checkLibNat.Text = "包括依赖文件";
            this.checkLibNat.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(288, 213);
            this.label6.MaximumSize = new System.Drawing.Size(150, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 48);
            this.label6.TabIndex = 28;
            this.label6.Text = "资源文件和依赖文件都可以使用启动器下载，所以如果要尽量减小压缩包的体积，可以不勾选这两项";
            // 
            // labINFO
            // 
            this.labINFO.AutoSize = true;
            this.labINFO.Location = new System.Drawing.Point(405, 171);
            this.labINFO.Name = "labINFO";
            this.labINFO.Size = new System.Drawing.Size(227, 24);
            this.labINFO.TabIndex = 29;
            this.labINFO.Text = "1.6.1之前的版本必须包含资源和依赖文件\r\n导入的客户端也必须包含资源和依赖文件";
            this.labINFO.Visible = false;
            // 
            // txtExtJArg
            // 
            this.txtExtJArg.Location = new System.Drawing.Point(298, 103);
            this.txtExtJArg.Name = "txtExtJArg";
            this.txtExtJArg.Size = new System.Drawing.Size(254, 21);
            this.txtExtJArg.TabIndex = 31;
            this.txtExtJArg.TextChanged += new System.EventHandler(this.txtExtJArg_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(297, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "额外的JVM Arguements";
            // 
            // frmPackUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 314);
            this.Controls.Add(this.txtExtJArg);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labINFO);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkLibNat);
            this.Controls.Add(this.checkRes);
            this.Controls.Add(this.checkAutoJava);
            this.Controls.Add(this.checkAutoStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtJavaXmx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonJavaw);
            this.Controls.Add(this.txtJavaw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Name = "frmPackUp";
            this.Text = "打包客户端";
            this.Load += new System.EventHandler(this.frmPackUp_Load);
            this.Shown += new System.EventHandler(this.frmPackUp_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.SaveFileDialog savePackUp;
        private System.Windows.Forms.CheckBox checkAutoStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtJavaXmx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonJavaw;
        private System.Windows.Forms.TextBox txtJavaw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkAutoJava;
        private System.Windows.Forms.CheckBox checkRes;
        private System.Windows.Forms.CheckBox checkLibNat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labINFO;
        private System.Windows.Forms.TextBox txtExtJArg;
        private System.Windows.Forms.Label label8;
    }
}