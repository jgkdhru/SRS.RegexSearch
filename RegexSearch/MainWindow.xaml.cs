using System.Windows;
using RegexSearch.Services;

namespace RegexSearch
{
    public partial class MainWindow : Window
    {
        private SearchService _service = new SearchService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RegexButton_Click(object sender, RoutedEventArgs e)
        {
            var results = _service.FindWithRegex(InputText.Text);
            ResultsGrid.ItemsSource = results;
            StatusText.Text = $"Найдено через Regex: {results.Count}";
        }

        private void AutomatonButton_Click(object sender, RoutedEventArgs e)
        {
            var results = _service.FindWithAutomaton(InputText.Text);
            ResultsGrid.ItemsSource = results;
            StatusText.Text = $"Найдено через автомат: {results.Count}";
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            var regex = _service.FindWithRegex(InputText.Text);
            var auto = _service.FindWithAutomaton(InputText.Text);

            bool isEqual = regex.Count == auto.Count;
            if (isEqual)
            {
                for (int i = 0; i < regex.Count; i++)
                {
                    if (regex[i].Value != auto[i].Value || regex[i].Position != auto[i].Position)
                    {
                        isEqual = false;
                        break;
                    }
                }
            }

            StatusText.Text = isEqual ? "УСПЕХ: Результаты совпадают!" : "ОШИБКА: Результаты НЕ совпадают!";
            StatusText.Foreground = isEqual ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
            ResultsGrid.ItemsSource = regex;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InputText.Text = "";
            ResultsGrid.ItemsSource = null;
            StatusText.Text = "Очищено";
            StatusText.Foreground = System.Windows.Media.Brushes.Black;
        }
    }
}