using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public abstract class TitleNominale : Title
    {
        private int nominale;
        private string dateEmit;
        private string maturity;
        private double duration;
        private int note;

        public TitleNominale(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty, _message_err)
        {
            this.nominale = _nominale;
        }

        public TitleNominale(string _isin, int _qtty, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, int _nominale, string dateEmit, string maturity, double duration, int note)
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp)
        {
            this.nominale = _nominale;
            this.dateEmit = dateEmit;
            this.maturity = maturity;
            this.duration = duration;
            this.note = note;
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
            return base.ToCSV() + nominale + ";" + dateEmit + ";" + maturity + ";"+duration+";"+note+";";
        }

        public override string ToString()
        {
            return base.ToString()+"Nominale : "+nominale;
        }
    }
}
