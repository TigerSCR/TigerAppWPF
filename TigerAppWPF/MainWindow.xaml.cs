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
using System.Threading;
using System.ComponentModel;

using System.IO;

namespace TigerAppWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver
    {
        private bool initialized = false;//definit si le portofolio existe pour éviter l'overrun du binding
        public BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            Engine.getEngine().registerObserver(this);
            Society.getSociety("Nom", "FR", "EUR");
            /*Assistant ast=new Assistant();
            ast.ShowDialog();*/
            
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog temp = new Microsoft.Win32.OpenFileDialog();
            Stream myStream = null;

            List<Tuple<string, int>> resultat = new List<Tuple<string, int>>();

            temp.Filter = "CSV files (*.csv)|*.csv";
            Nullable<bool> result = temp.ShowDialog();
            if (result == true)
            {
                try
                {
                    if ((myStream = temp.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader sr = new StreamReader(myStream);
                            String s = sr.ReadLine();
                            String[] temps;
                            while (s != null)
                            {
                                temps = s.Split(';');
                                resultat.Add(new Tuple<string, int>(temps[0], int.Parse(temps[1])));
                                s = sr.ReadLine();
                            }
                            sr.Close();
                        }
                    }
                    myStream.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace + "\n" + ex.HelpLink);
                }
                worker.RunWorkerAsync(resultat);
            }
                
        }

        #region BackgroundWorker ProgressBar
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Tuple<string, int>> resultat = e.Argument as List<Tuple<string, int>>;
            Engine.getEngine().setIsins(resultat, worker);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            notify();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar.Value = e.ProgressPercentage;
        }
        #endregion

        private void Outils_Calculer_Equity_Click(object sender, RoutedEventArgs e)
        {
            Engine.getEngine().calculateEquity();
        }

        private void Outils_Calculer_All_Click(object sender, RoutedEventArgs e)
        {
            Engine.getEngine().calculateAll();
        }

        public void notify()
        {
            if (portfolio.ItemsSource == null)
                portfolio.Items.Clear();
            if (equitymodule.ItemsSource == null)
                equitymodule.Items.Clear();

            if (!initialized)
            {
                portfolio.ItemsSource = Engine.getEngine().Portfolio;
            }
            else
            {
                equitymodule.ItemsSource = Repartiteur.getEngine().ModEqu.Results;
            }
            initialized = true;
        }

        private void Fermeture_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rapport_Click(object sender, RoutedEventArgs e)
        {
            RapportView rv = new RapportView();
            rv.Show();
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
