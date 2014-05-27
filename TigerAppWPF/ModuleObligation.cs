using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleObligation : Module
    {
        protected Dictionary<Title, double> results;

        public ModuleObligation(List<Title> source)
            :base(source)
        {
            results = new Dictionary<Title, double>();
        }

        protected override void calculate(List<Title> source)
        {
            base.calculate(source);
        }
    }
}
