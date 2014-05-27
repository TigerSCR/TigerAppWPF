using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public abstract class TitleNominale : Title
    {
        private int nominale;
        private TimeSpan maturity;
        private double duration;
        private double cpn;
        private int cpn_freq;
        private string date_cpn;
        private int nb_day_nxt_cpn;


        public TitleNominale(string _isin, int _qtty, int _nominale, string _message_err)
            : base(_isin, _qtty, _message_err)
        {
            this.nominale = _nominale;
        }

        public TitleNominale(string _isin, int _qtty, string country, string currency, string name, double value, int _id_Mcorp, string _name_Mcorp, int rating, int _nominale, string date_cpn, string maturity, double duration,
                             double cpn, int cpn_freq, int nb_day_nxt_cpn)
            : base(_isin, _qtty, country, currency, name, value, _id_Mcorp, _name_Mcorp, rating)
        {
            this.nominale = _nominale;
            this.date_cpn = date_cpn;
            this.maturity = CalcMaturity(maturity);
            this.duration = duration;
            this.cpn = cpn;
            this.cpn_freq = cpn_freq;
            this.nb_day_nxt_cpn = nb_day_nxt_cpn;
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
            return Convert.ToDateTime(maturity) - Convert.ToDateTime(date_cpn);
        }

        public override string ToCSV()
        {
            return base.ToCSV() + nominale + ";" + date_cpn + ";" + maturity + ";"+duration+";";
        }

        public override string ToString()
        {
            return base.ToString()+"Nominale : "+nominale;
        }
    }
}
