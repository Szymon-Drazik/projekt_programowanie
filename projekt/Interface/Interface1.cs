using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt.Nowy_folder
{
    interface Interface1
    {
        bool Create(Baza baza);
        Baza Read(int id);
        List<Baza> ReadAll();
        bool Update(Baza baza);
        bool Delete(int id);
        bool Add();

    }
}
