using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Lenguaje
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
			A();
			match("b");
			C();
		}
		private void A()
		{
			C();
			if (getClasificacion() ==  Tipos.Numero)
			{
				match(Tipos.Numero);
				match("b");
				C();
				D();
			}
			D();
		}
		private void C()
		{
			match("d");
			if(getContenido() == "a")
			{
				match("a");
			}
			else if(getContenido() == "d")
			{
				C();
			}
			else if(getContenido() == "d")
			{
				D();
			}
			else
			{
				D();
			}
		}
		private void D()
		{
			match("d");
		}
	}
}
