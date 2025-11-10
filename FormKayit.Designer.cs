using System;
using System.Windows.Forms;
using System.Drawing;

namespace kuafor
{
    public partial class FormKayit : Form
    {
        private void InitializeComponent()
        {
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblKullaniciAdi = new System.Windows.Forms.Label();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.lblSifre = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.lblSifreTekrar = new System.Windows.Forms.Label();
            this.txtSifreTekrar = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.btnKayitOl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBaslik
            // 
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBaslik.Location = new System.Drawing.Point(90, 20);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(267, 37);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Yeni Kullanıcı Kaydı";
            // 
            // lblKullaniciAdi
            // 
            this.lblKullaniciAdi.Location = new System.Drawing.Point(40, 80);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(100, 23);
            this.lblKullaniciAdi.TabIndex = 1;
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Location = new System.Drawing.Point(150, 75);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(180, 27);
            this.txtKullaniciAdi.TabIndex = 2;
            // 
            // lblSifre
            // 
            this.lblSifre.Location = new System.Drawing.Point(40, 120);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new System.Drawing.Size(100, 23);
            this.lblSifre.TabIndex = 3;
            this.lblSifre.Text = "Şifre:";
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(150, 115);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(180, 27);
            this.txtSifre.TabIndex = 4;
            // 
            // lblSifreTekrar
            // 
            this.lblSifreTekrar.Location = new System.Drawing.Point(40, 160);
            this.lblSifreTekrar.Name = "lblSifreTekrar";
            this.lblSifreTekrar.Size = new System.Drawing.Size(100, 23);
            this.lblSifreTekrar.TabIndex = 5;
            this.lblSifreTekrar.Text = "Şifre Tekrar:";
            // 
            // txtSifreTekrar
            // 
            this.txtSifreTekrar.Location = new System.Drawing.Point(150, 155);
            this.txtSifreTekrar.Name = "txtSifreTekrar";
            this.txtSifreTekrar.PasswordChar = '*';
            this.txtSifreTekrar.Size = new System.Drawing.Size(180, 27);
            this.txtSifreTekrar.TabIndex = 6;
            // 
            // lblRol
            // 
            this.lblRol.Location = new System.Drawing.Point(40, 200);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(100, 23);
            this.lblRol.TabIndex = 7;
            this.lblRol.Text = "Rol:";
            // 
            // cmbRol
            // 
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] {
            "Müşteri",
            "Çalışan"});
            this.cmbRol.Location = new System.Drawing.Point(150, 195);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(180, 28);
            this.cmbRol.TabIndex = 8;
            // 
            // btnKayitOl
            // 
            this.btnKayitOl.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnKayitOl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKayitOl.ForeColor = System.Drawing.Color.White;
            this.btnKayitOl.Location = new System.Drawing.Point(150, 240);
            this.btnKayitOl.Name = "btnKayitOl";
            this.btnKayitOl.Size = new System.Drawing.Size(180, 35);
            this.btnKayitOl.TabIndex = 9;
            this.btnKayitOl.Text = "Kayıt Ol";
            this.btnKayitOl.UseVisualStyleBackColor = false;
            this.btnKayitOl.Click += new System.EventHandler(this.btnKayitOl_Click);
            // 
            // FormKayit
            // 
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Controls.Add(this.lblBaslik);
            this.Controls.Add(this.lblKullaniciAdi);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.lblSifre);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.lblSifreTekrar);
            this.Controls.Add(this.txtSifreTekrar);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnKayitOl);
            this.Name = "FormKayit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kayıt Ol";
            //this.Load += new System.EventHandler(this.FormKayit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblBaslik, lblKullaniciAdi, lblSifre, lblSifreTekrar, lblRol;
        private TextBox txtKullaniciAdi, txtSifre, txtSifreTekrar;
        private ComboBox cmbRol;
        private Button btnKayitOl;
    }
}
