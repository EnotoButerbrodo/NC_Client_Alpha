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
using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NC_Client_Alpha
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            FirstSetup();
            GetImages(@"C:\Users\Игорь\Desktop\done\NCE_content\Backgrounds",
                backgrounds);
            Background.Source = backgrounds[0];


            //SettingsFile loadConfig = LoadConfig();
            //MessageBox.Show(loadConfig.Widowd_Height.ToString());
        }

        SettingsFile config_file = new SettingsFile();
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        void FirstSetup()
        {
            LoadConfig();
            Application.Current.MainWindow.Width = config_file.Window_Width;
            Application.Current.MainWindow.Height = config_file.Window_Height;

        }
        void LoadConfig()
        {
            string load_config;
            try
            {
            using (FileStream fs = new FileStream("config.json", FileMode.Open, FileAccess.Read))
            {
                using (var stream = new StreamReader(fs))
                {
                    load_config = stream.ReadToEnd();
                    config_file =  JsonSerializer.Deserialize<SettingsFile>(load_config);
                }
            }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SaveConfig()
        {
            string save_config = JsonSerializer.Serialize<SettingsFile>(config_file);

            try
            {
                using (FileStream fs = new FileStream("config.json", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using(var stream = new StreamWriter(fs))
                    {
                        stream.Write(save_config);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
