using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public class Corp : Title
    {
        private string dateEmit;
        private string dateBack;
        private bool is_covered;

        public Corp(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty,_message_err)
        {

        }

        public Corp(string _isin, int _qtty, int _nominale, string country, string currency, string name, double value, string dateEmit, string dateBack, bool is_covered)
            : base(_isin, _qtty, country, currency, name, value)
        {
            this.dateEmit = dateEmit;
            this.dateBack = dateBack;
            this.is_covered = is_covered;
        }

        public override string ToString()
        {
            return base.ToString() + " DateEmit : " + dateEmit + " DateBack: " + dateBack+" COVB"+is_covered;
        }

        override public string ToCSV()
        {
            if (this.IsValide)
                return "Corp;" + base.ToCSV() + ";" + dateEmit + ";" + dateBack+";"+is_covered;
            else return "Corp;" + base.ToCSV();
        }
    }
}
