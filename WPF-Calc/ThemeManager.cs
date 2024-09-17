using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace WPF_Calc
{

    internal static class ThemeManager
    {
        public static void ApplyTheme(Window window)
        {
            ResourceDictionary themeDictionary;

            if (Properties.Settings.Default.Theme)
            {
                // Ładuj motyw ciemny
                themeDictionary = new ResourceDictionary();
                themeDictionary.Source = new Uri("Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute);
            }
            else
            {
                // Ładuj motyw jasny
                themeDictionary = new ResourceDictionary();
                themeDictionary.Source = new Uri("Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute);
            }

            window.Resources.MergedDictionaries.Clear();
            window.Resources.MergedDictionaries.Add(themeDictionary);
        }



        public static void UpdateThemeCheckBox(Window window, CheckBox checkBox)
        {
            // Ustaw stan checkboxa zgodnie z wczytanymi ustawieniami
            checkBox.IsChecked = Properties.Settings.Default.Theme;
        }

        public static void ApplyThemeToAllWindows()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.IsLoaded)
                {
                    ApplyTheme(window);
                }
            }
        }
    }

}