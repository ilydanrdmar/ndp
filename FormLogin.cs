using System;
using System.Linq;
using System.Windows.Forms;
using kuafor.Models;

namespace kuafor
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz!");
                return;
            }

            using (var db = new AppDbContext())
            {
                var kullanici = db.Kullanicilar
                    .FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi && u.SifreHash == sifre);

                if (kullanici == null)
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                    return;
                }

                MessageBox.Show($"Hoş geldin {kullanici.KullaniciAdi}! ({kullanici.RolAdi})");

                // 🔹 Rol'e göre yönlendirme
                Form targetForm = null;

                if (kullanici is Admin)
                {
                    targetForm = new FormAdminPanel();
                }
                else if (kullanici is Calisan)
                {
                    targetForm = new FormCalisanPanel();
                }
                else if (kullanici is Musteri)
                {
                    targetForm = new FormMusteriPanel();
                }

                if (targetForm != null)
                {
                    // 🔹 Login formunu gizle, yeni formu göster
                    this.Hide();
                    targetForm.ShowDialog();

                    // 🔹 Kapatıldığında tekrar login ekranını göster
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Tanımlı bir rol bulunamadı!");
                }
            }
        }

        private void linkKayit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormKayit kayitFormu = new FormKayit();
            kayitFormu.ShowDialog();
        }

       
    }
}
