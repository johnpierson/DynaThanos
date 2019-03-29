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
using System.Windows.Threading;
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
        private static List<NodeView> _nodeViews;
        private static List<NodeModel> _fiftyPercent;
        private static InfoBubbleViewModel _infoBubble;
        public DynaThanosViewModel(ViewLoadedParams p)
        {
            loadedParams = p;
            _vm = loadedParams.DynamoWindow.DataContext as DynamoViewModel;
        }

        public static void ZoomToFit()
        {
            //get the nodes and shuffle them randomly
            var nodes = _vm.CurrentSpace.Nodes.ToList();
            Shuffle(nodes);
            //find fifty percent that need to be removed
            //"When I’m done, half of your nodes will still exist. Perfectly balanced, as all things should be. I hope they remember you."
            _fiftyPercent = nodes.GetRange(0, nodes.Count / 2);

            //zoom to the candidates to watch them fade away
            _vm.AddToSelectionCommand.Execute(_fiftyPercent);
            _vm.FitViewCommand.Execute(null);
        }


        public static void RespondToSnap()
        {

            int flag = 0;
            double fadeTime = 0.75;
            //obtain the node view to fade
            _nodeViews = loadedParams.DynamoWindow.FindVisualChildren<NodeView>().ToList();
            //loop through the nodes while delaying the time each loop
            while (flag < _fiftyPercent.Count())
            {
                _currentNode = _fiftyPercent[flag];
                var nodeView = _nodeViews.First(n => n.ViewModel.Id.ToString().Equals(_currentNode.GUID.ToString()));
                if (flag == _fiftyPercent.Count()-1)
                {
                    //future info bubble display
                    _infoBubble = nodeView.ViewModel.ErrorBubble;
                    nodeView.ViewModel.ErrorBubble.InfoBubbleState = InfoBubbleViewModel.State.Pinned;
                    nodeView.ViewModel.ErrorBubble.InfoBubbleStyle = InfoBubbleViewModel.Style.Warning;
                    nodeView.ViewModel.ErrorBubble.Content = "i don't feel so good....";
                    nodeView.ViewModel.ErrorBubble.FullContent = nodeView.Uid;
                }


                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(fadeTime)); 
                nodeView.BeginAnimation(Control.OpacityProperty, animation);
                flag++;
                fadeTime = fadeTime + 0.75;
            }
    
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(fadeTime);
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private static void TimerOnTick(object sender, EventArgs e)
        {
            var guids = _fiftyPercent.Select(n => n.GUID);

            _vm.Model.ExecuteCommand(new DynamoModel.DeleteModelCommand(guids));
            

            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerOnTick;

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
