using System.Globalization;
using System.Windows;

namespace LanacHotela
{
    public partial class App : Application
    {
        public static void SetCulture(string culture)
        {
            if (culture == "EN")
            {
                LanacHotelaPresentation.Properties.Resources.Culture = new CultureInfo("en");
            }
            else if (culture == "SR")
            {
                LanacHotelaPresentation.Properties.Resources.Culture = new CultureInfo("sr");
            }
        }
    }
}
