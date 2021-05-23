using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Xaml__
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TabControl tabs;
        private SolidColorBrush[] colors = new SolidColorBrush[] { new SolidColorBrush(Color.FromRgb(50, 50, 50)), 
                                                                    new SolidColorBrush(Color.FromRgb(0, 100, 0)),
                                                                    new SolidColorBrush(Color.FromRgb(0, 0, 100))};

        public MainWindow()
        {
            InitializeComponent();
            Translator.Init();

            var filesViewModel = new FilesViewModel();
            //filesViewModel.Files.Add(new FileItem("C:\\Users\\Admin\\Desktop\\MainWindow.xaml", false));
            //filesViewModel.Files.Add(new FileItem("..//path2.xaml", false, "text2"));
            DataContext = filesViewModel;

            //tabs = (TabControl)this.FindName("MainTabControl");
            //tabs.SelectedIndex = 0;
            this.MainTabControl.SelectedIndex = 0;

            this.MainMenu.DataContext = filesViewModel;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExpensiveTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox target = sender as TextBox;

            //Translator.TranslateAsync(target.Text);
            Translator.Translate(target.Text);

            target.TextChanged -= ExpensiveTextBox_TextChanged;
            target.Text = "";
            SolidColorBrush defaultColor = target.Foreground as SolidColorBrush;
            foreach(Token token in Translator.Tokens)
            {
                target.Foreground = colors[token.TableId];
                target.Text += Translator.GetRecordValue(token);
            }
            target.Foreground = defaultColor;
            target.TextChanged += ExpensiveTextBox_TextChanged;
        }
    }
}
