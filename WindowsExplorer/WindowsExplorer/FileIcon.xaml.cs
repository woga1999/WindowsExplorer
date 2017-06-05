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
    {
        TextBlock tb = new TextBlock();
        int fc; string name;
        ColorAnimation ca = new ColorAnimation(Colors.Aquamarine, new Duration(TimeSpan.FromMilliseconds(500)));

        public FileIcon(string str)
        {
            InitializeComponent();
            fc = 0;
            try
            {
                Rectangle rect = new Rectangle();
                ImageBrush ibrush = new ImageBrush();
                ibrush.ImageSource = getIcon(str);
                rect.Fill = ibrush;
                rect.Width = 70;
                rect.Height = 70;
                rect.VerticalAlignment = VerticalAlignment.Top;
                rect.HorizontalAlignment = HorizontalAlignment.Center;

                //TextBlock tb = new TextBlock();
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                FileInfo fi = new FileInfo(str);
                str = fi.Name;
                name = str;

                if (str.Length > 7)
                {
                    str = str.Substring(0, 7) + "...";
                }

                tb.Text = str;
                tb.Foreground = new SolidColorBrush(Colors.Black);
                tb.HorizontalAlignment = HorizontalAlignment.Center;

                tb.HorizontalAlignment = HorizontalAlignment.Center;

                root.VerticalAlignment = VerticalAlignment.Stretch;
                root.HorizontalAlignment = HorizontalAlignment.Stretch;
                root.Children.Add(rect);
                root.Children.Add(tb);
                root.Margin = new Thickness(2);
            }
            catch (Exception) { }
            this.MouseDown += new MouseButtonEventHandler(FileIcon_MouseDown);
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
            if (fc == 0)
            {

                root.Background = new SolidColorBrush(Colors.WhiteSmoke);
                tb.Foreground = new SolidColorBrush(Colors.Green);
                string shortName = tb.Text;
                tb.Text = name;
                name = shortName;
                fc = 1;
            }
            else
            {
                root.Background = new SolidColorBrush(Colors.Transparent);
                tb.Foreground = new SolidColorBrush(Colors.Black);
                string longName = tb.Text;
                tb.Text = name;
                name = longName;
                fc = 0;
            }
        }
        void FileIcon_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
