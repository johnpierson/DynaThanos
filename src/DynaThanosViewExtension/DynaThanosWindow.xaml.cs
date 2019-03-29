using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;
using WpfAnimatedGif;

namespace DynaThanosViewExtension
{
    /// <summary>
    /// Interaction logic for SampleWindow.xaml
    /// </summary>
    public partial class DynaThanosWindow : Window
    {
        public DynaThanosWindow()
        {
            InitializeComponent();
            this.Snap.Visibility = Visibility.Hidden;
            DynaThanosViewModel.ZoomToFit();
            this.Icon = new BitmapImage(
                new Uri("pack://application:,,,/DynaThanosViewExtension;component/Resources/thanosEmoji.png"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Loading.Visibility = Visibility.Hidden;
            this.Snap.Visibility = Visibility.Visible;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.75);
            timer.Tick += TimerOnTick;
            timer.Start();

        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            DynaThanosViewModel.RespondToSnap();
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerOnTick;
            Close();
        }
    }
}
