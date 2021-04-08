using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Path = System.IO.Path;
using System.Windows.Forms;
using System;
using System.Windows.Media;
using System.Collections.Generic;

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

                    Parallel.ForEach(picList, (f) => CopyFile(f, sortedJPGDir));
                }

                if (searchMovie)
                {
                    Directory.CreateDirectory(sortedMOVDir);
                    string[] movList = Directory.GetFiles(sourceDir, "*.mov", SearchOption.AllDirectories);
                    string[] mp4List = Directory.GetFiles(sourceDir, "*.mp4", SearchOption.AllDirectories);

                    Parallel.ForEach(movList, (f) => CopyFile(f, sortedMOVDir));

                    Parallel.ForEach(mp4List, (f) => CopyFile(f, sortedMOVDir));
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

        private void CopyFile(string filePath, string directoryPath)
        {
            var fileName = Path.GetFileName(filePath);

            File.Copy(filePath, Path.Combine(directoryPath, fileName), true);
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
