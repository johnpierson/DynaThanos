using System.Windows;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

                DynaThanosViewModel.RespondToSnap();
            
           
        }
    }
}
