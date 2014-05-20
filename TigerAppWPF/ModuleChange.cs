using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleChange : Module
    {
        //Static data
        String[] exception = new String[] { "BKK", "BGN", "LVL", "LTL" };
        public ModuleChange(List<Title> source)
            : base(source)
        { }

        protected override void calculate(List<Title> source)
        {
            foreach (Title t in source)
            {
                double taux = 0;
                if (exception.Contains(t.Currency))
                    taux += handleException(t);
                results.Add(t, t.Total * taux);
            }
        }

        private double handleException(Title t)
        {
            double r = 0;
            switch (Society.getSociety().Country)
            {
                case "DKK":
                    switch (t.Currency)
                    {
                        case "EUR":
                            r = 0.0239;
                            break;
                        case "LVL":
                            r = 0.0266;
                            break;
                        case "BGN":
                            r = 0.0345;
                            break;
                    }
                    break;
                case "LVL":
                    switch (t.Currency)
                    {
                        case "EUR":
                            r = 0.0264;
                            break;
                        case "LTL":
                            r = 0.0291;
                            break;
                        case "BGN":
                            r = 0.0370;
                            break;
                    }
                    break;
                case "LTL":
                    switch (t.Currency)
                    {
                        case "EUR":
                            r = 0.026;
                            break;
                        case "BGN":
                            r = 0.013;
                            break;
                    }
                    break;
                case "BGN":
                    if (t.Currency == "EUR")
                        r = 0.0104;
                    break;
            }
            return r;
        }
    }
}
