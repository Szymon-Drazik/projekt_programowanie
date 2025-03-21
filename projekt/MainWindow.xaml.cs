using projekt.Nowy_folder;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Interface1 _bazaRepository = new BazaRepository();
        private List<string> przepisy = new List<string>
        {
            //"Spaghetti Bolognese",
            //"Pizza Margherita",
            //"Pierogi Ruskie",
            //"Tiramisu",
            //"Schabowy z ziemniakami",
            //"Kotlet"
        };
        public MainWindow()
        {
            InitializeComponent();
            przepisy = _bazaRepository.ReadAll();

            cmb_listview.ItemsSource = przepisy;
        }
        private void comboBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (przepisy == null || przepisy.Count == 0)
                return;

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                string text = comboBox.Text.ToLower();

                // Filtrujemy listę, ale nie usuwamy oryginalnych danych
                var filteredList = przepisy
                    .Where(p => p.ToLower().Contains(text))
                    .ToList();

                comboBox.ItemsSource = filteredList;

                // Otwieramy listę podpowiedzi, jeśli są wyniki
                comboBox.IsDropDownOpen = filteredList.Count > 0;
            }
        }
        private void btn_add_Click_1(object sender, RoutedEventArgs e)
        {
        //    Window1 window1 = new Window1();
        //    window1.Show();
        //    this.Close();

        }

    }
}