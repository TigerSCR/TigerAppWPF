using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
namespace TigerAppWPF
{
    class Engine : ISubject
    {
        //Static var for Singleton pattern
        private static Engine engine;


        //Runtime var
        private List<Title> portfolio;
        private List<Tuple<string,int,int>> isins=new List<Tuple<string,int,int>>();

        private Engine()
        {
            this.portfolio = new List<Title>();
        }

        public static Engine getEngine()
        {
            if (engine == null)
                engine = new Engine();
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
                this.isins.Add(new Tuple<string,int,int>(i.Item1, i.Item2, 1));
            }
            this.getTitle();
        }

        public void getTitle()
        {
            this.portfolio = Connector.getConnector().getInfo(this.isins);
            this.notifyObservers();
        }

        public void calculateEquity()
        {
            Repartiteur.getEngine().equity(this.portfolio);
            this.notifyObservers();
        }

        public void calculateAll()
        {
            Repartiteur.getEngine().equity(this.portfolio);
            Repartiteur.getEngine().change(this.portfolio);
            Repartiteur.getEngine().spread(this.portfolio);
            Repartiteur.getEngine().concentration(this.portfolio);
            this.notifyObservers();
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
        { get { return Engine.getEngine().portfolio; } }
    }
}
