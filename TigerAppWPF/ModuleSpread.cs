﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TigerAppWPF
{
    class ModuleSpread : Module
    {
#region Static DATA
        double [,] nonEEA_A=new double[,]
        {{0.00,0.00,0.0110,0.0140,0.0250,0.0450,0.0450,0.0450},
        {0.00,0.00,0.0058,0.0070,0.0150,0.0251,0.0251,0.0251},
        {0.00,0.00,0.0050,0.0050,0.0100,0.0180,0.0180,0.0180},
        {0.00,0.00,0.0050,0.0050,0.0100,0.0050,0.0050,0.0050},
        {0.00,0.00,0.0050,0.0050,0.0050,0.0050,0.0050,0.0050}};
        double[,] nonEEA_B = new double[,]
        {{0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000},
        {0.0000,0.0000,0.0550,0.0700,0.1250,0.2250,0.2250,0.2250},
        {0.0000,0.0000,0.0840,0.1050,0.2000,0.3505,0.3505,0.3505},
        {0.0000,0.0000,0.1090,0.1300,0.2500,0.4405,0.4405,0.4405},
        {0.0000,0.0000,0.1340,0.1550,0.3000,0.4655,0.4655,0.4655}};

        double[,] corp_A = new double[,]
        {{0.0090,0.0110,0.0140,0.0250,0.0450,0.0750,0.0750,0.0300},
        {0.0053,0.0058,0.0070,0.0150,0.0251,0.0420,0.0420,0.0168},
        {0.0050,0.0050,0.0050,0.0100,0.0180,0.0050,0.0050,0.0116},
        {0.0050,0.0050,0.0050,0.0100,0.0050,0.0050,0.0050,0.0116},
        {0.0050,0.0050,0.0050,0.0050,0.0050,0.0050,0.0050,0.0050}};
        double[,] corp_B = new double[,]
        {{0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000},
        {0.0450,0.0550,0.0700,0.1250,0.2250,0.3750,0.3750,0.1500},
        {0.0715,0.0840,0.1050,0.2000,0.3505,0.5850,0.5850,0.2340},
        {0.0965,0.1090,0.1300,0.2500,0.4405,0.6100,0.6100,0.2920},
        {0.1215,0.1340,0.1550,0.3000,0.4655,0.6350,0.6350,0.3500}};

        double[,] covered_A = new double[,]
        {{0.0070,0.0090,0.0140,0.0250,0.0450,0.0750,0.0750,0.0300},
        {0.0050,0.0050,0.0070,0.0150,0.0251,0.0420,0.0420,0.0168},
        {0.0050,0.0050,0.0050,0.0100,0.0180,0.0050,0.0050,0.0116},
        {0.0050,0.0050,0.0050,0.0100,0.0050,0.0050,0.0050,0.0116},
        {0.0050,0.0050,0.0050,0.0050,0.0050,0.0050,0.0050,0.0050}};
        double[,] covered_B = new double[,]
        {{0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000},
        {0.0350,0.0450,0.0700,0.1250,0.2250,0.3750,0.3750,0.1500},
        {0.0715,0.0840,0.1050,0.2000,0.3505,0.5850,0.5850,0.2340},
        {0.0965,0.1090,0.1300,0.2500,0.4405,0.6100,0.6100,0.2920},
        {0.1215,0.1340,0.1550,0.3000,0.4655,0.6350,0.6350,0.3500}};
#endregion



        public ModuleSpread(List<Title> source)
            : base(source)
        { }

        //montant*(a+b*(z)) avec z=duration-c
        protected override void calculate(List<Title> source)
        {
            //formule temp en attendant les accesseurs
            foreach (Title t0 in source)
            {
                int x, y;
                double a, b,c,z;
                TitleNominale t=t0 as TitleNominale;

                x = ((int)t.GetMaturity.TotalDays%365 )/ 5;
                c = x * 5;
                y = t.GetRating;
                z = t.GetDuration - c;
                /*inutile désormais on dit directement dans title que pas de rating = 7
                 * if(y==null)
                    y=7;*/

                if (t is Corp)
                {
                    a=corp_A[x,y];
                    b=corp_B[x,y];

                    results.Add(t,t.Total*a*b*z);
                }
                else if (t is Govt && DataConfig.getDataConfig().ListUE.Contains(t.Country))
                {
                    a=covered_A[x,y];
                    b=covered_B[x,y];

                    results.Add(t,t.Total*a*b*z);
                }
                else
                {
                    a=nonEEA_A[x,y];
                    b=nonEEA_B[x,y];

                    results.Add(t,t.Total*a*b*z);
                }
            }
        }
    }
}
