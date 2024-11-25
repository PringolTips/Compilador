using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token
    {
        public enum Tipos
        {
            ST, SNT, FinProduccion, Epsilon , Or, Izquierdo, Derecho, Flecha, Tipo
            
        };
        public Token()
        {
            Contenido = "";
            Clasificacion = Tipos.Tipo;
        }
         public string Contenido
        {get ;set;}
        public Tipos Clasificacion
        {get; set;}

    }
}