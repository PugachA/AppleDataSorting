using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Path = System.IO.Path;
using System.Windows.Forms;
using System;
using System.Windows.Media;

namespace AppleDataSorting
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

        string sourceDir;
        string sortedJPGDir;
        string sortedMOVDir;
        bool searchPicture;
        bool searchMovie;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchPicture = (bool)pictureCheckBox.IsChecked;
            searchMovie = (bool)movieCheckBox.IsChecked;
            sourceDir = sourceTextBox.Text;
            sortedJPGDir = resultTextBox.Text + @"\Pictures";
            sortedMOVDir = resultTextBox.Text + @"\Movies";
            InfoTextBlock.Text = "Начали разбирать";
            InfoTextBlock.Foreground = Brushes.Green;
            Task categoriesTask = Task.Factory.StartNew(Processing);

        }

        private void Processing()
        {
            try
            {
                if (searchPicture)
                {
                    Directory.CreateDirectory(sortedJPGDir);
                    string[] picList = Directory.GetFiles(sourceDir, "*.jpg", SearchOption.AllDirectories);

                    foreach (string f in picList)
                    {
                        // Remove path from the file name.
                        string fName = Path.GetFileName(f);

                        // Use the Path.Combine method to safely append the file name to the path.
                        // Will overwrite if the destination file already exists.
                        File.Copy(f, Path.Combine(sortedJPGDir, fName), true);
                    }

                }

                if (searchMovie)
                {
                    Directory.CreateDirectory(sortedMOVDir);
                    string[] movList = Directory.GetFiles(sourceDir, "*.mov", SearchOption.AllDirectories);
                    string[] mp4List = Directory.GetFiles(sourceDir, "*.mp4", SearchOption.AllDirectories);


                    foreach (string f in movList)
                    {
                        // Remove path from the file name.
                        string fName = Path.GetFileName(f);

                        // Use the Path.Combine method to safely append the file name to the path.
                        // Will overwrite if the destination file already exists.
                        File.Copy(f, Path.Combine(sortedMOVDir, fName), true);
                    }

                    foreach (string f in mp4List)
                    {
                        // Remove path from the file name.
                        string fName = Path.GetFileName(f);

                        // Use the Path.Combine method to safely append the file name to the path.
                        // Will overwrite if the destination file already exists.
                        File.Copy(f, Path.Combine(sortedMOVDir, fName), true);
                    }
                }

                Dispatcher.Invoke(delegate { InfoTextBlock.Foreground = Brushes.Green; InfoTextBlock.Text = "Готово"; });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(delegate
                {
                    InfoTextBlock.Foreground = Brushes.Red;
                    InfoTextBlock.Text = $"Ошибка: {ex.Message}; {ex}";
                });
            }
        }

        private void sourceDirectoryPath_Click(object sender, RoutedEventArgs e)
        {
            //Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    FileInfo fi = new FileInfo(openFileDialog.FileName);
            //    sourceDir=fi.DirectoryName;
            //}
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();
            sourceTextBox.Text = folderBrowser.SelectedPath;
        }

        private void resultDirectoryPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();
            resultTextBox.Text = folderBrowser.SelectedPath;
        }
    }
}
