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
        private TimeSpan maturity;
        private double duration;

        public TitleNominale(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty, _message_err)
        {
            this.nominale = _nominale;
        }

        public TitleNominale(string _isin, int _qtty, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, int _nominale, string dateEmit, string maturity, double duration, int rating)
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp, rating)
        {
            this.nominale = _nominale;
            this.dateEmit = dateEmit;
            this.maturity = CalcMaturity(maturity);
            this.duration = duration;
            base.VolumeValide();
        }

        public int GetNominale
        { get { return nominale; } }

        public TimeSpan GetMaturity
        { get { return maturity; } }
        public double GetDuration
        { get { return duration; } }

        public override long Volume()
        {
            return base.Volume()*nominale;
        }

        private TimeSpan CalcMaturity(string maturity)
        {
            return Convert.ToDateTime(maturity) - Convert.ToDateTime(dateEmit);
        }

        public override string ToCSV()
        {
            return base.ToCSV() + nominale + ";" + dateEmit + ";" + maturity + ";"+duration+";";
        }

        public override string ToString()
        {
            return base.ToString()+"Nominale : "+nominale;
        }
    }
}
