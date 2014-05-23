using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
        public class Govt : TitleNominale
        {
            public Govt(string _isin, int _qtty, int _nominale, string _message_err)
                : base(_isin, _qtty, _nominale, _message_err)
            { }

            public Govt(string _isin, int _qtty, int _nominale, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, string dateEmit, string maturity, double duration, int note)
                : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp, _nominale, dateEmit, maturity, duration, note)
            { }

            override public string ToCSV()
            {
                return "Govt;" + base.ToCSV();
            }

            /*public Govt(Title _t)
                : base(_t)
            {}*/
        }
}
