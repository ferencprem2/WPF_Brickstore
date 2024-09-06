using LegoStorageFiles.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LegoStorageFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ObservableCollection<LegoBricks> legoBrickList = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFromBSXFile()
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    XDocument document = XDocument.Load(openFileDialog.FileName);
                    foreach (var item in document.Descendants("Item"))
                    {
                        legoBrickList.Add(new(item.Element("ItemID").Value, item.Element("ItemName").Value, item.Element("CategoryName").Value, item.Element("ColorName").Value, Convert.ToInt32(item.Element("Qty").Value)));
                    }

                    legoInfoHolderDg.ItemsSource = legoBrickList;
                }
                catch (Exception)
                {
                    MessageBox.Show("A kiválaszott elem rossz formátumú, vagy nem létezik");
                }
            }
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
            LoadFromBSXFile();
        }
        
        private void SearchByIdAndName(object sender, TextChangedEventArgs e)
        {
            if (searchByTxt.Text.Any(char.IsDigit))
            {
                legoInfoHolderDg.ItemsSource = legoBrickList.Where(x => x.ItemId.StartsWith(searchByTxt.Text));
                
            } else
            {
                legoInfoHolderDg.ItemsSource = legoBrickList.Where(x => x.ItemName.ToLower().StartsWith(searchByTxt.Text) || x.CategoryName.ToLower().StartsWith(searchByTxt.Text));
            }

            if (searchByTxt.Text == "")
            {
                legoInfoHolderDg.ItemsSource = legoBrickList;
            }
        }
    }
}