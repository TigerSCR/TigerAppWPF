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

        public Corp(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty,_message_err)
        {

        }

        public Corp(string _isin, int _qtty, int _nominale, string country, string currency, string name, double value, string dateEmit, string dateBack)
            : base(_isin, _qtty, country, currency, name, value)
        {
            this.dateEmit = dateEmit;
            this.dateBack = dateBack;
        }

        public override string ToString()
        {
            return base.ToString() + " DateEmit : " + dateEmit + " DateBack: " + dateBack;
        }

        override public string ToCSV()
        {
            if (this.IsValide)
                return "Corp;" + base.ToCSV() + ";" + dateEmit + ";" + dateBack;
            else return "Corp;" + base.ToCSV();
        }
    }
}
