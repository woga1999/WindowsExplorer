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
        private DirectoryExcept exception = new DirectoryExcept();
        public DateTime lastClick = DateTime.Now.AddSeconds(-1);
        public FileIcon originFicon = new FileIcon("");
        public FloderIcon origin = new FloderIcon("");
        private object dummyNode = null;
        public List<string> savePath = new List<string>();
        int folderCount = 0;

        public MainExplorer()
        {
            InitializeComponent();
            //exception = new DirectoryExcept(this);
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
            //}
            //catch (Exception) { }
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                foreach (string subitem_name in Directory.GetDirectories(item.Tag.ToString()))
                {
                    DirectoryInfo directory = new DirectoryInfo(subitem_name);
                    if ((directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = subitem_name.Substring(subitem_name.LastIndexOf("\\") + 1);

                        subitem.Tag = subitem_name;
                        subitem.FontWeight = FontWeights.Normal;

                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = sender as TreeView;
            TreeViewItem temp = tree.SelectedItem as TreeViewItem;
            savePath.Add(temp.Tag.ToString());
            folderCount++;
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
            ClickTreeView(SelectedPath);
            pathTextBox.Text = SelectedPath;
        }

        void ClickTreeView(string path)
        {
            RightWrap.Children.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            string name;
            foreach (var item in directoryInfo.GetDirectories())
            {
                if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    name = item.ToString();
                    FloderIcon floder = new FloderIcon(name);
                    if (path.Equals("C:\\") || path.Equals("D:\\"))
                    {
                        floder.Tag = path + name;
                    }
                    else
                    {
                        floder.Tag = path + "\\" + name;
                    }
                    RightWrap.Children.Add(floder);
                    floder.MouseLeftButtonDown += new MouseButtonEventHandler(bt_Click);
                }
            }

            foreach (var fileItem in directoryInfo.GetFiles())
            {
                if ((fileItem.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    name = fileItem.ToString();
                    if (path.Equals("C:\\") || path.Equals("D:\\"))
                    {
                        FileIcon fileIcon = new FileIcon(path + name);
                        fileIcon.Tag = path + name;
                        RightWrap.Children.Add(fileIcon);
                        fileIcon.MouseLeftButtonDown += new MouseButtonEventHandler(fIcon_MouseLeftButtonDown);
                    }

                    else
                    {
                        FileIcon fileIcon = new FileIcon(path + "\\" + name);
                        fileIcon.Tag = path + "\\" + name;
                        RightWrap.Children.Add(fileIcon);
                        fileIcon.MouseLeftButtonDown += new MouseButtonEventHandler(fIcon_MouseLeftButtonDown);
                    }
                }

            }
            pathTextBox.Text = path;
        }

        void bt_Click(object sender, MouseButtonEventArgs e)
        {
            FloderIcon folder = sender as FloderIcon; 

            if (e.ClickCount == 2)//더블 클릭 시
            {
                 ClickTreeView(folder.Tag.ToString()); //폴더랑 파일 보여주는 메서드
                savePath.Add(folder.Tag.ToString());
                folderCount++;
                SelectedPath = folder.Tag.ToString();
                pathTextBox.Text = folder.Tag.ToString();
                btn_back.IsEnabled = true;
            }
        }

        void fIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FileIcon fileIcon = sender as FileIcon;
            if(e.ClickCount == 2)//더블 클릭 시
            {
                System.Diagnostics.Process.Start(fileIcon.Tag.ToString()); //외부 파일 실행
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (folderCount != 0)
            {
                btn_forward.IsEnabled = true;
                for (int i =0; i<savePath.Count; i++)
                {
                    if (savePath[i] == SelectedPath)
                    {
                        if (i == 0)
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i]);
                            SelectedPath = savePath[i];
                            btn_back.IsEnabled = false;
                            break;
                        }
                        else if (i >= 1)
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i - 1]);
                            SelectedPath = savePath[i - 1];
                            break;
                        }
                    }
                }
            }
        }

        private void btn_forward_Click(object sender, RoutedEventArgs e)
        {
            if (folderCount != 0)
            {
                for (int i = 0; i < savePath.Count; i++)
                {
                    if (savePath[i] == SelectedPath)
                    {
                        if (i == savePath.Count-1)
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i]);
                            SelectedPath = savePath[i];
                            btn_back.IsEnabled = true;
                            btn_forward.IsEnabled = false;
                        }
                        else if (i < savePath.Count-1)
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i + 1]);
                            SelectedPath = savePath[i + 1];
                            btn_back.IsEnabled = true;
                            break;
                        }
                    }
                }
            }
        }

        private void btn_go_Click(object sender, RoutedEventArgs e)
        {
            string ofText = exception.judgeExistDirectory(pathTextBox.Text);
            if(ofText == "Don't Exist") { MessageBox.Show("입력하신 경로를 찾을 수 없습니다."); }
            else
            {
                RightWrap.Children.Clear();
                ClickTreeView(ofText);
                pathTextBox.Text = ofText;
                savePath.Add(ofText);
                folderCount++;
                SelectedPath = ofText;
            }
        }
    }
}
