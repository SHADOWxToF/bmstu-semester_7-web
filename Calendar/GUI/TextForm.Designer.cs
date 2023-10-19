namespace GUI
{
    partial class TextForm
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
            display = new TextBox();
            SuspendLayout();
            // 
            // display
            // 
            display.Location = new Point(12, 12);
            display.Multiline = true;
            display.Name = "display";
            display.ScrollBars = ScrollBars.Vertical;
            display.Size = new Size(384, 224);
            display.TabIndex = 51;
            display.ReadOnly = true;
            display.TabStop = false;
            // 
            // DisciplinesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(407, 245);
            Controls.Add(display);
            Name = "DisciplinesForm";
            Text = "DisciplinesForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public TextBox display;
    }
}