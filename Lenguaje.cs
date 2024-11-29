using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada         -Done
    Requerimiento 2: Implementar la cerradura Epsilo
    Requerimiento 3: Implementar la el operador OR
    Requerimiento 4: Indentar el código
*/

namespace Compilador
{
    public class Lenguaje : Sintaxis
    {
        private bool first = true;
        private string pertenece = "";
        private int espacios = 0;
        private int lis_ors = 0;
        private int cont = 0;
        private bool Primero_list = false;
        private List<Produccion> Producciones;
        public Lenguaje()
        {
            Producciones = new List<Produccion>();
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
            espacios++;
            Indentado();
            lenguajecs.WriteLine("public class Lenguaje : Sintaxis");
            Indentado();
            lenguajecs.WriteLine("{");
            espacios++;
            Indentado();
            lenguajecs.WriteLine("public Lenguaje()");
            Indentado();
            lenguajecs.WriteLine("{");
            Indentado();
            lenguajecs.WriteLine("}");
            Indentado();
            lenguajecs.WriteLine("public Lenguaje(string nombre) : base(nombre)");
            Indentado();
            lenguajecs.WriteLine("{");
            Indentado();
            lenguajecs.WriteLine("}");
        }

        public void genera()
        {
            match("namespace");
            match(":");
            match(Tipos.SNT);
            //int temp = reinicio;
            match(";");
            lectura();
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(0, SeekOrigin.Begin);
            log.BaseStream.SetLength(0);
            nextToken();
            linea = 1;
            match("namespace");
            match(":");
            esqueleto(Contenido);
            match(Tipos.SNT);
            match(";");
            producciones();
            espacios--;
            Indentado();
            lenguajecs.WriteLine("}");
            espacios--;
            Indentado();
            lenguajecs.WriteLine("}");
        }
        private void producciones()
        {
            if (Clasificacion == Tipos.SNT && first)
            {
                Indentado();
                lenguajecs.WriteLine("public void " + Contenido + "()");
                Indentado();
                lenguajecs.WriteLine("{");
                first = false;
            }
            else if (Clasificacion == Tipos.SNT)
            {
                Indentado();
                lenguajecs.WriteLine("private void " + Contenido + "()");
                Indentado();
                lenguajecs.WriteLine("{");
            }
            pertenece = Contenido;
            match(Tipos.SNT);
            match(Tipos.Flecha);
            espacios++;
            conjuntoTokens();
            Primero_list = false;
            espacios--;
            match(Tipos.FinProduccion);
            Indentado();
            lenguajecs.WriteLine("}");
            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }
        }

        private void conjuntoTokens()
        {

            if (Clasificacion == Tipos.SNT)
            {
                Indentado();
                lenguajecs.WriteLine("" + Contenido + "();");
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
            {
                Indentado();
                lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                match(Tipos.ST);
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                Indentado();
                lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                match(Tipos.Tipo);
            }
            else if (Clasificacion == Tipos.Izquierdo)
            {
                if (Primero_list)
                {
                    lis_ors++;
                }
                Primero_list = true;
                match(Tipos.Izquierdo);
                var v = Producciones.Find(v => v.nombre == pertenece);
                int temp = espacios;
                if (v.Ors[lis_ors].Es_epsilon)
                {
                    Indentado();
                    lenguajecs.Write("if (");
                    if (Clasificacion == Tipos.SNT)
                    {
                        var u = Producciones.Find(u => u.nombre == Contenido);
                        if (u.Primer_caracter == "\\;")
                        {
                            throw new Error("Produccion vacía en:  " + linea, log);
                        }
                        lenguajecs.WriteLine("getContenido() ==  " + "\"" + u.Primer_caracter + "\" );");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine(Contenido + "()");
                        espacios--;
                        match(Tipos.SNT);
                    }
                    else if (Clasificacion == Tipos.ST)
                    {
                        lenguajecs.WriteLine("getContenido() ==  " + Contenido + ")");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine("match(" + Contenido + ");");
                        espacios--;
                        match(Tipos.ST);
                    }
                    else if (Clasificacion == Tipos.Tipo)
                    {
                        lenguajecs.WriteLine("getClasificacion() ==  " + "Tipos." + Contenido + ")");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                        espacios--;
                        match(Tipos.Tipo);
                    }
                    espacios++;
                    if (v.Ors[lis_ors].Es_or == false)
                    {
                        conjuntoTokens();
                    }
                    espacios--;


                    //espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");
                    /* if (Clasificacion == Tipos.Izquierdo)
                     {*/

                    if (v.Ors[lis_ors].Es_or)
                    {
                        Indentado();
                        lenguajecs.Write("if(");
                        cont++;
                        if (Clasificacion == Tipos.SNT)
                        {
                            var u = Producciones.Find(u => u.nombre == Contenido);
                            lenguajecs.WriteLine("getContenido() == \"" + u.Primer_caracter + "\")");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine("{");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine(Contenido + "()");
                            espacios--;
                            Indentado();
                            lenguajecs.WriteLine("}");
                            espacios--;
                            match(Tipos.SNT);
                        }
                        if (Clasificacion == Tipos.ST)
                        {
                            lenguajecs.WriteLine("getContenido() == \"" + Contenido + "\")");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine("{");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                            espacios--;
                            Indentado();
                            lenguajecs.WriteLine("}");
                            espacios--;
                            match(Tipos.ST);
                        }
                        else if (Clasificacion == Tipos.Tipo)
                        {
                            lenguajecs.WriteLine("getClasigficacion() == Tipos." + Contenido + ")");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine("{");
                            espacios++;
                            Indentado();
                            lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                            espacios--;
                            Indentado();
                            lenguajecs.WriteLine("}");
                            espacios--;
                            match(Tipos.Tipo);
                        }
                        OR_Token();
                        cont = 0;
                        //}
                    }

                }
                else if (v.Ors[lis_ors].Es_or)
                {
                    Indentado();
                    lenguajecs.Write("if(");
                    cont++;
                    if (Clasificacion == Tipos.SNT)
                    {
                        var u = Producciones.Find(u => u.nombre == Contenido);
                        lenguajecs.WriteLine("getContenido() == \"" + u.Primer_caracter + "\")");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine(Contenido + "()");
                        espacios--;
                        Indentado();
                        lenguajecs.WriteLine("}");
                        match(Tipos.SNT);
                    }
                    if (Clasificacion == Tipos.ST)
                    {
                        lenguajecs.WriteLine("getContenido() == \"" + Contenido + "\")");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                        espacios--;
                        Indentado();
                        lenguajecs.WriteLine("}");
                        match(Tipos.ST);
                    }
                    else if (Clasificacion == Tipos.Tipo)
                    {
                        lenguajecs.WriteLine("getClasigficacion() == Tipos." + Contenido + ")");
                        Indentado();
                        lenguajecs.WriteLine("{");
                        espacios++;
                        Indentado();
                        lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                        espacios--;
                        Indentado();
                        lenguajecs.WriteLine("}");
                        match(Tipos.Tipo);
                    }
                    OR_Token();
                    elseEscrito = false;
                    cont = 0;

                }
                match(Tipos.Derecho);
                if (Clasificacion == Tipos.Epsilon)
                {
                    match(Tipos.Epsilon);
                }
                if (v.Ors[lis_ors].Es_or == false && v.Ors[lis_ors].Es_epsilon == false)
                {
                    throw new Error("No hay un OR o cerradura Epsilon en la linea: " + linea, log);
                }
            }
            if (Clasificacion != Tipos.FinProduccion && Clasificacion != Tipos.Derecho)
            {
                conjuntoTokens();
            }
        }
        private void lectura()
        {
            pertenece = Contenido;
            match(Tipos.SNT);
            match(Tipos.Flecha);
            Producciones.Add(new Produccion(pertenece, Contenido));
            tokens();
            match(Tipos.FinProduccion);
            if (Clasificacion == Tipos.SNT)
            {
                lectura();
            }
        }
        private void tokens()
        {
            var v = Producciones.Find(v => v.nombre == pertenece);
            if (Clasificacion == Tipos.SNT)
            {
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
            {
                match(Tipos.ST);
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                match(Tipos.Tipo);
            }
            if (Clasificacion == Tipos.Izquierdo)
            {
                match(Tipos.Izquierdo);
                v.Agregar_lista();
            }
            else if (Clasificacion == Tipos.Derecho)
            {

                match(Tipos.Derecho);
            }
            else if (Clasificacion == Tipos.Or)
            {

                v.Ors[lis_ors].Es_or = true;
                v.Ors[lis_ors].cont++;
                match(Tipos.Or);
            }
            else if (Clasificacion == Tipos.Epsilon)
            {
                v.Ors[lis_ors].Es_epsilon = true;
                match(Tipos.Epsilon);

            }
            if (Clasificacion != Tipos.FinProduccion)
            {
                tokens();
            }

        }
        private void Indentado()
        {
            for (int i = 0; i < espacios; i++)
            {
                lenguajecs.Write("\t");
            }
        }
        private bool elseEscrito = false, elseIf = false;
        private void OR_Token()
        {
            var v = Producciones.Find(v => v.nombre == pertenece);
            if (v.Ors[lis_ors].cont > cont && elseIf)
            {
                Indentado();
                lenguajecs.Write("else if(");
                elseIf = false;
            }
            else if (!elseIf)
            {
                elseIf = true;
            }
            else
            {
                if (!elseEscrito)
                {
                    Indentado();
                    lenguajecs.WriteLine("else");
                    elseEscrito = true;
                    Indentado();
                    lenguajecs.WriteLine("{");
                }

            }

            if (Clasificacion == Tipos.SNT)
            {
                if (v.Ors[lis_ors].cont > cont)
                {    
                    var u = Producciones.Find(u => u.nombre == Contenido);
                    lenguajecs.WriteLine("getContenido() == \"" + u.Primer_caracter + "\")");
                    Indentado();
                    lenguajecs.WriteLine("{");
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine(Contenido + "();");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");
                    cont++;

                }
                else
                {
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine(Contenido + "();");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");
                }
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
            {
                if (v.Ors[lis_ors].cont > cont)
                {
                    lenguajecs.WriteLine("getContenido() == \"" + Contenido + "\")");
                    Indentado();
                    lenguajecs.WriteLine("{");
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");
                    cont++;

                }
                else
                {
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");

                }
                match(Tipos.ST);
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                if (v.Ors[lis_ors].cont < cont)
                {
                    lenguajecs.WriteLine("getClasigficacion() == Tipos." + Contenido + ")");
                    Indentado();
                    lenguajecs.WriteLine("{");
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");
                    cont++;
                }
                else
                {
                    espacios++;
                    Indentado();
                    lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                    espacios--;
                    Indentado();
                    lenguajecs.WriteLine("}");

                }
                match(Tipos.Tipo);
            }
            else if (Clasificacion == Tipos.Or)
            {
                match(Tipos.Or);
            }

            if (Clasificacion != Tipos.Derecho)
            {
                OR_Token();
            }

        }
    }
}