using System.Windows;

namespace Dtail_EasyCollector_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.GarmentsList.Text);
        }

        private void RadioBtn_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
