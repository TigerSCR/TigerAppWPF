using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerAppWPF
{
    class Printer : IObserver
    {
        private static Printer printer;
        public List<Title> portfolioAffichage;

        private Printer()
        {
            this.portfolioAffichage = new List<Title>();
            Engine.getEngine().registerObserver(this); 
        }

        public static Printer getPrinter()
        {
            if (printer != null)
                return printer;
            else
                printer = new Printer();
            return printer;
        }

        public void notify()
        {
        }
    }
}
