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

        public ModuleEquity(List<Title> source)
            : base(source)
        {
        }

        protected override void calculate(List<Title> source)
        {
            //formula
            foreach(Title t in source)
            {
                    if (!t.Strategic)
                    {
                        if (t.Oecd || t.Eu)
                            results.Add(t,t.Total * (chocEquity + symAdjust));
                        else
                            results.Add(t,t.Total * (chocOtherEquity + symAdjust));
                    }
                    else
                        results.Add(t,t.Total * strategic);
            }
        }
    }
}