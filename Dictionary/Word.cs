using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SelectedLanguage Language { get; set; }

        public override string ToString()
        {
            return Name+" "+Description;
        }
    }
}
