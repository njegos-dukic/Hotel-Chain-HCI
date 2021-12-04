using LanacHotelaServiceLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LanacHotela
{
    public partial class LanacHotelaMain : Window
    {
        private readonly List<Hotel> selectedHotels = new List<Hotel>();

        private readonly List<AranzmanDetaljno> aranzmani = new List<AranzmanDetaljno>();
        private readonly List<HotelFinansije> finansije = new List<HotelFinansije>();
        private readonly List<Kontakt> kontakti = new List<Kontakt>();
        private readonly List<Hotel> hoteli = new List<Hotel>();
        private readonly List<Soba> sobe = new List<Soba>();
        private readonly List<Gost> gosti = new List<Gost>();
        private readonly List<Zaposleni> zaposleni = new List<Zaposleni>();

        public LanacHotelaMain(string language, bool isManager)
        {
            InitializeComponent();
            InitializeApp();
            UpdateLanguage();
            PopulateHotelSelector();
            UpdateDataGrid();

            if (!isManager)
            {
                finansijeTab.Visibility = Visibility.Hidden;
                kontaktiTab.Visibility = Visibility.Hidden;
                hoteliTab.Visibility = Visibility.Hidden;
                korisniciTab.Visibility = Visibility.Hidden;
            }

            languageComboBox.SelectedItem = language.ToUpper();
        }

        private void InitializeApp()
        {
            aranzmaniDataGrid.ItemsSource = aranzmani;
            hotelDataGrid.ItemsSource = hoteli;
            kontaktiDataGrid.ItemsSource = kontakti;
            finansijeDataGrid.ItemsSource = finansije;
            sobeDataGrid.ItemsSource = sobe;
            gostiDataGrid.ItemsSource = gosti;
            korisniciDataGrid.ItemsSource = zaposleni;

            hotelsComboBox.SelectionChanged += HotelSelectionChanged;

            noviKontaktTip.Items.Add("email");
            noviKontaktTip.Items.Add("telefon");

            noviHotelZvjezdice.Items.Add(1);
            noviHotelZvjezdice.Items.Add(2);
            noviHotelZvjezdice.Items.Add(3);
            noviHotelZvjezdice.Items.Add(4);
            noviHotelZvjezdice.Items.Add(5);

            languageComboBox.Items.Add("EN");
            languageComboBox.Items.Add("SR");
            languageComboBox.SelectionChanged += LanguageChanged;
        }

        private async void HotelChecked(object sender, RoutedEventArgs e)
        {
            dynamic checkboxData = e.OriginalSource;
            bool isChecked = checkboxData.IsChecked;
            int hotelID = int.Parse(checkboxData.CommandParameter);

            HotelService hotelService = new HotelService();
            Hotel checkedHotel = await hotelService.GetById(hotelID);

            if (isChecked)
            {
                selectedHotels.Add(checkedHotel);
            }
            else
            {
                selectedHotels.Remove(checkedHotel);
            }

            UpdateDataGrid();
        }

        private async void PopulateHotelSelector()
        {
            stackPanel.Children.Clear();
            HotelService hotelService = new HotelService();

            foreach (Hotel h in await hotelService.GetAll())
            {
                CheckBox checkBox = new CheckBox() { Content = h.Ime, CommandParameter = h.HotelID.ToString() };
                checkBox.Click += HotelChecked;
                stackPanel.Children.Add(checkBox);
            }
        }

        private async void UpdateDataGrid()
        {
            // Aranzmani i finansije
            aranzmani.Clear();
            finansije.Clear();

            AranzmanDetaljnoService aranzmanService = new AranzmanDetaljnoService();
            HotelFinansijeService finansijeService = new HotelFinansijeService();

            foreach (Hotel h in selectedHotels)
            {
                foreach (AranzmanDetaljno aranzman in await aranzmanService.GetAllForHotel(h.HotelID))
                {
                    if (aranzman != null)
                    {
                        aranzmani.Add(aranzman);
                    }
                }

                HotelFinansije finansija = await finansijeService.GetById(h.HotelID);
                if (finansija != null)
                {
                    finansije.Add(finansija);
                }
            }
            aranzmaniDataGrid.Items.Refresh();
            finansijeDataGrid.Items.Refresh();

            // Hoteli
            hoteli.Clear();
            hotelsComboBox.Items.Clear();
            novaSobaHotel.Items.Clear();
            HotelService hotelService = new HotelService();

            foreach (Hotel h in await hotelService.GetAll())
            {
                if (h != null)
                {
                    hoteli.Add(h);
                }

                ComboBoxItem cbHotelItem = new ComboBoxItem
                {
                    Tag = h.HotelID,
                    Content = h.Ime
                };
                hotelsComboBox.Items.Add(cbHotelItem);

                ComboBoxItem cbZaSobeItem = new ComboBoxItem
                {
                    Tag = h.HotelID,
                    Content = h.Ime
                };
                novaSobaHotel.Items.Add(cbZaSobeItem);
            }
            hotelDataGrid.Items.Refresh();
            hotelsComboBox.Items.Refresh();
            novaSobaHotel.Items.Refresh();

            // Sobe
            sobe.Clear();

            SobaService sobaService = new SobaService();
            {
                foreach (Soba soba in await sobaService.GetAll())
                {
                    if (soba != null)
                    {
                        sobe.Add(soba);
                    }
                }
            }
            sobeDataGrid.Items.Refresh();

            // Kontakti
            kontakti.Clear();
            noviHotelKontakt.Items.Clear();
            KontaktService kontaktService = new KontaktService();

            foreach (Kontakt k in await kontaktService.GetAll())
            {
                if (k != null)
                {
                    kontakti.Add(k);
                }

                if (k.HotelID == 0 && k.GostID == 0)
                {
                    noviHotelKontakt.Items.Add(k.KontaktID + ": " + k.Info);
                }
            }
            kontaktiDataGrid.Items.Refresh();

            // Gosti
            gosti.Clear();
            guestComboBox.Items.Clear();
            GostService gostService = new GostService();

            foreach (Gost g in await gostService.GetAll())
            {
                gosti.Add(g);
                ComboBoxItem cbGostItem = new ComboBoxItem
                {
                    Tag = g.GostID,
                    Content = g.Ime + " " + g.Prezime
                };
                guestComboBox.Items.Add(cbGostItem);
            }
            gostiDataGrid.Items.Refresh();

            // Zaposleni
            zaposleni.Clear();

            ZaposleniService zaposleniService = new ZaposleniService();
            {
                foreach (Zaposleni z in await zaposleniService.GetAll())
                {
                    if (z != null)
                    {
                        zaposleni.Add(z);
                    }
                }
            }
            korisniciDataGrid.Items.Refresh();
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            AranzmanDetaljno selectedAranzman = (AranzmanDetaljno)aranzmaniDataGrid.SelectedItem;
            AranzmanService aranzmanService = new AranzmanService();

            resultTextBlock.Text = "";

            if (selectedAranzman == null)
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.selectArrangment;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            if (await aranzmanService.Delete(selectedAranzman.AranzmanID) == -1)
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.arrangementDeleted;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }

            UpdateDataGrid();
        }

        private void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            string language = (string)languageComboBox.SelectedItem;
            App.SetCulture(language);
            UpdateLanguage();
        }

        private async void HotelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SobaService sobaService = new SobaService();
            ComboBoxItem item = (ComboBoxItem)hotelsComboBox.SelectedItem;
            if (item == null)
            {
                return;
            }

            int hotelID = (int)item.Tag;

            roomComboBox.Items.Clear();

            foreach (Soba s in await sobaService.GetAll())
            {
                if (s.HotelID == hotelID)
                {
                    ComboBoxItem cbSobaItem = new ComboBoxItem
                    {
                        Tag = s.SobaID,
                        Content = $"Soba {s.SobaID}"
                    };
                    roomComboBox.Items.Add(cbSobaItem);
                }
            }
        }

        private async void AddClick(object sender, RoutedEventArgs e)
        {
            if (noviAranzmanPocetak.SelectedDate == null
                || noviAranzmanKraj.SelectedDate == null
                || hotelsComboBox.SelectedItem == null
                || guestComboBox.SelectedItem == null
                || roomComboBox.SelectedItem == null)
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            resultTextBlock.Text = "";

            ComboBoxItem hotelItem = (ComboBoxItem)hotelsComboBox.SelectedItem;
            int hotelID = (int)hotelItem.Tag;

            ComboBoxItem gostItem = (ComboBoxItem)guestComboBox.SelectedItem;
            int gostID = (int)gostItem.Tag;

            ComboBoxItem sobaItem = (ComboBoxItem)roomComboBox.SelectedItem;
            int sobaID = (int)sobaItem.Tag;

            Aranzman aranzman = new Aranzman(0, noviAranzmanPocetak.SelectedDate.Value, noviAranzmanKraj.SelectedDate.Value, noviAranzmanJeOtkazan.IsChecked.Value, noviAranzmanJeZavrsen.IsChecked.Value, hotelID, gostID, sobaID);
            AranzmanService aranzmanService = new AranzmanService();

            if (await aranzmanService.Insert(aranzman) == -1)
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.arragementCreated;
                resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);
            }

            UpdateDataGrid();
        }

        private async void UpdateClick(object sender, RoutedEventArgs e)
        {
            AranzmanService aranzmanService = new AranzmanService();
            resultTextBlock.Text = "";
            int counter = 0;

            foreach (AranzmanDetaljno a in aranzmani)
            {
                if (await aranzmanService.Update(a) != -1)
                {
                    counter++;
                }
            }

            resultTextBlock.Text = LanacHotelaPresentation.Properties.Resources.updated;
            resultTextBlock.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void AddClickContact(object sender, RoutedEventArgs e)
        {
            if (noviKontaktTip.SelectedItem == null
                || noviKontaktInfo.Text.Length <= 5)
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatTextKontakt.Text = "";

            string kontaktTip = (string)noviKontaktTip.SelectedItem;
            string kontaktInfo = noviKontaktInfo.Text;

            Kontakt kontakt = new Kontakt(0, kontaktTip, kontaktInfo, 0, 0);
            KontaktService kontaktService = new KontaktService();

            if (await kontaktService.Insert(kontakt) == -1)
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.contactCreated;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);
                UpdateDataGrid();
            }
        }

        private async void UpdateClickContact(object sender, RoutedEventArgs e)
        {
            KontaktService kontaktService = new KontaktService();
            rezultatTextKontakt.Text = "";
            int counter = 0;

            foreach (Kontakt k in kontakti)
            {
                if (k.GostID != 0 && k.HotelID != 0)
                {
                    continue;
                }

                if (await kontaktService.Update(k) != -1)
                {
                    counter++;
                }
            }

            rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.updated;
            rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickContact(object sender, RoutedEventArgs e)
        {
            Kontakt selectedKontakt = (Kontakt)kontaktiDataGrid.SelectedItem;
            KontaktService kontaktService = new KontaktService();

            rezultatTextKontakt.Text = "";

            if (selectedKontakt == null)
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.selectContact;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await kontaktService.GetAllForHotel(selectedKontakt.HotelID)).Count == 1 || (await kontaktService.GetAllForGuest(selectedKontakt.GostID)).Count == 1)
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.onlyContact;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await kontaktService.Delete(selectedKontakt.KontaktID)) == -1)
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatTextKontakt.Text = LanacHotelaPresentation.Properties.Resources.contactDeleted;
                rezultatTextKontakt.Foreground = new SolidColorBrush(Colors.Green);
                UpdateDataGrid();
            }
        }

        private async void AddClickHotel(object sender, RoutedEventArgs e)
        {
            if (noviHotelIme.Text.Length <= 1
                || noviHotelZvjezdice.SelectedItem == null
                || noviHotelKontakt.SelectedItem == null
                || noviHotelUlica.Text.Length <= 1
                || noviHotelBroj.Text.Length <= 1
                || noviHotelGrad.Text.Length <= 1
                || noviHotelZIP.Text.Length <= 1
                || noviHotelDrzava.Text.Length <= 1)
            {
                rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                rezultatHotel.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatHotel.Text = "";

            try
            {
                string hotelIme = noviHotelIme.Text;
                int hotelZvjezdice = (int)noviHotelZvjezdice.SelectedItem;
                KontaktService ks = new KontaktService();
                Kontakt hotelKontakt = await ks.GetById(int.Parse(((string)noviHotelKontakt.SelectedItem).Split(':')[0]));
                string hotelUlica = noviHotelUlica.Text;
                string hotelBroj = noviHotelBroj.Text;
                string hotelGrad = noviHotelGrad.Text;
                int hotelZIP = int.Parse(noviHotelZIP.Text);
                string hotelDrzava = noviHotelDrzava.Text;

                Hotel hotel = new Hotel(0, hotelIme, hotelZvjezdice, hotelUlica, hotelBroj, hotelGrad, hotelZIP, hotelDrzava);
                HotelService hotelService = new HotelService();

                if (await hotelService.Insert(hotel) == -1)
                {
                    rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                    rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    foreach (Hotel h in await hotelService.GetAll())
                    {
                        if (h.Equals(hotel))
                        {
                            hotel.HotelID = h.HotelID;
                        }
                    }

                    KontaktService kontaktService = new KontaktService();
                    await kontaktService.Update(hotelKontakt, hotel.HotelID);

                    rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.hotelCreated;
                    rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);

                    CheckBox checkBox = new CheckBox() { Content = hotel.Ime, CommandParameter = hotel.HotelID.ToString() };
                    checkBox.Click += HotelChecked;
                    stackPanel.Children.Add(checkBox);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickHotel(object sender, RoutedEventArgs e)
        {
            HotelService hotelService = new HotelService();
            rezultatHotel.Text = "";
            int counter = 0;

            foreach (Hotel h in hoteli)
            {
                if (await hotelService.Update(h) != -1)
                {
                    counter++;
                }
            }

            rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.updated;
            rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);

            if (counter > 0)
            {
                selectedHotels.Clear();
                PopulateHotelSelector();
            }
            UpdateDataGrid();
        }

        private async void DeleteClickHotel(object sender, RoutedEventArgs e)
        {
            Hotel selectedHotel = (Hotel)hotelDataGrid.SelectedItem;
            HotelService hotelService = new HotelService();
            AranzmanService aranzmanService = new AranzmanService();

            rezultatHotel.Text = "";

            if (selectedHotel == null)
            {
                rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.selectHotel;
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await aranzmanService.GetAllForHotel(selectedHotel.HotelID)).Count > 0)
            {
                rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.hotelHasArrang;
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await hotelService.Delete(selectedHotel.HotelID)) == -1)
            {
                rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatHotel.Text = LanacHotelaPresentation.Properties.Resources.hotelDeleted;
                rezultatHotel.Foreground = new SolidColorBrush(Colors.Green);
                selectedHotels.Clear();
                PopulateHotelSelector();
                UpdateDataGrid();
            }
        }

        private async void AddClickSobe(object sender, RoutedEventArgs e)
        {
            if (novaSobaBrojSprata.Text.Length < 1
                || novaSobaBrojSobe.Text.Length < 1
                || novaSobaBrojKreveta.Text.Length < 1
                || NovaSobaImaTV.IsChecked.HasValue == false
                || novaSobaImaKlimu.IsChecked.HasValue == false
                || novaSobaCijenaNocenja.Text.Length < 1
                || novaSobaHotel.SelectedItem == null)
            {
                rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                rezultatSobe.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatSobe.Text = "";

            try
            {
                HotelService hotelService = new HotelService();
                ComboBoxItem hotelItem = (ComboBoxItem)novaSobaHotel.SelectedItem;
                int hotelID = (int)hotelItem.Tag;
                Hotel hotel = await hotelService.GetById(hotelID);

                int brojSprata = int.Parse(novaSobaBrojSprata.Text);
                int brojSobe = int.Parse(novaSobaBrojSobe.Text);
                int brojKreveta = int.Parse(novaSobaBrojKreveta.Text);
                bool imaTV = NovaSobaImaTV.IsChecked.Value;
                bool imaKlimu = novaSobaImaKlimu.IsChecked.Value;
                double cijenaNocenja = double.Parse(novaSobaCijenaNocenja.Text);

                Soba soba = new Soba(0, brojSprata, brojSobe, brojKreveta, imaTV, imaKlimu, cijenaNocenja, hotel.HotelID);
                SobaService sobaService = new SobaService();

                if (await sobaService.Insert(soba) == -1)
                {
                    rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                    rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.roomCreated;
                    rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickSobe(object sender, RoutedEventArgs e)
        {
            SobaService sobaService = new SobaService();
            rezultatSobe.Text = "";

            int counter = 0;

            foreach (Soba s in sobe)
            {
                if (await sobaService.Update(s) != -1)
                {
                    counter++;
                }
            }

            rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.updated;
            rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickSobe(object sender, RoutedEventArgs e)
        {
            Soba selectedSoba = (Soba)sobeDataGrid.SelectedItem;
            SobaService sobaService = new SobaService();

            rezultatSobe.Text = "";

            if (selectedSoba == null)
            {
                rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.selectRoom;
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await sobaService.GetAllReservations(selectedSoba.SobaID)).Count > 0)
            {
                rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.roomHasArr;
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await sobaService.Delete(selectedSoba.SobaID)) == -1)
            {
                rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatSobe.Text = LanacHotelaPresentation.Properties.Resources.roomDeleted;
                rezultatSobe.Foreground = new SolidColorBrush(Colors.Green);

                UpdateDataGrid();
            }
        }

        private async void AddClickGost(object sender, RoutedEventArgs e)
        {
            if (noviGostJMBG.Text.Length != 13
                || noviGostIme.Text.Length < 2
                || noviGostPrezime.Text.Length < 2)
            {
                rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                rezultatGost.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatGost.Text = "";

            try
            {
                string JMBG = noviGostJMBG.Text;
                string ime = noviGostIme.Text;
                string prezime = noviGostPrezime.Text;

                Gost gost = new Gost(0, JMBG, ime, prezime);
                GostService gostService = new GostService();

                if (await gostService.Insert(gost) == -1)
                {
                    rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                    rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.guestCreated;
                    rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickGost(object sender, RoutedEventArgs e)
        {
            GostService gostService = new GostService();
            rezultatGost.Text = "";

            int counter = 0;

            foreach (Gost g in gosti)
            {
                if (await gostService.Update(g) != -1)
                {
                    counter++;
                }
            }

            rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.updated;
            rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickGost(object sender, RoutedEventArgs e)
        {
            Gost selectedGost = (Gost)gostiDataGrid.SelectedItem;
            GostService gostService = new GostService();

            rezultatGost.Text = "";

            if (selectedGost == null)
            {
                rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.selectGuest;
                rezultatGost.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await gostService.GetAllReservations(selectedGost.GostID)).Count > 0)
            {
                rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.guestHasArr;
                rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if ((await gostService.Delete(selectedGost.GostID)) == -1)
            {
                rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                rezultatGost.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatGost.Text = LanacHotelaPresentation.Properties.Resources.guestDeleted;
                rezultatGost.Foreground = new SolidColorBrush(Colors.Green);

                UpdateDataGrid();
            }
        }

        private async void AddClickZaposleni(object sender, RoutedEventArgs e)
        {
            if (korisnikNoviIme.Text.Length < 4
                || korisnikNoviPassword.Password.Length < 4)
            {
                rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.fillAll;
                rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.DarkOrange);
                return;
            }

            rezultatTextZaposleni.Text = "";

            try
            {
                string username = korisnikNoviIme.Text;
                string password = korisnikNoviPassword.Password;
                bool isManager = korisnikNoviManager.IsChecked.Value;

                Zaposleni zaposleni = new Zaposleni(0, username, password, isManager, 1);
                ZaposleniService zaposleniService = new ZaposleniService();

                if (await zaposleniService.Insert(zaposleni) == -1)
                {
                    rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.failedCreate;
                    rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Red);
                }

                else
                {
                    rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.zaposleniCreated;
                    rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Green);

                    UpdateDataGrid();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void UpdateClickZaposleni(object sender, RoutedEventArgs e)
        {
            ZaposleniService zaposleniService = new ZaposleniService();
            rezultatTextZaposleni.Text = "";

            int counter = 0;

            foreach (Zaposleni z in zaposleni)
            {
                z.UpdatePassword();
                if (await zaposleniService.Update(z) != -1)
                {
                    counter++;
                }
            }

            rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.updated;
            rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Green);

            UpdateDataGrid();
        }

        private async void DeleteClickZaposleni(object sender, RoutedEventArgs e)
        {
            Zaposleni selectedZaposleni = (Zaposleni)korisniciDataGrid.SelectedItem;
            ZaposleniService zaposleniService = new ZaposleniService();

            rezultatTextZaposleni.Text = "";

            if (selectedZaposleni == null)
            {
                rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.selectZaposleni;
                rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }

            else if ((await zaposleniService.Delete(selectedZaposleni.ZaposleniID)) == -1)
            {
                rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.failedDelete;
                rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Red);
            }

            else
            {
                rezultatTextZaposleni.Text = LanacHotelaPresentation.Properties.Resources.zaposleniDeleted;
                rezultatTextZaposleni.Foreground = new SolidColorBrush(Colors.Green);

                UpdateDataGrid();
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            LanacHotelaPresentation.Login login = new LanacHotelaPresentation.Login((string)languageComboBox.SelectedItem);
            login.Show();
            Close();
        }

        private void UpdateLanguage()
        {
            MainWindowXAML.Title = LanacHotelaPresentation.Properties.Resources.appTitle;

            aranzmaniTab.Header = LanacHotelaPresentation.Properties.Resources.tabArrangements;
            arrangementHotel.Header = LanacHotelaPresentation.Properties.Resources.arrangementHotel;
            arrangementName.Header = LanacHotelaPresentation.Properties.Resources.arrangementName;
            arrangementSurname.Header = LanacHotelaPresentation.Properties.Resources.arrangementSurname;
            arrangementFrom.Header = LanacHotelaPresentation.Properties.Resources.arrangementFrom;
            arrangementTo.Header = LanacHotelaPresentation.Properties.Resources.arrangementTo;
            arrangementCancelled.Header = LanacHotelaPresentation.Properties.Resources.arrangementCancelled;
            arrangementFinished.Header = LanacHotelaPresentation.Properties.Resources.arrangementFinished;
            arrangementRoomID.Header = LanacHotelaPresentation.Properties.Resources.arrangementRoomID;
            arrangementPrice.Header = LanacHotelaPresentation.Properties.Resources.arrangementPrice;
            arrangementNights.Header = LanacHotelaPresentation.Properties.Resources.arrangementNights;

            addButton.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            deleteButton.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            updateButton.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            arrangementStartInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementStartInput;
            arrangementEndInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementEndInput;
            arrangementCancelledInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementCancelledInput;
            arrangementFinishedInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementFinishedInput;
            arrangementHotelInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementHotelInput;
            arrangementGuestInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementGuestInput;
            arrangementHotelInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementHotelInput;
            arrangementGuestInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementGuestInput;
            arrangementRoomInput.Text = LanacHotelaPresentation.Properties.Resources.arrangementRoomInput;

            hoteliTab.Header = LanacHotelaPresentation.Properties.Resources.tabHotels;
            hotelsHotel.Header = LanacHotelaPresentation.Properties.Resources.hotelsHotel;
            hotelsStars.Header = LanacHotelaPresentation.Properties.Resources.hotelsStars;
            hotelsStreet.Header = LanacHotelaPresentation.Properties.Resources.hotelsStreet;
            hotelsNumber.Header = LanacHotelaPresentation.Properties.Resources.hotelsNumber;
            hotelsCity.Header = LanacHotelaPresentation.Properties.Resources.hotelsCity;
            hotelsZIP.Header = LanacHotelaPresentation.Properties.Resources.hotelsZIP;
            hotelsCountry.Header = LanacHotelaPresentation.Properties.Resources.hotelsCountry;

            hotelsNameInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsNameInput;
            hotelsStarsInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsStarsInput;
            hotelsContactInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsContactInput;
            hotelsStreetInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsStreetInput;
            hotelsNumberInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsNumberInput;
            hotelsCityInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsCityInput;
            hotelsZIPInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsZIPInput;
            hotelsCountryInput.Text = LanacHotelaPresentation.Properties.Resources.hotelsCountryInput;

            addButtonHotel.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            deleteButtonHotel.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            updateButtonHotel.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            kontaktiTab.Header = LanacHotelaPresentation.Properties.Resources.tabContact;

            contactContactID.Header = LanacHotelaPresentation.Properties.Resources.contactContactID;
            contactType.Header = LanacHotelaPresentation.Properties.Resources.contactType;
            contactInfo.Header = LanacHotelaPresentation.Properties.Resources.contactInfo;
            contactHotelID.Header = LanacHotelaPresentation.Properties.Resources.contactHotelID;
            contactGuestID.Header = LanacHotelaPresentation.Properties.Resources.contactGuestID;

            contactTypeInput.Text = LanacHotelaPresentation.Properties.Resources.contactTypeInput;
            contactInfoInput.Text = LanacHotelaPresentation.Properties.Resources.contactInfoInput;

            dodajKontaktButton.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            obrisiKontaktButton.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            azurirajKontaktButton.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            sobeTab.Header = LanacHotelaPresentation.Properties.Resources.tabRooms;
            roomsRoomID.Header = LanacHotelaPresentation.Properties.Resources.roomsRoomID;
            roomsHotel.Header = LanacHotelaPresentation.Properties.Resources.roomsHotel;
            roomsFloor.Header = LanacHotelaPresentation.Properties.Resources.roomsFloor;
            roomsRoomNumber.Header = LanacHotelaPresentation.Properties.Resources.roomsRoomNumber;
            roomsNumberOfBeds.Header = LanacHotelaPresentation.Properties.Resources.roomsNumberOfBeds;
            roomsHasTV.Header = LanacHotelaPresentation.Properties.Resources.roomsHasTV;
            roomsHasAC.Header = LanacHotelaPresentation.Properties.Resources.roomsHasAC;
            roomsPrice.Header = LanacHotelaPresentation.Properties.Resources.roomsPrice;

            roomsHotelInput.Text = LanacHotelaPresentation.Properties.Resources.roomsHotelInput;
            roomsFloorInput.Text = LanacHotelaPresentation.Properties.Resources.roomsFloorInput;
            roomsRoomNumberInput.Text = LanacHotelaPresentation.Properties.Resources.roomsRoomNumberInput;
            roomsNumberOfBedsInput.Text = LanacHotelaPresentation.Properties.Resources.roomsNumberOfBedsInput;
            roomsHasTVInput.Text = LanacHotelaPresentation.Properties.Resources.roomsHasTVInput;
            roomsHasACInput.Text = LanacHotelaPresentation.Properties.Resources.roomsHasACInput;
            roomsPriceInput.Text = LanacHotelaPresentation.Properties.Resources.roomsPriceInput;

            addButtonSobe.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            deleteButtonSobe.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            updateButtonSobe.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            gostiTab.Header = LanacHotelaPresentation.Properties.Resources.guestsTab;

            guestsGuestID.Header = LanacHotelaPresentation.Properties.Resources.guestsGuestID;
            guestsJMBG.Header = LanacHotelaPresentation.Properties.Resources.guestsJMBG;
            guestsName.Header = LanacHotelaPresentation.Properties.Resources.guestsName;
            guestsSurname.Header = LanacHotelaPresentation.Properties.Resources.guestsSurname;

            guestsJMBGInput.Text = LanacHotelaPresentation.Properties.Resources.guestsJMBGInput;
            guestsNameInput.Text = LanacHotelaPresentation.Properties.Resources.guestsNameInput;
            guestsSurnameInput.Text = LanacHotelaPresentation.Properties.Resources.guestsSurnameInput;

            addButtonGost.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            deleteButtonGost.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            updateButtonSobeGost.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            finansijeTab.Header = LanacHotelaPresentation.Properties.Resources.financialTab;

            financialHotel.Header = LanacHotelaPresentation.Properties.Resources.financialHotel;
            financialTotalNights.Header = LanacHotelaPresentation.Properties.Resources.financialTotalNights;
            financialTotalIncome.Header = LanacHotelaPresentation.Properties.Resources.financialTotalIncome;

            korisniciTab.Header = LanacHotelaPresentation.Properties.Resources.zaposleniTab;

            korisnikIme.Header = LanacHotelaPresentation.Properties.Resources.zaposleniUsername;
            korisnikLozinka.Header = LanacHotelaPresentation.Properties.Resources.zaposleniPassword;
            korisnikMenadzer.Header = LanacHotelaPresentation.Properties.Resources.zaposleniJeMenadzer;

            korisnikKorisnickoImeInput.Text = LanacHotelaPresentation.Properties.Resources.zaposleniUsernameInput;
            korisnikPasswordInput.Text = LanacHotelaPresentation.Properties.Resources.zaposleniPasswordInput;
            korisnikManagerInput.Text = LanacHotelaPresentation.Properties.Resources.zaposleniIsManagerInput;

            dodajKorisnikaButton.Content = LanacHotelaPresentation.Properties.Resources.buttonAdd;
            obrisiKorisnikaButton.Content = LanacHotelaPresentation.Properties.Resources.buttonDelete;
            azurirajKorisnikaButton.Content = LanacHotelaPresentation.Properties.Resources.buttonUpdate;

            logOutButton.Content = LanacHotelaPresentation.Properties.Resources.logOutButtonText;

            resultTextBlock.Text = "";
            rezultatTextKontakt.Text = "";
            rezultatHotel.Text = "";
            rezultatSobe.Text = "";
            rezultatGost.Text = "";
            rezultatTextZaposleni.Text = "";
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
