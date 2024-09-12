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
        private string currentIdOptions = "";
        private string currentNameOptions = "";
        private string currentCategoryOptions = "";
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

                   
                }
                catch (Exception)
                {
                    MessageBox.Show("A kiválaszott elem rossz formátumú, vagy nem létezik");
                }
                
                legoInfoHolderDg.ItemsSource = legoBrickList;
            }
            sortByCategoryCbx.Items.Clear();
            sortByCategoryCbx.Items.Add("All");
            legoBrickList.Select(x => x.CategoryName).Distinct().ToList().ForEach(y => sortByCategoryCbx.Items.Add(y));
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
            LoadFromBSXFile();
        }
        
        private void SearchByItemName(object sender, TextChangedEventArgs e)
        {
            try
            {
                currentNameOptions = searchByTxt.Text.ToLower();
                sortByCategoryCbx.Items.Clear();
                sortByCategoryCbx.Items.Add("All");
                foreach (var item in legoBrickList.Where(x => x.CategoryName.ToLower().Contains(currentNameOptions)).ToList())
                {
                    if (!sortByCategoryCbx.Items.Contains(item.CategoryName))
                    {
                        sortByCategoryCbx.Items.Add(item.CategoryName);
                    }
                }
                ApplyFilters();
                
            }
            catch (Exception nameSearchException)
            {
                MessageBox.Show(nameSearchException.Message);
            }
        }

        private void SearchByItemId(object sender, TextChangedEventArgs e)
        {
            try
            {
                currentIdOptions = searchByIdTxt.Text.ToLower();
                ApplyFilters();
            }
            catch (Exception idSearchException)
            {
                MessageBox.Show(idSearchException.Message);
            }
        }

        private void CategorySorter(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sortByCategoryCbx.SelectedItem != null)
                {
                    currentCategoryOptions = sortByCategoryCbx.SelectedItem.ToString();
                    ApplyFilters();
                }
            }
            catch (Exception categorySearchException)
            {
                MessageBox.Show(categorySearchException.Message);
            }
        }

        private void ApplyFilters()
        {
            var view = CollectionViewSource.GetDefaultView(legoBrickList);
            view.Filter = item =>
            {
                if (item is LegoBricks legoBrick)
                {
                    bool matchesId = string.IsNullOrEmpty(currentIdOptions) || legoBrick.ItemId.ToLower().Contains(currentIdOptions);
                    bool matchesName = string.IsNullOrEmpty(currentNameOptions) || legoBrick.ItemName.ToLower().Contains(currentNameOptions);
                    bool mathcesCategory = string.IsNullOrEmpty(currentCategoryOptions) || currentCategoryOptions == "All" || legoBrick.CategoryName.Contains(currentCategoryOptions);

                    return matchesId && matchesName && mathcesCategory;
                }
                return false;
            };
            view.Refresh();
        }
    }
}