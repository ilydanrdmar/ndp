using kuafor.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace kuafor
{
    public partial class FormCalisanPanel : Form
    {
        private readonly Calisan _calisan;
        private readonly AppDbContext db = new AppDbContext();

        private int StartHour;
        private int EndHour;

        private const int HourHeight = 40;
        private const int DayWidth = 110;

        string[] Gunler = { "Pzt", "Salı", "Çar", "Per", "Cum", "Cts", "Paz" };

        public FormCalisanPanel(Calisan calisan)
        {
            InitializeComponent();
            _calisan = calisan;

            // 🔥 Form yüklenince takvim ve işlemler gelsin
            this.Load += FormCalisanPanel_Load;
        }

        private void FormCalisanPanel_Load(object sender, EventArgs e)
        {
            // Üst başlık
            lblCalisan.Text = $"{_calisan.Ad} {_calisan.Soyad}  |  Uzmanlık: {_calisan.Uzmanlik}";

            // SALON ÇALIŞMA SAATLERİ
            StartHour = _calisan.Salon.CalismaBaslangic.Hours;
            EndHour = _calisan.Salon.CalismaBitis.Hours;

            LoadYapabildigiIslemler();
            LoadTakvimGrid();
        }

        // Çalışanın yapabildiği işlemler (Admin tarafından atanmış)
        private void LoadYapabildigiIslemler()
        {
            lstIslemler.Items.Clear();
            var islemler = db.Calisanlar
                .Where(c => c.Id == _calisan.Id)
                .SelectMany(c => c.Islemler)
                .ToList();

            foreach (var islem in islemler)
                lstIslemler.Items.Add($"• {islem.Ad} ({islem.SureDakika} dk)");
        }

        private void LoadTakvimGrid()
        {
            panelTakvim.Controls.Clear();

            Panel grid = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(DayWidth * 7 + 60, (EndHour - StartHour) * HourHeight + 60),
                AutoScroll = true
            };

            panelTakvim.Controls.Add(grid);

            // GÜN BAŞLIKLARI
            for (int d = 0; d < 7; d++)
            {
                Label gunLbl = new Label
                {
                    Text = Gunler[d],
                    Location = new Point(d * DayWidth + 50, 10),
                    Size = new Size(DayWidth, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                grid.Controls.Add(gunLbl);
            }

            // SAATLER
            for (int saat = StartHour; saat <= EndHour; saat++)
            {
                Label lbl = new Label
                {
                    Text = $"{saat}:00",
                    Location = new Point(5, (saat - StartHour) * HourHeight + 50),
                    Size = new Size(45, 30)
                };
                grid.Controls.Add(lbl);
            }

            // HÜCRELER
            for (int gun = 0; gun < 7; gun++)
            {
                for (int saat = StartHour; saat < EndHour; saat++)
                {
                    Panel cell = new Panel
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White,
                        Location = new Point(gun * DayWidth + 50, (saat - StartHour) * HourHeight + 50),
                        Size = new Size(DayWidth, HourHeight)
                    };
                    grid.Controls.Add(cell);
                }
            }

            // UYGUNLUK EKLE
            UygunluklariIsle(grid);

            // RANDEVULARI EKLE
            TakvimeRandevuEkle(grid);
        }

        private void UygunluklariIsle(Panel grid)
        {
            var uygunluklar = db.CalisanUygunluklar
                .Where(u => u.CalisanId == _calisan.Id)
                .ToList();

            foreach (var u in uygunluklar)
            {
                int gun = u.Gun - 1;

                for (int saat = StartHour; saat < EndHour; saat++)
                {
                    bool calisiyor = saat >= u.Baslangic.Hours && saat < u.Bitis.Hours;

                    var cell = grid.Controls.OfType<Panel>()
                        .FirstOrDefault(p =>
                            p.Location.X == gun * DayWidth + 50 &&
                            p.Location.Y == (saat - StartHour) * HourHeight + 50
                        );

                    if (cell != null)
                        cell.BackColor = calisiyor ? Color.White : Color.LightGray;
                }
            }
        }

        private void TakvimeRandevuEkle(Panel grid)
        {
            var randevular = db.Randevular
                .Where(r => r.CalisanId == _calisan.Id)
                .ToList();

            foreach (var r in randevular)
            {
                int gunIndex = ((int)r.Baslangic.DayOfWeek + 6) % 7;

                int basSaat = r.Baslangic.Hour;
                int dakika = r.Baslangic.Minute;
                int sure = (int)(r.Bitis - r.Baslangic).TotalMinutes;

                int topOffset = (basSaat - StartHour) * HourHeight + (dakika * HourHeight / 60);

                Panel blok = new Panel
                {
                    Location = new Point(gunIndex * DayWidth + 50, topOffset + 50),
                    Size = new Size(DayWidth - 3, (sure * HourHeight / 60) - 2),
                    BackColor = r.OnayDurumu == 1 ? Color.LightGreen :
                                r.OnayDurumu == 2 ? Color.LightCoral :
                                                    Color.LightSkyBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = r
                };

                Label lbl = new Label
                {
                    Text = $"{r.Islem.Ad}\n{r.Musteri.Ad}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                blok.Controls.Add(lbl);
                blok.Click += RandevuDetayAc;

                grid.Controls.Add(blok);
                blok.BringToFront();
            }
        }

        private void RandevuDetayAc(object sender, EventArgs e)
        {
            Panel blok = (Panel)sender;
            Randevu r = (Randevu)blok.Tag;

            string mesaj =
                $"📌 Müşteri: {r.Musteri.Ad} {r.Musteri.Soyad}\n" +
                $"✂ İşlem: {r.Islem.Ad}\n" +
                $"📅 Tarih: {r.Baslangic:dd.MM.yyyy HH:mm}\n" +
                $"⏳ Süre: {(r.Bitis - r.Baslangic).TotalMinutes} dakika\n" +
                $"📍 Durum: {(r.OnayDurumu == 0 ? "Bekliyor" : r.OnayDurumu == 1 ? "Onaylandı" : "Reddedildi")}\n\n" +
                "Randevu için işlem seçiniz:";

            var result = MessageBox.Show(
                mesaj,
                "Randevu Detayı",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                r.OnayDurumu = 1;
                db.SaveChanges();
                MessageBox.Show("✔ Randevu Onaylandı");
            }
            else if (result == DialogResult.No)
            {
                r.OnayDurumu = 2;
                db.SaveChanges();
                MessageBox.Show("❌ Randevu Reddedildi");
            }

            LoadTakvimGrid();
        }

        private void FormCalisanPanel_Load_1(object sender, EventArgs e)
        {

        }
    }
}
