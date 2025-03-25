using Microsoft.Win32;
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
using System.Xml.Linq;

namespace projekt
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private byte[] imageBlob;
        public Window1()
        {
            InitializeComponent();
        }
        private void btn_loadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                img_display.Source = bitmap;


                imageBlob = File.ReadAllBytes(selectedFileName);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btn_save_Click_1(object sender, RoutedEventArgs e)
        {
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

            BazaRepository repo = new BazaRepository();
            bool success = repo.Create(nowyPrzepis);

            if (success)
            {
                MessageBox.Show("Przepis dodany pomyślnie!");
                txt_name.Clear();
                txt_ingredients.Clear();
                txt_description.Clear();
                img_display.Source = null;
            }
            else
            {
                MessageBox.Show("Wystąpił błąd podczas dodawania przepisu.");
            }
        }
    }
}
