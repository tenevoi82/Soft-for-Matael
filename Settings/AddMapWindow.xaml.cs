using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Settings
{
    /// <summary>
    /// Логика взаимодействия для AddMapWindow.xaml
    /// </summary>
    public partial class AddMapWindow : Window
    {
        public AddMapWindow(string FilePath,string MapName)
        {
            FileName = FilePath;
            this.MapName = MapName;
            InitializeComponent();
            txtName.Text = MapName;
            try
            {
                BitmapImage im = new BitmapImage(new Uri(FilePath));
                image.Source = im;
            }
            catch (Exception ex)
            {
                OpenFileDialog addmap = new OpenFileDialog();
                addmap.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                addmap.Filter = "Файлы рисунков (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                if (addmap.ShowDialog() == true)
                {
                    try
                    {
                        BitmapImage im = new BitmapImage(new Uri(addmap.FileName));
                        image.Source = im;
                        FileName = addmap.FileName;
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show(ex2.Message);
                    }
                }
            }
        }

        public AddMapWindow()
        {
            InitializeComponent();
            Loaded += AddMapWindow_Loaded;
        }

        public string FileName;
        public string MapName;

        public static string ClearExtension(string file) => string.Join(".", file.Split('.').Take(1));

        private void AddMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addmap = new OpenFileDialog();
            addmap.Filter = "Файлы рисунков (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            if (addmap.ShowDialog() != true) return;
            FileName = addmap.FileName;
            this.Title = FileName;
            try
            {
                BitmapImage im = new BitmapImage(new Uri(FileName));
                image.Source = im;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult = false;
                Close();
            }
            txtName.Text = ClearExtension(addmap.SafeFileName);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Неверное название");
                return;
            }
            if (image.Source == null)
            {
                MessageBox.Show("Выберите файл карты");
                return;
            }
            MapName = txtName.Text;
            DialogResult = true;
            Close();   
        }

        private void ChangeMapFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addmap = new OpenFileDialog();
            addmap.Filter = "Файлы рисунков (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png|Все файлы (*.*)|*.*";
            if (addmap.ShowDialog() != true) return;
            FileName = addmap.FileName;
            this.Title = FileName;
            try
            {
                BitmapImage im = new BitmapImage(new Uri(FileName));
                image.Source = im;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult = false;
                Close();
            }
            txtName.Text = ClearExtension(addmap.SafeFileName);
        }
    }
}
