namespace DemoSeleniumWF
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.uploadFile = new System.Windows.Forms.Button();
            this.linkFileName = new System.Windows.Forms.LinkLabel();
            this.buttonResetFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelResultTest = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 241);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "List of template test file";
            // 
            // uploadFile
            // 
            this.uploadFile.Location = new System.Drawing.Point(370, 146);
            this.uploadFile.Name = "uploadFile";
            this.uploadFile.Size = new System.Drawing.Size(94, 38);
            this.uploadFile.TabIndex = 15;
            this.uploadFile.Text = "Upload test file";
            this.uploadFile.UseVisualStyleBackColor = true;
            // 
            // linkFileName
            // 
            this.linkFileName.AutoSize = true;
            this.linkFileName.Location = new System.Drawing.Point(206, 225);
            this.linkFileName.Name = "linkFileName";
            this.linkFileName.Size = new System.Drawing.Size(0, 13);
            this.linkFileName.TabIndex = 14;
            // 
            // buttonResetFile
            // 
            this.buttonResetFile.Location = new System.Drawing.Point(270, 146);
            this.buttonResetFile.Name = "buttonResetFile";
            this.buttonResetFile.Size = new System.Drawing.Size(94, 38);
            this.buttonResetFile.TabIndex = 13;
            this.buttonResetFile.Text = "Reset file test";
            this.buttonResetFile.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(141, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "View file:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelResultTest
            // 
            this.labelResultTest.AutoSize = true;
            this.labelResultTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResultTest.ForeColor = System.Drawing.Color.Black;
            this.labelResultTest.Location = new System.Drawing.Point(142, 200);
            this.labelResultTest.Name = "labelResultTest";
            this.labelResultTest.Size = new System.Drawing.Size(0, 16);
            this.labelResultTest.TabIndex = 12;
            this.labelResultTest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelResultTest.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(582, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(38)))), ((int)(((byte)(90)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(141, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 48);
            this.button1.TabIndex = 9;
            this.button1.Text = "Run test";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colFileName,
            this.colDateModified,
            this.colAction});
            this.dataGridView1.Location = new System.Drawing.Point(12, 257);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(582, 150);
            this.dataGridView1.TabIndex = 17;
            // 
            // colNo
            // 
            this.colNo.FillWeight = 40.60914F;
            this.colNo.HeaderText = "No";
            this.colNo.Name = "colNo";
            // 
            // colFileName
            // 
            this.colFileName.FillWeight = 235.6617F;
            this.colFileName.HeaderText = "File name";
            this.colFileName.Name = "colFileName";
            // 
            // colDateModified
            // 
            this.colDateModified.FillWeight = 92.17365F;
            this.colDateModified.HeaderText = "Date modified";
            this.colDateModified.Name = "colDateModified";
            // 
            // colAction
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colAction.DefaultCellStyle = dataGridViewCellStyle1;
            this.colAction.FillWeight = 31.55556F;
            this.colAction.HeaderText = "";
            this.colAction.Name = "colAction";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 419);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uploadFile);
            this.Controls.Add(this.linkFileName);
            this.Controls.Add(this.buttonResetFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelResultTest);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button uploadFile;
        private System.Windows.Forms.LinkLabel linkFileName;
        private System.Windows.Forms.Button buttonResetFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelResultTest;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateModified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
    }
}

