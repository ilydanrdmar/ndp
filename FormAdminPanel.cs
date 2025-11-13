using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using kuafor.Models; // ✅ DbContext ve Salon modelin buradan geliyor

namespace kuafor
{
    public partial class FormAdminPanel : Form
    {
        private Panel panelMenu;
        private Panel panelMain;
        private Label lblBaslik;

        private Button btnSalonlar;
        private Button btnCalisanlar;
        private Button btnIslemler;
        private Button btnRandevular;
        private Button btnCikis;

        private readonly AppDbContext db = new AppDbContext();

        public FormAdminPanel()
        {
            InitializeComponent();
            this.ClientSize = new Size(1300, 700); // 💡 ekranı genişlettik
        }

        // 🔹 Form yüklendiğinde dinamik panel ve butonları oluştur
        private void FormAdminPanel_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // 🔹 Sol Menü Paneli
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.MediumSlateBlue
            };

            // 🔹 Başlık
            lblBaslik = new Label
            {
                Text = "👑 Admin Paneli",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            panelMenu.Controls.Add(lblBaslik);

            // 🔹 Butonlar
            btnSalonlar = CreateMenuButton("🏠 Salonlar", 80);
            btnIslemler = CreateMenuButton("💇 İşlemler", 130);
            btnCalisanlar = CreateMenuButton("👩‍💼 Çalışanlar", 180);
            btnRandevular = CreateMenuButton("📅 Randevular", 230);
            btnCikis = CreateMenuButton("🚪 Çıkış", 500);

            // 🔹 Eventler
            btnSalonlar.Click += (s, ev) => LoadSalonYonetimi();
            btnIslemler.Click += (s, ev) => LoadIslemYonetimi();
            btnCalisanlar.Click += (s, ev) => LoadCalisanYonetimi();
            btnRandevular.Click += (s, ev) => LoadRandevuYonetimi();
            btnCikis.Click += BtnCikis_Click;

            // 🔹 Menüye ekle
            panelMenu.Controls.Add(btnSalonlar);
            panelMenu.Controls.Add(btnIslemler);
            panelMenu.Controls.Add(btnCalisanlar);
            panelMenu.Controls.Add(btnRandevular);
            panelMenu.Controls.Add(btnCikis);

            // 🔹 Ana Panel
            panelMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // 🔹 Form’a ekle
            this.Controls.Add(panelMain);
            this.Controls.Add(panelMenu);

            // 🔹 Başlangıçta gösterilecek sayfa
            LoadSalonYonetimi();
        }

        private Button CreateMenuButton(string text, int top)
        {
            var btn = new Button
            {
                Text = text,
                Width = 180,
                Height = 40,
                Left = 10,
                Top = top,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.MediumSlateBlue,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.SlateBlue;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.MediumSlateBlue;

            return btn;
        }

        private void BtnCikis_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Oturumu kapatmak istediğine emin misin?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LoadSection(string sectionName)
        {
            panelMain.Controls.Clear();
            Label lbl = new Label
            {
                Text = $"📋 {sectionName} ekranı hazırlanıyor...",
                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                AutoSize = true,
                ForeColor = Color.DimGray,
                Location = new Point(50, 50)
            };
            panelMain.Controls.Add(lbl);
        }



        // =======================================================
        // 🏠 SALON YÖNETİMİ (FULL CRUD + FORM DOLDURMA + TEMİZLEME)
        // =======================================================
        private void LoadSalonYonetimi()
        {
            panelMain.Controls.Clear();

            Label lblTitle = new Label
            {
                Text = "🏠 Salon Yönetimi",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelMain.Controls.Add(lblTitle);

            // --- DataGridView ---
            DataGridView dgv = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(550, 250),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelMain.Controls.Add(dgv);

            // --- Form Elemanları ---
            Label lblAd = new Label { Text = "Salon Adı:", Location = new Point(600, 80), AutoSize = true };
            TextBox txtAd = new TextBox { Location = new Point(700, 75), Width = 200 };

            Label lblBas = new Label { Text = "Başlangıç:", Location = new Point(600, 120), AutoSize = true };
            DateTimePicker dtBas = new DateTimePicker
            {
                Location = new Point(700, 115),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            Label lblBit = new Label { Text = "Bitiş:", Location = new Point(600, 160), AutoSize = true };
            DateTimePicker dtBit = new DateTimePicker
            {
                Location = new Point(700, 155),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            Button btnEkle = new Button { Text = "Ekle", Location = new Point(600, 200) };
            Button btnSil = new Button { Text = "Sil", Location = new Point(680, 200) };
            Button btnGuncelle = new Button { Text = "Güncelle", Location = new Point(760, 200) };
            Button btnTemizle = new Button { Text = "Temizle", Location = new Point(860, 200) };

            panelMain.Controls.AddRange(new Control[]
            {
        lblAd, txtAd, lblBas, dtBas, lblBit, dtBit,
        btnEkle, btnSil, btnGuncelle, btnTemizle
            });

            // --- CRUD Fonksiyonları ---
            Action loadSalonlar = () =>
            {
                dgv.DataSource = db.Salonlar
                    .Select(s => new
                    {
                        s.Id,
                        s.Ad,
                        s.CalismaBaslangic,
                        s.CalismaBitis
                    })
                    .ToList();
            };
            loadSalonlar();

            // ✅ Ekle
            btnEkle.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAd.Text))
                {
                    MessageBox.Show("Salon adı boş olamaz!");
                    return;
                }

                var salon = new Salon
                {
                    Ad = txtAd.Text.Trim(),
                    CalismaBaslangic = dtBas.Value.TimeOfDay,
                    CalismaBitis = dtBit.Value.TimeOfDay
                };

                db.Salonlar.Add(salon);
                db.SaveChanges();
                loadSalonlar();
                MessageBox.Show("Salon başarıyla eklendi 💇‍♀️");
                ClearForm();
            };

            // ❌ Sil
            btnSil.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen silinecek salonu seçin!");
                    return;
                }

                int id = (int)dgv.CurrentRow.Cells["Id"].Value;
                var salon = db.Salonlar.Find(id);
                if (salon != null)
                {
                    db.Salonlar.Remove(salon);
                    db.SaveChanges();
                    loadSalonlar();
                    MessageBox.Show("Salon silindi ❌");
                    ClearForm();
                }
            };

            // 🔄 Güncelle
            btnGuncelle.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen güncellenecek salonu seçin!");
                    return;
                }

                int id = (int)dgv.CurrentRow.Cells["Id"].Value;
                var salon = db.Salonlar.Find(id);
                if (salon != null)
                {
                    salon.Ad = txtAd.Text.Trim();
                    salon.CalismaBaslangic = dtBas.Value.TimeOfDay;
                    salon.CalismaBitis = dtBit.Value.TimeOfDay;
                    db.SaveChanges();
                    loadSalonlar();
                    MessageBox.Show("Salon bilgileri güncellendi ✅");
                    ClearForm();
                }
            };

            // 🧹 Temizle
            btnTemizle.Click += (s, e) => ClearForm();

            // 📋 DataGridView Seçimden Form Doldurma
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtAd.Text = dgv.Rows[e.RowIndex].Cells["Ad"].Value?.ToString();

                    if (dgv.Rows[e.RowIndex].Cells["CalismaBaslangic"].Value is TimeSpan bas)
                        dtBas.Value = DateTime.Today.Add(bas);

                    if (dgv.Rows[e.RowIndex].Cells["CalismaBitis"].Value is TimeSpan bit)
                        dtBit.Value = DateTime.Today.Add(bit);
                }
            };

            // 🔧 Form Temizleme Fonksiyonu
            void ClearForm()
            {
                txtAd.Clear();
                dtBas.Value = DateTime.Now;
                dtBit.Value = DateTime.Now;
                dgv.ClearSelection();
            }
        }

        // --- ÇALIŞAN YÖNETİMİ ---
        private void LoadCalisanYonetimi()
        {
            panelMain.Controls.Clear();

            Label lbl = new Label
            {
                Text = "👩‍💼 Çalışan Yönetimi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(30, 20),
                AutoSize = true
            };
            panelMain.Controls.Add(lbl);

            // --- DataGridView ---
            DataGridView dgv = new DataGridView
            {
                Name = "dgvCalisanlar",
                Location = new Point(30, 70),
                Size = new Size(700, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelMain.Controls.Add(dgv);

            // --- Form Alanları ---
            Label lblSalon = new Label { Text = "Salon:", Location = new Point(760, 70), AutoSize = true };
            ComboBox cmbSalon = new ComboBox
            {
                Name = "cmbCalisanSalon",
                Location = new Point(760, 90),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSalon.DataSource = db.Salonlar.ToList();
            cmbSalon.DisplayMember = "Ad";
            cmbSalon.ValueMember = "Id";
            panelMain.Controls.Add(lblSalon);
            panelMain.Controls.Add(cmbSalon);

            Label lblAd = new Label { Text = "Ad:", Location = new Point(760, 130), AutoSize = true };
            TextBox txtAd = new TextBox { Location = new Point(760, 150), Width = 180 };
            panelMain.Controls.Add(lblAd);
            panelMain.Controls.Add(txtAd);

            Label lblSoyad = new Label { Text = "Soyad:", Location = new Point(760, 190), AutoSize = true };
            TextBox txtSoyad = new TextBox { Location = new Point(760, 210), Width = 180 };
            panelMain.Controls.Add(lblSoyad);
            panelMain.Controls.Add(txtSoyad);

            Label lblUzmanlik = new Label { Text = "Uzmanlık:", Location = new Point(760, 250), AutoSize = true };
            TextBox txtUzmanlik = new TextBox { Location = new Point(760, 270), Width = 180 };
            panelMain.Controls.Add(lblUzmanlik);
            panelMain.Controls.Add(txtUzmanlik);

            // --- Butonlar ---
            Button btnEkle = new Button { Text = "➕ Ekle", BackColor = Color.MediumSlateBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Location = new Point(740, 320), Size = new Size(80, 40) };
            Button btnSil = new Button { Text = "🗑️ Sil", BackColor = Color.IndianRed, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Location = new Point(830, 320), Size = new Size(80, 40) };
            Button btnGuncelle = new Button { Text = "✏️ Güncelle", BackColor = Color.Goldenrod, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Location = new Point(740, 370), Size = new Size(80, 40) };
            Button btnTemizle = new Button { Text = "🧹 Temizle", BackColor = Color.Gray, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Location = new Point(830, 370), Size = new Size(80, 40) };

            btnEkle.FlatAppearance.BorderSize = 0;
            btnSil.FlatAppearance.BorderSize = 0;
            btnGuncelle.FlatAppearance.BorderSize = 0;
            btnTemizle.FlatAppearance.BorderSize = 0;

            panelMain.Controls.AddRange(new Control[] { btnEkle, btnSil, btnGuncelle, btnTemizle });

            // --- Verileri Yükle ---
            void LoadCalisanlar()
            {
                dgv.DataSource = db.Calisanlar
                    .Select(c => new
                    {
                        c.Id,
                        c.Ad,
                        c.Soyad,
                        c.Uzmanlik,
                        Salon = c.Salon.Ad
                    })
                    .ToList();
            }
            LoadCalisanlar();

            // --- Ekle ---
            btnEkle.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
                {
                    MessageBox.Show("Lütfen ad ve soyad giriniz!");
                    return;
                }

                var calisan = new Calisan
                {
                    Ad = txtAd.Text.Trim(),
                    Soyad = txtSoyad.Text.Trim(),
                    Uzmanlik = txtUzmanlik.Text.Trim(),
                    SalonId = (int)cmbSalon.SelectedValue
                };
                db.Calisanlar.Add(calisan);
                db.SaveChanges();
                LoadCalisanlar();
                ClearForm();
                MessageBox.Show("Çalışan eklendi 💼");
            };

            // --- Sil ---
            btnSil.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Silinecek çalışanı seçiniz!");
                    return;
                }

                int id = (int)dgv.CurrentRow.Cells["Id"].Value;
                var calisan = db.Calisanlar.Find(id);
                if (calisan != null)
                {
                    db.Calisanlar.Remove(calisan);
                    db.SaveChanges();
                    LoadCalisanlar();
                    ClearForm();
                    MessageBox.Show("Çalışan silindi 🗑️");
                }
            };

            // --- Güncelle ---
            btnGuncelle.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Güncellenecek çalışanı seçiniz!");
                    return;
                }

                int id = (int)dgv.CurrentRow.Cells["Id"].Value;
                var calisan = db.Calisanlar.Find(id);
                if (calisan != null)
                {
                    calisan.Ad = txtAd.Text.Trim();
                    calisan.Soyad = txtSoyad.Text.Trim();
                    calisan.Uzmanlik = txtUzmanlik.Text.Trim();
                    calisan.SalonId = (int)cmbSalon.SelectedValue;
                    db.SaveChanges();
                    LoadCalisanlar();
                    ClearForm();
                    MessageBox.Show("Çalışan bilgileri güncellendi ✅");
                }
            };

            // --- Temizle ---
            btnTemizle.Click += (s, e) => ClearForm();

            // --- DataGridView'den Form Doldurma ---
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    txtAd.Text = dgv.Rows[e.RowIndex].Cells["Ad"].Value?.ToString();
                    txtSoyad.Text = dgv.Rows[e.RowIndex].Cells["Soyad"].Value?.ToString();
                    txtUzmanlik.Text = dgv.Rows[e.RowIndex].Cells["Uzmanlik"].Value?.ToString();
                    cmbSalon.SelectedIndex = cmbSalon.FindStringExact(dgv.Rows[e.RowIndex].Cells["Salon"].Value?.ToString());
                }
            };

            // --- Temizleme Fonksiyonu ---
            void ClearForm()
            {
                txtAd.Clear();
                txtSoyad.Clear();
                txtUzmanlik.Clear();
                if (cmbSalon.Items.Count > 0)
                    cmbSalon.SelectedIndex = 0;
                dgv.ClearSelection();
            }
        }



        // --- İŞLEM YÖNETİMİ ---
        private void LoadIslemYonetimi()
        {
            panelMain.Controls.Clear();

            // Başlık
            Label lbl = new Label
            {
                Text = "💇 İşlem Yönetimi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(30, 20),
                AutoSize = true
            };
            panelMain.Controls.Add(lbl);

            // DataGridView
            DataGridView dgv = new DataGridView
            {
                Name = "dgvIslemler",
                Location = new Point(30, 70),
                Size = new Size(700, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelMain.Controls.Add(dgv);

            // Salon Seçimi
            Label lblSalon = new Label
            {
                Text = "Salon Seç:",
                Location = new Point(760, 70),
                AutoSize = true
            };
            panelMain.Controls.Add(lblSalon);

            ComboBox cmbSalonSec = new ComboBox
            {
                Name = "cmbSalonSec",
                Location = new Point(760, 90),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSalonSec.DataSource = db.Salonlar.ToList();
            cmbSalonSec.DisplayMember = "Ad";
            cmbSalonSec.ValueMember = "Id";
            panelMain.Controls.Add(cmbSalonSec);

            // İşlem Adı
            Label lblAd = new Label { Text = "İşlem Adı:", Location = new Point(760, 130), AutoSize = true };
            TextBox txtIslemAd = new TextBox { Location = new Point(760, 150), Width = 180 };
            panelMain.Controls.Add(lblAd);
            panelMain.Controls.Add(txtIslemAd);

            // Süre (Dakika)
            Label lblSure = new Label { Text = "Süre (dk):", Location = new Point(760, 190), AutoSize = true };
            NumericUpDown nudSure = new NumericUpDown { Location = new Point(760, 210), Width = 180, Minimum = 5, Maximum = 300, Value = 30 };
            panelMain.Controls.Add(lblSure);
            panelMain.Controls.Add(nudSure);

            // Ücret
            Label lblUcret = new Label { Text = "Ücret (₺):", Location = new Point(760, 250), AutoSize = true };
            NumericUpDown nudUcret = new NumericUpDown { Location = new Point(760, 270), Width = 180, Minimum = 0, Maximum = 10000, DecimalPlaces = 2 };
            panelMain.Controls.Add(lblUcret);
            panelMain.Controls.Add(nudUcret);

            // Ekle Butonu
            Button btnEkle = new Button
            {
                Text = "➕ Ekle",
                BackColor = Color.MediumSlateBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(760, 320),
                Size = new Size(80, 40)
            };
            btnEkle.FlatAppearance.BorderSize = 0;
            panelMain.Controls.Add(btnEkle);

            // Sil Butonu
            Button btnSil = new Button
            {
                Text = "🗑️ Sil",
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(860, 320),
                Size = new Size(80, 40)
            };
            btnSil.FlatAppearance.BorderSize = 0;
            panelMain.Controls.Add(btnSil);

            // Verileri Yükle
            void LoadIslemler()
            {
                dgv.DataSource = db.Islemler
                    .Select(i => new
                    {
                        i.Id,
                        i.Ad,
                        Süre = i.SureDakika + " dk",
                        Ücret = i.Ucret + " ₺",
                        Salon = i.Salon.Ad
                    })
                    .OrderBy(i => i.Salon)
                    .ToList();
            }

            LoadIslemler();

            // --- Ekle Butonu Click ---
            btnEkle.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtIslemAd.Text))
                {
                    MessageBox.Show("İşlem adını giriniz!");
                    return;
                }

                if (cmbSalonSec.SelectedValue == null)
                {
                    MessageBox.Show("Bir salon seçiniz!");
                    return;
                }

                var yeniIslem = new Islem
                {
                    Ad = txtIslemAd.Text.Trim(),
                    SalonId = (int)cmbSalonSec.SelectedValue,
                    SureDakika = (int)nudSure.Value,
                    Ucret = nudUcret.Value
                };

                db.Islemler.Add(yeniIslem);
                db.SaveChanges();
                MessageBox.Show("İşlem eklendi 💇‍♀️");
                LoadIslemler();
            };

            // --- Sil Butonu Click ---
            btnSil.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null)
                {
                    MessageBox.Show("Silinecek işlemi seçiniz!");
                    return;
                }

                int id = (int)dgv.CurrentRow.Cells["Id"].Value;
                var islem = db.Islemler.Find(id);

                if (islem != null)
                {
                    db.Islemler.Remove(islem);
                    db.SaveChanges();
                    MessageBox.Show("İşlem silindi 🗑️");
                    LoadIslemler();
                }
            };
        }


        // =======================================================
        // 📅 RANDEVU YÖNETİMİ (FİLTRELİ + RENKLİ + İSTATİSTİK + SATIR BUTONLU)
        // =======================================================
        private void LoadRandevuYonetimi()
        {
            panelMain.Controls.Clear();

            // Başlık
            Label lbl = new Label
            {
                Text = "📅 Randevu Yönetimi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.MediumSlateBlue,
                Location = new Point(30, 20),
                AutoSize = true
            };
            panelMain.Controls.Add(lbl);

            // --- İstatistik Kutuları ---
            int baseX = 30;
            int baseY = 60;

            Label lblBekleyen = new Label
            {
                Text = "Bekleyen: 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.Goldenrod,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(130, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(baseX, baseY)
            };
            panelMain.Controls.Add(lblBekleyen);

            Label lblOnaylanan = new Label
            {
                Text = "Onaylanan: 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(130, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(baseX + 140, baseY)
            };
            panelMain.Controls.Add(lblOnaylanan);

            Label lblReddedilen = new Label
            {
                Text = "Reddedilen: 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(130, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(baseX + 280, baseY)
            };
            panelMain.Controls.Add(lblReddedilen);

            // --- ComboBox Filtresi ---
            Label lblFiltre = new Label
            {
                Text = "Durum Filtresi:",
                Location = new Point(450, baseY + 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelMain.Controls.Add(lblFiltre);

            ComboBox cmbDurum = new ComboBox
            {
                Location = new Point(570, baseY + 5),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDurum.Items.AddRange(new[] { "Bekleyen", "Onaylanan", "Reddedilen", "Tümü" });
            cmbDurum.SelectedIndex = 0;
            panelMain.Controls.Add(cmbDurum);

            // --- DataGridView ---
            DataGridView dgv = new DataGridView
            {
                Name = "dgvRandevular",
                Location = new Point(30, 120),
                Size = new Size(980, 350),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            panelMain.Controls.Add(dgv);

            // --- Buton sütunları ekle ---
            DataGridViewButtonColumn colOnay = new DataGridViewButtonColumn
            {
                Name = "BtnOnay",
                HeaderText = "Onayla",
                Text = "✅",
                UseColumnTextForButtonValue = true
            };
            DataGridViewButtonColumn colRed = new DataGridViewButtonColumn
            {
                Name = "BtnRed",
                HeaderText = "Reddet",
                Text = "❌",
                UseColumnTextForButtonValue = true
            };
            dgv.Columns.Add(colOnay);
            dgv.Columns.Add(colRed);

            // --- Veri Yükleme Fonksiyonu ---
            void LoadData(string durumFiltre = "Bekleyen")
            {
                var query = db.Randevular.AsQueryable();

                if (durumFiltre == "Bekleyen")
                    query = query.Where(r => r.OnayDurumu == 0);
                else if (durumFiltre == "Onaylanan")
                    query = query.Where(r => r.OnayDurumu == 1);
                else if (durumFiltre == "Reddedilen")
                    query = query.Where(r => r.OnayDurumu == 2);

                var data = query
                    .Select(r => new
                    {
                        r.Id,
                        Müşteri = r.Musteri.Ad + " " + r.Musteri.Soyad,
                        Çalışan = r.Calisan.Ad + " " + r.Calisan.Soyad,
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

                // 🎨 Renkleme
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string durum = row.Cells["Durum"].Value?.ToString();
                    if (durum == "Onaylandı")
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    else if (durum == "Reddedildi")
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                }

                // 🔢 İstatistikleri Güncelle
                lblBekleyen.Text = $"Bekleyen: {db.Randevular.Count(r => r.OnayDurumu == 0)}";
                lblOnaylanan.Text = $"Onaylanan: {db.Randevular.Count(r => r.OnayDurumu == 1)}";
                lblReddedilen.Text = $"Reddedilen: {db.Randevular.Count(r => r.OnayDurumu == 2)}";
            }

            // İlk açılışta sadece bekleyenler
            LoadData("Bekleyen");

            // ComboBox değişince tabloyu yenile
            cmbDurum.SelectedIndexChanged += (s, e) =>
            {
                string secilen = cmbDurum.SelectedItem.ToString();
                LoadData(secilen);
            };

            // 🖱️ Satır içi buton tıklama
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value;
                var r = db.Randevular.Find(id);
                if (r == null) return;

                if (dgv.Columns[e.ColumnIndex].Name == "BtnOnay")
                {
                    r.OnayDurumu = 1;
                    db.SaveChanges();
                    MessageBox.Show("Randevu onaylandı ✅");
                }
                else if (dgv.Columns[e.ColumnIndex].Name == "BtnRed")
                {
                    r.OnayDurumu = 2;
                    db.SaveChanges();
                    MessageBox.Show("Randevu reddedildi ❌");
                }

                LoadData(cmbDurum.SelectedItem.ToString());
            };
        }




    }
}
