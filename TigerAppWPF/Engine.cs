using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

using System.Windows;
namespace TigerAppWPF
{
    class Engine : ISubject
    {
        //Static var for Singleton pattern
        private static Engine engine;
        private Excel.Worksheet activeWorksheet;


        //Runtime var
        private List<Title> portfolio;
        private List<Tuple<string,int,string>> isins=new List<Tuple<string,int,string>>();

        private Engine()
        {
            this.portfolio = new List<Title>();
            this.portfolio.Add(new Equity("sdfsdf", 5000));
        }

        public static Engine getEngine(Excel.Worksheet ws=null)
        {
            if (engine == null)
                engine = new Engine();
            if (ws != null)
                engine.activeWorksheet = ws;
            return engine;
        }

        public void setIsins(List<Tuple<string, int>> temp)
        {
            foreach(var i in temp)
            {
                if (i.Item1 == null /*|| i.Item2 == null*/)
                {
                    ArgumentException except = new ArgumentException();
                    throw except;
                }
                this.isins.Add(new Tuple<string,int,string>(i.Item1, i.Item2,"test"));
            }
            this.getTitle();
        }

        public void getTitle()
        {
            this.portfolio[0].Qtty = 60;
            this.portfolio = Connector.getConnector().getInfo(this.isins);
<<<<<<< HEAD
            MessageBox.Show("updated");
=======
            this.notifyObservers();
>>>>>>> parent of dda3a32... Removal of observable pattern
        }

        public void calculate()
        {
            Repartiteur.getEngine().equity(this.portfolio);
        }

        public override string ToString()
        {
            string result="";
            result += "EQUITIES\n";
            foreach (Equity eq in portfolio)
            {
                result += eq.ToString() + "\n";
            }
            return result;
        }
    
        //Implementing ISubject
        private List<IObserver> observerCollection=new List<IObserver>();
        public void registerObserver(IObserver observer)
        {
            this.observerCollection.Add(observer);
        }
        public void unregisterObserver(IObserver observer)
        {
            this.observerCollection.Remove(observer);
        }
        public void notifyObservers()
        {
            foreach (IObserver observer in this.observerCollection)
            {
                observer.notify();
            }
        }

        public List<Title> Portfolio
        { get { return this.portfolio; } }
    }
}
