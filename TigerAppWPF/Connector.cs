using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bloomberglp.Blpapi;
using System.IO;
//using BEmu;

namespace TigerAppWPF
{
    class Connector
    {
        private static Connector _instance;
        static readonly object instanceLock = new object();

        static private Session session;
        private Service refDataSvc;
        private SessionOptions sessionOptions;
        static private Request request;
        static private List<Title> l_title;
        static Dictionary<string,Tuple<int, int, string>> d_title;

        static private bool isGetType;
        static private bool isGetCurve;
        static private CourbeSwap curve;

        private Connector()
        {
            OpenSession();
        }

        /// <summary>
        /// Pattern singleton : connecteur unique
        /// </summary>
        /// <returns>connecteur avec liaison BL étblie</returns>
        public static Connector getConnector()
        {
            lock (instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new Connector();
                }
            }
            return _instance;
        }
        

        /// <summary>
        /// Etablie la connection avec Bloomberg
        /// </summary>
        private void OpenSession()
        {
            sessionOptions = new SessionOptions();
            sessionOptions.ServerHost = "localhost";
            sessionOptions.ServerPort = 8194;
            session = new Session(sessionOptions);
            l_title = new List<Title>();
            if (!session.Start())
            {
                System.Console.WriteLine("Could not start session.");
                Console.WriteLine(session.ToString());
                Remplissage_Non_connection();
                return;
            }
            if (!session.OpenService("//blp/refdata"))
            {
                System.Console.WriteLine("Could not open service " +
                "//blp/refdata");
                return;
            }
            refDataSvc = session.GetService("//blp/refdata");
            if (refDataSvc == null)
            {
                Console.WriteLine("Cannot get service");
                return;
            }
            else
            {
                request = refDataSvc.CreateRequest("ReferenceDataRequest");
            }
        }

        /// <summary>
        /// Recupère les info specifiques des differents ISIN
        /// </summary>
        /// <param name="d_title">Isin avec leurs quantité</param>
        /// <returns>la liste de Titre avec les informations remplit</returns>
        public List<Title> getInfo(List<Tuple<string, int, int>> _d_title)
        {
                if (l_title.Count != 0)
                    return l_title;
                
                d_title = new Dictionary<string, Tuple<int, int, string>>();

                for (int i = 0; i < _d_title.Count; i += 30)
                {
                    for (int j = i; j < i + 30; j++) //gere le nombre limité de champs que l'on peut envoyer dans une seule requete
                    {
                        if (j >= _d_title.Count)
                            break;
                        var tuple = _d_title.ElementAt(j);
                        if (!tuple.Item1.Contains("Immo"))
                            d_title.Add(tuple.Item1, new Tuple<int, int, string>(tuple.Item2, tuple.Item3, ""));
                        else
                            d_title.Add(tuple.Item1, new Tuple<int, int, string>(tuple.Item2, tuple.Item3, ""));
                    }

                    foreach (var title in d_title)
                    {
                        request.Append("securities", "/isin/" + title.Key);
                        request.Append("fields", "MARKET_SECTOR_DES");
                    }

                    isGetType = true;
                    ResponseLoop(); // Recupère les secteurs de marché

                    isGetType = false;
                    ResponseLoop(); // Recupère les actions propres au secteur 

                    d_title.Clear();
                }

            return l_title;
        }

        public CourbeSwap GetCurve(string _isin)
        {
            if (curve == null || curve.isin != _isin)
            {
                curve = new CourbeSwap("EuroSwap", _isin, "BLC2 Curncy");// isin euroswap : "S0045Z"
                curve.SetRequest(request);
                isGetCurve = true;
                ResponseLoop();
                isGetCurve = false;
            }
            return curve;
        }

        /// <summary>
        /// Remplit la liste de titre par des valeurs par defaut quand la connection à Bl à echoué
        /// </summary>
        static public void Remplissage_Non_connection()
        {
            Console.WriteLine("Mode non connection, valeurs invalides");
            ReadCSV();
            //curve.ReadCSV();
        }

        /// <summary>
        /// Envoi les requêtes PREPARES à l'avance, et recupère les infos.
        /// </summary>
        private void ResponseLoop()
        {
            session.SendRequest(request, new CorrelationID(1)); ;
            request = refDataSvc.CreateRequest("ReferenceDataRequest"); //Remet à zero la request qui sera rempli après.
            bool continueToLoop = true;
            while (continueToLoop)
            {
                Event eventObj = session.NextEvent();
                switch (eventObj.Type)
                {
                    case Event.EventType.RESPONSE: // final event
                        continueToLoop = false;
                        handleResponseEvent(eventObj);
                        break;
                    case Event.EventType.PARTIAL_RESPONSE:
                        handleResponseEvent(eventObj);
                        break;
                    default:
                        handleOtherEvent(eventObj);
                        break;
                }
            }
        }

        /// <summary>
        /// Filtre la reponse pour les stocker dans des objets de types TITLE
        /// </summary>
        /// <param name="eventObj"></param>
        private static void handleResponseEvent(Event eventObj)
        {
            string marketSector;
            foreach (Message message in eventObj.GetMessages())
            {
                Element ReferenceDataResponse = message.AsElement;
                if (ReferenceDataResponse.HasElement("responseError"))
                {
                    //throw new Exception("responseError " + ReferenceDataResponse.ToString());
                    Console.WriteLine("Mode non connecté");
                    Remplissage_Non_connection();
                }

                Element securityDataArray = ReferenceDataResponse.GetElement("securityData");
                //Console.WriteLine(message.ToString());

                int numItems = securityDataArray.NumValues;
                for (int i = 0; i < numItems; ++i)
                {
                    Element securityData = securityDataArray.GetValueAsElement(i);
                    string security = securityData.GetElementAsString("security");
                    if(!isGetCurve)
                        security = security.Replace("/isin/", "");

                    //int sequenceNumber = securityData.GetElementAsInt32("sequenceNumber");
                    if (securityData.HasElement("securityError"))
                    {
                        Element securityError = securityData.GetElement("securityError");
                        throw new Exception("securityError : "+security+" invalide");
                    }

                    else
                    {
                        Element fieldData = securityData.GetElement("fieldData");

                        if (isGetCurve)
                        {
                            curve.ParseEquity(fieldData);
                        }

                        else
                        {

                            if (isGetType)
                                marketSector = fieldData.GetElementAsString("MARKET_SECTOR_DES");
                            else
                                marketSector = d_title[security].Item3;

                            switch (marketSector)
                            {
                                case "Equity":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                case "Corp":
                                    if (isGetType)
                                        RequestCorp(security);
                                    else
                                        ParseCorp(fieldData, security);
                                    break;

                                case "Govt":
                                    if (isGetType)
                                        RequestGovt(security);
                                    else
                                        ParseGovt(fieldData, security);
                                    break;

                                case "Index":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                case "Curncy":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                case "Mmkt":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                case "Mtge":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                case "Muni":
                                    if (isGetType)
                                        RequestEquity(security);
                                    else
                                        ParseEquity(fieldData, security);
                                    break;

                                default:
                                    throw new FormatException("market sector invalid: " + marketSector);
                            }
                        }
                    }
                }
            }
        }

        #region Sector

        static private void RequestCorp(string title)
        {
            d_title[title] = new Tuple<int,int,string>(d_title[title].Item1,d_title[title].Item2, "Corp");
            request.Append("securities", "/isin/"+title);
            //request.Append("fields", "MARKET_SECTOR_DES");
            request.Append("fields", "PX_LAST");
            request.Append("fields", "DDIS_CURRENCY");
            request.Append("fields", "COUNTRY_ISO");
            request.Append("fields", "MATURITY");
            request.Append("fields", "ISSUE_DT");
            request.Append("fields", "NAME");
            request.Append("fields", "IS_COVERED");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO_NAME");
            request.Append("fields", "YAS_MOD_DUR");

            request = Rating.AddRating(request);

        }

        private static void ParseCorp(Element fieldData, string security)
        {
            Corp corp;
            try
            {
                string maturity = fieldData.GetElementAsString("MATURITY");
                string dateEmit = fieldData.GetElementAsString("ISSUE_DT");
                string name = fieldData.GetElementAsString("NAME");
                string country = fieldData.GetElementAsString("COUNTRY_ISO");
                double px_last = fieldData.GetElementAsFloat64("PX_LAST");
                string currency = fieldData.GetElementAsString("DDIS_CURRENCY");
                bool is_covered = fieldData.GetElementAsBool("IS_COVERED");
                int id_Mcorp = fieldData.GetElementAsInt32("ID_BB_ULTIMATE_PARENT_CO");
                string name_Mcorp = fieldData.GetElementAsString("ID_BB_ULTIMATE_PARENT_CO_NAME");
                double duration = fieldData.GetElementAsFloat64("YAS_MOD_DUR");

                // rating
                int rating = Rating.GetQuality(fieldData);

                corp = new Corp(security, d_title[security].Item1, d_title[security].Item2, country, currency, name, 115.5d, id_Mcorp, name_Mcorp, dateEmit, maturity, duration,rating, is_covered);
            }

            catch (NotFoundException e)
            {
                corp = new Corp(security, d_title[security].Item1, d_title[security].Item2,e.Description() );
            }

            //curve.GetValue(dateEmit);
            
            l_title.Add(corp);
        }

        static private void RequestEquity(string title)
        {
            d_title[title] = new Tuple<int, int, string>(d_title[title].Item1, 0, "Equity");
            request.Append("securities", "/isin/" + title);
            //request.Append("fields", "MARKET_SECTOR_DES");
            request.Append("fields", "PX_LAST");
            request.Append("fields", "DDIS_CURRENCY");
            request.Append("fields", "COUNTRY_ISO");
            request.Append("fields", "NAME");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO_NAME");

            request = Rating.AddRating(request);
        }


        private static void ParseEquity(Element fieldData, string security)
        {
            Equity equit;
            try
            {
                string country = fieldData.GetElementAsString("COUNTRY_ISO");
                double px_last = fieldData.GetElementAsFloat64("PX_LAST");
                string currency = fieldData.GetElementAsString("DDIS_CURRENCY");
                string name = fieldData.GetElementAsString("NAME");
                int id_Mcorp = fieldData.GetElementAsInt32("ID_BB_ULTIMATE_PARENT_CO");
                string name_Mcorp = fieldData.GetElementAsString("ID_BB_ULTIMATE_PARENT_CO_NAME");

                int rating = Rating.GetQuality(fieldData);

                equit = new Equity(security, d_title[security].Item1, country, currency, name, px_last, id_Mcorp, name_Mcorp,rating);
            }
            catch (NotFoundException e)
            {
                equit = new Equity(security, d_title[security].Item1, e.Description());
            }
            l_title.Add(equit);
        }

        static private void RequestGovt(string title)
        {
            d_title[title] = new Tuple<int, int, string>(d_title[title].Item1, 0, "Govt");
            request.Append("securities", "/isin/" + title);
            //request.Append("fields", "MARKET_SECTOR_DES");
            request.Append("fields", "PX_LAST");
            request.Append("fields", "MATURITY");
            request.Append("fields", "ISSUE_DT");
            request.Append("fields", "DDIS_CURRENCY");
            request.Append("fields", "COUNTRY_ISO");
            request.Append("fields", "NAME");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO");
            request.Append("fields", "ID_BB_ULTIMATE_PARENT_CO_NAME");
            request.Append("fields", "YAS_MOD_DUR");

            request = Rating.AddRating(request);
        }


        private static void ParseGovt(Element fieldData, string security)
        {
            Govt govt;
            try
            {
                string country = fieldData.GetElementAsString("COUNTRY_ISO");
                double px_last = fieldData.GetElementAsFloat64("PX_LAST");
                string currency = fieldData.GetElementAsString("DDIS_CURRENCY");
                string name = fieldData.GetElementAsString("NAME");
                int id_Mcorp = fieldData.GetElementAsInt32("ID_BB_ULTIMATE_PARENT_CO");
                string name_Mcorp = fieldData.GetElementAsString("ID_BB_ULTIMATE_PARENT_CO_NAME");
                double duration = fieldData.GetElementAsFloat64("YAS_MOD_DUR");
                string maturity = fieldData.GetElementAsString("MATURITY");
                string dateEmit = fieldData.GetElementAsString("ISSUE_DT");

                int rating = Rating.GetQuality(fieldData);

                govt = new Govt(security, d_title[security].Item1, d_title[security].Item2, country, currency, name, px_last, id_Mcorp, name_Mcorp, dateEmit, maturity, duration,rating);
            }
            catch (NotFoundException e)
            {
                govt = new Govt(security, d_title[security].Item1, d_title[security].Item2, e.Description());
            }
            l_title.Add(govt);
        }

        #endregion 


        #region CSV

        public void WriteCSV()
        {
            using (StreamWriter sw = new StreamWriter(@"CSV\Titres.csv"))
            {
                foreach (var title in l_title)
                {
                    sw.WriteLine(title.ToCSV());
                }
                sw.Close();
            }
        }

        static public void ReadCSV()
        {
            // Read and show each line from the file.
            string line = "";
            string[] values;
            Console.WriteLine("Lecture CSV, ancienne liste Title ecrasée");
            l_title.Clear();
            using (StreamReader sr = new StreamReader(@"CSV\Titres.csv"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    values = line.Split(';');
                    switch (values[0])
                    {
                        case "Equity":
                            if (values.Length == 9)
                                l_title.Add(new Equity(values[1], int.Parse(values[2]), values[3], values[4], values[5], Convert.ToDouble(values[6]), int.Parse(values[7]), values[8], int.Parse(values[9])));
                            else l_title.Add(new Equity(values[1], int.Parse(values[2]), values[3]));
                            break;
                        case "Corp":
                            if (values.Length == 14)
                                l_title.Add(new Corp(values[1], int.Parse(values[2]),int.Parse(values[3]), values[4], values[5],values[6],double.Parse(values[7]),int.Parse(values[8]),values[9], values[10], values[11], double.Parse(values[12]), int.Parse(values[13]),bool.Parse(values[14])));
                            else l_title.Add(new Corp(values[1], int.Parse(values[2]), int.Parse(values[3]),values[4]));
                            break;
                    }
                }
                sr.Close();
            }
        }

        #endregion

        


        private static void handleOtherEvent(Event eventObj)
        {
            //System.Console.WriteLine("EventType=" + eventObj.Type);
            //foreach (Message message in eventObj.GetMessages())
            //{
            //    System.Console.WriteLine("correlationID=" +
            //    message.CorrelationID);
            //    System.Console.WriteLine("messageType=" +
            //    message.MessageType);
            //    Console.WriteLine(message.ToString());
            //    if (Event.EventType.SESSION_STATUS == eventObj.Type
            //    && message.MessageType.Equals("SessionTerminated"))
            //    {
            //        System.Console.WriteLine("Terminating: " +
            //        message.MessageType);
            //        System.Environment.Exit(1);
            //    }
            //}
        }
    }
}
