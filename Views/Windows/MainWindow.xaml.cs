


using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using _3D_viewer.ViewModels;

using _3D_viewer.Models;
//using System.Windows.UI.Xaml.Controls;

namespace _3D_viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {

            DataContext = new MainWindowViewModel();
            InitializeComponent();

           ///
           /// 
           /// 
          //Vector a = new Vector(1.0, 2.0);
          // Vector b = new Vector(3.0, 2.0);
          // Vector c = a + b;
          //double aL =  a.Length;
          // double bL = b.Length;
          // double cL = c.Length;
            ///
            OpenTkControl.Start(new GLWpfControlSettings()
            {
                MajorVersion = 1,
                MinorVersion = 1,
                RenderContinuously = false,
                
            });
            OpenTkControl.Loaded += new RoutedEventHandler(((MainWindowViewModel)DataContext).OnLoaded);
            ((MainWindowViewModel)DataContext).GLViewModel = OpenTkControl;
            OpenTkControl.Render += new Action<TimeSpan>(((MainWindowViewModel)DataContext).OnRendered);

            OpenTkControl.MouseMove += new MouseEventHandler(((MainWindowViewModel)DataContext).MouseMovee);
            //OpenTkControl.MouseLeftButtonDown += new MouseEventHandler(((MainWindowViewModel)DataContext).MouseMovee);
        }

        private void OpenTkControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void OpenTkControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}