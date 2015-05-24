using System;
using System.Collections.Generic;
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
using WPFLocalizeExtension.Engine;

namespace WPFLocalizeExample
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
            InitializeComponent();
        }

        private void ComboBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* Чтобы локализация заработала, кроме имплементации всего того, что есть в этом примере (код MainWindow.xaml,
             * MainWindow.xaml.cs, файлы ресурсов .resx в /Properties), нужно изменить .csproj файл соответствующего проекта
             * для файлов .resx, чтобы он выглядел как в этом проекте). */

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(((ComboBoxItem)sender).Name);
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = culture;
            Money.CultureInfo = culture;
        }
    }
}
