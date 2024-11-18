using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje L = new Lenguaje())
                {
                    L.genera();
                   /*while (!L.finArchivo())
                   {
                        L.nextToken();
                   }*/
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
    
}