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
using Microsoft.Win32;



namespace projekt
{
    public partial class Window2 : Window
    {
        private byte[] imageBlob;
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

                var filteredList = przepisy
                    .Where(p => p.ToLower().Contains(text))
                    .ToList();

                comboBox.ItemsSource = filteredList;

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

                
                 imageBlob = File.ReadAllBytes(selectedFileName);

                
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipe = cmb_listview.SelectedItem as string;
            txt_name.Clear();
            txt_ingredients.Clear();
            txt_description.Clear();
            img_display.Source = null;  


            if (!string.IsNullOrEmpty(selectedRecipe))
            {
                var (recipeDetails, recipeImage) = _bazaRepository.GetRecipeDetails(selectedRecipe);
                var detailsParts = recipeDetails.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                txt_name.Text = selectedRecipe;
                txt_ingredients.Text = detailsParts[0].Replace("Składniki: ", "");
                txt_description.Text = detailsParts[1].Replace("Opis: ", "");
                img_display.Source = recipeImage;
            }
            else
            {
                MessageBox.Show("Wybierz przepis z listy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {


            string selectedRecipe = cmb_listview.SelectedItem as string;
            string nazwa = txt_name.Text;
            string skladniki = txt_ingredients.Text;
            string opis = txt_description.Text;

            Baza nowyPrzepis = new Baza
            {
                Nazwa_przepisu = nazwa,
                Skladniki = skladniki,
                Opis = opis,
                Zdjecie = imageBlob
            };
            if (nowyPrzepis.Nazwa_przepisu != "")
            {
                BazaRepository repo = new BazaRepository();
                bool success = repo.Update(nowyPrzepis, selectedRecipe);

                if (success)
                {
                    MessageBox.Show("Przepis edytowany pomyślnie!");
                    cmb_listview.ClearValue(ComboBox.ItemsSourceProperty);
                    przepisy = _bazaRepository.ReadAll();

                    cmb_listview.ItemsSource = przepisy;
                    txt_name.Clear();
                    txt_ingredients.Clear();
                    txt_description.Clear();
                    img_display.Source = null;
                }
            }
            else
            {
                MessageBox.Show("Wystąpił błąd podczas edytowania przepisu.");
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
