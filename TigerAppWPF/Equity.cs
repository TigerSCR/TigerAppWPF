using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public class Equity : Title
    {
        private bool strategic = false;
        public Equity(string _isin, int _qtty, string _message_err)
            : base(_isin, _qtty, _message_err)
        {}

        public Equity(string _isin, int _qtty, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, int rating)
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp, rating)
        {}

        override public string ToCSV()
        {
            return "Equity;" +base.ToCSV();
        }

        public bool Strategic
        {
            get { return strategic; }
            set { strategic = value; }
        }


        /*public Equity(Title _t)
            : base(_t)
        {}*/
    }
}
