namespace kuafor
{
    partial class FormMusteriPanel
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHosgeldin;
        private System.Windows.Forms.ListBox lstRandevular;
        private System.Windows.Forms.Button BtnYeniRandevu;
        private System.Windows.Forms.Button BtnCikis;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHosgeldin = new System.Windows.Forms.Label();
            this.lstRandevular = new System.Windows.Forms.ListBox();
            this.BtnYeniRandevu = new System.Windows.Forms.Button();
            this.BtnCikis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHosgeldin
            // 
            this.lblHosgeldin.AutoSize = true;
            this.lblHosgeldin.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHosgeldin.Location = new System.Drawing.Point(200, 20);
            this.lblHosgeldin.Name = "lblHosgeldin";
            this.lblHosgeldin.Size = new System.Drawing.Size(155, 37);
            this.lblHosgeldin.TabIndex = 0;
            this.lblHosgeldin.Text = "Hoş geldin";
            // 
            // lstRandevular
            // 
            this.lstRandevular.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstRandevular.ItemHeight = 28;
            this.lstRandevular.Location = new System.Drawing.Point(100, 80);
            this.lstRandevular.Name = "lstRandevular";
            this.lstRandevular.Size = new System.Drawing.Size(400, 200);
            this.lstRandevular.TabIndex = 1;
            // 
            // BtnYeniRandevu
            // 
            this.BtnYeniRandevu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnYeniRandevu.Location = new System.Drawing.Point(200, 300);
            this.BtnYeniRandevu.Name = "BtnYeniRandevu";
            this.BtnYeniRandevu.Size = new System.Drawing.Size(200, 40);
            this.BtnYeniRandevu.TabIndex = 2;
            this.BtnYeniRandevu.Text = "Yeni Randevu Oluştur";
            this.BtnYeniRandevu.Click += new System.EventHandler(this.BtnYeniRandevu_Click);
            // 
            // BtnCikis
            // 
            this.BtnCikis.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnCikis.Location = new System.Drawing.Point(200, 350);
            this.BtnCikis.Name = "BtnCikis";
            this.BtnCikis.Size = new System.Drawing.Size(200, 40);
            this.BtnCikis.TabIndex = 3;
            this.BtnCikis.Text = "Çıkış Yap";
            this.BtnCikis.Click += new System.EventHandler(this.BtnCikis_Click);
            // 
            // FormMusteriPanel
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.lblHosgeldin);
            this.Controls.Add(this.lstRandevular);
            this.Controls.Add(this.BtnYeniRandevu);
            this.Controls.Add(this.BtnCikis);
            this.Name = "FormMusteriPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Müşteri Paneli";
            this.Load += new System.EventHandler(this.FormMusteriPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
