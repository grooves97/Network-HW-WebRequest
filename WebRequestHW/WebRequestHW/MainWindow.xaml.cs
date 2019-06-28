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

namespace WebRequestHW
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Get()
        {
            try
            {
                Uri url = new Uri(urlTextBox.Text);
                HttpWebRequest httpRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                httpRequest.Method = WebRequestMethods.Http.Post;
                HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;

                string data;

                var encoding = Encoding.GetEncoding(httpResponse.CharacterSet);
                using (var responseStream = httpResponse.GetResponseStream())
                using (var reader = new StreamReader(responseStream, encoding))
                    data = reader.ReadToEnd();


                List<string> sentences = new List<string>();
                foreach (string sentence in data.Split(new char[] { '>', '<' }
                        , StringSplitOptions.RemoveEmptyEntries))
                {
                    sentences.Add(sentence);
                }
                List<string> deleteSentence = new List<string>();
                foreach (string sentence in sentences)
                {
                    if (sentence.Contains("/") || sentence.Contains("\n") || sentence.Contains("=") || sentence.Contains("]") || sentence.Contains("[") || string.IsNullOrWhiteSpace(sentence))
                    {
                        deleteSentence.Add(sentence);
                    }
                }
                foreach (string sentence in deleteSentence)
                {
                    sentences.Remove(sentence);
                }

                Dictionary<string, int> vacabulary = new Dictionary<string, int>();
                foreach (string str in sentences)
                {
                    foreach (string word in str.Split(new char[] { ' ', ',', '.' }
                        , StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (vacabulary.ContainsKey(word))
                        {
                            vacabulary[word]++;
                        }
                        else
                        {
                            vacabulary.Add(word, 1);
                        }

                    }
                }
                foreach (KeyValuePair<string, int> pair in vacabulary)
                {
                    wordsListBox.Items.Add($"{pair.Value} {pair.Key}");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void GetTextButtonClick(object sender, RoutedEventArgs e)
        {
            Get();
        }
    }
}
