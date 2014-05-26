using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleConcentration : Module
    {
        //static data
        private double [] ct=new double[7] {0.03,0.03,0.03,0.015,0.015,0.015,0.015} ;
        private double [] gi=new double[7] {0.12,0.12,0.21,0.27,0.73,0.73,0.73};


        //runtime data
        Dictionary<int, List<Title>> listByCorporate;
                            //somme,rating,xsi,conci
        Dictionary<int, Tuple<double,int,double,double>> totalByCorporate;
        private double result;
        private int total;

        public ModuleConcentration(List<Title> source)
            : base(source)
        {
            listByCorporate = new Dictionary<int, List<Title>>();
            totalByCorporate = new Dictionary<int, Tuple<double, int, double, double>>();
            result = 0;
            total=source.Count;
        }
        
        protected override void calculate(List<Title> source)
        {
            //répartition par parent
            foreach (Title t in source)
            {
                if (listByCorporate.Keys.Contains(t.GetID_Corp))
                    listByCorporate[t.GetID_Corp].Add(t);
                else
                    listByCorporate.Add(t.GetID_Corp, new List<Title>() {t});
            }
            /*supression des titres uniques DELETED BECAUSE WE DON'T KNOW AND LINA THINKS NO SUPPRESION
            foreach(var v in listByCorporate)
            {
                if(v.Value.Count==1)
                {
                    listByCorporate.Remove(v.Key);
                    total--;
                }
            }*/

            //somme des valeurs
            foreach(var v in listByCorporate)
            {
                double somme=0;
                double rating=0;
                foreach(Title t in v.Value)
                {
                    somme+=t.Total;
                    rating+=t.GetRating*t.Total;
                }
                rating/=somme;
                double xsi =  Math.Max(0, (somme / total) - ct[(int)rating]);
                double conci=xsi*gi[(int)rating]*somme;
                totalByCorporate.Add(v.Key, new Tuple<double, int, double, double>(somme, (int)rating,xsi,conci));
            }

            //calcul final
            foreach (var v in totalByCorporate)
            {
                result += v.Value.Item4*v.Value.Item4;//somme les conci au carré
            }
            // prends la racine
            result = Math.Sqrt(result);
        }

        public double Result
        { get { return this.result; } }
    }
}
