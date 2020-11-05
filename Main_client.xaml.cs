using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NC_Client_Alpha
{
    /// <summary>
    /// Логика взаимодействия для Main_client.xaml
    /// </summary>
    public partial class Main_client : Window
    {
        public Main_client()
        {
            InitializeComponent();
        }









        private void GetImages(string path, List<BitmapImage> list)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                foreach (FileInfo fileInfo in folder.GetFiles())
                {
                    if (".jpg|.jpeg|.png".Contains(fileInfo.Extension.ToLower()))
                    {
                        BitmapImage src = new BitmapImage();
                        src.BeginInit();
                        src.UriSource = new Uri(fileInfo.FullName, UriKind.Absolute);
                        src.EndInit();
                        list.Add(src);
                    }
                }
            }
        }

        private void GetTexts(string path, List<string> list)
        {
            string line;
            using (StreamReader stream = new StreamReader(path))
            {
                while ((line = stream.ReadLine()) != null)
                {
                   list.Add(line);
                }
            }
        }
    }
}
