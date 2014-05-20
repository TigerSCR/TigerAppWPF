using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class Society
    {
        //Static for singleton pattern
        private static Society society;

        //Object var
        private String name;
        private String country;
        private String currency;

        private Society(String name, String country, String currency)
        { 
            this.name=name;
            this.country=country;
            this.currency = currency;
        }

        public static Society getSociety(String name = null, String country = null, String currency=null)
        {
            if (name != null && country != null && currency!=null)
                society = new Society(name, country, currency);
            return society;
        }

        //GETSET
        public String Country
        { get { return this.country; } }
        public String Currency
        { get { return this.currency; } }
    }
}
