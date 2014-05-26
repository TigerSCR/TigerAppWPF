using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bloomberglp.Blpapi;

namespace TigerAppWPF
{
    static public class Rating
    {

        static public Request AddRating(Request request)
        {
            request.Append("fields", "RTG_MOODY");
            request.Append("fields", "RTG_FITCH");
            request.Append("fields", "RTG_SP_LT_LC_ISSUER_CREDIT");
            return request;
        }

        static public int GetQuality(Element fieldData)
        {
            string rt_moody="", rt_fitch="", rt_sp="";
            if (fieldData.HasElement("RTG_MOODY"))
                rt_moody = fieldData.GetElementAsString("RTG_MOODY");
            if (fieldData.HasElement("RTG_FITCH"))
                rt_fitch = fieldData.GetElementAsString("RTG_FITCH");
            if (fieldData.HasElement("RTG_SP_LT_LC_ISSUER_CREDIT"))
                rt_sp = fieldData.GetElementAsString("RTG_SP_LT_LC_ISSUER_CREDIT");

            List<int> liste_note = new List<int>();
            liste_note.Add(Fitch_SP(rt_fitch));
            liste_note.Add(Fitch_SP(rt_sp));
            liste_note.Add(Moody(rt_moody));

            int nb_null = 0;
            liste_note.Sort();
            foreach (int x in liste_note)
            {
                if (x == 7)
                    nb_null++;
            }

            if (nb_null >= 2)
                return liste_note.ElementAt(0);
            else
                return liste_note.ElementAt(1);
        }

        static private int Fitch_SP(string rt_fitch)
        {
            if (rt_fitch == "")
                return 7;

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

        static private int Moody(string rt_moody)
        {
            if (rt_moody == "")
                return 7;

            if (rt_moody[0].Equals('A'))
            {
                if (rt_moody.Contains("Aa"))
                    return 1;
                else return 2;
            }
            else if (rt_moody.Contains('B'))
            {
                if (rt_moody.Contains("Baa"))
                    return 3;
                else if (rt_moody.Contains("Ba"))
                    return 4;
                else return 5;
            }
            else return 6;
        }
    }
}
