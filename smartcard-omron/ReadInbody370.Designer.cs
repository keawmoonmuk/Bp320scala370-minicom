namespace SmartcardApp
{
    partial class ReadInbody370
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
            this.components = new System.ComponentModel.Container();
            this.timer_readdatainbody370 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_readdatainbody370
            // 
            this.timer_readdatainbody370.Interval = 2000;
            this.timer_readdatainbody370.Tick += new System.EventHandler(this.timer_readdatainbody370_Tick);
            // 
            // ReadInbody370
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SmartcardApp.Properties.Resources.bsm370;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1014, 712);
            this.Name = "ReadInbody370";
            this.Text = "ReadInbody370";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_readdatainbody370;
    }
}