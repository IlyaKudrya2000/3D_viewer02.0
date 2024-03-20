


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

            OpenTkControl.Start(new GLWpfControlSettings()
            {
                MajorVersion = 1,
                MinorVersion = 1,
                RenderContinuously = false,
                UseDeviceDpi = true,

            });
            OpenTkControl.Loaded += new RoutedEventHandler(((MainWindowViewModel)DataContext).OnLoaded);
            ((MainWindowViewModel)DataContext).GLViewModel = OpenTkControl;
            OpenTkControl.Render += new Action<TimeSpan>(((MainWindowViewModel)DataContext).OnRendered);

            OpenTkControl.MouseMove += new MouseEventHandler(((MainWindowViewModel)DataContext).MouseMovee);

            OpenTkControl.MouseWheel += new MouseWheelEventHandler(((MainWindowViewModel)DataContext).MouseWheel);



        }

        private void OpenTkControl_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void OpenTkControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void OpenTkControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        private void OpenTkControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // OpenTkControl.Width = Width;
            // //OpenTkControl.Height = this.Height;
            // //OpenTkControl.Width = this.Width;
            // OpenTkControl.FrameBufferWidth = Width;
            // GL.Viewport(1, 1000, 0, 0);
            //OpenTkControl.RenderSize = (new Size(0, 0));
            


        }
    }
}