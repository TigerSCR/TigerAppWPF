using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public abstract class TitleNominale : Title
    {
        private int nominale;

        public TitleNominale(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty, _message_err)
        {
            this.nominale = _nominale;
        }

        public TitleNominale(string _isin, int _qtty, string country, string currency, string name, double value, int _nominale)
            : base(_isin, _qtty, country, currency, name, value)
        {
            this.nominale = _nominale;
            base.VolumeValide();
        }

        public int GetNominale
        { get { return nominale; } }

        public override long Volume()
        {
            return base.Volume()*nominale;
        }

        public override string ToCSV()
        {
            return base.ToCSV() +";" + nominale;
        }

        public override string ToString()
        {
            return base.ToString()+"Nominale : "+nominale;
        }
    }
}
