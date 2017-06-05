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
using System.Windows.Media.Animation;

namespace WindowsExplorer
{
    /// <summary>
    /// FloderIcon.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FloderIcon : UserControl
    {
        TextBlock folderTextBlock = new TextBlock();
        int flag;
        string name;

        public FloderIcon(string str)
        {
            InitializeComponent();
            flag = 0; name = str;
            Rectangle rect = new Rectangle();

            rect.Fill = new ImageBrush(new BitmapImage(new Uri(@"folderopened_yellow.png", UriKind.RelativeOrAbsolute)));
            rect.Width = 70;
            rect.Height = 70;
            rect.VerticalAlignment = VerticalAlignment.Top;
            rect.HorizontalAlignment = HorizontalAlignment.Center;

            //TextBlock tb = new TextBlock();
            folderTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            folderTextBlock.HorizontalAlignment = HorizontalAlignment.Center;


            if (str.Length > 7)
            {
                str = str.Substring(0, 7) + "...";
            }

            folderTextBlock.Text = str;
            folderTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            folderTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

            folderTextBlock.HorizontalAlignment = HorizontalAlignment.Center;

            root.VerticalAlignment = VerticalAlignment.Stretch;
            root.HorizontalAlignment = HorizontalAlignment.Stretch;
            root.Children.Add(rect);
            root.Children.Add(folderTextBlock);
            root.Margin = new Thickness(2);
            
            this.MouseUp += new MouseButtonEventHandler(FolderIcon_MouseUp);
            this.MouseEnter += new MouseEventHandler(FolderIcon_MouseEnter);
            this.MouseLeave += new MouseEventHandler(FolderIcon_MouseLeave);
        }

        void FolderIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (flag == 0)
            {
                root.Background = new SolidColorBrush(Colors.SteelBlue);
                folderTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                string shortName = folderTextBlock.Text;
                folderTextBlock.Text = name;
                name = shortName;
                flag = 1;
            }
            else
            {
                root.Background = new SolidColorBrush(Colors.Transparent);
                folderTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                string longName = folderTextBlock.Text;
                folderTextBlock.Text = name;
                name = longName;

                flag = 0;
            }
        }
        void FolderIcon_MouseLeave(object sender, MouseEventArgs e)
        {
           root.Background = new SolidColorBrush(Colors.Transparent);
            if(flag == 1)
            {
                root.Background = new SolidColorBrush(Colors.SteelBlue);
            }
            else if(flag == 0)
            {
                root.Background = new SolidColorBrush(Colors.Transparent);
            }
            
        }
        

        void FolderIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            root.Background = new SolidColorBrush(Colors.SkyBlue);
        }
    }
    
}
