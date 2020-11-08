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
using System.IO.Compression;
using System.Threading.Tasks;

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
            ReadImageFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
              "Monika", needImages, images);
            Background.Source = images[0];

        }

        SettingsFile config_file = new SettingsFile();
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        List<string> needImages = new List<string>()
        {
            "Default.png"
        };
        List<BitmapImage> images = new List<BitmapImage>();
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
            if (Directory.Exists(path))
            {
                DirectoryInfo folder = new DirectoryInfo(path);
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
        void ReadImageFromZip(string zipPath, string Folder, List<string> needingImagesList, List<BitmapImage> image_list)
        {
            ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
            {
                try
                {
                    foreach (var entry in archive.Entries)
                    {

                        if (entry.FullName.Contains(Folder) & needingImagesList.Contains(entry.Name))
                        {
                            try
                            {
                                
                               BitmapImage src = new BitmapImage();
                                src.DownloadCompleted += (s, e) =>
                                {
                                    archive.Dispose();
                                };
                               
                               src.BeginInit();
                               src.CacheOption = BitmapCacheOption.OnLoad;
                               src.StreamSource = entry.Open();
                               src.EndInit();
                               image_list.Add(src);
                               needingImagesList.RemoveAt(needingImagesList.FindIndex(item => item == entry.Name));
                               
                               //MessageBox.Show(image_list[0].ToString());
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "Hui");
                            }
                        }
    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Main_Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReadImageFromZip(@"C:\Users\Игорь\Desktop\done\NCE_content\images.zip",
               "Monika", needImages, images);
            Background.Source = images[0];
        }
    }
}
