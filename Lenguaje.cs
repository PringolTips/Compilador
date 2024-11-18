using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada
    Requerimiento 2: Implementar la cerradura Epsilo
    Requerimiento 3: Implementar la el operador OR
    Requerimiento 4: Indentar el código
*/

namespace Compilador
{
    public class Lenguaje : Sintaxis
    {
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        private void esqueleto(string nspace)
        {
            lenguajecs.WriteLine("using System;");
            lenguajecs.WriteLine("using System.Collections.Generic;");
            lenguajecs.WriteLine("using System.Linq;");
            lenguajecs.WriteLine("using System.Net.Http.Headers;");
            lenguajecs.WriteLine("using System.Reflection.Metadata.Ecma335;");
            lenguajecs.WriteLine("using System.Runtime.InteropServices;");
            lenguajecs.WriteLine("using System.Threading.Tasks;");
            lenguajecs.WriteLine("\nnamespace " + nspace);
            lenguajecs.WriteLine("{");
            lenguajecs.WriteLine("    public class Lenguaje : Sintaxis");
            lenguajecs.WriteLine("    {");
            lenguajecs.WriteLine("        public Lenguaje()");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
        }
        public void genera()
        {
            match("namespace");
            match(":");
            esqueleto(Contenido);
            match(Tipos.SNT);
            match(";");
            producciones();
            lenguajecs.WriteLine("    }");
            lenguajecs.WriteLine("}");
        }
        private void producciones()
        {
            if (Clasificacion == Tipos.SNT)
            {
                lenguajecs.WriteLine("        public void " + Contenido + "()");
                lenguajecs.WriteLine("        {");
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens();
            match(Tipos.FinProduccion);
            lenguajecs.WriteLine("        }");
            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }
        }
        private void conjuntoTokens()
        {
            if (Clasificacion == Tipos.SNT)
            {
                lenguajecs.WriteLine("            " + Contenido + "();");
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
            {
                lenguajecs.WriteLine("            match(\"" + Contenido + "\");");
                match(Tipos.ST);
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                lenguajecs.WriteLine("            match(Tipos." + Contenido + ");");
                match(Tipos.Tipo);
            }
            else if (Clasificacion == Tipos.Izquierdo)
            {
                match(Tipos.Izquierdo);
                lenguajecs.Write("            if (");
                if (Clasificacion == Tipos.ST)
                {
                    lenguajecs.WriteLine("getContenido() == \"" + Contenido + "\")");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                match(\"" + Contenido + "\");");
                    match(Tipos.ST);
                }
                else if (Clasificacion == Tipos.Tipo)
                {
                    lenguajecs.WriteLine("getClasigficacion() == Tipos." + Contenido + ")");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                match(Tipos." + Contenido + ");");
                    match(Tipos.Tipo);
                }
                match(Tipos.Derecho);
                lenguajecs.WriteLine("            }");
            }
            if (Clasificacion != Tipos.FinProduccion)
            {
                conjuntoTokens();
            }
        }
    }
}