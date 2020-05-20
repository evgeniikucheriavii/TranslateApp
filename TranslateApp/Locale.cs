using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents.DocumentStructures;

namespace TranslateApp
{
	public class Locale
	{
		//Название языка
		private string name;

		//Словарь с надписями на этом языке
		//Ключ - название элемента, который нужно перевести
		private Dictionary<string, string> values;

		public Locale(string name, Dictionary<string, string> values)
		{
			this.name = name;
			this.values = values;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public Dictionary<string, string> Values
		{
			get
			{
				return this.values;
			}
		}
	}
}
