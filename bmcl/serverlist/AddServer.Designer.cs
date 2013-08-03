namespace bmcl.serverlist
{
    partial class AddServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkIsHide = new System.Windows.Forms.CheckBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "地    址";
            // 
            // checkIsHide
            // 
            this.checkIsHide.AutoSize = true;
            this.checkIsHide.Location = new System.Drawing.Point(15, 105);
            this.checkIsHide.Name = "checkIsHide";
            this.checkIsHide.Size = new System.Drawing.Size(72, 16);
            this.checkIsHide.TabIndex = 2;
            this.checkIsHide.Text = "隐藏地址";
            this.checkIsHide.UseVisualStyleBackColor = true;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(93, 30);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(150, 21);
            this.txtServerName.TabIndex = 3;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(93, 66);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(150, 21);
            this.txtAddress.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(14, 181);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 42);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancal
            // 
            this.btnCancal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancal.Location = new System.Drawing.Point(153, 181);
            this.btnCancal.Name = "btnCancal";
            this.btnCancal.Size = new System.Drawing.Size(99, 42);
            this.btnCancal.TabIndex = 6;
            this.btnCancal.Text = "取消";
            this.btnCancal.UseVisualStyleBackColor = true;
            // 
            // AddServer
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 244);
            this.Controls.Add(this.btnCancal);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.checkIsHide);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddServer";
            this.Text = "服务器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkIsHide;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancal;
    }
}