using System;
using System.Windows;

namespace Translator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ClearText(object sender, RoutedEventArgs e)
        {
            Source.Clear();

            Result.Clear();
        }

        public void Translate(object sender, RoutedEventArgs e)
        {
            var appid  = "";

            var key    = "";

            var result = BaiduTranslator.GetFromAndTo(Option.Text);

            Result.Text = BaiduTranslator.Translate(BaiduTranslator.GetURL(
                result.Item1, result.Item2, appid, Source.Text, new Random().Next(), key));
        }
    }
}
