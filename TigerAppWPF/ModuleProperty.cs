using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleProperty : Module
    {
        //SOLID DATA
        private const double choc = 0.25;

        public ModuleProperty(List<Title> source)
            : base(source)
        {
            this.calculate(source);
        }

        protected override void calculate(List<Title> source)
        {
            foreach (Title t in source)
            {
                this.results.Add(t, t.Value * choc);
            }
        }
    }
}
