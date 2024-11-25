using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Cplusplus
{
	public class Lenguaje : Sintaxis
	{
		public Lenguaje()
		{
		}
		public Lenguaje(string nombre) : base(nombre)
		{
		}
		public void Programa()
		{
			if (getContenido() ==  "#" )
			{
				Librerias()
			}
			Main();
		}
		private void Librerias()
		{
			match("#");
			match("include");
			match("<");
			match(Tipos.Identificador);
			match(">");
		}
		private void Main()
		{
			match("void");
			match("main");
			match("(");
			match(")");
			BloqueInstrucciones();
		}
		private void BloqueInstrucciones()
		{
		}
		private void If()
		{
			match("if");
			match("(");
			match(Tipos.Condicion);
			match(")");
			if(getContenido() == "BloqueInstrucciones")
				{
					BloqueInstrucciones()
				}
			}
		}
		}
