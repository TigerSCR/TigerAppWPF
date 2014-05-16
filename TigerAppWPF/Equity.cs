﻿using System;
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

        public Equity(string _isin, int _qtty, string country, string currency, string name, double value)
            : base(_isin, _qtty, country, currency, name, value)
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
