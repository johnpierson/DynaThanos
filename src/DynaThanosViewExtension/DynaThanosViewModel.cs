using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Dynamo.Controls;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Xceed.Wpf.AvalonDock.Controls;

namespace DynaThanosViewExtension
{
    public class DynaThanosViewModel : NotificationObject
    {
        private string activeNodeTypes;
        private static ViewLoadedParams loadedParams;
        private static NodeModel _currentNode;
        private static DynamoViewModel _vm;

        public DynaThanosViewModel(ViewLoadedParams p)
        {
            loadedParams = p;
            _vm = loadedParams.DynamoWindow.DataContext as DynamoViewModel;
        }

       

        public static void RespondToSnap()
        {
            var nodes = _vm.CurrentSpace.Nodes.ToList();

            int flag = 0;
            var nodeViews = loadedParams.DynamoWindow.FindVisualChildren<NodeView>();

            while (flag < nodes.Count())
            {
                _currentNode = nodes[flag];
                var nodeView = nodeViews.First(n => n.ViewModel.Name.Equals(_currentNode.Name));

                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(flag+1));
                nodeView.BeginAnimation(Control.OpacityProperty, animation);
                flag++;
            }



        }

        private static void TimerOnTick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
