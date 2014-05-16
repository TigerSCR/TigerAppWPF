﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    public abstract class Title
    {
        private string isin;
        private int qtty;
        private string country;
        private string name;
        private double value;
        private double total;
        private string currency;
        private bool oecd = false;
        private bool eu= false;
        private bool strategic = false;
        private bool isvalide = true;
        private DataConfig config;
        private string message_err;

        public Title(string _isin, int _qtty, string _message_err)
        {
            this.isin = _isin;
            this.qtty = _qtty;
            this.MessageErreur = _message_err;
            this.isvalide = false;
        }

        public Title(string _isin, int _qtty, string country, string currency, string name, double value)
        {
            this.isin = _isin;
            this.qtty = _qtty;
            this.country = country;
            this.currency = currency;
            this.name = name;
            this.value = value;

            config = DataConfig.getDataConfig();
            if (config.ListOCDE.Contains(this.country))
                this.oecd = true;
            if (config.ListUE.Contains(this.country))
                this.eu = true;

            this.total = this.value * this.qtty;
        }

        #region Accesseurs
        public string Isin
        { get{return this.isin;} }
        public int Qtty
        {
            get { return this.qtty; }
            set { this.qtty = value; }
        }
        virtual public long Volume()
        { return qtty; }
        public bool IsValide
        {
            get { return isvalide; }
            set { isvalide = value; }
        }
        public double Value
        { get { return this.value; } }    
        public bool Oecd
        { get { return this.oecd; } }
        public bool Eu
        { get { return this.eu; } }
        public bool Strategic
        { get { return this.strategic; } }
        public string Name
        { get { return this.name; } }
        public string Country
        { get { return this.country; } }
        public string Currency
        { get { return this.currency; } }
        public double Total
        { get { return this.total; } }
        public string MessageErreur
        { get { return message_err;}
          set { if (message_err == null)
                    message_err = value;
                else
                    message_err+= " et "+value; 
          }
        }

        #endregion

        protected void VolumeValide()
        {
            if (Volume() > 5000000)
            {
                isvalide = false;
                this.MessageErreur = "Volume probablement trop grand";
            }
        }
        override public string ToString()
        {
            string s = isin + " : Pays : " + country + " Nom : " + name + " = "+qtty+ "(" + value+" "+ currency+")";
            if(message_err != null)
                s+="Erreur : "+message_err;
            return s;
        }

        virtual public string ToCSV()
        {
            if (isvalide)
                return isin + ";" + qtty + ";" + country + ";" + currency + ";" + name + ";" + value;
            else
                return isin + ";" + qtty+";"+message_err;
        }
    }
}
