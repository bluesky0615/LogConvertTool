namespace LogConvertTool
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_SelectFile = new System.Windows.Forms.Button();
            this.textBox_sourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox_convertCsv = new System.Windows.Forms.RichTextBox();
            this.button_Convert = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_logType = new System.Windows.Forms.ComboBox();
            this.button_config = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_MultiConvert = new System.Windows.Forms.Button();
            this.richTextBox_multiConvert = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_sourceDir = new System.Windows.Forms.TextBox();
            this.button_SelectDir = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_SelectFile
            // 
            this.button_SelectFile.Location = new System.Drawing.Point(473, 21);
            this.button_SelectFile.Name = "button_SelectFile";
            this.button_SelectFile.Size = new System.Drawing.Size(75, 23);
            this.button_SelectFile.TabIndex = 0;
            this.button_SelectFile.Text = "选择文件";
            this.button_SelectFile.UseVisualStyleBackColor = true;
            this.button_SelectFile.Click += new System.EventHandler(this.button_SelectFile_Click);
            // 
            // textBox_sourceFile
            // 
            this.textBox_sourceFile.Location = new System.Drawing.Point(96, 21);
            this.textBox_sourceFile.Name = "textBox_sourceFile";
            this.textBox_sourceFile.ReadOnly = true;
            this.textBox_sourceFile.Size = new System.Drawing.Size(357, 21);
            this.textBox_sourceFile.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log文件：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "输出文件：";
            // 
            // richTextBox_convertCsv
            // 
            this.richTextBox_convertCsv.Location = new System.Drawing.Point(96, 63);
            this.richTextBox_convertCsv.Name = "richTextBox_convertCsv";
            this.richTextBox_convertCsv.Size = new System.Drawing.Size(357, 60);
            this.richTextBox_convertCsv.TabIndex = 4;
            this.richTextBox_convertCsv.Text = "";
            // 
            // button_Convert
            // 
            this.button_Convert.Location = new System.Drawing.Point(473, 63);
            this.button_Convert.Name = "button_Convert";
            this.button_Convert.Size = new System.Drawing.Size(75, 23);
            this.button_Convert.TabIndex = 5;
            this.button_Convert.Text = "转换";
            this.button_Convert.UseVisualStyleBackColor = true;
            this.button_Convert.Click += new System.EventHandler(this.button_Convert_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "提取配置：";
            // 
            // comboBox_logType
            // 
            this.comboBox_logType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_logType.FormattingEnabled = true;
            this.comboBox_logType.Location = new System.Drawing.Point(106, 20);
            this.comboBox_logType.Name = "comboBox_logType";
            this.comboBox_logType.Size = new System.Drawing.Size(200, 20);
            this.comboBox_logType.TabIndex = 6;
            // 
            // button_config
            // 
            this.button_config.Location = new System.Drawing.Point(334, 18);
            this.button_config.Name = "button_config";
            this.button_config.Size = new System.Drawing.Size(75, 23);
            this.button_config.TabIndex = 0;
            this.button_config.Text = "设置";
            this.button_config.UseVisualStyleBackColor = true;
            this.button_config.Click += new System.EventHandler(this.button_Config_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_MultiConvert);
            this.groupBox2.Controls.Add(this.richTextBox_multiConvert);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_sourceDir);
            this.groupBox2.Controls.Add(this.button_SelectDir);
            this.groupBox2.Location = new System.Drawing.Point(8, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 141);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "多转一";
            // 
            // button_MultiConvert
            // 
            this.button_MultiConvert.Location = new System.Drawing.Point(476, 64);
            this.button_MultiConvert.Name = "button_MultiConvert";
            this.button_MultiConvert.Size = new System.Drawing.Size(75, 23);
            this.button_MultiConvert.TabIndex = 11;
            this.button_MultiConvert.Text = "转换";
            this.button_MultiConvert.UseVisualStyleBackColor = true;
            this.button_MultiConvert.Click += new System.EventHandler(this.button_MultiConvert_Click);
            // 
            // richTextBox_multiConvert
            // 
            this.richTextBox_multiConvert.Location = new System.Drawing.Point(99, 64);
            this.richTextBox_multiConvert.Name = "richTextBox_multiConvert";
            this.richTextBox_multiConvert.Size = new System.Drawing.Size(357, 60);
            this.richTextBox_multiConvert.TabIndex = 10;
            this.richTextBox_multiConvert.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "输出文件：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Logs目录：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_sourceDir
            // 
            this.textBox_sourceDir.Location = new System.Drawing.Point(99, 24);
            this.textBox_sourceDir.Name = "textBox_sourceDir";
            this.textBox_sourceDir.ReadOnly = true;
            this.textBox_sourceDir.Size = new System.Drawing.Size(357, 21);
            this.textBox_sourceDir.TabIndex = 7;
            // 
            // button_SelectDir
            // 
            this.button_SelectDir.Location = new System.Drawing.Point(476, 24);
            this.button_SelectDir.Name = "button_SelectDir";
            this.button_SelectDir.Size = new System.Drawing.Size(75, 23);
            this.button_SelectDir.TabIndex = 6;
            this.button_SelectDir.Text = "选择目录";
            this.button_SelectDir.UseVisualStyleBackColor = true;
            this.button_SelectDir.Click += new System.EventHandler(this.button_SelectDir_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox_convertCsv);
            this.groupBox1.Controls.Add(this.button_SelectFile);
            this.groupBox1.Controls.Add(this.textBox_sourceFile);
            this.groupBox1.Controls.Add(this.button_Convert);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(10, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 137);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "一转一";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(106, 188);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(357, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.comboBox_logType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_config);
            this.Name = "Form1";
            this.Text = "Log提取工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_SelectFile;
        private System.Windows.Forms.TextBox textBox_sourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox_convertCsv;
        private System.Windows.Forms.Button button_Convert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_logType;
        private System.Windows.Forms.Button button_config;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_MultiConvert;
        private System.Windows.Forms.RichTextBox richTextBox_multiConvert;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_sourceDir;
        private System.Windows.Forms.Button button_SelectDir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

