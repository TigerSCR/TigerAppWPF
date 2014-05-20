using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TigerAppWPF
{
    class Repartiteur
    {
        private static Repartiteur engine;

        //modules list
        private ModuleEquity modEqu;
        private ModuleChange modChange;


        private Repartiteur()
        { }

        public static Repartiteur getEngine()
        {
            if (engine == null)
                engine = new Repartiteur();
            return engine;
        }

#region Checkers
        private static bool inEquityModule(Title t)
        {
            return t is Equity;
        }
        private static bool inObligationModule(Title t)
        {
            return (t is Corp || t is Govt);
        }
        private static bool inChangeModule(Title t)
        {
            return t.Country != Society.getSociety().Country;
        }
#endregion
#region Repartitors
        public void equity(List<Title> portfolio)
        {
            List<Title> temp=new List<Title>();
            foreach(Title t in portfolio)
            {
                if (inEquityModule(t))
                    temp.Add(t);
            }
            this.modEqu = new ModuleEquity(temp);
        }

        public void change(List<Title> portfolio)
        {
            List<Title> temp = new List<Title>();
            foreach (Title t in portfolio)
            {
                if (inChangeModule(t))
                    temp.Add(t);
            }
            this.modChange = new ModuleChange(temp);
        }
#endregion
#region Get/Set
        public ModuleEquity ModEqu
        { get { return this.modEqu; } }
        public ModuleChange ModCha
        { get { return this.modChange; } }
#endregion
    }
}
