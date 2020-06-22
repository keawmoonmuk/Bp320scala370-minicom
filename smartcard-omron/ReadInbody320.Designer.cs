namespace SmartcardApp
{
    partial class ReadInbody320
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
            this.timer_readinbody320 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_readinbody320
            // 
            this.timer_readinbody320.Interval = 2000;
            this.timer_readinbody320.Tick += new System.EventHandler(this.timer_readinbody320_Tick);
            // 
            // ReadInbody320
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SmartcardApp.Properties.Resources.bp320;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1014, 712);
            this.Name = "ReadInbody320";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer timer_readinbody320;
    }
}