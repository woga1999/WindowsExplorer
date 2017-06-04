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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WindowsExplorer
{
    /// <summary>
    /// MainExplorer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainExplorer : UserControl
    {
        DirectoryInfo info;
        private object dummyNode = null;
        public MainExplorer()
        {
            InitializeComponent();
            info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
        public string SelectedPath { get; set; }
        private void treeView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string itemName in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = itemName;
                item.Tag = itemName;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                treeView.Items.Add(item);
            }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                {
                    TreeViewItem subitem = new TreeViewItem();
                    subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                    
                    subitem.Tag = s;
                    subitem.FontWeight = FontWeights.Normal;

                    subitem.Items.Add(dummyNode);
                    subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                    item.Items.Add(subitem);
                }

            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = sender as TreeView;
            TreeViewItem temp = tree.SelectedItem as TreeViewItem;

            if (temp == null)
                return;
            SelectedPath = "";
            string temp1 = "";
            string temp2 = "";
            while (true)
            {
                temp1 = temp.Header.ToString();
                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                SelectedPath = temp1 + temp2 + SelectedPath;
                if (temp.Parent.GetType().Equals(typeof(TreeView)))
                {
                    break;
                }
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }
            //show user selected path
            pathTextBox.Text = SelectedPath;
        }
        
    }
}
