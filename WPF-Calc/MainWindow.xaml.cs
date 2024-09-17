using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using WPF_Calc;
using System.Windows.Markup;
namespace WPF_Calc
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            //=========================================================
            string filePath = "wyniki.txt"; // Ścieżka do pliku tekstowego
            if (!File.Exists(filePath))
            {
                // Utwórz nowy pusty plik
                File.WriteAllText(filePath, "");
            }
            string[] allLines = File.ReadAllLines(filePath);
            Array.Reverse(allLines);
            string[] lastFiveLines = allLines.Take(6).ToArray();
            string lastFiveLinesString = string.Join(Environment.NewLine, lastFiveLines);
            txtRead.Text = lastFiveLinesString;
            //=========================================================
            
        }
        public class Calculator
        {
            public static string Calculate(string expression)
            {
                try
                {
                    // Sprawdź, czy wyrażenie zawiera operacje specjalne
                    if (expression.Contains("%"))
                    {
                        expression = HandlePercentage(expression);
                    }
                    else if (expression.Contains("^"))
                    {
                        expression = HandlePower(expression);
                        return expression;
                    }
                    // Wykonaj obliczenia
                    DataTable dt = new DataTable();
                    var result = dt.Compute(expression, "");
                    // Zwróć wynik jako string
                    return result.ToString();
                }
                catch (Exception ex)
                {
                    // Obsłuż błędy, np. błąd dzielenia przez zero
                    return "Błąd: " + ex.Message;
                }
            }
            private static string HandlePercentage(string expression)
            {
                if (expression.Contains("+"))
                {
                    string[] parts = expression.Split('+');
                    // Sprawdź, czy druga część zawiera znak procentu
                    if (parts.Length > 1 && parts[1].Contains("%"))
                    {
                        double numberPart = double.Parse(parts[0].Trim());
                        // Usuń znak procentu i zamień na odpowiednie działanie matematyczne
                        string secondPart = parts[1].Replace("%", "");
                        // Zamień na odpowiednie działanie matematyczne
                        double percentValue = double.Parse(secondPart) / 100;
                        // Oblicz wynik dodawania z uwzględnieniem procentu
                        double result = numberPart + (numberPart * percentValue);
                        return result.ToString();
                    }
                }
                else if (expression.Contains("-"))
                {
                    string[] parts = expression.Split('-');
                    // Sprawdź, czy druga część zawiera znak procentu
                    if (parts.Length > 1 && parts[1].Contains("%"))
                    {
                        double numberPart = double.Parse(parts[0].Trim());
                        // Usuń znak procentu i zamień na odpowiednie działanie matematyczne
                        double percentValue = double.Parse(parts[1].Replace("%", "")) / 100.0;
                        // Oblicz wynik odejmowania z uwzględnieniem procentu
                        double result = numberPart - (numberPart * percentValue);
                        // Zwróć wynik jako string
                        return result.ToString();
                    }
                }
                else if (expression.Contains("*"))
                {
                    // Change % -> /100
                    expression = expression.Replace("%", "/100");
                    return expression.ToString();
                }
                else if (expression.Contains("/"))
                {
                    string[] parts = expression.Split('/');
                    // Sprawdź, czy druga część zawiera znak procentu
                    if (parts.Length > 1 && parts[1].Contains("%"))
                    {
                        double dividend = double.Parse(parts[0].Trim());
                        // Usuń znak procentu i zamień na odpowiednie działanie matematyczne
                        double divisor = double.Parse(parts[1].Replace("%", "")) / 100.0;
                        // Sprawdź, czy dzielnik nie jest zerem
                        if (divisor != 0)
                        {
                            // Oblicz wynik dzielenia z uwzględnieniem procentu
                            double result = dividend / divisor;
                            // Zwróć wynik jako string
                            return result.ToString();
                        }
                        else
                        {
                            // Dzielenie przez zero - obsłuż błąd
                            throw new DivideByZeroException("Dzielenie przez zero");
                        }
                    }
                }
                return expression.ToString();
            }
            private static string HandlePower(string expression)
            {
                // Dzielimy wyrażenie na podstawę i wykładnik
                string[] parts = expression.Split('^');
                // Konwertujemy obie części na liczby
                double baseNumber = double.Parse(parts[0].Trim());
                double exponent = double.Parse(parts[1].Trim());
                // Obliczamy potęgę za pomocą funkcji Math.Pow
                double elo = Math.Pow(baseNumber, exponent);
                // Zwracamy wynik jako string i przerywamy dalsze obliczenia
                return elo.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            // Pobierz zawartość klikniętego przycisku
            string clickedContent = clickedButton.Content.ToString();
            // Dodaj odpowiednią operację w zależności od klikniętego przycisku
            switch (clickedContent)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                    txtResult.Text += "" + clickedContent + "";
                    break;
                case ",":
                    // Pobierz tekst z kontrolki TextBox
                    string text = txtResult.Text;
                    // Sprawdź długość tekstu
                    int length = text.Length;
                    // Sprawdź, czy tekst nie jest pusty i czy ostatni znak to przecinek
                    if (length > 0 && text[length - 1] != ',')
                    {
                        // Dodaj przecinek tylko, jeśli ostatni znak nie jest przecinkiem
                        txtResult.Text += clickedContent;
                    }
                    // Jeśli ostatni znak to przecinek, nic nie rób
                    break;
                case "(":
                    txtResult.Text += clickedContent;
                    break;
                case ")":
                    txtResult.Text += clickedContent;
                    break;
                case "X^":
                    txtResult.Text += "^";
                    break;
                case "=":
                    // Pobierz tekst z kontrolki TextBox
                    string expression = txtResult.Text;
                    expression = expression.Replace(',', '.');
                    // Oblicz wynik wyrażenia matematycznego
                    string result = Calculator.Calculate(expression);
                    string filePath = "wyniki.txt"; // Ścieżka do pliku tekstowego
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine(expression + "=" + result); // Zapisz wynik w nowej linii
                    }
                    string[] allLines = File.ReadAllLines(filePath);
                    Array.Reverse(allLines);
                    string[] lastFiveLines = allLines.Take(5).ToArray();
                    string lastFiveLinesString = string.Join(Environment.NewLine, lastFiveLines);
                    txtRead.Text = lastFiveLinesString;
                    // Sprawdź, czy wynik kończy się na ",0" i jeśli tak, usuń ten fragment
                    if (result.EndsWith(",0"))
                    {
                        result = result.Substring(0, result.Length - 2); // Usuń ostatnie dwa znaki
                    }
                    // Dodaj wynik do pliku tekstowego
                    // Wyświetl wynik w kontrolce TextBox
                    txtResult.Text = result;
                    break;
                case "0":
                    if (txtResult.Text == "0")
                    {
                        txtResult.Text = "0,";
                    }
                    else
                    {
                        txtResult.Text += clickedContent;
                    }
                    break;
                default:
                    if (clickedContent == "0" && txtResult.Text == "0")
                    {
                        txtResult.Text = "0";
                    }
                    // Warunek sprawdzający, czy pole txtResult.Text jest równe "0"
                    if (txtResult.Text == "0" && (clickedContent != "0" && clickedContent != ","))
                    {
                        txtResult.Text = ""; // Zresetuj pole txtResult.Text
                    }
                    // Warunek sprawdzający, czy pole txtResult.Text nie jest równe "0" i czy została kliknięta cyfra od 1 do 9
                    if (txtResult.Text != "0" && (clickedContent != "0" && clickedContent != ","))
                    {
                        txtResult.Text += clickedContent; // Dodaj klikniętą cyfrę do pola txtResult.Text
                    }
                    break;
            }
        }


        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            // Wyczyść zawartość pola tekstowego
            txtResult.Text = "0";
            //txtResult.Text = "100-20%";
        }
        private void Button_Clear_CE(object sender, RoutedEventArgs e)
        {
            // Pobierz aktualny tekst z pola tekstowego
            string currentText = txtResult.Text;
            // Sprawdź długość aktualnego tekstu
            int length = currentText.Length;
            // Usuń ostatni znak z tekstu w polu tekstowym
            if (length > 0)
            {
                txtResult.Text = currentText.Substring(0, length - 1);
            }
            // Sprawdź, czy po usunięciu ostatniego znaku pole tekstowe jest puste
            if (txtResult.Text.Length == 0)
            {
                txtResult.Text = "0";
            }
        }
        private void MenuItem_Option2_Settings(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(this);
            settings.Owner = this;
            settings.ShowDialog();
        }
        private void MenuItem_Option3_AboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
        private void WINDOW_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

   
        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
        
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    if (txtResult.Text != "0")
                    {
                        txtResult.Text += e.Key.ToString().Replace("NumPad", "");
                    }
                    else
                    {
                        txtResult.Text = "";
                        txtResult.Text += e.Key.ToString().Replace("NumPad", "");
                    }

                    break;
                case Key.Add:
                    txtResult.Text += "+";
                    break;
                case Key.Subtract:
                    txtResult.Text += "-";
                    break;
                case Key.Multiply:
                    txtResult.Text += "*";
                    break;
                case Key.Divide:
                    txtResult.Text += "/";
                    break;
                case Key.Return:
                    txtResult.Text += "EEE";
                    break;
                //case Key.Enter:
                //    Pobierz tekst z kontrolki TextBox
                //    string expression = txtResult.Text;
                //    expression = expression.Replace(',', '.');
                //    Oblicz wynik wyrażenia matematycznego
                //    string result = Calculator.Calculate(expression);
                //    string filePath = "wyniki.txt"; // Ścieżka do pliku tekstowego
                //    using (StreamWriter writer = File.AppendText(filePath))
                //    {
                //        writer.WriteLine(expression + "=" + result); // Zapisz wynik w nowej linii
                //    }
                //    string[] allLines = File.ReadAllLines(filePath);
                //    Array.Reverse(allLines);
                //    string[] lastFiveLines = allLines.Take(5).ToArray();
                //    string lastFiveLinesString = string.Join(Environment.NewLine, lastFiveLines);
                //    txtRead.Text = lastFiveLinesString;
                //    Sprawdź, czy wynik kończy się na ",0" i jeśli tak, usuń ten fragment
                //    if (result.EndsWith(",0"))
                //    {
                //        result = result.Substring(0, result.Length - 2); // Usuń ostatnie dwa znaki
                //    }
                //    Dodaj wynik do pliku tekstowego

                //    Wyświetl wynik w kontrolce TextBox
                //    txtResult.Text = result;
                //    break;
                case Key.Delete:
                    if (txtResult.Text.Length > 0)
                    {
                        txtResult.Text = txtResult.Text.Substring(0, txtResult.Text.Length - 1);
                    }

                    break;
                case Key.Back:
                    if (txtResult.Text.Length > 0)
                    {
                        txtResult.Text = txtResult.Text.Substring(0, txtResult.Text.Length - 1);
                    }

                    break;
                case Key.Escape:
                    txtResult.Text = "0";
                    break;
            }
        }




    }
}
