using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kuafor.Models;

namespace kuafor
{
    public partial class FormMusteriPanel : Form
    {
        private Musteri _aktifMusteri;

        public FormMusteriPanel(Musteri musteri)
        {
            _aktifMusteri = musteri;
            InitializeComponent();

            lblHosgeldin.Text = $"Hoş geldin {_aktifMusteri.KullaniciAdi}";
            LoadRandevular();
        }

        private void LoadRandevular()
        {
            lstRandevular.Items.Clear();

            using (var db = new AppDbContext())
            {
                var randevular = db.Randevular
                    .Where(r => r.MusteriId == _aktifMusteri.Id)
                    .ToList();

                if (randevular.Count == 0)
                {
                    lstRandevular.Items.Add("Randevu bulunmuyor.");
                    return;
                }

                foreach (var r in randevular)
                {
                    lstRandevular.Items.Add($"{r.Baslangic:dd.MM.yyyy HH:mm} - {r.Islem.Ad}");

                }
            }
        }

        private void BtnYeniRandevu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Randevu oluşturma ekranı daha sonra eklenecek.");
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMusteriPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
