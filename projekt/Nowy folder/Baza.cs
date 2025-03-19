using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt.Nowy_folder
{
    class Baza
    {
        public int Id { get; set; }
        public string Nazwa_przepisu { get; set; }

        public string Skladniki { get; set; }
        public string Opis { get; set; }
        public byte[] Zdjecie { get; set; }
    }
}
