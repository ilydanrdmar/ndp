using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using proje.Models;

namespace proje
{
    public partial class FormMusteriPanel : Form
    {
        private readonly Musteri _aktifMusteri;
        private readonly AppDbContext db = new AppDbContext();

        // Takvim yönetimi
        private DateTime aktifAy = DateTime.Now;
        private DateTime seciliTarih = DateTime.MinValue;
        private TimeSpan seciliSaat;

        // Seçimler
        private Islem seciliIslem;
        private Calisan seciliCalisan;

        // Dinamik UI bileşenleri
        private ComboBox cmbIslem, cmbCalisan;
        private FlowLayoutPanel panelTakvim, panelSaatler;
        private Label lblAy;

        public FormMusteriPanel(Musteri musteri)
        {
            _aktifMusteri = musteri;
            InitializeComponent();

            lblKullanici.Text = $"Hoş geldin {_aktifMusteri.KullaniciAdi}";

            // Varsayılan olarak randevu listesi açılır
            ShowRandevular();
        }

        // ============================================================
        // 1 — Randevularım (DataGrid)
        // ============================================================
        private void ShowRandevular()
        {
            panelContainer.Controls.Clear();

            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            panelContainer.Controls.Add(grid);

            var liste = db.Randevular
                .Include(r => r.Islem)
                .Include(r => r.Calisan)
                .Where(r => r.MusteriId == _aktifMusteri.Id)
                .OrderByDescending(r => r.Baslangic)
                .Select(r => new
                {
                    İşlem = r.Islem.Ad,
                    Çalışan = r.Calisan.Ad + " " + r.Calisan.Soyad,
                    Başlangıç = r.Baslangic,
                    Bitiş = r.Bitis,
                    Durum = r.OnayDurumu == 0 ? "Bekliyor" :
                            r.OnayDurumu == 1 ? "Onaylandı" : "Reddedildi"
                })
                .ToList();

            grid.DataSource = liste;
        }

        // ============================================================
        // 2 — Yeni Randevu Ekranı
        // ============================================================
        private void ShowRandevuOlustur()
        {
            panelContainer.Controls.Clear();

            Panel p = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            panelContainer.Controls.Add(p);

            // --- İşlem Seçimi ---
            Label lbl1 = new Label
            {
                Text = "İşlem Seç:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 20)
            };
            p.Controls.Add(lbl1);

            cmbIslem = new ComboBox
            {
                Location = new Point(20, 55),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbIslem.SelectedIndexChanged += CmbIslem_Changed;
            p.Controls.Add(cmbIslem);

            cmbIslem.DataSource = db.Islemler.GroupBy(i => i.Ad).Select(g => g.First()).ToList();
            cmbIslem.DisplayMember = "Ad";
            cmbIslem.ValueMember = "Id";

            // --- Çalışan seçimi ---
            Label lbl2 = new Label
            {
                Text = "Çalışan Seç:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 110)
            };
            p.Controls.Add(lbl2);

            cmbCalisan = new ComboBox
            {
                Location = new Point(20, 145),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCalisan.SelectedIndexChanged += CmbCalisan_Changed;
            p.Controls.Add(cmbCalisan);

            // --- Ay başlığı ---
            lblAy = new Label
            {
                Text = aktifAy.ToString("MMMM yyyy"),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 200),
                Width = 300
            };
            p.Controls.Add(lblAy);

            Button btnPrev = new Button
            {
                Text = "◀",
                Location = new Point(320, 200),
                Width = 40,
                Height = 35
            };
            btnPrev.Click += (s, e) =>
            {
                aktifAy = aktifAy.AddMonths(-1);
                lblAy.Text = aktifAy.ToString("MMMM yyyy");
                LoadTakvim();
            };
            p.Controls.Add(btnPrev);

            Button btnNext = new Button
            {
                Text = "▶",
                Location = new Point(370, 200),
                Width = 40,
                Height = 35
            };
            btnNext.Click += (s, e) =>
            {
                aktifAy = aktifAy.AddMonths(1);
                lblAy.Text = aktifAy.ToString("MMMM yyyy");
                LoadTakvim();
            };
            p.Controls.Add(btnNext);

            // --- Takvim paneli ---
            panelTakvim = new FlowLayoutPanel
            {
                Location = new Point(20, 250),
                Width = 450,
                Height = 230,
                BackColor = Color.WhiteSmoke
            };
            p.Controls.Add(panelTakvim);

            LoadTakvim();

            // --- Saat paneli ---
            Label lbl3 = new Label
            {
                Text = "Saat Seç:",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 500)
            };
            p.Controls.Add(lbl3);

            panelSaatler = new FlowLayoutPanel
            {
                Location = new Point(20, 540),
                Width = 450,
                Height = 200
            };
            p.Controls.Add(panelSaatler);
        }

        // ============================================================
        // TAKVİM
        // ============================================================
        private void LoadTakvim()
        {
            panelTakvim.Controls.Clear();

            int days = DateTime.DaysInMonth(aktifAy.Year, aktifAy.Month);

            for (int i = 1; i <= days; i++)
            {
                DateTime g = new DateTime(aktifAy.Year, aktifAy.Month, i);

                Button btn = new Button
                {
                    Text = i.ToString(),
                    Width = 60,
                    Height = 45,
                    BackColor = Color.White,
                    Tag = g
                };

                btn.Click += GunSecildi;
                panelTakvim.Controls.Add(btn);
            }
        }

        private void GunSecildi(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            seciliTarih = (DateTime)b.Tag;

            foreach (Button x in panelTakvim.Controls)
                x.BackColor = Color.White;

            b.BackColor = Color.MediumSlateBlue;

            LoadSaatler();
        }

        // ============================================================
        // İŞLEM SEÇİLDİ
        // ============================================================
        private void CmbIslem_Changed(object sender, EventArgs e)
        {
            if (cmbCalisan == null)
                return; // kritik fix: null referansı engeller

            seciliIslem = cmbIslem.SelectedItem as Islem;
            seciliCalisan = null;

            if (seciliIslem == null)
            {
                cmbCalisan.DataSource = null;
                return;
            }

            var calisanlar = db.Calisanlar
                .Include(c => c.Islemler)
                .Where(c => c.Islemler.Any(i => i.Id == seciliIslem.Id))
                .ToList();

            cmbCalisan.DataSource = calisanlar;
            cmbCalisan.DisplayMember = "Ad";
            cmbCalisan.ValueMember = "Id";

            panelSaatler.Controls.Clear();
        }


        // ============================================================
        // ÇALIŞAN SEÇİLDİ
        // ============================================================
        private void CmbCalisan_Changed(object sender, EventArgs e)
        {
            seciliCalisan = cmbCalisan.SelectedItem as Calisan;
            LoadSaatler();
        }

        // ============================================================
        // SAAT SLOT YÜKLEME
        // ============================================================
        private void LoadSaatler()
        {
            panelSaatler.Controls.Clear();

            if (seciliCalisan == null || seciliIslem == null || seciliTarih == DateTime.MinValue)
                return;

            byte gun = (byte)((int)seciliTarih.DayOfWeek == 0 ? 7 : (int)seciliTarih.DayOfWeek);

            var uygunluk = db.CalisanUygunluklar
                .FirstOrDefault(u => u.CalisanId == seciliCalisan.Id && u.Gun == gun);

            if (uygunluk == null)
                return;

            // Dolu saatler
            var doluSaatler = db.Randevular
                .Where(r => r.CalisanId == seciliCalisan.Id && r.Baslangic.Date == seciliTarih.Date)
                .Select(r => r.Baslangic.TimeOfDay)
                .ToList();

            for (TimeSpan t = uygunluk.Baslangic; t < uygunluk.Bitis; t = t.Add(TimeSpan.FromMinutes(30)))
            {
                if (doluSaatler.Contains(t))
                    continue;

                Button btn = new Button
                {
                    Width = 80,
                    Height = 40,
                    Text = $"{t:hh\\:mm}",
                    BackColor = Color.White,
                    Tag = t
                };

                btn.Click += SaatSecildi;
                panelSaatler.Controls.Add(btn);
            }
        }

        private void SaatSecildi(object sender, EventArgs e)
        {
            foreach (Button b in panelSaatler.Controls)
                b.BackColor = Color.White;

            Button secilen = (Button)sender;
            secilen.BackColor = Color.MediumSlateBlue;

            seciliSaat = (TimeSpan)secilen.Tag;

            RandevuOlustur();
        }

        // ============================================================
        // Randevu oluşturma + uygunluk kontrolü
        // ============================================================
        private void RandevuOlustur()
        {
            DateTime bas = seciliTarih.Date + seciliSaat;
            DateTime bit = bas.AddMinutes(seciliIslem.SureDakika);

            if (!RandevuUygunMu(bas, bit))
            {
                MessageBox.Show("Bu saatlerde çalışan uygun değil!");
                return;
            }

            var rn = new Randevu
            {
                MusteriId = _aktifMusteri.Id,
                CalisanId = seciliCalisan.Id,
                IslemId = seciliIslem.Id,
                Baslangic = bas,
                Bitis = bit,
                OnayDurumu = 0
            };

            db.Randevular.Add(rn);
            db.SaveChanges();

            MessageBox.Show("Randevu başarıyla oluşturuldu 💜");

            ShowRandevular();
        }

        // ============================================================
        // KAPSÜLLÜ UYGUNLUK KONTROLÜ
        // ============================================================
        private bool RandevuUygunMu(DateTime bas, DateTime bit)
        {
            byte gun = (byte)((int)bas.DayOfWeek == 0 ? 7 : (int)bas.DayOfWeek);

            var uygun = db.CalisanUygunluklar
                .FirstOrDefault(u => u.CalisanId == seciliCalisan.Id && u.Gun == gun);

            if (uygun == null ||
                bas.TimeOfDay < uygun.Baslangic ||
                bit.TimeOfDay > uygun.Bitis)
                return false;

            var mevcut = db.Randevular
                .Where(r => r.CalisanId == seciliCalisan.Id && r.Baslangic.Date == bas.Date)
                .ToList();

            foreach (var r in mevcut)
                if (!(bit <= r.Baslangic || bas >= r.Bitis))
                    return false;

            return true;
        }

        // ============================================================
        // SOL MENÜ BUTONLARI
        // ============================================================
        private void btnRandevularim_Click(object sender, EventArgs e)
        {
            ShowRandevular();
        }

        private void btnYeniRandevu_Click(object sender, EventArgs e)
        {
            ShowRandevuOlustur();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
