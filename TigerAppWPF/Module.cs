﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class Module
    {
        protected Dictionary<Title, double> results;
        public Module(List<Title> source)
        {
            results=new Dictionary<Title,double>();
        }
        protected virtual void calculate(List<Title> source)
        {}
        public override string ToString()
        {
            string reponse = "" ;
            foreach (var o in results)
            {
                reponse += o.Key.ToString() +" SCR = "+ o.Value + "\n";
            }
            return reponse;
        }

        public Dictionary<Title, double> Results
        { get { return this.results; } }
    }
}
