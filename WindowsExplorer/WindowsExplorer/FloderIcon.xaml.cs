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
        ColorAnimation ca = new ColorAnimation(Colors.Violet, new Duration(TimeSpan.FromMilliseconds(500)));
        TextBlock tb = new TextBlock();
        int fc;
        string name;
        public FloderIcon(string str)
        {
            InitializeComponent();
            fc = 0; name = str;
            Rectangle rect = new Rectangle();

            rect.Fill = new ImageBrush(new BitmapImage(new Uri(@"folderopened_yellow.png", UriKind.RelativeOrAbsolute)));
            rect.Width = 70;
            rect.Height = 70;
            rect.VerticalAlignment = VerticalAlignment.Top;
            rect.HorizontalAlignment = HorizontalAlignment.Center;

            //TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Bottom;
            tb.HorizontalAlignment = HorizontalAlignment.Center;


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
            
            this.MouseDown += new MouseButtonEventHandler(FolderIcon_MouseDown);

        }

        void FolderIcon_MouseDown(object sender, MouseButtonEventArgs e)
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
        void FolderIcon_MouseEnter()
        {

        }
    }
    
}
