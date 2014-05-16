﻿using System;
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
            for(int i=0;i<source.Count;i++)
            {
                    if (!source.ElementAt(i).Strategic)
                    {
                        if (source.ElementAt(i).Oecd || source.ElementAt(i).Eu)
                            results.Add(source.ElementAt(i),source.ElementAt(i).Total * (chocEquity + symAdjust));
                        else
                            results.Add(source.ElementAt(i),source.ElementAt(i).Total * (chocOtherEquity + symAdjust));
                    }
                    else
                        results.Add(source.ElementAt(i),source.ElementAt(i).Total * strategic);
            }
        }
    }
}