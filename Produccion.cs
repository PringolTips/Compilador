using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Produccion
    {
        
        public Produccion(String entrada, String Primero)
        {
            nombre = entrada;
            Primer_caracter = Primero;
            Ors = new List<Epsilon_or>();
        }
        public void Agregar_lista()
        {
            Ors.Add(new Epsilon_or());
        }
        public List<Epsilon_or> Ors { get; set; }
        public string nombre{ get; set; }
        public string  Primer_caracter{ get; set;}
        
        

    }
}