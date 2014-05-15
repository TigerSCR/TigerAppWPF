using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TigerAppWPF
{
    /// <summary>
    /// Logique d'interaction pour Assistant.xaml
    /// </summary>
    public partial class Assistant : Window
    {
        public Assistant()
        {
            InitializeComponent();
        }

        private void creerProfil_Click(object sender, RoutedEventArgs e)
        {
            creationProfil cp = new creationProfil();
            cp.ShowDialog();
        }
    }
}
