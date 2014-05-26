using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public class Corp : TitleNominale
    {
        private bool is_covered;

        public Corp(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty, _nominale, _message_err)
        {

        }

        public Corp(string _isin, int _qtty, int _nominale, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, string dateEmit, string maturity, double duration, int rating, bool is_covered )
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp, _nominale, dateEmit, maturity, duration, rating)
        {
            this.is_covered = is_covered;
        }

        public override string ToString()
        {
            return base.ToString()+" COVB "+is_covered;
        }

        override public string ToCSV()
        {
            if (this.IsValide)
                return "Corp;" + base.ToCSV() +is_covered;
            else return "Corp;" + base.ToCSV();
        }
    }
}
