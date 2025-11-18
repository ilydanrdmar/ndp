namespace kuafor
{
    partial class FormCalisanPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblCalisan;
        private Panel panelTakvim;
        private GroupBox groupBoxIslemler;
        private ListBox lstIslemler;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblCalisan = new System.Windows.Forms.Label();
            this.panelTakvim = new System.Windows.Forms.Panel();
            this.groupBoxIslemler = new System.Windows.Forms.GroupBox();
            this.lstIslemler = new System.Windows.Forms.ListBox();
            this.groupBoxIslemler.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCalisan
            // 
            this.lblCalisan.AutoSize = true;
            this.lblCalisan.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCalisan.Location = new System.Drawing.Point(20, 20);
            this.lblCalisan.Name = "lblCalisan";
            this.lblCalisan.Size = new System.Drawing.Size(224, 32);
            this.lblCalisan.TabIndex = 0;
            this.lblCalisan.Text = "Çalışan Adı Soyadı";
            // 
            // panelTakvim
            // 
            this.panelTakvim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTakvim.Location = new System.Drawing.Point(250, 70);
            this.panelTakvim.Name = "panelTakvim";
            this.panelTakvim.Size = new System.Drawing.Size(850, 650);
            this.panelTakvim.TabIndex = 1;
            // 
            // groupBoxIslemler
            // 
            this.groupBoxIslemler.Controls.Add(this.lstIslemler);
            this.groupBoxIslemler.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBoxIslemler.Location = new System.Drawing.Point(20, 70);
            this.groupBoxIslemler.Name = "groupBoxIslemler";
            this.groupBoxIslemler.Size = new System.Drawing.Size(220, 650);
            this.groupBoxIslemler.TabIndex = 2;
            this.groupBoxIslemler.TabStop = false;
            this.groupBoxIslemler.Text = "Yapabildiğim İşlemler";
            // 
            // lstIslemler
            // 
            this.lstIslemler.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstIslemler.FormattingEnabled = true;
            this.lstIslemler.ItemHeight = 23;
            this.lstIslemler.Location = new System.Drawing.Point(10, 25);
            this.lstIslemler.Name = "lstIslemler";
            this.lstIslemler.Size = new System.Drawing.Size(200, 602);
            this.lstIslemler.TabIndex = 0;
            // 
            // FormCalisanPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 750);
            this.Controls.Add(this.groupBoxIslemler);
            this.Controls.Add(this.panelTakvim);
            this.Controls.Add(this.lblCalisan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCalisanPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Çalışan Paneli";
            this.Load += new System.EventHandler(this.FormCalisanPanel_Load_1);
            this.groupBoxIslemler.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
