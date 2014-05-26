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
        private ModuleConcentration modConc;
        private ModuleSpread modSpread;
        private ModuleProperty modProp;


        private Repartiteur()
        { }

        public static Repartiteur getEngine()
        {
            if (engine == null)
                engine = new Repartiteur();
            return engine;
        }

#region Checkers
        protected static bool inEquityModule(Title t)
        {
            return t is Equity;
        }
        protected static bool inSpreadModule(Title t)
        {
            return (t is Corp || t is Govt);
        }
        protected static bool inChangeModule(Title t)
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

        public void spread(List<Title> portfolio)
        {
            List<Title> temp = new List<Title>();
            foreach (Title t in portfolio)
            {
                if (inSpreadModule(t))
                    temp.Add(t);
            }
            this.modSpread=new ModuleSpread(temp);
        }

        public void concentration(List<Title> portfolio)
        {
            this.modConc = new ModuleConcentration(portfolio);
        }
#endregion
#region Get/Set
        public ModuleEquity ModEqu
        { get { return this.modEqu; } }
        public ModuleChange ModChange
        { get { return this.modChange; } }
        public ModuleConcentration ModConc
        { get { return this.modConc; } }
        public ModuleSpread ModSpread
        { get { return this.modSpread; } }
        public ModuleProperty ModProp
        { get { return this.modProp; } }
#endregion
    }
}
