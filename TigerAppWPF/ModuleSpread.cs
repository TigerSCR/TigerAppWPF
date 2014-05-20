using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleSpread : Module
    {
        public ModuleSpread(List<Title> source)
            : base(source)
        { }

        protected override void calculate(List<Title> source)
        {
            base.calculate(source);
        }
    }
}
