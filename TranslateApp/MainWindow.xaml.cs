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

		//Список языков
		private List<Locale> locales;
		//Текущий язык
		private Locale currentLocale;
		//Путь к файлу с языками
		private string localesPath = "locales.json";
		
		public MainWindow()
		{
			InitializeComponent();

			//После инициализации приложения загружаем языки
			locales = new List<Locale>();
			GetLocales();

		}

		private void GetLocales()
		{
			//Получаем данные из файла
			string json = File.ReadAllText(localesPath);

			//Конвертируем данные в объект с динамическими полями
			dynamic data = JsonConvert.DeserializeObject(json);

			//Проходимся по языкам из файла
			foreach(var item in data.locales)
			{
				//Создаём словарь
				Dictionary<string, string> values = new Dictionary<string, string>();

				//Помещаем все значения из файла в словарь
				foreach(var val in item.values) 
				{
					values.Add(val.name.ToString(), val.value.ToString());
				}

				//Добавляем в список новый язык
				locales.Add(new Locale(item.name.ToString(), values));
			}

			//Обновляем кнопки
			UpdateButtons();
		}

		public void UpdateButtons()
		{
			//Удаляем старые кнопки
			LocaleButtons.Children.Clear();

			//Создаём новую кнопку для каждого языка и добавляем её на панель для кнопок
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
			//Получаем кнопку, которая была нажата
			Button btn = (Button)sender;

			//Ищем язык по названиию
			foreach(Locale locale in locales)
			{
				if (btn.Content.ToString() == locale.Name)
				{
					//Меняем текущий язык
					currentLocale = locale;
					break;
				}
			}

			//Обновляем надписи в приложении
			UpdateLocale();
		}

		private void UpdateLocale()
		{
			//Проверяем все записи в словаре текущего языка
			foreach(string key in currentLocale.Values.Keys)
			{
				//Ещем элемент, имя которого совпадает с ключом из словаря
				object elem = FindName(key);

				//Пробуем определить, какого типа может быть элемент и прописываем различную логику
				//Вы можете сделать этот код более универсальными, но в данном приложении всего два типа элементов - кнопки и текстовые блоки
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
