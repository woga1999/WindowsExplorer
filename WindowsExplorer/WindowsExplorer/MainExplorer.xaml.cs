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
        //public DateTime lastClick = DateTime.Now.AddSeconds(-1);
        //public FileIcon originFicon = new FileIcon("");
        //public FloderIcon origin = new FloderIcon("");
        private object dummyNode = null;
        public List<string> savePath = new List<string>();
        int folderCount = 0;

        public MainExplorer()
        {
            InitializeComponent();
            //exception = new DirectoryExcept(this);
        }
        public string SelectedPath { get; set; } //폴더 주소 저장

        private void treeView_Loaded(object sender, RoutedEventArgs e) //트리뷰 로드 
        {
            foreach (string itemName in Directory.GetLogicalDrives())
            {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = itemName;
                    item.Tag = itemName;
                    item.FontWeight = FontWeights.Normal;
                    item.Items.Add(dummyNode);
                    item.Expanded += new RoutedEventHandler(folder_Expanded); //아이템 늘릴 때 마다 하위 폴더들 늘어난다.
                    treeView.Items.Add(item);
                
            }
            //}
            //catch (Exception) { }
        }

        void folder_Expanded(object sender, RoutedEventArgs e) //폴더 확장
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
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded); //하위 폴더 확장
                        item.Items.Add(subitem);
                    }
                }
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) //트리뷰 아이템들 선택할 때마다 밑에 하위 폴더들 나타난다
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

        void ClickTreeView(string path) //트리뷰 선택시 혹은 폴더 선택시 하위 폴더들 wrapPanel들 띄우게
        {
            RightWrap.Children.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            string name;
            foreach (var item in directoryInfo.GetDirectories())
            {
                if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 폴더들은 뜨지 않게 한다
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
                    floder.MouseLeftButtonDown += new MouseButtonEventHandler(bt_Click); //폴더마다 클릭 이벤트 심는다
                }
            }

            foreach (var fileItem in directoryInfo.GetFiles())
            {
                if ((fileItem.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일들 뜨지 않는다.
                {
                    name = fileItem.ToString();
                    if (path.Equals("C:\\") || path.Equals("D:\\"))
                    {
                        FileIcon fileIcon = new FileIcon(path + name);
                        fileIcon.Tag = path + name;
                        RightWrap.Children.Add(fileIcon);
                        fileIcon.MouseLeftButtonDown += new MouseButtonEventHandler(fIcon_MouseLeftButtonDown); //파일마다 클릭이벤트 심기
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
            pathTextBox.Text = path; //주소 텍스트 박스에 주소 옮길 때마다 띄게
        }

        void bt_Click(object sender, MouseButtonEventArgs e) //폴더 클릭 이벤트 
        {
            FloderIcon folder = sender as FloderIcon; 

            if (e.ClickCount == 2)//더블 클릭 시
            {
                 ClickTreeView(folder.Tag.ToString()); //폴더랑 파일 보여주는 메서드
                savePath.Add(folder.Tag.ToString()); //폴더 돌아가며 볼때마다 주소를 리스트에 저장
                folderCount++; //수를 센다
                SelectedPath = folder.Tag.ToString(); //주소 저장
                pathTextBox.Text = folder.Tag.ToString(); //텍스트 박스에 주소 뜨게 한다.
                btn_back.IsEnabled = true;
            }
        }

        void fIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) //파일 아이콘 클릭시 
        {
            FileIcon fileIcon = sender as FileIcon;
            if(e.ClickCount == 2)//더블 클릭 시
            {
                System.Diagnostics.Process.Start(fileIcon.Tag.ToString()); //외부 파일 실행
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e) //뒤로가기 버튼
        {
            if (folderCount != 0) //폴더 클릭 한 카운트 수가 1 이상이여야 움직인다.
            {
                btn_forward.IsEnabled = true; //뒤로가기 버튼 클릭시 앞으로 가기 버튼 활성화
                for (int i =0; i<savePath.Count; i++) //리스트에 저장된 주소 리스트들 카운트해서 카운트 만큼
                {
                    if (savePath[i] == SelectedPath) //지금 주소랑 리스트에 저장된 주소랑 같으면
                    {
                        if (i == 0) //비교해서 전꺼 주소로 간다 대신 처음으로 가면 뒤로가기 버튼 입력하지 못한다.
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i]);
                            SelectedPath = savePath[i];
                            btn_back.IsEnabled = false;
                            break;
                        }
                        else if (i >= 1) //비교해서 전꺼 주소로 간다
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
            if (folderCount != 0) //폴더 클릭 카운트가 1이상이여야 작동
            {
                for (int i = 0; i < savePath.Count; i++) //리스트 수만큼 반복문 돌아서
                {
                    if (savePath[i] == SelectedPath) //지금 현 주소와 같으면
                    {
                        if (i == savePath.Count-1) // 현재 배열 다음 주소로 간다
                        {
                            RightWrap.Children.Clear();
                            ClickTreeView(savePath[i]);
                            SelectedPath = savePath[i];
                            btn_back.IsEnabled = true;
                            btn_forward.IsEnabled = false; // 다만 마지막 배열이면 앞으로가기 실행못시킨다.
                        }
                        else if (i < savePath.Count-1)// 현재 배열 다음 주소로 간다
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

        private void btn_go_Click(object sender, RoutedEventArgs e) //텍스트 박스에 입력을 하고 go 버튼을 누르면 그 주소에 있는 곳으로 간다.
        {
            string ofText = exception.judgeExistDirectory(pathTextBox.Text); //예외처리 빈칸이나 null, 경로가 존재하지 않을 시 
            if(ofText == "Don't Exist") { MessageBox.Show("입력하신 경로를 찾을 수 없습니다."); } //경로를 찾을 수 없다고 뜬다.
            else
            {
                RightWrap.Children.Clear(); //폴더 띄우는 공간 클리어하고
                ClickTreeView(ofText); //주소에 따라 폴더랑 파일 띄우고
                pathTextBox.Text = ofText; //텍스트박스에 주소 띄운다.
                savePath.Add(ofText); //뒤로가기 앞으로가기를 위해 주소 리스트에 저장~!
                folderCount++; //폴더 카운트
                SelectedPath = ofText; //셀렉주소에 텍스트 박스 쳤던 주소 저장
            }
        }
    }
}
