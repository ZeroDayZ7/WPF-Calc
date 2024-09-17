using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Calc
{
    /// <summary>
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private MainWindow _mainWindow;
        private CheckBox _chkDarkMode;

        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            _mainWindow = mainWindow;
            _chkDarkMode = FindName("chkDarkMode") as CheckBox;
            ThemeManager.UpdateThemeCheckBox(this, _chkDarkMode);

            this.Owner = App.Current.MainWindow;
        }



        private void chkDarkMode_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = true;
            Properties.Settings.Default.Save();
            ThemeManager.ApplyThemeToAllWindows();
        }

        private void chkDarkMode_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Theme = false;
            Properties.Settings.Default.Save();
            ThemeManager.ApplyThemeToAllWindows();
        }

     



        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }



    }
}