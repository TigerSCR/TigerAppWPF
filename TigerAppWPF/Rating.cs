using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public class Rating
    {
        Dictionary<string, int> d_rating = new Dictionary<string,int>();
        

        private Rating()
        {
        }

        public int CalcQuality(string rt_moody, string rt_fitch, string rt_sp)
        {

        }

        private int Fitch_Moody(string rt_fitch)
        {
            if (rt_fitch.Contains('A'))
            {
                if (rt_fitch.Contains("AA"))
                    return 1;
                else return 2;
            }
            else if (rt_fitch.Contains('B'))
            {
                if (rt_fitch.Contains("BBB"))
                    return 3;
                else if (rt_fitch.Contains("BB"))
                    return 4;
                else return 5;
            }
            else return 6;
        }

        private int Moody(string rt_moody)
        {
            if (rt_moody[0].Equals('A'))
            {
                if (rt_moody.Contains("Aa"))
                    return 1;
                else return 2;
            }
            else if (rt_moody.Contains('B'))
            {
                if (rt_moody.Contains("BBB"))
                    return 3;
                else if (rt_moody.Contains("BB"))
                    return 4;
                else return 5;
            }
            else return 6;
        }
    }
}
