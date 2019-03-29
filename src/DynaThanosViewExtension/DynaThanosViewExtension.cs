using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dynamo.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynaThanosViewExtension
{
    /// <summary>
    /// The View Extension framework for Dynamo allows you to extend
    /// the Dynamo UI by registering custom MenuItems. A ViewExtension has 
    /// two components, an assembly containing a class that implements 
    /// IViewExtension, and an ViewExtensionDefintion xml file used to 
    /// instruct Dynamo where to find the class containing the
    /// IViewExtension implementation. The ViewExtensionDefinition xml file must
    /// be located in your [dynamo]\viewExtensions folder.
    /// 
    /// This sample demonstrates an IViewExtension implementation which 
    /// shows a modeless window when its MenuItem is clicked. 
    /// The Window created tracks the number of nodes in the current workspace, 
    /// by handling the workspace's NodeAdded and NodeRemoved events.
    /// </summary>
    public class DynaThanosViewExtension : IViewExtension
    {
        private MenuItem dynaThanosMenuItem;

        public void Dispose()
        {
        }
        public static DynamoView view;
        public void Startup(ViewStartupParams p)
        {
        }

        public void Loaded(ViewLoadedParams p)
        {
            // Save a reference to your loaded parameters.
            // You'll need these later when you want to use
            // the supplied workspaces

            view = p.DynamoWindow as DynamoView;

            dynaThanosMenuItem = new MenuItem {Header = "DynaThanos"};
            dynaThanosMenuItem.Foreground = Brushes.BlueViolet;


            dynaThanosMenuItem.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/DynaThanosViewExtension;component/Resources/thanosEmoji.png"))
            };

            dynaThanosMenuItem.Click += (sender, args) =>
            {
                var viewModel = new DynaThanosViewModel(p);
                var window = new DynaThanosWindow
                {
                    // Set the data context for the main grid in the window.
                    MainGrid = { DataContext = viewModel },

                    // Set the owner of the window to the Dynamo window.
                    Owner = p.DynamoWindow
                };

                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;

                // Show a modeless window.
                window.Show();
            };
            p.AddMenuItem(MenuBarType.Help, dynaThanosMenuItem,0);
            //p.dynamoMenu.Items.Add(dynaThanosMenuItem);
        }

        public void Shutdown()
        {
        }

        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString();
            }  
        } 

        public string Name
        {
            get
            {
                return "DynaThanos";
            }
        }
        //public static DynamoViewModel dynView
        //{
        //    get { return view.DataContext as DynamoViewModel; }
        //}
    }
}
