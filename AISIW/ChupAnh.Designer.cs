namespace AISIW
{
    partial class ChupAnh
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
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.pb = new System.Windows.Forms.PictureBox();
            this.btnCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(-100, -100);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(50, 30);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(-100, -100);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(50, 30);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(-100, -100);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(157, 76);
            this.pb.TabIndex = 2;
            this.pb.TabStop = false;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(-100, -100);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(50, 30);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // ChupAnh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(402, 261);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChupAnh";
            this.Text = "ChupAnh";
            this.Load += new System.EventHandler(this.ChupAnh_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ChupAnh_DragDrop);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ChupAnh_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChupAnh_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChupAnh_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChupAnh_MouseUp);
            this.Validated += new System.EventHandler(this.ChupAnh_Validated);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.Button btnCopy;
    }
}