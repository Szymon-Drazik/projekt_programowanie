using System;
using System.Collections.Generic;
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
using System.IO;
using projekt.Nowy_folder;



namespace projekt
{
    public partial class Window2 : Window
    {
        private readonly Interface1 _bazaRepository = new BazaRepository();
        private List<string> przepisy;
        BazaRepository repo = new BazaRepository();
        public Window2()
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
        private void btn_loadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;

                // Wyświetl obraz w kontrolce Image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                img_display.Source = bitmap;

                // Konwersja obrazu do tablicy bajtów (blob)
                byte[] imageBlob = File.ReadAllBytes(selectedFileName);

                // Tutaj możesz użyć imageBlob według potrzeb, np. do zapisania w bazie danych
            }
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
