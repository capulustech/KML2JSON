using System.Collections.Generic;
using System.IO;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace kml2json
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool multiGeometry = false;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void FileAddressButton_Click(object sender, RoutedEventArgs e)
        {
            
            WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = fbd.ShowDialog();
            fileAddressBox.IsReadOnly = false;
            if (fbd.SelectedPath != null)
            {
                fileAddressBox.Text = fbd.SelectedPath;
            }
            fileAddressBox.IsReadOnly = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            Helper helper = new Helper();
            fileAddressBox.IsReadOnly = false;
            if ((fileAddressBox.Text != null)&&(fileAddressBox.Text.Length > 1))
            {
                if (Directory.Exists(fileAddressBox.Text))
                {
                    List<string> files = new List<string>(Directory.GetFiles(fileAddressBox.Text));
                    int i = 0;
                    files.Sort();
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".kml") || file.EndsWith(".KML"))
                        {
                            if (!multiGeometry)
                                helper.GetCordinates(file, fileAddressBox.Text);
                            else
                                helper.GetCordinatesMultiGeometry(file, fileAddressBox.Text);
                            i++;
                        }
                    }
                    if (i > 0)
                    {
                        MessageBox.Show("Success!\n" + i + " files processed and saved at\n" + fileAddressBox.Text + "\\json");
                    }
                    else
                    {
                        MessageBox.Show("No compatible file found! Make sure you have kml files in the selected directory.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Directory");
                }
            }
            fileAddressBox.IsReadOnly = true;
            startButton.IsEnabled = true;

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            fileAddressBox.Text = "Choose Directory...";
        }

        private void UtilizeImage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://utilizesoftwares.com");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool status = MultiGeometry.IsChecked.GetValueOrDefault();
            if (status)
            {
                multiGeometry = true;
            }
            else
            {
                multiGeometry = false;
            }
        }

        private void Host_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void InsertIntoDbButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
