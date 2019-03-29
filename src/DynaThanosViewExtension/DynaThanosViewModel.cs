using System;
using System.Collections.Generic;
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
        private static Random rng = new Random();

        public DynaThanosViewModel(ViewLoadedParams p)
        {
            loadedParams = p;
            _vm = loadedParams.DynamoWindow.DataContext as DynamoViewModel;
        }

       

        public static void RespondToSnap()
        {
            //get the nodes and shuffle them randomly
            var nodes = _vm.CurrentSpace.Nodes.ToList();
            Shuffle(nodes);
            //find fifty percent that need to be removed
            //"When I’m done, half of your nodes will still exist. Perfectly balanced, as all things should be. I hope they remember you."
            var fiftyPercent = nodes.GetRange(0, nodes.Count / 2);

            //zoom to the candidates to watch them fade away
            _vm.AddToSelectionCommand.Execute(fiftyPercent);
            _vm.FitViewCommand.Execute(null);

            int flag = 0;
            //obtain the node view to fade
            var nodeViews = loadedParams.DynamoWindow.FindVisualChildren<NodeView>();

            //loop through the nodes while delaying the time each loop
            while (flag < fiftyPercent.Count())
            {
                _currentNode = fiftyPercent[flag];
                var nodeView = nodeViews.First(n => n.ViewModel.Name.Equals(_currentNode.Name));

                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(flag+1));
                nodeView.BeginAnimation(Control.OpacityProperty, animation);
                flag++;
            }



        }
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
