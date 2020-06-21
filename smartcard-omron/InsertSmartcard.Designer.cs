namespace SmartcardApp
{
    partial class InsertSmartcard
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
            this.time_checkreadsmartcard = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // time_checkreadsmartcard
            // 
            this.time_checkreadsmartcard.Interval = 5000;
            this.time_checkreadsmartcard.Tick += new System.EventHandler(this.time_checkreadsmartcard_Tick);
            // 
            // InsertSmartcard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SmartcardApp.Properties.Resources.insertsmartcard;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1028, 644);
            this.Name = "InsertSmartcard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer time_checkreadsmartcard;
    }
}