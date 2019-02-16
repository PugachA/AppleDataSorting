using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Path = System.IO.Path;
using System.Windows.Forms;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sourceDir = sourceTextBox.Text;
            sortedJPGDir = resultTextBox.Text + @"\Pictures";
            sortedMOVDir = resultTextBox.Text + @"\Movies";
            InfoTextBlock.Text = "Начали разбирать";
            Task categoriesTask = Task.Factory.StartNew(Processing);

        }

        private void Processing()
        {
            Directory.CreateDirectory(sortedJPGDir);
            Directory.CreateDirectory(sortedMOVDir);
            string[] picList = Directory.GetFiles(sourceDir, "*.jpg");
            string[] movList = Directory.GetFiles(sourceDir, "*.mov");
            string[] mp4List = Directory.GetFiles(sourceDir, "*.mp4");
            foreach (string f in picList)
            {
                // Remove path from the file name.
                string fName = f.Substring(sourceDir.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(sourceDir, fName), Path.Combine(sortedJPGDir, fName), true);
            }

            foreach (string f in movList)
            {
                // Remove path from the file name.
                string fName = f.Substring(sourceDir.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(sourceDir, fName), Path.Combine(sortedMOVDir, fName), true);
            }

            foreach (string f in mp4List)
            {
                // Remove path from the file name.
                string fName = f.Substring(sourceDir.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(sourceDir, fName), Path.Combine(sortedMOVDir, fName), true);
            }

            Dispatcher.Invoke(delegate { InfoTextBlock.Text = "Готово"; });
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
