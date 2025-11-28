using kuafor.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

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
        // 📌 Haftalık takvim için
        private DateTime aktifHafta = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
        private Button btnOncekiHafta;
        private Button btnSonrakiHafta;
        private Label lblHaftaAraligi;


        string[] Gunler = { "Pzt", "Salı", "Çar", "Per", "Cum", "Cts", "Paz" };

        public FormCalisanPanel(Calisan calisan)
        {
            InitializeComponent();
            _calisan = calisan;

            this.Load += FormCalisanPanel_Load;
        }

        private void FormCalisanPanel_Load(object sender, EventArgs e)
        {
            if (_calisan.Salon == null)
            {
                MessageBox.Show("Bu çalışanın bağlı olduğu salon bulunamadı!",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblCalisan.Text = $"{_calisan.Ad} {_calisan.Soyad} | Uzmanlık: {_calisan.Uzmanlik}";

            StartHour = _calisan.Salon.CalismaBaslangic.Hours;
            EndHour = _calisan.Salon.CalismaBitis.Hours;

            LoadYapabildigiIslemler();
            CreateWeekHeader();
            LoadTakvimGrid();


        }
        private bool headerCreated = false;

        private void CreateWeekHeader()
        {
            if (headerCreated) return; // ⭐ zaten oluşturulduysa tekrar yapma
            headerCreated = true;

            // ◀ Önceki Hafta
            btnOncekiHafta = new Button
            {
                Text = "◀",
                Width = 40,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(panelTakvim.Left + 10, panelTakvim.Top - 40)
            };
            btnOncekiHafta.Click += (s, e) =>
            {
                aktifHafta = aktifHafta.AddDays(-7);
                LoadTakvimGrid();
            };
            this.Controls.Add(btnOncekiHafta);

            // ▶ Sonraki Hafta
            btnSonrakiHafta = new Button
            {
                Text = "▶",
                Width = 40,
                Height = 30,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(panelTakvim.Right - 50, panelTakvim.Top - 40)
            };
            btnSonrakiHafta.Click += (s, e) =>
            {
                aktifHafta = aktifHafta.AddDays(7);
                LoadTakvimGrid();
            };
            this.Controls.Add(btnSonrakiHafta);

            // 📅 Hafta aralığı label
            lblHaftaAraligi = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(panelTakvim.Left + 200, panelTakvim.Top - 35),
                Text = "" // ilk değer, grid yüklenince dolacak
            };

            this.Controls.Add(lblHaftaAraligi);
        }



        // Çalışanın yapabildiği işlemleri yükler
        private void LoadYapabildigiIslemler()
        {
            lstIslemler.Items.Clear();

            var islemler = db.Calisanlar
                .Include("Islemler")
                .First(c => c.Id == _calisan.Id)
                .Islemler
                .ToList();

            foreach (var islem in islemler)
                lstIslemler.Items.Add($"• {islem.Ad} ({islem.SureDakika} dk)");
        }

        // Takvimi çizer
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

            // 📅 Haftalık tarih aralığını label’da göster
            DateTime bas = aktifHafta;
            DateTime bit = aktifHafta.AddDays(6);
            lblHaftaAraligi.Text = $"{bas:dd MMMM} – {bit:dd MMMM}";

            // 📌 Gün başlıkları (tarihli)
            for (int d = 0; d < 7; d++)
            {
                DateTime tarih = aktifHafta.AddDays(d);

                Label gunLbl = new Label
                {
                    Text = $"{Gunler[d]}\n{tarih:dd}",
                    Location = new Point(d * DayWidth + 50, 10),
                    Size = new Size(DayWidth, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                grid.Controls.Add(gunLbl);
            }


            // Saat etiketleri
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

            // Boş hücreleri çiz
            for (int gun = 0; gun < 7; gun++)
            {
                for (int saat = StartHour; saat < EndHour; saat++)
                {
                    Panel cell = new Panel
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.LightGray,
                        Location = new Point(gun * DayWidth + 50, (saat - StartHour) * HourHeight + 50),
                        Size = new Size(DayWidth, HourHeight)
                    };
                    grid.Controls.Add(cell);
                }
            }

            UygunluklariIsle(grid);
            TakvimeRandevuEkle(grid);
        }

        // Uygunlukları beyaz renkle boyar
        private void UygunluklariIsle(Panel grid)
        {
            var uygunluklar = db.CalisanUygunlukTarihleri
                .Where(u => u.CalisanId == _calisan.Id)
                .ToList();

            for (int d = 0; d < 7; d++)
            {
                DateTime gunTarihi = aktifHafta.AddDays(d);

                var oGunUygunluk = uygunluklar
                    .Where(u => u.Tarih.Date == gunTarihi.Date)
                    .ToList();

                if (oGunUygunluk.Count == 0)
                    continue;

                foreach (var u in oGunUygunluk)
                {
                    for (int saat = StartHour; saat < EndHour; saat++)
                    {
                        TimeSpan cellStart = new TimeSpan(saat, 0, 0);
                        TimeSpan cellEnd = cellStart.Add(TimeSpan.FromHours(1));

                        bool calisiyor = u.Baslangic < cellEnd && u.Bitis > cellStart;

                        var cell = grid.Controls.OfType<Panel>()
                            .FirstOrDefault(p =>
                                p.Location.X == d * DayWidth + 50 &&
                                p.Location.Y == (saat - StartHour) * HourHeight + 50
                            );

                        if (cell != null)
                            cell.BackColor = calisiyor ? Color.White : Color.LightGray;
                    }
                }
            }
        }


        private void TakvimeRandevuEkle(Panel grid)
        {
            var randevular = db.Randevular
                .Where(r => r.CalisanId == _calisan.Id)
                .Include("Islem")
                .Include("Musteri")
                .ToList();

            foreach (var r in randevular)
            {

                // 📌 Bu randevu aktif haftaya ait değilse çizme
                DateTime rGun = r.Baslangic.Date;
                if (rGun < aktifHafta || rGun > aktifHafta.AddDays(6))
                    continue;

                int gunIndex = (int)(rGun - aktifHafta).TotalDays;

                int basSaat = r.Baslangic.Hour;
                int dakika = r.Baslangic.Minute;
                int sure = (int)(r.Bitis - r.Baslangic).TotalMinutes;

                int topOffset =
                    (basSaat - StartHour) * HourHeight +
                    (dakika * HourHeight / 60);

                Panel blok = new Panel
                {
                    Location = new Point(gunIndex * DayWidth + 50, topOffset + 50),
                    Size = new Size(DayWidth - 3, (sure * HourHeight / 60) - 2),
                    BackColor =
                        r.OnayDurumu == 1 ? Color.LightGreen :
                        r.OnayDurumu == 2 ? Color.LightCoral :
                                            Color.LightSkyBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = r
                };

                Label lbl = new Label
                {
                    Text = $"{r.Islem.Ad}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                blok.Controls.Add(lbl);

                // 🟢 Tıklamayı hem blok hem de label yakalasın
                blok.Click += RandevuDetayAc;
                lbl.Click += RandevuDetayAc;

                grid.Controls.Add(blok);
                blok.BringToFront();
            }
        }


        // Randevuya tıklayınca popup açılır
        private void RandevuDetayAc(object sender, EventArgs e)
        {
            // Eğer label'a tıkladıysan paneli Parent üzerinden al
            Panel blok;

            if (sender is Label lbl)
                blok = (Panel)lbl.Parent;
            else
                blok = (Panel)sender;

            Randevu r = (Randevu)blok.Tag;

            string mesaj =
                $"📌 Müşteri: {r.Musteri.Ad} {r.Musteri.Soyad}\n" +
                $"✂ İşlem: {r.Islem.Ad}\n" +
                $"📅 Tarih: {r.Baslangic:dd.MM.yyyy HH:mm}\n" +
                $"⏳ Süre: {(r.Bitis - r.Baslangic).TotalMinutes} dk\n" +
                $"📍 Durum: {(r.OnayDurumu == 0 ? "Bekliyor" : r.OnayDurumu == 1 ? "Onaylandı" : "Reddedildi")}\n\n";

            var result = MessageBox.Show(
                mesaj,
                "Randevu Detayı",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                r.OnayDurumu = 1; // onayla
                db.SaveChanges();
            }
            else if (result == DialogResult.No)
            {
                r.OnayDurumu = 2; // reddet
                db.SaveChanges();
            }

            LoadTakvimGrid();
        }


        // Uygunluk ekleme butonu
        private void btnUygunlukEkle_Click(object sender, EventArgs e)
        {
            var f = new FormCalisanUygunlukEkle(_calisan);
            f.ShowDialog();
            LoadTakvimGrid();
        }
        private void btnRandevularim_Click(object sender, EventArgs e)
        {
            panelTakvim.Visible = false;
            panelMain.Visible = true;

            LoadCalisanRandevularim();
        }
        private void LoadCalisanRandevularim()
        {
            panelMain.Controls.Clear();

            Label lbl = new Label
            {
                Text = "📅 Randevularım",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(30, 20),
                AutoSize = true
            };
            panelMain.Controls.Add(lbl);

            DataGridView dgv = new DataGridView
            {
                Location = new Point(30, 70),
                Size = new Size(900, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelMain.Controls.Add(dgv);

            var data = db.Randevular
                .Where(r => r.CalisanId == _calisan.Id)
                .Select(r => new
                {
                    r.Id,
                    Müşteri = r.Musteri.Ad + " " + r.Musteri.Soyad,
                    İşlem = r.Islem.Ad,
                    Başlangıç = r.Baslangic,
                    Bitiş = r.Bitis,
                    Durum = r.OnayDurumu == 0 ? "Bekliyor" :
                             r.OnayDurumu == 1 ? "Onaylandı" :
                             "Reddedildi"
                })
                .OrderByDescending(r => r.Başlangıç)
                .ToList();

            dgv.DataSource = data;

            // Renkleme
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string durum = row.Cells["Durum"].Value?.ToString();
                if (durum == "Onaylandı")
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                else if (durum == "Reddedildi")
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        private void btnTakvim_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;
            panelTakvim.Visible = true;

            LoadTakvimGrid();
        }
        private void btnTakvimeDon_Click(object sender, EventArgs e)
        {
            panelMain.Visible = false;   // randevular ekranını gizle
            panelTakvim.Visible = true;  // takvimi geri göster
            LoadTakvimGrid();            // takvimi tazele
        }


    }
}
//en sonki hal
