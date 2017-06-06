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
using System.Windows.Media.Animation;

namespace WindowsExplorer
{
    /// <summary>
    /// FileIcon.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FileIcon : UserControl
    {//각 파일이미지에 걸려있는 이미지와 이벤트 
        TextBlock fileTextBlock = new TextBlock();
        int flag; string name;
       
        public FileIcon(string str)
        {
            InitializeComponent();
            flag = 0;
            //try
            //{
                Rectangle rect = new Rectangle();
                ImageBrush ibrush = new ImageBrush();
                ibrush.ImageSource = getIcon(str);
                rect.Fill = ibrush;
                rect.Width = 70;
                rect.Height = 70;
                rect.VerticalAlignment = VerticalAlignment.Top;
                rect.HorizontalAlignment = HorizontalAlignment.Center;

                //TextBlock tb = new TextBlock();
                fileTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                fileTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                FileInfo fi = new FileInfo(str);
                str = fi.Name;
                name = str;

                if (str.Length > 7)
                {
                    str = str.Substring(0, 7) + "...";
                }

                fileTextBlock.Text = str;
                fileTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                fileTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                fileTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

                root.VerticalAlignment = VerticalAlignment.Stretch;
                root.HorizontalAlignment = HorizontalAlignment.Stretch;
                root.Children.Add(rect);
                root.Children.Add(fileTextBlock);
                root.Margin = new Thickness(2);
            //}
            this.MouseDown += new MouseButtonEventHandler(FileIcon_MouseDown);
            this.MouseEnter += new MouseEventHandler(FileIcon_MouseEnter);
            this.MouseLeave += new MouseEventHandler(FileIcon_MouseLeave);
        }

        public System.Windows.Media.ImageSource getIcon(string filename)
        {
            System.Windows.Media.ImageSource icon;
            
            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filename))
            {
                icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(

                sysicon.Handle,

                System.Windows.Int32Rect.Empty,

                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }

            return icon;
        }

        void FileIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (flag == 0)
            {

                root.Background = new SolidColorBrush(Colors.SkyBlue);
                fileTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                string shortName = fileTextBlock.Text;
                fileTextBlock.Text = name;
                name = shortName;
                flag = 1;
            }
            else
            {
                root.Background = new SolidColorBrush(Colors.Transparent);
                fileTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                string longName = fileTextBlock.Text;
                fileTextBlock.Text = name;
                name = longName;
                flag = 0;
            }
        }

        void FileIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            root.Background = new SolidColorBrush(Colors.SkyBlue);
        }

        void FileIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            root.Background = new SolidColorBrush(Colors.Transparent);
            if (flag == 1)
            {
                root.Background = new SolidColorBrush(Colors.SteelBlue);
            }
            else if (flag == 0)
            {
                root.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
