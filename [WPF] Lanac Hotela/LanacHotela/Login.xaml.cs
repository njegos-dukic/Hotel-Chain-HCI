using LanacHotelaServiceLayer;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LanacHotelaPresentation
{
    public partial class Login : Window
    {
        private bool processing = false;

        public Login()
        {
            InitializeComponent();
            languageSelector.Items.Add("EN");
            languageSelector.Items.Add("SR");
            languageSelector.SelectedItem = "EN";

            languageSelector.SelectionChanged += LanguageChanged;
        }

        public Login(string language)
        {
            InitializeComponent();
            languageSelector.Items.Add("EN");
            languageSelector.Items.Add("SR");
            languageSelector.SelectedItem = language.ToUpper();

            languageSelector.SelectionChanged += LanguageChanged;
        }

        private async void LogIn(object sender, RoutedEventArgs e)
        {
            await LoggingIn();
        }

        private async Task LoggingIn()
        {
            try
            {
                if (processing)
                {
                    return;
                }

                processing = true;
                ZaposleniService zaposleniService = new ZaposleniService();
                foreach (Zaposleni zaposleni in await zaposleniService.GetAll())
                {
                    if (zaposleni.KorisnickoIme == enteredUsername.Text && zaposleni.Lozinka == enteredPassword.Password)
                    {
                        LanacHotela.LanacHotelaMain lanacHotelaMain = new LanacHotela.LanacHotelaMain((string)languageSelector.SelectedItem, zaposleni.JeMenadzer);
                        lanacHotelaMain.Show();
                        Close();
                        return;
                    }
                }
            }

            finally
            {
                processing = false;
                loginBorder.BorderBrush = Brushes.Red;
                await Task.Delay(3000);
                loginBorder.BorderBrush = Brushes.Gray;
            }
        }

        private void LanguageChanged(object sender, SelectionChangedEventArgs e)
        {
            string language = (string)languageSelector.SelectedItem;
            LanacHotela.App.SetCulture(language);
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            loginUserText.Text = LanacHotelaPresentation.Properties.Resources.loginUserText;
            loginPassText.Text = LanacHotelaPresentation.Properties.Resources.loginPassText;
            loginLangText.Text = LanacHotelaPresentation.Properties.Resources.loginLangText;

            loginButtonText.Content = LanacHotelaPresentation.Properties.Resources.loginButtonText;
        }

        private async void InputKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await LoggingIn();
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
