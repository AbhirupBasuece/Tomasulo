namespace Tomasulo
{
    partial class FunctionalUnitsUsageForm
    {

        private System.ComponentModel.IContainer components = null;

            /*-----------------------------------------------------------------
            summary
            Clean up any resources being used.
            param name="disposing" is true if all resources should be disposed;
            ------------------------------------------------------------------*/
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
            this.ResourcesGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ResourcesGV)).BeginInit();
            this.SuspendLayout();
            // 
            // ResourcesGV
            // 
            this.ResourcesGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResourcesGV.Location = new System.Drawing.Point(12, 12);
            this.ResourcesGV.Name = "ResourcesGV";
            this.ResourcesGV.Size = new System.Drawing.Size(649, 336);
            this.ResourcesGV.TabIndex = 0;
            // 
            // FuncuinalUnitsUsageForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(673, 360);
            this.Controls.Add(this.ResourcesGV);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FuncuinalUnitsUsageForm";
            this.Text = "Funcuinal Units Usage";
            this.Load += new System.EventHandler(this.ResourcesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResourcesGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ResourcesGV;

    }
}