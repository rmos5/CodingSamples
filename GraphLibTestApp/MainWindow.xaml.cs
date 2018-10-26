using Graphs;
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

namespace GraphLibTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                DataContext = new MainContext();
            }
        }

        private void GenerateGraphButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext.GenerateGraph();
            }
            catch (Exception ex)
            {
                MessageBoxes.ShowErrorMessage(this, ErrorStringBuilder.BuildErrorString("Failed to generate graph.", ex));
            }

        }

        private void TraverseDFSButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext.TraverseDFS();
            }
            catch (Exception ex)
            {
                MessageBoxes.ShowErrorMessage(this, ErrorStringBuilder.BuildErrorString("Failed to traverse DFS.", ex));
            }
        }

        private void TraverseBFSButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext.TraverseBFS();
            }
            catch (Exception ex)
            {
                MessageBoxes.ShowErrorMessage(this, ErrorStringBuilder.BuildErrorString("Failed to traverse BFS.", ex));
            }
        }

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext.Reset();
            }
            catch (Exception ex)
            {
                MessageBoxes.ShowErrorMessage(this, ErrorStringBuilder.BuildErrorString("Failed to reset node.", ex));
            }
        }


        private void PathsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext.GetPaths();
            }
            catch (Exception ex)
            {
                MessageBoxes.ShowErrorMessage(this, ErrorStringBuilder.BuildErrorString("Failed to retrieve paths.", ex));
            }
        }

        private void MainGraphView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext != null)
                DataContext.SelectedNode = MainGraphView.SelectedItem as TraverseNode;
        }

        public new MainContext DataContext
        {
            get { return base.DataContext as MainContext; }
            set { base.DataContext = value as MainContext; }
        }

    }
}
