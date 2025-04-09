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
        private List<string> przepisy;
        BazaRepository repo = new BazaRepository();

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

                var filteredList = przepisy
                    .Where(p => p.ToLower().Contains(text))
                    .ToList();

                comboBox.ItemsSource = filteredList;

                comboBox.IsDropDownOpen = filteredList.Count > 0;
            }
        }
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            this.Close();
        }

        private void btn_edit_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 edytujprzepis = new Window2();
            edytujprzepis.Show();
            this.Close();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipe = cmb_listview.SelectedItem as string;
            txt_name.Clear();
            txt_ingredients.Clear();
            txt_description.Clear();
            img.Source = null;


            if (!string.IsNullOrEmpty(selectedRecipe))
            {
                var (recipeDetails, recipeImage) = _bazaRepository.GetRecipeDetails(selectedRecipe);
                var detailsParts = recipeDetails.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                txt_name.Text = selectedRecipe;
                txt_ingredients.Text = detailsParts[0].Replace("Składniki: ", "");
                txt_description.Text = detailsParts[1].Replace("Opis: ", "");
                img.Source = recipeImage;
            }
            else
            {
                MessageBox.Show("Wybierz przepis z listy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            string selectedRecipe = txt_name.Text;

            if (!string.IsNullOrEmpty(selectedRecipe))
            {
                var result = MessageBox.Show($"Czy na pewno chcesz usunąć przepis '{selectedRecipe}'?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool isDeleted = _bazaRepository.Delete(selectedRecipe);

                    if (isDeleted)
                    {
                        MessageBox.Show("Przepis został usunięty.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                        txt_name.Clear();
                        txt_ingredients.Clear();
                        txt_description.Clear();
                        img.Source = null;

                        przepisy = _bazaRepository.ReadAll();

                        cmb_listview.ItemsSource = przepisy;
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się usunąć przepisu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wybierz przepis do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


