using System;
using System.Linq;
using System.Windows.Forms;
using kuafor.Models;

namespace kuafor
{
    public partial class FormKayit : Form
    {
        public FormKayit()
        {
            InitializeComponent();
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();
            string sifreTekrar = txtSifreTekrar.Text.Trim();
            string rol = cmbRol.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre) ||
                string.IsNullOrEmpty(sifreTekrar) || string.IsNullOrEmpty(rol))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler uyuşmuyor!");
                return;
            }

            using (var db = new AppDbContext())
            {
                if (db.Kullanicilar.Any(x => x.KullaniciAdi == kullaniciAdi))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten alınmış!");
                    return;
                }

                // 🔹 Rol seçimine göre uygun alt sınıf oluştur
                Kullanici yeniKullanici;

                if (rol == "Admin")
                {
                    yeniKullanici = new Admin();
                }
                else if (rol == "Çalışan")
                {
                    yeniKullanici = new Calisan
                    {
                        Ad = "",
                        Soyad = "",
                        Uzmanlik = ""
                    };
                }
                else // Müşteri
                {
                    yeniKullanici = new Musteri
                    {
                        Ad = "",
                        Soyad = "",
                        Telefon = ""
                    };
                }

                // protected set olduğu için reflection ile değer atıyoruz
                typeof(Kullanici).GetProperty("KullaniciAdi")?.SetValue(yeniKullanici, kullaniciAdi);
                typeof(Kullanici).GetProperty("SifreHash")?.SetValue(yeniKullanici, sifre);

                db.Kullanicilar.Add(yeniKullanici);
                db.SaveChanges();

                MessageBox.Show("Kayıt başarılı! Giriş ekranına yönlendiriliyorsunuz...");
            }

            // 🔹 Giriş formuna dön
            this.Hide();
            FormLogin loginForm = new FormLogin();
            loginForm.ShowDialog();
            this.Close();
        }
    }
}
