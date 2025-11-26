namespace kuafor
{
    partial class FormMusteriPanel
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelSol;
        private System.Windows.Forms.Button btnRandevularim;
        private System.Windows.Forms.Button btnYeniRandevu;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.Label lblKullanici;
        private System.Windows.Forms.Panel panelContainer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSol = new System.Windows.Forms.Panel();
            this.btnYeniRandevu = new System.Windows.Forms.Button();
            this.btnRandevularim = new System.Windows.Forms.Button();
            this.btnCikis = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.lblKullanici = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();

            this.panelSol.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelSol
            // 
            this.panelSol.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.panelSol.Controls.Add(this.btnYeniRandevu);
            this.panelSol.Controls.Add(this.btnRandevularim);
            this.panelSol.Controls.Add(this.btnCikis);
            this.panelSol.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSol.Location = new System.Drawing.Point(0, 0);
            this.panelSol.Name = "panelSol";
            this.panelSol.Size = new System.Drawing.Size(200, 700);

            // 
            // btnYeniRandevu
            // 
            this.btnYeniRandevu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnYeniRandevu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniRandevu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnYeniRandevu.ForeColor = System.Drawing.Color.White;
            this.btnYeniRandevu.Location = new System.Drawing.Point(0, 60);
            this.btnYeniRandevu.Size = new System.Drawing.Size(200, 60);
            this.btnYeniRandevu.Text = "Yeni Randevu";
            this.btnYeniRandevu.Click += new System.EventHandler(this.btnYeniRandevu_Click);

            // 
            // btnRandevularim
            // 
            this.btnRandevularim.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRandevularim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRandevularim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRandevularim.ForeColor = System.Drawing.Color.White;
            this.btnRandevularim.Location = new System.Drawing.Point(0, 0);
            this.btnRandevularim.Size = new System.Drawing.Size(200, 60);
            this.btnRandevularim.Text = "Randevularım";
            this.btnRandevularim.Click += new System.EventHandler(this.btnRandevularim_Click);

            // 
            // btnCikis
            // 
            this.btnCikis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCikis.ForeColor = System.Drawing.Color.White;
            this.btnCikis.Size = new System.Drawing.Size(200, 60);
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);

            // 
            // panelUst
            // 
            this.panelUst.BackColor = System.Drawing.Color.White;
            this.panelUst.Controls.Add(this.lblKullanici);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(200, 0);
            this.panelUst.Size = new System.Drawing.Size(900, 70);

            // 
            // lblKullanici
            // 
            this.lblKullanici.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblKullanici.Location = new System.Drawing.Point(20, 20);
            this.lblKullanici.AutoSize = true;

            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(200, 70);
            this.panelContainer.BackColor = System.Drawing.Color.White;

            // 
            // FormMusteriPanel
            // 
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelSol);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "FormMusteriPanel";
            this.Text = "Müşteri Paneli";

            this.panelSol.ResumeLayout(false);
            this.panelUst.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
