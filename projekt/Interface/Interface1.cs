using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace projekt.Nowy_folder
{
    interface Interface1
    {
        bool Create(Baza baza);
        List<string> ReadAll();
        bool Update(Baza baza,string selected);
        bool Delete(string Nazwa_przepisu);
        (string, BitmapImage) GetRecipeDetails(string nazwaPrzepisu);

    }
}
