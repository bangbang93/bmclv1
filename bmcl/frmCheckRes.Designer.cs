namespace bmcl
{
    partial class frmCheckRes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckRes));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listRes = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLastMod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMd5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonSync = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.prs = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listRes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonSync);
            this.splitContainer1.Panel2.Controls.Add(this.buttonCheck);
            this.splitContainer1.Size = new System.Drawing.Size(725, 216);
            this.splitContainer1.SplitterDistance = 543;
            this.splitContainer1.TabIndex = 0;
            // 
            // listRes
            // 
            this.listRes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnLastMod,
            this.columnSize,
            this.columnStatus,
            this.columnMd5});
            this.listRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRes.Location = new System.Drawing.Point(0, 0);
            this.listRes.Name = "listRes";
            this.listRes.Size = new System.Drawing.Size(543, 216);
            this.listRes.TabIndex = 0;
            this.listRes.UseCompatibleStateImageBehavior = false;
            this.listRes.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "文件名";
            this.columnName.Width = 159;
            // 
            // columnLastMod
            // 
            this.columnLastMod.Text = "修改时间";
            this.columnLastMod.Width = 111;
            // 
            // columnSize
            // 
            this.columnSize.Text = "大小";
            // 
            // columnStatus
            // 
            this.columnStatus.Text = "状态";
            this.columnStatus.Width = 52;
            // 
            // columnMd5
            // 
            this.columnMd5.Text = "MD5值";
            this.columnMd5.Width = 0;
            // 
            // buttonSync
            // 
            this.buttonSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSync.Location = new System.Drawing.Point(56, 85);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(119, 43);
            this.buttonSync.TabIndex = 1;
            this.buttonSync.Text = "同步";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheck.Location = new System.Drawing.Point(56, 12);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(119, 43);
            this.buttonCheck.TabIndex = 0;
            this.buttonCheck.Text = "对比本地";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.prs);
            this.splitContainer2.Size = new System.Drawing.Size(725, 248);
            this.splitContainer2.SplitterDistance = 216;
            this.splitContainer2.TabIndex = 1;
            // 
            // prs
            // 
            this.prs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prs.Location = new System.Drawing.Point(0, 0);
            this.prs.Name = "prs";
            this.prs.Size = new System.Drawing.Size(725, 28);
            this.prs.TabIndex = 0;
            // 
            // frmCheckRes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 248);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCheckRes";
            this.Text = "同步资源文件";
            this.Load += new System.EventHandler(this.frmCheckRes_Load);
            this.Shown += new System.EventHandler(this.frmCheckRes_Shown);
            this.SizeChanged += new System.EventHandler(this.frmCheckRes_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listRes;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnLastMod;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnStatus;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.ColumnHeader columnMd5;
        private System.Windows.Forms.Button buttonSync;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ProgressBar prs;
    }
}