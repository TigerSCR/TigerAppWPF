using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleEquity : Module
    {

        //DATA
        private const double chocEquity = 0.39;
        private const double chocOtherEquity = 0.49;
        private const double symAdjust = -0.07;
        private const double strategic = 0.22;
        private const double corr = 0.75;

        //runtime
        private double sommeT1;
        private double sommeT2;
        private double result;

        public ModuleEquity(List<Title> source)
            : base(source)
        {
            sommeT1 = 0;
            sommeT2 = 0;
            result = 0;
        }

        protected override void calculate(List<Title> source)
        {
            //calcul pour chaque titre
            foreach(Title t in source)
            {
                    if (!t.Strategic)
                    {
                        if (t.Oecd || t.Eu)
                        {
                            results.Add(t,t.Total * (chocEquity + symAdjust));
                            sommeT1+=results.Last().Value;
                        }
                        else
                        {
                            results.Add(t,t.Total * (chocOtherEquity + symAdjust));
                            sommeT2+=results.Last().Value;
                        }
                    }
                    else
                        results.Add(t,t.Total * strategic);
            }

            //calcul du scr du module avec la corrélation type 1 vs type 2 "corr"
            foreach (var r in results)
            {
                result = Math.Sqrt(sommeT1 * sommeT1 + sommeT1 * corr * sommeT2 + sommeT2 * sommeT2);
            }
        }

        public double Result
        { get { return this.result; } }
    }
}