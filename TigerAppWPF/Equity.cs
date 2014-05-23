using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public class Equity : Title
    {
        public Equity(string _isin, int _qtty, string _message_err)
            : base(_isin, _qtty, _message_err)
        {}

        public Equity(string _isin, int _qtty, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp)
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp)
        {}

        override public string ToCSV()
        {
            return "Equity;" +base.ToCSV();
        }

        /*public Equity(Title _t)
            : base(_t)
        {}*/
    }
}
