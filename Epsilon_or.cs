using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Epsilon_or
    {
        public Epsilon_or()
        {
            Es_or = false;
            Es_epsilon = false;
            cont = 0;
        }
        public bool Es_or{ get; set; }
        public bool Es_epsilon{ get; set; }
        public int cont { get; set; }
        
    }
}