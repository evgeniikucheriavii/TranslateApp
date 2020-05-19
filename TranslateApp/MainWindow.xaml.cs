using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranslateApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private List<Locale> locales;
		private Locale currentLocale;
		private string localesPath = "locales.json";
		
		public MainWindow()
		{
			InitializeComponent();

			locales = new List<Locale>();

			GetLocales();

		}

		private void GetLocales()
		{
			string json = File.ReadAllText(localesPath);

			dynamic data = JsonConvert.DeserializeObject(json);

			foreach(var item in data.locales)
			{
				Dictionary<string, string> values = new Dictionary<string, string>();

				foreach(var val in item.values) 
				{
					values.Add(val.name.ToString(), val.value.ToString());
				}

				locales.Add(new Locale(item.name.ToString(), values));
			}

			UpdateButtons();
		}

		public void UpdateButtons()
		{
			LocaleButtons.Children.Clear();

			foreach(Locale locale in locales)
			{
				Button btn = new Button();

				btn.Content = locale.Name;
				btn.Click += LocaleButton_Click;
				btn.Width = 25;
				btn.Height = 25;
				btn.Margin = new Thickness(5.0);

				LocaleButtons.Children.Add(btn);
			}
		}

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentLocale == null)
			{
				MessageBox.Show($"Hello, {NameTextBox.Text}!", "Hello!", MessageBoxButton.OK);
			}
			else
			{
				MessageBox.Show($"{currentLocale.Values["HelloMessage"]}{NameTextBox.Text}", currentLocale.Values["HelloCaption"], MessageBoxButton.OK);
			}
			
		}

		private void LocaleButton_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;

			foreach(Locale locale in locales)
			{
				if (btn.Content.ToString() == locale.Name)
				{
					currentLocale = locale;
					break;
				}
			}

			UpdateLocale();
		}

		private void UpdateLocale()
		{
			foreach(string key in currentLocale.Values.Keys)
			{
				object elem = FindName(key);

				try
				{
					Button btn = (Button)elem;

					if (btn != null)
					{
						btn.Content = currentLocale.Values[key];
					}
				}
				catch (Exception e) { }

				try
				{
					TextBlock text = (TextBlock)elem;

					if(text != null)
					{
						text.Text = currentLocale.Values[key];
					}
				}
				catch (Exception e) { }
			}
		}
	}
}
